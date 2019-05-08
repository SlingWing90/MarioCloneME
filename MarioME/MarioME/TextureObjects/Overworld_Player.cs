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

using System.Windows.Forms;

namespace MarioME.TextureObjects
{
    class Overworld_Player
    {
        Texture2D _texture;
        Vector2 _position;
        bool _isMoving;
        Vector2 _newPosition;
        int _frame;

        bool _walksUp;
        bool _walksDown;
        bool _walksLeft;
        bool _walksRight;

        Rectangle[] _walkDown;
        Rectangle[] _walkRight;
        Rectangle[] _walkUp;
        Rectangle[] _walkRect;

        public Overworld_Player(Texture2D texture, Vector2 position) {
            _texture = texture;
            _position = position;
            _newPosition = position;
            _isMoving = false;

            _walkDown = new Rectangle[8];
            _walkRight= new Rectangle[8];
            _walkUp = new Rectangle[8];
            _walkRect = new Rectangle[8];

            _walksDown = false;
            _walksRight = false;
            _walksUp = false;
            _walksLeft = false;

            loadTileset();
        }

        void loadTileset() {
            _walkDown[0] = new Rectangle(10, 12, 16, 32);
            _walkDown[1] = new Rectangle(31, 12, 16, 32);
            _walkDown[2] = new Rectangle(52, 12, 16, 32);
            _walkDown[3] = new Rectangle(73, 12, 16, 32);
            _walkDown[4] = new Rectangle(94, 12, 16, 32);
            _walkDown[5] = new Rectangle(115, 12, 16, 32);
            _walkDown[6] = new Rectangle(136, 12, 16, 32);
            _walkDown[7] = new Rectangle(157, 12, 16, 32);


            _walkRight[0] = new Rectangle(10, 55, 16, 32);
            _walkRight[1] = new Rectangle(31, 55, 16, 32);
            _walkRight[2] = new Rectangle(52, 55, 16, 32);
            _walkRight[3] = new Rectangle(73, 55, 16, 32);
            _walkRight[4] = new Rectangle(94, 55, 16, 32);
            _walkRight[5] = new Rectangle(115, 55, 16, 32);
            _walkRight[6] = new Rectangle(136, 55, 16, 32);
            _walkRight[7] = new Rectangle(157, 55, 16, 32);

            _walkUp[0] = new Rectangle(10, 100, 16, 32);
            _walkUp[1] = new Rectangle(31, 100, 16, 32);
            _walkUp[2] = new Rectangle(52, 100, 16, 32);
            _walkUp[3] = new Rectangle(73, 100, 16, 32);
            _walkUp[4] = new Rectangle(94, 100, 16, 32);
            _walkUp[5] = new Rectangle(115, 100, 16, 32);
            _walkUp[6] = new Rectangle(136, 100, 16, 32);
            _walkUp[7] = new Rectangle(157, 100, 16, 32);

            _walksDown = true;

            //for (int x = 0; x < 8; x++)
            //{
            _walkRect = _walkDown;
            //}
        }

        public void Update(GameTime gameTime) {
            if (_position == _newPosition)
            {
                _frame = 0;
                this._isMoving = false;
                _walksDown = false;
                _walksRight = false;
                _walksUp = false;
                _walksLeft = false;
                _walkRect = _walkDown;
            }
            
            if (_position.X < _newPosition.X)
            {
                _position = new Vector2(_position.X + 1, _position.Y);
                _walksRight = true;
                _walkRect = _walkRight;
            }
            else if(_position.X > _newPosition.X){
                _position = new Vector2(_position.X - 1, _position.Y);
                _walksLeft = true;
                _walkRect = _walkRight;
            }
            else if (_position.Y > _newPosition.Y)
            {
                _position = new Vector2(_position.X, _position.Y-1);
                _walksUp = true;
                _walkRect = _walkUp;
            }
            else if(_position.Y < _newPosition.Y)
            {
                _position = new Vector2(_position.X, _position.Y+1);
                _walksDown = true;
                _walkRect = _walkDown;
            }

            if (_isMoving) {
                if (Math.Floor(gameTime.TotalGameTime.TotalMilliseconds) % 100 == 0) {
                    _frame++;
                    if (_frame >= 8)
                        _frame = 0;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch) {
            if(!_walksLeft)
                spriteBatch.Draw(this._texture, new Rectangle((int)_position.X, (int)_position.Y, 16, 32), _walkRect[_frame], Color.White, 0f, new Vector2(16,32), SpriteEffects.None, 0f);
            else
                spriteBatch.Draw(this._texture, new Rectangle((int)_position.X, (int)_position.Y, 16, 32), _walkRect[_frame], Color.White, 0f, new Vector2(16,32), SpriteEffects.FlipHorizontally, 0f);
        }

        public void Kill() {
            this._texture.Dispose();
        }

        public Vector2 Position
        {
            get { return this._position; }
            set { this._position = value; }
        }

        public Vector2 NewPosition {
            get { return this._newPosition; }
            set { this._newPosition = value; }
        }

        public bool IsMoving {
            get { return this._isMoving; }
            set { this._isMoving = value; }
        }
    }
}
