using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace riat_l2
{
    public class Input
    {
        public int K {get; set;}
        public decimal[] Sums {get; set;}
        public int[] Muls {get; set;}

        public Output CreateOutput()
        {
            return new Output
            {
                SumResult = Sums.Sum()*K,
                MulResult = Muls.Aggregate((a, b) => a*b),
                SortedInputs = Sums.Concat(Muls.Select(a => (decimal) a)).OrderBy(x => x).ToArray()
            };
        }
    }
}
