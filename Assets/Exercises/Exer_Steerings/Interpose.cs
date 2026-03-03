using UnityEngine;

namespace Steerings
{
    public class Interpose : SteeringBehaviour
    {

        
        // remove comments for steerings that must be provided with a target 
        // remove whole block if no explicit target required
        // (if FT or FTI policies make sense, then this method must be present)
        public GameObject target;
        public GameObject otherTarget;

        /*public override GameObject GetTarget()
        {
            return target;
        }*/
        
        public override Vector3 GetLinearAcceleration()
        {
            return Interpose.GetLinearAcceleration(Context, target, otherTarget);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, GameObject otherTarget)
        {
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
            Vector3 direction = (target.transform.position + otherTarget.transform.position) / 2;

            SURROGATE_TARGET.transform.position = direction;
            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
        }

    }
}