using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ILib.ScWidgets
{
	//[CreateAssetMenu(menuName = "ILib/Create ScWidgetSkin")]
	public class IMGUIWidgetSkin : ScriptableObject, ISerializationCallbackReceiver
	{
		[System.Serializable]
		class CustomEntry
		{
			public string Name = "";
			public Object Object = null;
		}

		[System.Serializable]
		public class ContentsColor
		{
			public Color Base = new Color(1f, 1f, 1f, 1f);
			public Color Main = new Color(1f, 0.776f, 0.4f, 1f);
			public Color Active = new Color(1f, 0.776f, 0.4f, 1f);
			public Color Disable = new Color(1f, 0.92f, 0.80f, 0.9f);
			public Color Line = new Color(0.839f, 0.56f, 0.368f, 0.9f);
			public Color Text = new Color(0f, 0f, 0f, 0.85f);

			public Color TextOnActive = new Color(1, 1, 1, 0.85f);
			public Color TextOnDisable = new Color(0.4f, 0.4f, 0.4f, 0.5f);

		}

		public Font Font;
		public Sprite Box;
		public Sprite Button;
		public Sprite Circle;
		public Sprite Check;
		public Sprite Line;
		public Sprite Arrow;
		public ContentsColor Color;

		Texture2D m_WhiteTexture = null;
		public Texture2D WhiteTexture => m_WhiteTexture ?? (m_WhiteTexture = Texture2D.whiteTexture);

		[SerializeField]
		CustomEntry[] m_CustomEntiries = null;

		Dictionary<string, CustomEntry> m_Dic;

		public Object GetCustomSkin(string name)
		{
			if (m_Dic == null) return null;
			CustomEntry entry;
			if (m_Dic.TryGetValue(name, out entry))
			{
				return entry.Object;
			}
			return null;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (m_CustomEntiries != null)
			{
				m_Dic = m_CustomEntiries.ToDictionary(x => x.Name);
			}
		}
	}

}