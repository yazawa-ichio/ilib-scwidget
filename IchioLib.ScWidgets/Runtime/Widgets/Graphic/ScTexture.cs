using UnityEngine;

namespace ILib.ScWidgets
{
	public class ScTexture : ScGraphic
	{
		Texture m_Texture;
		public Texture Texture
		{
			get => m_Texture;
			set 
			{
				m_Texture = value;
				SetDitry();
			}
		}
	}
}
