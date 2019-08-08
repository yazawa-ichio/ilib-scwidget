using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILib.ScWidgets
{

	public class SkinIMGUIDrawer : Conductor<SkinIMGUIDrawer, SkinIMGUIDrawer.Entry>
	{
		public class Entry
		{
			public GUIStyle Style;
			public Dictionary<string, GUIStyle> Styles;
			public int ControlId;
			public bool DisableDragIfHotControl;
			public int Order;
		}

		public IMGUIWidgetSkin Skin { get; set; }

		public bool IsEditorGUI { get; set; }
		public float Scale { get; set; } = 1f;
		List<int> m_Order = new List<int>();

		public bool IsDisableDrag()
		{
			foreach (var entry in GetEntry())
			{
				if (entry.DisableDragIfHotControl && entry.ControlId != 0 && entry.ControlId == GUIUtility.hotControl)
				{
					return true;
				}
			}
			return false;
		}

		public SkinIMGUIDrawer(IScWidget root, IMGUIWidgetSkin skin)
		{
			Skin = skin;
			SetContext(this);
			SetRootWidget(root);
		}

		public SkinIMGUIDrawer(IMGUIWidgetSkin skin)
		{
			Skin = skin;
			SetContext(this);
		}

		public void DrawScreen(Vector2 target)
		{
			Draw(target, new Vector2(Screen.width, Screen.height));
		}

		public void Draw(Vector2 target, Vector2 screen)
		{
			var scale = Mathf.Min(screen.x / target.x, screen.y / target.y);
			var width = (screen.x / scale - target.x) / 2f;
			var height = (screen.y / scale - target.y) / 2f;
			Draw(new Rect((int)width, (int)height, target.x, target.y), scale);
		}

		Rect m_PrevRect;
		public void Draw(Rect rect, float scale = 1f)
		{
			if (rect != m_PrevRect)
			{
				Root.SetDitry();
				m_PrevRect = rect;
			}
			if (scale != 1f)
			{
				GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), Vector2.zero);
			}
			var e = Event.current;
			if (e.type == EventType.Layout)
			{
				CalcLayout(rect);
				Prepare(Root);
			}

			var color = GUI.color;
			var backgroundColor = GUI.backgroundColor;
			var contentColor = GUI.contentColor;
			try
			{
				Run(Root);
			}
			finally
			{
				GUI.color = color;
				GUI.backgroundColor = backgroundColor;
				GUI.contentColor = contentColor;
			}
			if (scale != 1f)
			{
				GUIUtility.ScaleAroundPivot(new Vector2(1f, 1f), Vector2.zero);
			}
			InvokeAction();
		}

		public abstract class IMGUIHandler<T> : Handler<T> where T : IScWidget
		{

			protected GUIStyle GetStyle(SkinIMGUIDrawer context, IScWidget widget, string def)
			{
				var entry = context.GetEntry(widget);
				if (entry.Style == null)
				{
					entry.Style = new GUIStyle(def);
				}
				return entry.Style;
			}

			protected GUIStyle GetStyle(SkinIMGUIDrawer context, IScWidget widget)
			{
				var entry = context.GetEntry(widget);
				if (entry.Style == null)
				{
					entry.Style = CreateStyle(context, widget);
				}
				return entry.Style;
			}

			protected GUIStyle GetStyle(string key, SkinIMGUIDrawer context, IScWidget widget)
			{
				var entry = context.GetEntry(widget);
				if (entry.Styles == null)
				{
					entry.Styles = new Dictionary<string, GUIStyle>();
				}
				GUIStyle style;
				if (!entry.Styles.TryGetValue(key, out style))
				{
					entry.Styles[key] = style = CreateStyle(key, context, widget);
				}
				return style;
			}

			protected GUIStyle GetStyle(string key, SkinIMGUIDrawer context, IScWidget widget, string def)
			{
				var entry = context.GetEntry(widget);
				if (entry.Styles == null)
				{
					entry.Styles = new Dictionary<string, GUIStyle>();
				}
				GUIStyle style;
				if (!entry.Styles.TryGetValue(key, out style))
				{
					entry.Styles[key] = style = new GUIStyle(def);
				}
				return style;
			}


			protected virtual GUIStyle CreateStyle(SkinIMGUIDrawer context, IScWidget widget)
			{
				return new GUIStyle();
			}

			protected virtual GUIStyle CreateStyle(string key, SkinIMGUIDrawer context, IScWidget widget)
			{
				var style = new GUIStyle();
				return style;
			}

			static readonly RectOffset s_ZeroOffset = new RectOffset(0, 0, 0, 0);

			protected float DrawProgress(SkinIMGUIDrawer context, ScWidget widget, float val, float min, float max)
			{
				var sliderBase = GetStyle("SliderBase", context, widget, "HorizontalSlider");
				var sliderActive = GetStyle("SliderActive", context, widget, "HorizontalSlider");
				var thumb = GetStyle("Thumb", context, widget, "HorizontalSliderThumb");
				var rect = widget.GetRect();
				var baseSprite = context.Skin.Box;
				var height = Mathf.Clamp(rect.height, baseSprite.texture.height * 1.2f, baseSprite.texture.height * 2f);
				sliderBase.normal.background = baseSprite.texture;
				sliderBase.active.background = baseSprite.texture;
				sliderBase.fixedHeight = height;
				sliderBase.border.Set(baseSprite.border.x, baseSprite.border.y);

				sliderBase.padding.Reset();
				sliderBase.stretchHeight = false;
				sliderActive.margin.Reset();
				sliderActive.normal.background = baseSprite.texture;
				sliderActive.active.background = baseSprite.texture;

				sliderActive.border.Set(baseSprite.border.x, baseSprite.border.y);

				sliderActive.fixedHeight = height - 4;
				sliderActive.stretchHeight = false;
				sliderActive.margin.Set(top: 2, bottom: 2);
				sliderActive.padding = s_ZeroOffset;

				thumb.normal.background = context.Skin.Box.texture;
				thumb.hover.background = context.Skin.Box.texture;
				thumb.active.background = context.Skin.Box.texture;
				if (context.IsEditorGUI)
				{
					thumb.fixedWidth = height - 8f;
					thumb.fixedHeight = height - 8f;
				}
				else
				{
					thumb.fixedWidth = height - 2f;
					thumb.fixedHeight = height - 2f;
				}

				var bgColor = GUI.backgroundColor;
				try
				{
					var sliderRect = new Rect(rect);
					sliderRect.x += 2;
					sliderRect.width -= 4;
					GUI.backgroundColor = context.Skin.Color.Disable;
					GUI.Label(sliderRect, "", sliderBase);

					sliderRect.y += 2;
					sliderRect.x += 2;
					sliderRect.width -= 4;
					sliderRect.width = Mathf.Max(thumb.fixedWidth - 4, (sliderRect.width * val - min) / max);
					GUI.backgroundColor = context.Skin.Color.Active;
					GUI.Label(sliderRect, "", sliderActive);

					GUI.backgroundColor = context.Skin.Color.Base;
					var entry = context.GetEntry(widget);
					entry.ControlId = GUIUtility.GetControlID(FocusType.Passive) + 1;
					entry.DisableDragIfHotControl = true;
					return GUI.HorizontalSlider(rect, val, min, max, GetStyle("Empty", context, widget), thumb);
				}
				finally
				{
					GUI.backgroundColor = bgColor;
				}
			}
		}

		class ImageHandler : IMGUIHandler<ScImange>
		{
			protected override void Run(SkinIMGUIDrawer context, ScImange widget)
			{
				if (widget.IsOutsideClip()) return;
				if (widget.Sprite != null)
				{
					GUI.DrawTextureWithTexCoords(widget.GetRect(), widget.Sprite.texture, widget.Sprite.rect);
				}
			}
		}

		class TextureHandler : IMGUIHandler<ScTexture>
		{
			protected override void Run(SkinIMGUIDrawer context, ScTexture widget)
			{
				if (widget.IsOutsideClip()) return;
				GUI.DrawTexture(widget.GetRect(), widget.Texture, ScaleMode.ScaleAndCrop, true, 0, widget.GetColor(), 0, 0);
			}
		}

		class TextHandler : IMGUIHandler<ScText>
		{
			protected override void Run(SkinIMGUIDrawer context, ScText widget)
			{
				if (widget.IsOutsideClip()) return;
				var style = GetStyle(context, widget, "Label");
				style.alignment = widget.TextAnchor;
				if (style.font != widget.Font)
				{
					style.font = widget.Font;
				}
				style.fontSize = widget.FontSize;
				style.fontStyle = widget.FontStyle;
				var color = GUI.contentColor;
				try
				{
					GUI.contentColor = context.Skin.Color.Text * widget.GetMaskColor();
					GUI.Label(widget.GetRect(), widget.Text, style);
				}
				finally
				{
					GUI.contentColor = color;
				}
			}
		}

		class ButtonHandler : IMGUIHandler<ScButton>
		{
			protected override GUIStyle CreateStyle(SkinIMGUIDrawer context, IScWidget widget)
			{
				GUIStyle style = new GUIStyle("Button");
				style.normal.background = context.Skin.Button.texture;
				style.active.background = context.Skin.Button.texture;
				style.hover.background = context.Skin.Button.texture;
				style.onNormal.background = context.Skin.Button.texture;
				style.onActive.background = context.Skin.Button.texture;
				style.onHover.background = context.Skin.Button.texture;
				var border = context.Skin.Button.border;
				style.border.Set((int)border.x, (int)border.y, (int)border.z, (int)border.w);
				return style;
			}

			protected override void Run(SkinIMGUIDrawer context, ScButton widget)
			{
				if (widget.IsOutsideClip()) return;
				var style = GetStyle(context, widget);
				var tmpColor = GUI.backgroundColor;
				try
				{
					GUI.backgroundColor = context.Skin.Color.Main;
					if (GUI.Button(widget.GetRect(), "", style))
					{
						context.Schedule(() =>
						{
							widget.Invoke();
						});
						GUI.FocusControl("");
					}
				}
				finally
				{
					GUI.backgroundColor = tmpColor;
				}
			}
		}

		class ToggleHandler : IMGUIHandler<ScToggle>
		{
			protected override GUIStyle CreateStyle(SkinIMGUIDrawer context, IScWidget widget)
			{
				var style = new GUIStyle();
				style.alignment = TextAnchor.MiddleCenter;
				style.padding.Set(2);
				style.normal.textColor = new Color(1f, 1f, 1f, 1f);
				style.active.textColor = new Color(1f, 1f, 1f, 1f);
				style.hover.textColor = new Color(1f, 1f, 1f, 1f);
				style.onNormal.textColor = new Color(1f, 1f, 1f, 1f);
				style.onActive.textColor = new Color(1f, 1f, 1f, 1f);
				style.onHover.textColor = new Color(1f, 1f, 1f, 1f);
				style.normal.background = context.Skin.Circle.texture;
				style.active.background = context.Skin.Circle.texture;
				style.onNormal.background = context.Skin.Circle.texture;
				style.onActive.background = context.Skin.Circle.texture;
				style.onHover.background = context.Skin.Circle.texture;
				style.border.Reset();
				return style;
			}

			void RunSwitchStyle(GUIStyle style, SkinIMGUIDrawer context, ScToggle widget)
			{
				var rect = widget.GetRect();
				style.fontSize = (int)(rect.height / 2.5f);
				var border = context.Skin.Circle.border;
				style.border.left = (int)border.x;
				style.border.right = (int)border.y;

				var tmpColor = GUI.backgroundColor;
				var tmpContentColor = GUI.contentColor;
				try
				{
					GUI.backgroundColor = widget.Value ? context.Skin.Color.Active : context.Skin.Color.Disable;
					GUI.contentColor = widget.Value ? context.Skin.Color.TextOnActive : context.Skin.Color.TextOnDisable;
					style.alignment = widget.Value ? TextAnchor.MiddleLeft : TextAnchor.MiddleRight;
					style.padding.left = 2 + (int)(rect.height / 4f);
					style.padding.right = (int)(rect.height / 4f);
					string text = widget.Value ? "ON" : "OFF";
					var ret = GUI.Toggle(widget.GetRect(), widget.Value, text, style);
					if (ret != widget.Value)
					{
						context.Schedule(() =>
						{
							widget.Set(ret);
						});
						GUI.FocusControl("");
					}
				}
				finally
				{
					GUI.backgroundColor = tmpColor;
					GUI.contentColor = tmpContentColor;
				}

				var leftPos = rect.position + new Vector2(2f, 2f);
				var height = rect.height - 4;
				var rightPos = new Vector2(rect.xMax - height - 2, leftPos.y);
				var size = new Vector2(height, height);
				if (widget.Value)
				{
					GUI.DrawTexture(new Rect(rightPos, size), context.Skin.Circle.texture);
				}
				else
				{
					GUI.DrawTexture(new Rect(leftPos, size), context.Skin.Circle.texture);
				}
			}

			void RunCircleStyle(GUIStyle style, SkinIMGUIDrawer context, ScToggle widget)
			{
				var rect = widget.GetRect();
				style.normal.background = context.Skin.Circle.texture;
				style.active.background = context.Skin.Circle.texture;
				style.onNormal.background = context.Skin.Circle.texture;
				style.onActive.background = context.Skin.Circle.texture;
				style.onHover.background = context.Skin.Circle.texture;
				style.fontSize = (int)(rect.height / 3f);
				style.padding.left = 2;
				style.padding.right = 0;
				var tmpColor = GUI.backgroundColor;
				var tmpContentColor = GUI.contentColor;
				try
				{
					GUI.backgroundColor = widget.Value ? context.Skin.Color.Active : context.Skin.Color.Disable;
					GUI.contentColor = widget.Value ? context.Skin.Color.TextOnActive : context.Skin.Color.TextOnDisable;
					string text = widget.Value ? "ON" : "OFF";
					var ret = GUI.Toggle(widget.GetRect(), widget.Value, text, style);
					if (ret != widget.Value)
					{
						context.Schedule(() =>
						{
							widget.Set(ret);
						});
						GUI.FocusControl("");
					}
				}
				finally
				{
					GUI.backgroundColor = tmpColor;
					GUI.contentColor = tmpContentColor;
				}
			}

			protected override void Run(SkinIMGUIDrawer context, ScToggle widget)
			{
				if (widget.IsOutsideClip()) return;
				var style = GetStyle(context, widget);
				var rect = widget.GetRect();
				style.fixedWidth = rect.width;
				style.fixedHeight = rect.height;
				if (rect.width > rect.height * 1.9f)
				{
					RunSwitchStyle(style, context, widget);
				}
				else
				{
					RunCircleStyle(style, context, widget);
				}
			}
		}

		class CheckBoxHandler : IMGUIHandler<ScCheckBox>
		{
			protected override GUIStyle CreateStyle(SkinIMGUIDrawer context, IScWidget widget)
			{
				GUIStyle style = new GUIStyle("Button");
				style.normal.background = context.Skin.Button.texture;
				style.active.background = context.Skin.Button.texture;
				style.hover.background = context.Skin.Button.texture;
				style.onNormal.background = context.Skin.Button.texture;
				style.onActive.background = context.Skin.Button.texture;
				style.onHover.background = context.Skin.Button.texture;
				var border = context.Skin.Button.border;
				style.border = new RectOffset((int)border.x, (int)border.y, (int)border.z, (int)border.w);
				return style;
			}

			protected override void Run(SkinIMGUIDrawer context, ScCheckBox widget)
			{
				if (widget.IsOutsideClip()) return;
				var style = GetStyle(context, widget);
				var rect = widget.GetRect();
				style.fixedWidth = rect.width;
				style.fixedHeight = rect.height;

				var bgColor = GUI.backgroundColor;
				try
				{
					if (widget.Value)
					{
						GUI.backgroundColor = context.Skin.Color.Active;
					}
					else
					{
						GUI.backgroundColor = context.Skin.Color.Disable;
					}
					var ret = GUI.Toggle(rect, widget.Value, "", style);
					if (ret != widget.Value)
					{
						context.Schedule(() => {
							widget.Value = ret;
						});
						GUI.FocusControl("");
					}
				}
				finally
				{
					GUI.backgroundColor = bgColor;
				}

				if (Event.current.type == EventType.Repaint)
				{
					rect.position += new Vector2(2f, 2f);
					rect.size -= new Vector2(4f, 4f);
					style.fixedWidth = rect.width;
					style.fixedHeight = rect.height;
					style.Draw(rect, context.Skin.Check.texture,false,false,false,false);

					var color = GUI.backgroundColor;
					try
					{
						if (widget.Value)
						{
							GUI.backgroundColor = context.Skin.Color.Active;
						}
						else
						{
							GUI.backgroundColor = context.Skin.Color.Disable;
						}
						var label = GetStyle("Label", context, widget, "Label");
						label.fixedWidth = rect.width;
						label.fixedHeight = rect.height;
						label.stretchHeight = true;
						label.stretchWidth = true;
						label.normal.background = context.Skin.Check.texture;
						label.Draw(rect, false, false, false, false);
					}
					finally
					{
						GUI.backgroundColor = bgColor;
					}

				}

			}
		}

		class RadioButtonHandler : IMGUIHandler<ScRadioButton>
		{
			protected override GUIStyle CreateStyle(string key, SkinIMGUIDrawer context, IScWidget widget)
			{
				if (key == "Dummy") return new GUIStyle();

				var style = new GUIStyle();
				style.alignment = TextAnchor.MiddleCenter;
				style.padding = new RectOffset(2, 0, 0, 0);
				style.normal.background = context.Skin.Circle.texture;
				style.active.background = context.Skin.Circle.texture;
				style.hover.background = context.Skin.Circle.texture;

				style.onNormal.background = context.Skin.Circle.texture;
				style.onActive.background = context.Skin.Circle.texture;
				style.onHover.background = context.Skin.Circle.texture;

				return style;
			}

			protected override void Run(SkinIMGUIDrawer context, ScRadioButton widget)
			{
				if (widget.IsOutsideClip()) return;
				var dummtyStyle = GetStyle("Dummy", context, widget);
				var buttonStyle = GetStyle("Button", context, widget);
				var rect = widget.GetRect();
				var size = Vector2.Max(new Vector2(16, 16), Vector2.Min(rect.size, context.Skin.Circle.rect.size));
				if (size.x > size.y)
				{
					size.x = size.y;
				}
				else
				{
					size.y = size.x;
				}

				var bgColor = GUI.backgroundColor;
				try
				{
					GUI.backgroundColor = context.Skin.Color.Line;
					GUI.Label(new Rect(rect.position, size), "", buttonStyle);
					GUI.backgroundColor = context.Skin.Color.Base;
					GUI.Label(new Rect(rect.position + new Vector2(1f, 1f), size - new Vector2(2f, 2f)), "", buttonStyle);
					if (widget.Value)
					{
						GUI.backgroundColor = context.Skin.Color.Active;
						GUI.Label(new Rect(rect.position + size / 4f, size - size / 2f), "", buttonStyle);
					}
				}
				finally
				{
					GUI.backgroundColor = bgColor;
				}
				var ret = GUI.Toggle(widget.GetRect(), widget.Value, "", dummtyStyle);
				if (ret && ret != widget.Value)
				{
					context.Schedule(() =>
					{
						widget.Select();
					});
					GUI.FocusControl("");
				}
			}
		}

		class ClipMaskHandler : IMGUIHandler<ScClipMask>
		{
			protected override void Run(SkinIMGUIDrawer context, ScClipMask widget)
			{
				var rect = widget.GetRect();
				GUI.BeginClip(rect);
			}

			protected override void Finish(SkinIMGUIDrawer context, ScClipMask widget)
			{
				GUI.EndClip();
			}
		}

		class ScrollViewHandler : IMGUIHandler<ScScrollView>
		{

			void DrawScrollBar(SkinIMGUIDrawer context, ScScrollView widget, bool horizontal)
			{
				var styleName = horizontal ? "HorizontalScrollbar" : "VerticalScrollbar";
				var style = GetStyle(styleName, context, widget, styleName);
				var border = context.Skin.Box.border;
				style.normal.background = context.Skin.Box.texture;

				var rect = widget.GetRect();
				var pos = horizontal ? widget.ScrollViewPostion.x : widget.ScrollViewPostion.y;
				var contentsRect = widget.GetContentsRect();

				var scroll = new Rect(rect);

				float maxPos = 0;
				float tabSize = 0;
				float scrollSize = 0;
				if (horizontal)
				{
					scroll.xMax -= 10;
					scroll.xMin += 4;
					maxPos = contentsRect.width - rect.width;
					style.fixedHeight = 8;
					style.border.Set(9, 9);
					scroll.yMin = scroll.yMax - 8;
					tabSize = Mathf.Clamp(scroll.width * (rect.width / contentsRect.width), 20f, rect.width - 20f);
					scrollSize = scroll.width;
				}
				else
				{
					scroll.yMax -= 10;
					scroll.yMin += 4;
					maxPos = contentsRect.height - rect.height;
					style.fixedWidth = 8;
					scroll.xMin = scroll.xMax - 8;
					tabSize = Mathf.Clamp(scroll.height * (rect.height / contentsRect.height), 20f, rect.height - 20f);
					scrollSize = scroll.height;
				}

				if (Event.current.type == EventType.Repaint)
				{
					style.Draw(scroll, false, false, false, false);

					var rate = pos / maxPos;
					var tabPos = (rate * (scrollSize - tabSize - 4)) + tabSize / 2f + 2f;
					if (horizontal)
					{
						scroll.xMin = rect.x + tabPos - tabSize / 2f + 4;
						scroll.xMax = rect.x + tabPos + tabSize / 2f + 4;
						scroll.yMin += 1;
						style.fixedHeight = 6;
					}
					else
					{
						scroll.yMin = rect.y + tabPos - tabSize / 2f + 4;
						scroll.yMax = rect.y + tabPos + tabSize / 2f + 4;
						scroll.xMin += 1;
						style.fixedWidth = 6;
					}
					var bgColor = GUI.backgroundColor;
					try
					{
						GUI.backgroundColor = context.Skin.Color.Active;
						style.Draw(scroll, false, false, false, false);
					}
					finally
					{
						GUI.backgroundColor = bgColor;
					}
				}

			}


			protected override void Run(SkinIMGUIDrawer context, ScScrollView widget)
			{
				var rect = widget.GetRect();
				var pos = widget.ScrollViewPostion;
				var contentsRect = widget.GetContentsRect();
				var e = Event.current;
				if (rect.Contains(e.mousePosition))
				{
					if (e.type == EventType.ScrollWheel)
					{
						pos += e.delta;
						e.Use();
					}
					if (!context.IsDisableDrag() && e.type == EventType.MouseDrag)
					{
#if UNITY_STANDALONE || UNITY_EDITOR
						pos += e.delta * new Vector2(-1f, -1f);
#else
						pos += e.delta * new Vector2(-1f, 1f);
#endif
						if (contentsRect.width <= rect.width) pos.x = 0;
						if (contentsRect.height <= rect.height) pos.y = 0;
						if (pos != Vector2.zero)
						{
							e.Use();
							GUI.FocusControl("");
							GUIUtility.hotControl = 0;
						}
					}
				}
				widget.ScrollViewPostion = pos;
				GUI.BeginClip(rect);
				GUI.BeginGroup(new Rect(widget.ScrollViewPostion * new Vector2(-1f, -1f) - contentsRect.position, contentsRect.size + contentsRect.position));
			}

			protected override void Finish(SkinIMGUIDrawer context, ScScrollView widget)
			{
				GUI.EndGroup();
				GUI.EndClip();

				var rect = widget.GetRect();
				var pos = widget.ScrollViewPostion;
				var contentsRect = widget.GetContentsRect();

				var yMin = 0;
				var yMax = contentsRect.yMax - contentsRect.position.y - rect.height;
				var xMax = contentsRect.xMax - contentsRect.position.x - rect.width;
				var xMin = 0;
				if (contentsRect.width <= rect.width)
				{
					pos.x = 0;
				}
				else
				{
					if(!widget.HideScrollBar) DrawScrollBar(context, widget, true);
					if (Event.current.type == EventType.Layout)
					{
						pos.x = Mathf.Clamp(pos.x, xMin, xMax);
					}
				}

				if (contentsRect.height <= rect.height)
				{
					pos.y = 0;
				}
				else
				{
					if (!widget.HideScrollBar) DrawScrollBar(context, widget, false);
					if (Event.current.type == EventType.Layout)
					{
						pos.y = Mathf.Clamp(pos.y, yMin, yMax);
					}
				}
				widget.ScrollViewPostion = pos;
			}
		}

		class SliderHandler : IMGUIHandler<ScSlider>
		{
			protected override void Run(SkinIMGUIDrawer context, ScSlider widget)
			{
				if (widget.IsOutsideClip()) return;
				var val = DrawProgress(context, widget, widget.Value, widget.MinValue, widget.MaxValue);
				if (val != widget.Value)
				{
					widget.Set(val);
					GUI.FocusControl("");
				}
			}
		}

		class ProgressBarHandler : IMGUIHandler<ScProgressBar>
		{
			protected override void Run(SkinIMGUIDrawer context, ScProgressBar widget)
			{
				if (widget.IsOutsideClip()) return;
				var rect = widget.GetRect();
				DrawProgress(context,widget, widget.Value, 0, 1f);
			}
		}

		class PageViewHandler : IMGUIHandler<ScPageView>
		{
			protected override void Run(SkinIMGUIDrawer context, ScPageView widget)
			{
				if (widget.IsOutsideClip()) return;
				var rect = widget.GetRect();
				GUI.BeginGroup(widget.GetRect());
			}

			protected override void Finish(SkinIMGUIDrawer context, ScPageView widget)
			{
				if (widget.IsOutsideClip()) return;
				GUI.EndGroup();
			}
		}

		class InputFieldHandler : IMGUIHandler<ScInputField>
		{
			static readonly RectOffset s_Border = new RectOffset(9, 9, 9, 9);
			protected override void Run(SkinIMGUIDrawer context, ScInputField widget)
			{
				if (widget.IsOutsideClip()) return;
				var entry = context.GetEntry(widget);
				var rect = widget.GetRect();

				var style = GetStyle(context, widget, "TextField");
				style.normal.background = context.Skin.Box.texture;
				style.normal.textColor = new Color(0.4f, 0.4f, 0.4f);
				style.active.background = context.Skin.Box.texture;
				style.active.textColor = new Color(0, 0, 0);
				style.hover.background = context.Skin.Box.texture;
				style.hover.textColor = new Color(0, 0, 0);
				style.focused.background = context.Skin.Box.texture;
				style.focused.textColor = new Color(0, 0, 0);
				style.border = s_Border;
				style.fontSize = widget.FontSize;
				style.alignment = widget.TextAnchor;

				if (Event.current.type == EventType.Repaint)
				{
					var bgColor = GUI.backgroundColor;
					try
					{
						if (entry.ControlId != 0 && entry.ControlId + 1 == GUIUtility.keyboardControl)
						{
							GUI.backgroundColor = context.Skin.Color.Active;
						}
						else
						{
							GUI.backgroundColor = context.Skin.Color.Disable;
						}
						style.Draw(rect, false, false, false, false);
					}
					finally
					{
						GUI.backgroundColor = bgColor;
					}
				}

				rect.position += new Vector2(2f, 2f);
				rect.size -= new Vector2(4f, 4f);

				{
					var bgColor = GUI.backgroundColor;
					var cursorColor = GUI.skin.settings.cursorColor;
					try
					{
						GUI.skin.settings.cursorColor = new Color(0, 0, 0);
						GUI.backgroundColor = context.Skin.Color.Base;
						entry.ControlId = GUIUtility.GetControlID(FocusType.Keyboard);
						var input = GUI.TextField(rect, widget.Input, style);
						if (input != widget.Input)
						{
							context.Schedule(() =>
							{
								widget.Set(input);
							});
						}
					}
					finally
					{
						GUI.backgroundColor = bgColor;
						GUI.skin.settings.cursorColor = cursorColor;
					}
				}
			}

		}

	}

}
