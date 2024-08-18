using System;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Cannon : MonoBehaviour
{
    [Header("Cannon Parts")]
    [SerializeField] private Transform _barrelPivot;
    [SerializeField] private Transform _leftWheel;
    [SerializeField] private Transform _rightWheel;
    [SerializeField] private Transform _cannonBallSpawn;
    
    [Header("Cannon Stats")]
    [SerializeField] private float _rotSpeed;
    [SerializeField] private float _force;
    [SerializeField] private float _fireRate;
    private float _lastTimeFire = 0f;
    
    [Header("Particles")]
    [SerializeField] private ParticleSystem _launchParticles;

    [Header("Audio")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _cannonFireSound;
    
    [Header("Cannonball")]
    [SerializeField] private CannonBall _cannonBallPrefab;

    [Header("Projection")] 
    [SerializeField] private Projection _projection;

    // Limits
    private bool _canRotateUpward;
    private bool _canRotateDownward;
    private bool _canRotateLeft;
    private bool _canRotateRight;
    
    // Vectors
    private Vector3 _initialForwardVector;
    private Vector3 _forwardVector;
    private Vector3 _upwardVector;
    private Vector3 _barrelForwardVector;
    
    // Angles
    private float _horizontalAngle;
    private float _horizontalDir;
    private float _verticalAngleUpward;
    private float _verticalAngleDownward;
    

    void Start()
    {
        _initialForwardVector = transform.forward;
        _upwardVector = transform.up;
    }

    private void FixedUpdate()
    {
        
    }

    void Update()
    {
        HandleControls();
        LaunchCannonball();
        _projection.SimulateTrajectory(_cannonBallPrefab, _cannonBallSpawn.position, _cannonBallSpawn.forward * _force );
    }

    void DrawTrajectoryLine()
    {
        
    }

    #region Controls
    
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
            _barrelPivot.Rotate(Vector3.right * _rotSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.W) && _canRotateUpward)
        {
            _barrelPivot.Rotate(Vector3.left * _rotSpeed * Time.deltaTime);    
        }
    }

    void HorizontalControl()
    {
        if (Input.GetKey(KeyCode.A) && _canRotateLeft) {
            transform.Rotate(Vector3.down * _rotSpeed * Time.deltaTime);
            _leftWheel.Rotate(Vector3.forward * _rotSpeed * 1.5f * Time.deltaTime);
            _rightWheel.Rotate(Vector3.back * _rotSpeed * 1.5f * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D) && _canRotateRight) {
            transform.Rotate(Vector3.up * _rotSpeed * Time.deltaTime);
            _leftWheel.Rotate(Vector3.back * _rotSpeed * 1.5f * Time.deltaTime);
            _rightWheel.Rotate(Vector3.forward * _rotSpeed * 1.5f * Time.deltaTime);
        }
    }

    void LimitCannonAngle()
    {
        // Horizontal limits
        CalculateHorizontalAngle();
        HorizontalLimits();
        
        // Vertical limits
        CalculateVerticalAngle();
        VerticalLimits();
    }

    void HorizontalLimits()
    {
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
    }

    void VerticalLimits()
    {
        if (_verticalAngleUpward >= 90)
        {
            _canRotateDownward = false;
            _canRotateUpward = true;
        }
        else if (_verticalAngleDownward >= 90)
        {
            _canRotateUpward = false;
            _canRotateDownward = true;
        }
        else
        {
            _canRotateUpward = true;
            _canRotateDownward = true;
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
        _verticalAngleUpward = Vector3.Angle(_barrelForwardVector, _upwardVector);
        _verticalAngleDownward = Vector3.Angle(_barrelForwardVector, _forwardVector);
    }
    
    #endregion
    
    #region Shoot
    void LaunchCannonball()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time - _lastTimeFire >= 1 / _fireRate)
        {
            Fire();
            PlayLaunchParticles();
            PlayLaunchSound();
            _lastTimeFire = Time.time;
        }
    }

    void Fire()
    {
        var ball = Instantiate(_cannonBallPrefab, _cannonBallSpawn.position, _cannonBallSpawn.rotation);
        ball.Init(_cannonBallSpawn.forward * _force);
    }

    void PlayLaunchParticles() {_launchParticles.Play();}

    void PlayLaunchSound() {_audioSource.PlayOneShot(_cannonFireSound);}
    
    #endregion

    #region Debugging
    void OnDrawGizmos()
    {
        DrawForwardVector();
        DrawInitialForwardVector();
        DrawBarrelForwardVector();
        DrawBarrelUpwardVector();
        DrawCrossProductVector();
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
    
    void DrawCrossProductVector()
    {
        Debug.DrawLine(transform.position, transform.position + Vector3.Cross(_initialForwardVector, _forwardVector), Color.white);
    }
    
    #endregion
}
