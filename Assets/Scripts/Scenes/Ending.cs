using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : Scene {

    [HideInInspector]
    public Movement Alice;
    [HideInInspector]
    public Movement MainCamera;
    [HideInInspector]
    public SkyboxController GradientSkybox;

    public Color Sky;
    public Color Ground;

    public override bool Construct(string pastSceneName)
    {
        switch (pastSceneName)
        {
            case "QueenOfHearts":
                StartCoroutine(TransitionFromQueenOfHearts(SceneChanger.CurrentScene as QueenOfHearts));
                return true;
            default:
                Debug.Log("Failed to transition to " + SceneName + " from " + pastSceneName);
                return false;
        }
    }
    public override bool Destroy(string nextSceneName)
    {
        return false;
    }

    private IEnumerator TransitionFromQueenOfHearts(QueenOfHearts queenOfHearts)
    {
        State = SceneState.TransitioningTo;
        Alice = queenOfHearts.Alice;
        MainCamera = queenOfHearts.MainCamera;
        GradientSkybox = queenOfHearts.GradientSkybox;

        while (queenOfHearts.State != SceneState.Complete) yield return new WaitForEndOfFrame();

        Alice.Animator.Play("Sleeping");
        Alice.transform.position = new Vector3(9.3f,0,8.5f);
        var lookDir = new Vector3(-1,0,-0.5f).normalized;
        Alice.StartCoroutine(Alice.LookAt(Alice.transform.position + lookDir, Vector3.up));

        var rabbitHole = new Vector3(3.4f, 0.9f, 21.4f);
        MainCamera.transform.position = rabbitHole;
        MainCamera.transform.LookAt(Alice.transform.position, Vector3.up);

        ActivateObjects();

        GradientSkybox.ChangeIntensity(1);
        GradientSkybox.ChangeColor(1, Ground);
        GradientSkybox.ChangeColor(2, Sky);

        MainCamera.StartCoroutine(MainCamera.MoveTo(PositionOffsets[0]));
        MainCamera.StartCoroutine(MainCamera.Track(Alice.gameObject, LookPositionOffsets[0]));

        while (MainCamera.IsMoving) yield return new WaitForEndOfFrame();

        MainCamera.StartCoroutine(MainCamera.RotateAround(Alice.transform.position, PositionOffsets[1]));
        MainCamera.StartCoroutine(MainCamera.Track(Alice.gameObject, LookPositionOffsets[1]));

        yield return new WaitForSeconds(5);
        Application.Quit();
    }
}
