using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_AStar.Code
{
    class G
    {
        /*public static float RadiansToDegrees(float r)
        {
            return r * (180 * (float)Math.PI);
        }
        public static float DegreesToRadians(float d)
        {
            return d / (180 / (float)Math.PI);
        }*/

        public static float LengthDirX(float len, float dir)
        {
            return (float)Math.Cos((dir)) * len;
        }

        public static float LengthDirY(float len, float dir)
        {
            return (float)Math.Sin((dir)) * len;
        }

        public static float PointDirection(float x1, float y1, float x2, float y2)
        {
            return ((float)Math.Atan2(y2 - y1, x2 - x1));
        }
    }
}
