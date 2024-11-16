using System;

namespace Utility
{
    public static class IntExtensions
    {
        public static int IndexOfLeftMostBit(this UInt16 s)
        {
            int i = -1;
            while (s > 0)
            {
                i++;
                s =  (UInt16)(s >> 1);
            }

            return i;

        }
        
        public static byte SplitInterleaved(this UInt16 s, bool first)
        {
            byte i = 0;
            if (!first)
            {
                s = (UInt16) (s >> 1);
            }

            for (int p = 0; p < 8; p++)
            {
                if ((s & 1) > 0) i |= (byte) (1 << p);
                s = (UInt16) (s >> 2);
            }

            return i;
        }
        
        
        /// <summary>
        /// a function that divides a float 0–1 based on a byte. by mainaining as much distance as possible, favoring distance between lower numbers.
        /// never returns 0;
        /// </summary>
        public static float ByteToFloatSplit(this byte b) 
        {
            if (b == 0) return 1;
            var significant = ((ushort)b).IndexOfLeftMostBit();
            var div = (byte)( 1 << significant);
            b = (byte)(b & ~div);
            return (b + 0.5f) / div ;
        }
    }
}