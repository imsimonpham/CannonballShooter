using UnityEngine;

public class Box : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            Destroy(gameObject, 1f);
        }
    }
}
