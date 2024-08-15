using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Transform _barrelPivot;
    [SerializeField] private Transform _leftWheel;
    [SerializeField] private Transform _rightWheel;
    [SerializeField] private float _rotateSpeed;

    [SerializeField] private bool _canRotateUpward;
    [SerializeField] private bool _canRotateDownward;
    [SerializeField] private bool _canRotateLeft;
    [SerializeField] private bool _canRotateRight;
    
    [SerializeField] private Vector3 _initialForwardVector;
    [SerializeField] private Vector3 _forwardVector;
    [SerializeField] private float _horizontalAngle;
    [SerializeField] private float _horizontalDir;
    
    [SerializeField] private Vector3 _barrelForwardVector;
    [SerializeField] private Vector3 _upwardVector;
    [SerializeField] private float _verticalAngle;
    [SerializeField] private float _verticalDir;
    

    void Start()
    {
        _initialForwardVector = transform.forward;
        
        // Define the angle in degrees
        float angleDegrees = 135f;
        float angleRadians = angleDegrees * Mathf.Deg2Rad;

        // Compute the components of the vector
        float x = Mathf.Cos(angleRadians);
        float y = Mathf.Sin(angleRadians);
        
        // Create the vector
        _upwardVector = new Vector3(x, y, 0).normalized;
    }
    
    void Update()
    {
        HandleControls();
    }

    private void HandleControls()
    {
        VerticalControl();
        HorizontalControl();
        LimitCannonAngle();
    }

    void VerticalControl()
    {
        if (Input.GetKey(KeyCode.S) && _canRotateDownward)
        {
            _barrelPivot.Rotate(Vector3.right * _rotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.W) && _canRotateUpward)
        {
            _barrelPivot.Rotate(Vector3.left * _rotateSpeed * Time.deltaTime);    
        }
    }

    void HorizontalControl()
    {
        if (Input.GetKey(KeyCode.A) && _canRotateLeft) {
            transform.Rotate(Vector3.down * _rotateSpeed * Time.deltaTime);
            _leftWheel.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
            _rightWheel.Rotate(Vector3.back * _rotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D) && _canRotateRight) {
            transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
            _leftWheel.Rotate(Vector3.back * _rotateSpeed * 1.5f * Time.deltaTime);
            _rightWheel.Rotate(Vector3.forward * _rotateSpeed * 1.5f * Time.deltaTime);
        }
    }

    void LimitCannonAngle()
    {
        // Horizontal limits
        CalculateHorizontalAngle();
        if (_horizontalAngle >= 60)
        {
            if (_horizontalDir < 0)
            {
                _canRotateLeft = false;
                _canRotateRight = true;
            }
            else if(_horizontalDir > 0)
            {
                _canRotateRight = false;
                _canRotateLeft = true;
            }
        }
        else
        {
            _canRotateLeft = true;
            _canRotateRight = true;
        }
        
        // Vertical limits
        CalculateVerticalAngle();
        if (_verticalAngle >= 45)
        {
            if (_verticalDir < 0)
            {
                _canRotateDownward = false;
                _canRotateUpward = true;
            }
            else if(_verticalDir > 0)
            {
                _canRotateUpward = false;
                _canRotateDownward = true;
            }
        }
        else
        {
            _canRotateDownward = true;
            _canRotateUpward = true;
        }
    }

    void CalculateHorizontalAngle()
    {
        _forwardVector = transform.forward;
        _horizontalAngle = Vector3.Angle(_initialForwardVector, _forwardVector);
        
        // Calculate direction of the rotation
        Vector3 crossProduct = Vector3.Cross(_initialForwardVector, _forwardVector);
        _horizontalDir = Vector3.Dot(transform.up, crossProduct);
    }

    void CalculateVerticalAngle()
    {
        _barrelForwardVector = _barrelPivot.forward;
        _verticalAngle = Vector3.Angle(_barrelForwardVector, _upwardVector);
        
        //Calculate direction of the rotation
        Vector3 crossProduct = Vector3.Cross(_barrelForwardVector, _upwardVector);
        _verticalDir = Vector3.Dot(transform.right, crossProduct);
    }

    void OnDrawGizmos()
    {
        DrawForwardVector();
        DrawInitialForwardVector();
        DrawBarrelForwardVector();
        DrawBarrelUpwardVector();
    }

    void DrawForwardVector()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
    }

    void DrawInitialForwardVector()
    {
        Debug.DrawLine(transform.position, transform.position + _initialForwardVector, Color.yellow);
    }

    void DrawBarrelForwardVector()
    {
        Debug.DrawLine(_barrelPivot.position, _barrelPivot.position + _barrelPivot.forward, Color.green);
    }
    
    void DrawBarrelUpwardVector()
    {
        Debug.DrawLine(_barrelPivot.position, _barrelPivot.position + _upwardVector, Color.blue);
    }
}
