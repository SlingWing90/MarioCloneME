using System;
using System.Collections.Generic;
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

namespace MarioME.EnemyObjects
{
    class Chompy: EnemyObject
    {
        public Chompy(Texture2D texture, World world, Vector2 position) : base(texture, world, position) {
            this._maxFrames = 2;
            this._rect = new Rectangle[this._maxFrames+1]; //+1 für die kleinen kügelchen

            this._width = 104;
            this._height = 115;

            this.loadTiles();
            this.InitPhysic(world, position);
        }

        protected override void InitPhysic(World world, Vector2 position)
        {
            this._body = BodyFactory.CreateCircle(world, ConvertUnits.ToSimUnits(52), 1f);
            base.InitPhysic(world, position);
            this.initFixtures(world, position);
        }

        void loadTiles() {
            this._rect[0] = new Rectangle(147, 11, 104, 115);
            this._rect[1] = new Rectangle(286, 10, 117, 115);
            this._rect[2] = new Rectangle(68, 183, 28, 28);
        }

        public void initFixtures(World world, Vector2 position)
        { 
            
        }
    }
}
