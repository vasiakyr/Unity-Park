using UnityEngine;
using TMPro;

public class CollectGoal : MonoBehaviour
{
    public TextMeshProUGUI targetText;
    public GameObject nextTarget;
    public string nextMessage = "Μπράβο! Πήγαινε στον επόμενο στόχο.";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // αλλάζει το μήνυμα στην οθόνη
            if (targetText != null)
            {
                targetText.text = nextMessage;
            }

            // εμφανίζει τον επόμενο στόχο
            if (nextTarget != null)
            {
                nextTarget.SetActive(true);
            }

            // εξαφανίζει τη σφαίρα
            gameObject.SetActive(false);
        }
    }
}