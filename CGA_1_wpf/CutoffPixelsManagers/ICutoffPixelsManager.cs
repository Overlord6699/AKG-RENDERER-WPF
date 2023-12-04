using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CGA_1_wpf.CutoffPixelsManagers
{
    public interface ICutoffPixelsManager
    {
        bool IsTriangleVisible(in Vector3 triangleNormal, in Vector3 camera);
    }
}
