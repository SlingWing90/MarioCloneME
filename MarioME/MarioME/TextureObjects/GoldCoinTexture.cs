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
    class GoldCoinTexture
    {
        int _count;
        Texture2D _texture;
        SpriteFont _fnt;
        int[] _frame = new int[20];
        int _frameC;

        public GoldCoinTexture(Texture2D texture, SpriteFont fnt) {
            this._texture = texture;
            this._fnt = fnt;
            this._count = 0;
            loadTiles();
        }

        void loadTiles() {
            this._frame[0] = 0;
            this._frame[1] = 0;

            this._frame[2] = 32;
            this._frame[3] = 0;

            this._frame[4] = 64;
            this._frame[5] = 0;

            this._frame[6] = 96;
            this._frame[7] = 0;
            //
            this._frame[8] = 32;
            this._frame[9] = 48;

            this._frame[10] = 0;
            this._frame[11] = 48;
            //
            this._frame[12] = 96;
            this._frame[13] = 24;

            this._frame[14] = 64;
            this._frame[15] = 24;

            this._frame[16] = 32;
            this._frame[17] = 24;
            //
            this._frame[18] = 0;
            this._frame[19] = 24;
        }

        public void Draw(SpriteBatch spriteBatch, Camera2D cam, GameWindow window) {
            spriteBatch.Draw(this._texture, new Rectangle((int)cam.Pos.X - window.ClientBounds.Width / 2+32, 24, 32, 24), new Rectangle(this._frame[this._frameC], this._frame[this._frameC + 1], 32, 24), Color.White, 0f, new Vector2(32, 24), SpriteEffects.None, 0f);
            spriteBatch.DrawString(this._fnt, this._count.ToString(), new Vector2((int)cam.Pos.X - window.ClientBounds.Width / 2+32, 5), Color.Black);
        }

        public void Update(GameTime gameTime) {
            if (Math.Floor(gameTime.TotalGameTime.TotalMilliseconds % 20) == 0)
            {
                this._frameC += 2;
                if (this._frameC >= 20)
                    this._frameC = 0;
            }
        }

        public int Score {
            get { return this._count; }
            set { this._count = value; }
        }
    }
}
