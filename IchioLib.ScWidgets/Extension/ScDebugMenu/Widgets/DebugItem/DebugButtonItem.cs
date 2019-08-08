#if DEBUG || ILIB_DEBUG_MENU
using UnityEngine;
using ILib.ScWidgets;

namespace ILib.Debugs
{
	public class DebugButtonItem : DebugItemBase
	{
		ScButton m_Button;
		ScText m_ButtonText;

		public string ButtonText
		{
			get => m_ButtonText.Text;
			set => m_ButtonText.Text = value;
		}

		public System.Action OnClick { set => m_Button.OnClick = value; }

		protected override IScWidget Create()
		{
			return m_Button = new ScButton
			{
				Child = m_ButtonText = new ScText
				{
					Text = "実行",
					TextAnchor = TextAnchor.MiddleCenter,
					TextClipping = TextClipping.Clip,
				}
			};
		}
	}

	public class DebugMultiButtonItem : DebugItemBase
	{
		ScButton[] m_Button;
		ScText m_ButtonText;
		ScHorizontalRatioSplitter m_ButtonSpritter;

		public System.Action<int> OnClick { set; private get; }

		public string[] ButtonText
		{
			set => Set(value);
		}

		public void Set(string[] buttonTexts)
		{
			if (m_Button != null)
			{
				for (int i = 0; i < m_Button.Length; i++)
				{
					m_ButtonSpritter.Remove(m_Button[i]);
				}
			}
			m_Button = new ScButton[buttonTexts.Length];
			for (int i = 0; i < buttonTexts.Length; i++)
			{
				var index = i;
				m_ButtonSpritter.Set(new ScButton
				{
					Child = m_ButtonText = new ScText
					{
						Text = buttonTexts[index],
						TextAnchor = TextAnchor.MiddleCenter,
						TextClipping = TextClipping.Clip,
					},
					OnClick = () => OnClick?.Invoke(index)
				}, 1);
			}
		}

		protected override IScWidget Create()
		{
			return m_ButtonSpritter = new ScHorizontalRatioSplitter();
		}

	}
}
#endif
