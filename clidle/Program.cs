namespace CliIdle
{
    class WordList
    {
        public List<string> words = new List<string>();
        private string path;

        public WordList(string p)
        {
            this.path = p;
            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    words.Add(line);
                }
            }
        }

        public void Write()
        {
            foreach (string w in words)
            {
                Console.WriteLine(w);
            }
        }
    }
    class Word
    {
        public const int maxlet = 5;
        public string Text;
        public string[] Colors = new string[maxlet];

        public Word(string t)
        {
            this.Text = t.ToUpper();
            Array.Fill(Colors,"g");
        }

        public void Write()
        {
            for (int i = 0; i < maxlet; i++)
            {
                // change colors to black here if you have semi transparent terminal
                switch (Colors[i])
                {
                    case "g":
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        // Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    case "w":
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        // Console.ForegroundColor = ConsoleColor.Black;
                        break;
                    case "n":
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        // Console.ForegroundColor = ConsoleColor.Black;
                        break;
                }
                Console.Write(Text[i]+" ");
            }
            Console.ResetColor();
            Console.WriteLine("");
        }

        public void Compare(Word w)
        {
            if (Text == w.Text)
            {
                Array.Fill(Colors,"w");
                return;
            }

            string greens = "";
            for (int i = 0; i < maxlet; i++)
            {
                if (Text[i] == w.Text[i])
                {
                    Colors[i] = "w";
                    greens += Text[i];
                }
                else if (w.Text.Contains(Text[i])&&!(greens.Contains(Text[i])))
                {
                    Colors[i] = "n";
                }
                else
                {
                    Colors[i] = "g";
                }
            }
        }
    }

    class Keyboard
    {
        public Word MainWord;
        public List<Word> Guesses;
        public char[] Letters = new char[26]{'Q','W','E','R','T','Y','U','I','O','P','A','S','D','F','G','H','J','K','L','Z','X','C','V','B','N','M'};
        public string[] Colors = new string[26];

        public Keyboard(Word w, List<Word> wl)
        {
            this.MainWord = w;
            this.Guesses = wl;
            Array.Fill(Colors,"lg");
        }

        public void Recolor(List<Word> gss)
        {
            List<char> ltrs = new List<char>();
            List<string> clrs = new List<string>();
            foreach (Word g in gss)
            {
                for (int i = 0; i < g.Text.Length; i++)
                {
                    if (!(ltrs.Contains(g.Text[i])))
                    {
                        ltrs.Add(g.Text[i]);
                        clrs.Add(g.Colors[i]);
                    }
                    else if (g.Colors[i] == "w")
                    {
                        clrs[ltrs.IndexOf(g.Text[i])] = "w";
                    }
                }
            }

            int newindex;
            for (int i = 0; i < ltrs.Count; i++)
            {
                newindex = Letters.IndexOf(ltrs[i]);
                Colors[newindex] = clrs[i];
            }
        }

        public void Write()
        {
            for (int i = 0; i < 26; i++)
            {
                switch (Colors[i])
                {
                    // change colors from white to black here if you have semi transparent terminal
                    case "g":
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        // Console.ForegroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "w":
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        // Console.ForegroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "n":
                        Console.BackgroundColor = ConsoleColor.DarkYellow;
                        // Console.ForegroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case "lg":
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                        break;
                }
                Console.Write(Letters[i]+" ");
                if(i==9||i==18||i==25) Console.WriteLine("");
            }
            Console.ResetColor();
        }
        
    }
    public class Program
    {
        static bool Check(string s,WordList wll)
        {
            if (s.Length == 5&& wll.words.Contains(s.ToLower())) return true;
            else return false;
        }
        private static Random rand = new Random();
        public static void Main(string[] args)
        {
            WordList wl = new WordList("~/words.txt");
            WordList vg = new WordList("~/valid_guesses.txt");
            bool lost = true;
            const int tries = 6;
            List <Word> guesses = new List<Word>();
            string neword = wl.words[rand.Next(wl.words.Count)];
            Word mainword = new Word(neword);
            string given_word;
            Keyboard k = new Keyboard(mainword,guesses);
            for (int i = 0; i < tries; i++)
            {
                Console.Clear();
                foreach (var z in guesses)
                {
                    z.Write();
                }

                for (int j = guesses.Count; j < tries; j++)
                {
                    Console.WriteLine("_ _ _ _ _ ");
                }
                Console.WriteLine();
                k.Recolor(guesses);
                k.Write();
                Console.WriteLine();
                given_word = Console.ReadLine();
                if (Check(given_word,vg))
                {
                    guesses.Add(new Word(given_word));
                    guesses[i].Compare(mainword);
                    if (guesses[i].Text == mainword.Text)
                    {
                        lost = false;
                        break;
                    }
                }
                else
                {
                    i--;
                }
            }

            if (lost)
            {
                Console.WriteLine($"The word was {mainword.Text}");
            }
            else
            {
                Console.Clear();
                foreach (var z in guesses)
                {
                    z.Write();
                }

                for (int j = guesses.Count; j < tries; j++)
                {
                    Console.WriteLine("_ _ _ _ _ ");
                }
            }
        }
    }
}
