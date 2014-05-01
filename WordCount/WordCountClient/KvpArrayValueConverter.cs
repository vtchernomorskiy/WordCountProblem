using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace WordCountClient
{
    public class KvpArrayValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var kvpArray = value as KeyValuePair<string, string>[];
            var str = value as string;
            if (null != kvpArray)
            {
                var sb = new StringBuilder();
                sb.AppendLine("CURRENT DICTIONARY STATE");
                sb.AppendLine("========================");
                kvpArray.ToList().ForEach(kvp =>
                {
                    sb.AppendLine(string.Format("{0} - {1}", kvp.Key, kvp.Value));
                });

                return sb.ToString();
            }
            if (null != str)
            {
                return str;
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
