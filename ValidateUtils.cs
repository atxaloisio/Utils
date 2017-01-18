using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils
{
    public static class ValidateUtils
    {
        public static bool isDate(string data)
        {
            Regex r = new Regex(@"(\d{2}\/\d{2}\/\d{4})");
            if (r.Match(data).Success)
            {
                DateTime Temp;


                if (DateTime.TryParse(data, out Temp) == true)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }

        public static bool isDateTime(string data)
        {
            Regex r = new Regex(@"(\d{2}\/\d{2}\/\d{4} \d{2}:\d{2})");
            return r.Match(data).Success;
        }
    }
}
