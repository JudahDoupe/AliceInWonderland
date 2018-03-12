using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : Scene
{
    public Movement Alice;
    public Movement MainCamera;
    public Movement Rabbit;
    public SkyboxController GradientSkybox;

    public Vector3[] RabbitPositions;

    public Color Sky;
    public Color Ground;

    public override bool Construct(string pastSceneName)
    {
        State = SceneState.TransitioningTo;

        ActivateObjects();

        Alice.Animator.Play("Sleeping");
        Alice.transform.position = new Vector3(9.3f, 0, 8.5f);
        var lookDir = new Vector3(-1, 0, -0.5f).normalized;
        Alice.transform.LookAt(Alice.transform.position + lookDir, Vector3.up);

        GradientSkybox.ChangeIntensity(1);
        GradientSkybox.ChangeColor(1, Ground);
        GradientSkybox.ChangeColor(2, Sky);

        State = SceneState.Active;

        StartCoroutine(Game());
        return true;
    }
    public override bool Destroy(string nextSceneName)
    {
        
        switch (nextSceneName)
        {
            case "DownTheRabbitHole":
                StartCoroutine(TransitionToDownTheRabbitHole());
                return true;
            default:
                return false;
        }
    }

    private IEnumerator Game()
    {
        var rabbit = Rabbit.gameObject.GetComponent<Rabbit>();
        var i = 0;
        while (!rabbit.Detected)
        {
            if (!Rabbit.IsMoving)
            {
                Rabbit.StartCoroutine(Rabbit.MoveTo(RabbitPositions[i % 2]));
                Rabbit.StartCoroutine(Rabbit.LookAt(RabbitPositions[i % 2],Vector3.up));
                i++;
            }
            yield return new WaitForEndOfFrame();
        }
        Alice.Animator.SetBool("WakeUp", true);
        MainCamera.StartCoroutine(MainCamera.Track(Rabbit.gameObject, new Vector3(0, 1, 0)));
        Rabbit.StartCoroutine(Rabbit.MoveTo(RabbitPositions[2]));
        Rabbit.StartCoroutine(Rabbit.LookAt(RabbitPositions[2], Vector3.up));

        while (Rabbit.IsMoving) yield return new WaitForEndOfFrame();
        Rabbit.gameObject.SetActive(false);
        //MainCamera.StartCoroutine(MainCamera.Track(Alice.gameObject, LookPositionOffsets[0]));
        MainCamera.StartCoroutine(MainCamera.Follow(Alice.gameObject, PositionOffsets[0]));
        Alice.Animator.SetBool("WakeUp", false);

    }

    private IEnumerator TransitionToDownTheRabbitHole()
    {
        State = SceneState.TransitioningAway;

        var rabbitHole = new Vector3(3.4f, 0.9f, 21.4f);

        Alice.StartCoroutine(Alice.LookForward(Vector3.zero));
        Alice.StartCoroutine(Alice.MoveTo(rabbitHole + Vector3.down));
        Alice.Animator.SetBool("IsMoving",true);

        while (Alice.IsMoving) yield return new WaitForEndOfFrame();

        Alice.transform.position = new Vector3(0, -10, 1);
        Alice.transform.localEulerAngles = Vector3.zero;
        Alice.Animator.SetBool("IsFalling",true);
        MainCamera.StartCoroutine(MainCamera.MoveTo(rabbitHole));
        MainCamera.StartCoroutine(MainCamera.LookAt(rabbitHole + Vector3.down,Vector3.up));

        while (MainCamera.IsMoving) yield return new WaitForEndOfFrame();

        DeactivateObjects();

        State = SceneState.Complete;
    }
}
