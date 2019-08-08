using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{
	public static class Extensions
	{
		public static void Reset(this RectOffset self)
		{
			self.left = 0;
			self.right = 0;
			self.top = 0;
			self.bottom = 0;
		}

		public static void Set(this RectOffset self, int left = 0, int right = 0, int top = 0, int bottom = 0)
		{
			self.left = left;
			self.right = right;
			self.top = top;
			self.bottom = bottom;
		}

		public static void Set(this RectOffset self, float left = 0, float right = 0, float top = 0, float bottom = 0)
		{
			self.left = (int)left;
			self.right = (int)right;
			self.top = (int)top;
			self.bottom = (int)bottom;
		}
	}
}
