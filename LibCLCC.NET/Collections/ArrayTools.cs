using System;
using System.Collections.Generic;

namespace LibCLCC.NET.Collections
{
	/// <summary>
	/// Tools for arrays.
	/// </summary>
	public static class ArrayTools
	{
		/// <summary>
		/// Is an array contains an element.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arr"></param>
		/// <param name="t"></param>
		/// <returns></returns>
		public static bool Contains<T>(T [ ] arr , T t)
		{
			foreach (var item in arr)
			{
				if (item.Equals(t)) return true;
			}
			return false;
		}
		/// <summary>
		/// Randomly pick one in the array.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arr"></param>
		/// <param name="random"></param>
		/// <returns></returns>
		public static T PickOne<T>(this T [ ] arr , Random random)
		{
			return arr [ random.Next(arr.Length) ];
		}
		/// <summary>
		/// Randomly pick on in the list.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arr"></param>
		/// <param name="random"></param>
		/// <returns></returns>
		public static T PickOne<T>(this List<T> arr , Random random)
		{
			return arr [ random.Next(arr.Count) ];
		}
	}
}
