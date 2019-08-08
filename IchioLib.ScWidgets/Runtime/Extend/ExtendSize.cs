using UnityEngine;

namespace ILib.ScWidgets
{
	public class ExtendWidth : IExtend
	{
		public static ExtendWidth Get(float val) => new ExtendWidth { Width = val };

		public float Width { get; set; }
	}

	public class ExtendHeight : IExtend
	{
		public static ExtendHeight Get(float val) => new ExtendHeight { Height = val };

		public float Height { get; set; }
	}

	public class ExtendSize : IExtend
	{
		public static ExtendSize Get(float x, float y) => new ExtendSize { Width = x, Height = y };
		public static ExtendSize Get(Vector2 size) => new ExtendSize { Size = size };

		public float Width { get; set; }
		public float Height { get; set; }
		public Vector2 Size
		{
			get => new Vector2(Width, Height);
			set
			{
				Width = value.x;
				Height = value.y;
			}
		}
	}

	public interface IFixedWidth
	{
		float Width { get; }
	}

	public interface IFixedHeight
	{
		float Height { get; }
	}

	public static class ExtendSizeExtensions
	{
		public static Vector2 GetFixedExtendSize(this IScWidget self, Vector2 def)
		{
			return self.GetFixedExtendSize(def, Vector2.zero);
		}

		public static Vector2 GetFixedExtendSize(this IScWidget self, Vector2 def, Vector2 min)
		{
			var ret = def;
			var size = self.GetExtend<ExtendSize>();
			if (size != null)
			{
				ret = size.Size;
			}
			var fw = (self.Layout as IFixedWidth) ?? (self as IFixedWidth);
			if (fw != null)
			{
				ret.x = fw.Width;
			}
			var fh = (self.Layout as IFixedHeight) ?? (self as IFixedHeight);
			if (fh != null)
			{
				ret.y = fh.Height;
			}
			var w = self.GetExtend<ExtendWidth>();
			if (w != null)
			{
				ret.x = w.Width;
			}
			var h = self.GetExtend<ExtendHeight>();
			if (h != null)
			{
				ret.y = h.Height;
			}
			return Vector2.Max(ret, min);
		}
	}


}
