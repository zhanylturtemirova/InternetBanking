using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Services
{
    interface IGeneratePassword
    {
        string GetLowerCaseString(char[] lowerCaseAlphabet);
        string GetUpperCaseString(char[] upperCaseAlphabet);
        string GetIntegerString();
        string GetSymbolString(char[] symbols);
        string CreatePassword();
    }
}
