public interface IState
{
    void Tick(float _delta);
    void OnEnter();
    void OnExit();
}