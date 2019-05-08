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

namespace MarioME.ActionObjects
{
    class Spring: ActionObject
    {
        int _frame;
        bool _animationStart;
        Rectangle[] rect;
        public Spring(World world, Vector2 position, float width, float height, float mass, Texture2D texture) : base(world, position, width, height, mass, texture) {
            rect = new Rectangle[6];
            _animationStart = false;
            
            this.body.BodyType = BodyType.Static;
            this.body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.Body.Position.Y < fixtureA.Body.Position.Y) {
                _animationStart = true;
                fixtureB.Body.ApplyLinearImpulse(new Vector2(0f, ConvertUnits.ToDisplayUnits(-0.18f)));
            }

            return true;
        }
        
        public void loadTiles(int textureID) {
            
            switch (textureID) { 
                case 0:
                    rect[0] = new Rectangle(320, 119, 32, 18);
                    rect[1] = new Rectangle(358, 105, 32, 32);
                    rect[2] = new Rectangle(398, 93, 32, 44);
                    rect[3] = new Rectangle(439, 73, 32, 64);
                    rect[4] = new Rectangle(398, 93, 32, 44);
                    rect[5] = new Rectangle(358, 105, 32, 32);
                    break;
            }
        }

        public override void Update(GameTime gameTime) {
            if (_animationStart) {
                if (Math.Floor(gameTime.TotalGameTime.TotalMilliseconds) % 10 == 0) {
                    _frame++;
                    if (_frame == 6) {
                        _frame = 0;
                        _animationStart = false;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            int bdyPosX = (int)ConvertUnits.ToDisplayUnits(this.body.Position.X);
            int bdyPosY = (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y);
            spriteBatch.Draw(texture, new Rectangle(bdyPosX + (int)(this.width), bdyPosY +16, this.rect[this._frame].Width, this.rect[this._frame].Height), this.rect[this._frame], Color.White, 0f, new Vector2(this.rect[this._frame].Width, this.rect[this._frame].Height), SpriteEffects.None, 0f);
        }
    }
}
