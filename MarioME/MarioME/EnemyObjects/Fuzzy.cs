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
    enum FuzzyColor{
        Black= 0,
        Yellow= 1,
        Green= 2
    }

    class Fuzzy: EnemyObject
    {
        public Fuzzy(Texture2D texture, World world, FuzzyColor color, Vector2 position)
            : base(texture, world, position)
        {
            
            this._width = 26;
            this._height = 24;

            this._maxFrames = 4;
            this._rect = new Rectangle[_maxFrames];

            this.loadTiles(color);

            this.InitPhysic(world, position);
        }

        protected override void InitPhysic(World world, Vector2 position)
        {
            base.InitPhysic(world, position);
            this._body.CollisionCategories = Category.Cat10;
            this._body.OnCollision += new OnCollisionEventHandler(_body_OnCollision);
        }

        bool _body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            //this._body.IgnoreCollisionWith(fixtureB.Body);
            return false;
        }

        void loadTiles(FuzzyColor color) {
            switch (color) { 
                case FuzzyColor.Black:
                    this._rect[0] = new Rectangle(2, 2, 30, 28);
                    this._rect[1] = new Rectangle(32, 2, 30, 28);
                    this._rect[2] = new Rectangle(62, 2, 30, 28);
                    this._rect[3] = new Rectangle(92, 2, 30, 28);
                    break;

                case FuzzyColor.Yellow:
                    this._rect[0] = new Rectangle(0, 32, 30, 28);
                    this._rect[1] = new Rectangle(32, 32, 30, 28);
                    this._rect[2] = new Rectangle(62, 32, 30, 28);
                    this._rect[3] = new Rectangle(92, 32, 30, 28);
                    break;

                case FuzzyColor.Green:
                    this._rect[0] = new Rectangle(2, 64, 30, 28);
                    this._rect[1] = new Rectangle(32, 64, 30, 28);
                    this._rect[2] = new Rectangle(62, 64, 30, 28);
                    this._rect[3] = new Rectangle(92, 64, 30, 28);
                    break;
            }
        }
    }
}
