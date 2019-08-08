#if DEBUG || ILIB_DEBUG_MENU
namespace ILib.Debugs
{
	using Widgets;
	public abstract class PageJumpContent<TContext> : Content<TContext, DebugPageItem> where TContext : class
	{

		protected abstract string Label { get; }
		protected virtual string JumpPage { get => "@" + Label; }

		protected sealed override void Register(DebugPageItem widget)
		{
			widget.Label = Label;
			widget.OnTap = () => Menu.SetPage(JumpPage);
		}

	}

}
#endif
