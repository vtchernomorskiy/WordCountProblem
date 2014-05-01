using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace WordCountUtilities
{
    public static class SharedDictionary
    {
        private static readonly Dictionary<string, int> _dict = new Dictionary<string,int>();

        public static KeyValuePair<string, string>[] CheckIncrement(string key, int incrementBy)
        {
            lock (_dict)
            {
                if (_dict.ContainsKey(key))
                {
                    _dict[key] += incrementBy;
                }
                else
                {
                    _dict.Add(key, incrementBy);
                }

                var ret = new List<KeyValuePair<string, string>>();
                foreach (var kvp in _dict)
                {
                    ret.Add(new KeyValuePair<string,string>(kvp.Key, kvp.Value.ToString()));
                }

                return ret.ToArray();
            }
        }
    }
}
