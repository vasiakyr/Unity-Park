using UnityEngine;

public class PizzaCollectible : MonoBehaviour
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

        if (QuestManager.Instance != null && QuestManager.Instance.CanCollectPizza())
        {
            collected = true;

            QuestManager.Instance.CollectPizza();

            audioSource.Play();

            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;

            Destroy(gameObject, 1f);
        }
    }
}