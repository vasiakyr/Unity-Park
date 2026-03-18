using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTrigger : MonoBehaviour
{
    public string playerTag = "Player";
    public string nextSceneName;
    public ObjectiveUI objectiveUI;
    public float delayBeforeNextScene = 2f;

    private bool completed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (completed) return;

        if (other.CompareTag(playerTag))
        {
            completed = true;

            if (objectiveUI != null)
            {
                objectiveUI.ShowMessage("Μπράβο! Ο στόχος ολοκληρώθηκε!");
            }

            Invoke(nameof(LoadNextScene), delayBeforeNextScene);
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}