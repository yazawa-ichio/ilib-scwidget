#if DEBUG || ILIB_DEBUG_MENU
using System.Collections.Generic;
using UnityEngine;
using ILib.ScWidgets;
using System.Linq;

namespace ILib.Debugs.Widgets
{
	public class DebugRadioButtonItem : ScMinimumWidget , IFixedHeight
	{
		class Entry : ScMinimumWidget
		{
			DebugItemTapHandler m_TapHandler;

			ScRadioButton m_Button;

			public bool Value
			{
				get => m_Button.Value;
				set => m_Button.Value = value;
			}

			ScText m_Label;
			public string Label
			{
				get => m_Label.Text;
				set => m_Label.Text = value;
			}

			public System.Action<bool> OnChange { set => m_Button.OnChange = value; }

			public Entry(ScRadioButton.IRoot root)
			{
				m_Button = new ScRadioButton()
				{
					Layout = new Layout { Size = new Vector2(36, 36), Anchor = LayoutAnchor.Left, Pos = new Vector2(8, 0) },
					Root = root
				};
				root.Register(m_Button);
				m_Label = new ScText()
				{
					Layout = new StretchLayout { Margin = new RectOffset(48, 16, 0, 0) },
					TextAnchor = TextAnchor.MiddleLeft,
					TextClipping = TextClipping.Clip,
				};
				m_TapHandler = new DebugItemTapHandler();
				m_TapHandler.OnTap = m_Button.Invoke;
				m_Children.Add(m_TapHandler);
				m_Children.Add(m_Button);
				m_Children.Add(m_Label);
			}
		}

		ScRadioButton.IRoot m_Root;
		Entry[] m_Entries;
		ExtendHeight m_ExtendHeight;
		string[] m_Items = System.Array.Empty<string>();

		public string[] Items
		{
			set => SetItems(value);
		}

		public System.Action<int, string> OnChanged { set; private get; }

		public float Height
		{
			get
			{
				var scroll = Parent as ScScrollView;
				if (scroll != null)
				{
					return scroll.Height * m_Items.Length;
				}
				return 0;
			}
		}

		public DebugRadioButtonItem()
		{
			m_Root = new ScRadioButton.SimpleRoot();
		}

		public void SetItems(string[] items, int select = -1)
		{
			m_Items = items;
			List<Entry> entries = new List<Entry>();
			for (int i = 0; i < items.Length; i++)
			{
				var index = i;
				var item = items[i];
				entries.Add(new Entry(m_Root)
				{
					Name = item,
					Label = item,
					OnChange = (ret) => { if (ret) { Select(index); } }
				});
			}
			m_Entries = entries.ToArray();
			if (select >= 0)
			{
				m_Entries[select].Value = true;
			}
			m_Children.Clear();
			m_Children.AddRange(m_Entries);
		}

		void Select(int index)
		{
			OnChanged?.Invoke(index, m_Entries[index].Name);
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
			var height = m_Rect.height / m_Items.Length;
			var pos = m_Rect.position;
			var size = m_Rect.size;
			size.y = height;
			foreach (var entry in m_Entries)
			{
				entry.CalcLayout(new Rect(pos, size));
				pos.y += height;
			}
		}

	}

}
#endif
