using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    [SerializeField] private Transform _obstaclesParent;
    private Scene _simulationScene;
    private PhysicsScene _physicsScene;

    [SerializeField] private int _maxPhysicsFrameIterations;
    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _steps;

    void Start()
    {
        CreatePhysicsScene();
    }
    
    private void CreatePhysicsScene()
    {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        foreach (Transform obj in _obstaclesParent)
        {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            if (ghostObj.TryGetComponent<Renderer>(out Renderer renderer))
                renderer.enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
        }
    }

    public void SimulateTrajectory(CannonBall cannonballPrefab, Vector3 pos, Vector3 velocity)
    {
        var ghostObj = Instantiate(cannonballPrefab, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, _simulationScene);
        
        ghostObj.Init(velocity);
        _line.positionCount = _maxPhysicsFrameIterations;

        for (var i = 0; i < _maxPhysicsFrameIterations; i++)
        {
            _physicsScene.Simulate(Time.fixedDeltaTime * _steps);
            _line.SetPosition(i, ghostObj.transform.position);
        }

        Destroy(ghostObj.gameObject);
    }
}


