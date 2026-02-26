using UnityEngine;
using TMPro;

public class PlayerCollision : MonoBehaviour
{
    public GameObject gameOverUI;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Car"))
        {
            Debug.Log("Player hit a car!");

            if (gameOverUI != null)
                gameOverUI.SetActive(true);

            Time.timeScale = 0f; // παγώνει το παιχνίδι
        }
    }
}