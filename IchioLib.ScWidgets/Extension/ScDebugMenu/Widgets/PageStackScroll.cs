#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;
using System.Linq;

namespace ILib.Debugs.Widgets
{
	internal class PageStackScroll : ScWidget
	{
		class Page
		{
			public string Name;
			public Vector2 Position;
			public IScWidget[] Widgets;
			public IContent[] Contents;
		}

		ScScrollView m_ScrollView;
		Stack<Page> m_PageStack = new Stack<Page>();

		public float Height
		{
			get => m_ScrollView.Height;
			set => m_ScrollView.Height = value;
		}

		public PageStackScroll(float height)
		{
			m_ScrollView = new ScScrollView();
			m_ScrollView.Direction = ScScrollView.Mode.Vertical;
			m_ScrollView.Height = height;
			Add(m_ScrollView);
		}


		public void Init(string name, IContent[] contents, IScWidget[] widgets)
		{
			m_PageStack.Clear();
			Push(name, contents, widgets);
		}

		public void Push(string name, IContent[] contents, IScWidget[] widgets)
		{
			if (m_PageStack.Count > 0)
			{
				m_PageStack.Peek().Position = m_ScrollView.ScrollViewPostion;
			}
			var page = new Page { Name = name, Position = Vector2.zero, Widgets = widgets, Contents = contents };
			m_PageStack.Push(page);
			Set(page);
		}

		public string Pop()
		{
			m_PageStack.Pop();
			if (m_PageStack.Count == 0) return "";
			Set(m_PageStack.Peek());
			return m_PageStack.Peek().Name;
		}

		void Set(Page page)
		{
			m_ScrollView.Children = System.Array.Empty<IScWidget>();
			for (int i = 0; i < page.Widgets.Length; i++)
			{
				IScWidget widget = page.Widgets[i];
				m_ScrollView.Add(widget);
				m_ScrollView.Add(new ScTexture
				{
					Layout = new HorizontalStretchLayout
					{
						Height = 1,
					},
					Texture = Texture2D.whiteTexture,
					Color = Color.black,
				});
			}
			m_ScrollView.ScrollViewPostion = page.Position;
		}

		public void Update()
		{
			if (m_PageStack.Count == 0) return;
			var page = m_PageStack.Peek();
			foreach (var c in page.Contents)
			{
				c.Update();
			}
		}

	}
}
#endif