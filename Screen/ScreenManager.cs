using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MoonGame.Screens;
using MoonGame.Utility;
using MoonGame.States;

namespace MoonGame
{
    /// <summary>
    /// A ScreenManager for Monogame. Allows the program to switch
    /// screens (rooms / Levels etc...) in and out.
    /// DOES NOT save the state.
    /// Example Init:
    ///    ScreenManager screenManager = new ScreenManager();
    ///    screenManager.run(); // starts the game loop.
    /// </summary>
    class ScreenManager : Game
    {
        public static GraphicsDeviceManager GraphicsDeviceMgr;
        public static SpriteBatch Sprites;
        public static Dictionary<string, Texture2D> Textures2D;
        public static Dictionary<string, Texture3D> Textures3D;
        public static Dictionary<string, SpriteFont> Fonts;
        public static Dictionary<string, Model> Models;
        public static List<GameScreen> ScreenList;
        public static ContentManager ContentMgr;

        /// <summary>
        /// Default constructor. Use this in place of the default template
        /// Game1 class in Program.cs
        /// set your game resolution here. Or set it up to take ints as params.
        /// </summary>
        public ScreenManager()
        {
            GraphicsDeviceMgr = new GraphicsDeviceManager(this);
            GraphicsDeviceMgr.PreferredBackBufferWidth = 1280;
            GraphicsDeviceMgr.PreferredBackBufferHeight = 720;

            GraphicsDeviceMgr.IsFullScreen = false;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Textures2D = new Dictionary<string, Texture2D>();
            Textures3D = new Dictionary<string, Texture3D>();
            Models = new Dictionary<string, Model>();
            Fonts = new Dictionary<string, SpriteFont>();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ContentMgr = Content;
            Sprites = new SpriteBatch(GraphicsDevice);

            // Load Assets Here. WHOLE GAME, like GUI etc..

            AddScreen(new TestScreen());
        }

        protected override void UnloadContent()
        {
            foreach(var screen in ScreenList)
            {
                screen.UnloadAssets();
            }
            Textures2D.Clear();
            Textures3D.Clear();
            Fonts.Clear();
            Models.Clear();
            ScreenList.Clear();
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            try
            {
                if(Keyboard.GetState().IsKeyDown(Globals.quitKey) &&
                    Keyboard.GetState().IsKeyDown(Globals.quitKeyMod))
                {
                    QuitGameState.Quitting();
                    Exit();
                }
                if (Keyboard.GetState().IsKeyDown(Globals.fullScreenKey))
                {
                    GraphicsDeviceMgr.IsFullScreen = !GraphicsDeviceMgr.IsFullScreen;
                    GraphicsDeviceMgr.ApplyChanges();
                }


                var startIndex = ScreenList.Count - 1;
                while(ScreenList[startIndex].IsPopUp && ScreenList[startIndex].IsActive)
                {
                    startIndex--;
                }
                for(var i = startIndex; i < ScreenList.Count; i++)
                {
                    ScreenList[i].Update(gameTime);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                base.Update(gameTime);
            }
        }


        protected override void Draw(GameTime gameTime)
        {
            var startIndex = ScreenList.Count - 1;
            while(ScreenList[startIndex].IsPopUp)
            {
                startIndex--;
            }
            GraphicsDevice.Clear(ScreenList[startIndex].BackgroundColor);
            GraphicsDeviceMgr.GraphicsDevice.Clear(ScreenList[startIndex].BackgroundColor);

            for(var i = startIndex; i < ScreenList.Count; i++)
            {
                ScreenList[i].Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public static void AddFont(string fontName)
        {
            if (Fonts == null)
            {
                Fonts = new Dictionary<string, SpriteFont>();
            }
            if(!Fonts.ContainsKey(fontName))
            {
                Fonts.Add(fontName, ContentMgr.Load<SpriteFont>(fontName));
            }
        }

        public static void RemoveFont(string fontName)
        {
            if(Fonts.ContainsKey(fontName))
            {
                Fonts.Remove(fontName);
            }
        }

        public static void AddTexture2D(string textureName)
        {
            if (Textures2D == null)
            {
                Textures2D = new Dictionary<string, Texture2D>();
            }
            if(!Textures2D.ContainsKey(textureName))
            {
                Textures2D.Add(textureName, ContentMgr.Load<Texture2D>(textureName));
            }
        }

        public void RemoveTexture2D(string textureName)
        {
            if(Textures2D.ContainsKey(textureName))
            {
                Textures2D.Remove(textureName);
            }
        }

        public static void AddTexture3D(string textureName)
        {
            if (Textures3D == null)
            {
                Textures3D = new Dictionary<string, Texture3D>();
            }
            if (!Textures3D.ContainsKey(textureName))
            {
                Textures3D.Add(textureName, ContentMgr.Load<Texture3D>(textureName));
            }
        }

        public static void RemoveTexture3D(string textureName)
        {
            if (Textures3D.ContainsKey(textureName))
            {
                Textures3D.Remove(textureName);
            }
        }

        public static void AddModel(string modelName)
        {
            if (Models == null)
            {
                Models = new Dictionary<string, Model>();
            }
            if (!Models.ContainsKey(modelName))
            {
                Models.Add(modelName, ContentMgr.Load<Model>(modelName));
            }
        }

        public static void RemoveModel(string modelName)
        {
            if (Models.ContainsKey(modelName))
            {
                Models.Remove(modelName);
            }
        }

        public static void AddScreen(GameScreen screen)
        {
            if (ScreenList == null)
            {
                ScreenList = new List<GameScreen>();
            }
            ScreenList.Add(screen);
            screen.LoadAssets();
        }

        public static void RemoveScreen(GameScreen screen)
        {
            screen.UnloadAssets();
            ScreenList.Remove(screen);
            if(ScreenList.Count < 1)
            {
                AddScreen(new TestScreen());
            }
        }

        public static void ChangeScreens(GameScreen currentScreen, GameScreen newScreen)
        {
            RemoveScreen(currentScreen);
            AddScreen(newScreen);
        }
    }
}
