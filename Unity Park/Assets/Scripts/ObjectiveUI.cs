using UnityEngine;
using TMPro;
using System.Collections;

public class ObjectiveUI : MonoBehaviour
{
    public TextMeshProUGUI messageText;

    [TextArea]
    public string startMessage = "Στόχος: Βρες το παιδί απέναντι.";

    public float messageDuration = 4f; // πόσα δευτερόλεπτα θα φαίνεται

    private void Start()
    {
        ShowMessage(startMessage);
    }

    public void ShowMessage(string msg)
    {
        StopAllCoroutines();
        StartCoroutine(ShowMessageRoutine(msg));
    }

    IEnumerator ShowMessageRoutine(string msg)
    {
        messageText.text = msg;
        messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(messageDuration);

        messageText.gameObject.SetActive(false);
    }
}