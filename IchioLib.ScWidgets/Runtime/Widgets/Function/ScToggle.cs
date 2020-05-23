using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{
	public class ScToggle : ScFunction
	{
		bool m_Value;
		public bool Value
		{
			get => m_Value;
			set
			{
				SetImpl(value);
			}
		}
		public System.Action<bool> OnChange { set; private get; }

		public void Invoke()
		{
			if (Interactable)
			{
				SetImpl(!m_Value);
			}
		}

		public void Set(bool val)
		{
			if (Interactable)
			{
				SetImpl(val);
			}
		}

		void SetImpl(bool val)
		{
			SetDitry();
			m_Value = val;
			OnChange?.Invoke(val);
		}

	}
}