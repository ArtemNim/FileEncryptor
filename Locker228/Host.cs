using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.IO;
using static System.Math;

namespace Locker228
{
    public abstract class Host : SymmetricAlgorithm { }
    public class HostManaged : Host
    {
        public string password;
        public static SymmetricAlgorithm host = SymmetricAlgorithm.Create();
       
        public HostManaged()
        {
            // define the valid block and key sizes
            LegalKeySizesValue = new KeySizes[] { new KeySizes(256, 256, 0) };
            LegalBlockSizesValue = new KeySizes[] { new KeySizes(64, 64, 0) };
           
        }
        public override void GenerateKey()
        {
            
            host.KeySize = 256;
            byte[] salt1 = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                salt1[i] = (byte)i;
            }

            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(password, salt1, 1000);
            host.Key = k1.GetBytes(32);
        }
        public override void GenerateIV()
        {
           // LegalKeySizesValue = new KeySizes[] { new KeySizes(64, 64, 0) };
            
            //host.BlockSize = 128;//change
            byte[] salt2 = new byte[16];

            for (int i = 0; i < 16; i++)
            {
                salt2[i] = (byte)(i - 9);
            }

            Rfc2898DeriveBytes k2 = new Rfc2898DeriveBytes(password, salt2, 1000);//
            host.IV = k2.GetBytes(16);
        }
        public override ICryptoTransform CreateDecryptor(byte[] p_key, byte[] p_iv)
        {
            return null;
           // return new XTEAManagedDecryptor(p_key);
        }

        public override ICryptoTransform CreateEncryptor(byte[] p_key, byte[] p_iv)
        {
            return new HostManagedEncryptor(p_key);
        }




    }
    public abstract class HostManagedAbstractTransform:ICryptoTransform
    {
        public uint[] round_keys;
        public HostManagedAbstractTransform (byte[]key)
        {
            round_keys = ConvertBytesToUints(key, 0, key.Length);//перетворює ключ в масив 32-бітних раундових ключів
        }
        public uint[] ConvertBytesToUints(byte[] data, int offset, int count)
        {
            uint[] k = new uint[count/32];
            for (int i = offset, j = 0; i < offset + 32; i += 32,j++,offset+=32)
            {
                k[j] = BitConverter.ToUInt32(data, i);
            }
            uint[] k_rez = new uint[32];//готовый массив ключей
            for (int i = 0; i < 25; i++)
            {
                k_rez[i] = k[i % 8];
            }
            for (int i = 7, j = 25; i > -1; i--, j++)
            {
                k_rez[j] = k[i];
            }
            return k_rez;
        }
        //public ConvertUintToBytes(uintdata)
        public virtual void Dispose()
        {
            // we use no unmanaged resources
        }
        public bool CanReuseTransform
        {
            get
            {
                return false;
            }
        }

        public bool CanTransformMultipleBlocks
        {
            get
            {
                return false;
            }
        }

        public int InputBlockSize
        {
            get
            {
                return 8;
            }
        }

        public int OutputBlockSize
        {
            get
            {
                return 8;
            }
        }
        public abstract int TransformBlock(byte[] buffer, int offset, int count, byte[] output, int output_offset);

        public abstract byte[] TransformFinalBlock(byte[] buffer, int offset, int count);



    }
    public class HostManagedEncryptor : HostManagedAbstractTransform
    {
        public HostManagedEncryptor(byte[] key) : base(key) { }
        byte[] HostEncrypt(byte[]data, int offset, int count)
        {
            uint[] key = ConvertBytesToUints(data, offset, count);
            uint rez;
            uint[,] s_box = { { 4, 10,  9,   2 ,  13,  8,   0,   14,  6,   11,  1,   12,  7,   15,  5,   3 },
                              { 14 , 11,  4,   12,  6,   13,  15,  10,  2,   3,   8,   1,   0,   7,   5,   9 },
                             { 5,   8,   1,   13,  10,  3,   4,   2,   14,  15,  12,  7,   6,   0,   9,   11 },
                             { 7,   13,  10,  1,   0,   8,   9,   15,  14,  4 ,  6,   12,  11,  2,   5,   3 },
                              { 6,   12 , 7 ,  1,   5,   15,  13,  8,   4,   10,  9 ,  14,  0 ,  3,   11,  2 },
                             { 4,   11,  10,  0,   7,   2 ,  1 ,  13,  3,   6,   8 ,  5 ,  9 ,  12 , 15,  14 },
                             { 13,  11 , 4 ,  1,   3,   15,  5,   9,   0,   10 , 14,  7 ,  6,   8 ,  2,   12 },
                             { 1 ,  15 , 13,  0 ,  5 ,  7 ,  10,  4 ,  9 ,  2 ,  3 ,  14 , 6 ,  11 , 8 ,  12 } } ;
            uint L = Convert.ToUInt32(data.Take(data.Length/2)), R = Convert.ToUInt32(data.Skip(data.Length / 2).Take(data.Length/2));
            for (int i = 0; i < 32; i++)
            {
                rez = Convert.ToUInt32((L+key[i]) % Pow(2,32));
                rez = s_box[i,rez];
                rez <<= 11;
                rez = (rez + R) % 2;
                L = R;
                R = rez;
            }
            byte[] data_crypt = new byte[64];
            for (int i = 0, j = 32; i < 32; i++,j++)
            {
                data_crypt[i] = Convert.ToByte(L);
                data_crypt[j] = Convert.ToByte(R);
            }
            return data_crypt;
        }
        public override void Dispose()
        {
            
        }

        public override int TransformBlock(byte[] buffer, int offset, int count, byte[] output, int output_offset) => -1;

        public override byte[] TransformFinalBlock(byte[] buffer, int offset, int count) => new byte[] { };
    }
    class host28147_89
    {
        public static void Encrypt_AES(OpenFileDialog op, string password, CipherMode mode)
        {

           HostManaged host = new HostManaged();
           
            host.password = password;
            host.Padding = PaddingMode.ISO10126;
            byte[] textbytes = File.ReadAllBytes(op.FileName);
            host.Mode = mode;

            host.KeySize = 256;
            byte[] salt1 = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                salt1[i] = (byte)i;
            }

            Rfc2898DeriveBytes k1 = new Rfc2898DeriveBytes(password, salt1, 1000);
            host.Key = k1.GetBytes(32);


           // host.BlockSize = 128;//change
            byte[] salt2 = new byte[16];

            for (int i = 0; i < 16; i++)
            {
                salt2[i] = (byte)(i - 9);
            }

            Rfc2898DeriveBytes k2 = new Rfc2898DeriveBytes(password, salt2, 1000);//
            host.BlockSize = 64;
            host.IV = k2.GetBytes(8);
            ICryptoTransform icrypt = host.CreateEncryptor(host.Key, host.IV);
            byte[] s = icrypt.TransformFinalBlock(textbytes, 0, textbytes.Length);
            icrypt.Dispose();
            File.WriteAllText(op.FileName, Convert.ToBase64String(s));
        }
    }
}
