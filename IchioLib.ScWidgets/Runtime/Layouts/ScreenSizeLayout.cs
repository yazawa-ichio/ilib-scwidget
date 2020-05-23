using UnityEngine;

namespace ILib.ScWidgets
{
	public class ScreenSizeLayout : LayoutBase
	{
		public override Rect CalcRect(Rect viewRect)
		{
			return new Rect(0, 0, Screen.width, Screen.height);
		}
	}
}