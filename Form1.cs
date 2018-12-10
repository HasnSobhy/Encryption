using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Collections;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        int key, index, n, i;
        String pmessage, cmessage;
        String upletters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        char[] ptext;        //read from richtextbox1(plain text) that read the plain text
        Char[] ctext;        //read from the list the encrypted message and show it in richtextbox2(cipher text)
        List<Char> CTEXT;    //use list instead of array because of i don't want to declare size of array

        char[] upalpha;      //array  to store upper letters
        char[] loalpha;       //array to store lower letters

        //declared to use in decryption
        char[] ciphertext;
        char[] plaintext;
        List<Char> CIPHERTEXT;

        private bool button3WasClicked = false;

        char[,] matrix;
        string finalMsg = string.Empty;

        private List<string> subKeys = new List<string>();
        string DesKey;
        string Desmsg;

        string prevLeft, prevRight;
        string leftPlain, rightPlain;


        byte[] PC1 = new byte[]{57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51,
                43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30,
                22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4};

        byte[] shiftsArray = new byte[] { 1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1 };

        byte[] PC2 = new byte[]{14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16,
                7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53,
                46, 42, 50, 36, 29, 32};

        byte[] IP = new byte[] { 58, 50, 42, 34, 26, 18, 10, 2, 60, 52, 44, 36, 28, 20, 12, 4, 62, 54, 46, 38, 30, 22, 14, 6, 64, 56, 48, 40, 32, 24, 16, 8, 57, 49, 41, 33, 25, 17, 9, 1, 59, 51, 43, 35, 27, 19, 11, 3, 61, 53, 45, 37, 29, 21, 13, 5, 63, 55, 47, 39, 31, 23, 15, 7 };

        byte[] IP_INVERSE = new byte[] { 40, 8, 48, 16, 56, 24, 64, 32, 39, 7, 47, 15, 55, 23, 63, 31, 38, 6, 46, 14, 54, 22, 62, 30, 37, 5, 45, 13, 53, 21, 61, 29, 36, 4, 44, 12, 52, 20, 60, 28, 35, 3, 43, 11, 51, 19, 59, 27, 34, 2, 42, 10, 50, 18, 58, 26, 33, 1, 41, 9, 49, 17, 57, 25 };

        byte[] D_BOX = new byte[] { 32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1 };

        byte[] P = new byte[] { 16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25 };

        byte[,] S_BOX = new byte[,] {
        { 14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7, 0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8, 4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0, 15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6,13},
        {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10, 3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5, 0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15, 13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9},
        {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8, 13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1, 13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7, 1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12},
        {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15, 13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9, 10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4, 3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14},
        {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9, 14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6, 4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14, 11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3},
        {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11, 10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8, 9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6, 4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13},
        {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1, 13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6, 1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2, 6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12},
        {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7, 1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2, 7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8, 2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
        };





        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Caesar Cipher");
            comboBox1.Items.Add("Play Faire");
            comboBox1.Items.Add("DES");
            comboBox1.Items.Add("RC4");

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            pmessage = richTextBox1.Text.ToString();
            ptext = pmessage.ToCharArray();
            Desmsg = richTextBox1.Text.ToString();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            cmessage = richTextBox2.Text.ToString();
            ciphertext = cmessage.ToCharArray();    //stored ciphered message to character array 

        }

        private void button3_Click(object sender, EventArgs e)
        {



            button3WasClicked = true;
            richTextBox1.Text = "";
            richTextBox2.Text = "";
            label5.Text = "";


        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "DES")
            {
                textBox1.MaxLength = 8;

            }  
                DesKey = textBox1.Text;

            
            
        }
        private void button2_Click(object sender, EventArgs e)
        {

            //this button for decryption




            richTextBox1.Text = "";

            upalpha = upletters.ToCharArray();    //convert string of capital letters to array of characters
            loalpha = (upletters.ToLower()).ToCharArray();  //convert string of capital letters to array of small characters
            CIPHERTEXT = new List<char>();     //use list instead of array because of i don't want to declare size of array

            if (richTextBox2.Text == "")
            {
                DialogResult dialogResult = MessageBox.Show("Cipher Text Can't be Empty !", "Warning", MessageBoxButtons.OK);

            }
            else if (textBox1.Text == "")
            {
                DialogResult dialogResult = MessageBox.Show("Key Field Can't be Empty !", "Warning", MessageBoxButtons.OK);
            }
            else if (comboBox1.Text == "")
            {

                DialogResult dialogResult = MessageBox.Show("Must Choose Technique !", "Warning", MessageBoxButtons.OK);
            }
            else
            {
                if (comboBox1.Text == "Caesar Cipher")
                {
                    key = int.Parse(textBox1.Text.ToString());         //convert string to integer
                    for (n = 0; n < ciphertext.Length; n++)
                    {


                        for (i = 0; i < upalpha.Length; i++)
                        {
                            if (ciphertext[n] == ' ')
                            {
                                CIPHERTEXT.Add(' ');   //this for seperator between words 
                                break;   //if it found seperator , put seperator and break the loop 

                            }

                            else if (Char.IsLower(ciphertext[n]))     //excute this block if the chacracter is small

                            {


                                if (ciphertext[n].ToString() == loalpha[i].ToString())
                                {
                                    index = (i - key) % 26;
                                    if (index < 0)     //I wrote if because of the modulo of negative number by 26 will be the the same negative value
                                                       //for example if index=(2-3) %26 >>then index will be equal to -1 so that we add it to 26 to be the value of index equal to 25 
                                    {
                                        index = index + 26;
                                    }

                                    CIPHERTEXT.Add(loalpha[index]);   //added the index of the new character to list 

                                }

                            }
                            else if (Char.IsUpper(ciphertext[n]))
                            {
                                if (ciphertext[n].ToString() == upalpha[i].ToString())
                                {
                                    index = (i - key) % 26;
                                    if (index < 0)
                                    {
                                        index = index + 26;
                                    }
                                    CIPHERTEXT.Add(upalpha[index]);
                                }


                            }

                        }

                    }
                    label5.Text = "<<<<";
                    label5.ForeColor = Color.Red;
                    label5.BackColor = Color.White;

                    plaintext = CIPHERTEXT.ToArray();                 //convert list to array of character

                    richTextBox1.Text = new String(plaintext);   //convert array of character to string 
                }

                else if (comboBox1.Text == "Play Faire")
                {
                    //for decryption
                    richTextBox1.Text = DecryptMsg(textBox1.Text, richTextBox2.Text);
                }
                else if (comboBox1.Text == "DES")
                {
                    richTextBox1.Text = DesDEcrypt(richTextBox2.Text);
                }
                else if (comboBox1.Text == "RC4")
                {

                    richTextBox1.Text = RC4decrypt(richTextBox2.Text, textBox1.Text);
                }
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            //this button for encryption



            richTextBox2.Text = "";

            upalpha = upletters.ToCharArray();    //array  to store upper letters
            loalpha = (upletters.ToLower()).ToCharArray();   //array to store lower letters
            CTEXT = new List<char>();     //use list instead of array because of i don't want to declare size of array

            if (richTextBox1.Text == "")
            {
                DialogResult dialogResult = MessageBox.Show("Plain Text Can't be Empty !", "Warning", MessageBoxButtons.OK);

            }
            else if (textBox1.Text == "")
            {
                DialogResult dialogResult = MessageBox.Show("Key Field Can't be Empty !", "Warning", MessageBoxButtons.OK);
            }
            else if (comboBox1.Text == "")
            {

                DialogResult dialogResult = MessageBox.Show("Must Choose Technique !", "Warning", MessageBoxButtons.OK);
            }

            else
            {
                if (comboBox1.Text == "Caesar Cipher")
                {
                    key = int.Parse(textBox1.Text.ToString());         //convert string to integer


                    for (n = 0; n < ptext.Length; n++)
                    {


                        for (i = 0; i < upalpha.Length; i++)
                        {

                            if (ptext[n] == ' ')      //this if because of separation between words
                            {
                                CTEXT.Add(' ');
                                break;
                            }

                            else if (Char.IsLower(ptext[n]))     //excute this block if the chacracter is small
                            {
                                if (ptext[n].ToString() == loalpha[i].ToString())
                                {
                                    index = (key + i) % 26;

                                    CTEXT.Add(loalpha[index]);         //added the index of the new character to list 

                                }

                            }
                            else if (Char.IsUpper(ptext[n]))
                            {
                                if (ptext[n].ToString() == upalpha[i].ToString())
                                {
                                    index = (key + i) % 26;
                                    CTEXT.Add(upalpha[index]);
                                }


                            }

                        }

                    }

                    ctext = CTEXT.ToArray();                 //convert list to array 
                    richTextBox2.Text = new String(ctext);   //convert array of character to string 
                    while (!(button3WasClicked))
                    {
                        label5.Text = ">>>>";
                        label5.ForeColor = Color.Red;
                        label5.BackColor = Color.Black;
                        Thread.Sleep(500);
                        label5.ForeColor = Color.Blue;
                        label5.BackColor = Color.White;
                        Thread.Sleep(500);

                    }
                }


                else if (comboBox1.Text == "Play Faire")
                {

                    richTextBox2.Text = (EncryptMsg(textBox1.Text, richTextBox1.Text));

                }
                else if (comboBox1.Text == "DES")
                {
                    richTextBox2.Text = DesEncrypt((richTextBox1.Text).ToString());
                }
                else if (comboBox1.Text == "RC4")
                {

                    richTextBox2.Text = RC4Encrypt(richTextBox1.Text, textBox1.Text);
                }
            }
        }




        //play faire functions

        // remove dublicate character from string
        public static string RemoveDuplicates(string input)
        {
            return new string(input.ToCharArray().Distinct().ToArray());
        }

        //function for putting key and alphapet in matrix in distinct way

        public char[,] GetMatrix(string key)
        {
            int counter = 0;                                                    //used with count index to the string when put it into matrix
            key = textBox1.Text.ToString();
            string keyPlusAlphapet = key + upletters.ToLower();                 //concatinate key+alphapetletters
            string alldata = RemoveDuplicates(keyPlusAlphapet);                 //remove du[licate letters
            var stringBuilder = new StringBuilder(alldata);                     //to remove char i 

            for (i = 0; i < stringBuilder.Length; i++)
            {
                if (stringBuilder[i] == 'j' || stringBuilder[i] == 'J')
                {
                    stringBuilder.Remove(i, 1);
                }
            }

            //used string builder that have fumctions of replace and insert and delete (it is dynamic object)

            matrix = new char[5, 5];
            for (i = 0; i < 5; i++)
            {
                for (n = 0; n < 5; n++)
                {
                    matrix[i, n] = stringBuilder[counter];
                    counter++;
                }
            }
            return matrix;
        }


        //function for putting x for dublicating consequtive letters

        private string getMessage(string message)
        {

            int n = 0;
            StringBuilder sb = new StringBuilder(message);



            for (int i = 0; (i + 1) < sb.Length; i++)
            {
                if (sb[i] == ' ')        //if meet space and if length of previous chracters is odd , put x
                {

                    if ((message.Substring(n, i - 1)).Length % 2 != 0)
                    {
                        sb[i] = 'x';

                        n = i + 1;
                    }
                    else
                    {
                        continue;
                    }

                }
                else if (char.ToLower(sb[i]) == char.ToLower(sb[i + 1]))
                {
                    if (sb.ToString().Substring(0, i).Length % 2 == 0)   //use to insert x instead of 2consequtive characters ليكون لناتج مثل الكتاب
                    {
                        if (char.IsLower(sb[i + 1]))
                            sb.Insert(i + 1, 'x');
                        else
                            sb.Insert(i + 1, 'X');
                    }
                    else
                        continue;
                }
            }


            finalMsg = sb.ToString();
            if (finalMsg.Length % 2 != 0)
                finalMsg = finalMsg + 'x';
            return finalMsg;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        //put 2 consequtive characters in array of string
        private string[] dividestring(string msg)
        {


            string fMsg = getMessage(msg);
            int size = fMsg.Length;



            int c = 0;
            string[] strArray = new string[size / 2];

            for (i = 0; i < strArray.Length; i++)
            {

                strArray[i] = fMsg.Substring(c, 2);  // seconed parameter of substring is the number of characters to be extracted
                c = c + 2;

            }
            return strArray;
        }


        //function to get dimension for the characters in the matrix
        public int[] getDimensions(char char1)
        {

            int[] dimension = new int[2];

            if (char1 == 'j')
            {
                char1 = 'i';
            }
            else if (char1 == 'J')
            {
                char1 = 'I';
            }

            for (i = 0; i < 5; i++)
            {
                for (n = 0; n < 5; n++)
                {
                    if (char1 == ' ')
                    {
                        break;
                    }
                    else
                    {

                        if (char.IsUpper(char1))           //if character is upper convert matrix to upper to compare true
                        {

                            if (char.ToUpper(matrix[i, n]) == char1)
                            {
                                dimension[0] = i;
                                dimension[1] = n;
                                break;
                            }

                        }
                        else
                        {
                            if (char.ToLower(matrix[i, n]) == char1)
                            {
                                dimension[0] = i;
                                dimension[1] = n;
                                break;
                            }

                        }
                    }
                }
            }
            return dimension;
        }

        //function that have all functions to encrypt message by playfaire algorithm
        public string EncryptMsg(string KEY, string MSG)
        {
            string cipherMsg = string.Empty;

            string[] divString = dividestring(MSG);
            char[,] fMatrix = GetMatrix(KEY);

            char c1, c2;

            int[] dimension1 = new int[2];
            int[] dimension2 = new int[2];


            for (int j = 0; j < divString.Length; j++)
            {
                c1 = (divString[j].ToString())[0];  //put first character of string array in c1
                c2 = (divString[j].ToString())[1];



                dimension1 = getDimensions(c1);
                dimension2 = getDimensions(c2);

                //state of the same row
                if (dimension1[0] == dimension2[0])
                {
                    if (dimension1[1] < 4)
                    {
                        dimension1[1]++;
                    }
                    else
                    {
                        dimension1[1] = 0;
                    }
                    if (dimension2[1] < 4)
                    {
                        dimension2[1]++;
                    }
                    else
                    {
                        dimension2[1] = 0;
                    }
                }

                //same couloumn
                else if (dimension1[1] == dimension2[1])
                {
                    if (dimension1[0] < 4)
                    {
                        dimension1[0]++;
                    }
                    else
                    {
                        dimension1[0] = 0;
                    }
                    if (dimension2[0] < 4)
                    {
                        dimension2[0]++;
                    }
                    else
                    {
                        dimension2[0] = 0;
                    }
                }

                //have differrent row and coloumn
                else
                {
                    int temp = dimension1[1];
                    dimension1[1] = dimension2[1];
                    dimension2[1] = temp;
                }

                //to make empty space between words
                if (c1 == ' ' && c2 == ' ')
                {

                    cipherMsg = cipherMsg + c1 + c2;
                }
                else if (c1 == ' ' && c2 != ' ')
                {
                    c1 = ' ';
                    cipherMsg = cipherMsg + c1 + fMatrix[dimension2[0], dimension2[1]];
                }
                else if (c1 != ' ' && c2 == ' ')
                {
                    c2 = ' ';
                    cipherMsg = cipherMsg + fMatrix[dimension1[0], dimension1[1]] + c2;
                }
                else
                {

                    if (char.IsLower(c1) && char.IsLower(c2))
                    {
                        cipherMsg = cipherMsg + fMatrix[dimension1[0], dimension1[1]] + fMatrix[dimension2[0], dimension2[1]];
                    }
                    else if (char.IsUpper(c1) && char.IsUpper(c2))
                    {
                        cipherMsg = cipherMsg + char.ToUpper(fMatrix[dimension1[0], dimension1[1]]) + char.ToUpper(fMatrix[dimension2[0], dimension2[1]]);
                    }
                    else if (char.IsLower(c1) && char.IsUpper(c2))
                    {
                        cipherMsg = cipherMsg + fMatrix[dimension1[0], dimension1[1]] + char.ToUpper(fMatrix[dimension2[0], dimension2[1]]);
                    }
                    else
                    {
                        cipherMsg = cipherMsg + char.ToUpper(fMatrix[dimension1[0], dimension1[1]]) + fMatrix[dimension2[0], dimension2[1]];
                    }
                }

            }
            return cipherMsg;
        }

        //function that have all functions to decrypt message by playfaire algorithm
        public string DecryptMsg(string key, string msg)
        {
            string cipherMsg = string.Empty;

            string[] divString = dividestring(msg);
            char[,] fMatrix = GetMatrix(key);

            char c1, c2;

            int[] dimension1 = new int[2];
            int[] dimension2 = new int[2];


            for (int j = 0; j < divString.Length; j++)
            {
                c1 = (divString[j].ToString())[0];  //put first character of string array in c1
                c2 = (divString[j].ToString())[1];

                dimension1 = getDimensions(c1);
                dimension2 = getDimensions(c2);

                //state of the same row
                if (dimension1[0] == dimension2[0])
                {
                    if (dimension1[1] > 0)
                    {
                        dimension1[1]--;
                    }
                    else
                    {
                        dimension1[1] = 4;
                    }
                    if (dimension2[1] > 0)
                    {
                        dimension2[1]--;
                    }
                    else
                    {
                        dimension2[1] = 4;
                    }
                }

                //same couloumn
                else if (dimension1[1] == dimension2[1])
                {
                    if (dimension1[0] > 0)
                    {
                        dimension1[0]--;
                    }
                    else
                    {
                        dimension1[0] = 4;
                    }
                    if (dimension2[0] > 0)
                    {
                        dimension2[0]--;
                    }
                    else
                    {
                        dimension2[0] = 4;
                    }
                }

                //have differrent row and coloumn
                else
                {
                    int temp = dimension1[1];
                    dimension1[1] = dimension2[1];
                    dimension2[1] = temp;
                }

                if (c1 == ' ' && c2 == ' ')
                {
                    c1 = ' ';
                    c2 = ' ';
                    cipherMsg = cipherMsg + c1 + c2;
                }
                else if (c1 == ' ' && c2 != ' ')
                {
                    c1 = ' ';
                    cipherMsg = cipherMsg + c1 + fMatrix[dimension2[0], dimension2[1]];
                }
                else if (c1 != ' ' && c2 == ' ')
                {
                    c2 = ' ';
                    cipherMsg = cipherMsg + fMatrix[dimension1[0], dimension1[1]] + c2;
                }
                else
                {

                    //////////////////
                    if (char.IsLower(c1) && char.IsLower(c2))
                    {
                        cipherMsg = cipherMsg + fMatrix[dimension1[0], dimension1[1]] + fMatrix[dimension2[0], dimension2[1]];
                    }
                    else if (char.IsUpper(c1) && char.IsUpper(c2))
                    {
                        cipherMsg = cipherMsg + char.ToUpper(fMatrix[dimension1[0], dimension1[1]]) + char.ToUpper(fMatrix[dimension2[0], dimension2[1]]);
                    }
                    else if (char.IsLower(c1) && char.IsUpper(c2))
                    {
                        cipherMsg = cipherMsg + fMatrix[dimension1[0], dimension1[1]] + char.ToUpper(fMatrix[dimension2[0], dimension2[1]]);
                    }
                    else
                    {
                        cipherMsg = cipherMsg + char.ToUpper(fMatrix[dimension1[0], dimension1[1]]) + fMatrix[dimension2[0], dimension2[1]];
                    }
                }

            }

            StringBuilder st = new StringBuilder(cipherMsg);
            for (i = 0; i + 1 < st.Length; i++)
            {
                if (st[i] == 'x' || st[i] == 'X')
                {
                    if (st[i - 1] == st[i + 1])
                        st.Remove(i, 1);
                    else
                        continue;
                }
            }
            if (finalMsg.Length != st.Length)
            {
                st.Remove((st.Length) - 1, 1);
                return st.ToString();
            }
            else
                return st.ToString();

        }


        //des

        public string permute(string plain, byte[] table)
        {
            StringBuilder builder = new StringBuilder();


            foreach (byte b in table)
            {
                builder.Append(plain[b - 1]);
            }


            return builder.ToString();

        }

        public string rotateFunc(string inp, byte shifts)
        {
            StringBuilder b1 = new StringBuilder(inp);
            for (int i = 0; i < shifts; i++)
            {
                char chr = b1[0];
                b1.Remove(0, 1);
                b1.Append(chr);
            }
            string rotatedKey = b1.ToString();
            while (rotatedKey.Length < inp.Length)
            {
                rotatedKey = "0" + rotatedKey;
            }
            return rotatedKey;
        }

        public void generateKey()
        {
            
                 if (textBox1.Text.Length < 8)
                {
                    MessageBox.Show("You Should Enter 8 character");
                }
            
            DesKey = StringToBinary(DesKey);

            DesKey = permute(DesKey, PC1);

            prevLeft = DesKey.Substring(0, DesKey.Length / 2);
            prevRight = DesKey.Substring(DesKey.Length / 2);
            string newLeft, newRight, newSubKey;
            for (int i = 0; i < 16; i++)
            {
                newLeft = rotateFunc(prevLeft, shiftsArray[i]);
                newRight = rotateFunc(prevRight, shiftsArray[i]);

                newSubKey = newLeft + newRight;
                prevLeft = newLeft;
                prevRight = newRight;

                newSubKey = permute(newSubKey, PC2);
                subKeys.Add(newSubKey);
            }

        }

        public string DBoxFunc(string rPlain)
        {
            StringBuilder biulder = new StringBuilder();
            foreach (byte index in D_BOX)
            {
                biulder.Append(rPlain[index - 1]);
            }
            rightPlain = biulder.ToString();
            return rightPlain;
        }

        public string toBinary(string inp)
        {
            StringBuilder sb1 = new StringBuilder();
            foreach (char index in inp.ToCharArray())
            {
                sb1.Append(Convert.ToString(index, 2).PadLeft(8, '0'));
            }
            return sb1.ToString();
        }
       

        private static String XOR(String firstPar, String secondPar)
        {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < firstPar.Length; i++)
            {
                output.Append(firstPar[i] ^ secondPar[i]);
            }
            return output.ToString();
        }


        public void mixer(string key)
        {
            string temp = rightPlain;
            string newright = DBoxFunc(rightPlain);
            newright = XOR(newright, key);
            newright = SBOX(newright);
            newright = permute(newright, P);
            rightPlain = XOR(newright, leftPlain);
            leftPlain = temp;

        }

        public int binaryToDecimal(string binary)
        {
            int num, binary_val, decimal_val = 0, base_val = 1, rem;

            num = int.Parse(binary); /* maximum five digits */
            binary_val = num;
            while (num > 0)
            {
                rem = num % 10;
                decimal_val = decimal_val + rem * base_val;
                num = num / 10;
                base_val = base_val * 2;
            }
            return decimal_val;
        }

        //from 48bit to 32bit
        private String SBOX(String newright)
        {
            if (newright.Length != 48)
            {
                return null;
            }

            int row, coulm, sboxValue, x, y;

            StringBuilder sb = new StringBuilder();
            string sub6char, str, fresult;
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < 48; i += 6)
            {
                sub6char = newright.Substring(i, 6);

                s.Append(sub6char[0]);
                s.Append(sub6char[5]);
                //row = Convert.ToInt32(s.ToString());
                row = binaryToDecimal(s.ToString());

                s.Clear();

                str = sub6char.Substring(1, 4).ToString();
                  coulm = binaryToDecimal(str);
               // coulm = Convert.ToInt32(str);

                 sboxValue = (row * 16) + coulm;
                sboxValue = S_BOX[i / 6, sboxValue];
                fresult = Convert.ToString(sboxValue, 2);


                while (fresult.Length < 4)
                {
                    fresult = "0" + fresult;
                }
                sb.Append(fresult);

            }
            rightPlain = sb.ToString();
            return rightPlain;
        }

     
      public static string StringToBinary(string data)
          {
            StringBuilder sb = new StringBuilder();

            foreach (char c in data.ToCharArray())
            {
                sb.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
            }
            return sb.ToString();
          }


        public static string BinaryToString(string data)
        {
            List<Byte> byteList = new List<Byte>();

            for (int i = 0; i < data.Length; i += 8)
            {
                byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            }
            return Encoding.Default.GetString(byteList.ToArray());
        }


        public string DesEncrypt(string mesage)
        {
            string plain;
            string strperm;
            string final;

            generateKey();

           if (mesage.Length % 8 != 0)
                for (int i = 0; i < mesage.Length % 8; i++)
                
                    mesage += "0";
                    

            StringBuilder resu = new StringBuilder();
            for (int p = 0; p < mesage.Length / 8; p++)
            {
                plain = mesage.Substring(8 * p ,  8);
                plain = StringToBinary(plain);
                plain = permute(plain, IP);
                leftPlain = plain.Substring(0, plain.Length / 2);
                rightPlain = plain.Substring(plain.Length / 2);


                for (int i = 0; i < 16; i++)
                {
                    mixer(subKeys[i]);
                    


                }
                strperm = rightPlain + leftPlain;
               
                final = permute(strperm, IP_INVERSE);
                final = BinaryToString(final);
                resu.Append(final);


            }
            return resu.ToString();


         


        }

        public string DesDEcrypt(string cipher)
        {

           

            string plain;
            string strperm;
            string final;

           

            StringBuilder resu = new StringBuilder();

            for (int p = 0; p < cipher.Length / 8; p++)
            {
                plain = cipher.Substring(8 * p, 8);
                plain = StringToBinary(plain);
                plain = permute(plain, IP);
                leftPlain = plain.Substring(0, plain.Length / 2);
                rightPlain = plain.Substring(plain.Length / 2);


                for (int i = 15;i>=0; i--)
                {
                    mixer(subKeys[i]);

                }

                strperm = rightPlain + leftPlain;
               
                strperm = permute(strperm, IP_INVERSE);
                final = BinaryToString(strperm);
               resu.Append(final);

            }
            return resu.ToString();



        }

        //RC4 FUNCTION

        public static string RC4Encrypt(string input, string key)
        {
            StringBuilder result = new StringBuilder();
            int x, y, j = 0;
            int[] box = new int[256];

            for (int i = 0; i < 256; i++)
            {
                box[i] = i;           //step1 intialization
            }

            for (int i = 0; i < 256; i++)                        //step2 initialization
            {
                j = (key[i % key.Length] + box[i] + j) % 256;     //to get index j

                x = box[i];                                      //swapping
                box[i] = box[j];
                box[j] = x;
            }

            for (int i = 0; i < input.Length; i++)
            {
                y = i % 256;
                j = (box[y] + j) % 256;

                x = box[y];             //swapping
                box[y] = box[j];
                box[j] = x;

                result.Append((char)(input[i] ^ box[(box[y] + box[j]) % 256]));
            }
            return result.ToString();
        }

        public static string RC4decrypt(string input, string key)
        {
            return RC4Encrypt(input, key);


        }
    }
}

