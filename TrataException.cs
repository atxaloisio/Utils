using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Utils
{
    public static class TrataException
    {
        public static string getAllMessage(Exception ex)
        {
            string retorno = string.Empty;
            string msgEntity = string.Empty;

            retorno = ex.Message;

            if (ex is DbEntityValidationException)
            {
                string tipo = ex.GetType().ToString();
                foreach (DbEntityValidationResult item in ((DbEntityValidationException)ex).EntityValidationErrors)
                {
                    foreach (DbValidationError item2 in item.ValidationErrors)
                    {
                        msgEntity = msgEntity + "\n" + item2.ErrorMessage;
                    }
                    
                }

                retorno += "\n" + msgEntity;
            }
                        
            if (ex.InnerException != null)
            {
                retorno += "\n" + getAllMessage(ex.InnerException);
            }

            return retorno;
        }
    }
}
