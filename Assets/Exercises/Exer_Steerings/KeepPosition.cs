using UnityEngine;

namespace Steerings
{

    public class KeepPosition : SteeringBehaviour
    {

        public GameObject target;
        public float requiredDistance;
        public float requiredAngle;

        /* COMPLETE */ 

        public override Vector3 GetLinearAcceleration()
        {
            /* COMPLETE */

            return KeepPosition.GetLinearAcceleration(Context, target, requiredDistance, requiredAngle);
        }

        public static Vector3 GetLinearAcceleration (SteeringContext me, GameObject target,
                                                     float distance, float angle)
        {
            /* COMPLETE */ 
            float desiredAngle = target.transform.rotation.eulerAngles.z + angle;
            Vector3 directionFromTarget = Utils.OrientationToVector(desiredAngle);
            Vector3 displacment = directionFromTarget * distance;
            Vector3 desiredPosition = target.transform.position + displacment;
            
            SURROGATE_TARGET.transform.position = desiredPosition;

            return Arrive.GetLinearAcceleration(me, SURROGATE_TARGET);
        }

    }
}