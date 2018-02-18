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
    class Solid : CollideObject
    {
        public Solid(float x, float y) : base(x, y)
        {
            base.AddInstance(this, typeof(Solid));
            AStarGrid.Grid[((int)X - Node.NODE_SIZE / 2) / 32][((int)Y - Node.NODE_SIZE / 2) / 32].Walkable = false;
        }

        public override void OnRemove()
        {
            AStarGrid.Grid[((int)X - Node.NODE_SIZE / 2) / 32][((int)Y - Node.NODE_SIZE / 2) / 32].Walkable = true;
            DestroyInstance(this, typeof(Solid));
            base.OnRemove();
        }
    }
}
