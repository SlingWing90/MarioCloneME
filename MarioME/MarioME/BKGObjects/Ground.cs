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
    
    #region Ground
    class Ground : BKGObject
    {
        // Grade Wege
        int[] _leftBlock = new int[2];
        int[] _centerBlock = new int[2];
        int[] _rightBlock = new int[2];
        //int[] _upBlock = new int[2];
        int[] _downRightBlock = new int[2];
        int[] _downLeftBlock = new int[2];

        int[] _bottomLeftEndBlock = new int[2];
        int[] _bottomRightEndBlock = new int[2];

        int[] _bottomLeftDownBlock = new int[2];
        int[] _bottomRightDownBlock = new int[2];

        int[] _bottomCenterBlock = new int[2];

        // Untergrund
        int[] _leftBlankBlock = new int[2];
        int[] _centerBlankBlock = new int[2];
        int[] _rightBlankBlock = new int[2];
        int[] _downRightBlankBlock = new int[2];
        int[] _downLeftBlankBlock = new int[2];

        // Schraegen
        bool _hasBegin;
        bool _hasEnd;

        //bool _hasUp;
        bool _hasRightDown;
        bool _hasLeftDown;

        bool _hasBottom;
        /*  has Bottom*/

        int _blockCountX = 0;
        int _blockCountY = 0;

        //groundEnum _groundBeginTransform;
        //groundEnum _groundEndTransform;
        
        public Ground(World world, Vector2 position, float width, float height, Texture2D texture)
            : base(world, position, width, height, texture)
        {
            this.body.BodyType = BodyType.Static;
            this._hasBegin = false;
            this._hasEnd = false;
            //this._hasUp = false;
            this._hasRightDown = false;
            this._hasLeftDown = false;

            this._hasBottom = false;
            this._blockCountX = Convert.ToInt32(width / 32);
            this._blockCountY = Convert.ToInt32(height / 32);

            //this._groundBeginTransform = groundBeginTransform;
            //this._groundEndTransform = groundEndTransform;

            SetUpPhysics(world, position, width, height, 1);
        }

        protected override void SetUpPhysics(World world, Vector2 position, float width, float height, float mass)
        {
            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(width - _blockCountX * _blockCountX), ConvertUnits.ToSimUnits(height), mass, ConvertUnits.ToSimUnits(position));
            body.BodyType = BodyType.Static;
            body.Restitution = 0.3f;
            body.Friction = 0.5f;
            //body.FixedRotation = true;
            this.body.CollisionCategories = Category.Cat1;
            this.body.SleepingAllowed = false;
        }
        public void loadTileset(int lvlID)
        {
            switch (lvlID)
            {
                case 0:
                    // Boden
                    _leftBlock[0] = 4;
                    _leftBlock[1] = 108;
                    _centerBlock[0] = 38;
                    _centerBlock[1] = 108;
                    _rightBlock[0] = 72;
                    _rightBlock[1] = 108;
                    //_upBlock[0] = 174;
                    //_upBlock[1] = 107;
                    _downRightBlock[0] = 140;
                    _downRightBlock[1] = 108;
                    _downLeftBlock[0] = 106;
                    _downLeftBlock[1] = 108;

                    // Blanks
                    _leftBlankBlock[0] = 4;
                    _leftBlankBlock[1] = 142;
                    _centerBlankBlock[0] = 38;
                    _centerBlankBlock[1] = 142;
                    _rightBlankBlock[0] = 72;
                    _rightBlankBlock[1] = 142;
                    _downRightBlankBlock[0] = 140;
                    _downRightBlankBlock[1] = 142;
                    _downLeftBlankBlock[0] = 106;
                    _downLeftBlankBlock[1] = 142;

                    // Bottom
                    _bottomCenterBlock[0] = 38;
                    _bottomCenterBlock[1] = 210;

                    _bottomLeftEndBlock[0] = 4;
                    _bottomLeftEndBlock[1] = 210;

                    _bottomLeftDownBlock[0] = 106;
                    _bottomLeftDownBlock[1] = 210;

                    _bottomRightDownBlock[0] = 140;
                    _bottomRightDownBlock[1] = 210;

                    _bottomRightEndBlock[0] = 72;
                    _bottomRightEndBlock[1] = 210;
                    break;
            }

            //MessageBox.Show(ConvertUnits.ToDisplayUnits(this.body.Position.X).ToString());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < this._blockCountX; x++)
            {

                for (int y = 0; y <= this._blockCountY; y++)
                {
                    if (y == 0)
                    {
                        if(x > 0 && x < this._blockCountX-1)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, ((int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2) /*- 16*/), /*(int)width*/32, /*(int)height*/32), new Rectangle(_centerBlock[0], _centerBlock[1], 32, 32), Color.White);

                        if (this._hasBegin && x == 0)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height)*/32), new Rectangle(_leftBlock[0], _leftBlock[1], 32, 32), Color.White);

                        if (this._hasEnd && x == this._blockCountX - 1)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_rightBlock[0], _rightBlock[1], 32, 32), Color.White);

                        //if (this._hasUp && x == this._blockCountX - 1)
                        //    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2), (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_upBlock[0], _upBlock[1], 32, 32), Color.White);

                        if (this._hasRightDown && x == this._blockCountX - 1)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_downRightBlock[0], _downRightBlock[1], 32, 32), Color.White);

                        if (this._hasLeftDown && x == 0)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_downLeftBlock[0], _downLeftBlock[1], 32, 32), Color.White);

                        // Blank
                        if ((!this._hasLeftDown && !this._hasBegin))
                        {
                            if(x == 0)
                                spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, ((int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2) /*- 16*/), /*(int)width*/32, /*(int)height*/32), new Rectangle(_centerBlock[0], _centerBlock[1], 32, 32), Color.White);   
                        }

                        if ((!this._hasRightDown && !this._hasEnd)) { 
                           if(x == this._blockCountX -1)
                                spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, ((int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2) /*- 16*/), /*(int)width*/32, /*(int)height*/32), new Rectangle(_centerBlock[0], _centerBlock[1], 32, 32), Color.White);   
                        }
                    }
                    else
                    {
                        if (x > 0 && x < this._blockCountX - 1)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1- _blockCountX, ((int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2) /*- 16*/), /*(int)width*/32, /*(int)height*/32), new Rectangle(_centerBlankBlock[0], _centerBlankBlock[1], 32, 32), Color.White);

                        if (this._hasBegin && x == 0)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height)*/32), new Rectangle(_leftBlankBlock[0], _leftBlankBlock[1], 32, 32), Color.White);

                        if (this._hasEnd && x == this._blockCountX - 1)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_rightBlankBlock[0], _rightBlankBlock[1], 32, 32), Color.White);

                        //if (this._hasUp && x == this._blockCountX - 1)
                        //    spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2), (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_upBlock[0], _upBlock[1], 32, 32), Color.White);

                        if (this._hasRightDown && x == this._blockCountX - 1)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_downRightBlankBlock[0], _downRightBlankBlock[1], 32, 32), Color.White);

                        if (this._hasLeftDown && x == 0)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_downLeftBlankBlock[0], _downLeftBlankBlock[1], 32, 32), Color.White);

                        if (this._hasBottom && y == this._blockCountY && x >= 1 && x < this._blockCountX - 1)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_bottomCenterBlock[0], _bottomCenterBlock[1], 32, 32), Color.White);

                        if (this._hasBottom && this._hasBegin && y == this._blockCountY && x == 0)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_bottomLeftEndBlock[0], _bottomLeftEndBlock[1], 32, 32), Color.White);

                        if (this._hasBottom && this._hasLeftDown && y == this._blockCountY && x == 0)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_bottomLeftDownBlock[0], _bottomLeftDownBlock[1], 32, 32), Color.White);
                        
                        if (this._hasBottom && this._hasEnd && y == this._blockCountY && x == this._blockCountX - 1)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_bottomRightEndBlock[0], _bottomRightEndBlock[1], 32, 32), Color.White);

                        if (this._hasBottom && this._hasRightDown && y == this._blockCountY && x == this._blockCountX - 1)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, (int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2), /*(int)width*/32, /*(int)height*/32), new Rectangle(_bottomRightDownBlock[0], _bottomRightDownBlock[1], 32, 32), Color.White);
                        
                        // Blank
                        if ((!this._hasLeftDown && !this._hasBegin) && x == 0)
                            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, ((int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2) /*- 16*/), /*(int)width*/32, /*(int)height*/32), new Rectangle(_centerBlankBlock[0], _centerBlankBlock[1], 32, 32), Color.White);

                        if((!this._hasRightDown && !this._hasEnd) && x == this._blockCountX -1)
                               spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(this.body.Position.X) + 32 * x - (int)(this.width / 2) - x * 1 - _blockCountX, ((int)ConvertUnits.ToDisplayUnits(this.body.Position.Y) + 32 * y - (int)(this.height / 2) /*- 16*/), /*(int)width*/32, /*(int)height*/32), new Rectangle(_centerBlankBlock[0], _centerBlankBlock[1], 32, 32), Color.White);
                        
                    }
                    ///////
                }
            }
        }

        public void Kill(World w) {
            //w.RemoveBody(this.body);
            //w.BodyList.Remove(this.body);
            this.texture = null;
            this.body.Dispose();
            //this.texture.Dispose();
        }

        public bool HasBegin
        {
            get { return this._hasBegin; }
            set { this._hasBegin = value; }
        }

        public bool HasEnd
        {
            get { return this._hasEnd; }
            set { this._hasEnd = value; }
        }

        public bool HasRightDown
        {
            get { return this._hasRightDown; }
            set { this._hasRightDown = value; }
        }

        public bool HasLeftDown
        {
            get { return this._hasLeftDown; }
            set { this._hasLeftDown = value; }
        }

        public bool HasBottom
        {
            get { return this._hasBottom; }
            set { this._hasBottom = value; }
        }

    }
    #endregion
    /*
    #region GroundBevel
    class GroundBevel : BKGObject {
        int _blockCountX;
        int _blockCountY;

        bool _goesUp;
        bool _goesRight;

        int[] _bevelUp = new int[2];
        int[] _bevelDown = new int[2];

        float _blankHeight;

        Vector2[] bevelVector = new Vector2[3];
        int _differenceWidth;
        int _differenceHeight;
        
        public GroundBevel(World world, Vector2 position, float width, float height, float blankHeight, Texture2D texture):base(world, position, width, height, texture){
            if (width != height)
                throw new Exception("Höhe und Breite müssen gleich sein");
            
            if (position.X < width)
                _goesRight = true;
            else _goesRight = false;

            if (position.Y < height)
                _goesUp = false;
            else _goesUp = true;

            _differenceWidth = (int)(this.Position.X - this.width);
            _differenceHeight = 96;// (int)(this.Position.Y - this.height);

            if (_differenceWidth < 0)
                _differenceWidth = _differenceWidth * -1;

            if (_differenceHeight < 0)
                _differenceHeight = _differenceHeight * -1;

            _blockCountX = _differenceWidth / 32;
            _blockCountY = _differenceHeight / 32;
            
            _blankHeight = blankHeight;

            bevelVector[0] = new Vector2(0, 0);
            bevelVector[1] = new Vector2(0, 96);
            bevelVector[2] = new Vector2(96, 96);

            SetUpPhysics(world, position, width, height, 1f);            
        }

        public void loadTileset(int lvlID) {
            switch (lvlID) { 
                case 0:
                    _bevelUp[0] = 174; 
                    _bevelUp[1] = 176;
                    _bevelDown[0] = 208;
                    _bevelDown[1] = 176;
                    break;
            }
        }

        protected override void SetUpPhysics(World world, Vector2 position, float width, float height, float mass)
        {
            //Vector2 displayPosition = ConvertUnits.ToDisplayUnits(position);
            //MessageBox.Show("X: " + position.X + " Y: " + position.Y + " W: " + width+ " H: "+height);
            //body = BodyFactory.
            //body = BodyFactory.CreateLoopShape(world, new Vertices(bevelVector));
            body = BodyFactory.CreateEdge(world, ConvertUnits.ToSimUnits(new Vector2(position.X, position.Y)), ConvertUnits.ToSimUnits(width, height));
            body.CollisionCategories = Category.Cat7;
            //body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(width - _blockCountX), ConvertUnits.ToSimUnits(height), mass, ConvertUnits.ToSimUnits(position));
            body.BodyType = BodyType.Static;
            body.Restitution = 0.3f;
            body.Friction = 5.5f;
            body.Rotation = 0f;
            //this.body.CollisionCategories = Category.Cat1;   
        }

        public override void Draw(SpriteBatch spriteBatch)
        {*/
            /* 3   3
             * x | y
             * 3 | 0
             * 2 | 1
             * 1 | 2
             */
            //if (_goesUp) {
    /*spriteBatch.Draw(this.texture, new Rectangle(0, 0, 32, 32), new Rectangle(_bevelUp[0], _bevelUp[1], 32, 32), Color.Blue, 0f, new Vector2(32, 32), SpriteEffects.None, 0f);
                        
        for (int x = 0; x < _blockCountX; x++) {
            for (int y = 0; y < _blockCountY; y++) {
                if (x + y == _blockCountX) {
                    spriteBatch.Draw(this.texture, this.Position, new Rectangle(_bevelUp[0], _bevelUp[1], 32, 32), Color.White, 0f, new Vector2(32, 32), 0f, SpriteEffects.None, 0f);
                }
            }
        }
    //}            
}
}
#endregion*/
}
