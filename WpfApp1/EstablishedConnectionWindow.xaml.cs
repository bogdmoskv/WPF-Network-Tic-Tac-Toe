using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для EstablishedConnectionWindow.xaml
    /// </summary>
    public partial class EstablishedConnectionWindow : Window
    {
        private readonly DoubleAnimation _oa;
        
        public EstablishedConnectionWindow()
        {
            InitializeComponent();
            _oa = new DoubleAnimation();
            _oa.From = Opacity;
            _oa.To = 0.7;
            _oa.RepeatBehavior = RepeatBehavior.Forever;
            _oa.AutoReverse = true;
            _oa.Duration = new Duration(TimeSpan.FromMilliseconds(800d));

            Timer();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            BeginAnimation(OpacityProperty, _oa);
        }

        private void Timer()
        {
            var timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 4);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
