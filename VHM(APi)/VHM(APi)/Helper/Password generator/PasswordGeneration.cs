using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VHM_APi_.Helper.Password_generator
{
    public class PasswordGeneration
    {
        private static int[] numbers = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        private static char[] LowerCases = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private static char[] UpperCases = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static char[] SpecialCases = { '!', '@', '#', '$', '%', '^', '&', '*', '?', '_', '-' };
        public static string Generator()
        {
            Random random = new Random();


            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < 2; i++)
            {
                builder.Append(numbers[random.Next(0, numbers.Length)]);
                builder.Append(LowerCases[random.Next(0, LowerCases.Length)]);
                builder.Append(UpperCases[random.Next(0, UpperCases.Length)]);
                builder.Append(SpecialCases[random.Next(0, SpecialCases.Length)]);
            }
            return Guid.NewGuid().ToString("N") + builder.ToString();
        }
    }
}
