using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI_AStar.Code;
using AI_AStar.Code.GameObjects;

namespace AI_AStar
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;


        MouseState mouse;
        KeyboardState keyboard;
        SpriteFont font;

        Camera camera;
        int monitorWidth;
        int monitorHeight;

        Random random;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            monitorWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            monitorHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = monitorWidth;
            graphics.PreferredBackBufferHeight = monitorHeight;
            Window.Position = new Point(0, 0);
            Window.IsBorderless = true;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            random = new Random();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            Sprite.SpriteBatch = spriteBatch;
            font = Content.Load<SpriteFont>("Font");
            camera = new Camera(monitorWidth, monitorHeight);

            GameObject.InitGame(spriteBatch, random, font, camera, Content);
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            keyboard = Keyboard.GetState();
            mouse = Mouse.GetState();

            GameObject.Keyboard = keyboard;
            GameObject.Mouse = mouse;
            Camera.Mouse = mouse;

            foreach (KeyValuePair<Type, List<GameObject>> list in GameObject.SuperList.ToList())
            {
                foreach (GameObject obj in list.Value.ToList())
                {
                    obj.BeginUpdate();
                }
            }

            foreach (KeyValuePair<Type, List<GameObject>> list in GameObject.SuperList.ToList())
            {
                foreach (GameObject obj in list.Value.ToList())
                {
                    obj.Update();
                }
            }

            foreach (KeyValuePair<Type, List<GameObject>> list in GameObject.SuperList.ToList())
            {
                foreach (GameObject obj in list.Value.ToList())
                {
                    obj.EndUpdate();
                }
            }
            GameObject.PreviousKeyboardState = keyboard;
            GameObject.PreviousMouseState = mouse;

            camera.Update();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //DRAW GAME OBJECTS
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.Transform);
            foreach (KeyValuePair<Type, List<GameObject>> list in GameObject.SuperList.ToList())
            {
                foreach (GameObject obj in list.Value.ToList())
                {
                    obj.Draw();
                }
            }
            spriteBatch.End();

            //DRAW ON GUI LAYER
            spriteBatch.Begin(SpriteSortMode.FrontToBack,
           BlendState.AlphaBlend,
           SamplerState.PointClamp, null, null, null, Matrix.CreateTranslation(new Vector3(0 - monitorWidth, 0 - monitorHeight, 0)) * Matrix.CreateRotationZ(0) * Matrix.CreateScale(new Vector3(1, 1, 0)) *
                Matrix.CreateTranslation(new Vector3(monitorWidth, monitorHeight, 0)));

            foreach (KeyValuePair<Type, List<GameObject>> list in GameObject.SuperList.ToList())
            {
                foreach (GameObject obj in list.Value)
                {
                    obj.DrawGUI();
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
