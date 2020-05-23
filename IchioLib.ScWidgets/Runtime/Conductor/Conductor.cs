using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace ILib.ScWidgets
{
	public interface IEntryEventCallback
	{
		void OnRegister(IScWidget widget);
		void OnRemove();
	}

	public abstract class Conductor<T, UEntry> : ConductorBase<T> where T : Conductor<T, UEntry>
	{

		public Conductor()
		{
			SetContext((T)this);
		}

		Dictionary<IScWidget, UEntry> m_Entry = new Dictionary<IScWidget, UEntry>();
		HashSet<IScWidget> m_Used = new HashSet<IScWidget>();

		protected IEnumerable<UEntry> GetEntry()
		{
			return m_Entry.Values;
		}

		public UEntry GetEntry(IScWidget widget)
		{
			if (widget == null) return default;
			m_Used.Add(widget);
			UEntry entry;
			if (m_Entry.TryGetValue(widget, out entry))
			{
				return entry;
			}
			m_Entry[widget] = entry = CreateEntry();
			if (entry is IEntryEventCallback c)
			{
				c.OnRegister(widget);
			}
			return entry;
		}

		protected virtual UEntry CreateEntry()
		{
			return Activator.CreateInstance<UEntry>();
		}

		protected void RemoveUnusedEntry()
		{
			foreach (var key in m_Entry.Keys.ToArray())
			{
				if (!m_Used.Contains(key))
				{
					var entry = m_Entry[key];
					m_Entry.Remove(key);
					if (entry is IEntryEventCallback c)
					{
						c.OnRemove();
					}
				}
			}
			m_Used.Clear();
		}


	}
}