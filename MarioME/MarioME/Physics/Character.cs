using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MarioME
{
    public class Character : PhysicsObject
    {
        public float forcePower;
        protected KeyboardState keyState;
        protected KeyboardState oldState;
        protected bool isJumping;

        public Character(World world, Vector2 position, float width, float height, float mass, Texture2D texture)
            : base(world, position, width, height, mass, texture)
        {
            this.body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (body.Position.Y < fixtureB.Body.Position.Y)
            {
                this.isJumping = false;
            }
            else
                this.isJumping = true;
            
            //return this.isJumping;
            //throw new System.NotImplementedException();
            return true;
        }

        public virtual void Update(GameTime gameTime)
        {
            HandleInput(gameTime);
        }

        protected virtual void HandleInput(GameTime gameTime)
        {
            
            keyState = Keyboard.GetState();
            
            //Apply force in the arrow key direction
            Vector2 force = Vector2.Zero;
            if (keyState.IsKeyDown(Keys.D)) {
                if (!isJumping)
                    force.X = 0.1f;
                else force.X = 0.03f;
            }

            if (keyState.IsKeyDown(Keys.A))
            {
                if (!isJumping)
                    force.X = -0.1f;
                else force.X = -0.03f;
            }

            if (keyState.IsKeyDown(Keys.W) && !isJumping)
            {
                force.Y = -3.0f;
                isJumping = true;
            }

            if(!(body.Position.Y > this.height))
                body.ApplyLinearImpulse(force, body.Position);
            
            oldState = keyState;
        }

    }
}