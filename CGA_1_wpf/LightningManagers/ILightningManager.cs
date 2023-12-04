using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CGA_1_wpf.LightningManagers
{
    public interface ILightningManager
    {
        byte[] GetColorWithLightEffects(in Vector3 normal, in Vector3 lightDir);
    }
}
