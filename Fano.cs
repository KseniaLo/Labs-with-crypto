using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Lab_4
{
    class Fano
    {
        string text;
        List<char> symbol = new List<char>();
        List<int> ammount;
        List<double> P;
        List<int> L;
        string[] C;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        // Search of non-repeating numbers.
        private List<char> FindSymbols(string text)
        {
            bool exist = false;

            symbol.Add(text[0]);

            for (int i = 1; i < text.Length; i++)
            {
                for (int j = 0; j < symbol.Count; j++)
                    if (text[i] == symbol[j] || String.Compare(text[i].ToString(), symbol[j].ToString(), true) == 0)
                    {
                        exist = true;
                        break;
                    }
                if (!exist && text[i].ToString() != "\r" && text[i].ToString() != "\n")
                    symbol.Add(text[i]);
                exist = false;
            }
            return symbol;
        }

        // Count the amount of incomes of all found symbols.
        private List<int> Count(List<char> symbol, string text)
        {
            ammount = new List<int>(symbol.Count);

            for (int j = 0; j < symbol.Count; j++)
                ammount.Add(0);

            for (int i = 0; i < text.Length; i++)
                for (int j = 0; j < symbol.Count; j++)
                {
                    if (text[i] == symbol[j] || String.Compare(text[i].ToString(), symbol[j].ToString(), true) == 0)
                    {
                        ammount[j]++;
                        break;
                    }
                }
            return ammount;
        }

        // Calculate probabilities.
        private List<double> Probabilities(List<double> P)
        {
            P = new List<double>();

            for (int j = 0; j < ammount.Count; j++)
                P.Add(0);

            double sum = 0;
            for (int i = 0; i < ammount.Count; i++)
                sum += ammount[i];
            for (int i = 0; i < ammount.Count; i++)
                P[i] = (double)ammount[i] / sum;
            sum = 0;

            return P;
        }

        // Sorting of elements.
        private List<char> Sort(List<int> ammount, List<char> symbol)
        {
            List<char> copySymbols = new List<char>();
            List<int> copyAmmount = new List<int>();
            for (int j = 0; j < symbol.Count; j++)
                copyAmmount.Add(ammount[j]);
            for (int j = 0; j < symbol.Count; j++)
                copySymbols.Add(symbol[j]);

            copyAmmount.Sort();
            copyAmmount.Reverse();

            int index;

            for (int i = 0; i < symbol.Count; i++)
            {
                index = i;
                for (int j = 0; j < symbol.Count; j++)
                    if (ammount[i] == copyAmmount[j])
                        copySymbols[i] = symbol[j];
            }

            return copySymbols;
        }

        // Average length of the code-word.
        private void AverageLength(List<double> P, List<int> L, RichTextBox rtb)
        {
            double sum = 0;
            for (int i = 0; i < P.Count; i++)
                sum += (P[i] * L[i]);

            rtb.AppendText("Средняя длина по Фано = " + Math.Round(sum, 3).ToString() + "\n\r");
        }

        // Fano Algorithm.
        private void Algorithm(ref List<double> P, ref string[] C, ref List<int> L, int l, int r)
        {
            if (r == l)
                return;
            double sum = 0, sumd = 0;
            for (int i = l; i <= r; i++)
                sum += P[i];
            int m = r;
            while (Math.Abs(sum - sumd) > Math.Abs(sum - sumd - 2 * P[m]))
            {
                sum -= P[m];
                sumd += P[m];
                m--;
            }
            for(int i=l;i<=r;i++)
            {
                C[i] += (i <= m ? '0' : '1').ToString();
                L[i]++;
            }

            Algorithm(ref P, ref C, ref L, l, m);
            Algorithm(ref P, ref C, ref L, m + 1, r);
        }

        // Получение кодов смволов.
        public void GetCodes(DataGridView dgv, RichTextBox rtb)
        {
            symbol = FindSymbols(text);
            ammount = Count(symbol, text);
            symbol = Sort(ammount, symbol);
            ammount.Sort();
            ammount.Reverse();
            P = Probabilities(P);

            L = new List<int>();
            for (int i = 0; i < P.Count; i++)
                L.Add(0);

            C = new string[P.Count];

            Algorithm(ref P, ref C, ref L, 0, P.Count - 1);

            AverageLength(P, L, rtb);

            dgv.RowCount = P.Count;

            for (int i = 0; i < P.Count; i++)
            {
                dgv[0, i].Value = symbol[i].ToString();
                dgv[1, i].Value = P[i].ToString();
                dgv[2, i].Value = C[i].ToString();
                dgv[3, i].Value = L[i].ToString();
            }
        }
    }
}