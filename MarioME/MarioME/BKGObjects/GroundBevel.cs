/*
 * TODO Ist groundEnum.Left notwendig?
 */

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

using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using FarseerPhysics.Common;

using System.Windows.Forms;


namespace MarioME.BKGObjects
{
    enum groundEnum
    {
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }

    class GroundBevel: BKGObject
    {
        float _width;
        float _height;

        float _blankWidth;
        float _blankHeigth;

        int _blockCountX;
        int _blockCountY;

        int _blockBlankCountY;

        Vector2 _startPos; 

        groundEnum directionUpDown;
        groundEnum directionLeftRight;
        SpriteEffects directionEffect;

        Rectangle[] _rect;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="world">Physic-Welt</param>
        /// <param name="position">StartVektor</param>
        /// <param name="width">EndPos-X</param>
        /// <param name="height">EndPos-Y</param>
        /// <param name="blankHeight">Höhe des leerfeldes</param>
        /// <param name="texture">Textur</param>
        public GroundBevel(World world, Vector2 position, float width, float height, float blankHeight ,Texture2D texture) : base(world, position, width, height, texture) {
            _rect = new Rectangle[4];
            this._startPos = position;

            this._width = position.X - width;
            if (this._width < 0)
                this._width *= -1;

            this._height = position.Y - height;
            if (this._height < 0)
                this._height *= -1;

            if (position.Y < height)
                directionUpDown = groundEnum.Down;
            else
                directionUpDown = groundEnum.Up;

            if (position.X < width)
                directionLeftRight = groundEnum.Right;
            else
                directionLeftRight = groundEnum.Left;

            this._blockCountX = (int)this._width / 32;
            this._blockCountY = (int)this._height / 32;

            this._blockBlankCountY = (int)blankHeight / 32;

            SetUpPhysics(world, position, width, height, 1f);
        }

        public void loadTiles(int tileID){
            switch (tileID) { 
                case 0:
                    if(directionUpDown == groundEnum.Up){
                        _rect[0] = new Rectangle(174, 176, 32, 32); //Hoch
                        _rect[1] = new Rectangle(174, 107, 32, 32); // Bevel Blank Links
                    }
                    else if (directionUpDown == groundEnum.Down)
                    {
                        _rect[0] = new Rectangle(208, 176, 32, 32); //Runter
                        _rect[1] = new Rectangle(276, 107, 32, 32); // Bevel Blank Rechts
                    }
                    _rect[2] = new Rectangle(38, 142, 32, 32); // NormalBlank
                break;
            }
        }

        protected override void SetUpPhysics(World world, Vector2 position, float width, float height, float mass)
        {
            this.body = BodyFactory.CreateEdge(world, new Vector2(ConvertUnits.ToSimUnits(position.X), ConvertUnits.ToSimUnits(position.Y)), new Vector2(ConvertUnits.ToSimUnits(width), ConvertUnits.ToSimUnits(height)));
            this.body.BodyType = BodyType.Static;
            this.body.Friction = 1.6f;
            this.body.CollisionCategories = Category.Cat7;
            //base.SetUpPhysics(world, position, width, height, mass);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (directionLeftRight == groundEnum.Right) {
                if (directionUpDown == groundEnum.Up)
                {
                    #region Rechts Hoch
                    for (int x = this._blockCountY; x > 0; x--)
                    {
                        for (int y = 0; y < this._blockCountY; y++)
                        {
                            if (y + x == _blockCountX)
                            {
                                if (y > 0)
                                    spriteBatch.Draw(this.texture, new Rectangle((int)_startPos.X + x * 32 - 8 + 32, (int)_startPos.Y + y * 32 - 64, 32, 32), _rect[1], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);

                                spriteBatch.Draw(this.texture, new Rectangle((int)_startPos.X + x * 32 - 8, (int)_startPos.Y + y * 32 - 64, 32, 32), _rect[0], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                            }

                            if (y == _blockCountY - 1 && x == _blockCountX - 1)
                                spriteBatch.Draw(this.texture, new Rectangle((int)_startPos.X + x * 32 - 8 + 32, (int)_startPos.Y + y * 32 - 64, 32, 32), _rect[2], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);

                        }
                    }
                    #endregion
                }
                else if (directionUpDown == groundEnum.Down)
                {
                    #region Rechts Runter
                    for (int x =0 ; x < this._blockCountY; x++)
                    {
                        for (int y = 0; y < this._blockCountY; y++)
                        {
                            if (y == x)
                            {
                                if (y > 0)
                                    spriteBatch.Draw(this.texture, new Rectangle((int)_startPos.X + x * 32, (int)_startPos.Y + y * 32 +32, 32, 32), _rect[1], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);

                                spriteBatch.Draw(this.texture, new Rectangle((int)_startPos.X + x * 32+32, (int)_startPos.Y + y * 32+32, 32, 32), _rect[0], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                            }

                            if (y == _blockCountY - 1 && x == _blockCountX - 1)
                                spriteBatch.Draw(this.texture, new Rectangle((int)_startPos.X + x * 32-32, (int)_startPos.Y + y * 32+32, 32, 32), _rect[2], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);

                        }
                    }
                    #endregion
                }
            }
            ///// Blank Filler
            for (int x = 0; x < this._blockCountX; x++) {
                for (int y = 0; y < this._blockBlankCountY; y++) {
                    if (y == 0)
                    {
                        if (directionUpDown == groundEnum.Up)
                        {
                            if (directionLeftRight == groundEnum.Right)
                            {
                                if (x == 0)
                                    spriteBatch.Draw(this.texture, new Rectangle((int)_startPos.X + x * 32 + 32 - 8, (int)_startPos.Y + (_blockCountX * 32) + (y * 32) - 64, 32, 32), _rect[1], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                                else
                                    spriteBatch.Draw(this.texture, new Rectangle((int)_startPos.X + x * 32 + 32 - 8, (int)_startPos.Y + (_blockCountX * 32) + (y * 32) - 64, 32, 32), _rect[2], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                            }
                        }
                        else if (directionUpDown == groundEnum.Down)
                        {    
                            if (directionLeftRight == groundEnum.Right) {
                                if (x == _blockCountX-1)
                                    spriteBatch.Draw(this.texture, new Rectangle((int)_startPos.X + x * 32+32, (int)_startPos.Y + (_blockCountX * 32) + (y * 32)+32, 32, 32), _rect[1], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                                else
                                    spriteBatch.Draw(this.texture, new Rectangle((int)_startPos.X + x * 32+32, (int)_startPos.Y + (_blockCountX * 32) + (y * 32)+32, 32, 32), _rect[2], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                            }
                        }
                    }
                    else
                    {
                        if (directionUpDown == groundEnum.Up)
                        {
                            if(directionLeftRight == groundEnum.Right)
                                spriteBatch.Draw(this.texture, new Rectangle((int)_startPos.X + x * 32 + 32 - 8, (int)_startPos.Y + (_blockCountX * 32) + (y * 32) - 64, 32, 32), _rect[2], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                        }
                        else if (directionUpDown == groundEnum.Down) { 
                            if(directionLeftRight == groundEnum.Right)
                                spriteBatch.Draw(this.texture, new Rectangle((int)_startPos.X + x * 32+32, (int)_startPos.Y + (_blockCountX * 32) + (y * 32)+32, 32, 32), _rect[2], Color.White, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);        
                        }
                    }
                }
            }
             
        }

        public override void Kill()
        {
            base.Kill();
        }
    }
} 