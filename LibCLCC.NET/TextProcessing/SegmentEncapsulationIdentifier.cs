namespace LibCLCC.NET.TextProcessing {
    /// <summary>
    /// Identifier of encapsulation, like : "content"
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        public SegmentEncapsulationIdentifier(char L, char R) { this.L = L; this.R = R; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public SegmentEncapsulationIdentifier(char c) { this.L = c; this.R = c; }
    }
}
