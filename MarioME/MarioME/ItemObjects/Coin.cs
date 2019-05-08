using System;
using System.Collections.Generic;
using System.Linq;
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

namespace MarioME.ItemObjects
{
    class Coin:ItemObject
    {
        bool _coinAvailable;
        int[] _frame = new int[20];
        int _frameC;

        
        public Coin(World world, Vector2 position, float width, float height, float mass, Texture2D texture)
            : base(world, position, width, height, mass, texture)
        {
            //this.body.BodyType = BodyType.Static;
            //this.body.CollisionCategories = Category.Cat5;
            //this.body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            this._coinAvailable = true;
            this._playerGet = Item.Score;
            this._frameC = 0;
            loadTiles();
        }

        void loadTiles() { 
            //32 Breit 24 Hoch
            this._frame[0] = 0;
            this._frame[1] = 0;

            this._frame[2] = 32;
            this._frame[3] = 0;

            this._frame[4] = 64;
            this._frame[5] = 0;

            this._frame[6] = 96;
            this._frame[7] = 0;
            //
            this._frame[8] = 32;
            this._frame[9] = 48;

            this._frame[10] = 0;
            this._frame[11] = 48;
            //
            this._frame[12] = 96;
            this._frame[13] = 24;

            this._frame[14] = 64;
            this._frame[15] = 24;

            this._frame[16] = 32;
            this._frame[17] = 24;
            //
            this._frame[18] = 0;
            this._frame[19] = 24;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.CollisionCategories == Category.Cat3) {
                this.body.IgnoreCollisionWith(fixtureB.Body);
                //fixtureB.IgnoreCollisionWith(fixtureA.Body);
                this._coinAvailable = false;
            }
            
            return true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Math.Floor(gameTime.TotalGameTime.TotalMilliseconds % 20) == 0) {
                this._frameC+= 2;
                if (this._frameC >= 20)
                    this._frameC = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int bdyPosX = (int)ConvertUnits.ToDisplayUnits(this.body.Position.X);
            int bdyPosY = (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y);
            spriteBatch.Draw(texture, new Rectangle(bdyPosX - (int)(this.width / 2)+16, bdyPosY - (int)(this.height / 2), 32, 24), new Rectangle(this._frame[this._frameC], this._frame[this._frameC+1], 32, 24), Color.White, 0f, new Vector2(32, 24), SpriteEffects.None, 0f); 
        }

        public override void Kill() {
            this.body.OnCollision -= new OnCollisionEventHandler(body_OnCollision);
            this.body.Dispose();
            this.texture = null;
        }

        //public bool CoinAvailable {
        //    get { return this._coinAvailable; }
        // }
    }
}
