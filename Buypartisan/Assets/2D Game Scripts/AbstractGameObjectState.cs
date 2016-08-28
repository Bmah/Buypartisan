using UnityEngine;
//Tyler Welsh - 2016
/*
    This is the base abstract template of our state system. Each game state will inherit from this Abstract class
    and implement the below functions as they need.

    The protected Monobehavior is the parent script that is holding the current state
*/


public class AbstractGameObjectState : IGameObjectState {

    //The parent object of the current Game State
    //This will generally always be GameController
    protected MonoBehaviour parent;

    //Constructor to assign the parent script
    public AbstractGameObjectState(MonoBehaviour parent)
    {
        this.parent = parent;
    }
    //
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
	
}
