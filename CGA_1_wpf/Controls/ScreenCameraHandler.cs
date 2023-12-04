using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGA_1_wpf.Controls
{
    using CGA_1_wpf.Entities;
    using CGA_1_wpf.Utils;
    using System.Drawing;
    using System.Numerics;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    namespace WinForms3D
    {

        public class ScreenCameraHandler
        {
            Camera camera;
            Image control;

            Point oldMousePosition;
            Vector3 oldCameraPosition;

            Quaternion oldCameraRotation;

            float yCoeff = 10f;


            public Camera Camera
            {
                get => camera;
                set => PropertyChangedHelper.ChangeValue(ref camera, value);
            }

            public ScreenCameraHandler(Image control, Camera camera)
            {
                Control = control;
                Camera = camera;
            }



            public Image Control
            {
                get => control;
                set
                {
                    var oldControl = control;

                    if (PropertyChangedHelper.ChangeValue(ref control, value))
                    {

                        if (oldControl != null)
                        {
                            oldControl.MouseDown -= control_MouseDown;
                            oldControl.MouseMove -= control_MouseMove;
                            control.MouseUp -= Control_MouseUp;
                        }

                        if (control != null)
                        {
                            control.MouseDown += control_MouseDown;
                            control.MouseMove += control_MouseMove;
                            control.MouseUp += Control_MouseUp;
                        }
                    }
                }
            }

            private void Control_MouseUp(object sender, MouseEventArgs e)
            {
                left = false;
                right = false;
                control.Cursor = Cursors.Arrow;
            }

            bool right;
            bool left;

            private void control_MouseDown(object sender, MouseButtonEventArgs e)
            {
                MouseControls.getMouseButtons(e, out bool left, out bool right);
                oldMousePosition = e.GetPosition(sender as IInputElement);

                if (left && right)
                {
                    oldCameraPosition = camera.Position;
                    control.Cursor = Cursors.SizeNS;
                }
                else if (left)
                {
                    oldCameraRotation = camera.Rotation;
                    control.Cursor = Cursors.Hand;
                }
                else if (right)
                {
                    oldCameraPosition = camera.Position;
                    control.Cursor = Cursors.SizeAll;
                }
            }

            void control_MouseMove(object sender, MouseEventArgs e)
            {
                if (left && right)
                {
                    var deltaY = oldMousePosition.Y - e.GetPosition(sender as IInputElement).Y;
                    camera.Position = oldCameraPosition + new Vector3(0, 0, (float)deltaY / yCoeff);
                }
                else if (left)
                {
                    var oldNpc = control.NormalizePointClient(oldMousePosition);
                    var oldVector = camera.MapToSphere(oldNpc);

                    var curNpc = control.NormalizePointClient(e.GetPosition(sender as IInputElement));
                    var curVector = camera.MapToSphere(curNpc);

                    var deltaRotation = camera.CalculateQuaternion(oldVector, curVector);
                    camera.Rotation = deltaRotation * oldCameraRotation;
                }
                else if (right)
                {
                    var deltaPosition = new Vector3(e.GetPosition(sender as IInputElement).ToVector2() 
                        - oldMousePosition.ToVector2(), 0);
                    camera.Position = oldCameraPosition + (deltaPosition * new Vector3(1, -1, 1)) / 100;
                }
            }
        }
    }

}
