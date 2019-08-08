#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;

namespace ILib.Debugs
{
	public class HeaderButton : ScText, IFunction
	{
		bool m_Interactable = true;
		public bool Interactable
		{
			get => m_Interactable;
			set
			{
				SetDitry();
				m_Interactable = value;
			}
		}
		public System.Action OnClick { set; private get; }

		public void Invoke()
		{
			if (Interactable)
			{
				OnClick?.Invoke();
			}
		}

		class Handler : SkinIMGUIDrawer.IMGUIHandler<HeaderButton>
		{
			protected override GUIStyle CreateStyle(SkinIMGUIDrawer context, IScWidget widget)
			{
				var style = new GUIStyle("Label");
				style.normal.textColor = new Color(1f, 1f, 1f, 1f);
				style.active.textColor = new Color(1f, 1f, 1f, 1f);
				style.hover.textColor = new Color(1f, 0f, 0f, 1f);
				style.onNormal.textColor = new Color(1f, 1f, 1f, 1f);
				style.onActive.textColor = new Color(1f, 1f, 1f, 1f);
				style.onHover.textColor = new Color(1f, 0f, 0f, 1f);
				return style;
			}
			protected override void Run(SkinIMGUIDrawer context, HeaderButton widget)
			{
				if (widget.IsOutsideClip()) return;
				var style = GetStyle(context, widget);
				style.alignment = widget.TextAnchor;
				if (style.font != widget.Font)
				{
					style.font = widget.Font;
				}
				style.fontSize = widget.FontSize;
				var color = GUI.contentColor;
				try
				{
					GUI.contentColor = new Color(1f, 1f, 1f, 1f);
					if (GUI.Button(widget.GetRect(), widget.Text, style))
					{
						widget.Invoke();
					}
				}
				finally
				{
					GUI.contentColor = color;
				}
			}
		}

	}

}
#endif
