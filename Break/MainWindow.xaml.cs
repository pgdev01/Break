using System.Drawing;
using System.Media;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Break {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private NotifyIcon tray_icon = new NotifyIcon();
        private DispatcherTimer timer = new DispatcherTimer();
        private SoundPlayer player = new SoundPlayer("audio.wav");

        private bool counting = false;
        private ushort time_passed = 0;

        private const ushort time_limit = 3600;

        private void app_show() {
            tray_icon.Visible = false;
            window.Show();
            window.WindowState = WindowState.Maximized;
            window.Topmost = true;
        }
        private void app_hide() {
            window.Hide();
            window.WindowState = WindowState.Minimized;
            tray_icon.Visible = true;
        }
        private void timer_tick(object sender, System.EventArgs e) {
            time_passed += 1;
            text.Text = string.Format("{0} seconds left", time_limit-time_passed);
            
            if (time_passed == time_limit) {
                // Disable timer
                counting = false;
                timer.Stop();
                time_passed = 0;
                //Show app
                app_show();
                //Play audio
                player.Play();
            }
        }
        private void count() {
            counting = true;
            timer.Start();
        }
        private void Button_Click(object sender, RoutedEventArgs e) {
            if (!counting)
                count();
            app_hide();
        }
        private void tray_icon_click(object sender, System.EventArgs e) {
            app_show();
        }
        private void window_Loaded(object sender, RoutedEventArgs e) {
            app_hide();
        }
        public MainWindow() {
            InitializeComponent();
            tray_icon.Icon= new Icon("Break.ico");
            tray_icon.Click += tray_icon_click;
            timer.Interval = System.TimeSpan.FromSeconds(1);
            timer.Tick += timer_tick;
            if (!counting)
                count();
        }
    }
}