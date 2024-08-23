using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Explosion")] 
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] float _upwardModifier;
    private Vector3 _hitPoint;
    
    [Header("Audio")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _explosionSound;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(("Ball")))
        {
            CreateExplosion(collision);
            /*if(gameObject.TryGetComponent<Renderer>(out Renderer renderer))
                renderer.enabled = false;*/
            Destroy(gameObject, 2f);
        }
    }

    private void CreateExplosion(Collision collision)
    {
        // Cast a sphere at hit point and add force
        ContactPoint contact = collision.contacts[0];
        _hitPoint = contact.point;
            
        Collider[] colliders = Physics.OverlapSphere(_hitPoint, _explosionRadius);
            
        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent<Rigidbody>(out Rigidbody rb))
                rb.AddExplosionForce(_explosionForce, _hitPoint, _explosionRadius, _upwardModifier, ForceMode.Impulse);
        }

        // Add SFX and VFX
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        _audioSource.PlayOneShot(_explosionSound);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_hitPoint, _explosionRadius);
    }
}
