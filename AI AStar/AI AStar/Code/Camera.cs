using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using AI_AStar.Code;
using AI_AStar.Code.GameObjects;


namespace AI_AStar.Code
{
    //this class is so ugly, please dont look if not absolutely necessary
    class Camera
    {
        public Matrix Transform { get; set; }
        float widthScale;
        float heightScale;
        public float Width { get; private set; }
        public float Height { get; private set; }

        public static MouseState Mouse { get; set; }
        public float MouseX { get; set; }
        public float MouseY { get; set; }

        public int MonitorWidth { get; set; }
        public int MonitorHeight { get; set; }

        float ideal_width;
        float corresponding_width;
        float ideal_height;
        float aspect_ratio;
        float width_difference;


        Random random;

        public Camera(int monitorWidth, int monitorHeight)
        {
            MonitorHeight = monitorHeight;
            MonitorWidth = monitorWidth;
            random = new Random();

            //set a wanted height and hardcode that numbers corresponding 16:9 width
            ideal_height = 540;//540 , 360
            corresponding_width = 960;//960, 640

            //get the aspect ratio and multiply it by our ideal_height to set our ideal_width
            aspect_ratio = (float)monitorWidth / (float)monitorHeight;//yes, the (float) part matters, otherwise it will floor itself as an inte
            ideal_width = (float)Math.Round(ideal_height * aspect_ratio);

            //actually forgot what this is(something about pixel perfect scaling?)
            if (monitorWidth % ideal_width != 0)
            {
                float d = (float)Math.Round(monitorWidth / ideal_width);
                ideal_width = monitorWidth / d;
            }
            if (monitorHeight % ideal_height != 0)
            {
                float d = (float)Math.Round(monitorHeight / ideal_height);
                ideal_height = monitorHeight / d;
            }



            //get the difference of our corresponding_width width and ideal_width to make sure that the game resolution is as close to 960x540(ideal_height x corresponding_width)
            //but still in the same aspect ratio as the current monitor
            width_difference = ideal_width / corresponding_width;

            //set our game width and height with our width difference
            Width = (float)Math.Round((ideal_width / width_difference));
            Height = (float)Math.Round((ideal_height / width_difference));

            //check for odd numbers
            if (Width % 2 != 0)
            {
                Width++;
            }
            if (Height % 2 != 0)
            {
                Height++;
            }

            //A problem occurs here when we have a resolution like 1280x768, where width- and heightScale has a decimal number in it like .33.
            //Rhis leads to sprite distortion. The easiest solution is to simply round or floor the two variables. But then the view gets
            //bigger than intended. So what one needs to do is find a way to have decimals in these two variables without having distortion as a result.
            //It's either that or the fact that testing other resolutions on a monitor is faulty
            widthScale = monitorWidth / Width;
            heightScale = monitorHeight / Height;

            ScreenShake = 0;
        }

        public void ReInit()
        {
            Rotation = 0;
            Zoom = 1f;
        }

        public Vector2 Center { get; private set; }
        public Vector2 Position { get; set; }

        private float zoom = 1f;
        private float rotation = 0;

        public float X { get; set; }
        public float Y { get; set; }
        public float Zoom
        {
            get { return zoom; }
            set
            {
                zoom = value;
                if (zoom < 0.1f)
                {
                    zoom = 0.1f;
                }
            }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public float ScreenShake { get; set; }



        float positionClampX;
        float positionClampY;
        public void Update()
        {
            //clamp the camera position to the room
            positionClampX = (MonitorWidth / widthScale / Zoom) / 2;
            positionClampY = (MonitorHeight / heightScale / Zoom) / 2;
            Position = Vector2.Max(Position, new Vector2(0 + positionClampX, 0 + positionClampY) );

            ScreenShake = MathHelper.Lerp(ScreenShake, 0, 0.55f);

            Center = new Vector2((Position.X) + ScreenShake * random.Next(-2, 2), (Position.Y) + ScreenShake * random.Next(-2, 2));
            X = (int)Center.X;
            Y = (int)Center.Y;

            Transform = Matrix.CreateTranslation(new Vector3(-X, -Y, 0)) *
                                         Matrix.CreateRotationZ(Rotation) *
                                         Matrix.CreateScale(new Vector3(widthScale * Zoom, heightScale * Zoom, 0)) *
                                         Matrix.CreateTranslation(new Vector3(MonitorWidth * 0.5f, MonitorHeight * 0.5f, 0));

            //Mouse position that works for ALL resolutions but does not take rotation into account
            MouseX = (Mouse.X / widthScale / Zoom) + (Position.X - positionClampX);
            MouseY = (Mouse.Y / heightScale / Zoom) + (Position.Y - positionClampY);
        }
    }
}
