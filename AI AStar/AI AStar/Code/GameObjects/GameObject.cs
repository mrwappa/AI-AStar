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
    class GameObject
    {
        public static Dictionary<Type, List<GameObject>> SuperList = new Dictionary<Type, List<GameObject>>();

        public static void InitGame(SpriteBatch spriteBatch, Random random, SpriteFont font, Camera camera)
        {
            SpriteBatch = spriteBatch;
            Random = random;
            Font = font;
            Camera = camera;
        }

        public static SpriteBatch SpriteBatch;
        public static Random Random;
        public static SpriteFont Font;
        public static Camera Camera;

        public static MouseState Mouse;
        public static MouseState PreviousMouseState;
        public static KeyboardState Keyboard;
        public static KeyboardState PreviousKeyboardState;
        
        public float X { get; set; }
        public float Y { get; set; }

        public Sprite Sprite { get; set; }
        

        public float Depth { get; set; }
        public Color Color { get; set; }
        public float Alpha { get; set; }
        public float Angle { get; set; }

        public float XScale { get; set; }
        public float YScale { get; set; }

        public GameObject(float x, float y)
        {
            X = x;
            Y = y;
            AddInstance(this);
            Color = Color.White;
            Alpha = 1;
            Depth = 1;
            XScale = 1;
            YScale = 1;
        }

        public virtual void BeginUpdate()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void EndUpdate()
        {

        }

        public virtual void Draw()
        {
            if(Sprite != null)
            {
                Sprite.DrawSprite(X, Y, XScale, YScale, Angle, Color, Alpha, Depth);
            }
        }

        public virtual void DrawGUI()
        {

        }

        List<GameObject> list;
        public void AddInstance(GameObject gameObject)
        {
            Type type = gameObject.GetType();
            if (!SuperList.ContainsKey(type))
            {
                SuperList.Add(type, new List<GameObject>());
            }
            SuperList.TryGetValue(type, out list);
            list.Add(gameObject);
        }

        public void RemoveInstance(GameObject gameObject)
        {
            Type type = gameObject.GetType();
            SuperList.TryGetValue(type, out list);
            list.Remove(gameObject);
        }

        public bool GetKeyPressed(Keys key)
        {
            if (Keyboard.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key))
            {
                return true;
            }
            return false;
        }

        public bool GetMousePressed(ButtonState state)
        {
            //detta är dumt, men det funkar och jag orkar inte fixa en optimisering
            if (state == Mouse.RightButton && Mouse.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released)
            {
                return true;
            }
            else if (state == Mouse.LeftButton && Mouse.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else if (state == Mouse.MiddleButton && Mouse.MiddleButton == ButtonState.Pressed && PreviousMouseState.MiddleButton == ButtonState.Released)
            {
                return true;
            }
            return false;
        }

        public virtual GameObject GetObject(Type type)
        {
            SuperList.TryGetValue(type, out list);
            if (list.Count != 0)
            {
                return list[0];
            }
            return null;
        }

        public static Vector2 GridSnapMouse
        {
            get
            {
                return new Vector2((float)Math.Floor(Camera.MouseX / Node.NODE_SIZE) * Node.NODE_SIZE + 16,
                                    (float)Math.Floor(Camera.MouseY / Node.NODE_SIZE) * Node.NODE_SIZE + 16);
            }
        }
        public Vector2 SnapToGrid(float x, float y)
        {
            return new Vector2((float)Math.Floor(x / Node.NODE_SIZE) * Node.NODE_SIZE + 16,
                                    (float)Math.Floor(y / Node.NODE_SIZE) * Node.NODE_SIZE + 16);
        }
        public static float SnapToGrid(float x)
        {
            return ((float)Math.Floor(x / Node.NODE_SIZE) * Node.NODE_SIZE + 16);
        }

    }
}
