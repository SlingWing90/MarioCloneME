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
    class Fence: TextureObject
    {
        public Fence(Vector2 position, int width, int height, Texture2D texture) : base(position, width, height, texture) {
            this._hasStart = true;
            this._hasEnd = true;

            this._startBlock = new int[2];
            this._endBlock = new int[2];
            this._centerBlock = new int[2];

        }

        

        public void loadTiles(int lvlID){
            switch (lvlID) { 
                case 0:
                    this._startBlock[0]= 332;
                    this._startBlock[1] = 220;

                    this._centerBlock[0] =366 ;
                    this._centerBlock[1] =220 ;

                    this._endBlock[0] = 400;
                    this._endBlock[1] = 220;
                    break;
            }
        }
    }
}
