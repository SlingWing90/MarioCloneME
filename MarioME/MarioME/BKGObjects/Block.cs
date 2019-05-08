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

namespace MarioME.BKGObjects
{
    class Block: BKGObject
    {

        int[] _coordinates;
        int _blockCountX;
        int _blockCountY;


        public Block(World world, Vector2 position, float width, float height, float mass, Texture2D texture) : base(world, position, width, height, texture) {
            this._coordinates = new int[2];
            this._blockCountY = (int)height / 32;
            this._blockCountX = (int)width / 32;
            this.body.BodyType = BodyType.Static;
        }


        public void loadTileset(int lvlID)
        {
            switch (lvlID)
            {
                case 0:
                    this._coordinates[0] = 266;
                    this._coordinates[1] = 293;
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < _blockCountX; x++)
            {
                for (int y = 0; y < _blockCountY; y++)
                {
                    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) + 32 - x, ((int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) - (int)(this.height / 2) + 32 /*- 16*/), /*(int)width*/32, /*(int)height*/32), new Rectangle(this._coordinates[0], this._coordinates[1], 32, 32), new Color(255f, 255f, 255f), 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                }
            }
        }
    }
}
