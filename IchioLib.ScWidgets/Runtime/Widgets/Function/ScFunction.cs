using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{

	public interface IFunction
	{
		bool Interactable { get; }
	}

	public abstract class ScFunction : ScWidget, IFunction
	{
		bool m_Interactable = true;
		public bool Interactable
		{
			get => m_Interactable;
			set
			{
				SetDitry();
				m_Interactable = value;
			}
		}
	}
}