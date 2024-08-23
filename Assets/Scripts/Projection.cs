using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    [Header("Scene Objects")]
    [SerializeField] private Transform _obstaclesParent;
    private Scene _simulationScene;
    private PhysicsScene _physicsScene;

    [Header("Trajectory Line")]
    [SerializeField] private int _maxPhysicsFrameIterations;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _lineWidth;
    
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
            GameObject ghostObj = InstantiateAndMoveObjectsToSimulationScene(obj.gameObject, obj.position, obj.rotation);
            DisableRenderers(ghostObj.transform);
        }
    }

    public void SimulateTrajectoryLine(Cannonball cannonballPrefab, Vector3 pos, Vector3 velocity)
    {
        GameObject ghostObj = InstantiateAndMoveObjectsToSimulationScene(cannonballPrefab.gameObject, pos, Quaternion.identity);
        ghostObj.GetComponent<Cannonball>().Init(velocity, true);
        
        _lineRenderer.positionCount = _maxPhysicsFrameIterations;
        _lineRenderer.startWidth = _lineWidth;

        for (var i = 0; i < _maxPhysicsFrameIterations; i++)
        {
            _physicsScene.Simulate(Time.fixedDeltaTime);
            _lineRenderer.SetPosition(i, ghostObj.transform.position);
        }

        Destroy(ghostObj.gameObject);
    }
    
    private void DisableRenderers(Transform parentObj)
    {
        // disable renderer component in parent object(level 1)
        if (parentObj.TryGetComponent<Renderer>(out Renderer renderer))
            renderer.enabled = false;
        
        // disable box collider component in objects of the cubes (Target)
        if (parentObj.TryGetComponent<BoxCollider>(out BoxCollider collider) && (parentObj.CompareTag("Target")))
            collider.enabled = false;
        
        // disable renderer components in child objects (level 2)
        if (parentObj.childCount > 0)
        {
            foreach (Transform childObj_1 in parentObj.transform)
            {
                if(childObj_1.TryGetComponent<Renderer>(out Renderer renderer_child_1))
                    renderer_child_1.enabled = false;
                
                // disable box collider component in objects of the cubes (Target) and bomb. 
                if (childObj_1.TryGetComponent<BoxCollider>(out BoxCollider collider_child_1) && (childObj_1.CompareTag("Target") || childObj_1.CompareTag("Bomb")))
                    collider_child_1.enabled = false;
                
                // disable renderer components in child objects (level 3)
                if (childObj_1.childCount > 0)
                {
                    foreach (Transform childObj_2 in childObj_1.transform)
                    {
                        if(childObj_2.TryGetComponent<Renderer>(out Renderer renderer_child_2))
                            renderer_child_2.enabled = false;
                        
                        // disable box collider component in objects of the cubes (Target) 
                        if (childObj_2.TryGetComponent<BoxCollider>(out BoxCollider collider_child_2) && childObj_2.CompareTag("Target"))
                            collider_child_2.enabled = false;
                    }
                }
            }
        }
    }

    private GameObject InstantiateAndMoveObjectsToSimulationScene(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        var obj = Instantiate(prefab, pos, rotation);
        SceneManager.MoveGameObjectToScene(obj, _simulationScene);
        return obj;
    }
}


