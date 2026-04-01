using UnityEngine;

public class SimpleBallPickup : MonoBehaviour
{
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched by: " + other.name + " | tag: " + other.tag);

        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            Destroy(gameObject);
        }
    }
}