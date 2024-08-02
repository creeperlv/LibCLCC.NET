using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.XPath;

namespace LibCLCC.NET.Data
{
	/// <summary>
	/// Reference String.
	/// Use for reduce memory consumption.
	/// </summary>
	public struct RefString : IEnumerable<char>, IEnumerable
	{
		/// <summary>
		/// The NULL reference.
		/// </summary>
		public readonly static RefString NULL = new RefString() { Ref = null };
		/// <summary>
		/// Referred string
		/// </summary>
		public string Ref;
		/// <summary>
		/// Offset in Ref
		/// </summary>
		public int Offset;
		/// <summary>
		/// Length of the RefString
		/// </summary>
		public int Length;
		/// <summary>
		/// Create a RefString.
		/// </summary>
		/// <param name="string_value"></param>
		/// <param name="offset"></param>
		/// <param name="length"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RefString(string string_value, int offset, int length)
		{
			Ref = string_value;
			Offset = offset;
			Length = length;
		}
		/// <summary>
		/// Get certain character
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public char this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get
			{
				return Ref[index + Offset];
			}
		}

		/// <summary>
		/// Treat it as a read-only stan
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<char> AsReadOnlySpan() => (ReadOnlySpan<char>)this;
		/// <summary>
		/// Convert a string to a RefString
		/// </summary>
		/// <param name="s"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator RefString(string s) => new RefString { Ref = s, Offset = 0, Length = s.Length };
		/// <summary>
		/// Convert a RefString to ReadOnlySpan
		/// </summary>
		/// <param name="s"></param>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static implicit operator ReadOnlySpan<char>(RefString s)
		{
			if (s.Offset + s.Length > s.Ref.Length)
			{
				throw new IndexOutOfRangeException();
			}
			return s.Ref.AsSpan(s.Offset, s.Length);
		}
		/// <summary>
		/// If the referred string is null.
		/// </summary>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool IsNull() => Ref == null;
		/// <summary>
		/// If the referred string is not null.
		/// </summary>
		/// <returns></returns>

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly bool IsNotNull() => Ref != null;
		/// <summary>
		/// Move the offset of the string. The length will be adjusted as well.
		/// </summary>
		/// <param name="L"></param>
		/// <param name="R"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RefString operator +(RefString L, int R)
		{
			RefString result = L;
			result.Ref = L.Ref;
			result.Offset += R;
			result.Length -= R;
			if (result.Length < 0) throw new IndexOutOfRangeException();
			return result;
		}
		/// <summary>
		/// Move the offset of the string. The length will be adjusted as well.
		/// </summary>
		/// <param name="L"></param>
		/// <param name="R"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static RefString operator -(RefString L, int R)
		{
			RefString result = L;
			result.Ref = L.Ref;
			result.Offset -= R;
			result.Length += R;
			if (result.Offset < 0) throw new IndexOutOfRangeException();
			return result;
		}
		/// <summary>
		/// Check if equal to a string.
		/// </summary>
		/// <param name="L"></param>
		/// <param name="R"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(RefString L, string R)
		{

			var startI = L.Ref.IndexOf(R, L.Offset) - L.Offset;
			var Len = R.Length;
			if (startI == 0 && Len == L.Length)
			{
				for (int i = 0; i < Len; i++)
				{
					if (L[i] != R[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
		/// <summary>
		/// Compare 2 RefString even if the internal Referenced strings are different.
		/// </summary>
		/// <param name="L"></param>
		/// <param name="R"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator ==(RefString L, RefString R)
		{

			var Len = R.Length;
			if (Len == L.Length)
			{
				for (int i = 0; i < Len; i++)
				{
					if (L[i] != R[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
		/// <summary>
		/// Compare 2 RefString even if the internal Referenced strings are different.
		/// </summary>
		/// <param name="L"></param>
		/// <param name="R"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(RefString L, RefString R)
		{
			return !(L == R);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="R"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool StartsWith(string R)
		{
			return (Ref.IndexOf(R, Offset) - Offset) == 0;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="R"></param>
		/// <param name="_offset"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool StartsWith(string R, int _offset)
		{
			return (Ref.IndexOf(R, Offset + _offset) - Offset) == 0;
		}
		char ToOtherCase(char c)
		{
			if (c >= 'A' && c <= 'Z')
			{
				return (char)(c - 'A' + 'a');
			}
			if (c >= 'a' && c <= 'z')
			{
				return (char)(c - +'A' - 'a');
			}
			return c;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="R"></param>
		/// <param name="_offset"></param>
		/// <param name="CaseIrrelevant"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool StartsWith(string R, int _offset, bool CaseIrrelevant)
		{
			if (CaseIrrelevant)
			{

				for (int i = 0; i < R.Length; i++)
				{
					char v = R[i];
					if (Ref[Offset + _offset + i] != v && Ref[Offset + _offset + i] != ToOtherCase(v))
					{
						return false;
					}
				}
				return true;
			}
			else
			{
				return this.Ref.IndexOf(R, Offset + _offset) - Offset == 0;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="R"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool EndsWith(string R)
		{
			if (R.Length > Length) return false;
			return this.IndexOf(R, Length - R.Length) == Length - R.Length;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="R"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool EndsWith(RefString R)
		{
			if (R.Length > Length) return false;
			return IndexOfSL(R, Length - R.Length) == Length - R.Length;
		}
		/// <summary>
		/// The opposite of ==
		/// </summary>
		/// <param name="L"></param>
		/// <param name="R"></param>
		/// <returns></returns>
		/// <exception cref="IndexOutOfRangeException"></exception>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool operator !=(RefString L, string R)
		{
			return !(L == R);
		}
		/// <summary>
		/// Retrieves a substring. The substring has a given start position and continues to the end of the RefString.
		/// </summary>
		/// <param name="StartIndex"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly RefString Substring(int StartIndex)
		{
			return this + StartIndex;
		}
		/// <summary>
		/// Retrieves a substring. The substring has a given start position and has a given length.
		/// </summary>
		/// <param name="StartIndex"></param>
		/// <param name="SubStringLength"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly RefString Substring(int StartIndex, int SubStringLength)
		{
			var r = this + StartIndex;
			if (r.Length < SubStringLength)
			{
				throw new ArgumentOutOfRangeException(nameof(SubStringLength));
			}
			r.Length = SubStringLength;
			return r;
		}
		/// <summary>
		/// Returns a zero-based index of the first appearance of a given char within this sturct. 
		/// The method returns -1 if the target is not found in this instance.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int IndexOf(char c)
		{
			return Ref.IndexOf(c, Offset, Length) - Offset;
		}
		/// <summary>
		/// Returns a zero-based index of the first appearance of a given string within this sturct. 
		/// The method returns -1 if the target is not found in this instance.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int IndexOf(string str)
		{
			return Ref.IndexOf(str, Offset, Length) - Offset;
		}
		/// <summary>
		/// Will create Reference Object!
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int IndexOf(RefString str)
		{
			return Ref.IndexOf(str.FinalizeString(), Offset, Length) - Offset;
		}
		/// <summary>
		/// Will create Reference Object!
		/// </summary>
		/// <param name="str"></param>
		/// <param name="Offset"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int IndexOf(RefString str, int Offset)
		{
			return IndexOf(str.FinalizeString(), Offset);
		}
		/// <summary>
		/// Slow version of IndexOf, but use less memory.
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public readonly int IndexOfSL(RefString str)
		{
			for (int i = 0; i < this.Length - str.Length; i++)
			{
				var L = this + i;
				L.Length = str.Length;
				if (L == str)
				{
					return i;
				}
			}
			return -1;
		}
		/// <summary>
		/// Slow version of IndexOf, but use less memory.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="Offset"></param>
		/// <returns></returns>
		public readonly int IndexOfSL(RefString str, int Offset)
		{
			for (int i = Offset; i <= Length - str.Length; i++)
			{
				var L = this + i;
				L.Length = str.Length;
				if (L == str)
				{
					return i;
				}
			}
			return -1;
		}
		/// <summary>
		/// Returns a zero-based index of the first appearance of a given char within this sturct. 
		/// The method returns -1 if the target is not found in this instance.
		/// The search starts at a given position.
		/// </summary>
		/// <param name="c"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int IndexOf(char c, int offset)
		{
			return Ref.IndexOf(c, Offset + offset, Length) - Offset;
		}
		/// <summary>
		/// Returns a zero-based index of the first appearance of a given char within this sturct. 
		/// The method returns -1 if the target is not found in this instance.
		/// The search starts at a given position.
		/// </summary>
		/// <param name="c"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int IndexOf(char c, int offset, int count)
		{
			return Ref.IndexOf(c, Offset + offset, Math.Min(Length, count)) - Offset;
		}
		/// <summary>
		/// Returns a zero-based index of the first appearance of a given string within this sturct. 
		/// The method returns -1 if the target is not found in this instance.
		/// The search starts at a given position.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int IndexOf(string str, int offset)
		{
			int startIndex = Offset + offset;
			return Ref.IndexOf(str, startIndex, Length - offset) - Offset;
		}
		/// <summary>
		/// Returns a zero-based index of the first appearance of a given string within this sturct. 
		/// The method returns -1 if the target is not found in this instance.
		/// The search starts at a given position.
		/// </summary>
		/// <param name="str"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly int IndexOf(string str, int offset, int count)
		{
			return Ref.IndexOf(str, Offset + offset, Math.Min(Length, count)) - Offset;
		}
		/// <summary>
		/// Iterate through characters in range [offset..offset+length].
		/// </summary>
		/// <returns></returns>

		public IEnumerator<char> GetEnumerator()
		{
			return iterate();
		}
		/// <summary>
		/// Split the string.
		/// </summary>
		/// <param name="splitters"></param>
		/// <returns></returns>
		public IEnumerator<RefString> Split(params char[] splitters)
		{
			RefString result = new RefString(this.Ref, Offset, 0);
			for (int i = Offset; i < Length; i++)
			{
				var item = this.Ref[i];
				foreach (var tester in splitters)
				{
					if (item == tester)
					{
						yield return result;
						result = new RefString(this.Ref, i + 1, 0);
						goto SKIP;
					}
				}
				result.Length++;
			SKIP:;
			}
			yield return result;
		}
		/// <summary>
		/// Split the string.
		/// </summary>
		/// <param name="splitters"></param>
		/// <returns></returns>
		public RefStringSplitQuery SplitQuery(params char[] splitters)
		{
			return new RefStringSplitQuery { query = Split(splitters) };
		}
		IEnumerator<char> iterate()
		{
			for (int i = Offset; i < Length; i++)
			{
				yield return Ref[i];
			}
			yield break;
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return iterate();
		}
		/// <summary>
		/// Convert to a new string.
		/// </summary>
		/// <returns></returns>
		public string FinalizeString()
		{
			return Ref[Offset..(Offset + Length)];
		}
		/// <summary>
		/// Equality to any obj.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			if (obj is string s)
			{
				return this == s;
			}
			return base.Equals(obj);
		}
		/// <summary>
		/// Get the hash code of the RefString.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		/// <summary>
		/// ToString via FinalizeString().
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return FinalizeString();
		}
	}
}
