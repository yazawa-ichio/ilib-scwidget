#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;
using System.Linq;

namespace ILib.Debugs
{
	using AutoRegisters;

	internal interface IContent
	{
		int Priority { get; }
		string Category { get; }
		int CategoryPriority { get; }
		string Path { get; }
		System.Type ContextType { get; }
		IScWidget Register(ScDebugMenu menu);
		void Update();
	}

	internal interface IInternalContent { }

	public abstract class Content<TContext, UWidget> : IContent where TContext : class where UWidget : IScWidget, new()
	{
		public ScDebugMenu Menu { get; private set; }
		public TContext Context => Menu.Contexts.Get<TContext>();
		public virtual int Priority => 0;
		protected virtual string Path => typeof(TContext).Name;
		protected virtual string Category { get => ""; }
		protected virtual int CategoryPriority { get => 0; }

		string IContent.Path => Path;
		string IContent.Category => Category;
		int IContent.CategoryPriority => CategoryPriority;
		System.Type IContent.ContextType => typeof(TContext);

		protected UWidget Widget { get; private set; }

		IScWidget IContent.Register(ScDebugMenu menu)
		{
			Menu = menu;
			Widget = new UWidget();
			Register(Widget);
			return Widget;
		}

		protected abstract void Register(UWidget widget);
		public virtual void Update() { }
	}

	public static class Content
	{
		static IContent[] s_Contents;

		static Dictionary<string, List<string>> s_PageList = new Dictionary<string, List<string>>();

		static Content()
		{
			s_Contents = GetContents().ToArray();

			foreach (var content in s_Contents)
			{
				List<string> ret;
				if (!s_PageList.TryGetValue(content.Category, out ret))
				{
					s_PageList[content.Category] = ret = new List<string>();
				}
				if (!ret.Contains(content.Path))
				{
					ret.Add(content.Path);
				}
			}
		}

		static IEnumerable<IContent> GetContents()
		{
			var contentType = typeof(IContent);
			var internalContent = typeof(IInternalContent);
			var autoRegister = typeof(AutoRegisterTargetAttribute);
			foreach (var assemblie in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assemblie.GetTypes())
				{
					if (contentType.IsAssignableFrom(type) && !type.IsAbstract && !internalContent.IsAssignableFrom(type))
					{
						yield return System.Activator.CreateInstance(type) as IContent;
					}
					if (System.Attribute.IsDefined(type, autoRegister))
					{
						var attr = (AutoRegisterTargetAttribute)type.GetCustomAttributes(autoRegister, true)[0];
						foreach (var content in GetAutoContents(attr, type))
						{
							yield return content;
						}
					}
				}
			}
		}

		static IEnumerable<IContent> GetAutoContents(AutoRegisterTargetAttribute attr, System.Type type)
		{
			var flag = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.DeclaredOnly;
			foreach (var method in type.GetMethods(flag))
			{
				if (method.TryGetAttribute(out DebugButtonAttribute button))
				{
					button.Method = method;
					var content = new ButtonContent();
					content.Setup(attr, button, type);
					yield return content;
				}
				if (method.TryGetAttribute(out DebugLabelAttribute label))
				{
					label.Method = method;
					var content = new LabelContent();
					content.Setup(attr, label, type);
					yield return content;
				}
			}
			foreach (var prop in type.GetProperties(flag))
			{
				if (prop.TryGetAttribute(out DebugInputAttribute input))
				{
					input.Property = prop;
					yield return InputContent.Create(attr, input, prop.PropertyType, type);
				}
				if (prop.TryGetAttribute(out DebugSliderAttribute slider))
				{
					slider.Property = prop;
					yield return SliderContent.Create(attr, slider, prop.PropertyType, type);
				}
				if (prop.TryGetAttribute(out DebugToggleAttribute toggle) && prop.PropertyType == typeof(bool))
				{
					toggle.Property = prop;
					var content = new ToggleContent();
					content.Setup(attr, toggle, type);
					yield return content;
				}
			}
			foreach (var field in type.GetFields(flag))
			{
				if (field.TryGetAttribute(out DebugInputAttribute input))
				{
					input.Field = field;
					yield return InputContent.Create(attr, input, field.FieldType, type);
				}
				if (field.TryGetAttribute(out DebugSliderAttribute slider))
				{
					slider.Field = field;
					yield return SliderContent.Create(attr, slider, field.FieldType, type);
				}
				if (field.TryGetAttribute(out DebugToggleAttribute toggle) && field.FieldType == typeof(bool))
				{
					toggle.Field = field;
					var content = new ToggleContent();
					content.Setup(attr, toggle, type);
					yield return content;
				}
			}
		}


		static bool TryGetAttribute<T>(this System.Reflection.MemberInfo info, out T ret)
		{
			foreach (var attr in info.GetCustomAttributes(typeof(T), false))
			{
				if (attr is T)
				{
					ret = (T)attr;
					return true;
				}
			}
			ret = default(T);
			return false;
		}

		internal static IEnumerable<string> GetCategory(this ScDebugMenu self)
		{
			return GetCategoryImpl(self).OrderBy(x => x.Item2).Select(x => x.Item1).Distinct();
		}

		static IEnumerable<(string,int)> GetCategoryImpl(ScDebugMenu self)
		{
			HashSet<System.Type> typeSet = new HashSet<System.Type>(self.Contexts.GetTypes());
			foreach (var content in s_Contents)
			{
				if (typeSet.Contains(content.ContextType))
				{
					if (!string.IsNullOrEmpty(content.Category))
					{
						yield return (content.Category, content.CategoryPriority);
					}
					else
					{
						yield return (content.Category, -1);
					}
				}
			}
		}

		internal static IEnumerable<IContent> GetContent(this ScDebugMenu self, string page)
		{
			string category = self.Category;
			HashSet<System.Type> typeSet = new HashSet<System.Type>(self.Contexts.GetTypes());
			foreach (var content in s_Contents)
			{
				if (content.Category == category && content.Path == page && typeSet.Contains(content.ContextType))
				{
					yield return content;
				}
			}
		}

		internal static (IContent[], IScWidget[]) CreateWidget(this ScDebugMenu self, string page)
		{
			List<IScWidget> widgets = new List<IScWidget>();
			List<IContent> contents = new List<IContent>();
			foreach (var nextPage in GetNextPage(self.Category, page).Distinct())
			{
				widgets.Add(new Widgets.DebugPageItem()
				{
					Label = System.IO.Path.GetFileNameWithoutExtension(nextPage),
					OnTap = () => self.SetPage(nextPage),
				});
			}
			foreach (var c in self.GetContent(page).OrderBy(x => x.Priority))
			{
				contents.Add(c);
				widgets.Add(c.Register(self));
			}
			return (contents.ToArray(), widgets.ToArray());
		}

		static IEnumerable<string> GetNextPage(string category, string page)
		{
			List<string> ret;
			if (category == null || !s_PageList.TryGetValue(category, out ret))
			{
				yield break;
			}
			foreach (var pageName in ret)
			{
				if (string.IsNullOrEmpty(pageName) || pageName == page || pageName[0] == '@')
				{
					continue;
				}
				if (!string.IsNullOrEmpty(page) && !pageName.StartsWith(page))
				{
					continue;
				}
				var nextName = !string.IsNullOrEmpty(page) ? pageName.Replace(page + "/", "") : pageName;
				var index = nextName.IndexOf('/');
				if (index >= 0)
				{
					yield return nextName.Substring(0, index);
				}
				else
				{
					yield return pageName;
				}
			}
		}


	}

}
#endif
