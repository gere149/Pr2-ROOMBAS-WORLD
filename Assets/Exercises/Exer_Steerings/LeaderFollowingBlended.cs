using UnityEngine;

namespace Steerings
{

    public class LeaderFollowingBlended : SteeringBehaviour
    {
        
        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        public float wlr = 0.5f;

        public override GameObject GetTarget()
        {
            return target;
        }
      
        
        public override Vector3 GetLinearAcceleration()
        {
            /* COMPLETE */
            return LeaderFollowingBlended.GetLinearAcceleration(Context, target, requiredDistance, requiredAngle, wlr);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float requiredDistance, float requiredAngle, float wlr)
        {
            /*
             Compute both steerings
                lr = LinearRepulsion.GetLinearAcceleration(...)
                kp = KeepPosition...
             - if lr is zero return kp
             - else return the blending of lr and kp
             */

            /* COMPLETE */
            Vector3 lr = LinearRepulsion.GetLinearAcceleration(me);
            Vector3 kp = KeepPosition.GetLinearAcceleration(me, target, requiredDistance, requiredAngle);

            if(lr.Equals(Vector2.zero))
                return kp;
            else
                return lr * wlr + kp * (1- wlr);
        }
    }
}