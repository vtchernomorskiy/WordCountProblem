using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordCountUtilities
{
    public class WordCount
    {
        public string Word { get; set; }
        public int Count { get; set; }

        public override bool Equals(object obj)
        {
            var ret = false;

            if (null != obj && obj is WordCount)
            {
                var wc = (WordCount)obj;

                ret = wc.Word.Equals(Word) && wc.Count == Count;
            }

            return ret;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
