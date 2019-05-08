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
    class FlyingGoomba: EnemyObject
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

        public FlyingGoomba(Texture2D texture, World world, Vector2 position) : base(texture, world, position) {
            this._maxFrames = 8;
            this._rect = new Rectangle[this._maxFrames];

            this._width = 22;
            this._height = 17;

            this._spriteEffect = SpriteEffects.FlipHorizontally;

            this.loadTiles();
            this.InitPhysic(world, position);
        }

        protected override void InitPhysic(World world, Vector2 position)
        {
            base.InitPhysic(world, position);
            this._body.OnCollision += new OnCollisionEventHandler(_body_OnCollision);
        }

        public override void Update(GameTime gameTime)
        {
            if (this._isDead && !this._body.IsDisposed)
            {
                this._disposeTimer.Enabled = true;

                this._path = new Path();
                this._path.Add(new Vector2(this._body.Position.X, ConvertUnits.ToSimUnits(480)));
                this._path.Closed = true;

                this._body.OnCollision -= new OnCollisionEventHandler(this._body_OnCollision);

                if (this._body.Position.Y >= ConvertUnits.ToSimUnits(480))
                    this._body.Dispose();
                
            }

            if (this._live <= 0)
                this._isDead = true;
            base.Update(gameTime);
        }

        bool _body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            // fixtureB.Body.Position.Y < this._body.Position.Y - ConvertUnits.ToSimUnits(this._height / 2) && 
            if (fixtureB.Body.Position.Y < this._body.Position.Y - ConvertUnits.ToSimUnits(this._height / 2) && fixtureB.CollisionCategories == Category.Cat3)
            {
                //fixtureB.Body.ApplyForce(new Vector2(0f, ConvertUnits.ToDisplayUnits(-3f)));
                //fixtureA.Body.ApplyForce(new Vector2(0f, -3f));
                this._live--;
                //this._isDead = true;
            }

            return true;
        }

        void loadTiles() {
            this._rect[0] = new Rectangle(0, 0, 30, 18);
            this._rect[1] = new Rectangle(31, 0, 32, 18);
            this._rect[2] = new Rectangle(64, 0, 32, 18);
            this._rect[3] = new Rectangle(97, 0, 32, 18);
            this._rect[4] = new Rectangle(130, 0, 32, 18);
            this._rect[5] = new Rectangle(163, 0, 32, 18);
            this._rect[6] = new Rectangle(196, 0, 32, 18);
            this._rect[7] = new Rectangle(229, 0, 32, 18);
        }
    }
}
