#if DEBUG || ILIB_DEBUG_MENU
using ILib.ScWidgets;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.Debugs.Widgets
{
	public class DebugToggleItem : ScMinimumWidget
	{

		DebugItemTapHandler m_TapHandler;
		ScToggle m_Toggle;
		ScHorizontalRatioSplitter m_Splitter;


		public bool Value
		{
			get => m_Toggle.Value;
			set => m_Toggle.Value = value;
		}

		ScText m_Label;
		public string Label
		{
			get => m_Label.Text;
			set => m_Label.Text = value;
		}

		public System.Action<bool> OnChange { set => m_Toggle.OnChange = value; }

		public DebugToggleItem()
		{
			m_Splitter = new ScHorizontalRatioSplitter();
			m_Label = new ScText()
			{
				Layout = new StretchLayout { Margin = new RectOffset(16, 16, 0, 0) },
				TextAnchor = TextAnchor.MiddleLeft,
				TextClipping = TextClipping.Clip,
			};
			m_Toggle = new ScToggle()
			{
				Layout = new StretchLayout { Margin = new RectOffset(8, 8, 0, 0) },
			};
			m_Splitter.Set(m_Label, 5);
			m_Splitter.Set(m_Toggle, 2);
			m_TapHandler = new DebugItemTapHandler();
			m_TapHandler.OnTap = m_Toggle.Invoke;
			m_Children.Add(m_TapHandler);
			m_Children.Add(m_Splitter);
		}

	}
}
#endif
