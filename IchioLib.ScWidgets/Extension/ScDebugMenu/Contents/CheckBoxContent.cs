#if DEBUG || ILIB_DEBUG_MENU

namespace ILib.Debugs
{
	public abstract class CheckBoxContent<TContext> : Content<TContext, DebugCheckBoxItem> where TContext : class
	{

		protected abstract string Label { get; }
		protected abstract bool Value { get; set; }

		protected sealed override void Register(DebugCheckBoxItem widget)
		{
			widget.Label = Label;
			widget.Value = Value;
			widget.OnChange = SetValue;
		}

		public sealed override void Update()
		{
			Widget.Value = Value;
			OnUptete();
		}

		protected virtual void OnUptete() { }

		void SetValue(bool val)
		{
			Value = val;
		}

	}

}

#endif
