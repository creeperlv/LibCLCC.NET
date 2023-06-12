using LibCLCC.NET.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace LibCLCC.NET.TextProcessing
{
    /// <summary>
    /// General purpose scanner. If you are building a compiler against it, you may need a second-pass scanner to convert operators.
    /// </summary>
    public class GeneralPurposeScanner
    {
        /// <summary>
        /// Blank Splitters
        /// </summary>
        public char [ ] Splitters = new char [ ] { ' ' , '\t' , '\r' , '\n' };
        /// <summary>
        /// Works for operators. e.g.: ==, --
        /// </summary>
        public List<string> PredefinedSegmentTemplate = new List<string> { };
        /// <summary>
        /// Predefined Segment Characters.
        /// </summary>
        public List<char> PredefinedSegmentCharacters = new List<char> { };
        /// <summary>
        /// Encapsulation, like: '' , ""
        /// </summary>
        public List<SegmentEncapsulationIdentifier> SegmentEncapsulationIdentifiers = new List<SegmentEncapsulationIdentifier> {
            new SegmentEncapsulationIdentifier('\''),
            new SegmentEncapsulationIdentifier('\"')
        };
        /// <summary>
        /// Comments: //, #
        /// </summary>
        public List<LineCommentIdentifier> lineCommentIdentifiers = new List<LineCommentIdentifier>();
        /// <summary>
        /// Comments: /****/ 
        /// </summary>
        public List<ClosableCommentIdentifier> closableCommentIdentifiers = new List<ClosableCommentIdentifier>();
        /// <summary>
        /// Second stage parse for predefined identifiers;
        /// </summary>
        /// <param name="HEAD"></param>
        [Obsolete]
        public void SecondStageParse(ref Segment HEAD)
        {
            SecondStageScan(ref HEAD);
        }
        /// <summary>
        /// Scan for negative numbers;
        /// </summary>
        /// <param name="HEAD"></param>
        public void NegativeNumberScan(ref Segment HEAD)
        {
            Segment Cur = HEAD;
            while (true)
            {
                if (Cur == null)
                {
                    break;
                }
                if (Cur.content == "")
                {
                    if (Cur.Next == null)
                    {
                        break;
                    }
                }
                if (Cur.isEncapsulated == false)
                {
                    if (Cur.content != null)
                    {
                        if (Cur.content.TryParse(out long _) || Cur.content.TryParse(out ulong _)
                            || Cur.content.TryParse(out uint _) || Cur.content.TryParse(out int _)
                            || Cur.content.TryParse(out float _) || Cur.content.TryParse(out double _))
                        {
                            if (Cur.Prev != null)
                            {
                                if (Cur.Prev.content == "-")
                                {
                                    if (Cur.Prev.Prev != null)
                                    {
                                        var Attention = Cur.Prev.Prev;
                                        var Hit = false;
                                        foreach (var item in PredefinedSegmentCharacters)
                                        {
                                            if (Attention.content == $"{item}")
                                            {
                                                Hit = true;
                                                break;
                                            }
                                        }
                                        foreach (var item in PredefinedSegmentTemplate)
                                        {
                                            if (Attention.content == item)
                                            {
                                                Hit = true;
                                                break;
                                            }
                                        }
                                        if (Hit)
                                        {
                                            Cur.content = $"-{Cur.content}";
                                            Attention.Next = Cur;
                                            Cur.Prev = Attention;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                Cur = Cur.Next;
            }
        }
        /// <summary>
        /// Scan for numbers with scientific notation.
        /// </summary>
        /// <param name="HEAD"></param>
        public void ExponentialNumberScan(ref Segment HEAD)
        {
            Segment Cur = HEAD;
            while (true)
            {
                if (Cur == null)
                {
                    break;
                }
                if (Cur.content == "")
                {
                    if (Cur.Next == null)
                    {
                        break;
                    }
                }
                if (Cur.isEncapsulated == false)
                {
                    if (Cur.content != null)
                    {
                        if (Cur.content.TryParse(out long _) || Cur.content.TryParse(out ulong _)
                            || Cur.content.TryParse(out uint _) || Cur.content.TryParse(out int _)
                            || Cur.content.TryParse(out float _) || Cur.content.TryParse(out double _))
                        {
                            if (Cur.content.EndsWith("e") || Cur.content.EndsWith("E"))
                            {
                                Segment candidate = Cur;
                                Cur = Cur.Next;
                                bool Hit = false;
                                if (Cur.content == "-")
                                {
                                    Cur = Cur.Next;

                                    if (Cur.content.TryParse(out long _) || Cur.content.TryParse(out ulong _)
                                        || Cur.content.TryParse(out uint _) || Cur.content.TryParse(out int _))
                                    {
                                        if (!Cur.content.StartsWith("-"))
                                        {
                                            Hit = true;
                                            candidate.content += "-" + Cur.content;
                                        }
                                    }
                                }
                                if (!Hit)
                                    if (Cur.content.TryParse(out long _)
                                        || Cur.content.TryParse(out ulong _)
                                        || Cur.content.TryParse(out uint _)
                                        || Cur.content.TryParse(out int _))
                                    {
                                        Hit = true;
                                        candidate.content += Cur.content;

                                    }
                                if (Hit)
                                {
                                    var Tail = Cur.Next;
                                    candidate.Next = Tail;
                                    Tail.Prev = candidate;

                                }
                            }
                        }
                    }
                }
                Cur = Cur.Next;
            }
        }
        /// <summary>
        /// Second stage scan for predefined identifiers;
        /// </summary>
        /// <param name="HEAD"></param>
        public void SecondStageScan(ref Segment HEAD)
        {

            Segment Cur = HEAD;
            string attention = "";
            Segment AttSeg = Cur;
            while (true)
            {
                if (Cur == null)
                {
                    break;
                }
                if (Cur.content == "")
                {
                    if (Cur.Next == null)
                    {
                        break;
                    }
                }
                if (attention == "")
                {
                    AttSeg = Cur;
                }
                attention += Cur.content;
                bool Hit = false;
                foreach (var item in PredefinedSegmentTemplate)
                {
                    if (item == attention)
                    {
                        Hit = true;
                        AttSeg.content = item;
                        AttSeg.Next = Cur.Next;
                        attention = "";
                        if (Cur.Next != null)
                        {
                            Cur.Next.Prev = AttSeg;
                        }
                        break;
                    }
                    if (item.StartsWith(attention))
                    {
                        Hit = true;
                        break;
                    }
                }
                if (Hit == false)
                {
                    attention = "";
                }
                Cur = Cur.Next;
            }
        }
        /// <summary>
        /// Parse segment
        /// </summary>
        /// <param name="str"></param>
        /// <param name="DisableCommentChecker"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [Obsolete]
        public Segment Parse(string str , bool DisableCommentChecker , string ID = null)
        {
            return Scan(str , DisableCommentChecker , ID);
        }
        /// <summary>
        /// Scan segment
        /// </summary>
        /// <param name="str"></param>
        /// <param name="DisableCommentChecker"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public virtual Segment Scan(string str , bool DisableCommentChecker , string ID = null)
        {
            Segment root = new Segment { ID = ID };
            Segment current = root;
            bool isSegmentEncapsulation = false;
            string attention = "";
            SegmentEncapsulationIdentifier segmentEncapsulationIdentifier = null;
            LineCommentIdentifier CurrentLCI = null;
            ClosableCommentIdentifier CurrentCCI = null;
            int Line = 0;
            int sid = 0;
            using (StringReader sr = new StringReader(str))
            {
                while (true)
                {
                    int b8 = sr.Read();
                    if (b8 == -1) break;
                    char c = (char)b8;
                    if (CurrentLCI != null)
                    {
                        if (c == '\r')
                        {
                            if (sr.Peek() == '\n')
                            {
                                sr.Read();
                            }
                            Line++;
                            current.LineNumber = Line;
                            CurrentLCI = null;
                            continue;
                        }
                        if (c == '\n')
                        {
                            Line++;
                            current.LineNumber = Line;
                            CurrentLCI = null;
                            continue;
                        }
                        continue;
                    }
                    else if (CurrentCCI != null)
                    {
                        if (c == '\r')
                        {
                            if (sr.Peek() == '\n')
                            {
                                sr.Read();
                            }
                            Line++;
                            current.LineNumber = Line;
                            attention = "";
                            continue;
                        }
                        if (c == '\n')
                        {
                            Line++;
                            current.LineNumber = Line;
                            attention = "";
                            continue;
                        }
                        attention += c;
                        if (CurrentCCI.End.StartsWith(attention))
                        {
                            if (CurrentCCI.End == attention)
                            {
                                CurrentCCI = null;
                                continue;
                            }
                        }
                        else
                        {
                            attention = "" + c;
                        }
                        continue;
                    }
                    if (isSegmentEncapsulation)
                    {
                        if (c == segmentEncapsulationIdentifier.R)
                        {
                            segmentEncapsulationIdentifier = null;
                            isSegmentEncapsulation = false;
                            NewSegment(Line);
                        }
                        else
                        {

                            current.content += c;
                        }
                        continue;
                    }
                    else
                    {
                        if (Splitters.Contains<char>(c))
                        {
                            bool isNewLine = false;
                            if (c == ('\r'))
                            {
                                isNewLine = true;
                                if (sr.Peek() == '\n')
                                {
                                    sr.Read();
                                }
                            }
                            else if (c == ('\n'))
                            {
                                isNewLine = true;
                            }
                            if (isNewLine)
                            {
                                Line++;
                            }
                            if (current.content.Length > 0)
                            {
                                NewSegment(Line);

                            }
                        }
                        else if (PredefinedSegmentCharacters.Contains(c))
                        {
                            if (current.content.Length > 0)
                            {
                                NewSegment(Line);
                            }
                            {
                                current.isEncapsulated = false;
                                current.content += c;
                                NewSegment(Line);
                            }
                        }
                        else
                        {
                            current.content += c;
                            bool Hit = false;
                            {

                                foreach (var item in lineCommentIdentifiers)
                                {
                                    if (item.StartSequence.Length > attention.Length)
                                        if (item.StartSequence [ attention.Length ] == c)
                                        {
                                            attention += c;
                                            Hit = true;
                                        }
                                    if (item.StartSequence == attention)
                                    {
                                        //Comment Started.
                                        current.content = current.content.Substring(0 , Math.Max(0 , current.content.Length - attention.Length));
                                        attention = "";
                                        CurrentLCI = item;
                                        Hit = true;
                                        if (current.content != "")
                                        {
                                            NewSegment(Line);
                                        }
                                        break;
                                    }
                                }
                            }
                            if (!Hit)
                            {

                                foreach (var item in closableCommentIdentifiers)
                                {
                                    if (item.Start.Length > attention.Length)
                                        if (item.Start [ attention.Length ] == c)
                                        {
                                            attention += c;
                                            Hit = true;
                                        }
                                    if (item.Start == attention)
                                    {
                                        //Comment Started.
                                        current.content = current.content.Substring(0 , Math.Max(0 , current.content.Length - attention.Length));
                                        CurrentCCI = item;
                                        Hit = true;
                                        if (current.content != "")
                                        {
                                            NewSegment(Line);
                                        }
                                        attention = "";
                                        break;
                                    }
                                }
                            }
                            if (!Hit)
                            {
                                foreach (var item in SegmentEncapsulationIdentifiers)
                                {
                                    if (item.L == c)
                                    {
                                        if (current.content.Length > 0)
                                            current.content = current.content.Substring(0 , current.content.Length - 1);
                                        {
                                            segmentEncapsulationIdentifier = item;
                                            isSegmentEncapsulation = true;
                                            if (current.content.Length > 0)
                                                NewSegment(Line);
                                            current.EncapsulationIdentifier = segmentEncapsulationIdentifier;
                                            current.isEncapsulated = true;
                                            Hit = true;
                                            attention = "";
                                        }
                                        break;
                                    }
                                }
                            }
                            //if (!Hit)
                            //{
                            //    foreach (var item in PredefinedSegmentTemplate)
                            //    {
                            //        if (item.Length > attention.Length)
                            //            if (item[attention.Length] == c)
                            //            {
                            //                attention += c;
                            //                Hit = true;
                            //            }
                            //        if (item == attention)
                            //        {
                            //            if (current.content == item)
                            //            {
                            //                NewSegment(Line);
                            //            }
                            //            else
                            //            {
                            //                current.content = current.content.Substring(0, Math.Max(0, current.content.Length - attention.Length));
                            //                attention = "";
                            //                Hit = true;
                            //                NewSegment(Line);
                            //                current.content = item;
                            //                NewSegment(Line);
                            //            }
                            //            break;
                            //        }
                            //    }
                            //}
                            if (!Hit) attention = "";
                        }

                    }
                }

            }
            //current.Prev.Next = null;
            //current.Prev = null;
            void NewSegment(int LineNumber)
            {
                Segment segment = new Segment { ID = ID };
                segment.Index = sid;
                sid++;
                current.Next = segment;
                segment.LineNumber = LineNumber;
                segment.Prev = current;
                current = segment;
            }
            SecondStageScan(ref root);
            return root;
        }
    }
}
