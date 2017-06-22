using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Locker228
{
    public partial class MainWindow : Window
    {
        public OpenFileDialog openFileDialog1 = new OpenFileDialog();
        public FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
        public MainWindow()
        {
            InitializeComponent();
        }

        static int gener_random(int min, int max)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buf = new byte[4];
            int rand = 0;
            do
            {
                rng.GetBytes(buf);
                rand = BitConverter.ToInt32(buf, 0);
                rand /= 10000000;
                rand = Math.Abs(rand);
                rng.Dispose();
            } while (rand <= min || rand >= max);
            return rand;
        }

        public static int random(int min, int max)
        {
            string url = "http://www.random.org/integers/?num=" + 1;

            url += "&min=" + min;
            url += "&max=" + max;   
            url += "&col=1&base=10&format=html&rnd=new";
            var content = new WebClient { Encoding = Encoding.GetEncoding(1251) }.DownloadString(url);
            return Convert.ToInt32(content[7119 + 18].ToString() + content[7119 + 19].ToString() + content[7119 + 20].ToString());

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox.Text = openFileDialog1.FileName;
            }
        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CipherMode mode = CipherMode.CFB;
                switch (comboBox1.SelectedItem.ToString())
                {
                    case "System.Windows.Controls.ComboBoxItem: CBC":
                        mode = CipherMode.CBC;
                        break;
                    case "System.Windows.Controls.ComboBoxItem: CFB":
                        mode = CipherMode.CFB;
                        break;
                    case "System.Windows.Controls.ComboBoxItem: ECB":
                        mode = CipherMode.ECB;
                        break;
                }
                switch (comboBox.SelectedItem.ToString())
                {
                    case "System.Windows.Controls.ComboBoxItem: Aes":
                        Aes.Encrypt_AES(openFileDialog1, passwordBox.Password, mode);
                        break;
                    case "System.Windows.Controls.ComboBoxItem: Rijndael":
                        Rijndael.Encrypt_Rijndael(openFileDialog1, passwordBox.Password, mode);
                        break;
                    case "System.Windows.Controls.ComboBoxItem: RC2":
                        RC2.Encrypt_RC2(openFileDialog1, passwordBox.Password, mode);
                        break;
                    case "System.Windows.Controls.ComboBoxItem: HOST 28147-89":
                        host28147_89.Encrypt_AES(openFileDialog1, passwordBox.Password, mode);
                        break;
                }
            }
            catch (Exception en) { MessageBox.Show("Select all options", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
           try
             {
            CipherMode mode = CipherMode.CFB;
            switch (comboBox1.SelectedItem.ToString())
            {
                case "System.Windows.Controls.ComboBoxItem: CBC":
                    mode = CipherMode.CBC;
                    break;
                case "System.Windows.Controls.ComboBoxItem: CFB":
                    mode = CipherMode.CFB;
                    break;
                case "System.Windows.Controls.ComboBoxItem: ECB":
                    mode = CipherMode.ECB;
                    break;
            }
            switch (comboBox.SelectedItem.ToString())
            {
                case "System.Windows.Controls.ComboBoxItem: Aes":
                    Aes.Decrypt_AES(openFileDialog1, passwordBox.Password, mode);
                    break;
                case "System.Windows.Controls.ComboBoxItem: Rijndael":
                    Rijndael.Decrypt_Rijndael(openFileDialog1, passwordBox.Password, mode);
                    break;
                case "System.Windows.Controls.ComboBoxItem: RC2":
                    RC2.Decrypt_RC2(openFileDialog1, passwordBox.Password, mode);
                    break;
                   
            }
            }
            catch (Exception en)
            { MessageBox.Show("Select all options", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPStatus status = IPStatus.Unknown;
                status = new Ping().Send("google.com.ua").Status;
                MessageBox.Show("There is a connection");
                try
                {
                    if (passwordBox.Visibility == Visibility.Visible)
                    {
                        passwordBox.Password = "";
                        char ch;
                        Random r = new Random();
                        for (int i = 0; i < r.Next(6, 10); i++)
                        {
                            ch = Convert.ToChar(random(48, 122));
                            passwordBox.Password += ch;
                        }
                    }
                    else
                    {
                        textBox1.Text = "";
                        char ch;
                        Random r = new Random();
                        for (int i = 0; i < r.Next(6, 10); i++)
                        {
                            ch = Convert.ToChar(random(48, 122));
                            textBox1.Text += ch;
                        }
                    }
                }
                catch (Exception exeption) { MessageBox.Show(exeption.Message); }
            }
            catch (Exception exep)
            {
                 MessageBox.Show("There is no connection");
                    if (passwordBox.Visibility == Visibility.Visible)
                    {
                        passwordBox.Password = "";
                        char ch;
                        Random r = new Random();
                        for (int i = 0; i < r.Next(6, 10); i++)
                        {
                            ch = Convert.ToChar(gener_random(48, 122));
                            passwordBox.Password += ch;
                        }
                    }
                    else
                    {
                        textBox1.Text = "";
                        char ch;
                        Random r = new Random();
                        for (int i = 0; i < r.Next(6, 10); i++)
                        {
                            ch = Convert.ToChar(gener_random(48, 122));
                            textBox1.Text += ch;
                        }
                    }
            }
           
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Visibility == Visibility.Visible)
            {
                passwordBox.Visibility = Visibility.Hidden;
                textBox1.Visibility = Visibility.Visible;
                textBox1.Text = passwordBox.Password;
            }
            else
            {
                passwordBox.Visibility = Visibility.Visible;
                textBox1.Visibility = Visibility.Hidden;
                passwordBox.Password = textBox1.Text ;
            }
        }

        //////////////////////////Folder Lock//////////////////////////////

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            string[] files = Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.*", SearchOption.AllDirectories);

            CipherMode mode = CipherMode.CFB;
            switch (comboBox3.SelectedItem.ToString())
            {
                case "System.Windows.Controls.ComboBoxItem: CBC":
                    mode = CipherMode.CBC;
                    break;
                case "System.Windows.Controls.ComboBoxItem: CFB":
                    mode = CipherMode.CFB;
                    break;
                case "System.Windows.Controls.ComboBoxItem: ECB":
                    mode = CipherMode.ECB;
                    break;
            }
            switch (comboBox2.SelectedItem.ToString())
            {
                case "System.Windows.Controls.ComboBoxItem: Aes":
                    for (int i = 0; i < files.Length; i++)
                    {
                        try
                        {
                            open.FileName = files[i];
                            Aes.Encrypt_AES(open, passwordBox_Copy.Password, mode);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }

                    break;

                case "System.Windows.Controls.ComboBoxItem: Rijndael":
                    for (int i = 0; i < files.Length; i++)
                    {
                        try
                        {
                            open.FileName = files[i];
                            Rijndael.Encrypt_Rijndael(open, passwordBox_Copy.Password, mode);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                    break;
                case "System.Windows.Controls.ComboBoxItem: RC2":
                    for (int i = 0; i < files.Length; i++)
                    {
                        try
                        {
                            open.FileName = files[i];
                            RC2.Encrypt_RC2(open, passwordBox_Copy.Password, mode);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                    break;
            }
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                textBox2.Text = folderBrowserDialog.SelectedPath;
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();

            string[] files = Directory.GetFiles(folderBrowserDialog.SelectedPath, "*.*", SearchOption.AllDirectories);

            CipherMode mode = CipherMode.CFB;
            switch (comboBox3.SelectedItem.ToString())
            {
                case "System.Windows.Controls.ComboBoxItem: CBC":
                    mode = CipherMode.CBC;
                    break;
                case "System.Windows.Controls.ComboBoxItem: CFB":
                    mode = CipherMode.CFB;
                    break;
                case "System.Windows.Controls.ComboBoxItem: ECB":
                    mode = CipherMode.ECB;
                    break;
            }
            switch (comboBox2.SelectedItem.ToString())
            {
                case "System.Windows.Controls.ComboBoxItem: Aes":
                    for (int i = 0; i < files.Length; i++)
                    {
                        try
                        {
                            open.FileName = files[i];
                            Aes.Decrypt_AES(open, passwordBox_Copy.Password, mode);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }

                    break;

                case "System.Windows.Controls.ComboBoxItem: Rijndael":
                    for (int i = 0; i < files.Length; i++)
                    {
                        try
                        {
                            open.FileName = files[i];
                            Rijndael.Decrypt_Rijndael(open, passwordBox_Copy.Password, mode);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                    break;
                case "System.Windows.Controls.ComboBoxItem: RC2":
                    for (int i = 0; i < files.Length; i++)
                    {
                        try
                        {
                            open.FileName = files[i];
                            RC2.Decrypt_RC2(open, passwordBox_Copy.Password, mode);
                        }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                    break;
            }
        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox_Copy.Visibility == Visibility.Visible)
            {
                passwordBox_Copy.Visibility = Visibility.Hidden;
                textBox3.Visibility = Visibility.Visible;
                textBox3.Text = passwordBox_Copy.Password;
            }
            else
            {
                passwordBox_Copy.Visibility = Visibility.Visible;
                textBox3.Visibility = Visibility.Hidden;
                passwordBox_Copy.Password = textBox3.Text;
            }
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPStatus status = IPStatus.Unknown;
                status = new Ping().Send("google.com.ua").Status;
                MessageBox.Show("There is a connection");
                try
                {
                    if (passwordBox_Copy.Visibility == Visibility.Visible)
                    {
                        passwordBox_Copy.Password = "";
                        char ch;
                        Random r = new Random();
                        for (int i = 0; i < r.Next(6, 10); i++)
                        {
                            ch = Convert.ToChar(random(48, 122));
                            passwordBox_Copy.Password += ch;
                        }
                    }
                    else
                    {
                        textBox3.Text = "";
                        char ch;
                        Random r = new Random();
                        for (int i = 0; i < r.Next(6, 10); i++)
                        {
                            ch = Convert.ToChar(random(48, 122));
                            textBox3.Text += ch;
                        }
                    }
                }
                catch (Exception exeption) { MessageBox.Show(exeption.Message); }
            }
            catch (Exception exep)
            {
                MessageBox.Show("There is no connection");
                if (passwordBox_Copy.Visibility == Visibility.Visible)
                {
                    passwordBox_Copy.Password = "";
                    char ch;
                    Random r = new Random();
                    for (int i = 0; i < r.Next(6, 10); i++)
                    {
                        ch = Convert.ToChar(gener_random(48, 122));
                        passwordBox_Copy.Password += ch;
                    }
                }
                else
                {
                    textBox3.Text = "";
                    char ch;
                    Random r = new Random();
                    for (int i = 0; i < r.Next(6, 10); i++)
                    {
                        ch = Convert.ToChar(gener_random(48, 122));
                        textBox3.Text += ch;
                    }
                }
            }

        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    } 
}
