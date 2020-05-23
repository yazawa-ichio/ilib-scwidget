using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{
	public class ScButton : ScFunction
	{
		public System.Action OnClick { set; private get; }

		public void Invoke()
		{
			if (Interactable)
			{
				OnClick?.Invoke();
			}
		}
	}
}