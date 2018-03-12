using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Flower : MonoBehaviour
{
    public static List<Flower> Flowers = new List<Flower>();
    public static Flower LastFlower;

    public int FlowerNum;
    public bool IsGrown = true;

    private float _time = 2;

    public void OnClick()
    {
        if(LastFlower != this)
            Select();
    }
    public void Select()
    {
        if (!Flowers.Contains(this))
        {
            StartCoroutine(Shrink());
            return;
        }

        if (LastFlower == null)
        {
            StartCoroutine(Shrink());
            LastFlower = this;
        }
        else
        {
            StartCoroutine(LastFlower.FlowerNum == FlowerNum ? Shrink() : LastFlower.Grow());
            LastFlower = null;
        }
    }

    public void Reset()
    {
        foreach (var flower in Flowers)
        {
            Destroy(flower);
        }
        Flowers = new List<Flower>();
    }
    public static bool BedCleared()
    {
        return Flowers.All(x => !x.IsGrown);
    }

    public IEnumerator Shrink()
    {
        IsGrown = false;
        for (var i = 1f; i > 0; i -= Time.deltaTime / _time)
        {
            yield return new WaitForEndOfFrame();
            transform.localScale = new Vector3(i, i, i);
        }
    }

    public IEnumerator Grow()
    {
        IsGrown = true;
        for (var i = 0f; i < 1; i += Time.deltaTime / _time)
        {
            yield return new WaitForEndOfFrame();
            transform.localScale = new Vector3(i,i,i);
        }
    }

}
