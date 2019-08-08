using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{
	public class ScInputField : ScFunction
	{
		public System.Action<string> OnChanged { set; private get; }
		public string Input { get; set; }

		public int FontSize { get; set; }

		public TextAnchor TextAnchor { get; set; } = TextAnchor.MiddleLeft;


		public void Set(string input)
		{
			if (Interactable)
			{
				Input = input;
				OnChanged?.Invoke(Input);
			}
		}
	}
}
