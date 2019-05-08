using System;
using System.Collections.Generic;
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
/*
using System.Windows.Forms;

using MarioME.BKGObjects;
using MarioME.TextureObjects;
*/

namespace MarioME.Helper
{      
    class PathClass
    {
        Path _path;

        public void Test_Path() {
            this._path = new Path();
            this._path.Add(new Vector2(ConvertUnits.ToSimUnits(32),ConvertUnits.ToSimUnits(32)));
            this._path.Add(new Vector2(ConvertUnits.ToSimUnits(32+512),ConvertUnits.ToSimUnits(32)));
            this._path.Closed = true;
        }

        public Path CreatePathOnXAxis(int Range, Body body){
            this._path = new Path();
            this._path.Add(new Vector2(body.Position.X, body.Position.Y));
            this._path.Add(new Vector2(body.Position.X + ConvertUnits.ToSimUnits(Range), body.Position.Y));
            this._path.Add(new Vector2(body.Position.X, body.Position.Y));
            this._path.Add(new Vector2(body.Position.X - ConvertUnits.ToSimUnits(Range), body.Position.Y));
            this._path.Closed = true;
            return this._path;
        }

        public Path CreatePathOnYAxis(int Range, Body body, bool DownFirst)
        {
            this._path = new Path();
            if (DownFirst)
            {
                this._path.Add(new Vector2(body.Position.X, body.Position.Y));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y + ConvertUnits.ToSimUnits(Range)));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y + ConvertUnits.ToSimUnits(Range)));
            }
            else if(!DownFirst){
                this._path.Add(new Vector2(body.Position.X, body.Position.Y));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y - ConvertUnits.ToSimUnits(Range)));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y - ConvertUnits.ToSimUnits(Range)));
            }
            this._path.Closed = true;
            return this._path;
        }

        public Path CreateRectanglePath(int width, Body body, int height, bool goesLeft) {
            this._path = new Path();

            if (!goesLeft) {
                this._path.Add(new Vector2(body.Position.X, body.Position.Y));
                //this._path.Add(new Vector2(body.Position.X, body.Position.Y));
                
                this._path.Add(new Vector2(body.Position.X+ConvertUnits.ToSimUnits(width), body.Position.Y));
                this._path.Add(new Vector2(body.Position.X + ConvertUnits.ToSimUnits(width), body.Position.Y));

                this._path.Add(new Vector2(body.Position.X + ConvertUnits.ToSimUnits(width), body.Position.Y+ConvertUnits.ToSimUnits(height)));
                this._path.Add(new Vector2(body.Position.X + ConvertUnits.ToSimUnits(width), body.Position.Y + ConvertUnits.ToSimUnits(height)));

                this._path.Add(new Vector2(body.Position.X, body.Position.Y + ConvertUnits.ToSimUnits(height)));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y + ConvertUnits.ToSimUnits(height)));

                this._path.Add(new Vector2(body.Position.X, body.Position.Y));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y));
            }

            return this._path;
        }

        public Path CreateSwingingPath(float radiusX, float radiusY, Body body, bool beginsRight) {
            this._path = new Path();
            if(beginsRight){
                this._path.Add(new Vector2(body.Position.X + ConvertUnits.ToSimUnits(radiusX), body.Position.Y));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y+ConvertUnits.ToSimUnits(radiusY)));
                this._path.Add(new Vector2(body.Position.X - ConvertUnits.ToSimUnits(radiusX), body.Position.Y));
            }else if(!beginsRight){
                this._path.Add(new Vector2(body.Position.X - ConvertUnits.ToSimUnits(radiusX), body.Position.Y));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y+ConvertUnits.ToSimUnits(radiusY)));
                this._path.Add(new Vector2(body.Position.X + ConvertUnits.ToSimUnits(radiusX), body.Position.Y));
            }
            this._path.Closed = true;
            return this._path;
        }

        public Path CreateSwingingAndReturnPath(float radiusX, float radiusY, Body body, bool beginsRight)
        {
            this._path = new Path();
            if (beginsRight)
            {
                this._path.Add(new Vector2(body.Position.X + ConvertUnits.ToSimUnits(radiusX), body.Position.Y));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y + ConvertUnits.ToSimUnits(radiusY)));
                this._path.Add(new Vector2(body.Position.X - ConvertUnits.ToSimUnits(radiusX), body.Position.Y));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y + ConvertUnits.ToSimUnits(radiusY)));
                this._path.Add(new Vector2(body.Position.X + ConvertUnits.ToSimUnits(radiusX), body.Position.Y));
            }
            else if (!beginsRight)
            {
                this._path.Add(new Vector2(body.Position.X - ConvertUnits.ToSimUnits(radiusX), body.Position.Y));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y + ConvertUnits.ToSimUnits(radiusY)));
                this._path.Add(new Vector2(body.Position.X + ConvertUnits.ToSimUnits(radiusX), body.Position.Y));
                this._path.Add(new Vector2(body.Position.X, body.Position.Y + ConvertUnits.ToSimUnits(radiusY)));
                this._path.Add(new Vector2(body.Position.X - ConvertUnits.ToSimUnits(radiusX), body.Position.Y));
            }
            this._path.Closed = true;
            return this._path;
        }

        // TODO Überarbeiten
        public Path CreateJumpingPath(float maxJumpHeight, float maxWidth, float jumpStep, Body body, bool goesRightFirst) {
            float cHeight = ConvertUnits.ToSimUnits(maxJumpHeight);
            float cWidth = ConvertUnits.ToSimUnits(maxWidth);
            float cStep = ConvertUnits.ToSimUnits(jumpStep);
            float bdyPosX = body.Position.X;
            float bdyPosY = body.Position.Y;

            int count = Convert.ToInt32(Math.Floor(Convert.ToDouble(maxWidth / jumpStep)));

            this._path = new Path();

            this._path.Add(new Vector2(bdyPosX, bdyPosY));

            if (goesRightFirst)
            {
                // Nach rechts
                for (int x = 0; x < count / 2; x++)
                {   
                    this._path.Add(new Vector2(bdyPosX+((jumpStep*x)/2), bdyPosY+maxJumpHeight));
                    this._path.Add(new Vector2(bdyPosX + jumpStep*x, bdyPosY));
                }
                // Zurück zur Mitte
                for (int x = count / 2; x > 0 ; x--)
                {
                    this._path.Add(new Vector2(bdyPosX - ((jumpStep * x) / 2), bdyPosY + maxJumpHeight));
                    this._path.Add(new Vector2(bdyPosX - jumpStep * x, bdyPosY));
                }
                //Nach Links
                for (int x = 0; x < count; x++)
                {
                    this._path.Add(new Vector2(bdyPosX - ((jumpStep * x) / 2), bdyPosY + maxJumpHeight));
                    this._path.Add(new Vector2(bdyPosX - jumpStep * x, bdyPosY));
                }
                // Zurück zur Mitte
                for (int x = count / 2; x > 0 ; x--)
                {
                    this._path.Add(new Vector2(bdyPosX + ((jumpStep * x) / 2), bdyPosY + maxJumpHeight));
                    this._path.Add(new Vector2(bdyPosX + jumpStep * x, bdyPosY));
                }
            }
            else {
                for (int x = 0; x < count / 2; x++)
                {

                }

                for (int x = 0; x < count / 2; x++)
                {

                }
            }

            this._path.Closed = true;
            return this._path;
        }

        public Path CreatePathFromList(List<Path> pathList) {
            this._path = new Path();

            foreach (Path p in pathList) {
                foreach (Vector2 v in p.ControlPoints)
                    this._path.Add(v);
            }
            this._path.Closed = true;
            return this._path;
        }

        public Path Path {
            get { return this._path; }
            set { this._path = value; }
        }

    }
}
