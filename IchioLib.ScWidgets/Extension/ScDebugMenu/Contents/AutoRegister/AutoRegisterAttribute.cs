using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace ILib.Debugs.AutoRegisters
{
	using Widgets;

	public abstract class AutoRegisterAttribute : Attribute
	{
		public string Name { get; private set; }
		public int Priority { get; set; }
		public string Category { get; set; }
		public int CategoryPriority { get; set; }
		public string Path { get; set; }

		public AutoRegisterAttribute(string name)
		{
			Name = name;
		}

	}

	public class DebugLabelAttribute : AutoRegisterAttribute
	{
		internal MethodInfo Method { get; set; }
		public DebugLabelAttribute(string name) : base(name) { }
	}

	public class DebugButtonAttribute : AutoRegisterAttribute
	{
		internal MethodInfo Method { get; set; }
		public DebugButtonAttribute(string name) : base(name) { }
	}

	public class DebugSliderAttribute : AutoRegisterAttribute
	{
		internal PropertyInfo Property { get; set; }
		internal FieldInfo Field { get; set; }
		public float MinValue { get; set; } = 0f;
		public float MaxValue { get; set; } = 1f;
		public DebugSliderAttribute(string name) : base(name) { }
	}

	public class DebugInputAttribute : AutoRegisterAttribute
	{
		internal PropertyInfo Property { get; set; }
		internal FieldInfo Field { get; set; }
		public DebugInputAttribute(string name) : base(name) { }
	}

	public class DebugToggleAttribute : AutoRegisterAttribute
	{
		internal PropertyInfo Property { get; set; }
		internal FieldInfo Field { get; set; }
		public DebugToggleAttribute(string name) : base(name) { }
	}

}
