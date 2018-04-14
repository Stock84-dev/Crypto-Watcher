/******************************************************************************
 * CRYPTO WATCHER - cryptocurrency alert system that notifies you when certain 
 * cryptocurrency fulfills your condition.
 * Copyright (c) 2017-2018 Stock84-dev
 * https://github.com/Stock84-dev/Crypto-Watcher
 *
 * This file is part of CRYPTO WATCHER.
 *
 * CRYPTO WATCHER is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * CRYPTO WATCHER is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with CRYPTO WATCHER.  If not, see <http://www.gnu.org/licenses/>.
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWatcher.Alert
{
	static class TA
	{
		public static List<float> GetRSI(List<float> input, int period)
		{
			List<float> output = new List<float>();
			float smooth_up = 0, smooth_down = 0, per = 1 / (float)period;
			int i;
			for (i = 1; i <= period; ++i)
			{
				float upward = input[i] > input[i - 1] ? input[i] - input[i - 1] : 0;
				float downward = input[i] < input[i - 1] ? input[i - 1] - input[i] : 0;
				smooth_up += upward;
				smooth_down += downward;
			}

			smooth_up /= period;
			smooth_down /= period;
			output.Add(100 * (smooth_up / (smooth_up + smooth_down)));

			for (i = period + 1; i < input.Count; ++i)
			{
				float upward = input[i] > input[i - 1] ? input[i] - input[i - 1] : 0;
				float downward = input[i] < input[i - 1] ? input[i - 1] - input[i] : 0;

				smooth_up = (upward - smooth_up) * per + smooth_up;
				smooth_down = (downward - smooth_down) * per + smooth_down;

				output.Add(100 * (smooth_up / (smooth_up + smooth_down)));
			}

			return output;
		}

		public static Stoch[] GetStoch(APIs.Candlestick[] candlesticks, int kperiod = 14, int dperiod = 3, int smooth = 1)
		{
			float[] high = new float[candlesticks.Length];
			float[] low = new float[candlesticks.Length];
			float[] close = new float[candlesticks.Length];

			for (int i = 0; i < candlesticks.Length; i++)
			{
				high[i] = candlesticks[i].high;
				low[i] = candlesticks[i].low;
				close[i] = candlesticks[i].close;
			}

			return GetStoch(high, low, close, kperiod, dperiod, smooth);
		}

		// TODO: implement smooth 
		public static Stoch[] GetStoch(float[] high, float[] low, float[] close, int kperiod = 14, int dperiod = 3, int smooth = 1)
		{
			Stoch[] stochs = new Stoch[high.Length - kperiod];

			for (int c = kperiod; c < high.Length; c++)
			{
				float highest = high[c], lowest = low[c];
				for (int i = c - kperiod; i < c; i++)
				{
					if (highest < high[i])
						highest = high[i];
					else if (lowest > low[i])
						lowest = low[i];
				}
				stochs[c - kperiod].stoch = (close[c] - lowest) / (highest - lowest) * 100; // %K
				if (c - kperiod - dperiod + 1 <= 0)
					stochs[c - kperiod].stoch_ma = 0;
				else
				{
					List<float> tmp = new List<float>(dperiod);

					for (int i = c - kperiod - dperiod; i < c - kperiod; i++)
					{
						tmp.Add(stochs[i].stoch);
					}
					stochs[c - kperiod].stoch_ma = GetSMA(tmp.ToArray(), dperiod).Last(); // %D
				}
			}
			return stochs;
		}

		public static MACD GetMACD(List<float> input, int fastLength, int slowLength)
		{
			int maxLength = fastLength > slowLength ? fastLength : slowLength;
			List<float> emaFast = GetEMA(input, fastLength);
			List<float> emaSlow = GetEMA(input, slowLength);
			MACD output = new MACD();
			output.Macd = Enumerable.Repeat(0f, maxLength - 1).ToList();
			output.Macd.AddRange(emaFast.GetRange(maxLength-1, emaFast.Count-maxLength+1).Zip(emaSlow.GetRange(maxLength-1, emaSlow.Count- maxLength+1), (x, y) => x - y).ToList());
			output.Signal = Enumerable.Repeat(0f, maxLength - 1).ToList();
			output.Signal.AddRange(GetEMA(output.Macd.GetRange(maxLength-1, output.Macd.Count-maxLength+1), 9));
			output.Histogram = Enumerable.Repeat(0f, maxLength + 9 - 2).ToList();
			output.Histogram.AddRange(output.Macd.GetRange(maxLength+9-2, output.Macd.Count-maxLength-7).Zip(output.Signal.GetRange(maxLength + 9 - 2, output.Macd.Count - maxLength - 7), (x, y) => x - y).ToList());

			return output;
		}

		public static List<float> GetEMA(List<float> input, int period)
		{
			List<float> output = Enumerable.Repeat(0f, period-1).ToList();
			output.Add(input.GetRange(0, period).Average());
			
			for (int i = period; i < input.Count; i++)
			{
				float value = input[i] * (2f / (period + 1)) + output.Last() * (1 - (2f / (period + 1)));
				output.Add(value);
			}

			return output;
		}

		public static float[] GetSMA(float[] input, int period = 3)
		{
			float sum = 0;
			int i;
			float[] output = new float[input.Length - period + 1];
			for (i = 0; i < period; ++i)
			{
				sum += input[i];
			}

			output[0] = (sum / period);

			for (i = period; i < input.Length; ++i)
			{
				sum += input[i];
				sum -= input[i - period];
				output[i - period + 1] = sum / period;
			}

			return output;
		}

		public struct Stoch
		{
			public float stoch { get; set; }
			public float stoch_ma { get; set; }

			public Stoch(float stoch, float stoch_ma)
			{
				this.stoch = stoch;
				this.stoch_ma = stoch_ma;
			}
		}

		public struct MACD
		{
			public List<float> Macd { get; set; }
			public List<float> Signal { get; set; }
			public List<float> Histogram { get; set; }
		}
	}
}
