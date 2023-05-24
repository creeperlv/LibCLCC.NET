using System;

namespace LibCLCC.NET.TextProcessing
{
    /// <summary>
    /// One segment
    /// </summary>
    [Serializable]
    public class Segment
    {
        /// <summary>
        /// The ID specified.
        /// </summary>
        public string ID;
        /// <summary>
        /// Index in the source content, added by order it was parsed. There may be blank index number unused in final result for the Second Stage Parse will substitute some segments.
        /// </summary>
        public int Index;
        /// <summary>
        // Line number in the source.
        /// </summary>
        public int LineNumber;
        /// <summary>
        /// Previous segment, null indicates it is the root segment
        /// </summary>
        [NonSerialized]
        public Segment Prev = null;
        /// <summary>
        /// Next Segment
        /// </summary>
        [NonSerialized]
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
        public string SequentialToString(string intermediate = ">" , bool ShowLineNumber = false , bool ShowID = false)
        {
            if (ShowLineNumber == true)
            {
                return (isEncapsulated ? ($"{EncapsulationIdentifier.L}" + (ShowID ? $"({ID})" : "") +
                    $"{LineNumber}:{content}{EncapsulationIdentifier.R}") : ((ShowID ? $"({ID})" : "") + $"{LineNumber}:{content}"))
                    + intermediate + (Next == null ? "" : Next.SequentialToString(intermediate , ShowLineNumber , ShowID));

            }
            else
            {
                return (isEncapsulated ? $"{EncapsulationIdentifier.L}{content}{EncapsulationIdentifier.R}" : content)
                    + intermediate + (Next == null ? "" : Next.SequentialToString(intermediate , ShowLineNumber , ShowID));
            }
        }
        /// <summary>
        /// If L is earlier than R in the parsed content.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static bool operator <(Segment L , Segment R)
        {
            return L.Index < R.Index;
        }
        /// <summary>
        /// If L is older than R in the parsed content.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        /// <returns></returns>
        public static bool operator >(Segment L , Segment R)
        {
            return L.Index > R.Index;
        }
        /// <summary>
        /// Concatenate 2 segment lists. From the end of L and the start of R.
        /// </summary>
        /// <param name="L"></param>
        /// <param name="R"></param>
        public static void Concatenate(Segment L , Segment R)
        {
            if (L.Next == null)
            {
                L.Next = R;
            }
            else
            {
                Concatenate(L.Next , R);
            }
        }
        /// <summary>
        /// Concatenate 2 segment lists. From the end of current and the start of R.
        /// </summary>
        /// <param name="R"></param>
        public void Concatenate(Segment R)
        {
            Concatenate(this , R);
        }
    }
}
