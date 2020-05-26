using MaterialDesignThemes.Wpf.Converters.CircularProgressBar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Idemy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += new EventHandler(timerTick);
        }

        bool isDragging = false;

        void timerTick(object Sender, EventArgs e)
        {
            if(!isDragging)
            {
                seekBarPlayer.Value = mainVideo.Position.TotalSeconds;
            }
        }

        private void seekBarPlayer_DragStarted(object sender, DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void seekBarPlayer_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            isDragging = false;
            mainVideo.Position = TimeSpan.FromSeconds(seekBarPlayer.Value);
        }

        private void mainVideo_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (mainVideo.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = mainVideo.NaturalDuration.TimeSpan;
                seekBarPlayer.Maximum = ts.TotalSeconds;
                seekBarPlayer.SmallChange = 1;
                seekBarPlayer.LargeChange = Math.Min(10, ts.Seconds / 10);
            }
            timer.Start();
            mainVideo.LoadedBehavior = MediaState.Manual;
            mainVideo.Play();
        }
        private void seekBarPlayer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mainVideo.Position = TimeSpan.FromSeconds(seekBarPlayer.Value);
        }

        bool paused = false;
        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (paused) mainVideo.Play();
            else mainVideo.Pause();
            paused = !paused;
        }
        Point startPoint;
        private void Canvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            Canvas.SetZIndex(card4, 100);
        }

        private void Canvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                Canvas.SetBottom(card4, diff.Y);
            }
        }

        private void canvasCard4_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Canvas.SetBottom(card4, 0);
        }
    }
}
