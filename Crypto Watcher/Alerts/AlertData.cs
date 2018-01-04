using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alerts
{
	// data that is required for all alerts
	class AlertData
	{
		public AlertData() { }

		public AlertData(string baseSymbol, string quoteSymbol, string baseName, string exchangeName, string marketRoute)
		{
			BaseSymbol = baseSymbol;
			QuoteSymbol = quoteSymbol;
			BaseName = baseName;
			ExchangeName = exchangeName;
			MarketRoute = marketRoute;
		}

		public string BaseSymbol { get; set; }
		public string QuoteSymbol { get; set; }
		public string BaseName { get; set; }
		public string ExchangeName { get; set; }
		public string MarketRoute { get; set; }

		public string ToLine()
		{
			return 
				BaseSymbol + ";" +
				QuoteSymbol + ";" +
				BaseName + ";" +
				ExchangeName + ";" +
				MarketRoute + ";";
		}
	}
}
