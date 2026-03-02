using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public GameObject redLamp;
    public GameObject greenLamp;

    // κρατάμε κατάσταση
    private bool isRed = false;

    // τα αμάξια θα ρωτάνε αυτά:
    public bool IsRed => isRed;
    public bool IsGreen => !isRed;

    private void Start()
    {
        SetGreen(); // αρχικά “κανονική” κατάσταση
    }

    public void SetRed()
    {
        isRed = true;
        redLamp.SetActive(true);
        greenLamp.SetActive(false);
    }

    public void SetGreen()
    {
        isRed = false;
        redLamp.SetActive(false);
        greenLamp.SetActive(true);
    }
}