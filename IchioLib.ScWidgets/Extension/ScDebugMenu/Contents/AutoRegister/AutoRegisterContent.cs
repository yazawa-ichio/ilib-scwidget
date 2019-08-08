#if DEBUG || ILIB_DEBUG_MENU
using ILib.ScWidgets;

namespace ILib.Debugs.AutoRegisters
{
	internal abstract class AutoRegisterContent<TAttribute, UWidget> : IContent, IInternalContent where TAttribute : AutoRegisterAttribute where UWidget : IScWidget, new()
	{
		AutoRegisterTargetAttribute m_TragetAttr;

		protected TAttribute Attr { get; private set; }

		protected ScDebugMenu Menu { get; private set; }
		
		protected virtual string Label => Attr.Name;

		protected object Context => Menu.Contexts.Get(ContextType);

		public System.Type ContextType { get; private set; }

		int IContent.Priority => Attr.Priority;

		string IContent.Path => Attr.Path ?? m_TragetAttr.Path ?? "";

		string IContent.Category => Attr.Category ?? m_TragetAttr.Category ?? "";

		int IContent.CategoryPriority => UnityEngine.Mathf.Max(Attr.CategoryPriority, m_TragetAttr.CategoryPriority);

		public void Setup(AutoRegisterTargetAttribute target, TAttribute attr, System.Type contextType)
		{
			m_TragetAttr = target;
			Attr = attr;
			ContextType = contextType;
		}

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

}
#endif
