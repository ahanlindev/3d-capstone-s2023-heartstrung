// NOTE: This state machine setup borrows from this article: 
// https://medium.com/c-sharp-progarmming/make-a-basic-fsm-in-unity-c-f7d9db965134

public class BaseState
{
    public string name;
    protected BaseStateMachine _baseStateMachine;

    public BaseState(string name, BaseStateMachine stateMachine)
    {
        this.name = name;
        this._baseStateMachine = stateMachine;
    }

    /// <summary>Code to execute upon entering this state</summary>
    public virtual void Enter() { }

    /// <summary>Code to execute each frame (analogous to Update)</summary>
    public virtual void UpdateLogic() { }

    /// <summary>Code to execute on each physics tick (analogous to FixedUpdate)</summary>
    public virtual void UpdatePhysics() { }

    /// <summary>Code to execute upon exiting this state</summary>
    public virtual void Exit() { }
}