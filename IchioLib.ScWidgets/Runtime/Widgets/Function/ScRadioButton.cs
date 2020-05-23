using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ILib.ScWidgets
{

	public class ScRadioButton : ScFunction
	{
		public interface IRoot
		{
			void Register(ScRadioButton button);
			void Select(ScRadioButton button);
			bool IsSelect(ScRadioButton button);
		}

		public class SimpleRoot : IRoot
		{
			List<ScRadioButton> m_Buttons = new List<ScRadioButton>();
			ScRadioButton m_Select;
			Dictionary<string, ScRadioButton> m_GroupSelect;

			public System.Action<ScRadioButton> OnSelect { set; private get; }

			public void Register(ScRadioButton button) => m_Buttons.Add(button);

			public void Select(ScRadioButton button)
			{
				if (string.IsNullOrEmpty(button.Group))
				{
					m_Select = button;
				}
				else
				{
					if (m_GroupSelect == null) m_GroupSelect = new Dictionary<string, ScRadioButton>();
					m_GroupSelect[button.Group] = button;
				}
				foreach (var b in m_Buttons)
				{
					if (button.Group == b.Group)
					{
						b.ChangeEvent(b == button);
					}
				}
				OnSelect?.Invoke(button);
			}

			public bool IsSelect(ScRadioButton button)
			{
				var group = button.Group;
				if (string.IsNullOrEmpty(group))
				{
					return m_Select == button;
				}
				else if (m_GroupSelect != null && m_GroupSelect.ContainsKey(group))
				{
					return m_GroupSelect[group] == button;
				}
				return false;
			}
		}

		public bool Value
		{
			get
			{
				PrepareRoot();
				return Root.IsSelect(this);
			}
			set
			{
				if (value)
				{
					PrepareRoot();
					Root.Select(this);
				}
			}
		}

		public string Group { get; set; }

		public System.Action<bool> OnChange { set; private get; }

		public IRoot Root { get; set; }

		public void Invoke()
		{
			if (Interactable)
			{
				Select();
			}
		}

		public void Select()
		{
			PrepareRoot();
			Root.Select(this);
			SetDitry();
		}

		void ChangeEvent(bool select)
		{
			OnChange?.Invoke(select);
		}

		public void Clear()
		{
			PrepareRoot();
			Root.Select(null);
			SetDitry();
		}

		void PrepareRoot()
		{
			if (Root != null) return;
			//親が管理出来る場合はそれを使う
			if (Parent is IRoot rootWidget)
			{
				rootWidget.Register(this);
				Root = rootWidget;
				Root.Register(this);
				return;
			}
			IRoot root = null;
			foreach (var c in Parent.GetChildren())
			{
				if (c is ScRadioButton b)
				{
					if (b.Root != null)
					{
						root = b.Root;
						break;
					}
				}
			}
			if (root == null)
			{
				root = new SimpleRoot();
			}
			Root = root;
			Root.Register(this);
		}

	}

	public class ScRadioButtonRoot : ScFunction, ScRadioButton.IRoot
	{
		ScRadioButton.SimpleRoot m_Root;
		ScRadioButton.SimpleRoot Root
		{
			get
			{
				if (m_Root != null) return m_Root;
				m_Root = new ScRadioButton.SimpleRoot();
				return m_Root;
			}
		}

		public System.Action<ScRadioButton> OnSelect { set => Root.OnSelect = value; }

		bool ScRadioButton.IRoot.IsSelect(ScRadioButton button) => Root.IsSelect(button);

		void ScRadioButton.IRoot.Register(ScRadioButton button) => Root.Register(button);

		void ScRadioButton.IRoot.Select(ScRadioButton button) => Root.Register(button);
	}

}