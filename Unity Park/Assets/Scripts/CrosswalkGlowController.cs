using UnityEngine;

public class CrosswalkGlowController : MonoBehaviour
{
    public Renderer crosswalkRenderer;
    public Material normalMaterial;
    public Material glowMaterial;

    private int carsNear = 0;

    void Start()
    {
        SetNormal();
    }

    public void CarEntered()
    {
        carsNear++;
        SetGlow();
    }

    public void CarExited()
    {
        carsNear--;

        if (carsNear <= 0)
        {
            carsNear = 0;
            SetNormal();
        }
    }

    void SetNormal()
    {
        if (crosswalkRenderer != null && normalMaterial != null)
            crosswalkRenderer.material = normalMaterial;
    }

    void SetGlow()
    {
        if (crosswalkRenderer != null && glowMaterial != null)
            crosswalkRenderer.material = glowMaterial;
    }
}