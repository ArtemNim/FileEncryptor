using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Windows.Forms;

namespace Locker228
{
    class RC2
    {
        public static void Encrypt_RC2(OpenFileDialog op, string password, CipherMode mode)
        {
            RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
            rc2.Padding = PaddingMode.ISO10126;
            byte[] textbytes = File.ReadAllBytes(op.FileName);
            byte[] salt1 = new byte[16];
            byte[] salt2 = new byte[8];
            rc2.Mode = mode;
            for (int i = 0; i < 16; i++)// задаем соль по конкретному алгоритму
            {
                salt1[i] = (byte)i;
            }
            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(password, salt1, 1000);//
            rc2.Key = k1.GetBytes(16);//генерируем ключ
            for (int i = 0; i < 8; i++)
            {
                salt2[i] = (byte)(i - 9);
            }
            Rfc2898DeriveBytes k2 = new Rfc2898DeriveBytes(password, salt2, 1000);//
            rc2.IV = k2.GetBytes(8);//генерируем вектор инициализации
            ICryptoTransform icrypt = rc2.CreateEncryptor(rc2.Key, rc2.IV);
            byte[] s = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();
            File.WriteAllText(op.FileName, Convert.ToBase64String(s));
        }


        public static void Decrypt_RC2(OpenFileDialog op, string password, CipherMode mode)
        {
            try
            {
                RC2CryptoServiceProvider rc2 = new RC2CryptoServiceProvider();
                rc2.Padding = PaddingMode.ISO10126;
                byte[] str = Convert.FromBase64String(File.ReadAllText(op.FileName));
                byte[] salt1 = new byte[16];
                rc2.Mode = mode;
                for (int i = 0; i < 16; i++)// задаем соль по конкретному алгоритму
                {
                    salt1[i] = (byte)i;
                }
                Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(password, salt1, 1000);//
                rc2.Key = k1.GetBytes(16);//генерируем ключ
                byte[] salt2 = new byte[8];
                for (int i = 0; i < 8; i++)
                {
                    salt2[i] = (byte)(i - 9);
                }
                Rfc2898DeriveBytes k2 = new Rfc2898DeriveBytes(password, salt2, 1000);//
                rc2.IV = k2.GetBytes(8);//генерируем вектор инициализации
                ICryptoTransform icrypt = rc2.CreateDecryptor(rc2.Key, rc2.IV);
                byte[] s = icrypt.TransformFinalBlock(str, 0, str.Length);
                icrypt.Dispose();
                File.WriteAllBytes(op.FileName, s);
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

    }
}
