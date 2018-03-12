using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public Material GradientSkybox;

    public float Intensity
    {
        get
        {
            return GradientSkybox.GetFloat("_Intensity");
        }
    }

    public bool Hault;

    void Start()
    {
        DoSky();
    }

    public void ChangeIntensity(float value, float time = 0f)
    {
        Hault = true;
        if (Math.Abs(time) < 0.001)
            GradientSkybox.SetFloat("_Intensity", value);
        else
            StartCoroutine(changeIntensity(value, time));
    }
    public void ChangeColor(int colorNum, Color color, float time = 0f)
    {
        Hault = true;
        string colorName = "_Color" + colorNum;

        if (Math.Abs(time) < 0.001)
            GradientSkybox.SetColor(colorName, color);
        else
            StartCoroutine(changeColor(colorName, color, time));
    }

    public void DoTunnel()
    {
        Hault = true;
        GradientSkybox.SetVector("_UpVector",new Vector4(0,0,-1,0));
        GradientSkybox.SetFloat("_Banding", 0.9f);
        GradientSkybox.SetFloat("_Exponent", 0.25f);

    }
    public void DoSky()
    {
        Hault = true;
        GradientSkybox.SetVector("_UpVector", new Vector4(0, 1, 0, 0));
        GradientSkybox.SetFloat("_Banding", 1);
        GradientSkybox.SetFloat("_Exponent", 1);
    }

    private IEnumerator changeIntensity(float value,  float time)
    {
        Hault = false;
        var oldValue = GradientSkybox.GetFloat("_Intensity");
        for (var i = 0f; i < time; i += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            if (Hault) yield break;
            var x = Mathf.Lerp(oldValue, value, i / time);
            GradientSkybox.SetFloat("_Intensity", x);
        }
        GradientSkybox.SetFloat("_Intensity", value);
    }
    private IEnumerator changeColor(string colorName, Color color, float time)
    {
        Hault = false;
        for (var i = 0f; i < time; i += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            if (Hault) yield break;
            var newColor = Color.Lerp(GradientSkybox.GetColor(colorName), color, i / time);
            GradientSkybox.SetColor(colorName, newColor);
        }
        GradientSkybox.SetColor(colorName, color);
    }
}
