﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGA_1_wpf.Utils
{
    class PropertyChangedHelper
    {
        public static bool ChangeValue<T>(ref T oldValue, T newValue)
        {
            if (object.Equals(oldValue, newValue))
                return false;

            oldValue = newValue;
            return true;
        }
    }
}
