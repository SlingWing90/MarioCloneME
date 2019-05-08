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

namespace MarioME
{
    class Overworld
    {
        GameState _gameState;
        Texture2D _texture;
        Camera2D _overworldCam;
        SpriteFont _fnt;

        Rectangle[] isleCloudRect;
        Rectangle[] isleGreenlandRect;
        Rectangle[] isleDesertRect;
        Rectangle[] isleSnowRect;
        Rectangle[] isleForestRect;

        Rectangle[] isleStagefield;

        Rectangle[] waterRect;
        int _waterFrame;

        Stages.Stage_Demo demo;

        Overworld_Player _player;
        Texture2D texturePlayer;
        bool _overworldIsOpen = true;
        int _lvlID;
        int _stageID;
        string lvlTitle = "";
        Settings _settings;

        GraphicsDeviceManager _graphics;

        public Overworld(ContentManager content, GameWindow window, int startID, Settings settings, GraphicsDeviceManager graphics) {
            this._settings = settings;
            texturePlayer = content.Load<Texture2D>("Sprites/Character/OverworldPlayer");
            
            // Spielerposition festlegen
            _player = new Overworld_Player(texturePlayer, new Vector2(32 + 32 + 32 - 8, 42 + 32 + 32 + 8));
            
            
            _fnt = content.Load<SpriteFont>("Fonts/MarioFont");

            isleCloudRect = new Rectangle[48];
            isleForestRect = new Rectangle[18];
            isleStagefield = new Rectangle[6];  // ELemente wie Levels, Arrows, Terrain, Boss etc
            waterRect = new Rectangle[4];
            _waterFrame = 0;

            _texture = content.Load<Texture2D>("Tiles/Overworld_Tileset");
            _overworldCam = new Camera2D();

            _lvlID = startID;
            _stageID = 0;

            switch (this._lvlID)
            {
                case 1:
                    _player.Position = new Vector2(_player.Position.X + 128, _player.Position.Y);
                    break;
                case 2:
                    _player.Position = new Vector2(_player.Position.X + 128, _player.Position.Y);
                    break;
                case 3:
                    _player.Position = new Vector2(_player.Position.X+128, _player.Position.Y + 58);
                    break;
                case 4:
                    _player.Position = new Vector2(_player.Position.X + 64, _player.Position.Y + 58);
                    break;
            }

            _player.NewPosition = _player.Position;


            this._graphics = graphics;

            loadWorld(startID, window);
        }

        void loadWorld(int startID, GameWindow window) {
            this._overworldCam.Pos = new Vector2(window.ClientBounds.Width / 2 - 1, window.ClientBounds.Height / 2);
                    
            /*
            switch (startID)
            {
                case 0:
                    this._overworldCam.Pos = new Vector2(window.ClientBounds.Width / 2 - 1, window.ClientBounds.Height / 2);
                    break;
            }
             */

            #region Background - Wasseranimation
            waterRect[0] = new Rectangle(0, 327, 16, 16);
            waterRect[1] = new Rectangle(16, 327, 16, 16);
            waterRect[2] = new Rectangle(32, 327, 16, 16);
            waterRect[3] = new Rectangle(48, 327, 16, 16);
            #endregion

            #region Kopfbereich
            int count = 0;
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    isleCloudRect[count] = new Rectangle(x * 32+x+1, y * 32 + y+1, 32, 32);
                    count++;
                }
            }
            #endregion

            #region Isle 1 - GoombaLand
            // Isle 1 - GoombaLan
            count = 0;
            for (int x = 8; x < 14; x++) {
                for (int y = 10; y < 13; y++)
                {
                    isleForestRect[count] = new Rectangle(x * 32+x, y * 32+y, 32, 32);
                    count++;
                }
            }
            #endregion

