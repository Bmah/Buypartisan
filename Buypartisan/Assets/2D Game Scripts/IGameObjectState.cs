//Tyler Welsh - 2016
/*
    This is an interface object. To find more on interfaces please refer to the Unity documentation.
    This interface if for the Game States with the functions:
        Update()
        FixedUpdate()
        LateUpdate()

    The Game States must Implement these functions
*/
public interface IGameObjectState
{
    void Update();
    void FixedUpdate();
    void LateUpdate();
	
}
