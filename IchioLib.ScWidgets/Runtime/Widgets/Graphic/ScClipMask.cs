using UnityEngine;

namespace ILib.ScWidgets
{
	public interface IClipWidget
	{
		Rect GetClipRect();
	}

	public class ScClipMask : ScGraphic, IClipWidget
	{
		public Rect GetClipRect()
		{
			return m_Rect;
		}
	}
}
