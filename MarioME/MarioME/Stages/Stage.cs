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

namespace MarioME.Stages
{
    class Stage
    {
        Texture2D _textureHeartSlice;
        Texture2D _textureGoldCoin;
        Texture2D _texturePlayer;
        SpriteFont _fnt;
        HealthSliceTexture _liveHUD;
        protected GoldCoinTexture _coinCount;
        JumpCharacter _player;
        protected World _world;
        Camera2D _cam;
        Vector2 _startPos;
        bool _isDisposed;

        public Stage(int startX, int startY, int lvlID, ContentManager Content, GameWindow Window) {
            this._startPos = new Vector2(startX, startY);
            
            this._world = new World(new Vector2(0, 20f));
            this._cam = new Camera2D();
            this._isDisposed = false;

            loadGeneral(Content, Window);
            
        }

        void loadGeneral(ContentManager content, GameWindow Window) {
            // Lebensanzeige            
            this._textureHeartSlice = content.Load<Texture2D>("Sprites/Misc/HealthSlices");
            this._liveHUD = new HealthSliceTexture(this._textureHeartSlice);

            //Goldanzeige
            this._textureGoldCoin = content.Load<Texture2D>("Sprites/Misc/coin");
            this._fnt = content.Load<SpriteFont>("Fonts/MarioFont");
            this._coinCount = new GoldCoinTexture(this._textureGoldCoin, this._fnt);

            // Spieler
            this._texturePlayer = content.Load<Texture2D>("Sprites/Character/PlayerSprite");
            this._player = new JumpCharacter(this._world, this._startPos, 25, 32, 5.5f, this._texturePlayer);

            // Kameraposition
            this._cam.Pos = new Vector2(Window.ClientBounds.Width / 2 - 1, Window.ClientBounds.Height / 2);   
        }

        public void updateGeneral(GameTime gameTime, GameWindow Window) {
            if (!this._isDisposed)
            {
                // Spieler "Updaten"
                this._player.Update(gameTime, Window);

                // Kameraführung
                if (ConvertUnits.ToDisplayUnits(this._player.Position.X) >= Window.ClientBounds.Width / 2 && ConvertUnits.ToDisplayUnits(this._player.Position.X) <= 4345)
                {
                    this._cam.Pos = (new Vector2(this._player.Position.X * 99.2f, this._cam._pos.Y/*Window.ClientBounds.Height / 2*/));
                }
                
                // Gold-Zähler animation aktuallisieren
                this._coinCount.Update(gameTime);

                // Leben-HUD aktuallisieren
                this._liveHUD.FormattedLive = this._player.PlayerLive;
                /*
                //Kamera-Debug Steuerung
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                    this._cam.Move(new Vector2(-5f, 0));


                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                    this._cam.Move(new Vector2(5f, 0));


                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                    this._cam.Move(new Vector2(0, -5f));


                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                    this._cam.Move(new Vector2(0, 5f));
                */
                // Physikwelt aktuallisieren
                this._world.Step((float)(gameTime.ElapsedGameTime.TotalMilliseconds * 0.001));
            }
        }

        public void drawGeneral(SpriteBatch spriteBatch, GameWindow Window) {
            if (!this._isDisposed)
            {
                // Live
                _liveHUD.Draw(spriteBatch, this._cam, Window);

                // Coin Muss in die jeweilige Stage
                this._coinCount.Draw(spriteBatch, this._cam, Window);

                // Spieler
                this._player.Draw(spriteBatch);
            }
        }

        public bool killGeneral() {
            if (!this._isDisposed)
            {
                /* Darf die Texturen nicht schmeißen, da sonst exception */
                this._textureHeartSlice = null;//.Dispose();
                this._texturePlayer = null;//.Dispose();

                this._world.BodyList.Clear();
                this._world = null;

                this._cam = null;
                this._liveHUD = null;
                this._player = null;
                this._isDisposed = true;
                return this._isDisposed;
            }
            return false;
        }

        public Camera2D Camera {
            get { return this._cam; }
        }

        public JumpCharacter Player {
            get { return this._player; }
        }

        public World World {
            get { return this._world; }
        }
    }
}
