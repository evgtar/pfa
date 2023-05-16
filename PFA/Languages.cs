using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace EvgTar.PFA
{
    public class Languages
    {
        private readonly ResourceManager rm;
        public string Language { get; set; } = "English";
        public string LastError { get; private set; } = "";

        public Languages(string language)
        {
            Language = language.Trim();
            try
            {
                rm = new ResourceManager("PFA.NLS." + Language, Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + $@"\Languages\{Language}.dll"));
            }
            catch { }
        }
        public string LNGetString(string Code, string DefValue)
        {
            string ret = rm?.GetString(Code) ?? DefValue;
            return string.IsNullOrEmpty(ret) ? DefValue : ret;
        }
    }
}