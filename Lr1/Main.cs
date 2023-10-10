using System.Diagnostics;

namespace Lr1
{
    public partial class Main : Form
    {

        public String Alphabet { get; set; } = "ABCDEFGHIJKLMNOPQRSTUVWXYZ .,0123456789";

        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var keyWord = textBox1.Text.ToUpper();
            var wordK = Convert.ToInt32(numericUpDown1.Value);

            var alphabet = Alphabet.Select(x => x.ToString()).ToList();

            var text = richTextBox1.Text.Trim().ToUpper();

            var newAlphabet = GenCezarAlphabet(alphabet, keyWord, wordK);

            var codedResult = CodeCezar(alphabet, newAlphabet, text);

            richTextBox2.Text = codedResult;

        }

        private string CodeCezar(List<string> alphabet, List<string> newAlphabet, string text)
        {

            var newString = "";

            foreach (var character in text)
            {
                var index = alphabet.IndexOf(character.ToString());
                newString += newAlphabet[index];
            }

            return newString;


        }

        private string DeCodeCezar(List<string> alphabet, List<string> newAlphabet, string text)
        {

            var newString = "";

            foreach (var character in text)
            {
                var index = newAlphabet.IndexOf(character.ToString());
                newString += alphabet[index];
            }

            return newString;


        }


        private List<string> GenCezarAlphabet(List<string> alphabet, string keyWord, int wordK)
        {
            var newAlphabet = new List<string>();
            var keyWordAsList = keyWord.Select(x => x.ToString()).ToList();
            var alphabetWithoutKeyWord = alphabet.Where(x => !keyWord.Contains(x));
            var partToAddFromFiltredAlphabet = alphabetWithoutKeyWord.Take(0..^wordK);
            var startPartToAdd = alphabetWithoutKeyWord.Take(^wordK..^0);

            newAlphabet.AddRange(startPartToAdd);
            newAlphabet.AddRange(keyWordAsList);
            newAlphabet.AddRange(partToAddFromFiltredAlphabet);
            Debug.WriteLine("Новый алфавит: " + String.Join("", newAlphabet));
            return newAlphabet;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var keyWord = textBox1.Text.ToUpper();
            var wordK = Convert.ToInt32(numericUpDown1.Value);

            var alphabet = Alphabet.Select(x => x.ToString()).ToList();

            var text = richTextBox1.Text.Trim().ToUpper();

            var newAlphabet = GenCezarAlphabet(alphabet, keyWord, wordK);

            var codedResult = DeCodeCezar(alphabet, newAlphabet, text);

            richTextBox2.Text = codedResult;
        }


        private string CodeTable(string text, int columns, int rows)
        {
            var tableSize = columns * rows;
            (columns, rows) = (rows, columns);

            var splitedText = text.Chunk(tableSize);

            var rez = "";

            foreach (var chunk in splitedText)
            {
                var tmpArray = new char[rows, columns];

                var tmpString = chunk; 

                if (chunk.Length<tableSize)
                {
                    tmpString = string.Join("", tmpString).PadRight(tableSize, ' ').Select(x=>x).ToArray();
                }

                var chunki = 0;

                for (int i = 0; i < rows; i++)
                {
                    for (int l = 0; l < columns; l++)
                    {
                        tmpArray[i, l] = tmpString[chunki++];
                    }
                }


                for (int i = 0; i < columns; i++)
                {
                    for (int l = 0; l < rows; l++)
                    {
                        if (tmpArray[l, i] != null)
                        {
                            rez += tmpArray[l, i];
                        }
                    }
                }
                PrintArrayToDebug("Code table", tmpArray);



            }

            return rez;

        }

        private void PrintArrayToDebug(string text, char[,] array)
        {
            Debug.WriteLine(text);

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int l = 0; l < array.GetLength(1); l++)
                {
                    Debug.Write("["+array[i,l]+"] ");
                }
                Debug.WriteLine("");

            }
        }

        private string DeCodeTable(string text, int columns, int rows)
        {
            var tableSize = columns * rows;

            var splitedText = text.Chunk(tableSize);

            var rez = "";

            foreach (var chunk in splitedText)
            {
                var tmpArray = new char[columns, rows];

                var tmpString = chunk;

                if (chunk.Length < tableSize)
                {
                    tmpString = string.Join("", tmpString).PadRight(tableSize, ' ').Select(x => x).ToArray();
                }

                var chunki = 0;

                for (int i = 0; i < rows; i++)
                {
                    for (int l = 0; l < columns; l++)
                    {
                        tmpArray[l, i] = tmpString[chunki++];
                    }
                }


                for (int i = 0; i < columns; i++)
                {
                    for (int l = 0; l < rows; l++)
                    {
                        if (tmpArray[i, l] != null)
                        {
                            rez += tmpArray[i, l];
                        }
                    }
                }
                PrintArrayToDebug("Decode table", tmpArray);


            }

            return rez;

        }

        private void button4_Click(object sender, EventArgs e)
        {

            var columns = Convert.ToInt32(numericUpDown2.Value);
            var rows = Convert.ToInt32(numericUpDown3.Value);

            var text = richTextBox4.Text.Trim();

            var codedResult = CodeTable(text, columns, rows);

            richTextBox3.Text = codedResult;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var columns = Convert.ToInt32(numericUpDown2.Value);
            var rows = Convert.ToInt32(numericUpDown3.Value);

            var text = richTextBox4.Text.Trim();

            var codedResult = DeCodeTable(text, columns, rows);

            richTextBox3.Text = codedResult;
        }
    }
}