using System.Collections.Generic;
using UnityEngine;

public class PhotoluminescentCrosswalkGroup : MonoBehaviour
{
    [Header("DEBUG")]
    public bool forceGlow = false;              // ✅ αν το τσεκάρεις, η διάβαση γίνεται πράσινη πάντα
    public bool showDebugLog = true;
    [Range(0f, 1f)] public float lastLightHit;  // για να βλέπεις αν “πιάνει” φως

    [Header("Behavior")]
    public bool fullGlowWhileLit = true;

    [Header("Charge (0..1)")]
    [Range(0f, 1f)] public float charge = 0f;
    public float decaySpeed = 0.02f;

    [Header("Colors")]
    public Color dayBaseColor = Color.white;
    public Color glowColor = new Color(0.25f, 1.0f, 0.6f);
    public float maxEmissionIntensity = 10f;

    [Header("Light detection (choose one)")]
    public string lightTag = "GlowLight";       // auto-find by tag
    public float considerLightsWithin = 120f;

    [Tooltip("Αν το γεμίσεις, θα χρησιμοποιήσει αυτά τα φώτα και δεν θα ψάχνει με tag.")]
    public List<Light> manualLights = new List<Light>();

    [Header("Performance")]
    public float refreshLightsEvery = 0.5f;

    readonly List<Light> cachedLights = new List<Light>();
    readonly List<Renderer> renderers = new List<Renderer>();
    MaterialPropertyBlock mpb;
    float refreshTimer;

    void Awake()
    {
        mpb = new MaterialPropertyBlock();
        GetComponentsInChildren(true, renderers);

        // Σιγουριά: ενεργοποιούμε emission keyword στα materials
        foreach (var r in renderers)
        {
            if (r != null && r.sharedMaterial != null)
                r.sharedMaterial.EnableKeyword("_EMISSION");
        }

        RefreshLights();
        refreshTimer = refreshLightsEvery;
    }

    void Update()
    {
        if (forceGlow)
        {
            charge = 1f;
            ApplyLookToAll();
            return;
        }

        refreshTimer -= Time.deltaTime;
        if (refreshTimer <= 0f)
        {
            RefreshLights();
            refreshTimer = refreshLightsEvery;
        }

        float lightHit = EstimateLightOnCrosswalk();
        lastLightHit = lightHit;

        if (fullGlowWhileLit)
        {
            if (lightHit > 0.01f) charge = 1f;
            else charge = Mathf.Clamp01(charge - decaySpeed * Time.deltaTime);
        }
        else
        {
            // αν θες “φόρτιση” αντί για άμεσο 1
            if (lightHit > 0.01f) charge = Mathf.Clamp01(charge + lightHit * Time.deltaTime);
            else charge = Mathf.Clamp01(charge - decaySpeed * Time.deltaTime);
        }

        ApplyLookToAll();

        if (showDebugLog)
            Debug.Log($"[Crosswalk] lights={cachedLights.Count} lightHit={lightHit:F2} charge={charge:F2}");
    }

    void RefreshLights()
    {
        cachedLights.Clear();

        // Αν έχεις manual lights, χρησιμοποιούμε αυτά (πιο σίγουρο)
        if (manualLights != null && manualLights.Count > 0)
        {
            foreach (var l in manualLights)
                if (l != null && l.enabled) cachedLights.Add(l);
            return;
        }

        // αλλιώς auto-find με tag
        var gos = GameObject.FindGameObjectsWithTag(lightTag);
        foreach (var go in gos)
        {
            var l = go.GetComponent<Light>();
            if (l != null && l.enabled) cachedLights.Add(l);
        }
    }

    Bounds GetBounds()
    {
        Bounds b = new Bounds(transform.position, Vector3.zero);
        bool hasAny = false;

        foreach (var r in renderers)
        {
            if (r == null) continue;
            if (!hasAny) { b = r.bounds; hasAny = true; }
            else b.Encapsulate(r.bounds);
        }

        return b;
    }

    float EstimateLightOnCrosswalk()
    {
        var bounds = GetBounds();
        Vector3 p = bounds.center;
        p.y = bounds.max.y + 0.05f;

        float sum = 0f;

        foreach (var l in cachedLights)
        {
            if (l == null || !l.enabled) continue;

            float d = Vector3.Distance(l.transform.position, p);
            if (d > considerLightsWithin) continue;

            // πολύ σημαντικό: αν είσαι έξω από το Range του φωτός, μηδέν.
            if (d > l.range) continue;

            float att = Mathf.Clamp01(1f - (d / l.range));

            float dirFactor = 1f;
            if (l.type == LightType.Spot)
            {
                Vector3 toP = (p - l.transform.position).normalized;
                float cos = Vector3.Dot(l.transform.forward, toP);
                dirFactor = Mathf.Clamp01((cos - 0.2f) / 0.8f);
            }

            // normalization intensity (για Unity/URP, δοκιμαστικά)
            float intensityNorm = Mathf.Clamp01(l.intensity / 8000f);

            sum += att * dirFactor * intensityNorm;
        }

        return Mathf.Clamp01(sum);
    }

    void ApplyLookToAll()
    {
        float t = Mathf.Clamp01(charge);

        Color baseColor = Color.Lerp(dayBaseColor, glowColor, t);
        float e = t * maxEmissionIntensity;
        Color emission = glowColor * e;

        foreach (var r in renderers)
        {
            if (r == null) continue;

            r.GetPropertyBlock(mpb);

            // URP Lit + fallback:
            mpb.SetColor("_BaseColor", baseColor);
            mpb.SetColor("_Color", baseColor);

            mpb.SetColor("_EmissionColor", emission);

            r.SetPropertyBlock(mpb);
        }
    }
}