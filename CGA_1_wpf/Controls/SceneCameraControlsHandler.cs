using CGA_1_wpf.Entities;
using CGA_1_wpf.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CGA_1_wpf.Controls
{
    public class SceneCameraControlsHandler
    {
        const float WHEEL_MULTIPLIER = 1f, MOVE_SPEED = 1f;
        double ROTATE_SPEED = 0.01f;

        Control control;
        Camera camera;

        bool leftB, rightB;
        bool up; bool down; bool left; bool right;

        Point mouse;

        public Camera Camera
        {
            get => camera;
            set => PropertyChangedHelper.ChangeValue(ref camera, value);
        }

        public SceneCameraControlsHandler(Control control, Camera camera)
        {
            Control = control;
            Camera = camera;
        }

        public Control Control
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
                        oldControl.MouseUp -= Control_MouseUp;
                        control.MouseWheel -= Control_MouseWheel;
                        oldControl.MouseEnter -= Control_MouseEnter;
                        oldControl.MouseLeave -= Control_MouseLeave;
                    }

                    if (control != null)
                    {
                        control.MouseDown += control_MouseDown;
                        control.MouseMove += control_MouseMove;
                        control.MouseUp += Control_MouseUp;
                        control.MouseWheel += Control_MouseWheel;
                        control.MouseEnter += Control_MouseEnter;
                        control.MouseLeave += Control_MouseLeave;
                    }
                }
            }
        }

        private void Control_MouseWheel(object sender, MouseWheelEventArgs e)
        {

            if (e.Delta > 0)
                move(0, 0, WHEEL_MULTIPLIER);
            else
                move(0, 0, -WHEEL_MULTIPLIER);
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            var frm = this.control;
 /*           frm.KeyDown -= Frm_KeyDown;
            frm.KeyUp -= Frm_KeyUp;*/
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            var frm = this.control;
            //frm.KeyDown += Frm_KeyDown;
           // frm.KeyUp += Frm_KeyUp;
        }


/*
        void Frm_KeyUp(object sender, KeyEventArgs e)
        {
            handleKeyCode(e, false);
            handleMove();
        }

        void Frm_KeyDown(object sender, KeyEventArgs e)
        {
            handleKeyCode(e, true);
            handleMove();
        }*/

        void handleMove()
        {
            if (up)
                move(0, 0, 1);
            else if (down)
                move(0, 0, -1);

            if (left)
                move(1, 0, 0);
            else if (right)
                move(-1, 0, 0);
        }

        void move(float dx, float dy, float dz)
        {
            camera.Position += Vector3.Transform(new Vector3(dx, dy, dz) * MOVE_SPEED, camera.Rotation);
        }

        void rotate(float p, float y, float r)
        {
            camera.Rotation = Quaternion.CreateFromYawPitchRoll(y, p, r) * camera.Rotation;
        }


        private void Control_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MouseControls.getMouseButtons(e, out leftB, out rightB);
        }

        private void control_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseControls.getMouseButtons(e, out leftB, out rightB);
            mouse = e.GetPosition(sender as IInputElement);
        }

        void control_MouseMove(object sender, MouseEventArgs e)
        {
            if (rightB)
            {
                var delta = Point.Subtract(mouse, e.GetPosition(sender as IInputElement));
                move((float) (-delta.X * ROTATE_SPEED), (float) (delta.Y * ROTATE_SPEED), 0);
                mouse = e.GetPosition(sender as IInputElement);
            }
            if (leftB)
            {

                var delta = Point.Subtract(mouse, e.GetPosition(sender as IInputElement));
                rotate((float) (-delta.Y * ROTATE_SPEED), (float) (delta.X * ROTATE_SPEED), 0);
                mouse = e.GetPosition(sender as IInputElement);
            }
        }

    }
}
