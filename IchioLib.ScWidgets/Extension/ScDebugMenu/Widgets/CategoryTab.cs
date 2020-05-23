#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;

namespace ILib.Debugs.Widgets
{
	public class CategoryTab : ScWidget
	{
		ScDebugMenu m_Menu;
		ScText m_Text;
		ScTexture m_Texture;

		public System.Action<string> OnSelect { private get; set; }

		public CategoryTab(string name, ScDebugMenu menu, float height)
		{
			Name = name;
			m_Texture = new ScTexture
			{
				Texture = Texture2D.whiteTexture,
				Color = menu.Skin.Color.Main,
				Layout = new HorizontalStretchLayout()
				{
					Height = 2,
					Anchor = LayoutAnchor.Top,
					PosY = height - 8,
					Margin = new Vector2(4, 4)
				}
			};
			m_Text = new ScText
			{
				Text = string.IsNullOrEmpty(name) ? "共通" : name,
				TextAnchor = TextAnchor.UpperCenter,
				FontSize = 16,
				Layout = new StretchLayout { Margin = new RectOffset(0, 0, 4, 0) }
			};
			Add(m_Text);
			m_Menu = menu;
		}

		public void SetSelect(bool select)
		{
			Remove(m_Texture);
			if (select)
			{
				Add(m_Texture);
				m_Text.MaskColor = m_Menu.Skin.Color.TextOnActive;
			}
			else
			{
				m_Text.MaskColor = m_Menu.Skin.Color.TextOnDisable;
			}
		}

		void Select()
		{
			OnSelect?.Invoke(Name);
		}

		class Hander : SkinIMGUIDrawer.IMGUIHandler<CategoryTab>
		{
			protected override void Run(SkinIMGUIDrawer context, CategoryTab widget)
			{
				var ret = GUI.Button(widget.GetRect(), "", GetStyle(context, widget, "Label"));
				if (ret)
				{
					widget.Select();
				}
			}
		}


	}

}
#endif