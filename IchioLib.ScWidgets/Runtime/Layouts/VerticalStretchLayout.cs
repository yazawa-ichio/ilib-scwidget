using UnityEngine;

namespace ILib.ScWidgets
{
	public class VerticalStretchLayout : LayoutBase, IFixedWidth
	{
		public Vector2 Margin { get; set; }
		public float Width { get; set; }
		public float PosX { get; set; }

		LayoutAnchor m_Anchor = LayoutAnchor.None;
		public LayoutAnchor Anchor
		{
			get => m_Anchor;
			set
			{
				m_Anchor = value;
				Debug.AssertFormat((value & (LayoutAnchor.Left | LayoutAnchor.Right)) > 0, "invalid Anchor. {0}", value);
			}
		}

		public override Rect CalcRect(Rect viewRect)
		{
			var top = viewRect.yMin + Margin.x;
			var height = viewRect.width - Margin.x - Margin.y;
			var left = 0f;
			switch (m_Anchor)
			{
				case LayoutAnchor.None:
					left = viewRect.center.y - Width / 2f;
					break;
				case LayoutAnchor.Left:
					left = viewRect.xMin;
					break;
				case LayoutAnchor.Right:
					left = viewRect.xMax - Width;
					break;
				default:
					throw new System.InvalidOperationException($"Invalid Anchor,{Anchor}");
			}
			return new Rect(left + PosX, top, Width, height);
		}
	}
}