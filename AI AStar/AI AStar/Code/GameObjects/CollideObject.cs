﻿using Microsoft.Xna.Framework;
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
            BoxHeight = 32;
            BoxWidth = 32;
        }

        public int BoxX => (int)X - BoxWidth / 2;
        public int BoxY => (int)Y - BoxHeight / 2;
        public int BoxWidth { get; set; }
        public int BoxHeight { get; set; }

        public Rectangle BoundingBox => new Rectangle(BoxX, BoxY, BoxWidth, BoxHeight);
        

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

        public void DestroyInstance(CollideObject gameObject, Type type)
        {
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

        public CollideObject BoxCollision(float ExtraX, float ExtraY, CollideObject obj)
        {
            if (BoundingBox.X + ExtraX + BoundingBox.Width > obj.BoundingBox.X && BoundingBox.X + ExtraX < obj.BoundingBox.X + obj.BoundingBox.Width &&
                        BoundingBox.Y + ExtraY + BoundingBox.Height > obj.BoundingBox.Y && BoundingBox.Y + ExtraY < obj.BoundingBox.Y + obj.BoundingBox.Height)
            {
                return obj;
            }
            return null;
        }
    }
}
