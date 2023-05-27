using System.Collections.Generic;

namespace LibCLCC.NET.TextProcessing
{
    /// <summary>
    /// GeneralPurposeParser adjusted for command-line.
    /// </summary>
    public class CommandLineScanner : GeneralPurposeScanner {
        /// <summary>
        /// Initialize configuration.
        /// </summary>
        public CommandLineScanner() {
            PredefinedSegmentCharacters = new List<char>();
            PredefinedSegmentTemplate = new List<string>();
        }
    }
}
