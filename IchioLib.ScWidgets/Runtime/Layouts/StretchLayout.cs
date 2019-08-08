using UnityEngine;

namespace ILib.ScWidgets
{
	public class StretchLayout : LayoutBase
	{
		RectOffset m_Margin = null;
		public RectOffset Margin { get => m_Margin; set => m_Margin = value; }

		public override Rect CalcRect(Rect viewRect)
		{
			if (m_Margin == null) return viewRect;
			var pos = viewRect.position;
			pos.x += m_Margin.left;
			pos.y += m_Margin.top;
			var size = viewRect.size;
			size.x -= m_Margin.left + m_Margin.right;
			size.y -= m_Margin.top + m_Margin.bottom;
			return new Rect(pos, size);
		}
	}
}
