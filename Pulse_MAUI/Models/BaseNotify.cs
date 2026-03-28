using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Pulse_MAUI.Models;

namespace Pulse_MAUI.Models
{
	/// <summary>
	/// Class implementing INotifyPropertyChanged. It handles the IsDirty flag when one
	/// of the fields in the model has been modified.
	/// </summary>
	public class BaseNotify : INotifyPropertyChanged, IDisposable
	{
		readonly Dictionary<string, List<Action>> _actions = new Dictionary<string, List<Action>>();

		/// <summary>
		/// Occurs when property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Pulse_MAUI.Models.BaseNotify"/> class.
		/// </summary>
		public BaseNotify()
		{
			PropertyChanged += OnPropertyChanged;
		}

		/// <summary>
		/// Releases all resource used by the <see cref="T:Pulse_MAUI.Models.BaseNotify"/> object.
		/// </summary>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="T:Pulse_MAUI.Models.BaseNotify"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="T:Pulse_MAUI.Models.BaseNotify"/> in an unusable state.
		/// After calling <see cref="Dispose"/>, you must release all references to the
		/// <see cref="T:Pulse_MAUI.Models.BaseNotify"/> so the garbage collector can reclaim the memory that the
		/// <see cref="T:Pulse_MAUI.Models.BaseNotify"/> was occupying.</remarks>
		public virtual void Dispose()
		{
			ClearEvents();
		}

		/// <summary>
		/// Sets the property changed.
		/// </summary>
		/// <returns><c>true</c>, if property changed was set, <c>false</c> otherwise.</returns>
		/// <param name="currentValue">Current value.</param>
		/// <param name="newValue">New value.</param>
		/// <param name="propertyName">Property name.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public bool SetPropertyChanged<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		{
			return PropertyChanged.SetProperty(this, ref currentValue, newValue, propertyName);
		}

		/// <summary>
		/// Sets the property changed.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		public void SetPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		/// <summary>
		/// OnPropertyChanged method.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="propertyChangedEventArgs">The ${ParameterType} instance containing the event data.</param>
		void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
		{
			List<Action> actionList;
			if (!_actions.TryGetValue(propertyChangedEventArgs.PropertyName, out actionList))
				return;

			foreach (Action action in actionList)
			{
				action();
			}
		}

		/// <summary>
		/// Clears the events handlers.
		/// </summary>
		public void ClearEvents()
		{
			//Super awesome trick to wipe attached event handlers - +1 Clancey
			_actions.Clear();
			if (PropertyChanged == null)
				return;

			var invocation = PropertyChanged.GetInvocationList();
			foreach (var p in invocation)
				PropertyChanged -= (PropertyChangedEventHandler)p;
		}
	}

	/// <summary>
	/// IDirty Interface definition.
	/// </summary>
	public interface IDirty
	{
		bool IsDirty
		{
			get;
			set;
		}
	}
}

namespace System.ComponentModel
{
	public static class BaseNotify
	{
		/// <summary>
		/// Just adding some new functionality to System.ComponentModel
		/// </summary>
		/// <returns><c>true</c>, if property was set, <c>false</c> otherwise.</returns>
		/// <param name="handler">Handler.</param>
		/// <param name="sender">Sender.</param>
		/// <param name="currentValue">Current value.</param>
		/// <param name="newValue">New value.</param>
		/// <param name="propertyName">Property name.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool SetProperty<T>(this PropertyChangedEventHandler handler, object sender, ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
				return false;

			currentValue = newValue;

			var dirty = sender as IDirty;
            if (dirty != null)
            {
                dirty.IsDirty = true;
            }

            if (handler == null)
            {
                return true;
            }

			handler.Invoke(sender, new PropertyChangedEventArgs(propertyName));
			return true;
		}
	}
}