using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InvestingIS
{
    public class Utils
    {
        public static Regex CyrillicRegex = new Regex("[а-яА-Я]");
        public static Regex EnglishRegex = new Regex("[a-zA-Z]");
        public static Regex SpecSymbolsRegex = new Regex("[^A-Za-z0-9]");
        public static Regex NumbersRegex = new Regex("[0-9]");
    }
}
