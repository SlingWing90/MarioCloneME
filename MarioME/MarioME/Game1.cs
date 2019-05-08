/*
 * TODO GiantGoomba überarbeiten. Evtl in (demnächst) neue BossObject-Klasse.
 */
#define XNA40
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;

using System.Windows.Forms;

using MarioME.BKGObjects;
using MarioME.TextureObjects;



/*
 * Category-Table
 * 1: Ground
 * 2: Water
 * 3: Player
 * 4: Enemy
 * 5: Aufnehmbare Objekte
 * 6: Aktivierbare Objekte
 * 7: Schrägen (Notlösung)
 * 8: Spiked Enemy
 * 9: LevelFinish
 * 10: ´NonHittable Enemy
 */

namespace MarioME
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    
    enum GameState
    {
        isLoading = 0,
        isMenu = 1,
        isOverworld = 2,
        isPlaying = 3
    }

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        

        GameState gameState;

        // Einstiegspunkt
        // Evtl noch Intro und Hauptmenü
        Settings settings;
        Overworld overworld;
        //Stages.Stage_Demo demo;
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            settings = new Settings();
            
            //this.gameState = GameState.isLoading;
            //MessageBox.Show(Window.ClientBounds.Width.ToString() + ":" + Window.ClientBounds.Height.ToString());
            graphics.PreferredBackBufferWidth = settings.WindowWidth;
            graphics.PreferredBackBufferHeight = settings.WindowHeight;
            graphics.IsFullScreen = settings.Fullscreen;
            //Deactivated += new EventHandler<EventArgs>(Game1_Deactivated);
            //Activated += new EventHandler<EventArgs>(Game1_Activated);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {   // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Startlevel festlegen
            overworld = new Overworld(Content, Window, 3, settings, this.graphics);
            //gameState = GameState.isOverworld;
            
            //demo = new Stages.Stage_Demo(16, 0, 0, Content, Window);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            /*
            if (demo != null)
            {
                demo.killStage(0);
                demo.killGeneral();
                demo = null;
            }
            */
            if (overworld != null) {
                overworld.Kill();
                overworld = null;
            }
            
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Q))
                this.Exit();

            //if(gameState == GameState.isOverworld)
            overworld.Update(gameTime, Content, Window, GraphicsDevice);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            //
            // TODO: Add your drawing code here
            
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, overworld.Cam.get_transformation(GraphicsDevice));

            overworld.Draw(spriteBatch, Window, GraphicsDevice);

            spriteBatch.End();

            base.Draw(gameTime);
        }
        
    }
}
