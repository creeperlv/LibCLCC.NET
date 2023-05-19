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
        /// <param name="Accept_f">If accept xxx.xxxf</param>
        /// <param name="AllowNoInteger">If accept .xxx</param>
        public static void ScanFloatPoint(ref Segment HEAD , bool Accept_f = true , bool AllowNoInteger = true)
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
                    if (long.TryParse(Cur.Prev.content , out var integer_part))
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
                    if (long.TryParse(R.content , out var dec_part))
                    {
                        formation += "." + R.content;
                    }
                    else
                    {
                        var pes_dec = Cur.Next.content;
                        if (pes_dec.EndsWith("f") && Accept_f)
                        {
                            pes_dec = pes_dec.Substring(0 , pes_dec.Length - 1);
                            if (long.TryParse(pes_dec , out dec_part))
                            {

                                formation += "." + Cur.Next.content;
                            }
                            else
                            {

                                Cur = Cur.Next;
                                continue;
                            }
                        }
                        else
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
                }
            }
        }
    }
}
