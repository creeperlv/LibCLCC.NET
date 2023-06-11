using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace LibCLCC.NET.Data
{
    /// <summary>
    /// Converters that parse string to numbers.
    /// </summary>
    public static class NumberConverters
    {
        /// <summary>
        /// A StartsWith that will skip a few count of from start.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <param name="Start"></param>
        /// <returns></returns>
        public static bool StartsWith(this string value , string pattern , int Start)
        {
            if (value.Length < pattern.Length + Start)
            {
                return false;
            }

            var Length = pattern.Length;
            for (int i = 0 ; i < Length ; i++)
            {
                if (value [ i + Start ] != pattern [ i ])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// Parse a string to an int32.
        /// <br/>
        /// Accepted format: 0x, 0o, 0b and scientific notation.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool TryParse(this string input , out int data)
        {

            bool Negative = input.StartsWith("-");
            input = input.ToUpper();
            int Start = 0;
            int Length = input.Length;
            if (Negative)
            {
                Start += 1;
            }
            if (input.EndsWith("L"))
            {
                data = -1;
                return false;
            }
            if (input.StartsWith("0X" , Start))
            {
                Start += 2;
                int _data = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    _data *= 16;
                    if ((input [ i ] >= '0' && input [ i ] <= '9'))
                    {
                        _data += input [ i ] - '0';
                    }
                    else
                    if ((input [ i ] >= 'A' && input [ i ] <= 'F'))
                    {
                        _data += input [ i ] - 'A' + 10;
                    }
                    else if ((input [ i ] == '_')) continue;
                    else
                    {
                        data = -1;
                        return false;
                    }
                }
                data = _data * (Negative ? -1 : 1);
                return true;
            }
            else if (input.StartsWith("0O" , Start))
            {
                Start += 2;
                int _data = 8;
                for (int i = Start ; i < Length ; i++)
                {
                    _data *= 16;
                    if ((input [ i ] >= '0' && input [ i ] <= '7'))
                    {
                        _data += input [ i ] - '0';
                    }
                    else if (input [ i ] == '_') continue;
                    else
                    {
                        data = -1;
                        return false;
                    }
                }
                data = _data * (Negative ? -1 : 1);
                return true;
            }
            else
            if (input.StartsWith("0B" , Start))
            {
                Start += 2;
                int _data = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    _data *= 2;
                    if (input [ i ] == '0')
                    {
                        _data += 0;
                    }
                    else
                    if (input [ i ] == '1')
                    {
                        _data += 1;
                    }
                    else if ((input [ i ] == '_')) continue;
                    else
                    {
                        data = -1;
                        return false;
                    }
                }
                data = _data * (Negative ? -1 : 1);
                return true;
            }
            else
            {

                int _data = 0;
                int exp = 0;
                byte mde = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    if (mde == 0)
                        _data *= 10;
                    if (mde == 1)
                        exp *= 10;
                    if ((input [ i ] >= '0' && input [ i ] <= '9'))
                    {
                        if (mde == 0)
                            _data += (input [ i ] - '0');
                        if (mde == 1)
                            exp += (input [ i ] - '0');
                    }
                    else if ((input [ i ] == '_')) continue;
                    else if ((input [ i ] == 'E')) mde = 1;
                    else if ((input [ i ] == '-') && mde == 1 && exp >= 0) exp *= -1;
                    else
                    {
                        data = -1;
                        return false;
                    }
                }
                if (exp > 0)
                    for (int i = 0 ; i < exp ; i++)
                    {
                        _data *= 10;
                    }
                else for (int i = 0 ; i > exp ; i--)
                    {
                        _data /= 10;
                    }
                data = _data * (Negative ? -1 : 1);
                return true;
            }
        }
        /// <summary>
        /// Parse a string to a single.
        /// <br/>
        /// Accepted format: scientific notation.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool TryParse(this string input , out float data)
        {

            bool Negative = input.StartsWith("-");
            input = input.ToUpper();
            int Start = 0;
            int Length = input.Length;
            if (Negative)
            {
                Start = 1;
            }
            if (input.EndsWith("F"))
            {
                Length -= 1;
            }
            {

                int _data = 0;
                int exp = 0;
                byte mde = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    if (mde == 0)
                        _data *= 10;
                    if (mde == 1)
                        exp *= 10;
                    if ((input [ i ] >= '0' && input [ i ] <= '9'))
                    {
                        if (mde == 0)
                            _data += (input [ i ] - '0');
                        if (mde == 2)
                        {
                            _data += (input [ i ] - '0');
                            exp += 1;
                        }
                        if (mde == 1)
                            exp += (input [ i ] - '0');
                    }
                    else if ((input [ i ] == '_')) continue;
                    else if ((input [ i ] == '.')) mde = 2;
                    else if ((input [ i ] == 'E')) mde = 1;
                    else if ((input [ i ] == '-') && mde == 1 && exp >= 0) exp *= -1;
                    else
                    {
                        data = -1;
                        return false;
                    }
                }
                data = _data * (Negative ? -1 : 1);
                if (exp > 0)
                    for (int i = 0 ; i < exp ; i++)
                    {
                        data *= 10f;
                    }
                else for (int i = 0 ; i > exp ; i--)
                    {
                        data /= 10f;
                    }
                return true;
            }
        }
        /// <summary>
        /// Parse a string to a double.
        /// <br/>
        /// Accepted format: scientific notation.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool TryParse(this string input , out double data)
        {

            bool Negative = input.StartsWith("-");
            input = input.ToUpper();
            int Start = 0;
            int Length = input.Length;
            if (Negative)
            {
                Start = 1;
            }
            if (input.EndsWith("D"))
            {
                Length -= 1;
            }
            {

                long _data = 0;
                int exp = 0;
                short mde = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    if (mde == 0)
                        _data *= 10;
                    if (mde == 1)
                        exp *= 10;
                    if ((input [ i ] >= '0' && input [ i ] <= '9'))
                    {
                        if (mde == 0)
                            _data += (input [ i ] - '0');
                        if (mde == 2)
                        {
                            _data += (input [ i ] - '0');
                            exp += 1;
                        }
                        if (mde == 1)
                            exp += (input [ i ] - '0');
                    }
                    else if ((input [ i ] == '_')) continue;
                    else if ((input [ i ] == '.')) mde = 2;
                    else if ((input [ i ] == 'E')) mde = 1;
                    else if ((input [ i ] == '-') && mde == 1 && exp >= 0) exp *= -1;
                    else
                    {
                        data = -1;
                        return false;
                    }
                }
                data = _data * (Negative ? -1 : 1);
                if (exp > 0)
                    for (int i = 0 ; i < exp ; i++)
                    {
                        data *= 10f;
                    }
                else for (int i = 0 ; i > exp ; i--)
                    {
                        data /= 10f;
                    }
                return true;
            }
        }
        /// <summary>
        /// Parse a string to an unsigned int32.
        /// <br/>
        /// Accepted format: 0x, 0o, 0b and scientific notation. As well as ending with `u`
        /// </summary>
        /// <param name="input"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool TryParse(this string input , out uint data)
        {

            bool Negative = input.StartsWith("-");
            if (Negative)
            {
                data = 0;
                return false;
            }
            input = input.ToUpper();
            int Start = 0;
            int Length = input.Length;
            if (Negative)
            {
                Start = 1;
            }
            if (input.EndsWith("L"))
            {
                data = 0;
                return false;
            }
            if (input.EndsWith("U"))
            {
                Length -= 1;
            }
            if (input.StartsWith("0X", Start))
            {
                Start += 2;
                uint _data = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    _data *= 16;
                    if ((input [ i ] >= '0' && input [ i ] <= '9'))
                    {
                        _data += (uint)input [ i ] - '0';
                    }
                    else
                    if ((input [ i ] >= 'A' && input [ i ] <= 'F'))
                    {
                        _data += (uint)input [ i ] - 'A' + 10;
                    }
                    else if ((input [ i ] == '_')) continue;
                    else
                    {
                        data = 0;
                        return false;
                    }
                }
                data = _data;
                return true;
            }
            else if (input.StartsWith("0O", Start))
            {
                Start += 2;
                uint _data = 8;
                for (int i = Start ; i < Length ; i++)
                {
                    _data *= 16;
                    if ((input [ i ] >= '0' && input [ i ] <= '7'))
                    {
                        _data += (uint)input [ i ] - '0';
                    }
                    else if ((input [ i ] == '_')) continue;
                    else
                    {
                        data = 0;
                        return false;
                    }
                }
                data = _data;
                return true;
            }
            else
            if (input.StartsWith("0B", Start))
            {
                Start += 2;
                uint _data = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    _data *= 2;
                    if (input [ i ] == '0')
                    {
                        _data += 0;
                    }
                    else
                    if (input [ i ] == '1')
                    {
                        _data += 1;
                    }
                    else if ((input [ i ] == '_')) continue;
                    else
                    {
                        data = 0;
                        return false;
                    }
                }
                data = _data;
                return true;
            }
            else
            {

                uint _data = 0;
                int exp = 0;
                byte mde = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    if (mde == 0)
                        _data *= 10;
                    if (mde == 1)
                        exp *= 10;
                    if ((input [ i ] >= '0' && input [ i ] <= '9'))
                    {
                        if (mde == 0)
                            _data += (uint)(input [ i ] - '0');
                        if (mde == 1)
                            exp += (input [ i ] - '0');
                    }
                    else if ((input [ i ] == '_')) continue;
                    else if ((input [ i ] == 'E')) mde = 1;
                    else if ((input [ i ] == '-') && mde == 1 && exp >= 0) exp *= -1;
                    else
                    {
                        data = 0;
                        return false;
                    }
                }
                if (exp > 0)
                    for (int i = 0 ; i < exp ; i++)
                    {
                        _data *= 10;
                    }
                else for (int i = 0 ; i > exp ; i--)
                    {
                        _data /= 10;
                    }
                data = _data;
                return true;
            }
        }

        /// <summary>
        /// Parse a string to an int64.
        /// <br/>
        /// Accepted format: 0x, 0o, 0b and scientific notation.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool TryParse(this string input , out long data)
        {

            bool Negative = input.StartsWith("-");
            input = input.ToUpper();
            int Start = 0;
            int Length = input.Length;
            if (Negative)
            {
                Start = 1;
            }
            if (input.EndsWith("L"))
            {
                Length -= 1;
            }
            if (input.StartsWith("0X", Start))
            {
                Start += 2;
                long _data = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    _data *= 16;
                    if ((input [ i ] >= '0' && input [ i ] <= '9'))
                    {
                        _data += input [ i ] - '0';
                    }
                    else
                    if ((input [ i ] >= 'A' && input [ i ] <= 'F'))
                    {
                        _data += input [ i ] - 'A' + 10;
                    }
                    else if ((input [ i ] == '_')) continue;
                    else
                    {
                        data = -1;
                        return false;
                    }
                }
                data = _data * (Negative ? -1 : 1);
                return true;
            }
            else if (input.StartsWith("0O", Start))
            {
                Start += 2;
                long _data = 8;
                for (int i = Start ; i < Length ; i++)
                {
                    _data *= 16;
                    if ((input [ i ] >= '0' && input [ i ] <= '7'))
                    {
                        _data += input [ i ] - '0';
                    }
                    else if ((input [ i ] == '_')) continue;
                    else
                    {
                        data = -1;
                        return false;
                    }
                }
                data = _data * (Negative ? -1 : 1);
                return true;
            }
            else
            if (input.StartsWith("0B", Start))
            {
                Start += 2;
                long _data = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    _data *= 2;
                    if (input [ i ] == '0')
                    {
                        _data += 0;
                    }
                    else
                    if (input [ i ] == '1')
                    {
                        _data += 1;
                    }
                    else if ((input [ i ] == '_')) continue;
                    else
                    {
                        data = -1;
                        return false;
                    }
                }
                data = _data * (Negative ? -1 : 1);
                return true;
            }
            else
            {

                long _data = 0;
                int exp = 0;
                byte mde = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    if (mde == 0)
                        _data *= 10;
                    if (mde == 1)
                        exp *= 10;
                    if ((input [ i ] >= '0' && input [ i ] <= '9'))
                    {
                        if (mde == 0)
                            _data += (input [ i ] - '0');
                        if (mde == 1)
                            exp += (input [ i ] - '0');
                    }
                    else if ((input [ i ] == '_')) continue;
                    else if ((input [ i ] == 'E')) mde = 1;
                    else if ((input [ i ] == '-') && mde == 1 && exp >= 0) exp *= -1;
                    else
                    {
                        data = -1;
                        return false;
                    }
                }
                if (exp > 0)
                    for (int i = 0 ; i < exp ; i++)
                    {
                        _data *= 10;
                    }
                else for (int i = 0 ; i > exp ; i--)
                    {
                        _data /= 10;
                    }
                data = _data * (Negative ? -1 : 1);
                return true;
            }
        }
        /// <summary>
        /// Parse a string to an int64.
        /// <br/>
        /// Accepted format: 0x, 0o, 0b and scientific notation.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool TryParse(this string input , out ulong data)
        {

            bool Negative = input.StartsWith("-");
            input = input.ToUpper();
            int Start = 0;
            int Length = input.Length;
            if (Negative)
            {
                Start = 1;
            }
            if (input.EndsWith("UL"))
            {
                Length -= 2;
            }
            else if (input.EndsWith("L"))
            {
                data = 0;
                return false;
            }
            if (input.StartsWith("0X", Start))
            {
                Start += 2;
                ulong _data = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    _data *= 16;
                    if ((input [ i ] >= '0' && input [ i ] <= '9'))
                    {
                        _data += (ulong)input [ i ] - '0';
                    }
                    else
                    if ((input [ i ] >= 'A' && input [ i ] <= 'F'))
                    {
                        _data += (ulong)input [ i ] - 'A' + 10;
                    }
                    else if ((input [ i ] == '_')) continue;
                    else
                    {
                        data = 0;
                        return false;
                    }
                }
                data = _data;
                return true;
            }
            else if (input.StartsWith("0O", Start))
            {
                Start += 2;
                ulong _data = 8;
                for (int i = Start ; i < Length ; i++)
                {
                    _data *= 16;
                    if ((input [ i ] >= '0' && input [ i ] <= '7'))
                    {
                        _data += (ulong)input [ i ] - '0';
                    }
                    else if ((input [ i ] == '_')) continue;
                    else
                    {
                        data = 0;
                        return false;
                    }
                }
                data = _data;
                return true;
            }
            else
            if (input.StartsWith("0B", Start ))
            {
                Start += 2;
                ulong _data = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    _data *= 2;
                    if (input [ i ] == '0')
                    {
                        _data += 0;
                    }
                    else
                    if (input [ i ] == '1')
                    {
                        _data += 1;
                    }
                    else if ((input [ i ] == '_')) continue;
                    else
                    {
                        data = 0;
                        return false;
                    }
                }
                data = _data;
                return true;
            }
            else
            {

                ulong _data = 0;
                int exp = 0;
                byte mde = 0;
                for (int i = Start ; i < Length ; i++)
                {
                    if (mde == 0)
                        _data *= 10;
                    if (mde == 1)
                        exp *= 10;
                    if ((input [ i ] >= '0' && input [ i ] <= '9'))
                    {
                        if (mde == 0)
                            _data += (ulong)(input [ i ] - '0');
                        if (mde == 1)
                            exp += (input [ i ] - '0');
                    }
                    else if ((input [ i ] == '_')) continue;
                    else if ((input [ i ] == 'E')) mde = 1;
                    else if ((input [ i ] == '-') && mde == 1 && exp >= 0) exp *= -1;
                    else
                    {
                        data = 0;
                        return false;
                    }
                }
                if (exp > 0)
                    for (int i = 0 ; i < exp ; i++)
                    {
                        _data *= 10;
                    }
                else for (int i = 0 ; i > exp ; i--)
                    {
                        _data /= 10;
                    }
                data = _data;
                return true;
            }
        }
    }
}
