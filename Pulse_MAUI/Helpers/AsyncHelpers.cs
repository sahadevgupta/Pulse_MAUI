namespace Pulse_MAUI.Helpers
{
	/// <summary>
	/// Helper class for running async tasks synchronously.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public static class AsyncHelpers
	{
		/// <summary>
		/// Execute's an async Task<T> method which has a void return value synchronously
		/// </summary>
		/// <param name="task">Task<T> method to execute</param>
		public static void RunSync(Func<Task> task)
		{
			var oldContext = SynchronizationContext.Current;
			var synch = new ExclusiveSynchronizationContext();
			SynchronizationContext.SetSynchronizationContext(synch);
			synch.Post(async _ =>
				{
					try
					{
						await task();
					}
					catch (Exception e)
					{
						synch.InnerException = e;
						throw;
					}
					finally
					{
						synch.EndMessageLoop();
					}
				}, null);
			synch.BeginMessageLoop();

			SynchronizationContext.SetSynchronizationContext(oldContext);
		}

		/// <summary>
		/// Execute's an async Task<T> method which has a T return type synchronously
		/// </summary>
		/// <typeparam name="T">Return Type</typeparam>
		/// <param name="task">Task<T> method to execute</param>
		/// <returns></returns>
		public static T RunSync<T>(Func<Task<T>> task)
		{
			var oldContext = SynchronizationContext.Current;
			var synch = new ExclusiveSynchronizationContext();
			SynchronizationContext.SetSynchronizationContext(synch);
			T ret = default(T);
			synch.Post(async _ =>
				{
					try
					{
						ret = await task();
					}
					catch (Exception e)
					{
						synch.InnerException = e;
						throw;
					}
					finally
					{
						synch.EndMessageLoop();
					}
				}, null);
			synch.BeginMessageLoop();
			SynchronizationContext.SetSynchronizationContext(oldContext);
			return ret;
		}

        /// <summary>
        /// {CC2D43FA-BBC4-448A-9D0B-7B57ADF2655C}
        /// </summary>
        /// <seealso cref="System.Threading.SynchronizationContext" />
        private class ExclusiveSynchronizationContext : SynchronizationContext
		{
			private bool done;
			public Exception InnerException { get; set; }
			readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
			readonly Queue<Tuple<SendOrPostCallback, object>> items =
				new Queue<Tuple<SendOrPostCallback, object>>();

            /// <summary>
            /// When overridden in a derived class, dispatches a synchronous message to a synchronization context.
            /// </summary>
            /// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback" /> delegate to call.</param>
            /// <param name="state">The object passed to the delegate.</param>
            /// <exception cref="NotSupportedException">We cannot send to our same thread</exception>
            public override void Send(SendOrPostCallback d, object state)
			{
				throw new NotSupportedException("We cannot send to our same thread");
			}

            /// <summary>
            /// When overridden in a derived class, dispatches an asynchronous message to a synchronization context.
            /// </summary>
            /// <param name="d">The <see cref="T:System.Threading.SendOrPostCallback" /> delegate to call.</param>
            /// <param name="state">The object passed to the delegate.</param>
            public override void Post(SendOrPostCallback d, object state)
			{
				lock (items)
				{
					items.Enqueue(Tuple.Create(d, state));
				}
				workItemsWaiting.Set();
			}

            /// <summary>
            /// Ends the message loop.
            /// </summary>
            public void EndMessageLoop()
			{
				Post(_ => done = true, null);
			}

            /// <summary>
            /// Begins the message loop.
            /// </summary>
            /// <exception cref="AggregateException">AsyncHelpers.Run method threw an exception.</exception>
            public void BeginMessageLoop()
			{
				while (!done)
				{
					Tuple<SendOrPostCallback, object> task = null;
					lock (items)
					{
						if (items.Count > 0)
						{
							task = items.Dequeue();
						}
					}
					if (task != null)
					{
						task.Item1(task.Item2);
						if (InnerException != null) // the method threw an exeption
						{
							throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
						}
					}
					else
					{
						workItemsWaiting.WaitOne();
					}
				}
			}

            /// <summary>
            /// When overridden in a derived class, creates a copy of the synchronization context.
            /// </summary>
            /// <returns>
            /// A new <see cref="T:System.Threading.SynchronizationContext" /> object.
            /// </returns>
            public override SynchronizationContext CreateCopy()
			{
				return this;
			}
		}
	}

}

