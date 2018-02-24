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
            BoxHeight = 32;
            BoxWidth = 32;
        }

        public int BoxX => (int)X - BoxWidth / 2;
        public int BoxY => (int)Y - BoxHeight / 2;
        public int BoxWidth { get; set; }
        public int BoxHeight { get; set; }

        public Rectangle BoundingBox => new Rectangle(BoxX, BoxY, BoxWidth, BoxHeight);
        public Vector2 BoxPosition => new Vector2(BoxX, BoxY);

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

        public bool LineToEdgeIntersection(Vector2 lineStart, Vector2 lineEnd, CollideObject obj)
        {
            //BoundingPoints Object
            Vector2 OBottomRight = obj.BoxPosition + new Vector2(obj.BoxWidth, obj.BoxHeight);
            Vector2 OBottomLeft = obj.BoxPosition + new Vector2(0, obj.BoxHeight);
            Vector2 OTopLeft = obj.BoxPosition + new Vector2(0, 0);
            Vector2 OTopRight = obj.BoxPosition + new Vector2(obj.BoxWidth, 0);

            OBottomRight = Rotate(obj.BoxX + obj.BoxWidth, obj.BoxY + obj.BoxHeight, obj.Angle, OBottomRight + new Vector2(0, 0));
            OBottomLeft = Rotate(obj.BoxX + obj.BoxWidth, obj.BoxY + obj.BoxHeight, obj.Angle, OBottomLeft + new Vector2(0,0));
            OTopLeft = Rotate(obj.BoxX + obj.BoxWidth, obj.BoxY + obj.BoxHeight, obj.Angle, OTopLeft + new Vector2(0, 0));
            OTopRight = Rotate(obj.BoxX + obj.BoxWidth, obj.BoxY + obj.BoxHeight, obj.Angle, OTopRight + new Vector2(0, 0));

            Vector2[] ORectangleEdges = new Vector2[4];
            Vector2[] LineEdges = new Vector2[2];

            ORectangleEdges[0] = OBottomRight;
            ORectangleEdges[1] = OBottomLeft;
            ORectangleEdges[2] = OTopLeft;
            ORectangleEdges[3] = OTopRight;

            LineEdges[0] = lineStart;
            LineEdges[1] = lineEnd;

            for (int i = 0; i < LineEdges.Length; i++)
            {
                for (int j = 0; j < ORectangleEdges.Length; j++)
                {
                    if (LineIntersection(LineEdges[i], LineEdges[(i + 1) % LineEdges.Length], ORectangleEdges[j], ORectangleEdges[(j + 1) % ORectangleEdges.Length]))
                        return true;
                }
            }

            return false;
        }

        public bool CheckEdges(CollideObject t, CollideObject o)
        {
            //This whole operation of checking edges is expensive and therefore we make sure
            //that the box is actually within the possible viscinity of collision
            //Which I actually don't do right now XDDDDDDXDXDXDXDXDDXXDXDDDD


            //BoundingPoints This
            Vector2 TBottomRight = t.BoxPosition + new Vector2(t.BoxWidth, t.BoxHeight);
            Vector2 TBottomLeft = t.BoxPosition + new Vector2(0, t.BoxHeight);
            Vector2 TTopLeft = t.BoxPosition + new Vector2(0, 0);
            Vector2 TTopRight = t.BoxPosition + new Vector2(t.BoxWidth, 0);

            //BoundingPoints Other
            Vector2 OBottomRight = o.BoxPosition + new Vector2(o.BoxWidth, o.BoxHeight);
            Vector2 OBottomLeft = o.BoxPosition + new Vector2(0, o.BoxHeight);
            Vector2 OTopLeft = o.BoxPosition + new Vector2(0, 0);
            Vector2 OTopRight = o.BoxPosition + new Vector2(o.BoxWidth, 0);

            //RotatePoints This
            TBottomRight = Rotate(t.BoxX + t.BoxWidth / 2, t.BoxY + t.BoxHeight / 2, t.Angle, TBottomRight + new Vector2(-0.5f, -0.5f));
            TBottomLeft = Rotate(t.BoxX + t.BoxWidth / 2, t.BoxY + t.BoxHeight / 2, t.Angle, TBottomLeft + new Vector2(0.5f, -0.5f));
            TTopLeft = Rotate(t.BoxX + t.BoxWidth / 2, t.BoxY + t.BoxHeight / 2, t.Angle, TTopLeft + new Vector2(0.5f, 0.5f));
            TTopRight = Rotate(t.BoxX + t.BoxWidth / 2, t.BoxY + t.BoxHeight / 2, t.Angle, TTopRight + new Vector2(-0.5f, 0.5f));

            //RotatePoints Other
            OBottomRight = Rotate(o.BoxX + o.BoxWidth / 2, o.BoxY + o.BoxHeight / 2, o.Angle, OBottomRight + new Vector2(-0.5f, -0.5f));
            OBottomLeft = Rotate(o.BoxX + o.BoxWidth / 2, o.BoxY + o.BoxHeight / 2, o.Angle, OBottomLeft + new Vector2(0.5f, -0.5f));
            OTopLeft = Rotate(o.BoxX + o.BoxWidth / 2, o.BoxY + o.BoxHeight / 2, o.Angle, OTopLeft + new Vector2(0.5f, 0.5f));
            OTopRight = Rotate(o.BoxX + o.BoxWidth / 2, o.BoxY + o.BoxHeight / 2, o.Angle, OTopRight + new Vector2(-0.5f, 0.5f));

            //Define all Edges from RotatePoints
            Vector2[] TRectangleEdges = new Vector2[4];
            Vector2[] ORectangleEdges = new Vector2[4];

            Vector2[] TRectangleDiagonals = new Vector2[4];
            Vector2[] ORectangleDiagonals = new Vector2[4];

            TRectangleEdges[0] = TBottomRight;
            TRectangleEdges[1] = TBottomLeft;
            TRectangleEdges[2] = TTopLeft;
            TRectangleEdges[3] = TTopRight;

            ORectangleEdges[0] = OBottomRight;
            ORectangleEdges[1] = OBottomLeft;
            ORectangleEdges[2] = OTopLeft;
            ORectangleEdges[3] = OTopRight;

            TRectangleDiagonals[0] = TTopLeft;
            TRectangleDiagonals[1] = TBottomRight;
            TRectangleDiagonals[2] = TBottomLeft;
            TRectangleDiagonals[3] = TTopRight;

            ORectangleDiagonals[0] = OTopLeft;
            ORectangleDiagonals[1] = OBottomRight;
            ORectangleDiagonals[2] = OBottomLeft;
            ORectangleDiagonals[3] = OTopRight;

            //Check Intersection for rectangle edges
            for (int i = 0; i < TRectangleEdges.Length; i++)
            {
                for (int j = 0; j < ORectangleEdges.Length; j++)
                {
                    if (LineIntersection(TRectangleEdges[i], TRectangleEdges[(i + 1) % TRectangleEdges.Length], ORectangleEdges[j], ORectangleEdges[(j + 1) % ORectangleEdges.Length]))
                        return true;
                }
            }

            //Check Intersection for internal diagonal lines
            for (int i = 0; i < TRectangleDiagonals.Length; i++)
            {
                for (int j = 0; j < ORectangleDiagonals.Length; j++)
                {
                    if (LineIntersection(TRectangleDiagonals[i], TRectangleDiagonals[(i + 1) % TRectangleDiagonals.Length], ORectangleDiagonals[j], ORectangleDiagonals[(j + 1) % ORectangleDiagonals.Length]))
                        return true;
                }
            }

            return false;
        }

        public bool LineToObjectCollision(Vector2 lineStart, Vector2 lineEnd, Type type)
        {

            if (CollisionList.TryGetValue(type, out list) && list.Count != 0)
            {
                foreach (CollideObject obj in list)
                {
                    if(LineToEdgeIntersection(lineStart,lineEnd,obj))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Vector2 Rotate(float cx, float cy, float angle, Vector2 point)
        {
            return new Vector2((float)Math.Cos(angle) * (point.X - cx) - (float)Math.Sin(angle) * (point.Y - cy) + cx,
                               (float)Math.Sin(angle) * (point.X - cx) + (float)Math.Cos(angle) * (point.Y - cy) + cy);
        }

        public bool LineIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
        {
            bool isIntersecting = false;


            float denominator = (p4.Y - p3.Y) * (p2.X - p1.X) - (p4.X - p3.X) * (p2.Y - p1.Y);

            //Make sure the denominator is > 0, if so the lines are parallel
            if (denominator != 0)
            {
                float u_a = ((p4.X - p3.X) * (p1.Y - p3.Y) - (p4.Y - p3.Y) * (p1.X - p3.X)) / denominator;
                float u_b = ((p2.X - p1.X) * (p1.Y - p3.Y) - (p2.Y - p1.Y) * (p1.X - p3.X)) / denominator;

                //Is intersecting if u_a and u_b are between 0 and 1
                if (u_a >= 0 && u_a <= 1 && u_b >= 0 && u_b <= 1)
                {
                    isIntersecting = true;
                }
            }

            return isIntersecting;
        }
    }
}
