
using UnityEngine;

public class ANT_Blackboard : MonoBehaviour
{
    

    [Header("Two point wandering")]
    public GameObject pointA;
    public GameObject pointB;
    public float timeBetweenTimeOuts = 10f;
    public float initialSeekWeight = 0.2f;
    public float seekIncrement = 0.2f;
    public float loactionReachedRadius = 10f;

    [Header("Seed colecting")]
    public GameObject nest;
    public float seedDetectionRadius = 100f;
    public float seedReachedRadius = 5f;
    public float nestReachedRadius = 20f;

    //[Header("Peril Fleeing")]


    void Start()
    {

    }

   
}
