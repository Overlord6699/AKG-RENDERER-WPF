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

    }
}
