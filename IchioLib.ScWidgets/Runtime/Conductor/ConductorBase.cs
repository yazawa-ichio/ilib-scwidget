using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace ILib.ScWidgets
{

	public abstract class ConductorBase<TContext>
	{
		internal protected interface IHandler
		{
			Type TargetType { get; }
			int Priority { get; }
			void Prepare(TContext context, IScWidget widget);
			void Run(TContext context, IScWidget widget);
			void Finish(TContext context, IScWidget widget);
		}

		public abstract class Handler<U> : IHandler where U : IScWidget
		{
			public virtual int Priority { get; }
			protected virtual void Prepare(TContext context, U widget) { }
			protected abstract void Run(TContext context, U widget);
			protected virtual void Finish(TContext context, U widget) { }


			Type IHandler.TargetType => typeof(U);

			void IHandler.Prepare(TContext context, IScWidget widget) => Prepare(context, (U)widget);

			void IHandler.Run(TContext context, IScWidget widget) => Run(context, (U)widget);

			void IHandler.Finish(TContext context, IScWidget widget) => Finish(context, (U)widget);

		}

		static protected Dictionary<Type, IHandler> s_Handler;

		static ConductorBase()
		{
			s_Handler = CreateHanders();
		}

		static Dictionary<Type, IHandler> CreateHanders()
		{
			var ret = new Dictionary<Type, IHandler>();
			foreach (var assembile in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var type in assembile.GetTypes())
				{
					if (!typeof(IHandler).IsAssignableFrom(type) || type.IsAbstract)
					{
						continue;
					}
					var hander = (IHandler)Activator.CreateInstance(type, true);
					if (ret.ContainsKey(hander.TargetType))
					{
						var cur = ret[hander.TargetType];
						ret[hander.TargetType] = hander.Priority > cur.Priority ? hander : cur;
					}
					else
					{
						ret[hander.TargetType] = hander;
					}
				}
			}
			return ret;
		}

		protected IScWidget Root { get; private set; }
		protected TContext Context { get; private set; }
		protected Action m_Action;

		public ConductorBase() { }

		public ConductorBase(TContext context)
		{
			Context = context;
		}

		public void SetContext(TContext context)
		{
			Context = context;
		}

		public void SetRootWidget(IScWidget root)
		{
			Root = root;
		}

		protected void CalcLayout(UnityEngine.Rect rect)
		{
			Root.CalcLayout(rect);
		}

		protected void Prepare(IScWidget widget)
		{
			IHandler handler;
			if (s_Handler.TryGetValue(widget.GetType(), out handler))
			{
				handler.Prepare(Context, widget);
			}
			else
			{
				FallbackPrepare(Context, widget);
			}

			foreach (var child in widget.GetChildren())
			{
				Prepare(child);
			}
		}

		protected virtual void FallbackPrepare(TContext context, IScWidget widget)
		{

		}

		protected void Run(IScWidget widget)
		{
			IHandler handler = null;
			try
			{
				if (s_Handler.TryGetValue(widget.GetType(), out handler))
				{
					handler.Run(Context, widget);
				}
				else
				{
					FallbackRun(Context, widget);
				}
				foreach (var child in widget.GetChildren())
				{
					Run(child);
				}
			}
			finally
			{
				if (handler != null)
				{
					handler.Finish(Context, widget);
				}
				else
				{
					FallbackFinish(Context, widget);
				}
			}
		}

		protected virtual void FallbackRun(TContext context, IScWidget widget)
		{

		}

		protected virtual void FallbackFinish(TContext context, IScWidget widget)
		{

		}

		protected void Run<T>(Action<TContext, IScWidget, T> action) where T : class
		{
			Func<TContext, IScWidget, T, IDisposable> func = (x1, x2, x3) =>
			{
				action?.Invoke(x1, x2, x3);
				return null;
			};
			Run(Root, func);
		}

		protected void Run<T>(Func<TContext, IScWidget, T, IDisposable> func) where T : class
		{
			Run(Root, func);
		}

		void Run<T>(IScWidget widget, Func<TContext, IScWidget, T, IDisposable> func) where T : class
		{
			IHandler _handler;
			s_Handler.TryGetValue(widget.GetType(), out _handler);
			IDisposable disposable = null;
			try
			{
				if (_handler is T hander)
				{
					disposable = func?.Invoke(Context, widget, hander);
				}
				foreach (var child in widget.GetChildren())
				{
					Run(child, func);
				}
			}
			finally
			{
				disposable?.Dispose();
			}
		}

		public void Schedule(Action action)
		{
			m_Action = action;
		}

		protected void InvokeAction()
		{
			m_Action?.Invoke();
			m_Action = null;
		}


	}

}