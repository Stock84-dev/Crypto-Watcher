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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoWatcher.Utilities
{
	/// <summary>
	/// usage
	///Container c = new Container();
	///c.CreateEventForKey("a");             // Create the member in the dictionary
	///c.EventForKey("a").Add(str => Console.WriteLine(str));
	///c.EventForKey("a").Add(str => Console.WriteLine(str.ToUpper()));
	///c.OnEventForKey("a", "baa baa black sheep");
	/// </summary>
	public class MyEvent<T>
	{
		private readonly object _door = new object();

		protected Dictionary<string, Member> members = new Dictionary<string, Member>();

		public void CreateEventForKey(string key)
		{
			lock(_door) members[key] = new Member();
		}

		/// <summary>
        /// Calls all events.
        /// </summary>
        /// <param name="k">Key name.</param>
        /// <param name="parameters">Parameters passed to called functions.</param>
		public void OnEventForKey(string k, T parameter)
		{
			lock (_door)
			{
				if (members.ContainsKey(k))
					members[k].OnEvent(parameter);
				else throw new ArgumentException();
			}
		}

		/// <summary>
		/// Add action to list to get called.
		/// </summary>
		public List<Action<T>> EventForKey(string k)
		{
			lock (_door)
			{
				if (members.ContainsKey(k))
					return members[k].AnEvent;
				else throw new KeyNotFoundException();
			}
		}

		public bool ContainsKey(string key)
		{
			lock (_door)
			{
				if (members.ContainsKey(key))
					return true;
			}
			return false;
		}

		public void Remove(string key)
		{
			lock (_door) members.Remove(key);
		}

		public void RemoveEvent(string key, Action<T> action)
		{
			lock (_door) members[key].RemoveEvent(action);
		}

		public int Count(string key)
		{
			lock (_door) return members[key].AnEvent.Count;
		}

		public class Member
		{
			public List<Action<T>> AnEvent = new List<Action<T>>();
			public void OnEvent(T parameter)
			{
				if (AnEvent != null)
				{
					// creating copy to prevent data corruption caused by threading, not best practice
					AnEvent.ToList().ForEach(action => action(parameter));
				}
			}

			public void AddEvent(Action<T> action)
			{
				this.AnEvent.Add(action);
			}

			public void RemoveEvent(Action<T> action)
			{
				AnEvent.Remove(action);
			}
		}
	}
}
