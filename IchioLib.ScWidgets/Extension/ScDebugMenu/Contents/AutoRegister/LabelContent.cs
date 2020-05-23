#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.Debugs.AutoRegisters
{
	using Widgets;
	internal class LabelContent : AutoRegisterContent<DebugLabelAttribute, DebugLabelItem>
	{
		protected override void Register(DebugLabelItem widget)
		{
			Update();
		}

		public override void Update()
		{
			Widget.Label = "" + Attr.Method.Invoke(Context, null);
		}
	}
}
#endif