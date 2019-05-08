using System;
using System.Collections.Generic;
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

using ProjectMercury;
using ProjectMercury.Emitters;
using ProjectMercury.Modifiers;
using ProjectMercury.Renderers;
using ProjectMercury.Controllers;

using MarioME.BKGObjects;
using MarioME.TextureObjects;
using MarioME.EnemyObjects;
using MarioME.ActionObjects;
using MarioME.ItemObjects;
using MarioME.Background;
using MarioME.TrackObjects;
using MarioME.Helper;

namespace MarioME.Stages
{
    class Stage_Demo: Stage
    {
        //// Helper
        PathClass PathGenerator;
        CameraEffect cameraEffect;
        //// Hintergrundklasse
        Background.Background _background;
        ////  Texturen
        // Tileset
        Texture2D _textureTileset;
        // Enemies
        Texture2D _textureGoomba;
        Texture2D _textureSpikedGoomba;
        Texture2D _textureFlyingGoomba;
        Texture2D _textureKoopa;
        Texture2D _textureFishBertha;
        Texture2D _textureFuzzy;
        Texture2D _textureGiantGoomba;
        Texture2D _textureChompy;
        // Items
        Texture2D _textureCoin;
        Texture2D _textureItems;
        //// Schriftarten
        SpriteFont _font;
        //// Titeltext
        string _headText;
        //// Listen
        List<BKGObject> _bkgList;
        List<TextureObject> _txtList;
        List<ActionObject> _actionList;
        List<EnemyObject> _enemyList;
        List<TrackObject> _trackList;
        List<ItemObject> _itemList;

        bool _isDisposed;

        //// Mercury
        Renderer _renderer;
        ParticleEffect _particleEffect;

        GraphicsDevice _graphics;

        int _lvlID;

