#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.Debugs
{
	using Widgets;
	public abstract class ToggleContent<TContext> : Content<TContext, DebugToggleItem> where TContext : class
	{

		protected abstract string Label { get; }
		protected abstract bool Value { get; set; }

		protected sealed override void Register(DebugToggleItem widget)
		{
			widget.Label = Label;
			widget.Value = Value;
			widget.OnChange = OnChanged;
		}

		void OnChanged(bool val)
		{
			Value = val;
		}
	}
}
#endif