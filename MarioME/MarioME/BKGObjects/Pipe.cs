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
    class Pipe:BKGObject
    {
        Rectangle[] rect;
        int _blockCountY;

        public Pipe(World world, Vector2 position, float width, float height, Texture2D texture, bool isLevelFinish)
            : base(world, position, width, height, texture) {
                rect = new Rectangle[2];
                this._blockCountY = (int)height / 32;
                this.body.IsStatic = true;
                this.body.FixedRotation = true;
                this.body.ResetMassData();
                if (isLevelFinish)
                    this.body.CollisionCategories = Category.Cat9;
        }

        public void loadTileset(int textureID) {
            switch (textureID) { 
                case 0:
                    rect[0] = new Rectangle(204, 375, 60, 43);
                    rect[1] = new Rectangle(273, 386, 50, 32);
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < this._blockCountY; y++) {
                if (y == 0) {
                    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X)+30, ((int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y+30), /*(int)width*/60, /*(int)height*/43), rect[0], Color.White, 0f, new Vector2(60, 43), SpriteEffects.None, 0f);
                }
                else
                    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X)+30, ((int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y+30), 50, 32), rect[1], Color.White, 0f, new Vector2(50, 32), SpriteEffects.None, 0f);

            }
            //base.Draw(spriteBatch);
        }        
    }
}
