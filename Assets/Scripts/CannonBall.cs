using UnityEngine;

public class CannonBall : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] float _upwardModifier = 1f;
    private Vector3 _hitPoint;

    public void Init(Vector3 velocity)
    {
        _rb.AddForce(velocity, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Target"))
        {
            ContactPoint contact = collision.contacts[0];
            _hitPoint = contact.point;
            
            Collider[] colliders = Physics.OverlapSphere(_hitPoint, _explosionRadius);
            
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<Rigidbody>(out Rigidbody rb))
                {
                    Vector3 dir = collider.transform.position - _hitPoint;
                    rb.AddExplosionForce(_explosionForce, _hitPoint, _explosionRadius, _upwardModifier, ForceMode.Impulse);
                }
            }

        }
        
        if (collision.transform.CompareTag("Ground"))
        {
            Destroy(gameObject, 0.5f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_hitPoint, _explosionRadius);
    }
}
