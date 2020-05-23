#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ILib.Debugs
{
	public class Contexts
	{
		Dictionary<Type, WeakReference> m_Dic = new Dictionary<Type, WeakReference>();

		public void Bind<T>(T obj) where T : class
		{
			Bind(typeof(T), obj);
		}

		internal void Bind(Type type, object obj)
		{
			WeakReference reference;
			if (!m_Dic.TryGetValue(type, out reference))
			{
				m_Dic[type] = reference = new WeakReference(obj);
			}
			else
			{
				reference.Target = obj;
			}
		}

		public void Unbind<T>(T obj) where T : class
		{
			Unbind(typeof(T), obj);
		}

		internal void Unbind(Type type, object obj)
		{
			WeakReference reference;
			if (m_Dic.TryGetValue(type, out reference) && reference.IsAlive)
			{
				if (reference.Target == obj)
				{
					m_Dic.Remove(type);
				}
			}
		}

		internal bool TryGet<T>(out T context) where T : class
		{
			WeakReference reference;
			if (m_Dic.TryGetValue(typeof(T), out reference) && reference.IsAlive)
			{
				context = reference.Target as T;
				return context != null;
			}
			context = null;
			return false;
		}

		internal T Get<T>() where T : class
		{
			WeakReference reference;
			if (m_Dic.TryGetValue(typeof(T), out reference) && reference.IsAlive)
			{
				return reference.Target as T;
			}
			return default;
		}

		internal object Get(Type type)
		{
			WeakReference reference;
			if (m_Dic.TryGetValue(type, out reference) && reference.IsAlive)
			{
				return reference.Target;
			}
			return default;
		}

		internal IEnumerable<Type> GetTypes()
		{
			foreach (var kvp in m_Dic)
			{
				if (kvp.Value.IsAlive)
				{
					yield return kvp.Key;
				}
			}
		}


	}
}
#endif