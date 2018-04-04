using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetBanking.Services
{
    public class GeneratePasswordService:IGeneratePassword
    {
        public static char[] lowerCaseAlphabet = Enumerable.Range('a', 26).Select(x => (char)x).ToArray();
        public static char[] upperCaseAlphabet = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();
        public static char[] symbols = new char[] { '@', '$', '#', '*', '!', '&', '?', '^' };

        public string GetLowerCaseString(char[] lowerCaseAlphabet)
        {
            Random rnd = new Random();
            string lowerCase = string.Empty;
            for (int i = 0; i < 2; i++)
            {
                int n = rnd.Next(0, lowerCaseAlphabet.Length);
                lowerCase += lowerCaseAlphabet[n];
            }
            return lowerCase;
        }

        public string GetUpperCaseString(char[] upperCaseAlphabet)
        {
            Random rnd = new Random();
            string upperCase = string.Empty;
            for (int i = 0; i < 2; i++)
            {
                int n = rnd.Next(0, upperCaseAlphabet.Length + 1);
                upperCase += upperCaseAlphabet[n];
            }
            return upperCase;
        }

        public string GetIntegerString()
        {
            Random rnd = new Random();
            string number = string.Empty;
            for (int i = 0; i < 2; i++)
            {
                int n = rnd.Next(0, 10);
                number += n;
            }
            return number;
        }

        public string GetSymbolString(char[] symbols)
        {
            Random rnd = new Random();
            string symbol = string.Empty;
            for (int i = 0; i < 2; i++)
            {
                int n = rnd.Next(0, symbols.Length);
                symbol += symbols[n];
            }
            return symbol;
        }

        public string CreatePassword()
        {
            string lowerCase = GetLowerCaseString(lowerCaseAlphabet);
            string upperCase = GetLowerCaseString(upperCaseAlphabet);
            string number = GetIntegerString();
            string symbol = GetSymbolString(symbols);
            string str1 = string.Concat(string.Concat(string.Concat(lowerCase, upperCase), number), symbol);
            return Shuffle(str1).ToString();

        }

        public static string Shuffle(string str)
        {
            char[] array = str.ToCharArray();
            Random rng = new Random();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }

        public List<string> Blacklist = new List<string>()
        {
            "Password1!",
            "Admin123!",
            "Password123!",
            "Qwerty123!"
        };
        public List<string> GetBlackList()
        {
            return Blacklist;
        }
    }
}
