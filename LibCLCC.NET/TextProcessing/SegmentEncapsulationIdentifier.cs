namespace LibCLCC.NET.TextProcessing {
    /// <summary>
    /// 
    /// </summary>
    public class SegmentEncapsulationIdentifier {
        /// <summary>
        /// Left char
        /// </summary>
        public char L;
        /// <summary>
        /// Right char
        /// </summary>
        public char R;
        /// <summary>
        /// 
        /// </summary>
        public SegmentEncapsulationIdentifier() { }
        public SegmentEncapsulationIdentifier(char L, char R) { this.L = L; this.R = R; }
        public SegmentEncapsulationIdentifier(char c) { this.L = c; this.R = c; }
    }
}
