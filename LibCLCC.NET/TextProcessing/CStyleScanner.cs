using System.Collections.Generic;

namespace LibCLCC.NET.TextProcessing
{
    /// <summary>
    /// A c-style canner.
    /// </summary>
    public class CStyleScanner : GeneralPurposeScanner
    {
        /// <summary>
        /// Initialize configuration.
        /// </summary>
        public CStyleScanner()
        {
            lineCommentIdentifiers = new List<LineCommentIdentifier> { new LineCommentIdentifier { StartSequence = "//" } };
            closableCommentIdentifiers = new List<ClosableCommentIdentifier> { new ClosableCommentIdentifier { Start = "/*" , End = "*/" } };
            PredefinedSegmentTemplate = new List<string> {
            "==", "--", "++", "<=", ">=", "=>", ">>", "<<","&&","||","!=","->"
            };
            PredefinedSegmentCharacters = new List<char> {
            '[', ']', '(', ')', '{', '}','.',',',';','&','|','<','>','/','+','-','*','%','#','?','!',':','~','^','='
            };
        }
    }
}
