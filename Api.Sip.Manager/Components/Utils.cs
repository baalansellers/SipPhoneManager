using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace Api.Sip.Manager.Components
{
    public class Utils
    {
        public static string EncodeBase64(string content)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
        }
    }
}
