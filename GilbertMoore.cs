using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Lab_4
{
    class GilbertMoore
    {
        string text;
        List<char> symbol = new List<char>();
        List<int> ammount;
        List<double> P;
        List<double> Q;
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

        // Calculation of cumulative probabilities.
        private List<double> Cumulative(List<int> ammount, List<double> P)
        {
            Q = new List<double>();

            for (int j = 0; j < ammount.Count + 1; j++)
                Q.Add(0);

            double sum = 0;

            for (int i = 0; i < ammount.Count; i++)
            {
                Q[i] = sum;
                sum += P[i];
            }
            Q[ammount.Count] = sum;

            return Q;
        }

        // Average length of the code-word.
        private void AverageLength(List<double> P, List<int> L, RichTextBox rtb)
        {
            double sum = 0;
            for (int i = 0; i < P.Count; i++)
                sum += (P[i] * L[i]);

            rtb.AppendText("Средняя длина по Гилберту-Муру = " + Math.Round(sum, 3).ToString() + "\n\r");
        }


        // Get symbol codes.
        public void GetCodes(DataGridView dgv, RichTextBox rtb)
        {
            symbol = FindSymbols(text);
            ammount = Count(symbol, text);
            symbol = Sort(ammount, symbol);
            ammount.Sort();
            ammount.Reverse();
            P = Probabilities(P);

            L = new List<int>();
            Q = new List<double>();
            C = new string[P.Count];

            double pr = 0;
            for(int i=0;i<P.Count;i++)
            {
                Q.Add(pr + P[i] / 2);
                pr += P[i];
                L.Add((int)Math.Ceiling(-Math.Log(P[i], 2)) + 1);
            }
            for (int i = 0; i < P.Count; i++)
            {
                for (int j = 0; j < L[i]; j++)
                {
                    Q[i] *= 2;
                    C[i] += ((int)Math.Floor(Q[i])).ToString();
                    Q[i] -= (int)Math.Floor(Q[i]);
                }
            }

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
