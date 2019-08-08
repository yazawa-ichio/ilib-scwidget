#if DEBUG || ILIB_DEBUG_MENU
namespace ILib.Debugs
{
	using Widgets;
	public abstract class IntFieldContent<TContext> : Content<TContext, DebugIntFieldItem> where TContext : class
	{

		protected abstract string Label { get; }
		protected abstract int Value { get; set; }

		protected sealed override void Register(DebugIntFieldItem widget)
		{
			widget.Label = Label;
			widget.Value = Value;
			widget.OnChanged = OnChanged;
		}

		void OnChanged(int val)
		{
			Value = val;
		}
	}
}
#endif
