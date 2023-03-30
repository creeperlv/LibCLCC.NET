namespace LibCLCC.NET.TextProcessing
{
    /// <summary>
    /// One segment
    /// </summary>
    public class Segment
    {
        /// <summary>
        // Line number in the source.
        /// </summary>
        public int LineNumber;
        /// <summary>
        /// Previous segment, null indicates it is the root segment
        /// </summary>
        public Segment Prev = null;
        /// <summary>
        /// Next Segment
        /// </summary>
        public Segment Next = null;
        /// <summary>
        /// Content
        /// </summary>
        public string content = "";
        /// <summary>
        /// Is Encapsulated
        /// </summary>
        public bool isEncapsulated = false;
        /// <summary>
        /// The encapsulation character.
        /// </summary>
        public SegmentEncapsulationIdentifier EncapsulationIdentifier;
        /// <summary>
        /// Combined ToString().
        /// </summary>
        /// <returns></returns>
        public string SequentialToString(string intermediate = ">", bool ShowLineNumber = false)
        {
            if (ShowLineNumber == true)
            {
                return (isEncapsulated ? $"{EncapsulationIdentifier.L}{LineNumber}:{content}{EncapsulationIdentifier.R}" : $"{LineNumber}:{content}")
                    + intermediate + (Next == null ? "" : Next.SequentialToString(intermediate, ShowLineNumber));

            }
            else
            {
                return (isEncapsulated ? $"{EncapsulationIdentifier.L}{content}{EncapsulationIdentifier.R}" : content)
                    + intermediate + (Next == null ? "" : Next.SequentialToString(intermediate, ShowLineNumber));
            }
        }
    }
}
