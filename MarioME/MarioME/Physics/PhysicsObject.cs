using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarioME
{
    public class PhysicsObject
    {
        public Body body;
        protected float height;
        protected Vector2 origin;
        protected Texture2D texture;
        protected float width;

        public PhysicsObject(World world, Vector2 position, float width, float height, float mass, Texture2D texture)
        {
            this.texture = texture;
            origin = new Vector2(texture.Width/2, texture.Height/2); //
            this.width = width;//*2;
            this.height = height;



            SetUpPhysics(world, position, width, height, mass);
        }

        public virtual Vector2 Position
        {
            get { return body.Position; }
        }

        protected virtual void SetUpPhysics(World world, Vector2 position, float width, float height, float mass)
        {//ToSimUnits
            //body = BodyFactory.Crea
            body = BodyFactory.CreateRectangle(world, ConvertUnits.ToSimUnits(width), ConvertUnits.ToSimUnits(height), mass, ConvertUnits.ToSimUnits(position));
            body.BodyType = BodyType.Dynamic;
            body.Restitution = 0.3f;
            body.Friction = 0.5f;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)ConvertUnits.ToDisplayUnits(body.Position.X), (int)ConvertUnits.ToDisplayUnits(body.Position.Y), (int)width, (int)height), null, Color.White, body.Rotation, origin,
                             SpriteEffects.None, 0f);
        }

        private int round(float toRound)
        {
            float remainder = toRound - (int) toRound;
            if (remainder >= 0.5)
                return ((int) toRound + 1);
            else
                return (int) toRound;
        }
    }
}