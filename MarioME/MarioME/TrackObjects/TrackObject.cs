using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

namespace MarioME.TrackObjects
{
    class TrackObject
    {
        protected Path _path;
        protected Texture2D _texture;

        protected Rectangle[] _rectTrack;
        Rectangle[] _rectPod;

        protected Body _bdyPod;

        float _bdySpd;
        float _time;

        public TrackObject(int tilesetID, ContentManager Content, World world, Texture2D texture, float Speed)
        {
            this._texture = texture;

            this._rectTrack = new Rectangle[5];
            this._rectPod = new Rectangle[3];

            this._bdySpd = Speed;
            this._time = 0f;

            loadTileset(tilesetID, Content);
            setupPhysic(world);
        }

        protected virtual void setupPhysic(World world) { 
            // ToSim!
            // Position des Body wird in der SubKlasse festgelegt
            this._bdyPod = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(64), ConvertUnits.ToSimUnits(17), 1f);
            this._bdyPod.BodyType = BodyType.Kinematic;
            //this._bdyPod.Friction = 0.5f;
            this._bdyPod.FixedRotation = true;
        }

        protected virtual void loadTileset(int tilesetID, ContentManager Content){
            switch (tilesetID) { 
                case 0:
                    this._rectTrack[0] = new Rectangle(500, 278, 6, 32); // Hoch
                    this._rectTrack[1] = new Rectangle(500, 244, 12, 32); // Hoch nach Rechts
                    this._rectTrack[2] = new Rectangle(534, 244, 32, 6); // Oben
                    this._rectTrack[3] = new Rectangle(588, 244, 12, 32); // Hoch nach Links
                    this._rectTrack[4] = new Rectangle(594, 278, 6, 32); // Runter

                    this._rectPod[0] = new Rectangle(518, 290, 64, 17); // Träger
                    this._rectPod[1] = new Rectangle(517, 252, 32, 32); // Träger-Links
                    this._rectPod[2] = new Rectangle(551, 252, 32, 32); // Träger-Rechts
                break;
            }            
        }

        public virtual void Update(GameTime gameTime) { 
            this._time += this._bdySpd;

            if (this._time > 1)
                this._time = 0;

            PathManager.MoveBodyOnPath(this._path, this._bdyPod, _time, 1f, 1/60f);
        }

        public virtual void Draw(SpriteBatch spriteBatch) { 
            float bdyPosX = ConvertUnits.ToDisplayUnits(this._bdyPod.Position.X);
            float bdyPosY = ConvertUnits.ToDisplayUnits(this._bdyPod.Position.Y);

            //Seil
            for (int x = 0; x < 12; x++)
            {
                spriteBatch.Draw(this._texture, new Rectangle((int)bdyPosX + 3, (int)bdyPosY - 35-x*32, 6, 32), this._rectTrack[0], Color.White, 0f, new Vector2(6, 32), SpriteEffects.None, 0f);
            }
            // Träger Links
            spriteBatch.Draw(this._texture, new Rectangle((int)bdyPosX, (int)bdyPosY-8, 32, 32), this._rectPod[1], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);

            // Träger Rechts
            spriteBatch.Draw(this._texture, new Rectangle((int)bdyPosX+32, (int)bdyPosY - 8, 32, 32), this._rectPod[2], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);

            // Träger
            spriteBatch.Draw(this._texture, new Rectangle((int)bdyPosX+32, (int)bdyPosY+8, 64, 17), this._rectPod[0], Color.White, 0f, new Vector2(64, 17), SpriteEffects.None, 0f);
            
            
        }

        public virtual void Kill() {
            this._texture = null;
            this._bdyPod.Dispose();
        }

        public Path Path {
            get { return this._path; }
            set { this._path = value; }
        }

        public Body Body {
            get { return this._bdyPod; }
        }
    }
}
