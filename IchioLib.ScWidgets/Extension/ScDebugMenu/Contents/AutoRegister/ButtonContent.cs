#if DEBUG || ILIB_DEBUG_MENU
namespace ILib.Debugs.AutoRegisters
{
	internal class ButtonContent : AutoRegisterContent<DebugButtonAttribute, DebugButtonItem>
	{
		protected virtual string ButtonText { get => "実行"; }

		protected sealed override void Register(DebugButtonItem widget)
		{
			widget.Label = Label;
			widget.ButtonText = ButtonText;
			widget.OnClick = OnClick;
		}

		protected void OnClick()
		{
			Attr.Method.Invoke(Context, null);
		}

	}


}
#endif
