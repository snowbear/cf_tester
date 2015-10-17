using System;
using System.Windows;

namespace Tester.Helpers
{
	public class Future<T>
	{
		private Action<T> _handler;

		public Future(out Action<T> endCallback)
		{
			endCallback = FinishCallback;
		}

		public Future<T> OnComplete(Action<T> handler)
		{
			_handler += handler;
			return this;
		}

		private void FinishCallback(T result)
		{
			Application.Current.Dispatcher.BeginInvoke(new Action(() =>
				{
					if (_handler != null)
						_handler(result);
				}));
		}

		public Future<TRes> Transform<TRes>(Func<T, TRes> map)
		{
			var promise = new Promise<TRes>();
			OnComplete(res => promise.Complete(map(res)));
			return promise.Future;
		}
	}

	public class Promise<T>
	{
		private readonly Action<T> _endFuture;

		public Future<T> Future { get; private set; }

		public Promise()
		{
			Action<T> endFuture;
			Future = new Future<T>(out endFuture);
			_endFuture = endFuture;
		}

		public void Complete(T result)
		{
			_endFuture(result);
		}
	}

	public static class Future
	{
		public static Future<T> FromDelegate<T>(Func<T> @delegate)
		{
			var promise = new Promise<T>();
            @delegate.BeginInvoke(ar =>
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => promise.Complete(@delegate.EndInvoke(ar))));
            }, null);
			return promise.Future;
		}
	}
}