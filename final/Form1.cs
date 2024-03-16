using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Security.Cryptography;

namespace final
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        /* asets*/
        public int E_num_of_letters = 26;
        public int A_num_of_letters = 35;

        char[,] km = new char[5, 5];

        static string alpha = "ABCDEFGHIKLMNOPQRSTUVWXYZ";

        string alpha_l = alpha.ToLower();

        string plain = "";

        char[] mychar = { '!', '.', 'L', 'O', 'C', 'K', 'E', 'D' };

        void mokey(string k)
        {
            k = k.Replace(" ", "");
            k = k.Replace('J', 'I');
            string ky = string.Empty;
            foreach (var item in k)
            {
                if (!ky.Contains(item))
                {
                    ky += item;
                }
            }

            foreach (var item in alpha)
            {
                if (!ky.Contains(item))
                {
                    ky += item;
                }
            }

            int index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    km[i, j] = ky[index++];
                }
            }

            dataGridView1.Rows.Clear();
            for (int i = 0; i < 5; i++)
            {
                object[] ob = new object[]
                {
                    km[i,0],km[i,1],km[i,2],km[i,3],km[i,4]
                };
                dataGridView1.Rows.Add(ob);

            }
        }

        void mopl(string pl)
        {
            pl = pl.Replace(" ", "");
            pl = pl.Replace('J', 'I');
            for (int i = 0; i < pl.Length; i += 2)
            {
                if (pl[i] == pl[i + 1])
                {
                    pl.Insert(i + 1, "X");
                }
            }

            if (pl.Length % 2 != 0)
            {
                pl += "X";
            }
            plain = pl;
        }

        void getxy(char ch, ref int x, ref int y)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (km[i, j] == ch)
                    {
                        x = i;
                        y = j;
                        return;
                    }
                }
            }
        }

        public static int mod(int x, int y, int z)
        {
            return z = x % y;
        }

        private string b_encript(char code, int index)
        {
            string text = code.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            string binary = "";
            foreach (byte b in bytes)
            {
                binary += Convert.ToString(b, 2).PadLeft(8, '0');
            }
            binary += "_" + index.ToString();
            return binary;
        }

        private string b_decript(string code)
        {
            string binary = code;
            byte[] bytes = new byte[binary.Length / 8];
            for (int i = 0; i < binary.Length; i += 8)
            {
                string byteString = binary.Substring(i, 8);
                byte byteValue = Convert.ToByte(byteString, 2);
                bytes[i / 8] = byteValue;
            }
            string result = Encoding.UTF8.GetString(bytes);
            return result;
        }

        public string encript(string key, string plain)
        {
            using (DESCryptoServiceProvider dos = new DESCryptoServiceProvider())
            {
                byte[] keys = Encoding.UTF8.GetBytes(key);
                ICryptoTransform encop = dos.CreateEncryptor(keys, keys);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, encop, CryptoStreamMode.Write);
                byte[] input = Encoding.UTF8.GetBytes(plain);
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }

        }

        public string decript(string key, string cipher)
        {
            byte[] output = Convert.FromBase64String(cipher);
            using (DESCryptoServiceProvider dos = new DESCryptoServiceProvider())
            {
                byte[] keys = Encoding.UTF8.GetBytes(key);
                ICryptoTransform encop = dos.CreateDecryptor(keys, keys);
                var ms = new MemoryStream();
                var cs = new CryptoStream(ms, encop, CryptoStreamMode.Write);
                cs.Write(output, 0, output.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private void DecryptFile(string inputFile, string outputFile, string password)
        {
            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch { }
        }

        private void EncryptFile(string inputFile, string outputFile, string password)
        {
            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);


                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text = from_input.Text;
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            string binary = "";
            foreach (byte b in bytes)
            {
                binary += Convert.ToString(b, 2).PadLeft(8, '0');
            }
            binary_output.Text = binary;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string binary = binary_output.Text;
            byte[] bytes = new byte[binary.Length / 8];
            for (int i = 0; i < binary.Length; i += 8)
            {
                string byteString = binary.Substring(i, 8);
                byte byteValue = Convert.ToByte(byteString, 2);
                bytes[i / 8] = byteValue;
            }
            string result = Encoding.UTF8.GetString(bytes);
            string_output.Text = result;
        }

        private void button3_Click(object sender, EventArgs e)
        {

            E_encoded_text.Text = "";
            if (ceaser_key.Text.Length != 0)
            {
                int ky = int.Parse(ceaser_key.Text);
                if (ky <= E_num_of_letters && ky >= -E_num_of_letters)
                {
                    char[] buffer = E_inport.Text.ToCharArray();
                    for (int i = 0; E_inport.Text.Length > i; i++)
                    {
                        char letter = buffer[i];
                        if (char.IsUpper(letter))
                        {
                            letter = (char)(letter + ky);
                            MessageBox.Show($"{letter}");
                            if (letter > 'Z')
                            {
                                letter = (char)(letter - E_num_of_letters);
                            }
                            else if (letter < 'A')
                            {
                                letter = (char)(letter + E_num_of_letters);
                            }
                        }
                        else if (char.IsLower(letter))
                        {
                            letter = (char)(letter + ky);
                            if (letter > 'z')
                            {
                                letter = (char)(letter - E_num_of_letters);
                            }
                            else if (letter < 'a')
                            {
                                letter = (char)(letter + E_num_of_letters);
                            }
                        }
                        E_encoded_text.Text += letter.ToString();
                    }

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            E_decoded_text.Text = "";
            if (ceaser_key.Text.Length != 0)
            {
                int ky = int.Parse(ceaser_key.Text) * -1;
                if (ky <= E_num_of_letters && ky >= -E_num_of_letters)
                {
                    char[] buffer = E_encoded_text.Text.ToCharArray();
                    for (int i = 0; E_encoded_text.Text.Length > i; i++)
                    {
                        char letter = buffer[i];
                        if (char.IsUpper(letter))
                        {
                            letter = (char)(letter + ky);
                            if (letter > 'Z')
                            {
                                letter = (char)(letter - E_num_of_letters);
                            }
                            else if (letter < 'A')
                            {
                                letter = (char)(letter + E_num_of_letters);
                            }
                        }
                        else if (char.IsLower(letter))
                        {
                            letter = (char)(letter + ky);
                            if (letter > 'z')
                            {
                                letter = (char)(letter - E_num_of_letters);
                            }
                            else if (letter < 'a')
                            {
                                letter = (char)(letter + E_num_of_letters);
                            }
                        }
                        E_decoded_text.Text += letter.ToString();
                    }

                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            A_encoded_text.Text = "";
            if (ceaser_key.Text.Length != 0)
            {
                int ky = int.Parse(ceaser_key.Text);
                if (ky <= A_num_of_letters && ky >= -A_num_of_letters)
                {
                    char[] buffer = A_inport.Text.ToCharArray();
                    for (int i = 0; A_inport.Text.Length > i; i++)
                    {
                        char letter = buffer[i];

                        letter = (char)(letter + ky);
                        if (letter > 'ا')
                        {
                            letter = (char)(letter - A_num_of_letters);
                        }
                        else if (letter < 'ي')
                        {
                            letter = (char)(letter + A_num_of_letters);
                        }

                        A_encoded_text.Text += letter.ToString();
                    }

                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            A_decoded_text.Text = "";
            if (ceaser_key.Text.Length != 0)
            {
                int ky = int.Parse(ceaser_key.Text) * -1;
                if (ky <= A_num_of_letters && ky >= -A_num_of_letters)
                {
                    char[] buffer = A_encoded_text.Text.ToCharArray();
                    for (int i = 0; A_encoded_text.Text.Length > i; i++)
                    {
                        char letter = buffer[i];

                        letter = (char)(letter + ky);
                        if (letter > 'ا')
                        {
                            letter = (char)(letter - A_num_of_letters);
                        }
                        else if (letter < 'ي')
                        {
                            letter = (char)(letter + A_num_of_letters);
                        }

                        A_decoded_text.Text += letter.ToString();
                    }

                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string cipher = string.Empty;
            mopl(p_input.Text.ToUpper());
            for (int i = 0; i < plain.Length; i += 2)
            {
                int x1 = 0, x2 = 0, y1 = 0, y2 = 0;

                getxy(plain[i], ref x1, ref y1);
                getxy(plain[i + 1], ref x2, ref y2);

                if (x1 == x2)
                {
                    cipher += km[x1, (y1 + 1) % 5];
                    cipher += km[x2, (y2 + 1) % 5];
                }
                else if (y1 == y2)
                {
                    cipher += km[(x1 + 1) % 5, y1];
                    cipher += km[(x2 + 1) % 5, y2];
                }
                else
                {
                    cipher += km[x1, y2];
                    cipher += km[x2, y1];
                }
            }
            P_encode.Text = cipher;
        }

        private void P_Key_TextChanged(object sender, EventArgs e)
        {
            mokey(P_Key.Text.ToUpper());
        }

        private void button9_Click(object sender, EventArgs e)
        {
            V_Encode_text.Text = "";
            if (V_input.Text != "" && V_Key.Text != "")
            {

                string key = V_Key.Text;

                string superkey = key;

                string pltext = V_input.Text;

                while (pltext.Length > superkey.Length)
                {
                    superkey += key;
                }

                for (int i = 0; i < pltext.Length; i++)
                {
                    if (alpha.Contains(pltext[i]))
                    {
                        int result = 0;

                        int later_imdex = mod((alpha.IndexOf(pltext[i]) + alpha.IndexOf(superkey[i].ToString().ToUpper())), 26, result);
                        if (later_imdex > 26)
                        {
                            later_imdex = later_imdex - 26;
                        }
                        V_Encode_text.Text += alpha[later_imdex];
                    }
                    else if (alpha_l.Contains(pltext[i]))
                    {
                        int result = 0;

                        int later_imdex = mod((alpha_l.IndexOf(pltext[i]) + alpha_l.IndexOf(superkey[i].ToString().ToLower())), 26, result);
                        if (later_imdex > 26)
                        {
                            later_imdex = later_imdex - 26;
                        }
                        V_Encode_text.Text += alpha_l[later_imdex];
                    }
                    else
                    {
                        V_Encode_text.Text += pltext[i];
                    }
                }

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            V_Decode_Text.Text = "";
            if (V_Encode_text.Text != "" && V_Key.Text != "")
            {

                string key = V_Key.Text;

                string superkey = key;

                string pltext = V_Encode_text.Text;

                while (pltext.Length > superkey.Length)
                {
                    superkey += key;
                }

                for (int i = 0; i < pltext.Length; i++)
                {
                    if (alpha.Contains(pltext[i]))
                    {
                        int result = 0;
                        int later_imdex = mod((alpha.IndexOf(pltext[i]) - alpha.IndexOf(superkey[i].ToString().ToUpper())), 26, result);
                        if (later_imdex < 0)
                        {
                            later_imdex = later_imdex + 26;
                        }
                        V_Decode_Text.Text += alpha[later_imdex];
                    }
                    else if (alpha_l.Contains(pltext[i]))
                    {
                        int result = 0;
                        int later_imdex = mod((alpha_l.IndexOf(pltext[i]) - alpha_l.IndexOf(superkey[i].ToString().ToLower())), 26, result);
                        if (later_imdex < 0)
                        {
                            later_imdex = later_imdex + 26;
                        }
                        V_Decode_Text.Text += alpha_l[later_imdex];
                    }
                    else
                    {
                        V_Decode_Text.Text += pltext[i];
                    }
                }

            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Dictionary<string, char> unquekey = new Dictionary<string, char>();
            Shadwaled_planText.Text = string.Empty;
            Shadwaled_cipherText.Text = string.Empty;
            string plaintext = R_input.Text;
            string key = R_Key.Text;
            int colum = key.Length;
            int row = plaintext.Length / colum;


            for (int i = 0; i < colum; i++)
            {
                char _value = key[i];
                string _key = b_encript(_value, i);
                unquekey.Add(_key, _value);
            }

            char[,] p1 = new char[row, colum];

            for (int i = 0; i < colum; i++)
            {
                Shadwaled_planText.Text += " " + key[i];
            }

            Shadwaled_planText.Text += "\r\n";

            int k = 0;

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < colum; j++)
                {
                    p1[i, j] = plaintext[k];
                    Shadwaled_planText.Text += " " + p1[i, j];
                    k++;
                }
                Shadwaled_planText.Text += "\r\n";
            }

            var sortedkey = unquekey.OrderBy(x => x.Value);

            foreach (var value in sortedkey)
            {
                var oregnal = value.Key.Split('_');

                var oregnalckar = b_decript(oregnal[0]);

                Shadwaled_cipherText.Text += " " + oregnalckar;
            }
            Shadwaled_cipherText.Text += "\r\n";

            Dictionary<string, string> usedkeys = new Dictionary<string, string>();

            for (int i = 0; i < row; i++)
            {
                foreach (var value in sortedkey)
                {
                    var oregnal = value.Key.Split('_');
                    for (int j = 0; j < colum; j++)
                    {
                        if (key[j] == value.Value)
                        {
                            if (!usedkeys.ContainsKey(j.ToString()))
                            {
                                if (usedkeys.Count != 0)
                                {
                                    try
                                    {
                                        string ifused = usedkeys.First(q => q.Value == oregnal[1]).Key;
                                        if (ifused != j.ToString())
                                        {
                                            usedkeys.Add(j.ToString(), oregnal[1]);
                                            Shadwaled_cipherText.Text += " " + p1[i, j];
                                        }
                                    }
                                    catch
                                    {
                                        usedkeys.Add(j.ToString(), oregnal[1]);
                                        Shadwaled_cipherText.Text += " " + p1[i, j];
                                    }

                                }
                                else
                                {
                                    usedkeys.Add(j.ToString(), oregnal[1]);
                                    Shadwaled_cipherText.Text += " " + p1[i, j];
                                }

                            }

                        }

                    }

                }
                Shadwaled_cipherText.Text += "\r\n";
                usedkeys.Clear();
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                D_Encode_text.Text = encript(D_Key.Text, D_input.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                D_Decode_text.Text = decript(D_Key.Text, D_Encode_text.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            //Add files
            OpenFileDialog filepath = new OpenFileDialog();
            filepath.Title = "Select File";
            filepath.InitialDirectory = @"C:\";
            filepath.Filter = "All files (*.*)|*.*";
            filepath.Multiselect = true;
            filepath.FilterIndex = 1;
            filepath.ShowDialog();
            foreach (String file in filepath.FileNames)
            {
                listBox1.Items.Add(file); //add file path to the listbox
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            //clear all values in listbox1
            listBox1.Items.Clear();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //Add folders
            FolderBrowserDialog folderpath = new FolderBrowserDialog();
            folderpath.ShowDialog();
            listBox2.Items.Add(folderpath.SelectedPath);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //clear all values in listbox2
            listBox2.Items.Clear();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            //This function will encrypt selected files
            //Password must have 8 characters!
            if (textBox1.Text.Length < 8)
            {
                MessageBox.Show("Password must have 8 characters!", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //This is for selected files
            if (listBox1.Items.Count > 0)
            {
                for (int num = 0; num < listBox1.Items.Count; num++)
                {
                    string e_file = "" + listBox1.Items[num];
                    if (!e_file.Trim().EndsWith(".!LOCKED") && File.Exists(e_file))
                    {
                        EncryptFile("" + listBox1.Items[num], "" + listBox1.Items[num] + ".!LOCKED", textBox1.Text);
                        File.Delete("" + listBox1.Items[num]);
                    }
                }
            }
            //This is for selected folders
            if (listBox2.Items.Count > 0)
            {
                for (int num = 0; num < listBox2.Items.Count; num++)
                {
                    string d_file = "" + listBox2.Items[num];
                    string[] get_files = Directory.GetFiles(d_file);
                    foreach (string name_file in get_files)
                    {
                        if (!name_file.Trim().EndsWith(".!LOCKED") && File.Exists(name_file))
                        {
                            EncryptFile(name_file, name_file + ".!LOCKED", textBox1.Text);
                            File.Delete(name_file);
                        }
                    }
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            //This function will decrypt selected files
            //Password must have 8 characters!
            //Password must be correct!
            if (textBox1.Text.Length < 8)
            {
                MessageBox.Show("Password must have 8 characters!", "Invalid Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //This is for selected files
            if (listBox1.Items.Count > 0)
            {
                for (int num = 0; num < listBox1.Items.Count; num++)
                {
                    string e_file = "" + listBox1.Items[num];
                    if (e_file.Trim().EndsWith(".!LOCKED") && File.Exists(e_file))
                    {
                        DecryptFile(e_file, e_file.TrimEnd(mychar), textBox1.Text);
                        File.Delete(e_file);
                    }
                }
            }
            //This is for selected folders
            if (listBox2.Items.Count > 0)
            {
                for (int num = 0; num < listBox2.Items.Count; num++)
                {
                    string d_file = "" + listBox2.Items[num];
                    string[] get_files = Directory.GetFiles(d_file);
                    foreach (string name_file in get_files)
                    {
                        if (name_file.Trim().EndsWith(".!LOCKED") && File.Exists(name_file))
                        {
                            DecryptFile(name_file, name_file.TrimEnd(mychar), textBox1.Text);
                            File.Delete(name_file);
                        }
                    }
                }
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //Clear values from listbox1, 2 and password text line
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            textBox1.Text = "";
        }
    }
}
