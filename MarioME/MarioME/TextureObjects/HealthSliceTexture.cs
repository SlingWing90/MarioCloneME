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
//using MarioME;

namespace MarioME.TextureObjects
{
    class HealthSliceTexture
    {
        int _live;
        int[] _liveFrame = new int[18];
        Texture2D _liveTexture;
        Vector2 _origin;

        public HealthSliceTexture(Texture2D liveTexture)
        {
            this._liveTexture = liveTexture;
            this._origin = new Vector2(liveTexture.Width / 2, liveTexture.Height / 2); 
            this._live = 16;
            loadTiles();
        }

        void loadTiles() {
            // 0
            _liveFrame[0] = 580;
            _liveFrame[1] = 960;

            // 1
            _liveFrame[2] = 583;
            _liveFrame[3] = 228;

            // 2
            _liveFrame[4] = 428;
            _liveFrame[5] = 228;
            
            // 3
            _liveFrame[6] = 272;
            _liveFrame[7] = 226;
            
            // 4
            _liveFrame[8] = 116;
            _liveFrame[9] = 226;
            
            // 5
            _liveFrame[10] = 582;
            _liveFrame[11] = 72;
            
            // 6
            _liveFrame[12] = 428;
            _liveFrame[13] = 72;
            
            // 7
            _liveFrame[14] = 272;
            _liveFrame[15] = 72;
            
            // 8
            _liveFrame[16] = 116;
            _liveFrame[17] = 72;
            
        }

        public void Draw(SpriteBatch spriteBatch, Camera2D cam, GameWindow Window) {
            spriteBatch.Draw(_liveTexture, new Rectangle((int)Math.Floor(cam.Pos.X)+Window.ClientBounds.Width/2+77, 210, 55, 55), new Rectangle(_liveFrame[_live], _liveFrame[_live + 1], 148, 148), Color.White, 0f, _origin,
                                     SpriteEffects.None, 0f);
        }

        /*
         * Hinweis: Das Leben wird in nochmal Formatiert. D.h: value = 2 -> Frame Nr.4 && this._live = 16 -> 8 "Lebensslices"
         */
        public int FormattedLive {
            get { return this._live / 2; }
            set { this._live = value * 2; }
        }

        public int RealLive
        {
            get { return this._live; }
            set { this._live = value; }
        }

    }
}
