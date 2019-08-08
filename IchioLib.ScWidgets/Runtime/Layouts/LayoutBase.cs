using UnityEngine;

namespace ILib.ScWidgets
{
	public abstract class LayoutBase : ILayout
	{
		public abstract Rect CalcRect(Rect viewRect);
	}
}
