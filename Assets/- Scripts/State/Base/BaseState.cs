// NOTE: This state machine setup borrows from this article: 
// https://medium.com/c-sharp-progarmming/make-a-basic-fsm-in-unity-c-f7d9db965134

public class BaseState
{
    public string name;
    protected BaseStateMachine _stateMachine;

    public BaseState(string name, BaseStateMachine stateMachine)
    {
        this.name = name;
        this._stateMachine = stateMachine;
    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}