using System;
using System.Collections.Generic;
using System.Text;

namespace LibCLCC.NET.Collections
{
	/// <summary>
	/// Some tool methods for Dictionaries.
	/// </summary>
	public static class DictionaryTools
	{
		/// <summary>
		/// Merge from DataSource to Target. The DataSource will remain unchanged.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="V"></typeparam>
		/// <param name="Target"></param>
		/// <param name="DataSource"></param>
		/// <param name="ReplaceSource"></param>
		public static void Merge<T,V>(this Dictionary<T,V> Target,Dictionary<T,V> DataSource,bool ReplaceSource=true)
		{
			foreach (var item in DataSource)
			{
				if (Target.ContainsKey(item.Key))
				{
					if (ReplaceSource) Target [ item.Key ] = item.Value;
				}
				else
				{
					Target.Add(item.Key, item.Value);
				}
			}
		}
	}
}
