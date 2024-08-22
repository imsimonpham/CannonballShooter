using UnityEngine;

public class Box : MonoBehaviour
{
    [Header("Audio")] 
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _hitSounds;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            PlayHitSound();
            Destroy(gameObject, 0.05f);
        }
           
    }

    void PlayHitSound()
    {
        int randomIndex = Random.Range(0, _hitSounds.Length);
        _audioSource.PlayOneShot(_hitSounds[randomIndex]);
    }
}
