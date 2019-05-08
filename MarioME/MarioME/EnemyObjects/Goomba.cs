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
    class Goomba: EnemyObject
    {
        /*
         * Hinweis:
         * Einzustellen sind:
         * X - Body (_body)
         * X - Rectangle-Array (_rect)
         * X - Maximale anzahl an Frames (_maxFrames)
         * - "Schwachpunkt" (_rectHittable)
         * X - Breite des Body (_width)
         * X - Höhe des Body (_height)
         * X - InitPhysic muss in der jeweiligen Klasse gestartet werden
         */ 

        public Goomba(Texture2D texture, World world, Vector2 position) : base(texture, world, position) {
            this._maxFrames = 7;
            this._rect = new Rectangle[this._maxFrames+1]; // +1 für Dead-Frame

            this._width = 20;
            this._height = 20;

            //this._rectHittable = new Rectangle(this.);

            this.loadTiles();
            this.InitPhysic(world, position);
        }

        void loadTiles() { 
            // Walking
            this._rect[0] = new Rectangle(1, 41, 17, 20);
            this._rect[1] = new Rectangle(41, 41, 18, 19);
            this._rect[2] = new Rectangle(80, 42, 20, 18);
            this._rect[3] = new Rectangle(121, 41, 18, 19);
            this._rect[4] = new Rectangle(161, 41, 18, 20);
            this._rect[5] = new Rectangle(202, 41, 16, 19);
            this._rect[6] = new Rectangle(2, 2, 16, 18);
            // Dead
            this._rect[7] = new Rectangle(77, 81, 25, 12);
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
                this._frame = 7;
                this._disposeTimer.Enabled = true;

                this._body.OnCollision -= new OnCollisionEventHandler(this._body_OnCollision);
                this._body.Dispose();
                
            }

            if (this._live <= 0)
                this._isDead = true;
            base.Update(gameTime);
        }

        bool _body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.Body.Position.Y < this._body.Position.Y - ConvertUnits.ToSimUnits(this._height / 2) && fixtureB.CollisionCategories == Category.Cat3)
            {
                //fixtureB.Body.ApplyForce(new Vector2(0f, ConvertUnits.ToDisplayUnits(-3f)));
                //fixtureA.Body.ApplyForce(new Vector2(0f, -3f));
                this._live--;
                //this._isDead = true;
            }

            return true;
        }

        public override void Kill()
        {
            base.Kill();
        }
    }
}
