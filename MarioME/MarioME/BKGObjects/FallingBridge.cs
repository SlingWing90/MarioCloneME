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

namespace MarioME.BKGObjects
{
    class FallingBridge: BKGObject
    {
        int _blockCountX;
        Timer _blockFallingDelay;
        int _count;
        int[] _blockPos = new int[2];    

        public FallingBridge(World world, Vector2 position, float width, float height,  Texture2D texture)
            : base(world, position, width, height, texture)
        {   
            this._blockCountX = (int)width / 32;
            this.body.OnCollision += new OnCollisionEventHandler(body_OnCollision);
            this.body.BodyType = BodyType.Static;

            this._blockFallingDelay = new Timer();
            this._blockFallingDelay.Tick += new EventHandler(_blockFallingDelay_Tick);
            this._blockFallingDelay.Interval = 1000;
            this._blockFallingDelay.Enabled = false;

        }

        public void loadTileset(int lvlID)
        {
            switch (lvlID)
            {
                case 0:
                    _blockPos[0] = 294;
                    _blockPos[1] = 224;
                    break;
            }
        }

        void _blockFallingDelay_Tick(object sender, EventArgs e)
        {
            //this.texture.
            _count++;
            if (_count >= 2)
            {
                this.body.BodyType = BodyType.Dynamic;
            }

            if (this._count >= 5) {
                this.body.Dispose();
                this.texture = null;//.Dispose();
                this._blockFallingDelay.Dispose();
            }
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, FarseerPhysics.Dynamics.Contacts.Contact contact)
        {
            if (fixtureB.CollisionCategories == Category.Cat3) {
                this._blockFallingDelay.Enabled = true;
                //fixtureA.IgnoreCollisionWith(fixtureB);
            }
            
            return true;
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(!this.body.IsDisposed){
                for (int x = 0; x < _blockCountX; x++)
                    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2), ((int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) - (int)(this.height / 2) /*- 16*/), /*(int)width*/32, /*(int)height*/32), new Rectangle(_blockPos[0], _blockPos[1], 32, 32), Color.LightSlateGray /*new Color(255f, 255f, 255f)*/);
            }
        }

        public override void Kill()
        {
            
        }
    }
}
