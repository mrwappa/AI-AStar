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
    class Room : GameObject
    {
        System.Drawing.Bitmap image;
        System.Windows.Forms.OpenFileDialog fileDialog;

        Vector2 PathPosition;

        public Room(float x, float y) : base(x, y)
        {
            fileDialog = new System.Windows.Forms.OpenFileDialog();
            fileDialog.Filter = "Data Files (*.png)|*.png";
            CreateInstances();
        }


        public void CreateGrid(int width, int height , System.Drawing.Bitmap aImage)
        {
            List<List<Node>> temp = new List<List<Node>>();
            AStarGrid = new Astar(temp);
            for (int i = 0; i < width; i++)//30
            {
                temp.Add(new List<Node>());
                for (int j = 0; j < height; j++)//17
                {
                    temp[i].Add(new Node(new Vector2(i, j), true));
                    if(image != null)
                    {
                        bool black = image.GetPixel(i, j).R == 0 && image.GetPixel(i, j).G == 0 && image.GetPixel(i, j).B == 0 && image.GetPixel(i, j).A == 255;
                        bool green = image.GetPixel(i, j).R == 0 && image.GetPixel(i, j).G == 255 && image.GetPixel(i, j).B == 0 && image.GetPixel(i, j).A == 255;
                        bool blue = image.GetPixel(i, j).R == 0 && image.GetPixel(i, j).G == 0 && image.GetPixel(i, j).B == 255 && image.GetPixel(i, j).A == 255;
                        if (black)
                        {
                            new Brick(i * Node.NODE_SIZE + 16, j * Node.NODE_SIZE + 16);
                        }
                        if (blue)
                        {
                            new PathFinder(i * Node.NODE_SIZE + 16, j * Node.NODE_SIZE + 16);
                        }
                        if (green)
                        {
                            
                            PathPosition = new Vector2(i * Node.NODE_SIZE + 16, j * Node.NODE_SIZE + 16);
                        }
                    } 
                }
            }

            

            if (PathPosition != null)
            {
                PathFinder pathFinder = GetObject(typeof(PathFinder)) as PathFinder;
                pathFinder.NewPath((int)PathPosition.X, (int)PathPosition.Y);
            }
            AStarGrid = new Astar(temp);

        }

        public void FileDialog()
        {
            DestroyAll();
            new Room(0,0);
            System.Windows.Forms.DialogResult r = fileDialog.ShowDialog();

            if(r == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.FileInfo fInfo = new System.IO.FileInfo(fileDialog.FileName);

                image = new System.Drawing.Bitmap(fInfo.ToString());
                CreateGrid(image.Width, image.Height,image);

            }
        }
        
        public void CreateInstances()
        {
            /*new PathFinder(32 + 16, 32 + 16);
            new Player(300, 300);*/
        }

        public override void Update()
        {
            if(GetKeyPressed(Keys.Tab))
            {
                FileDialog();
            }

            if(AStarGrid != null)
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
