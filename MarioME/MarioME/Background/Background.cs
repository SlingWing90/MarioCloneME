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

namespace MarioME.Background
{
    class Background
    {
        Camera2D _cam;
        Texture2D _texture;
        int _width;
        int _height;
        Rectangle[] rect;
        int[] count;

        public Background(int width, int height, Camera2D cam, Texture2D texture, int backgroundID) {
            this._width = width;
            this._height = height;
            this._cam = cam;
            this._texture = texture;

            loadBackground(backgroundID);

        }

        void loadBackground(int ID) {
            switch (ID) { 
                case 0:
                    rect = new Rectangle[3];
                    count = new int[3];

                    rect[0] = new Rectangle(182, 168, 276, 470);
                    count[0] = (_width / 276)+1;

                    rect[1] = new Rectangle(518, 200, 746, 670);
                    count[1] = (_width / 746)+1;

                    rect[2] = new Rectangle(189, 680, 262, 228);
                    count[2] = (_width / 262)+1;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch){
            
            for (int x = 0; x < count[0]; x++) {
                spriteBatch.Draw(this._texture, new Rectangle(x * 276, 0, 278, 470), rect[0], Color.White);
            }

            for (int x = 0; x < count[1]; x++) {
                spriteBatch.Draw(this._texture, new Rectangle(x * 748, 0, 748, _height), rect[1], Color.White);
            }

            for (int x = 0; x < count[2]; x++)
            {
                spriteBatch.Draw(this._texture, new Rectangle(x * 262, _height - 228, 262, 228), rect[2], Color.White);
            }
        }

        public void Kill() {
            this.rect = null;
            this._texture = null;
            this._cam = null;
        }
    }
}
