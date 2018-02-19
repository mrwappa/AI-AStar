using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_AStar.Code.GameObjects
{
    class Brick : Solid
    {
        public Brick(float x, float y) : base(x, y)
        {
            Sprite = new Sprite(Box, 1);
        }
    }
}
