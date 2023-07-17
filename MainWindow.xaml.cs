using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace monkeytype
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool testStarted;
        public DateTime startTime;
        public DispatcherTimer wpmTimer = new DispatcherTimer();

        public string wordList = "the be of and to in he have it that for they with as not on she at by this we you do but from or which one would all will there say who make when can more if no man out other so what time up go about than into could state only new year some take come these know see use get like then first any work now may such give over think most even find day also after way many must look before great back through long where much should well people down own just because good each those feel seem how high too place little world very still nation hand old life tell write become here show house both between need mean call develop under last right move thing general school never same another begin while number part turn real leave might want point form off child few small since against ask late home interest large person end open public follow during present without again hold govern around possible head consider word program problem however lead system set order eye plan run keep face fact group play stand increase early course change help line";

        public string typingTest;
        public int charsTyped;

        public int currentWordCount = 25;

        public string mode = "words";

        public MainWindow()
        {
            InitializeComponent();
            
            wpmTimer.Tick += new EventHandler(wpmTimer_Tick);
            wpmTimer.Interval = new TimeSpan(0, 0, 0, 0, 25);

            typingTest = RandomWords(wordList, currentWordCount);
            typeText.Text = typingTest.ToLower();
            textInput.MaxLength = typingTest.Length;
        }

        public string RandomWords(string words, int wordCount)
        {
            Random random = new Random();

            string[] wordList = words.Split(" ");
            string[] output = new string[wordCount];

            int[] last3 = new int[3];

            for (int i = 0; i < wordCount; i++)
            {
                int index = random.Next(wordList.Length);
                while (last3.Contains(index))
                {
                    index = random.Next(wordList.Length);
                }
                output[i] = wordList[index];
                last3[i % 3] = index;
            }

            return string.Join(" ", output);
        }

        private void textInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            charsTyped++;

            try
            {
                if (textInput.GetLineIndexFromCharacterIndex(textInput.Text.Length - 1) > 1 && testStarted)
                {
                    int line1Index = 0;

                    for (int i = 0; i < typeText.Text.Length; i++)
                    {
                        if (textInput.GetLineIndexFromCharacterIndex(i) != 0)
                        {
                            line1Index = i;
                            break;
                        }
                    }

                    typingTest = typingTest.Substring(line1Index);
                    textInput.Text = textInput.Text.Substring(line1Index);
                    textInput.CaretIndex = textInput.Text.Length;
                }
            }
            catch
            {
                // do nothing lol
            }
            

            if (!testStarted)
            {
                testStarted = true;
                startTime = DateTime.Now;
                wpmTimer.Start();
            }

            string typedText = textInput.Text;

            typeText.Inlines.Clear();

            for (int i = 0; i < typedText.Length; i++)
            {
                if (typedText[i] == typingTest[i])
                {
                    typeText.Inlines.Add(new Run(typedText[i].ToString()) { Foreground = Brushes.DarkSlateGray });
                }
                else
                {
                    typeText.Inlines.Add(new Run(typedText[i].ToString()) { Foreground = Brushes.Red });
                }
            }

            typeText.Inlines.Add(new Run(typingTest.Substring(textInput.Text.Length)) { Foreground = Brushes.Gainsboro });

            if (testStarted && typedText.Length == typingTest.Length)
            {
                wpmTimer.Stop();

                TimeSpan currentTime = DateTime.Now.Subtract(startTime);

                timeLabel.Content = string.Format("{0:00}:{1:00}.{2:000}", currentTime.Minutes, currentTime.Seconds, currentTime.Milliseconds);
            }
        }

        private void restartButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Restart(currentWordCount);
        }

        private void wpmTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan currentTime = DateTime.Now.Subtract(startTime);

            timeLabel.Content = string.Format("{0:00}:{1:00}.{2:000}", currentTime.Minutes, currentTime.Seconds, currentTime.Milliseconds);

            try
            {
                double time = (Convert.ToDouble(currentTime.Minutes) + (Convert.ToDouble(currentTime.Seconds) / 60)) + Convert.ToDouble(currentTime.Milliseconds) / 60000;

                wpmLabel.Content = MathF.Round((float)(Convert.ToDouble(charsTyped) / 5 / time));
            }
            catch
            {
                // do nothing hehe
            }
        }

        private void Restart(int wordCount)
        {
            textInput.Text = "";
            testStarted = false;
            typingTest = RandomWords(wordList, wordCount);
            typeText.Text = typingTest.ToLower();
            textInput.MaxLength = typingTest.Length;
            wpmTimer.Stop();
            timeLabel.Content = "00:00.000";
            charsTyped = 0;
        }

        private void word10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentWordCount = 10;
            word10.Foreground = Brushes.Gainsboro;
            word25.Foreground = Brushes.DarkSlateGray;
            word50.Foreground = Brushes.DarkSlateGray; 
            word100.Foreground = Brushes.DarkSlateGray;
            Restart(currentWordCount);
        }
        private void word25_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentWordCount = 25;
            word10.Foreground = Brushes.DarkSlateGray;
            word25.Foreground = Brushes.Gainsboro;
            word50.Foreground = Brushes.DarkSlateGray;
            word100.Foreground = Brushes.DarkSlateGray;
            Restart(currentWordCount);
        }
        private void word50_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentWordCount = 50;
            word10.Foreground = Brushes.DarkSlateGray;
            word25.Foreground = Brushes.DarkSlateGray;
            word50.Foreground = Brushes.Gainsboro;
            word100.Foreground = Brushes.DarkSlateGray;
            Restart(currentWordCount);
        }
        private void word100_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentWordCount = 100;
            word10.Foreground = Brushes.DarkSlateGray;
            word25.Foreground = Brushes.DarkSlateGray;
            word50.Foreground = Brushes.DarkSlateGray;
            word100.Foreground = Brushes.Gainsboro;
            Restart(currentWordCount);
        }

        private void time15_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentWordCount = 10;
            word10.Foreground = Brushes.Gainsboro;
            word25.Foreground = Brushes.DarkSlateGray;
            word50.Foreground = Brushes.DarkSlateGray;
            word100.Foreground = Brushes.DarkSlateGray;
            Restart(currentWordCount);
        }
        private void time30_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentWordCount = 25;
            word10.Foreground = Brushes.DarkSlateGray;
            word25.Foreground = Brushes.Gainsboro;
            word50.Foreground = Brushes.DarkSlateGray;
            word100.Foreground = Brushes.DarkSlateGray;
            Restart(currentWordCount);
        }
        private void time60_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentWordCount = 50;
            word10.Foreground = Brushes.DarkSlateGray;
            word25.Foreground = Brushes.DarkSlateGray;
            word50.Foreground = Brushes.Gainsboro;
            word100.Foreground = Brushes.DarkSlateGray;
            Restart(currentWordCount);
        }
        private void time120_MouseDown(object sender, MouseButtonEventArgs e)
        {
            currentWordCount = 100;
            word10.Foreground = Brushes.DarkSlateGray;
            word25.Foreground = Brushes.DarkSlateGray;
            word50.Foreground = Brushes.DarkSlateGray;
            word100.Foreground = Brushes.Gainsboro;
            Restart(currentWordCount);
        }
    }
}