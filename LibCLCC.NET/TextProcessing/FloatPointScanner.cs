namespace LibCLCC.NET.TextProcessing
{
    /// <summary>
    /// Scan and substitute float point segments, such as: 1 - . - 0 -> 1.0; a - . - 0 -> a - .0;
    /// </summary>
    public class FloatPointScanner
    {
        /// <summary>
        /// Scan for float points
        /// </summary>
        /// <param name="HEAD"></param>
        /// <param name="suffixes">Possible suffixes, such as "efdEFD"</param>
        /// <param name="AllowNoInteger">If accept .xxx</param>
        public static void ScanFloatPoint(ref Segment HEAD , string suffixes = "" , bool AllowNoInteger = true)
        {
            Segment Cur = HEAD;
            string formation = "";
            while (true)
            {
                if (Cur.Next == null)
                {
                    break;
                }
                if (Cur.content != ".")
                {

                    Cur = Cur.Next;
                    continue;
                }
                else
                {
                    Segment L = Cur;
                    Segment R = Cur.Next;
                    if (long.TryParse(Cur.Prev.content , out _))
                    {
                        L = L.Prev;
                        formation = Cur.Prev.content;
                    }
                    else
                    {
                        if (AllowNoInteger)
                        {
                            L = Cur;
                        }
                        else
                        {
                            Cur = Cur.Next;
                            continue;
                        }
                    }
                    if (R == null)
                    {
                        break;
                    }

                    if (long.TryParse(R.content , out _))
                    {
                        formation += "." + R.content;
                    }
                    else
                    {
                        bool Hit = false;
                        var pes_dec = Cur.Next.content;
                        foreach (var item in suffixes)
                        {
                            if (pes_dec.EndsWith(item))
                            {
                                Hit = true;
                                pes_dec = pes_dec.Substring(0 , pes_dec.Length - 1);
                                if (long.TryParse(pes_dec , out _))
                                {

                                    formation += "." + Cur.Next.content;
                                }
                                else
                                {

                                    Cur = Cur.Next;
                                    continue;
                                }

                            }
                        }
                        if (!Hit)
                        {
                            Cur = Cur.Next;
                            continue;
                        }
                    }
                    if (L.Prev != null)
                        L.Prev.Next = Cur;
                    else HEAD = Cur;
                    Cur.Prev = L.Prev;
                    Cur.content = formation;
                    if (R.Next != null)
                        R.Next.Prev = Cur;
                    Cur.Next = R.Next;
                    formation = "";
                }
            }
        }
    }
}
