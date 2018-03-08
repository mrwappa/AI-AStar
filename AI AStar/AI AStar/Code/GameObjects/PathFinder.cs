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
    class PathFinder : CollideObject
    {
        Stack<Node> Path;
        bool walkToLastNode;

        public float MovementSpeed { get; set; }
        public float XSpeed { get; set; }
        public float YSpeed { get; set; }
        public float XTarget { get; set; }
        public float YTarget { get; set; }
        public float Direction { get; set; }
        public float PathCounter { get; set; }

        GameObject Target;
        CollideObject CollideBlock;
        Color LineColor;

        
        float switchStateCounter;

        enum FinderState { Idle, Follow }

        public int CurrentState { get; set; }

        float xPrevious;
        float yPrevious;
        float xCurrent;
        float yCurrent;

        public PathFinder(float x, float y) : base(x, y)
        {
            Sprite = new Sprite(Box, 1);
            Color = Color.Red;
            MovementSpeed = 5f;
            CurrentState = (int)FinderState.Idle;
            LineColor = Color.Black;
        }


        

        public void NewPath(int targetX, int targetY)
        {
            Path = AStarGrid.FindPath(SnapToGrid(X, Y), SnapToGrid(targetX, targetY));
        }

        public override void Update()
        {
            //Target = GetObject(typeof(Player));
            Camera.Position = new Vector2(X, Y);
            if (Keyboard.IsKeyDown(Keys.Space) && GetMousePressed(Mouse.LeftButton))
            {
                if (GridSnapMouse != SnapToGrid(X, Y))
                {
                    //Path = AStarGrid.FindPath(SnapToGrid(X, Y), GridSnapMouse);
                    NewPath((int)GridSnapMouse.X, (int)GridSnapMouse.Y);
                }
            }
            /*switchStateCounter -= 1f / 60f;
            if (CurrentState == (int)FinderState.Idle)
            {
                Path = null;
                if (!LineToObjectCollision(new Vector2(X, Y), new Vector2(Target.X, Target.Y), typeof(Solid)))
                {
                    CurrentState = (int)FinderState.Follow;
                }
            }
            if (CurrentState == (int)FinderState.Follow)
            {
                pathCounter -= 1f / 60f;

                if (LineToObjectCollision(new Vector2(X, Y), new Vector2(Target.X, Target.Y), typeof(Solid)) && Path == null && pathCounter <= 0)
                {
                    Path = AStarGrid.FindPath(SnapToGrid(X, Y), SnapToGrid(Target.X,Target.Y));
                    pathCounter = 0.5f;
                }
                if(!LineToObjectCollision(new Vector2(X, Y), new Vector2(Target.X, Target.Y), typeof(Solid)) && pathCounter <= 0 && switchStateCounter <= 0)
                {
                    Path = null;
                    switchStateCounter = 0.4f;
                }
                if (Path == null)
                {
                    Direction = G.PointDirection(X, Y, Target.X, Target.Y);
                    XSpeed = G.LengthDirX(MovementSpeed, Direction);
                    YSpeed = G.LengthDirY(MovementSpeed, Direction);

                    CollideBlock = BoxCollisionList(XSpeed, 0, typeof(Solid));
                    if (CollideBlock != null)
                    {
                        //stop moving and get as close as possible
                        XSpeed = 0;
                        X = (float)Math.Round(X);
                        for (int i = 0; i < Math.Abs(XSpeed); i++)
                        {
                            if (BoxCollision(Math.Sign(XSpeed), 0, CollideBlock) != null)
                            {
                                break;
                            }
                            X += Math.Sign(XSpeed);
                        }

                        //prevents getting stuck when moving diagonally
                        if (X > CollideBlock.X && BoxCollision(0, 0, CollideBlock) != null)
                        {
                            float p_difference = Math.Abs((X - Sprite.Texture.Width / 2) - (CollideBlock.X + CollideBlock.Sprite.Texture.Width / 2));
                            if (p_difference > 0)
                            {
                                X += Math.Sign(p_difference);
                            }
                        }
                        else if (X <= CollideBlock.X && BoxCollision(0, 0, CollideBlock) != null)
                        {
                            float p_difference = Math.Abs((X + Sprite.Texture.Width / 2) - (CollideBlock.X - CollideBlock.Sprite.Texture.Width / 2));
                            if (p_difference > 0)
                            {
                                X -= Math.Sign(p_difference);
                            }

                        }
                        Path = AStarGrid.FindPath(SnapToGrid(X, Y), SnapToGrid(Target.X, Target.Y));
                    }

                    CollideBlock = BoxCollisionList(0, YSpeed, typeof(Solid));
                    if (CollideBlock != null)
                    {
                        //stop moving and get as close as possible
                        YSpeed = 0;
                        Y = (float)Math.Round(Y);
                        for (int i = 0; i < Math.Abs(YSpeed); i++)
                        {
                            if (BoxCollision(0, Math.Sign(YSpeed), CollideBlock) != null)
                            {
                                break;
                            }
                            Y += Math.Sign(YSpeed);
                        }
                        Path = AStarGrid.FindPath(SnapToGrid(X, Y), SnapToGrid(Target.X, Target.Y));
                    }

                    X += XSpeed;
                    Y += YSpeed;
                }
                else
                {

                }
            }*/
            
            if (Keyboard.IsKeyDown(Keys.LeftShift) && GetMousePressed(Mouse.LeftButton))
            {
                bool canMove = true;
                List<CollideObject> solids = GetList(typeof(Solid));
                if(solids != null)
                {
                    foreach (Solid obj in solids.ToList())
                    {
                        if (obj.X == GridSnapMouse.X && obj.Y == GridSnapMouse.Y)
                        {
                            canMove = false;
                        }
                    }
                    if (canMove)
                    {
                        X = GridSnapMouse.X;
                        Y = GridSnapMouse.Y;
                    }
                }
            }

            if (Path != null && Path.Count > 0)
            {
                XTarget = Path.Peek().Parent.Center.X;
                YTarget = Path.Peek().Parent.Center.Y;
                if (Path.Count == 1)
                {
                    if (X == Path.Peek().Parent.Center.X && Y == Path.Peek().Parent.Center.Y)
                    {
                        walkToLastNode = true;
                    }
                    if (walkToLastNode)
                    {
                        XTarget = Path.Peek().Center.X;
                        YTarget = Path.Peek().Center.Y;
                    }
                    if (X == Path.Peek().Center.X && Y == Path.Peek().Center.Y)
                    {
                        walkToLastNode = false;
                        Path.Pop();
                    }
                }
                Direction = G.PointDirection(X, Y, XTarget, YTarget);
                XSpeed = G.LengthDirX(MovementSpeed, Direction);
                YSpeed = G.LengthDirY(MovementSpeed, Direction);

                X += Math.Abs(X - XTarget) < Math.Abs(XSpeed) ? 0 : XSpeed;
                Y += Math.Abs(Y - YTarget) < Math.Abs(YSpeed) ? 0 : YSpeed;
                
                xPrevious = setValue(Convert.ToSingle(X));
                yPrevious = setValue(Convert.ToSingle(Y));

                if (Math.Abs(X - XTarget) < Math.Abs(XSpeed))
                {
                    X = XTarget;
                }
                if (Math.Abs(Y - YTarget) < Math.Abs(YSpeed))
                {
                    Y = YTarget;
                }
                
                if (X == XTarget && Y == YTarget)
                {
                    xCurrent = setValue(Convert.ToSingle(X));
                    yCurrent = setValue(Convert.ToSingle(Y));
                    if (Path.Count != 1 && Path.Count != 0)
                    {
                        Path.Pop();

                        X += xPrevious - xCurrent;
                        Y += yPrevious - yCurrent;
                        
                        
                    }

                }
                base.Update();
            }
        }

        //dö
        float setValue(float x)
        {
            return x;
        }

        public override void Draw()
        {
            if (Path != null && Path.Count > 0)
            {
                //DrawLine(Path.Peek().Parent.Center, Path.Peek().Parent.Center + new Vector2(1, 1), Color.Black,1, spriteBatch);
                foreach (Node obj in Path)
                {
                    DrawLine(obj.Center, obj.Parent.Center, Color.White, 0);
                }

            }
            
            if(Target != null)
            {
                Color = Color.Black;
                if (LineToObjectCollision(new Vector2(X,Y), new Vector2(Target.X, Target.Y),typeof(Solid)))
                {
                    Color = Color.Purple;
                }
                DrawLine(new Vector2(X, Y), new Vector2(Target.X, Target.Y), LineColor, 0);

            }

            base.Draw();
        }
    }
}
