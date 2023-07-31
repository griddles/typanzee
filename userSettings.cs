using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace typanzee
{
    public class userSettings
    {
        // highscores
        public float high10 { get; set; }
        public float high25 { get; set; }
        public float high50 { get; set; }
        public float high100 { get; set; }
        public float high15 { get; set; }
        public float high30 { get; set; }
        public float high60 { get; set; }
        public float high120 { get; set; }
        
        public string primary { get; set; }
        public string secondary { get; set; }
        public string tertiary { get; set; }
        public string background { get; set; }
        
        public userSettings()
        {
            primary = Brushes.Gainsboro.ToString();
            secondary = Brushes.Gainsboro.ToString();
            tertiary = Brushes.DarkSlateGray.ToString();
            background = Brushes.Black.ToString();
        }

        // ignore the static i dont know what it means either i just put it there when the error messages tell me to
        public static int AddNumbers(int a, int b) 
        {
            return a + b;
        }

        static void main()
        {
            int a = 5;
            int b = 2;
            int c = AddNumbers(a, b);
            // in this case, c = 7 now because the function added a and b together, and then returned the value into c
        }
    }
}
