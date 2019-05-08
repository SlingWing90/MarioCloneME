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
using MarioME;

namespace MarioME
{
    public class JumpCharacter : PhysicsObject
    {
        bool _levelFinished;

        int[] pStand = new int[12];
        int[] pWalk = new int[16];
        int[] pRun = new int[8];
        int[] pJump = new int[2];

        public float forcePower;
        protected KeyboardState keyState;
        protected KeyboardState oldState;

        protected bool isJumping;
        
        protected bool isStanding;
        protected bool isWalking;
        protected bool isLeft;

        protected int frame;

        int pLive;
        
        public JumpCharacter(World world, Vector2 position, float width, float height, float mass, Texture2D texture)
            : base(world, position, width, height, mass, texture)
        {
            this.body.FixedRotation = true;
            this.body.ResetMassData();
            this.body.CollisionCategories = Category.Cat3;    
            this.body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            this.body.Friction = 0.5f;

            this.isStanding = true;
            isJumping = false;
            isLeft = false;
            //this.shpLeftFoot = new CircleShape(10, 0.5f);

            this.pLive = 8;
            loadTiles();
            //this.body.IgnoreCCD = true;
            
        }

        void loadTiles()
        { 
            pStand[0] = 4; // 31
            pStand[1] = 5; // 45
            
            pStand[2] = 36; 
            pStand[3] = 5;
            
            pStand[4] = 68;
            pStand[5] = 5;
            
            pStand[6] = 100;
            pStand[7] = 5;
            
            pStand[8] = 132;
            pStand[9] = 5;
            
            pStand[10] = 166;
            pStand[11] = 5;
            
            pWalk[0] = 9;
            pWalk[1] = 123;
            
            pWalk[2] = 41;
            pWalk[3] = 122;
            
            pWalk[4] = 73;
            pWalk[5] = 121;
            
            pWalk[6] = 105;
            pWalk[7] = 121;
            
            pWalk[8] = 137;
            pWalk[9] = 123;
            
            pWalk[10] = 169;
            pWalk[11] = 122;
            
            pWalk[12] = 201;
            pWalk[13] = 122;
            
            pWalk[14] = 233;
            pWalk[15] = 122;
            
            pJump[0] = 8;
            pJump[1] = 68;
            
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            
            if (fixtureB.CollisionCategories == Category.Cat5)
            {
                this.body.IgnoreCollisionWith(fixtureB.Body);
            }

            if (fixtureB.CollisionCategories == Category.Cat2)
            {
                this.isJumping = true;
                this.body.IgnoreCollisionWith(fixtureB.Body);
            }

            if (fixtureB.CollisionCategories == Category.Cat4)
            {
                if(this.isJumping)
                    fixtureA.Body.ApplyForce(new Vector2(0f, ConvertUnits.ToDisplayUnits(-1.5f)));

                this.isJumping = true;
                //MessageBox.Show(ConvertUnits.ToDisplayUnits(this.body.LinearVelocity.Y).ToString());
            }

            

            if (body.Position.Y < fixtureB.Body.Position.Y || fixtureB.CollisionCategories == Category.Cat7)// || fixtureB.CollisionCategories == Category.Cat4)
            {
                this.isJumping = false;
            }

            // Kollision gegen Gegner
            if (fixtureB.CollisionCategories == Category.Cat10 || (fixtureB.CollisionCategories == Category.Cat4 && this.body.Position.Y + ConvertUnits.ToSimUnits(this.height) / 2 > fixtureB.Body.Position.Y))// && (Math.Floor(ConvertUnits.ToDisplayUnits(fixtureA.Body.Position.Y)) + 5 == Math.Floor(ConvertUnits.ToDisplayUnits(fixtureB.Body.Position.Y)) || ConvertUnits.ToDisplayUnits(fixtureA.Body.Position.Y) + 6 == Math.Floor(ConvertUnits.ToDisplayUnits(fixtureB.Body.Position.Y))) && (ConvertUnits.ToDisplayUnits(this.body.Position.X) > ConvertUnits.ToDisplayUnits(fixtureB.Body.Position.X) || ConvertUnits.ToDisplayUnits(this.body.Position.X) < ConvertUnits.ToDisplayUnits(fixtureB.Body.Position.X)))
            {
                //MessageBox.Show("BodyPosY: " + fixtureB.Body.Position.Y + " | MarioPos: " + (this.body.Position.Y).ToString() + " Mario Height: "+ConvertUnits.ToSimUnits(this.height).ToString());
                this.pLive--;
                if (ConvertUnits.ToDisplayUnits(this.body.Position.X) > ConvertUnits.ToDisplayUnits(fixtureB.Body.Position.X))
                    this.body.ApplyLinearImpulse(new Vector2(2.5f, 1.5f));
                else
                    this.body.ApplyLinearImpulse(new Vector2(-2.5f, 1.5f));
            }

            if (fixtureB.CollisionCategories == Category.Cat7) {
                this.isJumping = false;
            }

            if (fixtureB.CollisionCategories == Category.Cat8)
                this.pLive--;

            if (fixtureB.CollisionCategories == Category.Cat9) {
                this._levelFinished = true;    
            }

            return true;
        }

        public virtual void Update(GameTime gameTime, GameWindow Window)
        {
            if (ConvertUnits.ToDisplayUnits(this.body.LinearVelocity.Y) <= -900)
            {
                this.body.ApplyForce(new Vector2(0, ConvertUnits.ToDisplayUnits(1.5f)));
                //MessageBox.Show("Test");
            }

            if(keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.H))
                this._levelFinished = true;

