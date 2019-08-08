using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{

	public class ScPageView : IScWidget
	{
		public string Name { get; set; }

		ILayout IScWidget.Layout
		{
			get => m_Layout;
			set => Layout = value;
		}

		ILayout m_Layout;
		public ILayout Layout
		{
			protected get => m_Layout;
			set
			{
				if (m_Layout != null) SetDitry();
				m_Layout = value;
			}
		}

		public int Revision { get; protected set; }
		bool m_IsDirty = true;
		public bool IsDirty
		{
			get { return m_IsDirty; }
			protected set
			{
				if (m_IsDirty != value)
				{
					m_IsDirty = value;
					Revision++;
				}
			}
		}

		protected Rect m_Rect;
		protected Rect m_ParentRect;
		public IScWidget Parent { get; private set; }

		public IExtend[] Extends { get; set; }

		public IExtend Extend
		{
			set => Extends = new IExtend[] { value };
		}

		protected List<IScWidget> m_Children = new List<IScWidget>(1);

		int m_Index;
		public int Index
		{
			get => m_Index;
			set
			{
				if (m_Pages == null)
				{
					m_Index = 0;
				}
				else
				{
					m_Index = Mathf.Clamp(value, 0, m_Pages.Count);
				}
				SetDitry();
			}
		}

		protected List<IScWidget> m_Pages;
		public IScWidget[] Pages
		{
			set
			{
				if (m_Pages == null)
				{
					m_Pages = new List<IScWidget>();
				}
				else
				{
					m_Pages.Clear();
				}
				foreach (var child in value)
				{
					child.SetParent(this);
					m_Pages.Add(child);
				}
			}
		}

		void IScWidget.SetParent(IScWidget widget)
		{
			Parent = widget;
		}

		public List<IScWidget> GetChildren()
		{
			m_Children.Clear();
			if (m_Index < m_Pages.Count)
			{
				m_Children.Add(m_Pages[m_Index]);
			}
			return m_Children;
		}

		public void Add(IScWidget widget)
		{
			if (m_Pages == null)
			{
				m_Pages = new List<IScWidget>();
			}
			m_Pages.Add(widget);
			widget.SetParent(this);
			SetDitry();
		}

		public bool Remove(IScWidget widget)
		{
			if (m_Pages == null)
			{
				return false;
			}
			var ret = m_Pages.Remove(widget);
			if (ret)
			{
				m_Index = Mathf.Clamp(m_Index, 0, m_Pages.Count);
				SetDitry();
			}
			return ret;
		}

		public void SetDitry()
		{
			IsDirty = true;
			if (m_Pages == null)
			{
				return;
			}
			foreach (var child in m_Pages)
			{
				child.SetDitry();
			}
		}

		public virtual void CalcLayout(Rect rect)
		{
			m_ParentRect = rect;
			if (IsDirty)
			{
				IsDirty = false;
				if (Layout == null) Layout = new StretchLayout();
				m_Rect = Layout.CalcRect(rect);
			}
			foreach (var child in GetChildren())
			{
				child.CalcLayout(m_Rect);
			}
		}

		public Rect GetRect()
		{
			return m_Rect;
		}

		public Rect GetRelativeRect()
		{
			return new Rect(m_Rect.position - m_ParentRect.position, m_Rect.size);
		}

		public virtual Color GetMaskColor()
		{
			return Parent != null ? Parent.GetMaskColor() : new Color(1f, 1f, 1f, 1f);
		}

		public bool IsOutsideClip()
		{
			return !GetViewRectWithoutClip().Overlaps(m_Rect);
		}

		public Rect GetViewRectWithoutClip()
		{
			if (Parent == null)
			{
				if (this is IClipWidget clip)
				{
					return clip.GetClipRect();
				}
				return m_Rect;
			}
			else
			{
				var rect = Parent.GetViewRectWithoutClip();
				if (this is IClipWidget clip)
				{
					return clip.GetClipRect();
				}
				return rect;
			}
		}
		
	}

}
