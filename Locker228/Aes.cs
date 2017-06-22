using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace Locker228
{
    class Aes
    {
        public static void Encrypt_AES(OpenFileDialog op, string password, CipherMode mode)
        {
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Padding = PaddingMode.ISO10126;
            byte[] textbytes = File.ReadAllBytes(op.FileName);
            aes.Mode = mode;
            byte[] salt1 = new byte[32];
            byte[] salt2 = new byte[16];
            for (int i = 0; i < 32; i++)
            {
                salt1[i] = (byte)i;
            }
            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(password, salt1, 1000);
            aes.Key = k1.GetBytes(32);
            for (int i = 0; i < 16; i++)
            {
                salt2[i] = (byte)(i - 9);
            }
            Rfc2898DeriveBytes k2 = new Rfc2898DeriveBytes(password, salt2, 1000);//
            aes.IV = k2.GetBytes(16);
            ICryptoTransform icrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] s = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();
            File.WriteAllText(op.FileName, Convert.ToBase64String(s));
        }


        public static void Decrypt_AES(OpenFileDialog op, string password, CipherMode mode)
        {

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Padding = PaddingMode.ISO10126;
            byte[] str = Convert.FromBase64String(File.ReadAllText(op.FileName));
            byte[] salt1 = new byte[32];
            aes.Mode = mode;
            for (int i = 0; i < 32; i++)// задаем соль по конкретному алгоритму
            {
                salt1[i] = (byte)i;
            }
            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(password, salt1, 1000);//
            aes.Key = k1.GetBytes(32);//генерируем ключ
            byte[] salt2 = new byte[16];
            for (int i = 0; i < 16; i++)
            {
                salt2[i] = (byte)(i - 9);
            }
            Rfc2898DeriveBytes k2 = new Rfc2898DeriveBytes(password, salt2, 1000);//
            aes.IV = k2.GetBytes(16);//генерируем вектор инициализации
            ICryptoTransform icrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] s = icrypt.TransformFinalBlock(str, 0, str.Length);
            icrypt.Dispose();
            File.WriteAllBytes(op.FileName, s);

        }
    }
}
