using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACDBackend
{
    public class ACDEncryption
    {
        public static string key = "";

        public static void Decrypt(byte[] data)
        {
            int num = key.Length - 1;
            if (num < 0)
            {
                return;
            }
            int i = 0;
            int num2 = 0;
            int num3 = data.Length;
            while (i < num3)
            {
                int num4 = (int)((char)data[i] - key[num2]);
                data[i] = (byte)((num4 < 0) ? (num4 + 256) : num4);
                if (num2 == num)
                {
                    num2 = 0;
                }
                else
                {
                    num2++;
                }
                i++;
            }
        }

        public static void setupEncryption(string s)
        {
            s = s.ToLower();
            byte b = IntToByte(s.Aggregate(0, (int current, char t) => current + (int)t));
            int num = 0;
            for (int i = 0; i < s.Length - 1; i += 2)
            {
                num = num * (int)s[i] - (int)s[i + 1];
            }
            byte b2 = IntToByte(num);
            int num2 = 0;
            for (int j = 1; j < s.Length - 3; j += 3)
            {
                num2 *= (int)s[j];
                num2 /= (int)(s[j + 1] + '\u001b');
                num2 += -27 - (int)s[j - 1];
            }
            byte b3 = IntToByte(num2);
            int num3 = 5763;
            for (int k = 1; k < s.Length; k++)
            {
                num3 -= (int)s[k];
            }
            byte b4 = IntToByte(num3);
            int num4 = 66;
            for (int l = 1; l < s.Length - 4; l += 4)
            {
                num4 = (int)(s[l] + '\u000f') * num4 * (int)(s[l - 1] + '\u000f') + 22;
            }
            byte b5 = IntToByte(num4);
            int num5 = 101;
            for (int m = 0; m < s.Length - 2; m += 2)
            {
                num5 -= (int)s[m];
            }
            byte b6 = IntToByte(num5);
            int num6 = 171;
            for (int n = 0; n < s.Length - 2; n += 2)
            {
                num6 %= (int)s[n];
            }
            byte b7 = IntToByte(num6);
            int num7 = 171;
            for (int num8 = 0; num8 < s.Length - 1; num8++)
            {
                num7 = num7 / (int)s[num8] + (int)s[num8 + 1];
            }
            byte b8 = IntToByte(num7);

            key = string.Join("-", new object[]
            {
                b,
                b2,
                b3,
                b4,
                b5,
                b6,
                b7,
                b8
            });
        }

        private static byte IntToByte(int value)
        {
            return (byte)((value % 256 + 256) % 256);
        }
    }
}
