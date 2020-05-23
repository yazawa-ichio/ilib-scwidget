#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;

namespace ILib.Debugs.Widgets
{
	public class DebugItemTapHandler : ScMinimumWidget
	{

		public System.Action OnTap { private get; set; }

		public class Handler : SkinIMGUIDrawer.IMGUIHandler<DebugItemTapHandler>
		{
			protected override void Run(SkinIMGUIDrawer context, DebugItemTapHandler widget)
			{
				var ret = GUI.Button(widget.GetRect(), "", GetStyle(context, widget, "Label"));
				if (ret)
				{
					widget.OnTap?.Invoke();
				}
			}
		}
	}
}
#endif