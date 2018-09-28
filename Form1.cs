using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        ulong P, G, X1, X2;
        ulong Y1, Y2;

        // Searching of open key.
        private void button1_Click(object sender, EventArgs e)
        {
            // Assigning values to variables.
            P = Convert.ToUInt64(textBox1.Text); // P - prime number. P is bigger than the biggest number of the message.
            G = Convert.ToUInt64(textBox2.Text); // Q<P.
            X1 = Convert.ToUInt64(textBox3.Text);
            X2 = Convert.ToUInt64(textBox6.Text);

            // Calculation of open keys.
            Y1 = OpenKey(P, G, X1);
            Y2 = OpenKey(P, G, X2);

            // Showing results.
            textBox4.Text = Y1.ToString();
            textBox7.Text = Y2.ToString();
        }

        // Encrypt.
        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox2.Clear();
            string text = richTextBox1.Text;
            string[] mas = text.Split(new char[] { '(', ')', ' ', '\n', '\r', ',', '.', ';' }, StringSplitOptions.RemoveEmptyEntries);
            richTextBox2.AppendText("(" + Y1.ToString() + " ; ");

            for (int i = 0; i < mas.Length; i++)
            {
                ulong M = Convert.ToUInt64(mas[i]);
                ulong b = B(P, Y2, X1, M);
                richTextBox2.AppendText(b.ToString() + " ");
            }
            richTextBox2.AppendText(")");
        }

        // Decrypt.
        private void button3_Click(object sender, EventArgs e)
        {
            richTextBox3.Clear();
            string text = richTextBox2.Text;
            string[] mas = text.Split(new char[] { '(', ')', ' ', '\n', '\r', ',', '.', ';' }, StringSplitOptions.RemoveEmptyEntries);

            ulong y1 = Convert.ToUInt64(mas[0]);

            for (int i = 1; i < mas.Length; i++)
            {
                ulong m = M(P, y1, Convert.ToUInt64(mas[i]), X2);
                richTextBox3.AppendText(m.ToString() + " ");
            }
        }

        public ulong OpenKey(ulong P, ulong G, ulong X)
        {
            ulong tmp = 1;
            for (ulong i = 0; i < X; i++)
                tmp = ((tmp * G) % P);
            return tmp;
        }

        public ulong B(ulong P, ulong Y2, ulong X1, ulong M)
        {
            ulong tmp = 1;
            for (ulong i = 0; i < X1; i++)
                tmp = ((tmp * Y2) % P);
            tmp = ((tmp * M) % P);
            return tmp;
        }

        public ulong M(ulong P, ulong y1, ulong b, ulong X2)
        {
            ulong tmp = 1;
            for (ulong i = 0; i < P - 1 - X2; i++)
                tmp = ((tmp * y1) % P);
            tmp = ((tmp * b) % P);
            return tmp;
        }
    }
}