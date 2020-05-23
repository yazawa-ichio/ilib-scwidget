using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{
	public interface IOrderWidget
	{
		int Order { get; }
	}

	public class ScWindow : ScWidget, IOrderWidget
	{

		Color m_Color = new Color(1f, 1f, 1f, 1f);
		public Color Color
		{
			get => m_Color;
			set
			{
				m_Color = value;
				SetDitry();
			}
		}

		public int Order { get; set; }

		public override Color GetMaskColor()
		{
			return base.GetMaskColor() * Color;
		}
	}
}