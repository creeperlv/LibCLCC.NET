using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace LibCLCC.NET.IO
{
    /// <summary>
    /// Some functions about IO stream.
    /// </summary>
    public static class StreamUtilities
    {
        /// <summary>
        /// Read an int32 from stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer">Buffer, at least 4 bytes</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int32 ReadInt32(this Stream stream, byte[] buffer)
        {
            stream.Read(buffer, 0, 4);
            Span<byte> bytes = new Span<byte>(buffer, 0, 4);
            return BitConverter.ToInt32(bytes);
        }
        /// <summary>
        /// Read an int64 from stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer">Buffer, at least 8 bytes</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Int64 ReadInt64(this Stream stream, byte[] buffer)
        {
            stream.Read(buffer, 0, 8);
            Span<byte> bytes = new Span<byte>(buffer, 0, 8);
            return BitConverter.ToInt64(bytes);
        }
    }
}
