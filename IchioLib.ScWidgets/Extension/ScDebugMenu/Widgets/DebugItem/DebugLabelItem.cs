#if DEBUG || ILIB_DEBUG_MENU
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;

namespace ILib.Debugs.Widgets
{
	public class DebugLabelItem : ScMinimumWidget
	{
		ScText m_Label;
		public string Label
		{
			get => m_Label.Text;
			set => m_Label.Text = value;
		}

		public DebugLabelItem()
		{
			m_Label = new ScText()
			{
				Layout = new StretchLayout { Margin = new RectOffset(16, 16, 0, 0) },
				TextAnchor = TextAnchor.MiddleLeft,
				TextClipping = TextClipping.Clip,
			};
			m_Children.Add(m_Label);
		}

	}
}
#endif
