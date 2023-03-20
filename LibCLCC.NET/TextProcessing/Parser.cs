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
        /// Works for operators. e.g.: ==, --
        /// </summary>
        public char[] Splitters = new char[] { ' ', '\t', '\r', '\n' };
        public List<string> PredefinedSegmentTemplate = new List<string>();
        public List<char> SegmentEncapsulationIdentifiers = new List<char>();
        public List<LineCommentIdentifier> lineCommentIdentifiers = new List<LineCommentIdentifier>();
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
            using (StreamReader sr = new StreamReader(str)) {
                while (true) {
                    int b8 = sr.Read();
                    if (b8 == -1) break;
                    char c = (char)b8;
                    current.content += b8;
                    if (isSegmentEncapsulation) {

                    }
                    else {
                        if (Splitters.Contains<char>(c)) {
                            CloseSegment();
                        }
                        else {
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
            return root;
        }
    }

    /// <summary>
    /// One line comment like // or #
    /// </summary>
    public class LineCommentIdentifier {
        public char[] StartSequence;
    }
    /// <summary>
    /// Closable comment like /* */
    /// </summary>
    public class ClosableCommentIdentifier {
        public string Start;
        public string End;
    }
    /// <summary>
    /// One segment
    /// </summary>
    public class Segment {
        public Segment Prev = null;
        public Segment Next = null;
        public string content;
        public bool isEncapsulated;
        public char EncapsulationCharacter;
    }
}
