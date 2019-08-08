using UnityEngine;

namespace ILib.ScWidgets
{
	public class HorizontalStretchLayout : LayoutBase, IFixedHeight
	{
		public Vector2 Margin { get; set; }
		public float PosY { get; set; }
		public float Height { get; set; }

		LayoutAnchor m_Anchor = LayoutAnchor.None;
		public LayoutAnchor Anchor
		{
			get => m_Anchor;
			set
			{
				m_Anchor = value;
				Debug.AssertFormat((value & (LayoutAnchor.Top | LayoutAnchor.Bottom)) > 0 || value == 0, "invalid Anchor. {0}", value);
			}
		}

		public override Rect CalcRect(Rect viewRect)
		{
			var left = viewRect.xMin + Margin.x;
			var width = viewRect.width - Margin.x - Margin.y;
			var top = 0f;
			switch (m_Anchor)
			{
				case LayoutAnchor.None:
					top = viewRect.center.y - Height / 2f;
					break;
				case LayoutAnchor.Top:
					top = viewRect.yMin;
					break;
				case LayoutAnchor.Bottom:
					top = viewRect.yMax - Height;
					break;
				default:
					throw new System.InvalidOperationException($"Invalid Anchor,{Anchor}");
			}
			return new Rect(left, top + PosY, width, Height);
		}
	}
}
