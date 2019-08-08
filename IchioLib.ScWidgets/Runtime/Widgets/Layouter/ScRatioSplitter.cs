using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ILib.ScWidgets
{
	public abstract class ScRatioSplitterBase : ScMinimumWidget
	{
		public interface IEntry
		{
			int Weight { get; set; } 
		}

		protected class Entry : IEntry
		{
			public int Weight { get; set; }
			public IScWidget Widget { get; set; }
		}

		protected List<Entry> m_List = new List<Entry>();

		public IEntry Set(IScWidget widget, int weight)
		{
			foreach (var entry in m_List)
			{
				if (entry.Widget == entry)
				{
					entry.Weight = weight;
					if (m_Children.Contains(widget))
					{
						if (weight > 0)
						{
							m_Children.Add(widget);
						}
						else
						{
							m_Children.Remove(widget);
						}
					}
					return entry;
				}
			}
			var ret = new Entry { Widget = widget, Weight = weight };
			m_List.Add(ret);
			if (weight > 0)
			{
				m_Children.Add(widget);
			}
			return ret; 
		}

		public void Remove(IScWidget widget)
		{
			m_List.RemoveAll(x => x.Widget == widget);
			m_Children.RemoveAll(x => x == widget);
		}

		public void Clear()
		{
			foreach (var e in m_List)
			{
				m_Children.Remove(e.Widget);
			}
			m_List.Clear();
		}

	}

	public class ScHorizontalRatioSplitter : ScRatioSplitterBase
	{
		public override void CalcLayout(Rect rect)
		{
			m_ParentRect = rect;
			if (IsDirty)
			{
				IsDirty = false;
				if (Layout == null) Layout = new StretchLayout();
				m_Rect = Layout.CalcRect(rect);
			}

			var sum = m_List.Sum(x => x.Weight);
			Vector2 pos = m_Rect.position;
			Vector2 size = m_Rect.size;
			foreach (var entry in m_List)
			{
				size.x = ((float)entry.Weight / sum) * m_Rect.width;
				entry.Widget.CalcLayout(new Rect(pos, size));
				pos.x += size.x;
			}

		}
	}

	public class ScVerticalRatioSplitter : ScRatioSplitterBase
	{
		public override void CalcLayout(Rect rect)
		{
			m_ParentRect = rect;
			if (IsDirty)
			{
				IsDirty = false;
				if (Layout == null) Layout = new StretchLayout();
				m_Rect = Layout.CalcRect(rect);
			}

			var sum = m_List.Sum(x => x.Weight);
			Vector2 pos = m_Rect.position;
			Vector2 size = m_Rect.size;
			foreach (var entry in m_List)
			{
				size.x = ((float)entry.Weight / sum) * m_Rect.height;
				entry.Widget.CalcLayout(new Rect(pos, size));
				pos.y += size.y;
			}
		}
	}


}
