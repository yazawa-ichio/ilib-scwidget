#if DEBUG || ILIB_DEBUG_MENU
namespace ILib.Debugs
{
	public abstract class FloatFieldContent<TContext> : Content<TContext, DebugFloatFieldItem> where TContext : class
	{

		protected abstract string Label { get; }
		protected abstract float Value { get; set; }

		protected sealed override void Register(DebugFloatFieldItem widget)
		{
			widget.Label = Label;
			widget.Value = Value;
			widget.OnChanged = OnChanged;
		}

		void OnChanged(float val)
		{
			Value = val;
		}
	}
}
#endif
