using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace Pomodoro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public int Work { get; set; }
        public int IterationCount { get; set; }
        public int SmallBreak { get; set; }
        public int LongBreak { get; set; }
        public int Break { get; set; }
        public int PomodoroCount { get; set; }

        public int Time { get; set; }

        private DispatcherTimer Timer;

        public MainWindow()
        {
            InitializeComponent();

            //Initialize properties in minutes
            Work = 25;
            SmallBreak = 5;
            IterationCount = 3;
            LongBreak = 15;

            //First two iterations of pomodoro have a 5m breaks
            Break = SmallBreak;

            //Amount of seconds for the working time
            Time = Work * 60;
            
            //Initialize Timer
            Timer = new DispatcherTimer();
            
            //Set interval to one second so that Tick event will be called every second
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick += Timer_tick;
            Timer.Stop();
        }

        private void Timer_tick(object sender, EventArgs e)
        {
           
            if(IterationCount > 0)
            {
                //This logic looks bad but it is not that bad.
                //Iteration should go in this sequence: IF => ELSE => ELSE IF => IF...
                if (Time > 0)
                {
                    Time--;
                    DisplayText(Time);

                }
                //When one iteration (25+5) ends, reset values and begin next iteration.
                else if (Work == 0 && Break == 0)
                {

                    SystemSounds.Exclamation.Play();
                    MessageBox.Show(this, "Let's Do it!", "Work", MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);

                    //Reinitialize work and break time for new iteration.
                    Work = 25;
                    //If this is third iteration (n == 1), then use longer break time.
                    Break = (IterationCount > 1) ? SmallBreak : LongBreak;
                    Time = Work * 60;


                    IterationCount--;
                }
                //When work time finishes, start small break time and display message
                else
                {

                    //Setting work and break to 0 so that I can track when iteration ends using previous "else if"
                    Work = 0;
                    //Set time to Break time (5 or 15m)
                    Time = Break * 60;
                    Break = 0;
                    SystemSounds.Exclamation.Play();
                    MessageBox.Show(this, "Free time.", "Rest", MessageBoxButton.OK, MessageBoxImage.Question, MessageBoxResult.OK);

                }
            }
            //One whole pomodoro is finished, start again.
            else 
            {
                PomodoroCount++;
                IterationCount = 3;
            }

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Timer.Start();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Timer.Stop();
        }

        private void btnTimeController_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if (button.Name == "btnLongBreak")
            {
                Timer.Stop();

                Time = LongBreak * 60;

                //Set Work and Break to 0 so that timer_Click logic will start 
                //new iteration after this Break ends. 
                Work = 0;
                Break = 0;

                //Also reset IterationCount. Setting it to 4 because timer_Click's "else if"
                //has IterationCount--. This way it will be 3 after the break ends.
                IterationCount = 4;

                DisplayText(Time);
            }
            else if (button.Name == "btnSmallBreak")
            {
                Timer.Stop();

                Time = SmallBreak * 60;

                Work = 0;
                Break = 0;
                IterationCount = 4;

                DisplayText(Time);
                
            }
            else if (button.Name == "btnWork")
            {
                Timer.Stop();

                Work = 25;
                Time = Work * 60;

                DisplayText(Time);
            }
        }

        public void DisplayText(int Time)
        {
            string mm = (Time / 60) < 10 ? string.Format($"0{Time / 60}") : (Time / 60).ToString();
            string ss = (Time % 60) < 10 ? string.Format($"0{Time % 60}") : (Time % 60).ToString();

            TBCountDown2.Title = "Timer (" + string.Format("{0}:{1}", mm, ss) + ")";
            TBCountDown.Text = string.Format("{0}:{1}", mm, ss);
        }

    }
}
