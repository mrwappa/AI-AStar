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
    class Player : CollideObject
    {
        bool W;
        bool A;
        bool S;
        bool D;

        float movementSpeed;
        float axisXAcceleration;
        float axisYAcceleration;
        float axisXRestitution;
        float axisYRestitution;

        float xSpeed;
        float ySpeed;
        float axisXMax;
        float axisYMax;
        int axisXDir;
        int axisYDir;
        float axisXAdd;
        float axisYAdd;
        float axisXSub;
        float axisYSub;

        CollideObject CollideBlock;
        CollideObject CollideInstance;

        public Player(float x, float y) : base(x, y)
        {
            Sprite = new Sprite(Box, 1);
            Color = Color.Green;

            movementSpeed = 5;

            axisXMax = movementSpeed;
            axisYMax = movementSpeed;

            axisXAcceleration = 1f;
            axisYAcceleration = 1f;

            axisXRestitution = 1f;
            axisYRestitution = 1f;
        }

        public override void Update()
        {
            //check keys
            W = Keyboard.IsKeyDown(Keys.W);
            A = Keyboard.IsKeyDown(Keys.A);
            S = Keyboard.IsKeyDown(Keys.S);
            D = Keyboard.IsKeyDown(Keys.D);

            //axes
            axisXDir = Convert.ToInt32(D) - Convert.ToInt32(A);
            axisYDir = Convert.ToInt32(S) - Convert.ToInt32(W);

            //acceleration
            axisXAdd = axisXDir * axisXAcceleration;
            axisYAdd = axisYDir * axisYAcceleration;

            //restitution
            axisXSub = MathHelper.Min(axisXRestitution, Math.Abs(xSpeed)) * Math.Sign(xSpeed) * Convert.ToInt32(axisXDir == 0);
            axisYSub = MathHelper.Min(axisYRestitution, Math.Abs(ySpeed)) * Math.Sign(ySpeed) * Convert.ToInt32(axisYDir == 0);

            xSpeed = MathHelper.Clamp(xSpeed + axisXAdd - axisXSub, -axisXMax, axisXMax);
            ySpeed = MathHelper.Clamp(ySpeed + axisYAdd - axisYSub, -axisYMax, axisYMax);

            //adjust axes
            if (xSpeed != 0 && ySpeed != 0)
            {
                var dist = Math.Sqrt((xSpeed * xSpeed) + (ySpeed * ySpeed));
                var mdist = MathHelper.Min(movementSpeed +1, (float)dist);
                xSpeed = (xSpeed / (float)dist) * (mdist);
                ySpeed = (ySpeed / (float)dist) * (mdist);
            }

            CollideBlock = BoxCollisionList(xSpeed, 0, typeof(Solid));
            if (CollideBlock != null)
            {
                //stop moving and get as close as possible
                xSpeed = 0;
                X = (float)Math.Round(X);
                for (int i = 0; i < Math.Abs(xSpeed); i++)
                {
                    if (BoxCollision(Math.Sign(xSpeed), 0, CollideBlock) != null)
                    {
                        break;
                    }
                    X += Math.Sign(xSpeed);
                }
                
                //prevents getting stuck when moving diagonally
                if(X > CollideBlock.X && BoxCollision(0,0,CollideBlock) != null)
                {
                    float p_difference = Math.Abs((X - Sprite.Texture.Width / 2) - (CollideBlock.X + CollideBlock.Sprite.Texture.Width / 2));
                    if(p_difference > 0)
                    {
                        X += Math.Sign(p_difference);
                    }
                }
                else if(X <= CollideBlock.X && BoxCollision(0, 0, CollideBlock) != null)
                {
                    float p_difference = Math.Abs((X + Sprite.Texture.Width / 2) - (CollideBlock.X - CollideBlock.Sprite.Texture.Width / 2));
                    if(p_difference > 0)
                    {
                        X -= Math.Sign(p_difference);
                    }

                }
                
            }

            CollideBlock = BoxCollisionList(0, ySpeed, typeof(Solid));
            if(CollideBlock != null)
            {
                //stop moving and get as close as possible
                ySpeed = 0;
                Y = (float)Math.Round(Y);
                for (int i = 0; i < Math.Abs(ySpeed); i++)
                {
                    if (BoxCollision(0, Math.Sign(ySpeed), CollideBlock) != null)
                    {
                        break;
                    }
                    Y += Math.Sign(ySpeed);
                }
            }
      
            X += xSpeed;
            Y += ySpeed;

            X = MathHelper.Clamp(X, 0 + Sprite.Bounds.X / 2, Camera.Width - Sprite.Bounds.X / 2);
            Y = MathHelper.Clamp(Y, 0 + Sprite.Bounds.Y / 2, Camera.Height - Sprite.Bounds.Y / 2);

            base.Update();
        }
    }
}
