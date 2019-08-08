#if DEBUG || ILIB_DEBUG_MENU
namespace ILib.Debugs
{
	using Widgets;
	public abstract class InputFieldContent<TContext> : Content<TContext, DebugInputFieldItem> where TContext : class
	{

		protected abstract string Label { get; }
		protected abstract string Value { get; set; }

		protected sealed override void Register(DebugInputFieldItem widget)
		{
			widget.Label = Label;
			widget.Input = Value;
			widget.OnChanged = OnChanged;
		}

		void OnChanged(string val)
		{
			Value = val;
		}
	}
}
#endif
