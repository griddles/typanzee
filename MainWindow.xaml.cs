using System;
using System.Collections;
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

        public MainWindow()
        {
            InitializeComponent();
            
            wpmTimer.Tick += new EventHandler(wpmTimer_Tick);
            wpmTimer.Interval = new TimeSpan(0, 0, 0, 0, 25);

            typingTest = RandomWords(wordList, 25);
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
            textInput.Text = "";
            typingTest = RandomWords(wordList, 25);
            typeText.Text = typingTest.ToLower();
            textInput.MaxLength = typingTest.Length;
            wpmTimer.Stop();
            testStarted = false;
            timeLabel.Content = "00:00.000";
        }

        private void wpmTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan currentTime = DateTime.Now.Subtract(startTime);

            timeLabel.Content = string.Format("{0:00}:{1:00}.{2:000}", currentTime.Minutes, currentTime.Seconds, currentTime.Milliseconds);

            try
            {
                wpmLabel.Content = MathF.Round((float)((Convert.ToDouble(textInput.Text.Length) / 5) / (Convert.ToDouble(currentTime.Minutes) + (Convert.ToDouble(currentTime.Seconds) / 60)) + Convert.ToDouble(currentTime.Milliseconds) / 6000));
            }
            catch
            {
                // do nothing hehe
            }
        }
    }
}