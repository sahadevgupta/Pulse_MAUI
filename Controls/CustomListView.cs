using System;

namespace Pulse_MAUI.Controls
{
	/// <summary>
	/// Optimized Listview for performance.
	/// Fogbugz Case:
	/// Author: Manuel Dambrine
	/// Created: 29/03/2013
	/// </summary>
	public class CustomListView : ListView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="T:Pulse_MAUI.Controls.CustomListView"/> class.
		/// </summary>
		public CustomListView() : base(ListViewCachingStrategy.RecycleElement)
		{
			
		}
	}
}
