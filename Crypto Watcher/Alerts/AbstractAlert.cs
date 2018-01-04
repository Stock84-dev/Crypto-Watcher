using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts
{
	abstract class AbstractAlert
	{
		protected AlertData alertData = new AlertData();
		public Notification notification { get; set; }

		public bool IsMessageShowing { get; set; } = false;
		public DateTime LastTriggered { get; set; } = new DateTime(1, 1, 1, 1, 1, 1, 1);
		public bool IsNotifying { get; set; } = false;

		public AbstractAlert() { }

		public AbstractAlert(AlertData alertData, Notification notification)
		{
			this.alertData = alertData;
			this.notification = notification;
		}

		public abstract string ToLine();
		public abstract Task<bool> Test();
		public abstract string[] ToRow();
		public abstract string Message { get; }
		public abstract int GetAverageCost();

		public void Notify(DateTime currentTime)
		{
			LastTriggered = currentTime;
			notification.Notify(Alert.AlertList.IndexOf(this));
			IsNotifying = true;
		}

		public void StopNotifying(bool tryRemovingAlert = true)
		{
			notification.StopNotifying(Alert.AlertList.IndexOf(this), tryRemovingAlert);
			IsNotifying = false;
		}

		// creates different alert types from line
		public static AbstractAlert FromLine(string data)
		{
			int column = data.IndexOf(';');
			switch (column)
			{
				case 1:
					return new AbsoluteAlert(data.Substring(column + 1));
				default:
					return null;
			}
		}

		// returns alert data to line
		protected string BaseToLine()
		{
			return alertData.ToLine() + notification.ToLine() + LastTriggered.ToString() + ";";
		}

		// creates alert data from line
		protected void BaseFromLine(ref string data)
		{
			int i = data.IndexOf(';');
			alertData.BaseSymbol = data.Substring(0, i);
			data = data.Substring(i + 1);
			alertData.QuoteSymbol = data.Substring(0, (i = data.IndexOf(';')));
			data = data.Substring(i + 1);
			alertData.BaseName = data.Substring(0, (i = data.IndexOf(';')));
			data = data.Substring(i + 1);
			alertData.ExchangeName = data.Substring(0, (i = data.IndexOf(';')));
			data = data.Substring(i + 1);
			alertData.MarketRoute = data.Substring(0, (i = data.IndexOf(';')));
			data = data.Substring(i + 1);
			notification = new Notification(ref data);
			LastTriggered = DateTime.Parse(data.Substring(0, (i = data.IndexOf(';'))));
			data = data.Substring(i + 1);
		}
	}
}
