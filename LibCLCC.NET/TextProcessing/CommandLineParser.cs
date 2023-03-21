using System.Collections.Generic;

namespace LibCLCC.NET.TextProcessing {
    /// <summary>
    /// GeneralPurposeParser adjusted for command-line.
    /// </summary>
    public class CommandLineParser : GeneralPurposeParser {
        /// <summary>
        /// Initialize configuration.
        /// </summary>
        public CommandLineParser() {
            PredefinedSegmentCharacters = new List<char>();
            PredefinedSegmentTemplate = new List<string>();
        }
    }
}
