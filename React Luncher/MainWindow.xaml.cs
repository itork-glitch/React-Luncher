using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

namespace WpfApp1
{
    public partial class ReactLuncher : Window
    {
        private Process _process;
        public bool runInBackground { get; set; }
        private NotifyIcon _notifyIcon;
        public ReactLuncher()
        {
            InitializeComponent();
            DataContext = this;

            _notifyIcon = new NotifyIcon();
            _notifyIcon.Icon = new System.Drawing.Icon("assets/react.ico");
            _notifyIcon.Visible = true;

            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
            _notifyIcon.ContextMenuStrip.Items.Add("Wyjdz").Click += (s, e) => Close();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (_process != null && !_process.HasExited)
            {
                if (runInBackground)
                {
                    System.Windows.MessageBox.Show("Serwer jest obecnie uruchomiony w tle.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    System.Windows.MessageBox.Show("Konsola serwera jest już uruchomiona.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (runInBackground)
                {
                    _notifyIcon.ShowBalloonTip(3000, "Uruchamianie w tle", "Serwer obecnie jest uruchomiony w tle.", ToolTipIcon.Info);
                }

                string targetDirectory = targetDirectoryTextBox.Text;
                string command = "npm run dev";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    WorkingDirectory = targetDirectory,
                    FileName = "cmd.exe",
                    Arguments = "/C " + command,
                    CreateNoWindow = runInBackground,
                    WindowStyle = runInBackground ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal
                };

                _process = Process.Start(startInfo);
            }
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_process != null && !_process.HasExited)
            {
                _process.Kill();
                _notifyIcon.ShowBalloonTip(3000, "Wyłączono", "Serwer został wyłączony", ToolTipIcon.Info);
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _notifyIcon.Dispose();
        }
    }
}