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
    enum Item
    {
        Score = 0,
        Live = 1
    }

    class ItemObject: PhysicsObject
    {
        protected bool _isAvailable;
        protected Item _playerGet;

        public ItemObject(World world, Vector2 position, float width, float height, float mass, Texture2D texture):base(world, position, width, height, mass, texture) {

            this.body.BodyType = BodyType.Static;
            this.body.CollisionCategories = Category.Cat5;
            this._isAvailable = true;
            this.body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.CollisionCategories == Category.Cat3)
            {
                this.body.IgnoreCollisionWith(fixtureB.Body);
                //fixtureB.IgnoreCollisionWith(fixtureA.Body);
                this._isAvailable = false;
            }
            return true;
            //throw new System.NotImplementedException();
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }

        public bool IsAvailable {
            get { return this._isAvailable; }
        }

        public Item PlayerGet {
            get { return this._playerGet; }
        }

        public virtual void Kill() { 
        
        }
    }
}
