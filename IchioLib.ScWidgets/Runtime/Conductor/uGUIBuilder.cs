using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ILib.ScWidgets;

namespace ILib.ScWidgets
{

	public class uGUIBuilder : Conductor<uGUIBuilder, uGUIBuilder.Entry>
	{
		public class Entry : IEntryEventCallback
		{
			public GameObject Obj;
			public RectTransform ChildRoot;
			public Component Component;
			void IEntryEventCallback.OnRegister(IScWidget widget) { }
			void IEntryEventCallback.OnRemove()
			{
				GameObject.Destroy(Obj);
			}
		}

		public GameObject Build(IScWidget root)
		{
			SetContext(this);
			SetRootWidget(root);
			root.SetDitry();
			CalcLayout(new Rect(0, 0, Screen.width, Screen.height));
			Prepare(Root);
			Run(root);
			return GetEntry(root).Obj;
		}

		protected override void FallbackPrepare(uGUIBuilder context, IScWidget widget)
		{
			var parent = context.GetEntry(widget.Parent);
			var entry = context.GetEntry(widget);
			entry.Obj = new GameObject(widget.Name ?? widget.GetType().Name);
			entry.ChildRoot = entry.Obj.AddComponent<RectTransform>();
			entry.ChildRoot.anchorMax = new Vector2(0, 1);
			entry.ChildRoot.anchorMin = new Vector2(0, 1);
			if (parent != null && parent.ChildRoot != null)
			{
				entry.Obj.transform.SetParent(parent.ChildRoot);
			}
		}

		protected override void FallbackRun(uGUIBuilder context, IScWidget widget)
		{
			var entry = context.GetEntry(widget);
			var rect = widget.GetRelativeRect();
			entry.ChildRoot.anchoredPosition = (rect.position + rect.size * 0.5f) * new Vector2(1, -1);
			entry.ChildRoot.sizeDelta = rect.size;
		}

		public abstract class uGUIHandler<T, UComponent> : Handler<T> where T : IScWidget where UComponent : Component
		{
			protected UComponent Create(uGUIBuilder context, IScWidget widget)
			{
				var parent = context.GetEntry(widget.Parent);
				var entry = context.GetEntry(widget);
				entry.Obj = new GameObject(widget.Name ?? widget.GetType().Name);
				entry.ChildRoot = entry.Obj.AddComponent<RectTransform>();
				entry.ChildRoot.anchorMax = new Vector2(0, 1);
				entry.ChildRoot.anchorMin = new Vector2(0, 1);
				if (parent != null && parent.ChildRoot != null)
				{
					entry.Obj.transform.SetParent(parent.ChildRoot);
				}
				var ret = entry.Obj.AddComponent<UComponent>();
				entry.Component = ret;
				return ret;
			}

			protected override void Prepare(uGUIBuilder context, T widget)
			{
				Prepare(context, widget, Create(context, widget));
			}

			protected virtual void Prepare(uGUIBuilder context, T widget, UComponent component)
			{

			}

			protected UComponent GetComponent(uGUIBuilder context, IScWidget widget)
			{
				var entry = context.GetEntry(widget);
				if (entry.Obj == null) return default;
				return entry.Obj.GetComponent<UComponent>();
			}

			protected override void Run(uGUIBuilder context, T widget)
			{
				var entry = context.GetEntry(widget);
				var rect = widget.GetRelativeRect();
				entry.ChildRoot.anchoredPosition = (rect.position + rect.size * 0.5f) * new Vector2(1, -1);
				entry.ChildRoot.sizeDelta = rect.size;
				Run(context, widget, entry.Component as UComponent);
			}

			protected virtual void Run(uGUIBuilder context, T widget, UComponent component)
			{

			}
		}

		class CanvasHandler : uGUIHandler<ScWindow, Canvas>
		{
			protected override void Prepare(uGUIBuilder context, ScWindow widget, Canvas canvas)
			{
				canvas.renderMode = RenderMode.ScreenSpaceOverlay;
				if (widget.Parent == null)
				{
					var scaler = canvas.gameObject.AddComponent<CanvasScaler>();
					scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
					scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
				}
			}

			protected override void Run(uGUIBuilder context, ScWindow widget)
			{
				if (widget.Parent == null)
				{
					var entry = context.GetEntry(widget);
					var scaler = entry.Obj.GetComponent<CanvasScaler>();
					scaler.referenceResolution = widget.GetRect().size;
				}
				else
				{
					base.Run(context, widget);
				}
			}
		}

		class ImageHandler : uGUIHandler<ScImange, Image>
		{
			protected override void Run(uGUIBuilder context, ScImange widget, Image component)
			{
				component.sprite = widget.Sprite;
			}
		}

		class TextureHandler : uGUIHandler<ScTexture, RawImage>
		{
			protected override void Run(uGUIBuilder context, ScTexture widget, RawImage component)
			{
				component.texture = widget.Texture;
			}
		}

		class TextHandler : uGUIHandler<ScText, Text>
		{
			protected override void Run(uGUIBuilder context, ScText widget, Text component)
			{
				if (widget.Font != null)
				{
					component.font = widget.Font;
				}
				component.fontSize = widget.FontSize;
				component.text = widget.Text;
				component.alignment = widget.TextAnchor;
			}
		}

		class ButtonHandler : uGUIHandler<ScButton, Button>
		{
			protected override void Prepare(uGUIBuilder context, ScButton widget, Button component)
			{
				component.onClick.AddListener(widget.Invoke);
				component.gameObject.AddComponent<Image>().color = Color.blue;
			}
		}

		class ToggleHandler : uGUIHandler<ScToggle, Toggle>
		{
			protected override void Prepare(uGUIBuilder context, ScToggle widget, Toggle component)
			{
				component.onValueChanged.AddListener(widget.Set);
			}
		}

		class ClipMaskHandler : uGUIHandler<ScClipMask, RectMask2D>
		{
		}

		class ScrollViewHandler : uGUIHandler<ScScrollView, ScrollRect>
		{
		}


	}

}
