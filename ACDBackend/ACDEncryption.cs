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
            int i = 0;

            int num2 = 0;

            while (i < data.Length)
            {
                int num4 = (int)((char)data[i] - key[num2]);

                data[i] = (byte)((num4 < 0) ? (num4 + 256) : num4);

                if (num2 == key.Length - 1)
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

        public static void setupEncryption(string folderName)
        {
            folderName = folderName.ToLower();

            int aggregateSeed = folderName.Aggregate(0, (int current, char t) =>
            {
                Console.WriteLine($"Current: {current}, T: {t}, T(NUM): {(int)t}");

                return current + t;
            });

            /* Octet 1 */
            byte octet1 = intToByte(aggregateSeed);

            /* Octet 2 */
            int num = 0;
            for (int i = 0; i < folderName.Length - 1; i += 2)
            {
                num = (num * folderName[i]) - folderName[i + 1];
            }
            byte octet2 = intToByte(num);
            //-----------------------

            /* Octet 3 */
            int num2 = 0;
            for (int j = 1; j < folderName.Length - 3; j += 3)
            {
                num2 = ((num2 * folderName[j]) / (folderName[j + 1] + 27)) - 27 - folderName[j - 1];
            }
            byte octet3 = intToByte(num2);
            //-----------------------

            /* Octet 4 */
            int num3 = 5763;
            for (int k = 1; k < folderName.Length; k++)
            {
                num3 -= folderName[k];
            }
            byte octet4 = intToByte(num3);
            //-----------------------

            /* Octet 5 */
            int num4 = 66;
            for (int l = 1; l < folderName.Length - 4; l += 4)
            {
                num4 = (folderName[l] + 15) * num4 * (folderName[l - 1] + 15) + 22;
            }
            byte octet5 = intToByte(num4);
            //-----------------------

            /* Octet 6 */
            int num5 = 101;
            for (int m = 0; m < folderName.Length - 2; m += 2)
            {
                num5 -= folderName[m];
            }
            byte octet6 = intToByte(num5);
            //-----------------------

            /* Octet 7 */
            int num6 = 171;
            for (int n = 0; n < folderName.Length - 2; n += 2)
            {
                num6 %= folderName[n];
            }
            byte octet7 = intToByte(num6);
            //-----------------------

            /* Octet 8 */
            int num7 = 171;
            for (int num8 = 0; num8 < folderName.Length - 1; num8++)
            {
                num7 = num7 / folderName[num8] + folderName[num8 + 1];
            }
            byte octet8 = intToByte(num7);
            //-----------------------

            //Concatenating the key
            key = string.Join("-", new object[]
            {
                octet1,
                octet2,
                octet3,
                octet4,
                octet5,
                octet6,
                octet7,
                octet8
            });

            Console.WriteLine($"[ACD Encryption]: Got key - {key}");
        }

        private static byte intToByte(int value)
        {
            return (byte)((value % 256 + 256) % 256);
        }
    }
}
