﻿using System.Collections.Generic;

namespace LibCLCC.NET.TextProcessing {
    /// <summary>
    /// A c-style parser.
    /// </summary>
    public class CStyleParser : GeneralPurposeParser {
        /// <summary>
        /// Initialize configuration.
        /// </summary>
        public CStyleParser() {
            lineCommentIdentifiers = new List<LineCommentIdentifier> { new LineCommentIdentifier { StartSequence = "//" } };
            closableCommentIdentifiers = new List<ClosableCommentIdentifier> { new ClosableCommentIdentifier { Start = "/*", End = "*/" } };
            PredefinedSegmentTemplate = new List<string> {
            "==", "--", "++", "<=", ">=", "=>", ">>", "<<"
            };
            PredefinedSegmentCharacters = new List<char> {
            '[', ']', '(', ')', '{', '}','.',',',';'
            };
        }
    }
}
