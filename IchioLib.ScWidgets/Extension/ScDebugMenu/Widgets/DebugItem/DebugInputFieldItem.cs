#if DEBUG || ILIB_DEBUG_MENU
using System.Collections;
using UnityEngine;
using ILib.ScWidgets;

namespace ILib.Debugs
{

	public class DebugInputFieldItemBase : DebugItemBase
	{
		protected ScInputField m_InputField;

		protected override int DefContentsWeight { get => 3; }

		protected override IScWidget Create()
		{
			return m_InputField = new ScInputField()
			{
				Layout = new StretchLayout { Margin = new RectOffset(0, 0, 4, 4) },
				OnChanged = OnChangedInput,
			};
		}
		protected virtual void OnChangedInput(string val) { }

	}

	public class DebugInputFieldItem : DebugInputFieldItemBase
	{
		public string Input
		{
			get => m_InputField.Input;
			set => m_InputField.Input = value;
		}

		public System.Action<string> OnChanged { set => m_InputField.OnChanged = value; }

	}

	public abstract class DebugValueFieldItem<T> : DebugInputFieldItemBase
	{
		protected T m_Value;
		public T Value
		{
			get => m_Value;
			set
			{
				m_Value = value;
				m_InputField.Input = value.ToString();
			}
		}

		public System.Action<T> OnChanged { set; protected get; }
	}

	public class DebugIntFieldItem : DebugValueFieldItem<int>
	{
		protected override void OnChangedInput(string val)
		{
			int input = 0;
			if (!int.TryParse(val, out input))
			{
				m_InputField.Input = m_Value.ToString();
			}
			else
			{
				if (input != m_Value)
				{
					m_Value = input;
					OnChanged(input);
				}
			}
		}

	}

	public class DebugFloatFieldItem : DebugValueFieldItem<float>
	{
		protected override void OnChangedInput(string val)
		{
			float input = 0;
			if (!float.TryParse(val, out input))
			{
				m_InputField.Input = m_Value.ToString();
			}
			else
			{
				if (input != m_Value)
				{
					m_Value = input;
					OnChanged(input);
				}
			}
		}
	}
}
#endif
