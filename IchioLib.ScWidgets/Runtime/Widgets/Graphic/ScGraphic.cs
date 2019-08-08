using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{
	public interface IScGraphic
	{
		Color Color { get; set; }
		Color MaskColor { get; set; }
		Color GetColor();
	}

	public abstract class ScGraphic : ScWidget , IScGraphic
	{
		public Color Color { get; set; } = new Color(1f, 1f, 1f, 1f);
		public Color MaskColor { get; set; } = new Color(1f, 1f, 1f, 1f);

		public virtual Color GetColor()
		{
			return GetMaskColor() * Color;
		}

		public override Color GetMaskColor()
		{
			return base.GetMaskColor() * MaskColor;
		}

	}
}
