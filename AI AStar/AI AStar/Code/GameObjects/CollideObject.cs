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

        public void AddInstance(CollideObject gameObject, Type type)
        {
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

        public static List<CollideObject> GetList(Type type)
        {
            if (CollisionList.ContainsKey(type))
            {
                return CollisionList[type];
            }
            return null;
        }

        public CollideObject BoxCollisionList(float ExtraX, float ExtraY, Type type)
        {
            if (CollisionList.TryGetValue(type, out list) && list.Count != 0)
            {
                foreach (CollideObject obj in list)
                {
                    if (obj == this)
                    {
                        continue;
                    }
                    if (BoundingBox.X + ExtraX + BoundingBox.Width > obj.BoundingBox.X && BoundingBox.X + ExtraX < obj.BoundingBox.X + obj.BoundingBox.Width &&
                        BoundingBox.Y + ExtraY + BoundingBox.Height > obj.BoundingBox.Y && BoundingBox.Y + ExtraY < obj.BoundingBox.Y + obj.BoundingBox.Height)
                    {
                        return obj;
                    }
                }
            }
            return null;
        }

    }
}
