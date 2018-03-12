using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerPower : MonoBehaviour {

    public Material Material;
    public Vector2 OffsetMins;
    public Vector2 OffsetMaxs;
    public Vector2 OffsetSpeed;
    public float IntensityMin;
    public float IntensityMax;
    public float IntensitySpeed;

    private Vector2 tOffset = new Vector2(0,0);
    private float tIntesity = 0;

    void FixedUpdate()
    {
        var xOffset = Mathf.PingPong(tOffset.x, OffsetMaxs.x - OffsetMins.x) + OffsetMins.x;
        var yOffset = Mathf.PingPong(tOffset.y, OffsetMaxs.y - OffsetMins.y) + OffsetMins.y;
        var intensity = Mathf.PingPong(tIntesity, IntensityMax - IntensityMin + 0.00000001f) + IntensityMin;

        tOffset += OffsetSpeed / 6000;
        tIntesity += IntensitySpeed / 6000;

        Material.SetTextureOffset("_MainTex", new Vector2(xOffset, yOffset));
        Material.SetFloat("_Intensity", intensity);

    }
}
