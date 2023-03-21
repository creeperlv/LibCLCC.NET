namespace LibCLCC.NET.TextProcessing {
    /// <summary>
    /// One segment
    /// </summary>
    public class Segment {
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
        public string content;
        /// <summary>
        /// Is Encapsulated
        /// </summary>
        public bool isEncapsulated;
        /// <summary>
        /// The encapsulation character.
        /// </summary>
        public SegmentEncapsulationIdentifier EncapsulationIdentifier;
    }
}
