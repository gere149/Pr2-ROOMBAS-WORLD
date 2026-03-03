using UnityEngine;

namespace Steerings
{

    public class InterfereOA : SteeringBehaviour
    {

        // remove comments for steerings that must be provided with a target 
        // remove whole block if no explicit target required
        // (if FT or FTI policies make sense, then this method must be present)
        public GameObject target;
        public float requiredDistance;

        public override GameObject GetTarget()
        {
            return target;
        }

        public override Vector3 GetLinearAcceleration()
        {
            return InterfereOA.GetLinearAcceleration(Context, target, requiredDistance);
        }


        public static Vector3 GetLinearAcceleration(SteeringContext me, GameObject target, float requiredDistance)
        {
            Vector3 oaAcceleration = ObstacleAvoidance.GetLinearAcceleration(me);
            if (oaAcceleration.Equals(Vector3.zero))
                return Interfere.GetLinearAcceleration(me, target, requiredDistance);
            else
                return oaAcceleration;
        }
    }
}