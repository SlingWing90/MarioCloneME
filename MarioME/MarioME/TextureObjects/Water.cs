using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MarioME.TextureObjects
{
    class Water:TextureObject
    {
        int[] _blank = new int[2];
        int frameCount = 0;
        float totalTime = 0;

        public Water(Vector2 position, float width, float height, Texture2D texture):base(position, width, height, texture)
        {
            this.HasEnd = false;
            this.HasStart = false;
            this._centerBlock = new int[6];

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < this._blockCountX; x++) {
                for (int y = 0; y < this._blockCountY; y++) {
                    if (y == 0)
                    {
                        // Oberen Part zeichnen
                        spriteBatch.Draw(texture, new Rectangle((int)(this.position.X) + 32 * x - 1 * x, ((int)this.position.Y)+32*y, 32, 32), new Rectangle(this._centerBlock[frameCount], this._centerBlock[frameCount+1], 32, 32), new Color(255f, 255f, 255f));                       
                    }
                    else { 
                        // Unteren Part zeichnen
                        spriteBatch.Draw(texture, new Rectangle((int)(this.position.X) + 32 * x - 1 * x, ((int)this.position.Y)+32*y, 32, 32), new Rectangle(this._blank[0], this._blank[1], 32, 32), new Color(255f, 255f, 255f));                       
                    }
                }            
            }
        }

        public override void Update(GameTime gameTime)
        {
            //totalTime += (float)gameTime.TotalGameTime.TotalSeconds;
            if (Math.Floor(gameTime.TotalGameTime.TotalMilliseconds) % 1000 == 0)//(totalTime >= (float)(1 / 1))
            { 
                frameCount += 2;
                //totalTime -= (float)(1 / 1);
                if (frameCount >= 6)
                    frameCount = 0;
            }
            
        }

        public void loadTiles(int lvlID)
        {
            switch (lvlID) {
                case 0:
                    // Frame 1
                    this._centerBlock[0] = 490;
                    this._centerBlock[1] = 105;
                    // Frame 2
                    this._centerBlock[2] = 524;
                    this._centerBlock[3] = 105;
                    // Frame 3
                    this._centerBlock[4] = 558;
                    this._centerBlock[5] = 105;
                    // Blank
                    this._blank[0] = 490;
                    this._blank[1] = 139;
                    break;
            
            }
        }

    }
}
