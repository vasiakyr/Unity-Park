using UnityEngine;
using System.Collections;

public class TrafficLightController : MonoBehaviour
{
    public GameObject carRed;
    public GameObject carGreen;

    public float basePedTime = 5f;
    public float maxPedTime = 15f;

    Coroutine running;

    public void RequestCrossing(float extraTime)
    {
        float t = Mathf.Clamp(basePedTime + extraTime, basePedTime, maxPedTime);

        if (running != null) StopCoroutine(running);
        running = StartCoroutine(PedPhase(t));
    }

    IEnumerator PedPhase(float t)
    {
        if (carRed) carRed.SetActive(true);
        if (carGreen) carGreen.SetActive(false);

        yield return new WaitForSeconds(t);

        if (carRed) carRed.SetActive(false);
        if (carGreen) carGreen.SetActive(true);
    }
}