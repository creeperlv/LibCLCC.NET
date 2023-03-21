using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LibCLCC.NET.TextProcessing {
    /// <summary>
    /// General purpose parser. If you are building a compiler against it, you may need a second-pass parser to convert operators.
    /// </summary>
    public class Parser {
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
            '[', ']', '(', ')', '{', '}'
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
            ClosableCommentIdentifier closableCommentIdentifier = null;
            using (StreamReader sr = new StreamReader(str)) {
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
                    current.content += b8;
                    if (isSegmentEncapsulation) {
                        if (c == segmentEncapsulationIdentifier.R) {
                            segmentEncapsulationIdentifier = null;
                            NewSegment();
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
                        else {
                            bool Hit = false;
                            {

                                foreach (var item in lineCommentIdentifiers) {
                                    if (item.StartSequence == attention) {
                                        //Comment Started.
                                        current.content = current.content.Substring(current.content.Length - attention.Length);
                                        attention = "";
                                        CurrentLCI = item;
                                        Hit = true;
                                        NewSegment();
                                        break;
                                    }
                                }
                            }
                            if (!Hit)
                                foreach (var item in SegmentEncapsulationIdentifiers) {
                                    if (item.L == c) {
                                        if (current.content.Length > 0) {
                                            segmentEncapsulationIdentifier = item;
                                            NewSegment();
                                            current.EncapsulationIdentifier = segmentEncapsulationIdentifier;
                                            current.isEncapsulated = true;
                                        }
                                        break;
                                    }
                                }
                            if (!Hit)
                                foreach (var item in PredefinedSegmentTemplate) {
                                    if (item[attention.Length] == c) {
                                        attention += c;
                                    }
                                    if (item == attention) {
                                        /**
                                         * Separate segment
                                         * **/
                                        CloseSegment();
                                    }
                                }
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
