using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MarioME
{
    public class CompositePhysicsObject
    {
        protected PhysicsObject physObA, physObB;
        protected RevoluteJoint revJoint;

        public CompositePhysicsObject(World world, PhysicsObject physObA, PhysicsObject physObB, Vector2 relativeJointPosition)
        {
            this.physObA = physObA;
            this.physObB = physObB;
            revJoint = JointFactory.CreateRevoluteJoint(world, physObA.body, physObB.body, ConvertUnits.ToSimUnits(relativeJointPosition));
            physObA.body.IgnoreCollisionWith(physObB.body);
            physObB.body.IgnoreCollisionWith(physObA.body);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            physObA.Draw(spriteBatch);
            physObB.Draw(spriteBatch);
        }
    }
}