#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;

namespace ILib.Debugs.Widgets
{
	public class DebugPageItem : ScMinimumWidget
	{
		ScText m_Label;
		public string Label
		{
			get => m_Label.Text;
			set => m_Label.Text = value;
		}
		DebugItemTapHandler m_TapHandler;
		DebugPageArrow m_Arrow;

		public System.Action OnTap { set => m_TapHandler.OnTap = value; }

		public bool Back { set => m_Arrow.Back = value; }

		public DebugPageItem()
		{
			m_TapHandler = new DebugItemTapHandler();
			m_Label = new ScText()
			{
				Layout = new StretchLayout { Margin = new RectOffset(16, 16 + 32, 0, 0) },
				TextAnchor = TextAnchor.MiddleCenter,
				TextClipping = TextClipping.Clip,
				FontStyle = FontStyle.BoldAndItalic,
			};
			m_Arrow = new DebugPageArrow
			{
				Layout = new StretchLayout { Margin = new RectOffset(6, 6, 6, 6) },
			};
			m_Children.Add(m_TapHandler);
			m_Children.Add(m_Label);
			m_Children.Add(m_Arrow);
		}
	}

	public class DebugPageArrow : ScMinimumWidget
	{
		public bool Back { get; set; }

		class Handler : SkinIMGUIDrawer.IMGUIHandler<DebugPageArrow>
		{
			protected override void Run(SkinIMGUIDrawer context, DebugPageArrow widget)
			{
				if (widget.IsOutsideClip()) return;
				if (context.Skin.Arrow)
				{
					var rect = widget.GetRect();
					if (rect.height < rect.width)
					{
						var size = rect.height;
						if (widget.Back)
						{
							rect.xMax = rect.xMin + size;
						}
						else
						{
							rect.xMin = rect.xMax - size;
						}
					}
					else
					{
						var size = rect.width;
						var offest = (rect.height - rect.width) / 2f;
						rect.yMin += offest;
						rect.yMax -= offest;
					}
					GUI.DrawTexture(rect, context.Skin.Arrow.texture, ScaleMode.ScaleAndCrop, true, 0, context.Skin.Color.Main, 0, 0);
				}
			}
		}
	}
}
#endif