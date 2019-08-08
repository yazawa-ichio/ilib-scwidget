#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.Debugs.AutoRegisters
{
	internal class InputContent : AutoRegisterContent<DebugInputAttribute, DebugInputFieldItem>
	{
		public static IContent Create(AutoRegisterTargetAttribute target, DebugInputAttribute attr, System.Type type, System.Type contextType)
		{
			if (type == typeof(string))
			{
				var content = new InputContent();
				content.Setup(target, attr, contextType);
				return content;
			}
			if (type == typeof(int))
			{
				var content = new IntInputContent();
				content.Setup(target, attr, contextType);
				return content;
			}
			if (type == typeof(float))
			{
				var content = new FloatInputContent();
				content.Setup(target, attr, contextType);
				return content;
			}
			return null;
		}

		string GetValue()
		{
			if(Attr.Property != null)
			{
				return Attr.Property.GetMethod.Invoke(Context, null) as string;
			}
			if (Attr.Field != null)
			{
				return Attr.Field.GetValue(Context) as string;
			}
			return "";
		}

		void SetValue(string val)
		{
			if (Attr.Property != null)
			{
				Attr.Property.SetMethod.Invoke(Context, new object[] { val });
			}
			if (Attr.Field != null)
			{
				Attr.Field.SetValue(Context, val);
			}
		}

		protected override void Register(DebugInputFieldItem widget)
		{
			widget.Label = Label;
			widget.Input = GetValue();
			widget.OnChanged = OnChanged;
		}

		void OnChanged(string val)
		{
			SetValue(val);
		}
	}

	internal class IntInputContent : AutoRegisterContent<DebugInputAttribute, DebugIntFieldItem>
	{
		int GetValue()
		{
			if (Attr.Property != null)
			{
				return (int)Attr.Property.GetMethod.Invoke(Context, null);
			}
			if (Attr.Field != null)
			{
				return (int)Attr.Field.GetValue(Context);
			}
			return 0;
		}

		void SetValue(int val)
		{
			if (Attr.Property != null)
			{
				Attr.Property.SetMethod.Invoke(Context, new object[] { val });
			}
			if (Attr.Field != null)
			{
				Attr.Field.SetValue(Context, val);
			}
		}

		protected override void Register(DebugIntFieldItem widget)
		{
			widget.Label = Label;
			widget.Value = GetValue();
			widget.OnChanged = OnChanged;
		}

		void OnChanged(int val)
		{
			SetValue(val);
		}
	}

	internal class FloatInputContent : AutoRegisterContent<DebugInputAttribute, DebugFloatFieldItem>
	{
		float GetValue()
		{
			if (Attr.Property != null)
			{
				return (float)Attr.Property.GetMethod.Invoke(Context, null);
			}
			if (Attr.Field != null)
			{
				return (float)Attr.Field.GetValue(Context);
			}
			return 0;
		}

		void SetValue(float val)
		{
			if (Attr.Property != null)
			{
				Attr.Property.SetMethod.Invoke(Context, new object[] { val });
			}
			if (Attr.Field != null)
			{
				Attr.Field.SetValue(Context, val);
			}
		}

		protected override void Register(DebugFloatFieldItem widget)
		{
			widget.Label = Label;
			widget.Value = GetValue();
			widget.OnChanged = OnChanged;
		}

		void OnChanged(float val)
		{
			SetValue(val);
		}
	}
}
#endif
