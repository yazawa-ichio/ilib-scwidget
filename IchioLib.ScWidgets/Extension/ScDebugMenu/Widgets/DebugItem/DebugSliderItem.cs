#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;

namespace ILib.Debugs
{
	public class DebugSliderItemBase : DebugItemBase
	{
		protected ScHorizontalRatioSplitter m_ContentsSplitter;

		protected ScSlider m_Slider;

		protected ScInputField m_InputField;

		protected override int DefContentsWeight { get => 4; }

		int m_InputWeight = 2;
		public int InputWeight
		{
			get => m_InputWeight;
			set
			{
				m_InputWeight = value;
				m_ContentsSplitter.Set(m_InputField, value);
			}
		}

		int m_SliderWeight = 3;
		public int SliderWeight
		{
			get => m_SliderWeight;
			set
			{
				m_SliderWeight = value;
				m_ContentsSplitter.Set(m_Slider, value);
			}
		}

		protected override IScWidget Create()
		{
			m_ContentsSplitter = new ScHorizontalRatioSplitter()
			{
				Layout = new StretchLayout { Margin = new RectOffset(0, 0, 8, 8) },
			};
			m_ContentsSplitter.Set(m_InputField = new ScInputField()
			{
				OnChanged = OnChange
			}, InputWeight);
			m_ContentsSplitter.Set(m_Slider = new ScSlider()
			{
				OnChange = OnChange,
			}, SliderWeight);
			return m_ContentsSplitter;
		}

		protected virtual void OnChange(float val) { }
		protected virtual void OnChange(string val) { }

	}

	public class DebugIntSliderItem : DebugSliderItemBase
	{

		public int MaxValue
		{
			get => (int)m_Slider.MaxValue;
			set => m_Slider.MaxValue = value;
		}

		public int MinValue
		{
			get => (int)m_Slider.MinValue;
			set => m_Slider.MinValue = value;
		}

		public int Value
		{
			get => (int)m_Slider.Value;
			set
			{
				m_Slider.Value = value;
				if (m_InputField != null) m_InputField.Input = value.ToString();
			}
		}

		public System.Action<int> OnChanged { set; private get; }

		protected sealed override void OnChange(float val)
		{
			OnChanged?.Invoke((int)val);
			if (m_InputField != null) m_InputField.Input = ((int)val).ToString();
		}

		protected override void OnChange(string val)
		{
			int input = 0;
			if (!int.TryParse(val, out input))
			{
				m_InputField.Input = m_Slider.Value.ToString();
			}
			else
			{
				Value = input;
			}
		}
	}

	public class DebugSliderItem : DebugSliderItemBase
	{

		public float MaxValue
		{
			get => m_Slider.MaxValue;
			set => m_Slider.MaxValue = value;
		}

		public float MinValue
		{
			get => m_Slider.MinValue;
			set => m_Slider.MinValue = value;
		}

		public float Value
		{
			get => m_Slider.Value;
			set
			{
				m_Slider.Value = value;
				if (m_InputField != null) m_InputField.Input = value.ToString();
			}
		}

		public System.Action<float> OnChanged { set; private get; }

		protected sealed override void OnChange(float val)
		{
			OnChanged?.Invoke(val);
			if (m_InputField != null) m_InputField.Input = val.ToString();
		}

		protected override void OnChange(string val)
		{
			float input = 0;
			if (!float.TryParse(val, out input)) 
			{
				m_InputField.Input = m_Slider.Value.ToString();
			}
			else
			{
				Value = input;
			}
		}

	}

}
#endif
