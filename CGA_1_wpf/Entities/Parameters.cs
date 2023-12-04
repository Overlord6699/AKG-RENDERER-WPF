using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;

namespace CGA_1_wpf.Entities
{
    public class Parameters : ICloneable
    {
        public float Scaling { get; set; }  // Масштаб модели.
        public float ModelYaw { get; set; } // Поворот модели вокруг оси Y.
        public float ModelPitch { get; set; } // Поворот модели вокруг оси X.
        public float ModelRoll { get; set; } // Поворот модели вокруг оси Z.
        public float TranslationX { get; set; } // Смещение модели по оси X.
        public float TranslationY { get; set; } // Смещение модели по оси Y.
        public float TranslationZ { get; set; } // Смещение модели по оси Z.

        public Camera Camera { get; set; }

    /*    public float CameraPitch { get; set; } 
        public float CameraYaw { get; set; }
        public float CameraRoll { get; set; }*/
        public float FieldOfView { get; set; }  // Угол обзора камеры.
        public float AspectRatio { get; set; }  // Соотношение сторон окна отображения.
        public float NearPlaneDistance { get; set; }    // Расстояние до ближней плоскости отсечения.
        public float FarPlaneDistance { get; set; } // Расстояние до дальней плоскости отсечения.
        public int XMin { get; set; }   // Координаты X левой границы окна отображения.
        public int YMin { get; set; }   // Координаты Y нижней границы окна отображения.
        public int Width { get; set; }  // Ширина окна отображения.
        public int Height { get; set; } // Высота окна отображения.
        public bool ShowXZGrid { get; set; } //Отрисовка сетки

        public Parameters(double width, double height)
        {
            Scaling = 1;
            ModelYaw = 0;
            ModelPitch = 0;
            ModelRoll = 0;
            TranslationX = 0 + (10f);
            TranslationY = 0 - (15f);
            TranslationZ = 0 - (35f);
            Camera = new Camera(new Vector3(0,0,3), Quaternion.Identity);
            FieldOfView = 1;
            Height = (int)height;
            Width = (int)width;
            AspectRatio = (float)(width / height);
            NearPlaneDistance = 0.1f;
            FarPlaneDistance = 1000f;
            XMin = 0;
            YMin = 0;
        }

        public Parameters(float scaling, float modelYaw, float modelPitch, float modelRoll, float translationX,
                float translationY, float translationZ, float cameraPositionX, float cameraPositionY, float cameraPositionZ,
                float cameraYaw, float cameraPitch, float cameraRoll, float fieldOfView, float aspectRatio, float nearPlaneDistance,
                float farPlaneDistance, int xMin, int yMin, int width, int height)
        {
            Scaling = scaling;
            ModelYaw = modelYaw;
            ModelPitch = modelPitch;
            ModelRoll = modelRoll;
            TranslationX = translationX;
            TranslationY = translationY;
            TranslationZ = translationZ;

            Camera = new Camera(
                new Vector3(cameraPositionX, cameraPositionY, cameraPositionZ),
                Quaternion.CreateFromYawPitchRoll(cameraYaw, cameraPitch, cameraRoll)
            );

            FieldOfView = fieldOfView;
            AspectRatio = aspectRatio;
            NearPlaneDistance = nearPlaneDistance;
            FarPlaneDistance = farPlaneDistance;
            XMin = xMin;
            YMin = yMin;
            Height = height;
            Width = width;
        }

        public object Clone()
        {
            return new Parameters(Scaling, ModelYaw, ModelPitch, ModelRoll, TranslationX, TranslationY, TranslationZ,
                Camera.Position.X, Camera.Position.Y, Camera.Position.Z, Camera.Rotation.Y, Camera.Rotation.X, Camera.Rotation.Z, FieldOfView, AspectRatio, NearPlaneDistance,
                FarPlaneDistance, XMin, YMin, Width, Height);
        }
    }
}

