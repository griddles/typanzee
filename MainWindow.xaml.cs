using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace typanzee
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public userSettings userSettings;

        public bool testStarted;
        public DateTime startTime;
        public DispatcherTimer wpmTimer = new();

        private readonly string wordList = "the be of and to in he have it that for they with as not on she at by this we you do but from or which one would all will there say who make when can more if no man out other so what time up go about than into could state only new year some take come these know see use get like then first any work now may such give over think most even find day also after way many must look before great back through long where much should well people down own just because good each those feel seem how high too place little world very still nation hand old life tell write become here show house both between need mean call develop under last right move thing general school never same another begin while number part turn real leave might want point form off child few small since against ask late home interest large person end open public follow during present without again hold govern around possible head consider word program problem however lead system set order eye plan run keep face fact group play stand increase early course change help line";

        public string typingTest;
        public int charsTyped;

        public float currentWPM;
        public float currentHigh;

        public int currentWordCount = 25;

        public string mode = "word";
        public Label previousMode;
        public int testDuration = 30;

        public MainWindow()
        {
            InitializeComponent();

            userSettings = LoadSettings();

            try
            {
                
            }
            catch
            {
                SaveSettings(new userSettings());
            }

            if (userSettings == null || userSettings == new userSettings())
            {
                userSettings = new userSettings();
                SaveSettings(userSettings);
            }
            
            SetColors(userSettings);

            previousMode = word25;
            
            wpmTimer.Tick += wpmTimer_Tick;
            wpmTimer.Interval = new TimeSpan(0, 0, 0, 0, 25);

            typingTest = RandomWords(wordList, currentWordCount);
            typeText.Text = typingTest.ToLower();
            textInput.MaxLength = typingTest.Length;
        }

        public string RandomWords(string words, int wordCount)
        {
            Random random = new Random();

            string[] wordList = words.Split(" "); // splits apart the words into a list (easier to work with)
            string[] output = new string[wordCount]; // makes a new list to save the newly generated words in

            int[] last3 = new int[3]; // something to keep track of the last 3 words we generated, to prevent duplicates

            for (int i = 0; i < wordCount; i++) // the stuff inside the brackets here runs once for every word in the wordList
            {
                int index = random.Next(wordList.Length); // gets a random word from the wordList
                while (last3.Contains(index)) // this while loop just checks if its a duplicate
                {
                    index = random.Next(wordList.Length);
                }
                output[i] = wordList[index]; // adds the new word to the output list
                last3[i % 3] = index; // updates the duplicate checking stuff
            }

            return string.Join(" ", output); // turns the list into a string (easier to use later) and returns it
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
                if (typingTest.Length < 800 && mode == "time")
                {
                    typingTest += " " + RandomWords(wordList, 25);
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
                    typeText.Inlines.Add(new Run(typedText[i].ToString()) { Foreground = (Brush)new BrushConverter().ConvertFromString(userSettings.tertiary)! });
                }
                else
                {
                    typeText.Inlines.Add(new Run(typedText[i].ToString()) { Foreground = Brushes.Red });
                }
            }

            typeText.Inlines.Add(new Run(typingTest.Substring(textInput.Text.Length)) { Foreground = (Brush)new BrushConverter().ConvertFromString(userSettings.primary)! });

            TimeSpan currentTime = DateTime.Now.Subtract(startTime);
            if (testStarted && mode == "word" && typedText.Length == typingTest.Length)
            {
                EndTest(currentTime);
            }
        }

        private void restartButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Restart(currentWordCount);
        }

        private void wpmTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan currentTime = DateTime.Now.Subtract(startTime);

            timeLabel.Content = $"{currentTime.Minutes:00}:{currentTime.Seconds:00}.{currentTime.Milliseconds:000}";

            try
            {
                double time = (Convert.ToDouble(currentTime.Minutes) + (Convert.ToDouble(currentTime.Seconds) / 60)) + Convert.ToDouble(currentTime.Milliseconds) / 60000;

                currentWPM = (float)(Convert.ToDouble(charsTyped) / 5 / time);

                wpmLabel.Content = MathF.Round(currentWPM);
            }
            catch
            {
                // do nothing hehe
            }

            if (testStarted && mode == "time" && (currentTime.Seconds + (currentTime.Minutes * 60)) >= testDuration)
            {
                EndTest(currentTime);
            }
        }

        private void Restart(int wordCount)
        {
            textInput.Text = "";
            testStarted = false;
            textInput.IsReadOnly = false;
            typingTest = RandomWords(wordList, wordCount);
            typeText.Text = typingTest.ToLower();
            textInput.MaxLength = typingTest.Length;
            wpmHighLabel.Content = Math.Round(currentHigh);
            wpmTimer.Stop();
            timeLabel.Content = "00:00.000";
            charsTyped = 0;
            SaveSettings(userSettings);
        }
        
        public static userSettings LoadSettings()
        {
            string json = File.ReadAllText(@"C:\ProgramData\typanzee\settings.json");
            return JsonSerializer.Deserialize<userSettings>(json);
        }

        public static void SaveSettings(userSettings settings)
        {
            string json = JsonSerializer.Serialize(settings);
            Directory.CreateDirectory(@"C:\ProgramData\typanzee");
            File.WriteAllText(@"C:\ProgramData\typanzee\settings.json", json);
        }

        public void SetColors(userSettings settings)
        {
            BrushConverter convert = new BrushConverter(); // good practice and such
            
            typeText.Foreground = (Brush)convert.ConvertFromString(settings.primary)!; // TODO: could potentially cause problems with changing colors mid test, maybe cancel tests before changing colors?
            logo.Foreground = (Brush)convert.ConvertFromString(settings.secondary)!;
            logoImg.Fill = (Brush)convert.ConvertFromString(settings.secondary)!;
            grid.Background = (Brush)convert.ConvertFromString(settings.background)!;
        }
        
        private void palette_MouseDown(object sender, MouseButtonEventArgs e)
        {
            PaletteEditor paletteEditor = new PaletteEditor();
            paletteEditor.Show();
        }
        
        private void EndTest(TimeSpan currentTime) // why is there no way to do this legitimately
        {
            wpmTimer.Stop();
            textInput.IsReadOnly = true;

            timeLabel.Content = string.Format("{0:00}:{1:00}.{2:000}", currentTime.Minutes, currentTime.Seconds, currentTime.Milliseconds);

            if (currentWPM > currentHigh)
            {
                switch (mode)
                {
                    case "word":
                    {
                        switch (currentWordCount)
                        {
                            case 10:
                            {
                                userSettings.high10 = currentWPM;
                                break;
                            }
                            case 25:
                            {
                                userSettings.high25 = currentWPM;
                                break;
                            }
                            case 50:
                            {
                                userSettings.high50 = currentWPM;
                                break;
                            }
                            case 100:
                            {
                                userSettings.high100 = currentWPM;
                                break;
                            }
                        }
                        break;
                    }
                    case "time":
                    {
                        switch (testDuration)
                        {
                            case 15:
                            {
                                userSettings.high15 = currentWPM;
                                break;
                            }
                            case 30:
                            {
                                userSettings.high30 = currentWPM;
                                break;
                            }
                            case 60:
                            {
                                userSettings.high60 = currentWPM;
                                break;
                            }
                            case 120:
                            {
                                userSettings.high120 = currentWPM;
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }

        // all the mousedown bs
        private void word10_MouseDown(object sender, MouseButtonEventArgs e) // this is all dumb
        {
            mode = "word";
            currentWordCount = 10;
            currentHigh = userSettings.high10;
            previousMode.Foreground = Brushes.DarkSlateGray;
            word10.Foreground = Brushes.Gainsboro;
            previousMode = word10;
            Restart(currentWordCount);
        }
        private void word25_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mode = "word";
            currentWordCount = 25;
            currentHigh = userSettings.high25;
            previousMode.Foreground = Brushes.DarkSlateGray;
            word25.Foreground = Brushes.Gainsboro;
            previousMode = word25;
            Restart(currentWordCount);
        }
        private void word50_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mode = "word";
            currentWordCount = 50;
            currentHigh = userSettings.high50;
            previousMode.Foreground = Brushes.DarkSlateGray;
            word50.Foreground = Brushes.Gainsboro;
            previousMode = word50;
            Restart(currentWordCount);
        }
        private void word100_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mode = "word";
            currentWordCount = 100;
            currentHigh = userSettings.high100;
            previousMode.Foreground = Brushes.DarkSlateGray;
            word100.Foreground = Brushes.Gainsboro;
            previousMode = word100;
            Restart(currentWordCount);
        }

        private void time15_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mode = "time";
            testDuration = 15;
            currentWordCount = 100;
            currentHigh = userSettings.high15;
            previousMode.Foreground = Brushes.DarkSlateGray;
            time15.Foreground = Brushes.Gainsboro;
            previousMode = time15;
            Restart(currentWordCount);
        }
        private void time30_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mode = "time";
            testDuration = 30;
            currentWordCount = 100;
            currentHigh = userSettings.high30;
            previousMode.Foreground = Brushes.DarkSlateGray;
            time30.Foreground = Brushes.Gainsboro;
            previousMode = time30;
            Restart(currentWordCount);
        }
        private void time60_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mode = "time";
            testDuration = 60;
            currentWordCount = 100;
            currentHigh = userSettings.high60;
            previousMode.Foreground = Brushes.DarkSlateGray;
            time60.Foreground = Brushes.Gainsboro;
            previousMode = time60;
            Restart(currentWordCount);
        }
        private void time120_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mode = "time";
            testDuration = 120;
            currentWordCount = 100;
            currentHigh = userSettings.high120;
            previousMode.Foreground = Brushes.DarkSlateGray;
            time120.Foreground = Brushes.Gainsboro;
            previousMode = time120;
            Restart(currentWordCount);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        
    }
}