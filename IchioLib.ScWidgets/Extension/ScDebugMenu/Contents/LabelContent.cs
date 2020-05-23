#if DEBUG || ILIB_DEBUG_MENU
namespace ILib.Debugs
{
	using Widgets;
	public abstract class LabelContent<TContext> : Content<TContext, DebugLabelItem> where TContext : class
	{
		protected abstract string Label { get; }

		protected sealed override void Register(DebugLabelItem widget)
		{
			widget.Label = Label;
		}

		public override void Update()
		{
			Widget.Label = Label;
		}
	}

}
#endif