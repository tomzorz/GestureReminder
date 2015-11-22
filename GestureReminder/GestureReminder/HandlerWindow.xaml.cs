using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace GestureReminder
{
    /// <summary>
    /// Interaction logic for HandlerWindow.xaml
    /// </summary>
    public partial class HandlerWindow : Window
    {
        private readonly int _p1;
        private readonly int _p2;

        public HandlerWindow(int p1, int p2)
        {
            _p1 = p1;
            _p2 = p2;
            InitializeComponent();
            Topmost = true;
        }

        private void HandlerWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            Left = _p1;
            Top = _p2;
        }

        private double _enterX;
        private double _enterY;
        private double _currentX;
        private double _currentY;

        private void HandlerWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!_track) return;
            _currentX = e.GetPosition(this).X;
            _currentY = e.GetPosition(this).Y;
            Line.X1 = _enterX;
            Line.Y1 = _enterY;
            Line.X2 = _currentX;
            Line.Y2 = _currentY;
            Canvas.SetLeft(StatusGrid, _currentX - 100.0);
            Canvas.SetTop(StatusGrid, _currentY - 100.0);
            var d = Math.Floor(Math.Sqrt(Math.Pow(Line.X1 - Line.X2, 2) + Math.Pow(Line.Y1 - Line.Y2, 2)) / 4.0);
            _t = TimeSpan.FromMinutes(d);
            TextBlockTop.Text = string.Format("{0} hours {1} minutes", _t.Hours, _t.Minutes);
            TextBlockBottom.Text = "at " + DateTime.Now.Add(_t).ToString("HH:mm:ss");
        }

        private TimeSpan _t;

        private bool _track;

        public bool Ignore { get; private set; }

        private void HandlerWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Ignore = true;
            _track = true;
            WindowState = WindowState.Maximized;
            StatusGrid.Visibility = Visibility.Visible;
            StatusGrid.IsHitTestVisible = true;
            _enterX = e.GetPosition(this).X + Left;
            _enterY = e.GetPosition(this).Y + Top;
        }

        private void HandlerWindow_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _track = false;
            VisualStateManager.GoToElementState(LayoutRoot, "Edit", true);
            TextBox.Focus();
        }

        private void HandlerWindow_OnDeactivated(object sender, EventArgs e)
        {
            Topmost = true;
            Activate();
        }

        private void TextBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key != Key.Enter) return;
            Debug.WriteLine(TextBox.Text.Trim());
            Debug.WriteLine("{0} hours {1} minutes", _t.Hours, _t.Minutes);
            Debug.WriteLine("//todo save and remind... :)");
            VisualStateManager.GoToElementState(LayoutRoot, "Normal", true);
            StatusGrid.Visibility = Visibility.Collapsed;
            StatusGrid.IsHitTestVisible = false;
            Line.X1 = 0.0;
            Line.X2 = 0.0;
            Line.Y1 = 0.0;
            Line.Y2 = 0.0;
            Ignore = false;
            WindowState = WindowState.Normal;
            Topmost = true;
        }
    }
}
