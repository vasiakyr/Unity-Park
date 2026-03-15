using UnityEngine;

public class IceCreamCollectible : MonoBehaviour
{
    private bool collected = false;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        if (QuestManager.Instance != null && QuestManager.Instance.CanCollectIceCream())
        {
            collected = true;

            QuestManager.Instance.CollectIceCream();

            audioSource.Play();

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;

            Destroy(gameObject, 1f);
        }
    }
}