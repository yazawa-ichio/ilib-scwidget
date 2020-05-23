using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{
	public class ScGrid : ScWidget
	{
		int m_VerticalCount;
		public int VerticalCount
		{
			set
			{
				m_VerticalCount = value;
				SetDitry();
				Debug.Assert(value > 0);
			}
			get => m_VerticalCount;
		}
		int m_HorizontalCount;
		public int HorizontalCount
		{
			set
			{
				m_HorizontalCount = value;
				SetDitry();
				Debug.Assert(value > 0);
			}
			get => m_HorizontalCount;
		}

		public Vector2 Padding { get; set; }

		public override void CalcLayout(Rect rect)
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
			var pos = new Vector2();
			var w = m_Rect.width - Padding.x * (m_HorizontalCount - 1);
			var h = m_Rect.height - Padding.y * (m_VerticalCount - 1);
			var size = new Vector2(w / m_HorizontalCount, h / m_VerticalCount);
			int index = 0;
			foreach (var child in m_Children)
			{
				index++;
				child.CalcLayout(new Rect(m_Rect.position + pos, size));
				if (index < m_HorizontalCount)
				{
					pos.x += size.x + Padding.x;
				}
				else
				{
					index = 0;
					pos.x = 0;
					pos.y += size.y + Padding.y;
				}
			}
		}


	}

}