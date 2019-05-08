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

namespace MarioME.Helper
{
    class CameraEffect
    {
        float _time = 0;
        int count;
        public void ShakeCamera(Camera2D cam, float speed, int times) {
            
            count++;

            if (count <= times) {
                _time += speed;// 0.25f;

                if (_time > 1)
                    _time = -1f;

                cam._pos = new Vector2(cam.Pos.X, cam._pos.Y - _time);
            }
        }
    }
}
