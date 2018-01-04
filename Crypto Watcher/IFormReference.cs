using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWatcher
{
	// used to call form functions outside of the form
	interface IFormReference
	{
		void ShowWindow();
		void AddMessage(int id);
		void TryRemoveAlert(int id);
		void AddAlertToListView();
		void Popup_Click(object sender, EventArgs e);
	}
}
