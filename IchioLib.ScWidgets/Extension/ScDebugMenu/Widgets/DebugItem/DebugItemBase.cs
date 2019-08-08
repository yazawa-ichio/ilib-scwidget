#if DEBUG || ILIB_DEBUG_MENU
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;

namespace ILib.Debugs.Widgets
{
	public abstract class DebugItemBase : ScMinimumWidget
	{
		ScHorizontalRatioSplitter m_Splitter;
		ScText m_Label;
		public string Label
		{
			get => m_Label.Text;
			set => m_Label.Text = value;
		}

		protected virtual int LabelWeight { get => 5; }
		protected virtual int DefContentsWeight { get => 2; }

		IScWidget m_Contents;
		public int ContentsWeight { set => m_Splitter.Set(m_Contents, value); }

		public DebugItemBase()
		{
			m_Splitter = new ScHorizontalRatioSplitter();
			m_Label = new ScText()
			{
				Layout = new StretchLayout { Margin = new RectOffset(16, 16, 0, 0) },
				TextAnchor = TextAnchor.MiddleLeft,
				TextClipping = TextClipping.Clip,
			};
			m_Splitter.Set(m_Label, LabelWeight);
			var contents = new ScWidget
			{
				Layout = new StretchLayout { Margin = new RectOffset(8, 8, 0, 0) },
				Child = Create()
			};
			m_Contents = contents;
			m_Splitter.Set(contents, DefContentsWeight);
			m_Children.Add(m_Splitter);
		}

		protected abstract IScWidget Create();

	}
}
#endif
