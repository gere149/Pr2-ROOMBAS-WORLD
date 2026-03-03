using UnityEngine;
using BTs;

[CreateAssetMenu(fileName = "BT_one", menuName = "Behaviour Trees/BT_one", order = 1)]
public class BT_one : BehaviourTree
{
    
    public override void OnConstruction()
    {
        root = new Sequence();

        root.AddChild(new ACTION_Arrive("home"));
        root.AddChild(new ACTION_Arrive("gym"));
        root.AddChild(new ACTION_Somersault());
        root.AddChild(new ACTION_PlaySound("impactSound"));
    }
}
