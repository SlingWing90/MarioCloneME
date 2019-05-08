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

using MarioME.BKGObjects;
using MarioME.TextureObjects;


namespace MarioME.EnemyObjects
{
    class EnemyObject
    {
        /*
         * Hinweis:
         * Einzustellen sind:
         * - Body (_body)
         * - Rectangle-Array (_rect)
         * - Maximale anzahl an Frames (_maxFrames)
         * - "Schwachpunkt" (_rectHittable)
         * - Breite des Body (_width)
         * - Höhe des Body (_height)
         * 
         * - InitPhysic muss in der jeweiligen Klasse gestartet werden
         */ 
        protected Body _body;
        Texture2D _texture;

        protected int _width;
        protected int _height;

        protected float _density;

        protected int _frame;
        int _frameSpeed;
        protected int _maxFrames;

        bool _isDisposeable;

        protected Rectangle[] _rect;
        protected Rectangle _rectHittable;
        Rectangle _rectDrawing;

        protected SpriteEffects _spriteEffect;

        protected Path _path;

        float _time;
        float _moveSpeed;

        float _lastPositionX;

        protected bool _isDead;

        protected Timer _disposeTimer;
        int _disposeCount = 0;

        float _textureRotation;

        protected int _live;

        public EnemyObject(Texture2D texture, World world, Vector2 position)
        { 
            this._texture = texture;
            this._rectDrawing = new Rectangle(0, 0, 0, 0);
            this._frame = 0;
            this._frameSpeed = 100;

            this._isDisposeable = false;
            this._isDead = false;

            this._lastPositionX = position.X;

            this._time = 0;
            this._moveSpeed = 0.001f;
            //this._spriteEffect = SpriteEffects.FlipVertically;
            this._density = 1f;
            this._textureRotation = 0f;
            this._live = 1;
            this._disposeTimer = new Timer();
            this._disposeTimer.Tick += new EventHandler(_disposeTimer_Tick);
            this._disposeTimer.Interval = 1000;
            this._disposeTimer.Enabled = false;
        }

        void _disposeTimer_Tick(object sender, EventArgs e)
        {
            this._disposeCount++;

            if (this._disposeCount >= 3)
                this._isDisposeable = true;
        }

        protected virtual void InitPhysic(World world, Vector2 position){
            this._body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(this._width), ConvertUnits.ToSimUnits(this._height), this._density);
            this._body.CollisionCategories = Category.Cat4;
            this._body.Position = ConvertUnits.ToSimUnits(position);
            this._body.BodyType = BodyType.Dynamic;
            this._body.OnCollision += new OnCollisionEventHandler(_body_OnCollision);
        }

        bool _body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.CollisionCategories == Category.Cat4 || fixtureB.CollisionCategories == Category.Cat5)
                this._body.IgnoreCollisionWith(fixtureB.Body);
            return true;
            //throw new NotImplementedException();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            int bdyPosX = (int)ConvertUnits.ToDisplayUnits(this._body.Position.X);
            int bdyPosY = (int)ConvertUnits.ToDisplayUnits(this._body.Position.Y);
            spriteBatch.Draw(this._texture, new Rectangle(bdyPosX + this._rectDrawing.Width / 2, bdyPosY + this._rectDrawing.Height / 2 + 8, this._rectDrawing.Width, this._rectDrawing.Height), this._rectDrawing, Color.White, this._textureRotation, new Vector2((float)this._rectDrawing.Width, (float)this._rectDrawing.Height), this._spriteEffect, 0f);
        }

        public virtual void Update(GameTime gameTime) {
            if (Math.Floor(gameTime.TotalGameTime.TotalMilliseconds) % this._frameSpeed == 0 && !this._isDead)
            {
                    this._frame++;
                    if (this._frame >= _maxFrames)
                    this._frame = 0;
            }

            this._time += this._moveSpeed;

            if (this._time > 1)
                this._time = 0;

            if (ConvertUnits.ToDisplayUnits(this._body.Position.X) < this._lastPositionX)
            {
                this._spriteEffect = SpriteEffects.FlipHorizontally;
                this._lastPositionX = ConvertUnits.ToDisplayUnits(this._body.Position.X);
            }
            else if (ConvertUnits.ToDisplayUnits(this._body.Position.X) > this._lastPositionX)
            {
                this._spriteEffect = SpriteEffects.None;
                this._lastPositionX = ConvertUnits.ToDisplayUnits(this._body.Position.X);
            }
            else if(ConvertUnits.ToDisplayUnits(this._body.Position.X) == this._lastPositionX)
                this._spriteEffect = this._spriteEffect;

            this._rectDrawing = this._rect[this._frame];

            if(!this._isDead && this._path != null)
                PathManager.MoveBodyOnPath(this._path, this._body, this._time, 1f, 1f / 60f);
        }

        public virtual void Kill() {
            this._texture = null;
            this._disposeTimer.Enabled = false;
            this._disposeTimer.Tick -= new EventHandler(this._disposeTimer_Tick);
            this._disposeTimer.Dispose();
            
        }

        public Body Body {
            get { return this._body; }
        }

        /// <summary>
        /// Geschwindigkeit der Animation z.B 150f
        /// </summary>
        public int FrameSpeed {
            get { return this._frameSpeed; }
            set { this._frameSpeed = value; }
        }

        public bool IsDisposeable {
            get { return this._isDisposeable; }
            set { this._isDisposeable = value; }
        }

        public Path MovingPath {
            get { return this._path; }
            set { this._path = value; }
        }

        /// <summary>
        /// Geschwindigkeit der Bewegung z.B 0.001f
        /// </summary>
        public float MoveSpeed {
            get { return this._moveSpeed; }
            set { this._moveSpeed = value; }
        }

        public float Density {
            get { return this._density; }
            set { this._density = value; }
        }

        public float TextureRotation {
            get { return this._textureRotation; }
            set { this._textureRotation = value; }
        }
        /*
        public SpriteEffects spriteEffect {
            get { return this._spriteEffect; }
            set { this._spriteEffect = value; }
        }*/
    }
}
