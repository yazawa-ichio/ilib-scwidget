using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{

	public class ScMinimumWidget : IScWidget
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
		protected List<IScWidget> m_Children = new List<IScWidget>();

		public IExtend[] Extends { get; set; }

		public IExtend Extend
		{
			set => Extends = new IExtend[] { value };
		}

		void IScWidget.SetParent(IScWidget widget)
		{
			Parent = widget;
		}

		public List<IScWidget> GetChildren()
		{
			return m_Children;
		}

		public void SetDitry()
		{
			IsDirty = true;
			foreach (var child in GetChildren())
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
			foreach (var c in GetChildren())
			{
				c.CalcLayout(m_Rect);
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