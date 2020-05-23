using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{

	public class ScCheckBox : ScFunction
	{
		bool m_Value;
		public bool Value
		{
			get => m_Value;
			set
			{
				Set(value);
			}
		}

		public System.Action<bool> OnChange { set; private get; }

		public void Invoke()
		{
			if (Interactable)
			{
				Set(!m_Value);
			}
		}

		void Set(bool val)
		{
			SetDitry();
			m_Value = val;
			OnChange?.Invoke(val);
		}
	}

}