using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class TrataException
    {
        public static string getAllMessage(Exception ex)
        {
            string retorno = string.Empty;

            retorno = ex.Message;

            if (ex.InnerException != null)
            {
                retorno += "\n" + getAllMessage(ex.InnerException);
            }

            return retorno;
        }
    }
}
