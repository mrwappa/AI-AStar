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
    class CollideObject : GameObject
    {
        public static Dictionary<Type, List<CollideObject>> CollisionList = new Dictionary<Type, List<CollideObject>>();


        public CollideObject(float x, float y) : base(x, y)
        {
            BoundingBox = new Rectangle(0, 0, 0, 0);
        }

        public Rectangle BoundingBox { get; set; }

        List<CollideObject> list;

        public void AddInstance(CollideObject gameObject)
        {
            Type type = gameObject.GetType();
            if (!CollisionList.ContainsKey(type))
            {
                CollisionList.Add(type, new List<CollideObject>());
            }
            CollisionList.TryGetValue(type, out list);
            list.Add(gameObject);
        }

        public void RemoveInstance(CollideObject gameObject)
        {
            Type type = gameObject.GetType();
            CollisionList.TryGetValue(type, out list);
            list.Remove(gameObject);
        }
    }
}
