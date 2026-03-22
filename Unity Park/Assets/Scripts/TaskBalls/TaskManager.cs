using UnityEngine;

public class BallTaskManager : MonoBehaviour
{
    private int collectedBalls = 0;

    public void CollectBall()
    {
        collectedBalls++;

        Debug.Log("Μπάλες που μαζεύτηκαν: " + collectedBalls);

        if (collectedBalls >= 2)
        {
            Debug.Log("Μάζεψες και τις 2 μπάλες!");

         
        }
    }
}