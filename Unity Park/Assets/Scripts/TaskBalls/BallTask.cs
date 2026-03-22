using UnityEngine;

public class BallTask : MonoBehaviour
{
    public BallTaskManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.CollectBall();
            Destroy(gameObject);
        }
    }
}