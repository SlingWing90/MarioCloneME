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
using System.Windows.Forms;

namespace MarioME.BossObjects
{
    class BossObject
    {
        enum EnemyState { 
            Attacking= 0,
            Idle = 1,
            Searching = 2,
            Walking = 3,
            Hitted = 4
        }
        Texture2D _texture;

        protected Body _body;
        protected Rectangle _visualRectangle;
        protected int _width;
        protected int _height;
        protected int _density;

        //protected EnemyState enemyState;

        protected Rectangle _rect;

        public BossObject(World world, Texture2D texture, Vector2 position)
        {
            //this.enemyState = EnemyState.Walking;
            this._texture = texture;
        }

        protected virtual void InitPhysic(World world, Vector2 position)
        {
            this._body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(this._width), ConvertUnits.ToSimUnits(this._height), this._density);
            this._body.CollisionCategories = Category.Cat4;
            this._body.Position = ConvertUnits.ToSimUnits(position);
            this._body.BodyType = BodyType.Dynamic;
        }

        public virtual void SeekForPlayerBody(Body PlayerBody) {
            float pBdyX = ConvertUnits.ToDisplayUnits(PlayerBody.Position.X);
            float pBdyY = ConvertUnits.ToDisplayUnits(PlayerBody.Position.Y);

            float eBdyX = ConvertUnits.ToDisplayUnits(this._body.Position.X);
            float eBdyY = ConvertUnits.ToDisplayUnits(this._body.Position.Y); 
        }

        public virtual void Update(GameTime gameTime) { 
            //Update Visual Rectangle
        }
    }
}
