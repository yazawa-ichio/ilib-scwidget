#if DEBUG || ILIB_DEBUG_MENU
namespace ILib.Debugs
{
	using Widgets;
	public abstract class ButtonContent<TContext> : Content<TContext, DebugButtonItem> where TContext : class
	{

		protected abstract string Label { get; }
		protected virtual string ButtonText { get => "実行"; }

		protected sealed override void Register(DebugButtonItem widget)
		{
			widget.Label = Label;
			widget.ButtonText = ButtonText;
			widget.OnClick = OnClick;
		}

		protected abstract void OnClick();

	}


}
#endif