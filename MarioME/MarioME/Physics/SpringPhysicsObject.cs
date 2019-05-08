using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace MarioME
{
    public class SpringPhysicsObject : CompositePhysicsObject
    {
        protected AngleJoint springJoint;

        public SpringPhysicsObject(World world, PhysicsObject physObA, PhysicsObject physObB, Vector2 relativeJointPosition, float springSoftness, float springBiasFactor)
            : base(world, physObA, physObB, relativeJointPosition)
        {
            springJoint = JointFactory.CreateAngleJoint(world, physObA.body, physObB.body);
            springJoint.TargetAngle = 0;
            springJoint.Softness = springSoftness;
            springJoint.BiasFactor = springBiasFactor;
        }
    }
}