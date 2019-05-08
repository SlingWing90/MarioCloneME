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

using MarioME.BKGObjects;
using MarioME.TextureObjects;

namespace MarioME.EnemyObjects
{
    class FlyingSpikedGoomba: EnemyObject
    {
        public FlyingSpikedGoomba(Texture2D texture, World world, Vector2 position):base(texture, world, position) {
            this._maxFrames = 8;
            this._rect = new Rectangle[this._maxFrames];

            this._width = 22;
            this._height = 19;

            this.loadTiles();
            this.InitPhysic(world, position);
        }

        void loadTiles() {
            this._rect[0] = new Rectangle(0, 20, 30, 18);
            this._rect[1] = new Rectangle(31, 20, 32, 18);
            this._rect[2] = new Rectangle(64, 20, 32, 18);
            this._rect[3] = new Rectangle(97, 20, 32, 18);
            this._rect[4] = new Rectangle(130, 20, 32, 18);
            this._rect[5] = new Rectangle(163, 20, 32, 18);
            this._rect[6] = new Rectangle(196, 20, 32, 18);
            this._rect[7] = new Rectangle(229, 20, 32, 18);
        }

        protected override void InitPhysic(World world, Vector2 position)
        {
            base.InitPhysic(world, position);
            this._body.CollisionCategories = Category.Cat8;
        }
    }
}
