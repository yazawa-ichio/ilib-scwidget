using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;
using System;
using System.Linq;

namespace ILib.Debugs
{
	
	public class ScDebugMenu : MonoBehaviour
	{
		[SerializeField]
		IMGUIWidgetSkin m_Skin;

#if DEBUG || ILIB_DEBUG_MENU

		public IMGUIWidgetSkin Skin
		{
			set => m_Skin = value;
			get => m_Skin;
		}

		bool m_Initialized;
		SkinIMGUIDrawer m_Drawer;
		DebugScreen m_Root;
		bool m_Closed;
		bool m_SetCategory;
		bool m_SetPage;
		string m_Page;

		public string Category { get; set; }

		public Contexts Contexts { get; private set; } = new Contexts();

		Vector2 m_PortraitSize = new Vector2(720 / 2f, 1280 / 2f);
		Vector2 m_LandscapeSize = new Vector2(1280 / 2f, 720 / 2f);

		void Awake()
		{
			Initialize();
			enabled = false;
		}

		public void Open()
		{
			Initialize();
			enabled = true;
		}

		public void Close()
		{
			m_Closed = true;
		}

		void Initialize()
		{
			if (m_Initialized) return;
			m_Initialized = true;
			m_Drawer = new SkinIMGUIDrawer(m_Skin);
			m_Drawer.SetRootWidget(m_Root = new DebugScreen(this));
		}

		void Update()
		{
			if (m_Closed)
			{
				enabled = false;
			}
			if (m_SetCategory)
			{
				m_SetCategory = false;
				m_Root.SetCategory(Category);
			}
			if (m_SetPage)
			{
				m_SetPage = false;
				m_Root.SetPage(m_Page);
				m_Page = "";
			}
			m_Root.Update();
		}

		void OnGUI()
		{
			if (Screen.width > Screen.height)
			{
				m_Root.ContentsHeight = 40;
				m_Drawer.DrawScreen(m_LandscapeSize);
			}
			else
			{
				m_Root.ContentsHeight = 40;
				m_Drawer.DrawScreen(m_PortraitSize);
			}
		}

		private void OnEnable()
		{
			m_Closed = false;
			m_Root.ResetPage();
			m_Page = "";
			m_SetPage = false;
		}

		internal void SetPage(string page)
		{
			m_Page = page;
			m_SetPage = true;
		}

		internal void SetCategory(string category)
		{
			Category = category;
			m_SetCategory = true;
		}

#endif

	}
}
