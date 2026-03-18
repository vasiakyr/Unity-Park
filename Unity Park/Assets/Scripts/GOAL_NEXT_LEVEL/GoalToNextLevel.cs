using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalToNextLevel : MonoBehaviour
{
    [Tooltip("Βάλε το όνομα του επόμενου Scene όπως είναι στο Project (π.χ. Level2)")]
    public string nextSceneName = "Level2";

    [Tooltip("Το tag του παίκτη (πρέπει ο Player να έχει Tag: Player)")]
    public string playerTag = "Player";

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag(playerTag)) return;

        triggered = true;
        SceneManager.LoadScene(nextSceneName);
    }
}