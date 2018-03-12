using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Scene : MonoBehaviour
{
    public string SceneName = "New Scene";

    public Vector3[] PositionOffsets;
    public Vector3[] LookPositionOffsets;

    public List<GameObject> SceneObjects;

    public SceneState State;

    public virtual bool Construct(string pastSceneName)
    {
        return false;
    }
    public virtual bool Destroy(string nextSceneName)
    {
        return false;
    }

    public void ActivateObjects()
    {
        foreach (var obj in SceneObjects)
        {
            obj.SetActive(true);
        }
    }
    public void DeactivateObjects()
    {
        foreach (var obj in SceneObjects)
        {
            obj.SetActive(false);
        }
    }
}

public enum SceneState {
    Ready,
    TransitioningTo,
    Active,
    TransitioningAway,
    Complete,
}