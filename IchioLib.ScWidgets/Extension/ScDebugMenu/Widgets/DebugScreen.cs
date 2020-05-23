#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;
using System.Linq;
using System;

namespace ILib.Debugs.Widgets
{
	internal class DebugScreen : ScWidget
	{
		ScDebugMenu m_Menu;
		ScScrollView m_CategoryRoot;
		PageStackScroll m_MainScroll;
		CategoryTab[] m_CategoryTab;
		float m_HeaderFooterHeight = 80f;
		ScText m_HeaderText;
		Vector2 m_ScreenSize;
		string m_Page;


		public float ContentsHeight
		{
			get => m_MainScroll.Height;
			set => m_MainScroll.Height = value;
		}

		public DebugScreen(ScDebugMenu menu, float contentsHeight = 40, float headerFooterHeight = 80)
		{
			m_Menu = menu;
			m_HeaderFooterHeight = headerFooterHeight;
			Add(new ScTexture
			{
				//TODO:全面を覆うレイアウトを作る
				Layout = new StretchLayout { Margin = new RectOffset(-(int)1280, -(int)1280, -(int)1280, -(int)1280) },
				Texture = Texture2D.whiteTexture,
				Color = new Color(1, 1, 1, 0.85f)
			});
			Add(m_MainScroll = new PageStackScroll(contentsHeight)
			{
				Layout = new StretchLayout { Margin = new RectOffset(0, 0, (int)(m_HeaderFooterHeight / 2f), (int)(m_HeaderFooterHeight / 2f)) },
			});
			Add(CreateHeader());
			Add(CreateFooter());
		}

		IScWidget CreateHeader()
		{
			var skin = m_Menu.Skin;
			var height = m_HeaderFooterHeight / 2f;
			return new ScWidget
			{
				Layout = new HorizontalStretchLayout
				{
					Anchor = LayoutAnchor.Top,
					Height = height,
				},
				Children = new IScWidget[]{
					new ScTexture
					{
						Layout = new StretchLayout
						{
							Margin = new RectOffset(-(int)m_HeaderFooterHeight*2, -(int)m_HeaderFooterHeight*2, -(int)m_HeaderFooterHeight*2, 0)
						},
						Texture = Texture2D.whiteTexture,
						Color = skin.Color.Main,
					},
					new HeaderButton
					{
						Layout = new Layout(Vector2.zero,new Vector2(height * 1.5f, height), LayoutAnchor.RightBottom),
						Text = "CLOSE",
						TextAnchor = TextAnchor.LowerCenter,
						OnClick = OnClose,
					},
					new HeaderButton
					{
						Layout = new Layout(Vector2.zero,new Vector2(height * 1.5f, height), LayoutAnchor.LeftBottom),
						Text = "BACK",
						TextAnchor = TextAnchor.LowerCenter,
						OnClick = OnBack,
					},
					m_HeaderText = new ScText
					{
						Layout = new HorizontalStretchLayout{
							Margin = new Vector2(height * 1.5f, height * 1.5f),
							Height = height,
						},
						FontSize = 22,
						MaskColor = skin.Color.Text,
						TextAnchor = TextAnchor.MiddleCenter,
					},
					new ScTexture
					{
						Layout = new HorizontalStretchLayout
						{
							Anchor = LayoutAnchor.Top,
							Height = 2,
							PosY = height,
							Margin = new Vector2(-(int)m_HeaderFooterHeight*2, -(int)m_HeaderFooterHeight*2)
						},
						Texture = Texture2D.whiteTexture,
						Color = skin.Color.Line,
					},
				},
			};
		}

		IScWidget CreateFooter()
		{
			var skin = m_Menu.Skin;
			var height = m_HeaderFooterHeight / 2f;
			return new ScWidget
			{
				Layout = new HorizontalStretchLayout
				{
					Anchor = LayoutAnchor.Bottom,
					Height = m_HeaderFooterHeight / 2f,
				},
				Children = new IScWidget[]{
					new ScTexture
					{
						Layout = new StretchLayout
						{
							Margin = new RectOffset(-(int)m_HeaderFooterHeight*2, -(int)m_HeaderFooterHeight*2, 0, -(int)m_HeaderFooterHeight*2)
						},
						Texture = Texture2D.whiteTexture,
						Color = new Color(1, 1, 1, 1f)
					},
					new ScTexture
					{
						Layout = new HorizontalStretchLayout
						{
							Anchor = LayoutAnchor.Top,
							Height = 2,
							Margin = new Vector2(-(int)m_HeaderFooterHeight*2, -(int)m_HeaderFooterHeight*2)
						},
						Texture = Texture2D.whiteTexture,
						Color = skin.Color.Line,
					},
					m_CategoryRoot = new ScScrollView
					{
						Layout = new StretchLayout{ Margin = new RectOffset(0, 0, 0, -(int)m_HeaderFooterHeight) },
						Width = m_HeaderFooterHeight,
						HideScrollBar = true,
						Direction = ScScrollView.Mode.Horizontal,
					}
				}
			};
		}

		public void ResetPage()
		{
			m_CategoryTab = m_Menu.GetCategory().Select(x => new CategoryTab(x, m_Menu, m_HeaderFooterHeight / 2f)
			{
				Extend = ExtendWidth.Get(m_HeaderFooterHeight / 3f + GetStrWidth(x)),
				OnSelect = m_Menu.SetCategory,
			}).ToArray();
			m_CategoryRoot.Children = m_CategoryTab;
			SetCategory(m_Menu.Category);
		}

		float GetStrWidth(string str)
		{
			if (string.IsNullOrEmpty(str)) return 4 * 16f;
			float size = 0f;
			foreach (var c in str)
			{
				if (char.IsDigit(c) || (c >= 'A' && c <= 'z'))
				{
					size += 16f;
				}
				else
				{
					size += 22f;
				}
			}
			return size;
		}

		internal void SetCategory(string name)
		{
			foreach (var tab in m_CategoryTab)
			{
				tab.SetSelect(tab.Name == name);
			}
			if (string.IsNullOrEmpty(name))
			{
				m_HeaderText.Text = "共通";
			}
			else
			{
				m_HeaderText.Text = name;
			}
			m_Menu.Category = name;
			SetPage("");
		}

		public void SetPage(string page)
		{
			m_Page = page;
			(IContent[] contnts, IScWidget[] widgets) = m_Menu.CreateWidget(page);
			if (string.IsNullOrEmpty(page))
			{
				m_MainScroll.Init(page, contnts, widgets);
			}
			else
			{
				m_MainScroll.Push(page, contnts, widgets);
			}
		}

		public void Update()
		{
			m_MainScroll.Update();
		}

		void OnClose()
		{
			m_Menu.Close();
		}

		void OnBack()
		{
			if (string.IsNullOrEmpty(m_Page)) return;
			m_Page = m_MainScroll.Pop();
		}

	}

}
#endif