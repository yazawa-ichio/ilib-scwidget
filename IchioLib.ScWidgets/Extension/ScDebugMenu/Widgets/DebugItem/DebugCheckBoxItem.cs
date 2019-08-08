#if DEBUG || ILIB_DEBUG_MENU
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;

namespace ILib.Debugs
{
	public class DebugCheckBoxItem : ScMinimumWidget
	{

		DebugItemTapHandler m_TapHandler;

		ScCheckBox m_CheckBox;

		public bool Value
		{
			get => m_CheckBox.Value;
			set => m_CheckBox.Value = value;
		}

		ScText m_Label;
		public string Label
		{
			get => m_Label.Text;
			set => m_Label.Text = value;
		}

		public System.Action<bool> OnChange { set => m_CheckBox.OnChange = value; }

		public DebugCheckBoxItem()
		{
			m_CheckBox = new ScCheckBox()
			{
				Layout = new Layout { Size = new Vector2(36, 36), Anchor = LayoutAnchor.Left, Pos = new Vector2(8, 0) },
			};
			m_Label = new ScText()
			{
				Layout = new StretchLayout { Margin = new RectOffset(48, 16, 0, 0) },
				TextAnchor = TextAnchor.MiddleLeft,
				TextClipping = TextClipping.Clip,
			};
			m_TapHandler = new DebugItemTapHandler();
			m_TapHandler.OnTap = m_CheckBox.Invoke;
			m_Children.Add(m_TapHandler);
			m_Children.Add(m_CheckBox);
			m_Children.Add(m_Label);
		}

	}
}
#endif
