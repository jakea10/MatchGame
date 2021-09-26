using System;
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

namespace MatchGame
{
    using System.Windows.Threading;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            // Create a list of 8 pairs of animal emojis
            List<string> animalEmoji = new List<string>()
            {
                "🦊","🦊",
                "🐱","🐱",
                "🐴","🐴",
                "🐔","🐔",
                "🐰","🐰",
                "🐷","🐷",
                "🐮","🐮",
                "🐶","🐶",
            };

            // Create a new random number generator
            Random random = new Random();

            // Find every TextBlock in the main grid and
            // repeat the following statements for each one
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    // Pick a random number between 0 and the num. of emojis
                    // left in the list and call it "index"
                    int index = random.Next(animalEmoji.Count);

                    // Use the random num. called "index" to get a random emoji from the list
                    string nextEmoji = animalEmoji[index];

                    // Update the TextBlock with the random emoji from the list
                    textBlock.Text = nextEmoji;

                    // Remove the random emoji from the list
                    animalEmoji.RemoveAt(index);
                }
            }
            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        private TextBlock lastTextBlockClicked;
        private bool findingMatch = false; // keep track if player just clicked first animal in pair, and trying to find match      

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            // player clicked first animal in pair; make it invisible, track its TextBlock
            // in case it needs to make it visible again
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }

            // player found match; make second animal in pair invisible (unclickable)
            // reset findingMatch so next animal clicked is first one in pair again
            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }

            // player clicked on non-matching animal
            // make first animal clicked visible again, reset findingMatch
            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
