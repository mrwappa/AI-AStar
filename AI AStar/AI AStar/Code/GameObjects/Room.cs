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
    class Room : GameObject
    {
        public Room(float x, float y) : base(x, y)
        {
            CreateInstances();
        }

        public void CreateInstances()
        {
            new PathFinder(32 + 16, 32 + 16);
            new Player(300, 300);
        }

        public override void Update()
        {
            if (Mouse.LeftButton == ButtonState.Pressed && Keyboard.IsKeyUp(Keys.LeftShift) && Keyboard.IsKeyUp(Keys.Space))
            {
                if (GridSnapMouse.X < 960 && GridSnapMouse.Y < 540 && GridSnapMouse.X > 0 && GridSnapMouse.Y > 0)
                {
                    List<CollideObject> solids = CollideObject.GetList(typeof(Solid));
                    if (solids != null)
                    {
                        bool canCreate = true;
                        foreach (Solid obj in solids)
                        {
                            if (obj.X == GridSnapMouse.X && obj.Y == GridSnapMouse.Y)
                            {
                                canCreate = false;
                            }
                        }
                        if (canCreate)
                        {
                            new Brick(GridSnapMouse.X, GridSnapMouse.Y);
                        }
                    }
                    else
                    {
                        new Brick(GridSnapMouse.X, GridSnapMouse.Y);
                    }
                }

            }

            if (Mouse.RightButton == ButtonState.Pressed)
            {
                List<CollideObject> solids = CollideObject.GetList(typeof(Solid));
                foreach (Solid obj in solids.ToList())
                {
                    if (obj.X == GridSnapMouse.X && obj.Y == GridSnapMouse.Y)
                    {
                        DestroyInstance(obj);
                    }
                }
            }

            if(GetKeyPressed(Keys.R))
            {
                RestartRoom();
            }

            base.Update();
        }

        public void RestartRoom()
        {
            DestroyAll();
            new Room(0, 0);
            CreateInstances();
        }

        public override void Draw()
        {
            //draw box on a position that is snapped to the grid
            SpriteBatch.Draw(Box, GridSnapMouse, new Rectangle(0, 0, Box.Width, Box.Height), Color.Black, 0,
            new Vector2((Box.Width / 2), (Box.Height / 2)), new Vector2(1, 1), SpriteEffects.None, 1);
        }
        public override void DrawGUI()
        {
            List<CollideObject> tempList = CollideObject.GetList(typeof(Solid));
            if(tempList != null)
            {
                SpriteBatch.DrawString(Font, tempList.Count.ToString(), new Vector2(20, 80), Color.Black);
            }
            
        }
    }

    
}
