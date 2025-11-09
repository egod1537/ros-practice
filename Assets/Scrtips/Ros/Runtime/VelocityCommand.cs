using RosMessageTypes.Geometry;

namespace Main
{
    public struct VelocityCommand
    {
        public float linearX;
        public float angularZ;

        public VelocityCommand(TwistMsg twistMsg)
        {
            this.linearX = (float)twistMsg.linear.x;
            this.angularZ = -(float)twistMsg.angular.z;
        }
    }
}