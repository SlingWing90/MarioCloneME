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

namespace MarioME.ActionObjects
{
    class ActionObject:PhysicsObject
    {
        public ActionObject(World world, Vector2 position, float width, float height, float mass, Texture2D texture):base(world, position, width, height, mass, texture) {
               
        }

        public virtual void Update(GameTime gameTime) { 
        
        }

        public virtual void Draw(SpriteBatch spriteBatch) {
            
        }

    }
}
