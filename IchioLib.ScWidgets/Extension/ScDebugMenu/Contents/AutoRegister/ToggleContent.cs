#if DEBUG || ILIB_DEBUG_MENU
namespace ILib.Debugs.AutoRegisters
{

	internal class ToggleContent : AutoRegisterContent<DebugToggleAttribute, DebugToggleItem>
	{
		bool GetValue()
		{
			if (Attr.Property != null)
			{
				return (bool)Attr.Property.GetMethod.Invoke(Context, null) ;
			}
			if (Attr.Field != null)
			{
				return (bool)Attr.Field.GetValue(Context);
			}
			return false;
		}

		void SetValue(bool val)
		{
			if (Attr.Property != null)
			{
				Attr.Property.SetMethod.Invoke(Context, new object[] { val });
			}
			if (Attr.Field != null)
			{
				Attr.Field.SetValue(Context, val);
			}
		}

		protected sealed override void Register(DebugToggleItem widget)
		{
			widget.Label = Label;
			widget.Value = GetValue();
			widget.OnChange = OnChanged;
		}

		void OnChanged(bool val)
		{
			SetValue(val);
		}

	}
}
#endif
