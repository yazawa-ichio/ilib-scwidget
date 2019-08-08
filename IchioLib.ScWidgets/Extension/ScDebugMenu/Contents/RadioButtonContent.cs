#if DEBUG || ILIB_DEBUG_MENU
namespace ILib.Debugs
{
	using Widgets;
	public abstract class RadioButtonContent<TContext> : Content<TContext, DebugRadioButtonItem> where TContext : class
	{
		protected abstract string[] Items { get; }
		protected virtual int Select => -1;

		protected sealed override void Register(DebugRadioButtonItem widget)
		{
			widget.OnChanged = OnChanged;
			widget.SetItems(Items, Select);
		}

		protected abstract void OnChanged(int index, string name);

	}
}
#endif
