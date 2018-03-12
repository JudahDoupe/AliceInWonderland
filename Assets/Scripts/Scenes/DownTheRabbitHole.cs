using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownTheRabbitHole : Scene {

    [HideInInspector]
    public Movement Alice;
    [HideInInspector]
    public Movement MainCamera;
    [HideInInspector]
    public SkyboxController GradientSkybox;

    public float FallTime;

    public override bool Construct(string pastSceneName)
    {
        switch (pastSceneName)
        {
            case "Opening":
                StartCoroutine(TransitionFromOpening(SceneChanger.CurrentScene as Opening));
                return true;
            default:
                Debug.Log("Failed to transition to " + SceneName + " from " + pastSceneName);
                return false;
        }
    }
    public override bool Destroy(string nextSceneName)
    {
        switch (nextSceneName)
        {
            case "QueenOfHearts":
                StartCoroutine(TransitionToQueenOfHearts());
                return true;
            case "FlowerGarden":
                StartCoroutine(TransitionToFlowerGarden());
                return true;
            default:
                return false;
        }
    }

    private IEnumerator TransitionFromOpening(Opening opening)
    {
        State = SceneState.TransitioningTo;
        Alice = opening.Alice;
        MainCamera = opening.MainCamera;
        GradientSkybox = opening.GradientSkybox;

        ActivateObjects();
        var triggerFlower = SceneObjects[0].GetComponent<Flower>();

        while (opening.State != SceneState.Complete) yield return new WaitForEndOfFrame();

        while (MainCamera.IsMoving) yield return new WaitForEndOfFrame();

        GradientSkybox.ChangeIntensity(10);
        GradientSkybox.ChangeIntensity(0, FallTime);

        MainCamera.StartCoroutine(MainCamera.RotateAround(Alice.transform.position, PositionOffsets[0]));
        MainCamera.StartCoroutine(MainCamera.Track(Alice.gameObject, LookPositionOffsets[0]));

        while (GradientSkybox.Intensity > 0) yield return new WaitForEndOfFrame();

        Alice.Animator.SetBool("IsMoving",false);
        Alice.Animator.SetBool("IsFalling", false);
        yield return new WaitForSeconds(2);

        State = SceneState.Active;
        triggerFlower.transform.position = Alice.transform.position + new Vector3(2, 0, 1);
        triggerFlower.StartCoroutine(triggerFlower.Grow());
    }

    private IEnumerator TransitionToQueenOfHearts()
    {
        State = SceneState.TransitioningAway;

        Alice.MovementSpeed = 2;
        Alice.StartCoroutine(Alice.LookForward(Vector3.zero));
        Alice.StartCoroutine(Alice.MoveTo(Alice.transform.position + new Vector3(0,0,10)));
        Alice.Animator.SetBool("IsMoving", true);

        while (Alice.IsMoving) yield return new WaitForEndOfFrame();

        Alice.Animator.SetBool("IsMoving", false);

        DeactivateObjects();

        State = SceneState.Complete;
    }
    private IEnumerator TransitionToFlowerGarden()
    {
        State = SceneState.TransitioningAway;

        while (Alice.IsMoving) yield return new WaitForEndOfFrame();

        DeactivateObjects();

        State = SceneState.Complete;
    }

}
