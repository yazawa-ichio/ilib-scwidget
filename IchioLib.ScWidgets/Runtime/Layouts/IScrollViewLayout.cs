using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{
	public interface IScrollViewLayout
	{
		Rect CalcContentsRect(ScScrollView widget, List<IScWidget> children);
	}
}