using System.Collections;
using UnityEngine;

using Direction = UnityEngine.UI.Slider.Direction;

namespace ILib.ScWidgets
{
	public class ScScrollView : ScWidget, IClipWidget
	{
		public enum Mode
		{
			Vertical,
			Horizontal,
		}
		static readonly RectOffset s_DefVerticalMargin = new RectOffset(0, 0, 0, 0);
		static readonly RectOffset s_DefHorizontalMargin = new RectOffset(0, 0, 0, 0);

		public float Width { set; get; } = 0;
		public float Height { set; get; } = 0;
		public RectOffset Margin { get; set; }
		public Mode Direction { get; set; } = Mode.Vertical;
		public bool HideScrollBar { get; set; }

		Vector2 m_ScrollViewPostion;
		public Vector2 ScrollViewPostion
		{
			get => m_ScrollViewPostion;
			set
			{
				if (value != m_ScrollViewPostion)
				{
					m_ScrollViewPostion = value;
					SetDitry();
				}
			}
		}

		Rect m_ContentsRect;

		public override void CalcLayout(Rect rect)
		{
			m_ParentRect = rect;
			if (!HasDirty())
			{
				return;
			}

			if (IsDirty)
			{
				IsDirty = false;
				if (Layout == null) Layout = new StretchLayout();
				m_Rect = Layout.CalcRect(rect);
				if (m_Children == null)
				{
					m_ContentsRect = Rect.zero;
					return;
				}
			}

			var m = Margin ?? (Direction == Mode.Vertical ? s_DefVerticalMargin : s_DefHorizontalMargin);
			var w = (Width == 0 ? m_Rect.width : Width) - m.horizontal;
			var h = (Height == 0 ? m_Rect.height : Height) - m.vertical;
			var count = m_Children.Count;
			var pos = m_Rect.position + new Vector2(m.left, m.top);
			var defSize = new Vector2(w, h);
			var contentsSize = Vector2.zero;
			for (int i = 0; i < count; i++)
			{
				var c = m_Children[i];
				var size = c.GetFixedExtendSize(defSize);

				c.CalcLayout(new Rect(pos, size));

				switch (Direction)
				{
					case Mode.Vertical:
						pos.y += size.y;
						contentsSize.y += size.y;
						if (i == 0) contentsSize.x = w;
						contentsSize.x = Mathf.Max(contentsSize.x, size.x);
						break;
					case Mode.Horizontal:
						pos.x += size.x;
						contentsSize.x += size.x;
						if (i == 0) contentsSize.y = h;
						contentsSize.y = Mathf.Max(contentsSize.y, size.y);
						break;
				}
			}
			m_ContentsRect = new Rect(m_Rect.position + new Vector2(m.left, m.top), contentsSize);

		}

		bool HasDirty()
		{
			if (IsDirty) return true;
			if (m_Children == null) return false;
			foreach (var c in m_Children)
			{
				if (c.IsDirty) return true;
			}
			return false;
		}

		public Rect GetContentsRect()
		{
			return m_ContentsRect;
		}

		public Rect GetClipRect()
		{
			return new Rect(m_Rect.position + m_ScrollViewPostion, m_Rect.size);
		}

	}
}