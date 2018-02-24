using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_AStar.Code
{
    class Sprite
    {
        public static SpriteBatch SpriteBatch;

        public Texture2D Texture { get; private set; }

        private float XScale;
        private float YScale;

        public Vector2 Bounds => new Vector2(Texture.Width * XScale, Texture.Height * YScale);

        public int AnimationIndex { get; protected set; }
        public float AnimationSpeed { get; set; }
        public int NumberOfFrames { get; set; }
        float animationCounter { get; set; }

        public bool Drawable { get { return Texture != null; } }
        
        SpriteEffects flipEffect;

        public Sprite(Texture2D texture, int numberOfFrames)
        {
            Texture = texture;
            NumberOfFrames = numberOfFrames;
        }

        public void DrawSprite(float x, float y, float xscale, float yscale, float angle, Color color, float alpha, float depth)
        {
            //used for bounds
            XScale = xscale;
            YScale = yscale;

            //Animation
            AnimationSpeed = MathHelper.Clamp(AnimationSpeed, 0, 1);
            if (AnimationSpeed > 0)
            {
                animationCounter += AnimationSpeed;
                if (animationCounter >= 1)
                {
                    AnimationIndex++;
                    animationCounter--;
                    if (AnimationIndex >= NumberOfFrames)
                    {
                        AnimationIndex = 0;
                    }
                }
            }

            flipEffect = xscale < 0 && yscale < 0 ? SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally : SpriteEffects.None;
            flipEffect = yscale < 0 && xscale > 0 ? SpriteEffects.FlipVertically : SpriteEffects.None;
            flipEffect = yscale > 0 && xscale < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            //Draw Sprite
            SpriteBatch.Draw(Texture, new Vector2(x, y), new Rectangle(AnimationIndex * Texture.Width, 0, Texture.Width, Texture.Height), color * alpha, angle,
               new Vector2((Texture.Width / 2), (Texture.Height / 2)), new Vector2(Math.Sign(xscale), Math.Sign(yscale)), flipEffect, depth);
        }
    }
}
