using UnityEngine;

namespace Steerings
{

    public class Interfere : SteeringBehaviour
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
            return Interfere.GetLinearAcceleration(Context, target, requiredDistance);
        }

        
        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target, float requiredDistance)
        {
            /* COMPLETE this method. It must return the linear acceleration (Vector3) */
            SteeringContext targetVelocity = target.GetComponent<SteeringContext>();
            Vector3 velocity = targetVelocity.velocity.normalized;
            Vector3 displacementFromTarget = velocity * requiredDistance;
            Vector3 acceleration = targetVelocity.transform.position + displacementFromTarget;


            SURROGATE_TARGET.transform.position = acceleration;

            return Seek.GetLinearAcceleration(me, SURROGATE_TARGET);
        }

    }
}