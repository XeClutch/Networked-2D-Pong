using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFPongServer {
    static class Utils {
        // Methods
        public static float RandomFloat(float min, float max) {
            return (float)new Random().NextDouble() * (max - min) + min;
        }
    }
}