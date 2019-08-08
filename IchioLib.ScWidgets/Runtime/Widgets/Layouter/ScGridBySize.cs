using UnityEngine;

namespace ILib.ScWidgets
{
	public class ScGridBySize : ScWidget
	{

		public bool UsePercentage
		{
			set
			{
				m_UseWidthPercentage = m_UseHeightPercentage = value;
				SetDitry();
			}
		}


		bool m_UseWidthPercentage;
		public bool UseWidthPercentage
		{
			get => m_UseWidthPercentage;
			set
			{
				m_UseWidthPercentage = value;
				SetDitry();
			}
		}

		bool m_UseHeightPercentage;
		public bool UseHeightPercentage
		{
			get => m_UseHeightPercentage;
			set
			{
				m_UseHeightPercentage = value;
				SetDitry();
			}
		}

		public Vector2 CellSize
		{
			get => new Vector2(CellWidth, CellHeight);
			set
			{
				m_CellWidth = value.x;
				m_CellHeight = value.y;
				SetDitry();
			}
		}

		float m_CellWidth;
		public float CellWidth
		{
			get => m_CellWidth;
			set
			{
				m_CellWidth = value;
				SetDitry();
			}
		}

		float m_CellHeight;
		public float CellHeight
		{
			get => m_CellHeight;
			set
			{
				m_CellHeight = value;
				SetDitry();
			}
		}


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
			var size = CellSize;
			if (UseWidthPercentage)
			{
				size.x = size.x / 100f * m_Rect.width;
			}
			if (UseHeightPercentage)
			{
				size.y = size.y / 100f * m_Rect.height;
			}
			foreach (var child in m_Children)
			{
				child.CalcLayout(new Rect(m_Rect.position + pos, size));
				if (pos.x + size.x + size.x < m_Rect.width)
				{
					pos.x += size.x;
				}
				else
				{
					pos.x = 0;
					pos.y += size.y;
				}
			}
		}

	}

}
