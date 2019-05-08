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

using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;
using System.Windows.Forms;

namespace MarioME.TrackObjects
{
    class Track_1: TrackObject
    {
        Vector2 _position;
        public Track_1(int tilesetID, ContentManager Content, World world, Texture2D texture, float Speed, Vector2 position) : base(tilesetID, Content, world, texture, Speed) {
            this._path = new Path();
            this._bdyPod.Position = new Vector2(ConvertUnits.ToSimUnits(position.X), ConvertUnits.ToSimUnits(position.Y));
            this._position = position;
            loadTrack();
        }

        void loadTrack() {
            this._path.Add(ConvertUnits.ToSimUnits(_position)); // Startpunkt
            
            this._path.Add(new Vector2(ConvertUnits.ToSimUnits(_position.X+256), ConvertUnits.ToSimUnits(_position.Y)));
            
            this._path.Add(new Vector2(ConvertUnits.ToSimUnits(_position.X + 256), ConvertUnits.ToSimUnits(_position.Y + 64)));
            
            this._path.Add(new Vector2(ConvertUnits.ToSimUnits(_position.X), ConvertUnits.ToSimUnits(_position.Y + 64)));
            
            this._path.Closed = true;
        }

    }
}
