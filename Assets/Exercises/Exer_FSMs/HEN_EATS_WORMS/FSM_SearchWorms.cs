using FSMs;
using Steerings;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_SearchWorms", menuName = "Finite State Machines/FSM_SearchWorms", order = 1)]
public class FSM_SearchWorms : FiniteStateMachine
{
    /* Declare here, as attributes, all the variables that need to be shared among
     * states and transitions and/or set in OnEnter or used in OnExit 
     * For instance: steering behaviours, blackboard, ...*/

    private HEN_Blackboard blackboard;
    private WanderAround wanderAround;
    private Arrive arrive;
    private AudioSource audioSource;
    private GameObject theWorm;
    private float elapsedTime;

    public override void OnEnter()
    {
        /* Write here the FSM initialization code. This code is execute every time the FSM is entered.
         * It's equivalent to the on enter action of any state 
         * Usually this code includes .GetComponent<...> invocations */

        /* COMPLETE */
        blackboard = GetComponent<HEN_Blackboard>();
        wanderAround = GetComponent<WanderAround>();
        arrive = GetComponent<Arrive>();
        audioSource = GetComponent<AudioSource>();
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
        /* COMPLETE */
        
        /* STAGE 1: create the states with their logic(s)
         *-----------------------------------------------
         */
        State wander = new State("wander",
            () => { wanderAround.enabled = true; audioSource.clip = blackboard.cluckingSound;
                audioSource.Play();
            }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { wanderAround.enabled = false; audioSource.Stop(); }  // write on exit logic inisde {}  
        );

        State reachWorm = new State("reachWorm",
            () => { arrive.target = theWorm;  arrive.enabled = true; }, // write on enter logic inside {}
            () => { }, // write in state logic inside {}
            () => { arrive.enabled = false; arrive.target = null; }  // write on exit logic inisde {}  
        );

        State eat = new State("eat",
            () => { elapsedTime = 0; audioSource.clip = blackboard.eatingSound;
                audioSource.Play();
            }, // write on enter logic inside {}
            () => { elapsedTime += Time.deltaTime; }, // write in state logic inside {}
            () => {audioSource.Stop();
                Object.Destroy( theWorm );
            }  // write on exit logic inisde {}  
        );




        /* STAGE 2: create the transitions with their logic(s)
         * ---------------------------------------------------
        */
        Transition wormDetected = new Transition("wormDetected",
            () => { theWorm = SensingUtils.FindInstanceWithinRadius(gameObject, "WORM", blackboard.wormDetectableRadius);
                return theWorm;
            }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition wormReached = new Transition("wormReached",
            () => {return SensingUtils.DistanceToTarget(gameObject, theWorm) <= blackboard.wormReachedRadius; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition wormVanished = new Transition("wormVanished",
            () => { return theWorm == null; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );

        Transition TimeOut = new Transition("TimeOut",
            () => { return elapsedTime >= blackboard.timeToEatWorm; }, // write the condition checkeing code in {}
            () => { }  // write the on trigger code in {} if any. Remove line if no on trigger action needed
        );


        /* STAGE 3: add states and transitions to the FSM 
         * ----------------------------------------------
        */
        AddStates(wander, reachWorm, eat);
        AddTransition(wander, wormDetected, reachWorm);
        AddTransition(reachWorm, wormVanished, wander);
        AddTransition(reachWorm, wormReached, eat);
        AddTransition(eat, TimeOut, wander);



        // STAGE 4: set the initial state

        initialState = wander;
    }
}
