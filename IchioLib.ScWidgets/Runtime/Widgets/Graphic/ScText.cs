using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{
	public class ScText : ScGraphic
	{
		Font m_Font;
		public Font Font
		{
			get => m_Font;
			set
			{
				m_Font = value;
				SetDitry();
			}
		}

		public string Text { get; set; }

		public int FontSize { get; set; }

		public TextAnchor TextAnchor { get; set; } = TextAnchor.MiddleCenter;

		public TextClipping TextClipping { get; set; } = TextClipping.Overflow;

		public FontStyle FontStyle { get; set; } = FontStyle.Normal;

	}
}
