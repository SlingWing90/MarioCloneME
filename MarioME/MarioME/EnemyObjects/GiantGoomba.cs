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
    class GiantGoomba:EnemyObject
    {
        bool _isActivated;
        bool _isLevelFinisher;

        public GiantGoomba(Texture2D texture, World world, Vector2 position) : base(texture, world, position) {
            this._isActivated = true;
            this._isLevelFinisher = false;
            this._maxFrames = 7;
            this._rect = new Rectangle[this._maxFrames+1]; //+1 für DeadFrame

            this._width = 135;
            this._height = 168;

            this._live = 8;

            this.loadTiles();
            //generatePath();
            this.InitPhysic(world, position);

            this._body.OnCollision += new OnCollisionEventHandler(_body_OnCollision);

            //if (this.IsLevelFinisher)
            //    this._body.CollisionCategories = Category.Cat9;
        }

        bool _body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.Body.Position.Y < this._body.Position.Y - ConvertUnits.ToSimUnits(this._height / 2) && fixtureB.CollisionCategories == Category.Cat3)
            {
                //fixtureB.Body.ApplyForce(new Vector2(0f, ConvertUnits.ToDisplayUnits(-3f)));
                //fixtureA.Body.ApplyForce(new Vector2(0f, -3f));
                this._live--;
            }

            if (this._isDead)
            {
                this._disposeTimer.Enabled = true;
                this._body.Dispose();
            }

            return true;
        }

        void loadTiles() { 
            this._rect[6] = new Rectangle(21, 312, 166, 189); // Standing
            this._rect[5] = new Rectangle(188, 312, 166, 189);
            this._rect[4] = new Rectangle(355, 312, 166, 189);
            this._rect[3] = new Rectangle(522, 312, 166, 189);
            this._rect[2] = new Rectangle(689, 312, 166, 189);
            this._rect[1] = new Rectangle(856, 312, 166, 189);
            this._rect[0] = new Rectangle(1023, 312, 166, 189);

            this._rect[7] = new Rectangle(200, 101, 166, 189);

        }
        /*
        void generatePath() {
            this._path = null;
            this._path = new Path();
            this._path.Add(new Vector2(ConvertUnits.ToSimUnits(256), ConvertUnits.ToSimUnits(-160)));
            this._path.Add(new Vector2(ConvertUnits.ToSimUnits(256), ConvertUnits.ToSimUnits(192)));
            this._path.Add(new Vector2(ConvertUnits.ToSimUnits(256 + 256), ConvertUnits.ToSimUnits(160 + 96)));
            this._path.Add(new Vector2(ConvertUnits.ToSimUnits(256 + 256+128), ConvertUnits.ToSimUnits(160 + 96)));
        }
        */
        public override void Update(GameTime gameTime)
        {

            if (this._live <= 0)
            {
                this._isDead = true;
                this._frame = 7;
            }


            base.Update(gameTime);
        }

        public bool Activate {
            get { return this._isActivated; }
            set { this._isActivated = value; }
        }

        public bool IsLevelFinisher {
            get { return this._isLevelFinisher; }
            set { this._isLevelFinisher = value; }
        }
    }
}