            if (ConvertUnits.ToDisplayUnits(this.body.Position.Y) > Window.ClientBounds.Height || this.pLive == 0)
            {
                // Tod-Sequenz
                this.pLive = 0;
                this.body.Dispose();
            }

            //
            //  Animationssteuerung
            if (this.isWalking) {
                if (Math.Floor(gameTime.TotalGameTime.TotalMilliseconds) % 100 == 0)
                    frame += 2;
                if (frame >= 16)
                    frame = 0;
            }
            
            if(this.isStanding){
                if(Math.Floor(gameTime.TotalGameTime.TotalMilliseconds) % 200 == 0)
                    frame += 2;
                if (frame >= 12)
                    frame = 0;
            }
            //

            HandleInput(gameTime);
        }

        protected virtual void HandleInput(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            //Apply force in the arrow key direction
            Vector2 force = Vector2.Zero;

            //if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt)) {
            //    MessageBox.Show(ConvertUnits.ToDisplayUnits(body.Position.X).ToString() + ":" + ConvertUnits.ToDisplayUnits(body.Position.Y).ToString());
            //}

            if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D) || keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                if (!isJumping)
                    force.X = 0.1f;
                else force.X = 0.03f;

                this.isStanding = false;
                if (!this.isWalking) {
                    frame = 0;
                    this.isWalking = true;
                }
                this.isLeft = false;
            }

            if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A) || keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                if (!isJumping)
                    force.X = -0.1f;
                else force.X = -0.03f;

                this.isStanding = false;
                if (!this.isWalking)
                {
                    frame = 0;
                    this.isWalking = true;
                }
                isLeft = true;
            }

            if ((keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W) || keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up)) && !isJumping)
            {
                force.Y = -3.0f;
                isJumping = true;
                //body.ApplyLinearImpulse(force, body.Position);
            }
            body.ApplyLinearImpulse(force, body.Position);
            /* TODO PRÜFEN
            if (!(body.Position.Y > this.height))
                body.ApplyLinearImpulse(force, body.Position);
            */

            //if(!this.isJumping

            if ((keyState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.A) || keyState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.Left)) && (keyState.IsKeyUp(Microsoft.Xna.Framework.Input.Keys.D) || keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right)))
            {
                if (!isJumping)
                {
                    this.isWalking = false;
                    if (!this.isStanding)
                    {
                        frame = 0;
                        this.isStanding = true;
                    }
                }
            }

            //if (keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.G)) {
            //    MessageBox.Show(this.body.AngularVelocity.ToString());
            //}
            

            oldState = keyState;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            

            if (this.isJumping){
                if (isLeft)
                    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(body.Position.X)+8/* + texture.Width / 2 - 62*/, (int)ConvertUnits.ToDisplayUnits(body.Position.Y) + 24 /*+ texture.Height / 2 - 78*/, (int)width, (int)height), new Rectangle(pJump[0], pJump[1], 38, 48), Color.White, 0f, new Vector2(38, 48),
                                     SpriteEffects.FlipHorizontally, 0f);
                else
                    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(body.Position.X)+8/* + texture.Width / 2 - 62*/, (int)ConvertUnits.ToDisplayUnits(body.Position.Y) +24/*+ texture.Height / 2 - 78*/, (int)width, (int)height), new Rectangle(pJump[0], pJump[1], 38, 48), Color.White, 0f, new Vector2(38, 48),
                                     SpriteEffects.None, 0f);
            }
            else if (this.isWalking)
            {
                if (isLeft)
                    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(body.Position.X)+8/* + texture.Width / 2 - 42*/, (int)ConvertUnits.ToDisplayUnits(body.Position.Y)+23/* + texture.Height / 2 - 78*/, (int)width, (int)height), new Rectangle(pWalk[frame], pWalk[frame + 1], 32, 45), Color.White, 0f, new Vector2(32, 45),
                                     SpriteEffects.FlipHorizontally, 0f);
                else
                    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(body.Position.X)+8/* + texture.Width / 2 - 42*/, (int)ConvertUnits.ToDisplayUnits(body.Position.Y)+23 /*+ texture.Height / 2 - 78*/, (int)width, (int)height), new Rectangle(pWalk[frame], pWalk[frame + 1], 32, 45), Color.White, 0f, new Vector2(32, 45),
                                     SpriteEffects.None, 0f);
            }
            else if (this.isStanding)
            {
                if (isLeft)
                    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(body.Position.X)+8 /*+ texture.Width / 2 - 42*/, (int)ConvertUnits.ToDisplayUnits(body.Position.Y)+23 /*+ texture.Height / 2 - 78*/, (int)width, (int)height), new Rectangle(pStand[frame], pStand[frame + 1], 32, 45), Color.White, 0f, new Vector2(32, 45),
                                     SpriteEffects.FlipHorizontally, 0f);
                else
                    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(body.Position.X)+8 /*+ texture.Width / 2 - 42*/, (int)ConvertUnits.ToDisplayUnits(body.Position.Y) + 23/* + texture.Height / 2 - 78*/, (int)width, (int)height), new Rectangle(pStand[frame], pStand[frame + 1], 32, 45), Color.White, 0f, new Vector2(32, 45),
                                     SpriteEffects.None, 0f);
            }
            

            //spriteBatch.Draw(txtLeftFoot, new Rectangle((int)ConvertUnits.ToDisplayUnits(fixFootLeftToBody.Body.Position.X), (int)ConvertUnits.ToDisplayUnits(fixFootLeftToBody.Body.Position.Y), (int)width, (int)height), null, Color.White);
        }

        public int PlayerLive {
            get { return this.pLive; }
            set { this.pLive = value; }
        }

        public bool LevelFinished {
            get { return this._levelFinished; }
            set { this._levelFinished = value; }
        }

    }
}