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
    /*
     float bdyX = ConvertUnits.ToDisplayUnits(this.Position.X);
            float bdyY = ConvertUnits.ToDisplayUnits(this.Position.Y);
            spriteBatch.Draw(this.texture, new Rectangle(bdyX, bdyY, 32, 32), new Rectangle(), Color.White,);
     */
    class Live: ItemObject
    {
        Rectangle rect;
        public Live(World world, Vector2 position, float width, float height, float mass, Texture2D texture)
            : base(world, position, width, height, mass, texture) {
                this._playerGet = Item.Live;
                loadTiles();
                this.body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.CollisionCategories == Category.Cat3) {
                this.body.IgnoreCollisionWith(fixtureB.Body);
                //fixtureB.IgnoreCollisionWith(fixtureA.Body);
                //this._coinAvailable = false;
            }
            
            return true;
        }

        void loadTiles() {
            rect = new Rectangle(506, 43, 38, 40);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float bdyX = ConvertUnits.ToDisplayUnits(this.Position.X);
            float bdyY = ConvertUnits.ToDisplayUnits(this.Position.Y);
            spriteBatch.Draw(this.texture, new Rectangle((int)bdyX, (int)bdyY, 19, 20), rect, Color.White, 0f, new Vector2(38, 40), SpriteEffects.None, 0f);
            //base.Draw(spriteBatch);
        }

        public override void Kill()
        {
            this.body.OnCollision -= new OnCollisionEventHandler(body_OnCollision);
            this.body.Dispose();
            this.texture = null;
            //base.Kill();
        }
    }
}
