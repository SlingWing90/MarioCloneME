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

namespace MarioME.TextureObjects
{
    class TextureObject
    {
        //protected AnimatedTexture animTexture;
        protected Texture2D texture;
        protected int _blockCountX;
        protected int _blockCountY;

        protected bool _hasEnd = false;
        protected bool _hasStart = false;

        protected int[] _startBlock;
        protected int[] _centerBlock;
        protected int[] _endBlock;

        protected float _width;
        protected float _height;

        protected Vector2 position;

        public TextureObject(Vector2 position, float width, float height, Texture2D texture) {
            this.texture = texture;
            this.position = position;
            this._blockCountX = (int)(width / 32);
            this._blockCountY = (int)(height / 32);

            this._width = width;
            this._height = height;
        
        }

        public virtual void Update(GameTime gameTime) { 
        
        }


        public virtual void Draw(SpriteBatch spriteBatch) {
            for (int x = 0; x < this._blockCountX; x++)
            {
                if (this._hasStart && this._hasEnd && x > 0 && x < this._blockCountX - 1)
                    spriteBatch.Draw(texture, new Rectangle((int)(this.position.X) + 32 * x - 1 * x, (int)this.position.Y, 32, 32), new Rectangle(this._centerBlock[0], this._centerBlock[1], 32, 32), new Color(255f, 255f, 255f));
                //else
                //    spriteBatch.Draw(texture, new Rectangle((int)/*ConvertUnits.ToDisplayUnits(*/this.position.X + 32 * x, ((int)ConvertUnits.ToDisplayUnits(this.position.Y)), /*(int)width*/32, /*(int)height*/32), new Rectangle(this._centerBlock[0], this._centerBlock[1], 32, 32), new Color(255f, 255f, 255f));

                if (this._hasStart && x == 0)
                    spriteBatch.Draw(texture, new Rectangle((int)(this.position.X) + 32 * x, (int)(this.position.Y), 32, 32), new Rectangle(this._startBlock[0], this._startBlock[1], 32, 32), new Color(255f, 255f, 255f));

                
                if (this._hasEnd && x == this._blockCountX - 1)
                    spriteBatch.Draw(texture, new Rectangle((int)(this.position.X) + 32 * x-1*x, (int)(this.position.Y), 32, 32), new Rectangle(this._endBlock[0], this._endBlock[1], 32, 32), new Color(255f, 255f, 255f));

            }
        }
        
        public bool HasStart {
            set { this._hasStart = value; }
        }

        public bool HasEnd
        {
            set { this._hasEnd = value; }
        }
    }
}
