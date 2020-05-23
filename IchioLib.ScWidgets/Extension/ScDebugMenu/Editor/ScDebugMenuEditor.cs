using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ILib.ScWidgets;

namespace ILib.Debugs
{
	[CustomEditor(typeof(ScDebugMenu))]
	public class ScDebugMenuEditor : Editor
	{
		ScDebugMenu m_Menu;
		private void OnEnable()
		{
			m_Menu = target as ScDebugMenu;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			if (m_Menu.Skin == null)
			{
				if (GUILayout.Button("スキンを新規作成"))
				{
					CreateSkinAsset();
				}
			}
		}

		void CreateSkinAsset()
		{
			var savePath = EditorUtility.SaveFilePanelInProject("スキンを新規作成", "IMGUIWidgetSkin", "asset", "新しくデバッグメニュー用のスキンを作成します");
			if (!string.IsNullOrEmpty(savePath))
			{
				var skin = ScriptableObject.CreateInstance<IMGUIWidgetSkin>();
				AssetDatabase.CreateAsset(skin, savePath);
				m_Menu.Skin = skin;
				EditorUtility.SetDirty(m_Menu);
			}
		}
	}

}