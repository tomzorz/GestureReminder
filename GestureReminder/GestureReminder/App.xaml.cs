using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Application = System.Windows.Application;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace GestureReminder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            _n = new NotifyIcon {Icon = new Icon("Resources/icon.ico"), Visible = true};
            var r = NotifyIconHelpers.GetNotifyIconRectangle(_n, false);
            Init(r);
            _dt = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1.0)
            };
            _dt.Start();
            _dt.Tick += dt_Tick;
        }

        private NotifyIcon _n;

        void dt_Tick(object sender, EventArgs e)
        {
            var r = NotifyIconHelpers.GetNotifyIconRectangle(_n, false);
            if (_hw == null) return;
            if (_hw.Ignore) return;
            _hw.Left = r.X;
            _hw.Top = r.Y;
        }

        private HandlerWindow _hw;
        private DispatcherTimer _dt;

        private void Init(System.Drawing.Rectangle r)
        {
            _hw = new HandlerWindow(r.X, r.Y)
            {
                Width = r.Width,
                Height = r.Height,
            };
            _hw.Show();
        }

        private void App_OnExit(object sender, ExitEventArgs e)
        {
            _dt.Stop();
            _n.Visible = false;
        }
    }
}
