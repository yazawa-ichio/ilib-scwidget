using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{
	public class ScSlider : ScFunction
	{

		float m_Value;
		public float Value
		{
			get => m_Value;
			set
			{
				SetImpl(value);
			}
		}

		public float MinValue { get; set; } = 0;
		public float MaxValue { get; set; } = 1f;

		public System.Action<float> OnChange { set; private get; }

		public void Set(float val)
		{
			if (Interactable)
			{
				SetImpl(val);
			}
		}

		void SetImpl(float val)
		{
			SetDitry();
			m_Value = val;
			OnChange?.Invoke(val);
		}
	}
}