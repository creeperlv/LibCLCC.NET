using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibCLCC.NET.TextProcessing {
    /// <summary>
    /// General purpose parser. If you are building a compiler against it, you may need a second-pass parser to convert operators.
    /// </summary>
    public class GeneralPurposeParser {
        /// <summary>
        /// Blank Splitters
        /// </summary>
        public char[] Splitters = new char[] { ' ', '\t', '\r', '\n' };
        /// <summary>
        /// Works for operators. e.g.: ==, --
        /// </summary>
        public List<string> PredefinedSegmentTemplate = new List<string> {
            "==", "--", "++", "<=", ">=", "=>", ">>", "<<"
        };
        public List<char> PredefinedSegmentCharacters = new List<char> {
            '[', ']', '(', ')', '{', '}','.',',',';'
        };
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
        /// Parse segment
        /// </summary>
        /// <param name="str"></param>
        /// <param name="DisableCommentChecker"></param>
        /// <returns></returns>
        public Segment Parse(string str, bool DisableCommentChecker) {
            Segment root = new Segment();
            Segment current = root;
            bool isSegmentEncapsulation = false;
            string attention = "";
            SegmentEncapsulationIdentifier segmentEncapsulationIdentifier = null;
            LineCommentIdentifier CurrentLCI = null;
            ClosableCommentIdentifier CurrentCCI = null;
            using (StringReader sr = new StringReader(str)) {
                while (true) {
                    int b8 = sr.Read();
                    if (b8 == -1) break;
                    char c = (char)b8;
                    if (CurrentLCI != null) {
                        if (c == '\r' || c == '\n') {
                            CurrentLCI = null;
                            continue;
                        }
                    }
                    else if (CurrentCCI != null) {
                        attention += c;
                        if (CurrentCCI.End.StartsWith(attention)) {
                            if (CurrentCCI.End == attention) {
                                CurrentCCI = null;
                                continue;
                            }
                        }
                        else {
                            attention = "";
                        }
                    }
                    if (isSegmentEncapsulation) {
                        if (c == segmentEncapsulationIdentifier.R) {
                            segmentEncapsulationIdentifier = null;
                            NewSegment();
                        }
                        else {

                            current.content += c;
                        }
                    }
                    else {
                        if (Splitters.Contains<char>(c)) {
                            /**
                             * 
                             * **/
                            if (current.content.Length > 0) {
                                NewSegment();

                            }
                        }
                        else if (PredefinedSegmentCharacters.Contains(c)) {
                            if (current.content.Length > 0) {
                                NewSegment();
                            }
                            {
                                current.content += c;
                                NewSegment();
                            }
                        }
                        else {
                            current.content += c;
                            bool Hit = false;
                            {

                                foreach (var item in lineCommentIdentifiers) {
                                    if (item.StartSequence.Length > attention.Length)
                                        if (item.StartSequence[attention.Length] == c) {
                                            attention += c;
                                            Hit = true;
                                        }
                                    if (item.StartSequence == attention) {
                                        //Comment Started.
                                        current.content = current.content.Substring(0, Math.Max(0, current.content.Length - attention.Length));
                                        attention = "";
                                        CurrentLCI = item;
                                        Hit = true;
                                        NewSegment();
                                        break;
                                    }
                                }
                            }
                            if (!Hit) {

                                foreach (var item in closableCommentIdentifiers) {
                                    if (item.Start.Length > attention.Length)
                                        if (item.Start[attention.Length] == c) {
                                            attention += c;
                                            Hit = true;
                                        }
                                    if (item.Start == attention) {
                                        //Comment Started.
                                        current.content = current.content.Substring(0, Math.Max(0, current.content.Length - attention.Length));
                                        attention = "";
                                        CurrentCCI = item;
                                        Hit = true;
                                        NewSegment();
                                        break;
                                    }
                                }
                            }
                            if (!Hit) {
                                foreach (var item in SegmentEncapsulationIdentifiers) {
                                    if (item.L == c) {
                                        if (current.content.Length > 0)
                                            current.content = current.content.Substring(0, current.content.Length - 1);
                                        if (current.content.Length > 0) {
                                            segmentEncapsulationIdentifier = item;
                                            NewSegment();
                                            current.EncapsulationIdentifier = segmentEncapsulationIdentifier;
                                            current.isEncapsulated = true;
                                        }
                                        break;
                                    }
                                }
                            }
                            if (!Hit) {
                                foreach (var item in PredefinedSegmentTemplate) {
                                    if (item.Length > attention.Length)
                                        if (item[attention.Length] == c) {
                                            attention += c;
                                            Hit = true;
                                        }
                                    if (item == attention) {
                                        if (current.content == item) {
                                            NewSegment();
                                        }
                                        else {
                                            current.content = current.content.Substring(0, Math.Max(0, current.content.Length - attention.Length));
                                            attention = "";
                                            Hit = true;
                                            NewSegment();
                                            current.content = item;
                                            NewSegment();
                                        }
                                    }
                                }
                            }
                            if (!Hit) attention = "";
                        }

                    }
                }

            }
            void CloseSegment() {

            }
            void NewSegment() {
                Segment segment = new Segment();
                current.Next = segment;
                segment.Prev = current;
                current = segment;
            }
            return root;
        }
    }
}
