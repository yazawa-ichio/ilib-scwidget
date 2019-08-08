#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.Debugs.AutoRegisters
{
	
	internal class SliderContent : AutoRegisterContent<DebugSliderAttribute, DebugSliderItem>
	{
		public static IContent Create(AutoRegisterTargetAttribute target, DebugSliderAttribute attr, System.Type type, System.Type contextType)
		{
			if (type == typeof(int))
			{
				var content = new IntSliderContent();
				content.Setup(target, attr, contextType);
				return content;
			}
			if (type == typeof(float))
			{
				var content = new SliderContent();
				content.Setup(target, attr, contextType);
				return content;
			}
			return null;
		}

		float GetValue()
		{
			if (Attr.Property != null)
			{
				return (float)Attr.Property.GetMethod.Invoke(Context, null);
			}
			if (Attr.Field != null)
			{
				return (float)Attr.Field.GetValue(Context);
			}
			return 0;
		}

		void SetValue(float val)
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

		protected override void Register(DebugSliderItem widget)
		{
			widget.Label = Label;
			widget.Value = GetValue();
			widget.MaxValue = Attr.MaxValue;
			widget.MinValue = Attr.MinValue;
			widget.OnChanged = OnChanged;
		}

		void OnChanged(float val)
		{
			SetValue(val);
		}
	}

	internal class IntSliderContent : AutoRegisterContent<DebugSliderAttribute, DebugIntSliderItem>
	{
		int GetValue()
		{
			if (Attr.Property != null)
			{
				return (int)Attr.Property.GetMethod.Invoke(Context, null);
			}
			if (Attr.Field != null)
			{
				return (int)Attr.Field.GetValue(Context);
			}
			return 0;
		}

		void SetValue(int val)
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

		protected override void Register(DebugIntSliderItem widget)
		{
			widget.Label = Label;
			widget.Value = GetValue();
			widget.MaxValue = (int)Attr.MaxValue;
			widget.MinValue = (int)Attr.MinValue;
			widget.OnChanged = OnChanged;
		}

		void OnChanged(int val)
		{
			SetValue(val);
		}
	}
}
#endif
