using System;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MarioME
{
    public enum Activity
    {
        Jumping,
        Running,
        Idle,
        None
    }

    public class CompositeCharacter : Character
    {
        private const float nextJumpDelayTime = 1f;
        private const float runSpeed = 8;
        private const float jumpImpulse = -2;
        public Activity activity;
        private float centerOffset;
        public FixedAngleJoint fixedAngleJoint;
        private float jumpDelayTime;
        private Vector2 jumpForce;
        public RevoluteJoint motor;
        protected Activity oldActivity;
        public Body wheel;

        public CompositeCharacter(World world, Vector2 position, float width, float height, float mass, Texture2D texture)
            : base(world, position, width, height, mass, texture)
        {
            if (width > height)
            {
                throw new Exception("Error width > height: can't make character because wheel would stick out of body");
            }

            activity = Activity.None;

            wheel.OnCollision += new OnCollisionEventHandler(OnCollision);
        }

        public override Vector2 Position
        {
            get { return (ConvertUnits.ToDisplayUnits(body.Position) + Vector2.UnitY*centerOffset); }
        }

        protected override void SetUpPhysics(World world, Vector2 position, float width, float height, float mass)
        {
            //Create a fixture with a body almost the size of the entire object
            //but with the bottom part cut off.
            float upperBodyHeight = height - (width/2);

            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(width), ConvertUnits.ToSimUnits(upperBodyHeight), mass/2);
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 0.3f;
            body.Friction = 0.5f;
            //also shift it up a tiny bit to keey the new object's center correct
            body.Position = ConvertUnits.ToSimUnits(position - (Vector2.UnitY*(width/4)));
            centerOffset = position.Y - (float) ConvertUnits.ToDisplayUnits(body.Position.Y); //remember the offset from the center for drawing

            //Now let's make sure our upperbody is always facing up.
            fixedAngleJoint = JointFactory.CreateFixedAngleJoint(world, body);

            //Create a wheel as wide as the whole object
            wheel = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(width/2), mass/2);
            //And position its center at the bottom of the upper body
            wheel.Position = body.Position + ConvertUnits.ToSimUnits(Vector2.UnitY*(upperBodyHeight/2));
            wheel.BodyType = BodyType.Dynamic;
            wheel.Restitution = 0.3f;
            wheel.Friction = 0.5f;

            //These two bodies together are width wide and height high :)
            //So lets connect them together
            motor = JointFactory.CreateRevoluteJoint(world, body, wheel, Vector2.Zero);
            motor.MotorEnabled = true;
            motor.MaxMotorTorque = 1000f; //set this higher for some more juice
            motor.MotorSpeed = 0;

            //Make sure the two fixtures don't collide with each other
            wheel.IgnoreCollisionWith(body);
            body.IgnoreCollisionWith(wheel);

            //Set the friction of the wheel to float.MaxValue for fast stopping/starting
            //or set it higher to make the character slip.
            wheel.Friction = float.MaxValue;
        }

        //Fired when we collide with another object. Use this to stop jumping
        //and resume normal movement
        public bool OnCollision(Fixture fix1, Fixture fix2, Contact contact)
        {
            //Check if we are both jumping this frame and last frame
            //so that we ignore the initial collision from jumping away from 
            //the ground
            if (activity == Activity.Jumping && oldActivity == Activity.Jumping)
            {
                activity = Activity.None;
            }
            return true;
        }

        protected override void HandleInput(GameTime gameTime)
        {
            oldActivity = activity;
            keyState = Keyboard.GetState();

            HandleJumping(keyState, oldState, gameTime);

            if (activity != Activity.Jumping)
            {
                HandleRunning(keyState, oldState, gameTime);
            }

            if (activity != Activity.Jumping && activity != Activity.Running)
            {
                HandleIdle(keyState, oldState, gameTime);
            }

            oldState = keyState;
        }

        private void HandleJumping(KeyboardState state, KeyboardState oldState, GameTime gameTime)
        {

            if (jumpDelayTime < 0)
            {
                jumpDelayTime += (float) gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (state.IsKeyDown(Keys.Space) && oldState.IsKeyDown(Keys.Space) && activity != Activity.Jumping)
            {
                if (jumpDelayTime >= 0)
                {
                    motor.MotorSpeed = 0;
                    jumpForce.Y = jumpImpulse;
                    body.ApplyLinearImpulse(jumpForce, body.Position);
                    jumpDelayTime = -nextJumpDelayTime;
                    activity = Activity.Jumping;
                }
            }

            if (activity == Activity.Jumping)
            {
                if (keyState.IsKeyDown(Keys.Right))
                {
                    if (body.LinearVelocity.X < 0)
                    {
                        body.LinearVelocity = new Vector2(-body.LinearVelocity.X*2, body.LinearVelocity.Y);
                    }
                }
                else if (keyState.IsKeyDown(Keys.Left))
                {
                    if (body.LinearVelocity.X > 0)
                    {
                        body.LinearVelocity = new Vector2(-body.LinearVelocity.X*2, body.LinearVelocity.Y);
                    }
                }
            }
        }

        private void HandleRunning(KeyboardState state, KeyboardState oldState, GameTime gameTime)
        {
            if (keyState.IsKeyDown(Keys.Right))
            {
                motor.MotorSpeed = runSpeed;
                activity = Activity.Running;
            }
            else if (keyState.IsKeyDown(Keys.Left))
            {
                motor.MotorSpeed = -runSpeed;
                activity = Activity.Running;
            }

            if (keyState.IsKeyUp(Keys.Left) && keyState.IsKeyUp(Keys.Right))
            {
                motor.MotorSpeed = 0;
                activity = Activity.None;
            }
        }

        private void HandleIdle(KeyboardState state, KeyboardState oldState, GameTime gameTime)
        {
            if (activity == Activity.None)
            {
                activity = Activity.Idle;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //These first two draw calls draw the upper and lower body independently
            spriteBatch.Draw(texture, new Rectangle((int) ConvertUnits.ToDisplayUnits(body.Position.X), (int) ConvertUnits.ToDisplayUnits(body.Position.Y), (int) width, (int) (height - (width/2))), null, Color.White, body.Rotation, origin,
                             SpriteEffects.None, 0f);
            spriteBatch.Draw(texture, new Rectangle((int) ConvertUnits.ToDisplayUnits(wheel.Position.X), (int) ConvertUnits.ToDisplayUnits(wheel.Position.Y), (int) width, (int) width), null, Color.White, wheel.Rotation,
                             origin, SpriteEffects.None, 0f);

            //This last draw call shows how to draw these two bodies with one texture (drawn semi-transparent here so you can see the inner workings)            
            spriteBatch.Draw(texture, new Rectangle((int) Position.X, (int) (Position.Y), (int) width, (int) height), null, new Color(1, 1, 1, 0.5f), body.Rotation, origin, SpriteEffects.None, 0f);
        }
    }
}