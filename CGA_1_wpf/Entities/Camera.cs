using CGA_1_wpf.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CGA_1_wpf.Entities
{
    public class Camera
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public float Radius = .3f;

        float radius_squared { get => Radius * Radius; }


        public Camera(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;    
        }


        public Camera(Vector3 position, float cameraPitch, float cameraYaw, float cameraRoll)
        {
            Position = position;
            Rotation = Quaternion.CreateFromYawPitchRoll(cameraYaw, cameraPitch, cameraRoll);
        }



        public Vector3 MapToSphere(Vector2 v)
        {

            var P = new Vector3(v.X, -v.Y, 0);

            var XY_squared = P.LengthSquared();

            if (XY_squared <= .5f * radius_squared)
                P.Z = (float)Math.Sqrt(radius_squared - XY_squared);  // Pythagore
            else
                P.Z = 0.5f * radius_squared / (float)Math.Sqrt(XY_squared);  // Hyperboloid

            return Vector3.Normalize(P);
        }

        public Quaternion CalculateQuaternion(Vector3 startV, Vector3 currentV)
        {

            var cross = Vector3.Cross(startV, currentV);

            if (cross.Length() > MathUtils.Epsilon)
                return new Quaternion(cross, Vector3.Dot(startV, currentV));
            else
                return Quaternion.Identity;
        }
    }
}
