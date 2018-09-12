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

using CryptoWatcher.Utilities;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWatcher.Alert
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
		public float Trigger; // must be field, property cannot be passed as ref parameter using out keyword
		/// <summary>
		/// Use this if you don't want to automatically set previous value.
		/// </summary>
		public float current = -1;
		private float _previous = -1;

		public AbsoluteAlert() : base() { }

		public AbsoluteAlert(float trigger, TriggerType type)
		{
			Trigger = trigger;
			Type = type;
		}

		// creates alert from csv line (the one that is in file)
		public AbsoluteAlert(ref string data)
		{
			Type = (TriggerType)int.Parse(Utility.GetSubstring(data, ';', 0));
			Trigger = float.Parse(Utility.GetSubstring(data, ';', 1));
			_previous = float.Parse(Utility.GetSubstring(data, ';', 2));
			data = Utility.GetSubstring(data, ';', 3, false);
		}

		/// <summary>
		/// When you update value set it here and then test condition, this will automatically set previous value.
		/// </summary>
		public float Current { get { return current; } set { _previous = current; current = value; } }
		public TriggerType Type { get; set; }

		/// <summary>
		/// Returns controls that is used for control panel when creating alert.
		/// </summary>
		public static object[] GetOptions()
		{
			var txtValue = new MetroTextBox
			{
				Location = new System.Drawing.Point(83, 39),
				Name = "txtValue",
				Size = new System.Drawing.Size(124, 29),
				TabIndex = 10
			};

			var cBoxCondition = new MetroComboBox
			{
				Location = new System.Drawing.Point(83, 0),
				Name = "cBoxCondition",
				Size = new System.Drawing.Size(124, 29),
			};
			cBoxCondition.Items.AddRange(GetConditions());
			cBoxCondition.SelectedIndex = 0;

			var metroLabel3 = new MetroLabel
			{
				AutoSize = true,
				Location = new System.Drawing.Point(36, 49),
				Name = "lblValue",
				Size = new System.Drawing.Size(41, 19),
				Text = "Value:"
			};

			var metroLabel7 = new MetroLabel
			{
				AutoSize = true,
				Location = new System.Drawing.Point(5, 10),
				Name = "lblCondition",
				Size = new System.Drawing.Size(69, 19),
				Text = "Condition:"
			};

			return new object[] { txtValue, cBoxCondition, metroLabel3, metroLabel7 };
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
		/// Converts to csv line.
		/// </summary>
		public override string ToString()
		{
			return $"{(int)Type};{Trigger.ToString("R")};{_previous}";
		}
        /// <summary>
        /// Tests if condition is met.
        /// </summary>
		public bool Test()
		{
			// preventing firing alert when previous isn't initialized
			if (_previous == -1 && Type == TriggerType.crossing)
				return false;
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
					if (Current > Trigger && _previous < Trigger || Current < Trigger && _previous > Trigger)
						return true;
					break;
			}
			return false;
		}
		/// <summary>
		/// Returns true if values cross.
		/// </summary>
		public bool Test(AbsoluteAlert absoluteAlert)
		{
			if (_previous < absoluteAlert._previous && Current > absoluteAlert.Current || _previous > absoluteAlert._previous && Current < absoluteAlert.Current)
				return true;

			return false;
		}

		public string ConditionToString()
		{
			return ConditionToString(Type);
		}
	}
}
