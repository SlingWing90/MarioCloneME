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
    class SpikedGoomba: EnemyObject
    {
        /*
         * Hinweis:
         * Einzustellen sind:
         * X - Body (_body)
         * - Rectangle-Array (_rect)
         * - Maximale anzahl an Frames (_maxFrames)
         * - "Schwachpunkt" (_rectHittable)
         * - Breite des Body (_width)
         * - Höhe des Body (_height)
         * 
         * - InitPhysic muss in der jeweiligen Klasse gestartet werden
         */ 


        public SpikedGoomba(Texture2D texture, World world, Vector2 position) : base(texture, world, position) {
            this._maxFrames = 7;
            this._rect = new Rectangle[this._maxFrames];

            this._width = 20;
            this._height = 26;

            //this._rectHittable = new Rectangle(this.);

            this.loadTiles();
            this.InitPhysic(world, position);
        }

        protected override void InitPhysic(World world, Vector2 position)
        {
            base.InitPhysic(world, position);
            this._body.CollisionCategories = Category.Cat8;
        }

        void loadTiles() {
            // Walking
            this._rect[0] = new Rectangle(7, 46, 16, 26);
            this._rect[1] = new Rectangle(29, 45, 17, 27);
            this._rect[2] = new Rectangle(51, 46, 18, 26);
            this._rect[3] = new Rectangle(74, 47, 20, 25);
            this._rect[4] = new Rectangle(99, 46, 18, 26);
            this._rect[5] = new Rectangle(123, 45, 18, 27);
            this._rect[6] = new Rectangle(148, 46, 16, 26);
        }
    }
}
