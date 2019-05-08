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
    class Bridge: BKGObject
    {
        int _blockCountX;
        public int[] _blockPos = new int[2];
        int _decreaseXFactor;
        
        List<Body> _bodyList;
        Texture2D _texture;

        public Bridge(World world, Vector2 position, float width, float height, Texture2D texture)
            : base(world, position, width, height, texture)
        {
            this.body.FixedRotation = false;
            this.body.ResetMassData();
            this._texture = texture;

            this.body.BodyType = BodyType.Static;
            this._bodyList = new List<Body>();

            this._blockCountX = (int)width / 32;
            
            //SetUpPhysics(world, position, width, height, 1f);
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

        public override void Draw(SpriteBatch spriteBatch)
        {/*
            for(int x = 0; x < _blockCountX; x++)
             //   spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x-(16*(_blockCountX/2)), ((int)ConvertUnits.ToDisplayUnits(this.body.Position.Y)+16 ), 32, 32), new Rectangle(_blockPos[0], _blockPos[1], 32, 32), new Color(255f, 255f, 255f), 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
         */
            //for (int x = 0; x < 3; x++) { 
            //    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.bodies[x].Position.X) + 32 * x-(16*(_blockCountX/2)), ((int)ConvertUnits.ToDisplayUnits(this.bodies[x].Position.Y)+16 ), 32, 32), new Rectangle(_blockPos[0], _blockPos[1], 32, 32), new Color(255f, 255f, 255f), 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
            //}
            spriteBatch.Draw(this._texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + (int)width / 2 - 8/* + 32 * x - (16 * (_blockCountX / 2))*/, ((int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) +16), 32, 32), new Rectangle(_blockPos[0], _blockPos[1], 32, 32), new Color(255f, 255f, 255f), 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
               /* 
            for (int x = 0; x < _bodyList.Count; x++) {
                spriteBatch.Draw(this._texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this._bodyList[x].Position.X)+(int)width/2-8/* + 32 * x - (16 * (_blockCountX / 2))*///, ((int)ConvertUnits.ToDisplayUnits(this._bodyList[x].Position.Y) /*+ 16*/), 32, 32), new Rectangle(_blockPos[0], _blockPos[1], 32, 32), new Color(255f, 255f, 255f), 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                //MessageBox.Show(_bodyList[0].Position.X.ToString()+" - "+_bodyList[0].Position.Y.ToString());

            //}*/
        }

        public void ClipToNext(Body nextBody, World world) {
            PolygonShape shape = new PolygonShape(1f);
            shape.SetAsBox(ConvertUnits.ToSimUnits(16f), ConvertUnits.ToSimUnits(16f));

            Fixture fix = this.body.CreateFixture(shape);
            fix.Friction = 0.6f;
            JointFactory.CreateRevoluteJoint(world, this.body, nextBody, Vector2.Zero);
        }

        public void ClipToLast(Body lastBody, World world) {
            PolygonShape shape = new PolygonShape(1f);
            shape.SetAsBox(ConvertUnits.ToSimUnits(8f), ConvertUnits.ToSimUnits(8f));

            Fixture fix = this.body.CreateFixture(shape);
            fix.Friction = 0.6f;
            JointFactory.CreateRevoluteJoint(world, this.body, lastBody, Vector2.Zero);
        }

        public override void Kill()
        {
            this.body.Dispose();
            
            // Liste / Array an bodies killn
            texture = null;
            //base.Kill();
        }

        public BodyType BodyType {
            get { return this.body.BodyType; }
            set { this.body.BodyType = value; }
        }

        public float BodyMass {
            get { return this.body.Mass; }
            set { this.body.Mass = value; }
        }

    }
}
