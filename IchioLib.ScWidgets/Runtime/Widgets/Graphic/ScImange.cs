using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ILib.ScWidgets
{

	public class ScImange : ScGraphic
	{
		Sprite m_Sprite;
		public Sprite Sprite
		{
			get => m_Sprite;
			set
			{
				m_Sprite = value;
				SetDitry();
			}
		}

		public Image.Type Type { get; set; } = Image.Type.Simple;

	}
}