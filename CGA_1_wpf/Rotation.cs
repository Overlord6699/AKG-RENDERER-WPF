using CGA_1_wpf.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace CGA_1_wpf
{
    public interface IRotation
    {
        void Process(Parameters parameters, KeyEventArgs e);
    }

    public class MoveModel : IRotation
    {


        public void Process(Parameters modelParams, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.E:
                    modelParams.TranslationZ += .3f;
                    break;
                case Key.Q:
                    modelParams.TranslationZ -= .3f;
                    break;
                case Key.A:
                    modelParams.TranslationX -= .3f;
                    break;
                case Key.D:
                    modelParams.TranslationX += .3f;
                    break;
                case Key.W:
                    modelParams.TranslationY += .3f;
                    break;
                case Key.S:
                    modelParams.TranslationY -= .3f;
                    break;
            }
        }
    }

    public class RotateModel : IRotation
    {
        public void Process(Parameters modelParams, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    modelParams.ModelPitch -= (float)Math.PI / 12;
                    break;
                case Key.S:
                    modelParams.ModelPitch += (float)Math.PI / 12;
                    break;
                case Key.Q:
                    modelParams.ModelRoll += (float)Math.PI / 12;
                    break;
                case Key.E:
                    modelParams.ModelRoll -= (float)Math.PI / 12;
                    break;
                case Key.A:
                    modelParams.ModelYaw -= (float)Math.PI / 12;
                    break;
                case Key.D:
                    modelParams.ModelYaw += (float)Math.PI / 12;
                    break;
            }
        }
    }

    public class MoveCamera : IRotation
    {
        public void Process(Parameters modelParams, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Q:
                    modelParams.Camera.Position += new Vector3(0, 0, .3f);
                    break;
                case Key.E:
                    modelParams.Camera.Position -= new Vector3(0,0,.3f);
                    break;
                case Key.D:
                    modelParams.Camera.Position -= new Vector3(.3f, 0, 0);
                    break;
                case Key.A:
                    modelParams.Camera.Position += new Vector3(.3f, 0, 0);
                    break;
                case Key.S:
                    modelParams.Camera.Position += new Vector3(0, .3f, 0);
                    break;
                case Key.W:
                    modelParams.Camera.Position -= new Vector3(0, .3f, 0); ;
                    break;
            }
        }
    }

    public class RotateCamera : IRotation
    {
        const float STEP = (float)(Math.PI / 12);

        Quaternion ROTATION_STEP_X = Quaternion.CreateFromYawPitchRoll(STEP, 0, 0);

        Quaternion ROTATION_STEP_Y = Quaternion.CreateFromYawPitchRoll(0, STEP, 0);

        Quaternion ROTATION_STEP_Z = Quaternion.CreateFromYawPitchRoll(0, 0, STEP);


        public void Process(Parameters modelParams, KeyEventArgs e)
        {
            float accumulatedPitch = 0;
            float accumulatedYaw = 0;
            float accumulatedRoll = 0;

            switch (e.Key)
            {
                case Key.E:
                    accumulatedRoll += STEP;
                    break;
                case Key.Q:
                    accumulatedRoll -= STEP;
                    break;
                case Key.D:
                    accumulatedYaw += STEP;
                    break;
                case Key.A:
                    accumulatedYaw -= STEP;
                    break;
                case Key.W:
                    accumulatedPitch += STEP;
                    break;
                case Key.S:
                    accumulatedPitch -= STEP;
                    break;
            }

            Quaternion rotationQuaternion = Quaternion.CreateFromYawPitchRoll(accumulatedYaw, accumulatedPitch, accumulatedRoll);
            modelParams.Camera.Rotation *= rotationQuaternion;
        }
    }
}
