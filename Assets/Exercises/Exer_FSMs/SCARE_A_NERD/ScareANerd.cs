using FSMs;
using UnityEngine;
using Steerings;
using UnityEditor.Purchasing;

[CreateAssetMenu(fileName = "ScareANerd", menuName = "Finite State Machines/ScareANerd", order = 1)]
public class ScareANerd : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private GHOST_Blackboard blackboard;
    private SteeringContext steeringContext;
    private Arrive arrive;
    private Pursue pursue;
    private float elapsedTime;
    private GameObject victim;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */
        blackboard = GetComponent<GHOST_Blackboard>();
        steeringContext = GetComponent<SteeringContext>();
        arrive = GetComponent<Arrive>();
        pursue = GetComponent<Pursue>();
        base.OnEnter(); // do not remove
    }

    public override void OnExit()
    {
        /* Write here the FSM exiting code. This code is execute every time the FSM is exited.
         * It's equivalent to the on exit action of any state 
         * Usually this code turns off behaviours that shouldn't be on when one the FSM has
         * been exited. */
        base.DisableAllSteerings();
        base.OnExit();
    }

    public override void OnConstruction()
    {
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         */
        State goCastle = new State("GoCastle",
            () => { steeringContext.maxSpeed *= 4; 
                arrive.target = blackboard.castle; 
                arrive.enabled = true;  }, // write on enter logic inside {}
            () => {}, // write in state logic inside {}
            () => {
                steeringContext.maxSpeed /= 4;
                arrive.enabled = false; }  // write on exit logic inisde {}  
        );

        State hide = new State("Hide",
            () => { elapsedTime = 0; }, // write on enter logic inside {}
            () => { elapsedTime += Time.deltaTime; }, // write in state logic inside {}
            () => {}  // write on exit logic inisde {}  
        );

        State selectTarget = new State("SelectTarget",
            () => { victim = SensingUtils.FindInstanceWithinRadius(gameObject, blackboard.victimLabel, blackboard.nerdDetectionRadius); }, // write on enter logic inside {}
            () => {}, // write in state logic inside {}
            () => {}  // write on exit logic inisde {}  
        );

        State approach = new State("approach",
            () => { pursue.target = victim; pursue.enabled = true; }, // write on enter logic inside {}
            () => {}, // write in state logic inside {}
            () => {}  // write on exit logic inisde {}  
        );

        State cryBoo = new State("cryBoo",
            () => {}, // write on enter logic inside {}
            () => {}, // write in state logic inside {}
            () => { pursue.target = null; pursue.enabled = false; }  // write on exit logic inisde {}  
        );

        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------
        */
        Transition castleReached = new Transition("CastleReached",
            () => { return SensingUtils.DistanceToTarget(gameObject, blackboard.castle) <= blackboard.castleReachedRadius; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition timeOut = new Transition("timeOut",
            () => { return elapsedTime >= blackboard.hideTime; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition targetSelected = new Transition("targetSelected",
            () => { return SensingUtils.FindRandomInstanceWithinRadius(gameObject, blackboard.victimLabel, blackboard.nerdDetectionRadius); }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition targetIsClose = new Transition("targetIsClose",
            () => { return SensingUtils.FindInstanceWithinRadius(gameObject, blackboard.victimLabel, blackboard.closeEnoughToScare); }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
        */    
        AddStates(goCastle, hide, selectTarget, approach, cryBoo);

        AddTransition(goCastle, castleReached, hide);
        AddTransition(hide, timeOut, selectTarget);
        AddTransition(selectTarget, targetSelected, approach);
        AddTransition(approach, targetIsClose, cryBoo);
        AddTransition(cryBoo, timeOut, goCastle);



        // STAGE 4: set the initial state

        initialState = goCastle;
    }
}
