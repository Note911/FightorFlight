using UnityEngine;
using System.Collections;

//Actions are components that can be added to a playable entity.  The player can register a list of actions and invoke them based on user input
//Actions are stored in a library to ensure they are always availible but they may not all be used
//The Action Library will return a COPY of an action component and add it to the entity that requests it. (via string ref)
public class Action : MonoBehaviour
{
    public float actionCD;
    public bool canExecute = true;

    //Determines which part of the action is currently being executed or if the action is waiting to be executed
    public enum ActionState { INACTIVE, ENTER, EXECUTE, EXIT };
    public ActionState actionState = ActionState.INACTIVE;

    public PlayableEntity _player;  //Reference to the attacking player
    public Rigidbody2D _rbody;        //Reference to the players rigidbody

    protected virtual void Start() {
        _player = gameObject.GetComponent<PlayableEntity>();
        _rbody = gameObject.GetComponent<Rigidbody2D>();
    }
    
    private void Update() {
        switch(actionState){
            case (ActionState.ENTER):
                Enter();
                break;
            case (ActionState.EXECUTE):
                Execute();
                break;
            case (ActionState.EXIT):
                Exit();
                break;
            case (ActionState.INACTIVE):
                Inactive();
                break;
        }
    }

    public void Begin() {
        if(canExecute)
            OnEnter();
    }
    //Called when the action needs to be invoked
    virtual public void OnEnter() {
        if(canExecute)
            actionState = ActionState.ENTER;
    }

    virtual public void OnExecute() {
        InvokeCooldown();
        actionState = ActionState.EXECUTE;
    }

    //Called when the exit condition has been met
    virtual public void OnExit() {
        actionState = ActionState.EXIT;
    }

    //Called upon finishing the action completely
    virtual public void Finish() {
        actionState = ActionState.INACTIVE;
    }
    //Cancels the action.  Called if another player strikes someone and knocks them out of the attack
    public void Cancel() {
        Finish();
    } 

    //The functions below are called via the update method inside of a switch.  These methods will be overriden by the implemetation of actions in the parent class.
    //However these actions should only be called via the base class update.

        //Called every frame during the begining of an action
        virtual public void Enter() {}
        //Called every frame during an action
        virtual public void Execute() {}
        //Called every frame while exiting an action
        virtual public void Exit() {}
        //Called every frame while the action is inactive
        virtual public void Inactive() {}

    //

    private void InvokeCooldown() {
        if(actionCD > 0.0f) { 
            canExecute = false;
            Invoke("ReleaseCooldown", actionCD);
        }
    }

    private void ReleaseCooldown() {
        canExecute = true;
    }

}