        /*
         * Hinweis: lvlID ist in diesem Fall die richtige LevelID. Die Tilessets werden in der jeweiligen Klasse geladen,
         * und durch die ID werden die Koordinaten bzw die Sprites identifiziert
         */
        public Stage_Demo(int startX, int startY, int lvlID, ContentManager Content, GameWindow Window, GraphicsDeviceManager graphics, GraphicsDevice gd):base(startX, startY, lvlID, Content, Window) {
            PathGenerator = new PathClass();
            cameraEffect = new CameraEffect();

            this._lvlID = lvlID;

            this._graphics = gd;

            this._bkgList = new List<BKGObject>();
            this._txtList = new List<TextureObject>();
            this._enemyList = new List<EnemyObject>();
            this._actionList = new List<ActionObject>();
            this._trackList = new List<TrackObject>();
            this._itemList = new List<ItemObject>();
            
            this._font = Content.Load<SpriteFont>("Fonts/MarioFont");
            this._headText = "1-" + (lvlID + 1).ToString();
            //this._score = 0;

            loadStage(lvlID, Content, Window, graphics);
        }

        
        void loadStage(int lvlID, ContentManager Content, GameWindow Window, GraphicsDeviceManager graphics) {

            _background = new Background.Background(4800, Window.ClientBounds.Height, this.Camera, Content.Load<Texture2D>("Backgrounds/ForestLand"), 0);

            this._textureTileset = Content.Load<Texture2D>("Tiles/TS_1");
            this._textureGoomba = Content.Load<Texture2D>("Sprites/Enemies/PaperGoomba");
            this._textureKoopa = Content.Load<Texture2D>("Sprites/Enemies/PaperKoopa");
            this._textureCoin = Content.Load<Texture2D>("Sprites/Misc/coin");
            this._textureSpikedGoomba = Content.Load<Texture2D>("Sprites/Enemies/PaperSpikedGoomba");
            this._textureItems = Content.Load<Texture2D>("Sprites/Misc/PaperItems");
            this._textureFishBertha = Content.Load<Texture2D>("Sprites/Enemies/FishBertha");
            this._textureFuzzy = Content.Load<Texture2D>("Sprites/Enemies/PaperFuzzy");
            this._textureFlyingGoomba = Content.Load<Texture2D>("Sprites/Enemies/FlyingGoomba");
            this._textureChompy = Content.Load<Texture2D>("Sprites/Enemies/Chompy");
            // Boss
            this._textureGiantGoomba = Content.Load<Texture2D>("Sprites/Enemies/PaperGiantGoomba");

            Ground groundTemp;
            Bridge bridgeTemp;
            FallingBridge fbridgeTemp;
            Fence fenceTemp;
            Brick brickTemp;
            
            Coin coinTemp;
            Spring springTemp;
            Pipe pipeTemp;
            Live liveTemp;
            Water waterTemp;
            Block blockTemp;
            GroundBevel groundbevelTemp;
            Track_1 podTrackTemp;
            // In das neue AI-Konzept implementiert
            Fuzzy fuzzyTemp;
            Goomba goombaTemp;
            SpikedGoomba spikedGoombaTemp;
            FlyingGoomba flyingGoombaTemp;
            FlyingSpikedGoomba flyingSpikedGoombaTemp;
            Chompy chompyTemp;
            GiantGoomba giantGoombaTemp;
                
            //Effekte
            this._renderer = new SpriteBatchRenderer
            {
                GraphicsDeviceService = graphics
            };

            this._particleEffect = new ParticleEffect();

            this._renderer.LoadContent(Content);
            this._particleEffect = Content.Load<ParticleEffect>("Effects/Weather/Rain/SimpleRain");
            this._particleEffect.LoadContent(Content);
            this._particleEffect.Initialise();

            // Linken Bereich mit einem Body "sperren"
            Body b = BodyFactory.CreateRectangle(this._world, ConvertUnits.ToSimUnits(16), ConvertUnits.ToSimUnits(Window.ClientBounds.Height + Window.ClientBounds.Height / 2), 1, ConvertUnits.ToSimUnits(new Vector2(0, 0)));
            b.BodyType = BodyType.Static;

            switch (lvlID) {
                //Level 1
                case 0:
                    #region Level 1
                    ////// Temporär
                    //
                    //// Texturen und BKG
                    //
                    //// Ground
                    //
                    #region Ground
                    groundTemp = new Ground(this._world, new Vector2(48, Window.ClientBounds.Height - 48 - 16), 96,88, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasLeftDown = true;
                    groundTemp.HasEnd = true;
                    this._bkgList.Add(groundTemp);
                    /* Ground-Texture X:-4 Zeichenn*/
                    groundTemp = new Ground(this._world, new Vector2(/*236*/243, Window.ClientBounds.Height - 48 - 16), 96, 88, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;
                    groundTemp.HasBegin = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(239+239+3, Window.ClientBounds.Height - 48 - 16-64), 96, 184, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;
                    groundTemp.HasLeftDown = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(812 + 12+2+2, Window.ClientBounds.Height - 48 + 4), 256 + 128, 128, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;
                    groundTemp.HasLeftDown = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(1380-4+11, Window.ClientBounds.Height - 48+4), 256+128, 128, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;
                    groundTemp.HasLeftDown = true;
                    this._bkgList.Add(groundTemp);
                    
                    groundTemp = new Ground(this._world, new Vector2(1748+4, Window.ClientBounds.Height - 48+4-128), 128, 512-128, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;
                    groundTemp.HasLeftDown = true;
                    this._bkgList.Add(groundTemp);
                    /////////
                    groundTemp = new Ground(this._world, new Vector2(2180+10+2-96-16, Window.ClientBounds.Height - 48+4), 512-192, 128, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;
                    groundTemp.HasLeftDown = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(2180+10+2-96+192+128, Window.ClientBounds.Height - 48+4-256+96), 96, 512-64, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;
                    groundTemp.HasLeftDown = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(2832-128, Window.ClientBounds.Height - 220), 128, 96, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    groundTemp.HasBottom = true;
                    this._bkgList.Add(groundTemp);
                    
                    groundTemp = new Ground(this._world, new Vector2(2832+128, Window.ClientBounds.Height - 220), 128, 96, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    groundTemp.HasBottom = true;
                    this._bkgList.Add(groundTemp);
                    
                    groundTemp = new Ground(this._world, new Vector2(2832+256+128, Window.ClientBounds.Height - 220), 128, 96, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    groundTemp.HasBottom = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(2832 + 256 + 128 +256+128+ 10 + 2 - 96, Window.ClientBounds.Height - 48 + 4), 512 - 192, 128, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;
                    groundTemp.HasLeftDown = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(4400, Window.ClientBounds.Height - 48 + 4), 256, 128, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;
                    groundTemp.HasLeftDown = true;
                    this._bkgList.Add(groundTemp);
                    #endregion
                    //
                    #region Bridge
                    // 1
                    bridgeTemp = new Bridge(this._world, new Vector2(117 /* +46*/, Window.ClientBounds.Height-92), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;
                    //bridgeTemp.ClipToNext(_bkgList[0].body, this._world);

                    this._bkgList.Add(bridgeTemp);
                    
                    // 3
                    bridgeTemp = new Bridge(this._world, new Vector2(182 /* +46*/, Window.ClientBounds.Height-92), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;
                    //bridgeTemp.ClipToNext(_bkgList[/*_bkgList.Count-*/1].body, this._world);
                    //bridgeTemp.ClipToLast(_bkgList[1].body, this._world);

                    this._bkgList.Add(bridgeTemp);

                    // 2
                    bridgeTemp = new Bridge(this._world, new Vector2(149 /* +46*/, Window.ClientBounds.Height - 92), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;
                    //bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 1].body, this._world);
                    //bridgeTemp.ClipToLast(_bkgList[_bkgList.Count - 2].body, this._world);

                    this._bkgList.Add(bridgeTemp);

                    ////////////////
                    // 1
                    bridgeTemp = new Bridge(this._world, new Vector2(549 /* +46*/, Window.ClientBounds.Height - 92), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;
                    //bridgeTemp.ClipToNext(_bkgList[2].body, this._world);

                    this._bkgList.Add(bridgeTemp);

                    // 3
                    bridgeTemp = new Bridge(this._world, new Vector2(615, Window.ClientBounds.Height - 92), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;
                    //bridgeTemp.ClipToNext(_bkgList[3].body, this._world);
                    //bridgeTemp.ClipToLast(_bkgList[1].body, this._world);

                    this._bkgList.Add(bridgeTemp);

                    // 2
                    bridgeTemp = new Bridge(this._world, new Vector2(582, Window.ClientBounds.Height - 92), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;
                    //bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 2].body, this._world);
                    //bridgeTemp.ClipToLast(_bkgList[_bkgList.Count - 1].body, this._world);

                    this._bkgList.Add(bridgeTemp);
                    ////////////////////////
                    // 1
                    bridgeTemp = new Bridge(this._world, new Vector2(1834 /* +46*/, Window.ClientBounds.Height - 92), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;
                    //bridgeTemp.ClipToNext(_bkgList[2].body, this._world);

                    this._bkgList.Add(bridgeTemp);

                    // 3
                    bridgeTemp = new Bridge(this._world, new Vector2(1834+64+2, Window.ClientBounds.Height - 92), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;
                    //bridgeTemp.ClipToNext(_bkgList[3].body, this._world);
                    //bridgeTemp.ClipToLast(_bkgList[1].body, this._world);

                    this._bkgList.Add(bridgeTemp);

                    // 2
                    bridgeTemp = new Bridge(this._world, new Vector2(1834+32+1, Window.ClientBounds.Height - 92), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;
                    //bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 2].body, this._world);
                    //bridgeTemp.ClipToLast(_bkgList[_bkgList.Count - 1].body, this._world);

                    this._bkgList.Add(bridgeTemp);
                    #endregion
                    //
                    #region Falling Bridge
                    fbridgeTemp = new FallingBridge(this._world, new Vector2(382, Window.ClientBounds.Height - 64-64-32), 96, 32, this._textureTileset);
                    fbridgeTemp.loadTileset(0);

                    this._bkgList.Add(fbridgeTemp);

                    for (int x = 0; x < 6; x++) {
                        fbridgeTemp = new FallingBridge(this._world, new Vector2(571 + 256 + 256 - 96 + 21 + x * 32, Window.ClientBounds.Height - 64 - 64 - 64 + 7), 32, 32, this._textureTileset);
                        fbridgeTemp.loadTileset(0);

                        this._bkgList.Add(fbridgeTemp);
                    }
                    #endregion
                    //
                    #region Brick
                    brickTemp = new Brick(this._world, new Vector2(865.5f+7, Window.ClientBounds.Height-256+128-32+6+32), 128+128, 32, this._textureTileset);
                    brickTemp.loadTileset(0);
                    this._bkgList.Add(brickTemp);

                    brickTemp = new Brick(this._world, new Vector2(865.5f + 16 + 6, Window.ClientBounds.Height - 256 + 128 - 32 + 6 ), 128 + 64 + 32, 32, this._textureTileset);
                    brickTemp.loadTileset(0);
                    this._bkgList.Add(brickTemp);

                    brickTemp = new Brick(this._world, new Vector2(897.5f + 5, Window.ClientBounds.Height - 256 + 128 - 32 + 6-96+32+33), 128 + 64, 32, this._textureTileset);
                    brickTemp.loadTileset(0);
                    this._bkgList.Add(brickTemp);

                    brickTemp = new Brick(this._world, new Vector2(1311.5f, Window.ClientBounds.Height - 256 + 128 - 32 + 6 + 32), 128 + 128, 32, this._textureTileset);
                    brickTemp.loadTileset(0);
                    this._bkgList.Add(brickTemp);

                    brickTemp = new Brick(this._world, new Vector2(1295.5f, Window.ClientBounds.Height - 256 + 128 - 32 + 6), 128 + 64 + 32, 32, this._textureTileset);
                    brickTemp.loadTileset(0);
                    this._bkgList.Add(brickTemp);

                    brickTemp = new Brick(this._world, new Vector2(1279.5f, Window.ClientBounds.Height - 256 + 128 - 32 + 6 - 96 + 32 + 33), 128 + 64, 32, this._textureTileset);
                    brickTemp.loadTileset(0);
                    this._bkgList.Add(brickTemp);

                    brickTemp = new Brick(this._world, new Vector2(1600f+16, Window.ClientBounds.Height - 295+64), 32, 32, this._textureTileset);
                    brickTemp.loadTileset(0);
                    this._bkgList.Add(brickTemp);

                    brickTemp = new Brick(this._world, new Vector2(2180 + 10 + 2 - 96 + 192 + 128-64, Window.ClientBounds.Height-320), 32, 32, this._textureTileset);
                    brickTemp.loadTileset(0);
                    this._bkgList.Add(brickTemp);

                    brickTemp = new Brick(this._world, new Vector2(3964, Window.ClientBounds.Height -32-16-32), 32, 32, this._textureTileset);
                    brickTemp.loadTileset(0);
                    this._bkgList.Add(brickTemp);
                    
                    brickTemp = new Brick(this._world, new Vector2(3994, Window.ClientBounds.Height -32-16-32), 32, 32, this._textureTileset);
                    brickTemp.loadTileset(0);
                    this._bkgList.Add(brickTemp);
                    
                    brickTemp = new Brick(this._world, new Vector2(4024, Window.ClientBounds.Height -32-16-32), 32, 32, this._textureTileset);
                    brickTemp.loadTileset(0);
                    this._bkgList.Add(brickTemp);
                    #endregion
                    // 
                    #region Water
                    //waterTemp = new Water(new Vector2(64, Window.ClientBounds.Height - 64), 4800, 64, this._textureTileset);
                    //waterTemp.loadTiles(0);
                    //this._txtList.Add(waterTemp);
                    //this._animWaterList.Add(new Water(new Vector2(64, Window.ClientBounds.Height - 64), 4800, 64, this._textureTileset));
                    //this._animWaterList[this._animWaterList.Count - 1].loadTiles(0);
                    
                    #endregion
                    //
                    #region Fence
                    fenceTemp = new Fence(new Vector2(0, Window.ClientBounds.Height - 128-12), 96, 32, this._textureTileset);
                    fenceTemp.loadTiles(0);
                    this._txtList.Add(fenceTemp);

                    fenceTemp = new Fence(new Vector2(96+96, Window.ClientBounds.Height - 128-12), 96, 32, this._textureTileset);
                    fenceTemp.loadTiles(0);
                    this._txtList.Add(fenceTemp);
                    #endregion
                    //
                    #region Pipe
                    pipeTemp = new Pipe(this._world, new Vector2(4400, Window.ClientBounds.Height - 48 + 4-93), 50, 32, this._textureTileset, true);
                    pipeTemp.loadTileset(0);
                    this._bkgList.Add(pipeTemp);
                    #endregion
                    //
                    //// Enemies 
                    //
                    #region Goomba
                    goombaTemp = new Goomba(this._textureGoomba, this._world, new Vector2(478, Window.ClientBounds.Height - 288 + 54));
                    goombaTemp.MoveSpeed = 0.01f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(28, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, this._world, new Vector2(624, Window.ClientBounds.Height - 123));
                    goombaTemp.MoveSpeed = 0.008f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(56, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, this._world, new Vector2(1280, Window.ClientBounds.Height - 217));
                    goombaTemp.MoveSpeed = 0.005f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(80, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, this._world, new Vector2(896, Window.ClientBounds.Height - 217));
                    goombaTemp.MoveSpeed = 0.006f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(80, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);
                    
                    // ???
                    //goombaTemp = new Goomba(this._world, new Vector2(960 + 128, Window.ClientBounds.Height - 288 + 32 - 128), 20, 22, 1f, this._textureGoomba, true, 128f);
                    //this._enemyList.Add(goombaTemp);
                    // ???
                    //goombaTemp = new Goomba(this._world, new Vector2(1024 + 64 , 250), 20, 22, 1.5f, this._textureGoomba, true, 128f);
                    //this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, this._world, new Vector2(928, Window.ClientBounds.Height - 217));
                    goombaTemp.MoveSpeed = 0.004f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(48, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, this._world, new Vector2(1280, Window.ClientBounds.Height - 217));
                    goombaTemp.MoveSpeed = 0.004f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(80, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, this._world, new Vector2(2044, Window.ClientBounds.Height - 123));
                    goombaTemp.MoveSpeed = 0.004f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(120, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, this._world, new Vector2(1948, Window.ClientBounds.Height - 123));
                    goombaTemp.MoveSpeed = 0.005f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(120, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, this._world, new Vector2(1746, Window.ClientBounds.Height - 370));
                    goombaTemp.MoveSpeed = 0.006f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(48, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, this._world, new Vector2(1948, Window.ClientBounds.Height - 123));
                    goombaTemp.MoveSpeed = 0.006f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(96, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, this._world, new Vector2(1948, Window.ClientBounds.Height - 123));
                    goombaTemp.MoveSpeed = 0.007f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(80, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, this._world, new Vector2(3476, Window.ClientBounds.Height - 123));
                    goombaTemp.MoveSpeed = 0.007f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(96, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);
                    #endregion
                    //
                    #region Spiked Goomba
                    spikedGoombaTemp = new SpikedGoomba(this._textureSpikedGoomba, this._world, new Vector2(2415, Window.ClientBounds.Height-446));
                    spikedGoombaTemp.MoveSpeed = 0.008f;
                    spikedGoombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(32, spikedGoombaTemp.Body);
                    this._enemyList.Add(spikedGoombaTemp);

                    spikedGoombaTemp = new SpikedGoomba(this._textureSpikedGoomba, this._world, new Vector2(2698, Window.ClientBounds.Height-286));
                    spikedGoombaTemp.MoveSpeed = 0.006f;
                    spikedGoombaTemp.FrameSpeed = 100;
                    spikedGoombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(48, spikedGoombaTemp.Body);
                    this._enemyList.Add(spikedGoombaTemp);

                    spikedGoombaTemp = new SpikedGoomba(this._textureSpikedGoomba, this._world, new Vector2(3212, Window.ClientBounds.Height-286));
                    spikedGoombaTemp.MoveSpeed = 0.006f;
                    spikedGoombaTemp.FrameSpeed = 100;
                    spikedGoombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(48, spikedGoombaTemp.Body);
                    this._enemyList.Add(spikedGoombaTemp);
                    #endregion
                    //
                    #region Flying Goomba
                    flyingGoombaTemp = new FlyingGoomba(this._textureFlyingGoomba, this._world, new Vector2(2280, Window.ClientBounds.Height - 400));
                    flyingGoombaTemp.MoveSpeed = 0.005f;
                    flyingGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(200, flyingGoombaTemp.Body, true);
                    this._enemyList.Add(flyingGoombaTemp);

                    flyingGoombaTemp = new FlyingGoomba(this._textureFlyingGoomba, this._world, new Vector2(2280+500+32, Window.ClientBounds.Height - 200));
                    flyingGoombaTemp.MoveSpeed = 0.005f;
                    flyingGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(200, flyingGoombaTemp.Body, false);
                    this._enemyList.Add(flyingGoombaTemp);

                    flyingGoombaTemp = new FlyingGoomba(this._textureFlyingGoomba, this._world, new Vector2(2280+500+32+32, Window.ClientBounds.Height - 400));
                    flyingGoombaTemp.MoveSpeed = 0.005f;
                    flyingGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(200, flyingGoombaTemp.Body, true);
                    this._enemyList.Add(flyingGoombaTemp);

                    flyingGoombaTemp = new FlyingGoomba(this._textureFlyingGoomba, this._world, new Vector2(2280+500+32+256, Window.ClientBounds.Height - 400));
                    flyingGoombaTemp.MoveSpeed = 0.005f;
                    flyingGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(200, flyingGoombaTemp.Body, true);
                    this._enemyList.Add(flyingGoombaTemp);
                    
                    flyingGoombaTemp = new FlyingGoomba(this._textureFlyingGoomba, this._world, new Vector2(2280+500+32+32+256, Window.ClientBounds.Height - 200));
                    flyingGoombaTemp.MoveSpeed = 0.005f;
                    flyingGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(200, flyingGoombaTemp.Body, false);
                    this._enemyList.Add(flyingGoombaTemp);
                    #endregion
                    //
                    #region Flying Spiked Goomba
                    flyingSpikedGoombaTemp= new FlyingSpikedGoomba(this._textureFlyingGoomba, this._world, new Vector2(3700, Window.ClientBounds.Height -200));
                    flyingSpikedGoombaTemp.MoveSpeed = 0.008f;
                    flyingSpikedGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(32, flyingSpikedGoombaTemp.Body, true);
                    this._enemyList.Add(flyingSpikedGoombaTemp);

                    flyingSpikedGoombaTemp= new FlyingSpikedGoomba(this._textureFlyingGoomba, this._world, new Vector2(3732, Window.ClientBounds.Height -264));
                    flyingSpikedGoombaTemp.MoveSpeed = 0.008f;
                    flyingSpikedGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(64, flyingSpikedGoombaTemp.Body, true);
                    this._enemyList.Add(flyingSpikedGoombaTemp);
                    
                    flyingSpikedGoombaTemp= new FlyingSpikedGoomba(this._textureFlyingGoomba, this._world, new Vector2(3764, Window.ClientBounds.Height -328));
                    flyingSpikedGoombaTemp.MoveSpeed = 0.008f;
                    flyingSpikedGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(96, flyingSpikedGoombaTemp.Body, true);
                    this._enemyList.Add(flyingSpikedGoombaTemp);
                    #endregion
                    //
                    //
                    // ActionObjects
                    #region Spring
                    springTemp = new Spring(this._world, new Vector2(2208, Window.ClientBounds.Height -128-40+32+16), 16, 18, 1f, this._textureTileset);
                    springTemp.loadTiles(0);
                    this._actionList.Add(springTemp);
                    //this._springList.Add(springTemp);

                    springTemp = new Spring(this._world, new Vector2(2832 + 256 + 128 + 256 + 128 + 10 + 2 - 96+96, Window.ClientBounds.Height - 128-40+32+16), 16, 18, 1f, this._textureTileset);
                    springTemp.loadTiles(0);
                    this._actionList.Add(springTemp);

                    springTemp = new Spring(this._world, new Vector2(3994, Window.ClientBounds.Height - 128 - 40 + 32 + 16+8+4), 16, 18, 1f, this._textureTileset);
                    springTemp.loadTiles(0);
                    this._actionList.Add(springTemp);
                    #endregion
                    //
                    // ItemObjects
                    #region Coins
                    coinTemp = new Coin(this._world, new Vector2(370, Window.ClientBounds.Height - 128-40), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    //this._coinList.Add(coinTemp);
                    
                    coinTemp = new Coin(this._world, new Vector2(402, Window.ClientBounds.Height - 128-40), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    //this._coinList.Add(coinTemp);

                    coinTemp = new Coin(this._world, new Vector2(434, Window.ClientBounds.Height - 128-40), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    //this._coinList.Add(coinTemp);
  
                    coinTemp = new Coin(this._world, new Vector2(1028, Window.ClientBounds.Height - 128 - 40-20-2), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    //this._coinList.Add(coinTemp);

                    coinTemp = new Coin(this._world, new Vector2(1028 + 32, Window.ClientBounds.Height - 128 - 40 - 20-2), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    //this._coinList.Add(coinTemp);

                    coinTemp = new Coin(this._world, new Vector2(1028 + 64, Window.ClientBounds.Height - 128 - 40 - 20-2), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    //this._coinList.Add(coinTemp);

                    coinTemp = new Coin(this._world, new Vector2(1028 + 96, Window.ClientBounds.Height - 128 - 40 - 20-2), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    //this._coinList.Add(coinTemp);

                    coinTemp = new Coin(this._world, new Vector2(1028 + 96 + 32, Window.ClientBounds.Height - 128 - 40 - 20-2), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    //this._coinList.Add(coinTemp);

                    coinTemp = new Coin(this._world, new Vector2(1028 + 96 + 64, Window.ClientBounds.Height - 128 - 40 - 20-2), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    
                    coinTemp = new Coin(this._world, new Vector2(1380 - 4 + 512+256-16-96, Window.ClientBounds.Height - 48 + 4-48-8), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    
                    coinTemp = new Coin(this._world, new Vector2(1380 - 4 + 512+256+32-16-96, Window.ClientBounds.Height - 48 + 4-48-8), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);

                    coinTemp = new Coin(this._world, new Vector2(1380 - 4 + 512+256+64-16-96, Window.ClientBounds.Height - 48 + 4-48-8), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);

                    coinTemp = new Coin(this._world, new Vector2(1380 - 4 + 512+256+64-16-96+32, Window.ClientBounds.Height - 48 + 4-48-8), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);

                    coinTemp = new Coin(this._world, new Vector2(1380 - 4 + 512+256+64-16-96+64, Window.ClientBounds.Height - 48 + 4-48-8), 32, 24, 1, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    #endregion
                    //
                    #region Live
                    liveTemp = new Live(this.World, new Vector2(1600f + 16+8, Window.ClientBounds.Height - 295 + 64-16), 19, 20, 1f, this._textureItems);
                    _itemList.Add(liveTemp);

                    liveTemp = new Live(this.World, new Vector2(2832 + 128+8, Window.ClientBounds.Height - 220-32-16), 19, 20, 1f, this._textureItems);
                    _itemList.Add(liveTemp);
                    #endregion
                    //
                    #endregion
                    break;

                // Level 2     
                case 1:
                    #region Level 2
                    ////// Temporär
                    //
                    //// BKG Objects
                    //
                    // Ground
                    //
                    #region Ground
                    groundTemp = new Ground(this._world, new Vector2(68, Window.ClientBounds.Height - 48 - 16), 128, 88, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasRightDown = true;

                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(68, Window.ClientBounds.Height - 256-64), 128, 88, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasEnd = true;
                    groundTemp.HasBegin = true;
                    groundTemp.HasBottom = true;

                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(256+40, Window.ClientBounds.Height-174 ), 128, 256+128, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasLeftDown = true;
                    groundTemp.HasRightDown = true;

                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(560, Window.ClientBounds.Height-174 ), 96, 256+128, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasLeftDown = true;
                    
                    this._bkgList.Add(groundTemp);

                    groundbevelTemp = new GroundBevel(this._world, new Vector2(602.5f, Window.ClientBounds.Height - 374+8), 698.5f, Window.ClientBounds.Height - 374+8 + 96, 256+32, this._textureTileset);
                    groundbevelTemp.loadTiles(0);

                    this._bkgList.Add(groundbevelTemp);

                    groundTemp = new Ground(this._world, new Vector2(602.5f+48+99, Window.ClientBounds.Height-174+32 ), 96, 256, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;

                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(815 + 128+64-7, Window.ClientBounds.Height - 200 - 64 + 132-20+10), 96, 256, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;
                    groundTemp.HasLeftDown = true;

                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(2048, Window.ClientBounds.Height - 200 - 64 + 132-20+10), 96, 256, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasLeftDown = true;

                    this._bkgList.Add(groundTemp);
                    
                    groundbevelTemp = new GroundBevel(this._world, new Vector2(2091.5f, Window.ClientBounds.Height - 270), 2187.5f, Window.ClientBounds.Height - 174, 256 + 32, this._textureTileset);
                    groundbevelTemp.loadTiles(0);

                    this._bkgList.Add(groundbevelTemp);

                    groundTemp = new Ground(this._world, new Vector2(2187.5f+51, Window.ClientBounds.Height-86), 96, 174, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;

                    this._bkgList.Add(groundTemp);
                    
                    groundTemp = new Ground(this._world, new Vector2(2302.5f + 224+88, Window.ClientBounds.Height-86), 96, 174, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasLeftDown = true;
                    this._bkgList.Add(groundTemp);

                    groundbevelTemp = new GroundBevel(this._world, new Vector2(2091.5f+512+48, Window.ClientBounds.Height - 174), 2187.5f+512+48, Window.ClientBounds.Height - 270, 256 + 32, this._textureTileset);
                    groundbevelTemp.loadTiles(0);
                    this._bkgList.Add(groundbevelTemp);

                    groundTemp = new Ground(this._world, new Vector2(2790.5f, Window.ClientBounds.Height-86-49), 96, 174+96, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasRightDown = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(2790.5f + 512 + 196, Window.ClientBounds.Height - 240), 96, 64, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    groundTemp.HasBottom = true;
                    this._bkgList.Add(groundTemp);
                    
                    groundTemp = new Ground(this._world, new Vector2(2790.5f + 512 + 256+512, Window.ClientBounds.Height - 86), 256, 128, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasLeftDown = true;
                    groundTemp.HasRightDown = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(2790.5f + 512 + 256+512+512+128, Window.ClientBounds.Height - 86), 128, 128, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasLeftDown = true;
                    groundTemp.HasRightDown = true;
                    this._bkgList.Add(groundTemp);
                    #endregion

                    //Pipe
                    #region Pipe
                    pipeTemp = new Pipe(this._world, new Vector2(4100-32, Window.ClientBounds.Height - 128-33-16-4), 52,32,this._textureTileset, true);
                    pipeTemp.loadTileset(0);
                    _bkgList.Add(pipeTemp);
                    #endregion
                    
                    //Bridge
                    //
                    #region Bridge
                    ////////////////////////////
                    // Grundpfeiler 1
                    bridgeTemp = new Bridge(this._world, new Vector2(376, Window.ClientBounds.Height-350), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;

                    _bkgList.Add(bridgeTemp);
                    // Grundpfeiler 2
                    bridgeTemp = new Bridge(this._world, new Vector2(503, Window.ClientBounds.Height - 350), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;

                    _bkgList.Add(bridgeTemp);
                    // 1
                    bridgeTemp = new Bridge(this._world, new Vector2(408, Window.ClientBounds.Height-350), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 2].body, _world);

                    _bkgList.Add(bridgeTemp);
                    // 2
                    bridgeTemp = new Bridge(this._world, new Vector2(440, Window.ClientBounds.Height - 350), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 1].body, _world);

                    _bkgList.Add(bridgeTemp);
                    // 3
                    bridgeTemp = new Bridge(this._world, new Vector2(471, Window.ClientBounds.Height - 350), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 1].body, _world);
                    bridgeTemp.ClipToLast(_bkgList[_bkgList.Count - 3].body, _world);
                    
                    _bkgList.Add(bridgeTemp);
                    /////////////////////////////
                    //Grundpfeiler 1
                    bridgeTemp = new Bridge(this._world, new Vector2(815, Window.ClientBounds.Height - 200-64+10), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;

                    _bkgList.Add(bridgeTemp);
                    //Grundpfeiler 2
                    bridgeTemp = new Bridge(this._world, new Vector2(815+128, Window.ClientBounds.Height - 200-64+10), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;

                    _bkgList.Add(bridgeTemp);
                    // 1
                    bridgeTemp = new Bridge(this._world, new Vector2(815+32, Window.ClientBounds.Height - 200-64+10), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 2].body, this._world);
                    _bkgList.Add(bridgeTemp);
                    // 2
                    bridgeTemp = new Bridge(this._world, new Vector2(815+64, Window.ClientBounds.Height - 200-64+10), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 1].body, this._world);
                    _bkgList.Add(bridgeTemp);
                    // 3
                    bridgeTemp = new Bridge(this._world, new Vector2(815+96, Window.ClientBounds.Height - 200-64+10), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 1].body, this._world);
                    bridgeTemp.ClipToLast(_bkgList[_bkgList.Count - 3].body, this._world);
                    _bkgList.Add(bridgeTemp);
                    //////////////////////
                    // Grundpfeiler 1
                    bridgeTemp = new Bridge(this._world, new Vector2(2302.5f, Window.ClientBounds.Height-158), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;
                    this._bkgList.Add(bridgeTemp);

                    bridgeTemp = new Bridge(this._world, new Vector2(2302.5f+256, Window.ClientBounds.Height - 158), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Static;
                    this._bkgList.Add(bridgeTemp);

                    // 1
                    bridgeTemp = new Bridge(this._world, new Vector2(2302.5f + 32, Window.ClientBounds.Height - 158), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 2].body, this._world);
                    _bkgList.Add(bridgeTemp);
                    // 2
                    bridgeTemp = new Bridge(this._world, new Vector2(2302.5f + 64, Window.ClientBounds.Height - 158), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 1].body, this._world);
                    _bkgList.Add(bridgeTemp);
                    // 3
                    bridgeTemp = new Bridge(this._world, new Vector2(2302.5f + 96, Window.ClientBounds.Height - 158), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 1].body, this._world);
                    _bkgList.Add(bridgeTemp);
                    // 4
                    bridgeTemp = new Bridge(this._world, new Vector2(2302.5f + 128, Window.ClientBounds.Height - 158), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 1].body, this._world);
                    _bkgList.Add(bridgeTemp);
                    // 5
                    bridgeTemp = new Bridge(this._world, new Vector2(2302.5f + 160, Window.ClientBounds.Height - 158), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 1].body, this._world);
                    _bkgList.Add(bridgeTemp);
                    // 6
                    bridgeTemp = new Bridge(this._world, new Vector2(2302.5f + 192, Window.ClientBounds.Height - 158), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 1].body, this._world);
                    _bkgList.Add(bridgeTemp);
                    // 7
                    bridgeTemp = new Bridge(this._world, new Vector2(2302.5f + 224, Window.ClientBounds.Height - 158), 32, 32, this._textureTileset);
                    bridgeTemp.loadTileset(0);
                    bridgeTemp.BodyType = BodyType.Dynamic;
                    bridgeTemp.ClipToNext(_bkgList[_bkgList.Count - 1].body, this._world);
                    bridgeTemp.ClipToLast(_bkgList[_bkgList.Count - 7].body, this._world);
                    _bkgList.Add(bridgeTemp);
                    #endregion

                    //Brick
                    //
                    #region Brick
                    #endregion

                    //// ItemObjects
                    //Coin
                    #region Coin
                    coinTemp = new Coin(this._world, new Vector2(32, Window.ClientBounds.Height - 256 - 64-35), 8, 24, 1f, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    
                    coinTemp = new Coin(this._world, new Vector2(64, Window.ClientBounds.Height - 256 - 64-35), 8, 24, 1f, this._textureCoin);
                    this._itemList.Add(coinTemp);

                    coinTemp = new Coin(this._world, new Vector2(96, Window.ClientBounds.Height - 256 - 64-35), 8, 24, 1f, this._textureCoin);
                    this._itemList.Add(coinTemp);
                    #endregion

                    //// TrackObjects
                    #region Track
                    podTrackTemp = new Track_1(0, Content, _world, this._textureTileset, 0.0008f, new Vector2(1024, 128));
                    _trackList.Add(podTrackTemp);

                    podTrackTemp = new Track_1(0, Content, _world, this._textureTileset, 0.0018f, new Vector2(1024+512, 128));
                    _trackList.Add(podTrackTemp);

                    podTrackTemp = new Track_1(0, Content, _world, this._textureTileset, 0.0018f, new Vector2(2790.5f + 196, Window.ClientBounds.Height - 300));
                    _trackList.Add(podTrackTemp);

                    podTrackTemp = new Track_1(0, Content, _world, this._textureTileset, 0.0018f, new Vector2(2790.5f + 512 + 196 + 196, Window.ClientBounds.Height - 300));
                    _trackList.Add(podTrackTemp);

                    #endregion

                    //// TextureObjects
                    // Water
                    #region Water
                    waterTemp = new Water(new Vector2(1024, Window.ClientBounds.Height - 64), 1024, 64, this._textureTileset);
                    waterTemp.loadTiles(0);
                    _txtList.Add(waterTemp);
                    #endregion

                    //// EnemyObjects
                    #region EnemyObjects
                    // Goomba
                    goombaTemp = new Goomba(this._textureGoomba, _world, new Vector2(64, Window.ClientBounds.Height-380));
                    goombaTemp.MoveSpeed = 0.01f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(32, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, _world, new Vector2(288, Window.ClientBounds.Height - 380));
                    goombaTemp.MoveSpeed = 0.008f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(48, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, _world, new Vector2(288+1755, Window.ClientBounds.Height - 280));
                    goombaTemp.MoveSpeed = 0.008f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(32, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, _world, new Vector2(288+1755+128+64, Window.ClientBounds.Height - 175));
                    goombaTemp.MoveSpeed = 0.003f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(32, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);

                    goombaTemp = new Goomba(this._textureGoomba, _world, new Vector2(2611, Window.ClientBounds.Height - 175));
                    goombaTemp.MoveSpeed = 0.01f;
                    goombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(32, goombaTemp.Body);
                    this._enemyList.Add(goombaTemp);
                    // Spiked Goomba
                    //spikedGoombaTemp = new SpikedGoomba(this._textureSpikedGoomba, _world, new Vector2(2611+96+64+16, Window.ClientBounds.Height - 280));
                    //spikedGoombaTemp.MoveSpeed = 0.008f;
                    //spikedGoombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(32, spikedGoombaTemp.Body);
                    //this._enemyList.Add(spikedGoombaTemp);

                    //this.Camera._pos = new Vector2(288 + 1024+500, Window.ClientBounds.Height/2);
                    spikedGoombaTemp = new SpikedGoomba(this._textureSpikedGoomba, _world, new Vector2(288 + 96 + 96+64+16, Window.ClientBounds.Height - 380));
                    spikedGoombaTemp.MoveSpeed = 0.008f;
                    spikedGoombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(32, spikedGoombaTemp.Body);
                    this._enemyList.Add(spikedGoombaTemp);
                    // Flying Spiked Goomba
                    // Fuzzy
                    #region Fuzzy
                    fuzzyTemp = new Fuzzy(this._textureFuzzy, this._world, FuzzyColor.Black, new Vector2(1440, 32));
                    fuzzyTemp.MoveSpeed = 0.005f;
                    fuzzyTemp.MovingPath = PathGenerator.CreatePathOnYAxis(256, fuzzyTemp.Body, true);
                    this._enemyList.Add(fuzzyTemp);

                    fuzzyTemp = new Fuzzy(this._textureFuzzy, this._world, FuzzyColor.Black, new Vector2(1400, 288));
                    fuzzyTemp.MoveSpeed = 0.005f;
                    fuzzyTemp.MovingPath = PathGenerator.CreatePathOnYAxis(256, fuzzyTemp.Body, false);
                    this._enemyList.Add(fuzzyTemp);

                    fuzzyTemp = new Fuzzy(this._textureFuzzy, this._world, FuzzyColor.Black, new Vector2(2790.5f+256+32, Window.ClientBounds.Height - 332));
                    fuzzyTemp.MoveSpeed = 0.005f;
                    fuzzyTemp.MovingPath = PathGenerator.CreatePathOnXAxis(256+32, fuzzyTemp.Body);
                    this._enemyList.Add(fuzzyTemp);

                    fuzzyTemp = new Fuzzy(this._textureFuzzy, this._world, FuzzyColor.Black, new Vector2(2790.5f + 512+128+8, Window.ClientBounds.Height-320+32));
                    fuzzyTemp.MoveSpeed = 0.005f;
                    fuzzyTemp.MovingPath = PathGenerator.CreateRectanglePath(116, fuzzyTemp.Body, 112, false);
                    this._enemyList.Add(fuzzyTemp);

                    fuzzyTemp = new Fuzzy(this._textureFuzzy, this._world, FuzzyColor.Black, new Vector2(3846.5f, Window.ClientBounds.Height - 332));
                    fuzzyTemp.MoveSpeed = 0.005f;
                    fuzzyTemp.MovingPath = PathGenerator.CreatePathOnXAxis(256+32, fuzzyTemp.Body);
                    this._enemyList.Add(fuzzyTemp);

                    #endregion

                    #endregion

                    //// TextureObjects
                    #region TextureObjects
                    // Fence
                    fenceTemp = new Fence(new Vector2(16, Window.ClientBounds.Height - Window.ClientBounds.Height+84), 96, 32, this._textureTileset);
                    fenceTemp.loadTiles(0);
                    _txtList.Add(fenceTemp);
                    
                    fenceTemp = new Fence(new Vector2(242, Window.ClientBounds.Height - Window.ClientBounds.Height+84), 96, 32, this._textureTileset);
                    fenceTemp.loadTiles(0);
                    _txtList.Add(fenceTemp);
                    #endregion
                    #endregion
                    break;

                case 2: // TODO Mapdesign
                    #region Level 3
                    ////// Temporär
                    //
                    //// BKG Objects
                    //
                    // Ground
                    #region Ground
                    groundTemp = new Ground(this._world, new Vector2(136, Window.ClientBounds.Height - 48 - 16), 256, 88, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(648, Window.ClientBounds.Height -320), 96, 64, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    groundTemp.HasBottom = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(776, Window.ClientBounds.Height -288), 96, 64, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    groundTemp.HasBottom = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(904, Window.ClientBounds.Height -256), 96, 64, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    groundTemp.HasBottom = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(1048+256+96+64, Window.ClientBounds.Height - 240), 96, 64, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    groundTemp.HasBottom = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(1560, Window.ClientBounds.Height - 64), 96, 88, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    groundTemp.HasBottom = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(1784, Window.ClientBounds.Height - 196), 96, 356, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasLeftDown = true;
                    groundTemp.HasEnd = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(1784+320+16+31, Window.ClientBounds.Height - 64), 608, 88, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(1784+640+46, Window.ClientBounds.Height - 196), 96, 356, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasRightDown = true;
                    this._bkgList.Add(groundTemp);
                    #endregion

                    // Bridge
                    //

                    #region EnemyObject
                    flyingGoombaTemp = new FlyingGoomba(this._textureFlyingGoomba, this._world, new Vector2(64 + 256, Window.ClientBounds.Height - 230));
                    //flyingGoombaTemp.FrameSpeed = ;
                    flyingGoombaTemp.MoveSpeed = 0.0015f;
                    flyingGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(96, flyingGoombaTemp.Body, false);
                    _enemyList.Add(flyingGoombaTemp);
                    
                    flyingGoombaTemp = new FlyingGoomba(this._textureFlyingGoomba, this._world, new Vector2(64 + 256+96, Window.ClientBounds.Height - 230));
                    //flyingGoombaTemp.FrameSpeed = ;
                    flyingGoombaTemp.MoveSpeed = 0.0010f;
                    flyingGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(128, flyingGoombaTemp.Body, false);
                    _enemyList.Add(flyingGoombaTemp);

                    flyingGoombaTemp = new FlyingGoomba(this._textureFlyingGoomba, this._world, new Vector2(512, Window.ClientBounds.Height - 230));
                    //flyingGoombaTemp.FrameSpeed = ;
                    flyingGoombaTemp.MoveSpeed = 0.0007f;
                    flyingGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(128, flyingGoombaTemp.Body, false);
                    _enemyList.Add(flyingGoombaTemp);

                    fuzzyTemp = new Fuzzy(this._textureFuzzy, this._world, FuzzyColor.Black, new Vector2(648-48, Window.ClientBounds.Height - 320-46));
                    //fuzzyTemp.FrameSpeed = ;
                    fuzzyTemp.MoveSpeed = 0.005f;
                    fuzzyTemp.MovingPath = PathGenerator.CreateRectanglePath(96, fuzzyTemp.Body, 96, false);
                    _enemyList.Add(fuzzyTemp);

                    fuzzyTemp = new Fuzzy(this._textureFuzzy, this._world, FuzzyColor.Black, new Vector2(648-48+128, Window.ClientBounds.Height - 320-16));
                    //fuzzyTemp.FrameSpeed = ;
                    fuzzyTemp.MoveSpeed = 0.005f;
                    fuzzyTemp.MovingPath = PathGenerator.CreateRectanglePath(96, fuzzyTemp.Body, 96, false);
                    _enemyList.Add(fuzzyTemp);
                    
                    fuzzyTemp = new Fuzzy(this._textureFuzzy, this._world, FuzzyColor.Black, new Vector2(856, Window.ClientBounds.Height - 320+16));
                    //fuzzyTemp.FrameSpeed = ;
                    fuzzyTemp.MoveSpeed = 0.005f;
                    fuzzyTemp.MovingPath = PathGenerator.CreateRectanglePath(96, fuzzyTemp.Body, 96, false);
                    _enemyList.Add(fuzzyTemp);

                    flyingGoombaTemp = new FlyingGoomba(this._textureFlyingGoomba, this._world, new Vector2(1048, Window.ClientBounds.Height - 230));
                    //flyingGoombaTemp.FrameSpeed = ;
                    flyingGoombaTemp.MoveSpeed = 0.0010f;
                    flyingGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(128, flyingGoombaTemp.Body, false);
                    _enemyList.Add(flyingGoombaTemp);

                    flyingSpikedGoombaTemp = new FlyingSpikedGoomba(this._textureFlyingGoomba, this._world, new Vector2(1048+64, Window.ClientBounds.Height - 230));
                    //flyingGoombaTemp.FrameSpeed = ;
                    flyingSpikedGoombaTemp.MoveSpeed = 0.0010f;
                    flyingSpikedGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(128, flyingSpikedGoombaTemp.Body, false);
                    _enemyList.Add(flyingSpikedGoombaTemp);

                    flyingGoombaTemp = new FlyingGoomba(this._textureFlyingGoomba, this._world, new Vector2(1048+128+16, Window.ClientBounds.Height - 230));
                    //flyingGoombaTemp.FrameSpeed = ;
                    flyingGoombaTemp.MoveSpeed = 0.0010f;
                    flyingGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(128, flyingGoombaTemp.Body, false);
                    _enemyList.Add(flyingGoombaTemp);

                    flyingSpikedGoombaTemp = new FlyingSpikedGoomba(this._textureFlyingGoomba, this._world, new Vector2(1048+128+96, Window.ClientBounds.Height - 230));
                    //flyingGoombaTemp.FrameSpeed = ;
                    flyingSpikedGoombaTemp.MoveSpeed = 0.0010f;
                    flyingSpikedGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(128, flyingSpikedGoombaTemp.Body, false);
                    _enemyList.Add(flyingSpikedGoombaTemp);
                    
                    flyingGoombaTemp = new FlyingGoomba(this._textureFlyingGoomba, this._world, new Vector2(1048+128+96+64, Window.ClientBounds.Height - 230));
                    //flyingGoombaTemp.FrameSpeed = ;
                    flyingGoombaTemp.MoveSpeed = 0.0010f;
                    flyingGoombaTemp.MovingPath = PathGenerator.CreatePathOnYAxis(128, flyingGoombaTemp.Body, false);
                    _enemyList.Add(flyingGoombaTemp);

                    giantGoombaTemp = new GiantGoomba(this._textureGiantGoomba, this._world, new Vector2(2100, Window.ClientBounds.Height-198));
                    giantGoombaTemp.MoveSpeed = 0.0010f;
                    giantGoombaTemp.MovingPath = PathGenerator.CreatePathOnXAxis(200, giantGoombaTemp.Body);
                    _enemyList.Add(giantGoombaTemp);
                    //this.Camera.Pos = new Vector2(2200, this.Camera.Pos.Y);

                    #endregion

                    #region Spring
                    springTemp = new Spring(this._world, new Vector2(1560, Window.ClientBounds.Height-64-56), 32, 32, 1f, this._textureTileset);
                    springTemp.loadTiles(0);
                    _actionList.Add(springTemp);
                    #endregion
                    #endregion
                    break;
                case 3:
                    #region Level 4
                    //// BKG Objects
                    #region Ground
                    
                    groundTemp = new Ground(this._world, new Vector2(136, Window.ClientBounds.Height - 128+32), 256, 160, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    this._bkgList.Add(groundTemp);
                    
                    groundTemp = new Ground(this._world, new Vector2(136, Window.ClientBounds.Height - Window.ClientBounds.Height), 256, 160, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    groundTemp.HasBottom = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(136+256+128, Window.ClientBounds.Height - 128+32), 256, 160, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    this._bkgList.Add(groundTemp);

                    groundTemp = new Ground(this._world, new Vector2(136+256+128, Window.ClientBounds.Height - Window.ClientBounds.Height), 256, 160, this._textureTileset);
                    groundTemp.loadTileset(0);
                    groundTemp.HasBegin = true;
                    groundTemp.HasEnd = true;
                    groundTemp.HasBottom = true;
                    this._bkgList.Add(groundTemp);

                    #endregion

                    #region Brick

                    #endregion

                    #region TextureObject
                    fenceTemp = new Fence(new Vector2(24, Window.ClientBounds.Height-140), 192, 32, this._textureTileset);
                    fenceTemp.loadTiles(0);
                    _txtList.Add(fenceTemp);
                    #endregion

                    #region TrackObject
                    #endregion

                    //// EnemyObjects
                    #region EnemyObject
                    chompyTemp = new Chompy(this._textureChompy, this._world, new Vector2(565, Window.ClientBounds.Height+120));
                    chompyTemp.MoveSpeed = 0.005f;
                    chompyTemp.FrameSpeed = 200;
                    chompyTemp.MovingPath =PathGenerator.CreateSwingingAndReturnPath(300, -420, chompyTemp.Body, true);

                    this._enemyList.Add(chompyTemp);
                    
                    #endregion

                    #region ActionObjects
                    //springTemp = new Spring(this._world, new Vector2(1384, Window.ClientBounds.Height - 184), 32, 32, 1f, this._textureTileset);
                    //springTemp.loadTiles(0);
                    //this._actionList.Add(springTemp);
                    #endregion
                    #endregion
                    break;
            }

            // Temporäre Identifikatoren entfernen
            // Hinweis: Die Objekte können nicht geschmissen (Disposed) werden, da sonst die Physikalischen Eigenschaften nicht mehr gegeben sind
            pipeTemp = null;
            groundTemp = null;
            bridgeTemp = null;
            fenceTemp = null;
            goombaTemp = null;
            brickTemp = null;
            coinTemp = null;
            springTemp = null;
            spikedGoombaTemp = null;
            waterTemp = null;
            liveTemp = null;
            groundbevelTemp = null;
            podTrackTemp = null;
            flyingGoombaTemp = null;
            flyingSpikedGoombaTemp = null;
            chompyTemp = null;
            giantGoombaTemp = null;
        }

        public void drawStage(SpriteBatch spriteBatch, GameWindow Window) {
            if (!this._isDisposed)
            {
                _background.Draw(spriteBatch);

                spriteBatch.DrawString(this._font, this._headText, new Vector2((int)this.Camera.Pos.X, 5), Color.Black);
             
                foreach (TextureObject txt in this._txtList)
                    txt.Draw(spriteBatch);
                
                foreach (BKGObject bo in this._bkgList)
                    bo.Draw(spriteBatch);

                foreach (ActionObject ao in this._actionList)
                    ao.Draw(spriteBatch);

                foreach (ItemObject io in this._itemList)
                    io.Draw(spriteBatch);

                foreach (EnemyObject eo in this._enemyList)
                    eo.Draw(spriteBatch);

                foreach (TrackObject to in this._trackList)
                    to.Draw(spriteBatch);

                
                switch(this._lvlID){
                    case 2:
                    case 3:
                        spriteBatch.End();
                        spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, null, null, null, null, this.Camera.get_transformation(this._graphics));
                        //spriteBatch.Begin();

                        this._renderer.RenderEffect(this._particleEffect);
                        break;
                }
            }
            
        }

        public void updateStage(GameTime gameTime, GameWindow Window) {

            if (Player.LevelFinished)
            {
                this.killStage(0);
            }

            if (!this.StageIsDisposed)
            {

                foreach (TextureObject to in this._txtList)
                    to.Update(gameTime);

                foreach (ItemObject io in this._itemList)
                {
                    if (!io.IsAvailable)
                    {
                        io.Kill();
                        this._itemList.Remove(io);
                        if (io.PlayerGet == Item.Score)
                            this._coinCount.Score += 10;
                        else if (io.PlayerGet == Item.Live)
                            if (Player.PlayerLive <= 7)
                                this.Player.PlayerLive++;
                        break;
                    }
                    else
                        io.Update(gameTime);
                }
                
                foreach (ActionObject ao in this._actionList)
                    ao.Update(gameTime);

                foreach(EnemyObject eo in this._enemyList){

                    if (eo != null)
                        eo.Update(gameTime);

                    if (eo != null && eo.IsDisposeable)
                    {
                        if (eo.GetType().ToString() == "MarioME.EnemyObjects.GiantGoomba")
                            Player.LevelFinished = true;
                        
                        eo.Kill();
                        this._enemyList.Remove(eo);
                        
                        break;
                    }
                }
                
                //cameraEffect.ShakeCamera(this.Camera, 0.25f, 26);

                foreach (TrackObject to in this._trackList)
                    to.Update(gameTime);

                // Paricle Effect
                switch (this._lvlID) { 
                    case 2:
                    case 3:
                        this._particleEffect.Trigger(new Vector2(0, 0));
                        _particleEffect.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
                        break;
                }
                // Überraschungsmoment Level 2 Giant Goomba
                /*if (this._lvlID == 2)
                {
                    if (this.Player.Position.X >= ConvertUnits.ToSimUnits(256) && giantGoombaTemp == null)
                    {
                        giantGoombaTemp = new GiantGoomba(this._textureGiantGoomba, this._world, new Vector2(256, -128));
                        giantGoombaTemp.MoveSpeed = 0.01f;
                        this._enemyList.Add(giantGoombaTemp);

                        giantGoombaTemp.Activate = true;
                    }
                        
                }*/
                
            }
        }

        public bool killStage(int lvlID) {
            if (!this._isDisposed)
            {
                
                /* Hinweis: Darf Textur nicht schmeißen, da sonst Exception*/
                this._textureTileset = null; //.Dispose();
                
                foreach (BKGObject bo in this._bkgList) {
                    bo.Kill();
                }
                this._bkgList.Clear();

                foreach (EnemyObject eo in this._enemyList) {
                    eo.Kill();
                }
                this._enemyList.Clear();

                this._txtList.Clear();
                this._itemList.Clear();
                
                foreach(TrackObject to in this._trackList)
                    to.Kill();
                this._trackList.Clear();

                this._bkgList = null;
                this._txtList = null;
                this._itemList = null;
                this._enemyList = null;
                this._trackList = null;

                this._background.Kill();
                this._background = null;

                this._textureGoomba = null;
                this._textureTileset = null;
                this._textureKoopa = null;
                this._textureCoin = null;
                this._textureItems = null;
                this._textureFuzzy = null;
                this._textureChompy = null;

                this._world.BodyList.Clear();

                this._particleEffect = null;
                this._renderer.Dispose();

                /*
                 * Hinweis: Falls Stage-Spezifische Inhalte gelöst werden müssen
                switch (lvlID)
                {
                    case 0:
                        this._isDisposed = true;
                        return this._isDisposed;
                        break;
                }
                */
                this._isDisposed = true;
                return true;
            }
            return false;
        }

        public bool StageIsDisposed {
            get { return this._isDisposed; }
        }
    }
}