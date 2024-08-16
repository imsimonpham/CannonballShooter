using System;
using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;

    public void Init(Vector3 velocity)
    {
        _rb.AddForce(velocity, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            Destroy(gameObject, 0.5f);
        }
    }
}
