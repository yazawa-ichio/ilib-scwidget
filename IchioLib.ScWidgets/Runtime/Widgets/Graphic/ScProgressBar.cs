using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Direction = UnityEngine.UI.Slider.Direction;

namespace ILib.ScWidgets
{
	public class ScProgressBar : ScGraphic
	{

		float m_Value = 1f;
		public float Value
		{
			get => m_Value;
			set
			{
				m_Value = value;
				SetDitry();
			}
		}

		public Direction Direction { get; set; } = Direction.LeftToRight;
		public Vector2 Size { set; get; } = new Vector2(16, 16);
		public bool SyncSpriteSize { get; set; }

		ScImange CreateImageWidget(Sprite sprite)
		{
			var w = new ScImange();
			w.Sprite = sprite;
			if (sprite.border != Vector4.zero)
			{
				w.Type = UnityEngine.UI.Image.Type.Sliced;
			}
			return w;
		}

		public Sprite Knob
		{
			set
			{
				m_KnobWidget = CreateImageWidget(value);
				if (SyncSpriteSize)
				{
					Size = value.rect.size;
				}
			}
		}

		IScWidget m_KnobWidget;
		public IScWidget KnobWidget
		{
			set
			{
				m_Children.Remove(m_KnobWidget);
				m_Children.Add(value);
				m_KnobWidget = value;
				value.SetParent(this);
				SetDitry();
			}
		}

		public Sprite Fill
		{
			set => FillWidget = CreateImageWidget(value);
		}

		IScWidget m_FillWidget;
		public IScWidget FillWidget
		{
			set
			{
				m_Children.Remove(m_FillWidget);
				m_Children.Add(value);
				m_FillWidget = value;
				value.SetParent(this);
				SetDitry();
			}
		}

		public Sprite Bg
		{
			set => BgWidget = CreateImageWidget(value);
		}

		IScWidget m_BgWidget;
		public IScWidget BgWidget
		{
			set
			{
				m_Children.Remove(m_BgWidget);
				m_Children.Add(value);
				m_BgWidget = value;
				value.SetParent(this);
				SetDitry();
			}
		}

		Rect GetFillRect()
		{
			var rect = m_Rect;
			var val = Mathf.Clamp01(Value);
			switch (Direction)
			{
				case Direction.LeftToRight:
					return new Rect(rect.xMin, rect.yMin, rect.width * val, rect.height);
				case Direction.RightToLeft:
					return new Rect(rect.xMin + rect.width * (1f - val), rect.yMin, rect.width * val, rect.height);
				case Direction.TopToBottom:
					return new Rect(rect.xMin, rect.yMin + rect.height * val, rect.width, rect.height * val);
				case Direction.BottomToTop:
					return new Rect(rect.xMin, rect.yMin + rect.height * (1f - val), rect.width, rect.height * val);
			}
			return m_Rect;
		}

		Rect GetKnobRect()
		{
			var rect = m_Rect;
			var val = Mathf.Clamp01(Value);
			var size = Size;
			var pos = rect.center;
			switch (Direction)
			{
				case Direction.LeftToRight:
					pos.x = rect.xMin + rect.width * (1f - val);
					break;
				case Direction.RightToLeft:
					pos.x = rect.xMax - rect.width;
					break;
				case Direction.TopToBottom:
					pos.y = rect.yMin + rect.height * (1f - val);
					break;
				case Direction.BottomToTop:
					pos.y = rect.yMax - rect.height;
					break;
			}
			return new Rect(pos, size);
		}

		public override void CalcLayout(Rect rect)
		{
			m_ParentRect = rect;
			if (IsDirty)
			{
				IsDirty = false;
				if (Layout == null) Layout = new StretchLayout();
				m_Rect = Layout.CalcRect(rect);
			}

			m_BgWidget?.CalcLayout(m_Rect);
			m_FillWidget?.CalcLayout(GetFillRect());
			m_KnobWidget?.CalcLayout(GetKnobRect());

			if (m_Children == null)
			{
				return;
			}
			foreach (var child in m_Children)
			{
				child.CalcLayout(m_Rect);
			}
		}

	}
}