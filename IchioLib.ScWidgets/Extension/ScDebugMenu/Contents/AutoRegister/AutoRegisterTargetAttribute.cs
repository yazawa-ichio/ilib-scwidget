using System;

namespace ILib.Debugs.AutoRegisters
{
	[AttributeUsage(AttributeTargets.Class)]
	public class AutoRegisterTargetAttribute : Attribute
	{
		public string Category { get; set; }
		public int CategoryPriority { get; set; }
		public string Path { get; set; }
	}
}