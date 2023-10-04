﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace LibCLCC.NET.Data
{
    /// <summary>
    /// Reference String.
    /// Use for reduce memory consumption.
    /// </summary>
    public struct RefString
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
            return s.Ref.AsSpan(s.Offset,s.Length);
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
            result.Offset -= R;
            result.Length += R;
            if (result.Offset < 0) throw new IndexOutOfRangeException();
            return result;
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
            return Ref.IndexOf(c, Offset);
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
            return Ref.IndexOf(str, Offset);
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
            return Ref.IndexOf(c, Offset + offset);
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
            return Ref.IndexOf(str, Offset + offset);
        }
    }
}
