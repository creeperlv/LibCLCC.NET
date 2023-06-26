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
            "==",  "<=", ">=", "=>", ">>", "<<","&&","||","!=","->","++","--"
            };
            PredefinedSegmentCharacters = new List<char> {
            '[', ']', '(', ')', '{', '}','.',',',';','&','|','<','>','/','+','-','*','%','#','?','!',':','~','^','='
            };
        }
        /// <summary>
        /// Scan segments, then perform negative number scan and ExponentialNumberScan.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="DisableCommentChecker"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public override Segment Scan(string str , bool DisableCommentChecker , string ID = null)
        {
            var HEAD = base.Scan(str , DisableCommentChecker , ID);
            FloatPointScanner.ScanFloatPoint(ref HEAD , "fdeFDE" , true);
            this.NegativeNumberScan(ref HEAD);
            this.ExponentialNumberScan(ref HEAD);
            return HEAD;
        }
    }
}
