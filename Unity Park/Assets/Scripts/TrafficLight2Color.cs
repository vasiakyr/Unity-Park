using System.Collections;
using UnityEngine;

public class TrafficLight2Color : MonoBehaviour
{
    public enum State
    {
        CarsGreen_PedRed,
        CarsRed_PedGreen
    }

    [Header("Lamp Renderers")]
    public MeshRenderer carRed;
    public MeshRenderer carGreen;

    [Header("Optional Ped lamps (can be empty)")]
    public MeshRenderer pedRed;
    public MeshRenderer pedGreen;

    [Header("Materials")]
    public Material redOn;
    public Material redOff;
    public Material greenOn;
    public Material greenOff;

    [Header("Timings")]
    public float carsGreenMinTime = 4f;   // ελάχιστο πράσινο στα αμάξια πριν “κόψει”
    public float carsGreenMaxTime = 10f;  // αν δεν ζητήσει κανείς, μένει τόσο
    public float pedGreenBaseTime = 6f;   // βασικός χρόνος πεζών
    public float allRedBuffer = 0.3f;     // μικρό buffer ασφάλειας

    public State currentState { get; private set; } = State.CarsGreen_PedRed;

    private bool pedRequest;
    private float extraPedTime;

    private void Start()
    {
        SetState(State.CarsGreen_PedRed);
        StartCoroutine(MainLoop());
    }

    // Καλείται από τη διάβαση
    public void RequestCrossing(float extraTime)
    {
        pedRequest = true;
        if (extraTime > extraPedTime) extraPedTime = extraTime;
    }

    private IEnumerator MainLoop()
    {
        while (true)
        {
            // --- Cars Green / Ped Red ---
            SetState(State.CarsGreen_PedRed);

            float t = 0f;
            while (t < carsGreenMaxTime)
            {
                t += Time.deltaTime;

                // αν υπάρχει αίτημα, και πέρασε το min time, πάμε σε πεζούς
                if (pedRequest && t >= carsGreenMinTime)
                    break;

                yield return null;
            }

            // μικρό buffer
            yield return new WaitForSeconds(allRedBuffer);

            // --- Cars Red / Ped Green ---
            SetState(State.CarsRed_PedGreen);

            float pedTime = pedGreenBaseTime + extraPedTime;
            yield return new WaitForSeconds(pedTime);

            // reset request
            pedRequest = false;
            extraPedTime = 0f;

            // buffer
            yield return new WaitForSeconds(allRedBuffer);
        }
    }

    private void SetState(State s)
    {
        currentState = s;

        // όλα OFF
        SetLamp(carRed, redOff);
        SetLamp(carGreen, greenOff);
        if (pedRed) SetLamp(pedRed, redOff);
        if (pedGreen) SetLamp(pedGreen, greenOff);

        if (s == State.CarsGreen_PedRed)
        {
            SetLamp(carGreen, greenOn);
            if (pedRed) SetLamp(pedRed, redOn);
        }
        else // CarsRed_PedGreen
        {
            SetLamp(carRed, redOn);
            if (pedGreen) SetLamp(pedGreen, greenOn);
        }
    }

    private void SetLamp(MeshRenderer r, Material m)
    {
        if (r) r.material = m;
    }

    public bool CarsCanGo()
    {
        return currentState == State.CarsGreen_PedRed;
    }

    public bool CarsMustStop()
    {
        return currentState == State.CarsRed_PedGreen;
    }
}