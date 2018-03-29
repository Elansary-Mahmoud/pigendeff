using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace porc_Test
{
    class NumericComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            string left = (string)x;
            string right = (string)y;
            int max = Math.Min(left.Length, right.Length);
            for (int i = 0; i < max; i++)
            {
                if (left[i] != right[i])
                {
                    return left[i] - right[i];
                }
            }
            return left.Length - right.Length;
        }
    }
}
