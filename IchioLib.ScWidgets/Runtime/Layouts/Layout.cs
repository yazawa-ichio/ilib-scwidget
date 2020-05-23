using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{

	public class Layout : LayoutBase, IFixedWidth, IFixedHeight
	{
		public LayoutAnchor Anchor { get; set; } = LayoutAnchor.None;

		Vector2 m_Pos;
		public Vector2 Pos { get => m_Pos; set => m_Pos = value; }

		Vector2 m_Size;
		public Vector2 Size { get => m_Size; set => m_Size = value; }

		float IFixedWidth.Width => m_Size.x;

		float IFixedHeight.Height => m_Size.y;

		public Layout() { }
		public Layout(Vector2 pos, Vector2 size, LayoutAnchor anchor = LayoutAnchor.None)
		{
			m_Pos = pos;
			m_Size = size;
			Anchor = anchor;
		}


		public override Rect CalcRect(Rect viewRect)
		{
			Vector2 rectPos;
			switch (Anchor)
			{
				case LayoutAnchor.None:
					rectPos = viewRect.center + m_Pos - m_Size / 2f;
					break;
				case LayoutAnchor.Left:
					rectPos.x = viewRect.xMin + m_Pos.x;
					rectPos.y = viewRect.center.y + m_Pos.y - m_Size.y / 2f;
					break;
				case LayoutAnchor.Right:
					rectPos.x = viewRect.xMax + m_Pos.x - m_Size.x;
					rectPos.y = viewRect.center.y + m_Pos.y - m_Size.y / 2f;
					break;
				case LayoutAnchor.Top:
					rectPos.x = viewRect.center.y + m_Pos.x - m_Size.x / 2f;
					rectPos.y = viewRect.yMin + m_Pos.y;
					break;
				case LayoutAnchor.Bottom:
					rectPos.x = viewRect.center.y + m_Pos.x - m_Size.x / 2f;
					rectPos.y = viewRect.yMax + m_Pos.y - m_Size.y;
					break;
				case LayoutAnchor.LeftTop:
					rectPos = viewRect.position + m_Pos;
					break;
				case LayoutAnchor.RightTop:
					rectPos.x = viewRect.xMax + m_Pos.x - m_Size.x;
					rectPos.y = viewRect.yMin + m_Pos.y;
					break;
				case LayoutAnchor.RightBottom:
					rectPos.x = viewRect.xMax + m_Pos.x - m_Size.x;
					rectPos.y = viewRect.yMax + m_Pos.y - m_Size.y;
					break;
				case LayoutAnchor.LeftBottom:
					rectPos.x = viewRect.xMin + m_Pos.x;
					rectPos.y = viewRect.yMax + m_Pos.y - m_Size.y;
					break;
				default:
					throw new System.InvalidOperationException($"Invalid Anchor,{Anchor}");
			}
			return new Rect(rectPos, m_Size);
		}
	}
}