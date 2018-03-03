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
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWatcher.Alerts
{
	public enum TriggerType { crossing, lowerOrEqual, higherOrEqual, lower, higher  };
    /// <summary>
    /// Used for calculating conditions, not actual alert.
    /// </summary>
	class AbsoluteAlert
	{
        /// <summary>
        /// Value used for condition, e.g. higher than 70.
        /// </summary>
        public float Trigger; // must be field
		/// <summary>
		/// Use this if you don't want to automatically set previous value.
		/// </summary>
		public float current;
        /// <summary>
        /// When you update value set it here and then test condition.
        /// </summary>
		public float Current { get { return current; } set { previous = current; current = value; } }
		float previous = -1;
		public TriggerType Type { get; set; }

		public AbsoluteAlert() { }

		public AbsoluteAlert(float trigger, TriggerType type)
		{
			Trigger = trigger;
			Type = type;
		}

		// creates alert from csv line (the one that is in file)
		public AbsoluteAlert(ref string data)
		{
			int i;

			Type = (TriggerType)int.Parse(data.Substring(0, i = data.IndexOf(';')));
			data = data.Substring(i + 1);
			Trigger = float.Parse(data.Substring(0, i = data.IndexOf(';')));
			data = data.Substring(i + 1);
			// sometimes after last string there is nothing
			if ((i = data.IndexOf(';')) == -1)
				previous = float.Parse(data);
			else
			{
				previous = float.Parse(data.Substring(0, i));
				data = data.Substring(i + 1);
			}
		}
        /// <summary>
        /// Converts to csv line.
        /// </summary>
		public override string ToString()
		{
			// we don't need to store previous value if we aren't using it
			string previousValue_str = Type == TriggerType.crossing ? previous.ToString() : string.Empty;
			return $"{(int)Type};{Trigger};{previous}";
		}
        /// <summary>
        /// Tests if condition is met.
        /// </summary>
		public bool Test()
		{
			switch (Type)
			{
				case TriggerType.higher:
					if (Current > Trigger) return true;
					break;
				case TriggerType.higherOrEqual:
					if (Current >= Trigger) return true;
					break;
				case TriggerType.lower:
					if (Current < Trigger) return true;
					break;
				case TriggerType.lowerOrEqual:
					if (Current <= Trigger) return true;
					break;
				case TriggerType.crossing:
					if (previous == -1) throw new ArgumentException();
					if (Current > Trigger && previous < Trigger || Current < Trigger && previous > Trigger)
						return true;
					break;
			}
			return false;
		}
		/// <summary>
		/// Returns true if values ctross.
		/// </summary>
		/// <returns></returns>
		public bool Test(AbsoluteAlert absoluteAlert)
		{
			if (previous < absoluteAlert.previous && Current > absoluteAlert.Current || previous > absoluteAlert.previous && Current < absoluteAlert.Current)
				return true;

			return false;
		}

		public string ConditionToString()
		{
			return ConditionToString(Type);
		}

		public static string ConditionToString(TriggerType type)
		{
			switch (type)
			{
				case TriggerType.higher:
					return "Higher";
				case TriggerType.higherOrEqual:
					return "Higher or equal";
				case TriggerType.lower:
					return "Lower";
				case TriggerType.lowerOrEqual:
					return "Lower or equal";
				case TriggerType.crossing:
					return "Crossing";
			}
			throw new ArgumentException();
		}

		public static TriggerType StringToCondition(string type)
		{
			switch (type)
			{
				case "Higher": return TriggerType.higher;
				case "Higher or equal": return TriggerType.higherOrEqual;
				case "Lower": return TriggerType.lower;
				case "Lower or equal": return TriggerType.lowerOrEqual;
				case "Crossing": return TriggerType.crossing;
			}
			throw new ArgumentException();
		}
        /// <summary>
        /// Converts all condition types to string.
        /// </summary>
		public static string[] GetConditions()
		{
			var values = Enum.GetValues(typeof(TriggerType)).Cast<TriggerType>().ToArray();
			string[] types = new string[values.Count()];

			for (int i = 0; i < types.Length; i++)
			{
				types[i] = ConditionToString(values[i]);
			}
			return types;
		}

        /// <summary>
        /// Returns controls that is used for control panel when creating alert.
        /// </summary>
		public static object[] GetOptions()
		{
			var txtValue = new MetroFramework.Controls.MetroTextBox
			{
				Location = new System.Drawing.Point(83, 39),
				Name = "txtValue",
				Size = new System.Drawing.Size(124, 29),
				TabIndex = 10
			};

			var cBoxCondition = new MetroFramework.Controls.MetroComboBox
			{
				Location = new System.Drawing.Point(83, 0),
				Name = "cBoxCondition",
				Size = new System.Drawing.Size(124, 29),
			};
			cBoxCondition.Items.AddRange(GetConditions());
			cBoxCondition.SelectedIndex = 0;

			var metroLabel3 = new MetroFramework.Controls.MetroLabel
			{
				AutoSize = true,
				Location = new System.Drawing.Point(36, 49),
				Name = "lblValue",
				Size = new System.Drawing.Size(41, 19),
				Text = "Value:"
			};

			var metroLabel7 = new MetroFramework.Controls.MetroLabel
			{
				AutoSize = true,
				Location = new System.Drawing.Point(5, 10),
				Name = "lblCondition",
				Size = new System.Drawing.Size(69, 19),
				Text = "Condition:"
			};

			return new object[] { txtValue, cBoxCondition, metroLabel3, metroLabel7 };
		}
	}
}
