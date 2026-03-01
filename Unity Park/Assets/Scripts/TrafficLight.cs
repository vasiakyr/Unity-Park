using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public GameObject redLamp;
    public GameObject greenLamp;

    private void Start()
    {
        SetGreen(); // αρχικά “κανονική” κατάσταση
    }

    public void SetRed()
    {
        redLamp.SetActive(true);
        greenLamp.SetActive(false);
    }

    public void SetGreen()
    {
        redLamp.SetActive(false);
        greenLamp.SetActive(true);
    }
}