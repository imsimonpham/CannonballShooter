using UnityEngine;

public class Cannonball : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] float _upwardModifier;
    private Vector3 _hitPoint;
    
    private bool _isGhost;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _impactSound;

    public void Init(Vector3 velocity, bool isGhost)
    {
        _isGhost = isGhost;
        _rb.AddForce(velocity, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_isGhost) return;

        if (collision.transform.CompareTag("Target"))
        {
            _audioSource.PlayOneShot(_impactSound);
            CreateSmallPush(collision);
        }
        
        CreateSmallPush(collision);
        
        if (collision.transform.CompareTag("Ground"))
            Destroy(gameObject, 0.5f);
    }

    private void CreateSmallPush(Collision collision)
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(_hitPoint, _explosionRadius);
    }
}
