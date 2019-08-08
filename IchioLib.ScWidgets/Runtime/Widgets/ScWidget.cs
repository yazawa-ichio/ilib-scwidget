using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{
	public interface IScWidget
	{
		string Name { get; set; }
		ILayout Layout { set; get; }
		IScWidget Parent { get; }
		IExtend[] Extends { get; set; }
		bool IsDirty { get; }
		void SetParent(IScWidget widget);
		List<IScWidget> GetChildren();
		void SetDitry();
		void CalcLayout(Rect rect);
		Rect GetRect();
		Rect GetRelativeRect();
		Color GetMaskColor();
		Rect GetViewRectWithoutClip();
	}

	public class ScWidget : IScWidget
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

		protected List<IScWidget> m_Children = new List<IScWidget>();
		public IScWidget[] Children
		{
			set
			{
				m_Children.Clear();
				foreach (var child in value)
				{
					child.SetParent(this);
					m_Children.Add(child);
				}
			}
		}

		public IScWidget Child
		{
			set
			{
				if (m_Children == null)
				{
					m_Children = new List<IScWidget>();
				}
				else
				{
					m_Children.Clear();
				}
				m_Children.Add(value);
				value.SetParent(this);
			}
		}

		void IScWidget.SetParent(IScWidget widget)
		{
			Parent = widget;
		}

		public List<IScWidget> GetChildren()
		{
			return m_Children;
		}

		public void Add(IScWidget widget)
		{
			if (m_Children == null)
			{
				m_Children = new List<IScWidget>();
			}
			m_Children.Add(widget);
			widget.SetParent(this);
			SetDitry();
		}

		public bool Remove(IScWidget widget)
		{
			if (m_Children == null)
			{
				return false;
			}
			var ret = m_Children.Remove(widget);
			if (ret) SetDitry();
			return ret;
		}

		public void SetDitry()
		{
			IsDirty = true;
			if (m_Children == null)
			{
				return;
			}
			foreach (var child in m_Children)
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
			if (m_Children == null)
			{
				return;
			}
			foreach (var child in m_Children)
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
