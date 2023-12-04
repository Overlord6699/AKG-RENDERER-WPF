using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;

namespace CGA_1_wpf.Controls
{
    static class MouseControls
    {

        public static Vector2 NormalizePointClient(this Image control, Point position) =>
            new Vector2((float)(position.X * (2f / control.Width) - 1.0f), (float) (position.Y * (2f / control.Height) - 1.0f));

        public static void getMouseButtons(MouseButtonEventArgs e, out bool left, out bool right)
        {
            left = e.LeftButton == MouseButtonState.Pressed;
            right = e.RightButton == MouseButtonState.Pressed;
        }
    }
}
