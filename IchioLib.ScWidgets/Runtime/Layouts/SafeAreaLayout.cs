using UnityEngine;

namespace ILib.ScWidgets
{
	public class SafeAreaLayout : LayoutBase
	{
		public override Rect CalcRect(Rect viewRect)
		{
			return Screen.safeArea;
		}
	}
}
