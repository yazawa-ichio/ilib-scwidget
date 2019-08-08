using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{
	public interface IExtend { }

	public static class IExtendExtensions
	{
		public static T GetExtend<T>(this IScWidget self) where T : IExtend
		{
			if (self.Extends != null)
			{
				foreach (var e in self.Extends)
				{
					if (e is T ret) return ret;
				}
			}
			return default;
		}

		public static IEnumerable<T> GetExtends<T>(this IScWidget self) where T : IExtend
		{
			if (self.Extends != null)
			{
				foreach (var e in self.Extends)
				{
					if (e is T ret) yield return ret;
				}
			}
		}

		public static T GetOrAddExtend<T>(this IScWidget self) where T : IExtend, new()
		{
			var ret = self.GetExtend<T>();
			if(ret == default) ret = self.AddExtend<T>();
			return ret;
		}

		public static T AddExtend<T>(this IScWidget self, T extend) where T : IExtend
		{
			var extends = self.Extends;
			if (extends == null)
			{
				extends = new IExtend[] { extend };
			}
			else
			{
				var len = extends.Length;
				System.Array.Resize(ref extends, len + 1);
				extends[len] = extend;
			}
			return extend;
		}

		public static T AddExtend<T>(this IScWidget self) where T : IExtend, new()
		{
			return self.AddExtend(new T());
		}

	}
}