            #region Terrain & Design
            isleStagefield[0] = new Rectangle(20, 224, 16, 16); // Uncleared Level
            isleStagefield[1] = new Rectangle(42, 224, 16, 16); // Cleared Level
            //isleStagefield[2] = new Rectangle(29, 253, 10, 9); // Arrow
            isleStagefield[2] = new Rectangle(42, 253, 16, 8); // Weg
            isleStagefield[3] = new Rectangle(18, 277, 16, 16); // Boss Level
            isleStagefield[4] = new Rectangle(7, 307, 16, 16); // Pipe
            isleStagefield[5] = new Rectangle(34, 307, 16, 16); // Terrain (Stone) Level
            #endregion
        }

        public void Draw(SpriteBatch spriteBatch, GameWindow window, GraphicsDevice gd)
        {
            DrawIsleMap(spriteBatch, window);

            DrawStage(spriteBatch, window);

        }

        void DrawIsleMap(SpriteBatch spriteBatch, GameWindow window) {
            if (_overworldIsOpen)
            {
                #region Wasser
                for (int x = 0; x < window.ClientBounds.Width / 16; x++) {
                    for (int y = 0; y < window.ClientBounds.Height / 16; y++)
                    {
                        spriteBatch.Draw(this._texture, new Rectangle(x * 32, y * 32, 16, 16), waterRect[_waterFrame], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f); 
                    }    
                }
                #endregion

                #region Kopfbereich
                for (int x = 1; x <= window.ClientBounds.Width / 32; x++) {
                    for (int y = 1; y < 3; y++)
                    {
                        if (x == 1 && y == 1)
                            spriteBatch.Draw(this._texture, new Rectangle(x * 32, y * 32+8, 32, 32), isleCloudRect[0], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);

                        if (x >= 2 && x <= window.ClientBounds.Width / 32 && y == 1)
                            spriteBatch.Draw(this._texture, new Rectangle(x * 32-x, y * 32+8, 32, 32), isleCloudRect[6], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);

                        if (x == window.ClientBounds.Width / 32 && y == 1)
                            spriteBatch.Draw(this._texture, new Rectangle(x * 32, y * 32+8, 32, 32), isleCloudRect[42], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                        
                        if(x == 1 && y == 2)
                            spriteBatch.Draw(this._texture, new Rectangle(x * 32, y * 32+8, 32, 32), isleCloudRect[0], Color.White, 0f, new Vector2(32, 32), SpriteEffects.FlipVertically, 0f);

                        if (x >= 2 && x <= window.ClientBounds.Width / 32 && y == 2)
                            spriteBatch.Draw(this._texture, new Rectangle(x * 32 - x, y * 32+8, 32, 32), isleCloudRect[6], Color.White, 0f, new Vector2(32, 32), SpriteEffects.FlipVertically, 0f);

                        if (x == window.ClientBounds.Width / 32 && y == 2)
                            spriteBatch.Draw(this._texture, new Rectangle(x * 32, y * 32+8, 32, 32), isleCloudRect[42], Color.White, 0f, new Vector2(32, 32), SpriteEffects.FlipVertically, 0f);
                        
                    }
                }

                // Font
                spriteBatch.DrawString(_fnt, lvlTitle, new Vector2(window.ClientBounds.Width / 2-lvlTitle.Length*7, 24+8), Color.Black);
                
                #endregion

                #region Insel 1 - GoombaIsle
                // Isle 1
                spriteBatch.Draw(this._texture, new Rectangle(32+32, 96+32, 32, 32), isleForestRect[0], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                for (int x = 2; x < 7; x++)
                {
                    spriteBatch.Draw(this._texture, new Rectangle(x * 32 - (x)+32, 96+32, 32, 32), isleForestRect[4], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                }
                spriteBatch.Draw(this._texture, new Rectangle(7 * 32 - 7+32, 96+32, 32, 32), isleForestRect[15], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                for (int x = 0; x < 7; x++)
                {
                    if (x == 6)
                        spriteBatch.Draw(this._texture, new Rectangle(x * 32 - (x) + 32 - 1+32, 128 - 1+32, 32, 32), isleForestRect[4], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                    else
                        spriteBatch.Draw(this._texture, new Rectangle(x * 32 - (x) + 32+32, 128 - 1+32, 32, 32), isleForestRect[4], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);

                }
                spriteBatch.Draw(this._texture, new Rectangle(32+32, 128 + 32 - 2+32, 32, 32), isleForestRect[1], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                for (int x = 2; x < 7; x++)
                {
                    spriteBatch.Draw(this._texture, new Rectangle(x * 32 - (x)+32, 128 + 32 - 2+32, 32, 32), isleForestRect[4], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                }
                spriteBatch.Draw(this._texture, new Rectangle(7 * 32 - 7+32, 128 + 32 - 2+32, 32, 32), isleForestRect[16], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);

                spriteBatch.Draw(this._texture, new Rectangle(32+32, 128 + 64 - 4+32, 32, 32), isleForestRect[2], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                for (int x = 2; x < 7; x++)
                {
                    spriteBatch.Draw(this._texture, new Rectangle(x * 32 - (x)+32, 128 + 64 - 3+32, 32, 32), isleForestRect[8], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                }
                spriteBatch.Draw(this._texture, new Rectangle(7 * 32 - 7+32, 128 + 64 - 4+32, 32, 32), isleForestRect[17], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);

                //// Stage 1 - Wege
                // 1 - 2 - 3
                for (int x = 0; x < 8; x++) { 
                    spriteBatch.Draw(this._texture, new Rectangle(96+x*16, 86+30, 16, 9), isleStagefield[2], Color.White, 0f, new Vector2(16, 8), SpriteEffects.None, 0f);
                }
                // 3 - 4
                for (int y = 0; y < 4; y++)
                {
                    spriteBatch.Draw(this._texture, new Rectangle(204, 126+y*16, 16, 9), isleStagefield[2], Color.White, 1.58f, new Vector2(16, 8), SpriteEffects.None, 0f);
                }
                // 4 - Boss
                for (int x = 0; x < 6; x++)
                {
                    spriteBatch.Draw(this._texture, new Rectangle(96+32 + x * 16-4, 172, 16, 9), isleStagefield[2], Color.White, 0f, new Vector2(16, 8), SpriteEffects.None, 0f);
                }
                spriteBatch.Draw(this._texture, new Rectangle(96+32+5*16-100, 165, 20, 9), isleStagefield[2], Color.White, 1.58f, new Vector2(16, 8), SpriteEffects.None, 0f);
                // ? spriteBatch.Draw(this._texture, new Rectangle(96 + 32 + 5 * 16 - 100+32, 165-2, 20, 9), isleStagefield[2], Color.White, 1.58f, new Vector2(16, 8), SpriteEffects.None, 0f);

                // Boss - Pipe
                for (int x = 0; x < 4; x++)
                {
                    spriteBatch.Draw(this._texture, new Rectangle(96 + 32 + x * 16 - 4, 172-32+6, 16, 9), isleStagefield[2], Color.White, 0f, new Vector2(16, 8), SpriteEffects.None, 0f);
                }

                // Stage 1 - Levels
                // 1
                if(_settings.Level_1_1 == 0)
                    spriteBatch.Draw(this._texture, new Rectangle(64 - 8 + 32, 88 + 32, 16, 16), isleStagefield[0], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(this._texture, new Rectangle(64 - 8 + 32, 88 + 32, 16, 16), isleStagefield[1], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);
                // 2
                if (_settings.Level_1_2 == 0)
                    spriteBatch.Draw(this._texture, new Rectangle(120 + 32, 88 + 32, 16, 16), isleStagefield[0], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(this._texture, new Rectangle(120 + 32, 88 + 32, 16, 16), isleStagefield[1], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);
                
                // 3
                if (_settings.Level_1_3 == 0)
                    spriteBatch.Draw(this._texture, new Rectangle(184 + 32, 96 - 8 + 32, 16, 16), isleStagefield[0], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(this._texture, new Rectangle(184 + 32, 96 - 8 + 32, 16, 16), isleStagefield[1], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);
                
                // 4
                if (_settings.Level_1_4 == 0)
                    spriteBatch.Draw(this._texture, new Rectangle(184 + 32, 96 - 8 + 56 + 32, 16, 16), isleStagefield[0], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(this._texture, new Rectangle(184 + 32, 96 - 8 + 56 + 32, 16, 16), isleStagefield[1], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);
                
                // 5
                if (_settings.Level_1_5 == 0)
                    spriteBatch.Draw(this._texture, new Rectangle(120 + 32, 96 - 8 + 56 + 32, 16, 16), isleStagefield[0], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);
                else
                    spriteBatch.Draw(this._texture, new Rectangle(184 + 32, 96 - 8 + 56 + 32, 16, 16), isleStagefield[1], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);
                
                // Boss
                spriteBatch.Draw(this._texture, new Rectangle(64 - 8 + 32 + 32, 96 - 12 + 32 + 32, 16, 16), isleStagefield[3], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);
                
                // Pipe
                spriteBatch.Draw(this._texture, new Rectangle(64 - 8 + 32 + 32 + 32 + 32, 96 - 12 + 32 + 32, 16, 16), isleStagefield[4], Color.White, 0f, new Vector2(16, 16), SpriteEffects.None, 0f);
                
                // Isle 1 End
                #endregion

                _player.Draw(spriteBatch);
                
            }
        }

        void DrawStage(SpriteBatch spriteBatch, GameWindow window) {
            if (!_overworldIsOpen) {
                if (_stageID == 0) {
                    demo.drawStage(spriteBatch, window);
                    demo.drawGeneral(spriteBatch, window);
                    this.Cam = demo.Camera;
                }
            }
        }

        public void Update(GameTime gameTime, ContentManager content, GameWindow window, GraphicsDevice gd)
        {
            if (_overworldIsOpen)
            {
                // Wasser animation
                if (Math.Floor(gameTime.TotalGameTime.TotalMilliseconds) % 250 == 0) {
                    this._waterFrame++;
                    if (this._waterFrame >= 4)
                        this._waterFrame = 0;
                }

                // Debug Reset Level
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.E)) {
                    _settings.PropertyValues["Level_1_1"].PropertyValue = 0;
                    _settings.PropertyValues["Level_1_2"].PropertyValue = 0;
                    _settings.PropertyValues["Level_1_3"].PropertyValue = 0;
                    _settings.PropertyValues["Level_1_4"].PropertyValue = 0;
                    _settings.PropertyValues["Level_1_5"].PropertyValue = 0;
                    _settings.PropertyValues["Level_1_6"].PropertyValue = 0;
                    _settings.Save();
                }
                /*
                //Kamera-Debug Steuerung
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
                    this._overworldCam.Move(new Vector2(-5f, 0));


                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
                    this._overworldCam.Move(new Vector2(5f, 0));


                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up))
                    this._overworldCam.Move(new Vector2(0, -5f));


                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down))
                    this._overworldCam.Move(new Vector2(0, 5f));
                */
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter)) {
                    if (_stageID == 0 && _lvlID != 6) {
                        _overworldIsOpen = false;
                        
                        switch(_lvlID)
                        {
                            case 0:
                                demo = new Stages.Stage_Demo(32, 32, _lvlID, content, window, _graphics, gd);
                                break;
                            case 1:
                                demo = new Stages.Stage_Demo(32, window.ClientBounds.Height - 48 - 96, _lvlID, content, window, _graphics, gd);
                                //demo.Camera._pos = new Vector2(3524, window.ClientBounds.Height / 2);
                                break;
                            case 2:
                                demo = new Stages.Stage_Demo(32, window.ClientBounds.Height-48-96, _lvlID, content, window, _graphics, gd);
                                break;
                            case 3:
                                demo = new Stages.Stage_Demo(32, 32, _lvlID, content, window, _graphics, gd);
                                break;
                            case 4:
                                demo = new Stages.Stage_Demo(32, 32, _lvlID, content, window, _graphics, gd);   // TODO
                                break;
                            // BOSS
                            case 5:
                                demo = new Stages.Stage_Demo(32, 32, _lvlID, content, window, _graphics, gd);   // TODO
                                break;
                        }

                        this._player.Position = this._player.NewPosition;

                    }
                    
                    if (_lvlID == 6) {
                        _stageID++;
                        this._lvlID = 0;
                        this._player.Position = new Vector2(32, 32);
                        this._player.NewPosition = this._player.Position;
                    }

                    //MessageBox.Show(_lvlID.ToString());

                }

                _player.Update(gameTime);

                //Levelswitch
                //

                if (_stageID == 0)
                {
                    #region Steuerung in der Overworld - Stage 1

                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) && !_player.IsMoving)
                    {
                        switch (_lvlID) { 
                            case 0:
                            case 1:
                            case 5:
                                if (this._settings["Level_1_" + (_lvlID+1).ToString()].ToString() == "1")
                                {
                                    _player.NewPosition = new Vector2(_player.Position.X + 64, _player.Position.Y);
                                    _player.IsMoving = true;
                                    this._lvlID++;
                                }
                                break;

                            case 4:
                                _player.NewPosition = new Vector2(_player.Position.X + 64, _player.Position.Y);
                                _player.IsMoving = true;
                                this._lvlID--; // = 0;
                                break;
                            
                        }
                        //MessageBox.Show(_lvlID.ToString());
                    }
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) && !_player.IsMoving)
                    {
                        switch (_lvlID) { 
                            case 1:
                            case 2:
                                _player.NewPosition = new Vector2(_player.Position.X - 64, _player.Position.Y);
                                _player.IsMoving = true;
                                this._lvlID--; // = 0;
                                break;
                            case 3:
                                if (this._settings["Level_1_" + (_lvlID+1).ToString()].ToString() == "1")
                                {
                                    _player.NewPosition = new Vector2(_player.Position.X - 64, _player.Position.Y);
                                    _player.IsMoving = true;
                                    this._lvlID++; // = 0;
                                }
                                break;
                            case 4:
                                if (this._settings["Level_1_" + (_lvlID+1).ToString()].ToString() == "1")
                                {
                                    _player.NewPosition = new Vector2(_player.Position.X - 32, _player.Position.Y - 26);
                                    _player.IsMoving = true;
                                    this._lvlID++; // = 0;
                                }
                                break;
                            case 6: 
                                _player.NewPosition = new Vector2(_player.Position.X - 64, _player.Position.Y);
                                _player.IsMoving = true;
                                this._lvlID--; // = 0;
                                break;
                        }
                        
                        //MessageBox.Show(_lvlID.ToString());
                    }
                    
                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S) && !_player.IsMoving)
                    {
                        switch (_lvlID) { 
                            case 2:
                                if (this._settings["Level_1_" + (_lvlID + 1).ToString()].ToString() == "1")
                                {
                                    _player.NewPosition = new Vector2(_player.Position.X, _player.Position.Y + 56);
                                    _player.IsMoving = true;
                                    this._lvlID++; // = 0;
                                }
                                break;
                            case 5:
                                _player.NewPosition = new Vector2(_player.Position.X + 32, _player.Position.Y + 26);
                                _player.IsMoving = true;
                                this._lvlID--; // = 0;
                                break;
                        }
                        
                        //MessageBox.Show(_lvlID.ToString());
                    }

                    if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) && !_player.IsMoving)
                    {
                        switch (_lvlID) { 
                            case 3:
                                _player.NewPosition = new Vector2(_player.Position.X, _player.Position.Y - 56);
                                _player.IsMoving = true;
                                this._lvlID--; // = 0;
                                break;
                        }
                        //MessageBox.Show(_lvlID.ToString());

                    }

                    #endregion

                    #region Levelnamen
                    //MessageBox.Show(_lvlID.ToString());
                    //Levelnamen
                    switch (_lvlID) { 
                        case 0:
                            lvlTitle = "Die Grenze";
                            break;
                        case 1:
                            lvlTitle = "Die Bruecke";
                            break;
                        case 2:
                            lvlTitle = "King Goombas Lakai";
                            break;
                        case 3:
                            lvlTitle = "Chompy-Zentrum";
                            break;
                        case 4:
                            lvlTitle = "Goomba-Versteck";
                            break;
                        case 5:
                            lvlTitle = "King Goombas Schloss";
                            break;
                        case 6:
                            lvlTitle = "Desert-Island";
                            break;
                    }
                    #endregion
                }

                // Levelswitch Over
            }
            
            
            
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                if (demo != null)
                {
                    demo.killStage(_lvlID);
                    demo.killGeneral();
                    demo = null;
                    _overworldIsOpen = true;
                    this._player.IsMoving = false;
                    this.Cam._pos = new Vector2(window.ClientBounds.Width / 2, window.ClientBounds.Height / 2);
                }
            }



            if (!_overworldIsOpen && (demo.StageIsDisposed != true || demo != null))
            {
                demo.updateGeneral(gameTime, window);
                demo.updateStage(gameTime, window);

                if (demo.Player.PlayerLive <= 0 || demo.Player.LevelFinished)
                {
                    if (demo.Player.LevelFinished) {
                        _settings.PropertyValues["Level_1_" + (_lvlID + 1)].PropertyValue = 1;
                        _settings.Save();
                    }
                    demo.killStage(_lvlID);
                    demo.killGeneral();
                    demo = null;
                    _overworldIsOpen = true;
                    this._player.IsMoving = false;
                    this.Cam._pos = new Vector2(window.ClientBounds.Width / 2, window.ClientBounds.Height / 2);
                }
            }

        }

        public void Kill() {
            _player.Kill();
            this._overworldCam = null;
            this._texture = null;
            this._fnt = null;
            this._texture = null;
            this.isleCloudRect = null;
            this.isleDesertRect = null;
            this.isleForestRect = null;
            this.isleGreenlandRect = null;
            this.isleSnowRect = null;
            this.isleStagefield = null;
            this.waterRect = null;
            this.texturePlayer.Dispose();
            this._settings = null;
            this._graphics = null;
        }

        public GameState gameState {
            get { return this._gameState; }
            set { this._gameState = value; }
        }

        public Camera2D Cam {
            get { return this._overworldCam; }
            set { this._overworldCam = value; }
        }

        public World World {
            get {
                if (demo != null)
                {
                    return demo.World;
                }
                else
                    return null;
            }
        }
    }
}
