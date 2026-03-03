using FSMs;
using UnityEngine;
using Steerings;

[CreateAssetMenu(fileName = "FSM_TwoPointWandering", menuName = "Finite State Machines/FSM_TwoPointWandering", order = 1)]
public class FSM_TwoPointWandering : FiniteStateMachine
{

    private WanderAround wanderAround;
    private SteeringContext steeringContext;
    private ANT_Blackboard blackboard;

    private float elapsedTime = 0;


    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is executed every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        /* COMPLETE */
        blackboard = GetComponent<ANT_Blackboard>();
        wanderAround = GetComponent<WanderAround>();
        steeringContext = GetComponent<SteeringContext>();
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */

        /* COMPLETE */

        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         */

        State goingA = new State("Going_A",
           () => { /* COMPLETE */
               elapsedTime = 0;
               wanderAround.attractor = blackboard.pointA;
               wanderAround.enabled = true;
           },
           () => { elapsedTime += Time.deltaTime;}, 
           () => {/* COMPLETE */
               wanderAround.enabled = false;
           }
       );

        State goingB = new State("Going_B",
           () => {/* COMPLETE */
               elapsedTime = 0;
               wanderAround.attractor = blackboard.pointB;
               wanderAround.enabled = true;
           },
           () => { elapsedTime += Time.deltaTime; },
           () => { /* COMPLETE */ 
                wanderAround.enabled= false; 
           }
       );


        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------
        */

        
        Transition pointAReached = new Transition("Point A Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.pointA) <= blackboard.loactionReachedRadius; }, // write the condition checkeing code in {}
            () => { steeringContext.seekWeight = blackboard.initialSeekWeight; }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition pointBReached = new Transition("Point B Reached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.pointB) <= blackboard.loactionReachedRadius; }, // write the condition checkeing code in {}
            () => { steeringContext.seekWeight = blackboard.initialSeekWeight; }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition timeOut = new Transition("timeOut",
            () => { return elapsedTime >= blackboard.timeBetweenTimeOuts; }, // write the condition checkeing code in {}
            () => { steeringContext.seekWeight += blackboard.seekIncrement; elapsedTime = 0; }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );


        /* COMPLETE, create the transitions */

        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
         */

        AddStates(goingA, goingB);
        AddTransition(goingA, pointAReached, goingB);
        AddTransition(goingB, pointBReached, goingA);
        AddTransition(goingA, timeOut, goingA);
        AddTransition(goingB, timeOut, goingB);

        /* COMPLETE, add the transitions */

        /* STAGE 4: set the initial state */

        initialState = goingA;
    }
}
