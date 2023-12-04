using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CGA_1_wpf.CutoffPixelsManagers
{
    public class SimpleCutoffManager: ICutoffPixelsManager
    {
        private float _gap = -0.3f;

        public SimpleCutoffManager(in float gap) { 
            _gap = gap;
        }

        /*
    2) Реализовать отбраковку невидимых и задних поверхностей трехмерных объектов
    (отбраковка граней, чья нормаль не направлена в сторону камеры)
    */
        public bool IsTriangleVisible(in Vector3 triangleNormal, in Vector3 camera)
        {
            return Vector3.Dot(triangleNormal, camera) > _gap;
        }
    }
}
