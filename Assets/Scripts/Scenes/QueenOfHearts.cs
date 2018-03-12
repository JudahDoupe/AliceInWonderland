using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class QueenOfHearts : Scene {

    [HideInInspector]
    public Movement Alice;
    [HideInInspector]
    public Movement MainCamera;
    [HideInInspector]
    public SkyboxController GradientSkybox;

    public Color Sky;
    public Color Ground;

    public Movement Joker;
    public GameObject CardPrefab;

    private GameObject Star;

    public Material[] CardMaterials = new Material[13];

    public override bool Construct(string pastSceneName)
    {
        switch (pastSceneName)
        {
            case "DownTheRabbitHole":
                StartCoroutine(TransitionFromDownTheRabbitHole(SceneChanger.CurrentScene as DownTheRabbitHole));
                return true;
            case "FlowerGarden":
                StartCoroutine(TransitionFromFlowerGarden(SceneChanger.CurrentScene as FlowerGarden));
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
            case "Ending":
                StartCoroutine(TransitionToEnding());
                return true;
            default:
                return false;
        }
    }

    private IEnumerator TransitionFromDownTheRabbitHole(DownTheRabbitHole rabbitHole)
    {
        State = SceneState.TransitioningTo;
        Alice = rabbitHole.Alice;
        MainCamera = rabbitHole.MainCamera;
        GradientSkybox = rabbitHole.GradientSkybox;

        while (rabbitHole.State != SceneState.Complete) yield return new WaitForEndOfFrame();

        GradientSkybox.ChangeColor(1, Ground);
        GradientSkybox.ChangeColor(2, Sky);
        GradientSkybox.ChangeIntensity(1,3);

        ActivateObjects();

        Joker.StartCoroutine(Joker.MoveTo(Joker.transform.position + new Vector3(0, 0, 5)));
        Joker.StartCoroutine(Joker.LookAt(Alice.transform.position, Vector3.up));

        while (Joker.IsMoving) yield return new WaitForEndOfFrame();

        MainCamera.StartCoroutine(MainCamera.Follow(Alice.gameObject, PositionOffsets[0]));
        MainCamera.StartCoroutine(MainCamera.Track(Alice.gameObject, LookPositionOffsets[0]));

        State = SceneState.Active;
        StartCoroutine(Game());
    }

    private IEnumerator TransitionFromFlowerGarden(FlowerGarden flowerGarden)
    {
        State = SceneState.TransitioningTo;
        Alice = flowerGarden.Alice;
        MainCamera = flowerGarden.MainCamera;
        GradientSkybox = flowerGarden.GradientSkybox;

        while (flowerGarden.State != SceneState.Complete) yield return new WaitForEndOfFrame();

        GradientSkybox.ChangeColor(1, Ground);
        GradientSkybox.ChangeColor(2, Sky);
        GradientSkybox.ChangeIntensity(1, 3);

        Alice.Animator.SetBool("IsMoving", false);
        ActivateObjects();
        Star = SceneObjects[2];
        Star.SetActive(false);

        Joker.StartCoroutine(Joker.MoveTo(Joker.transform.position + new Vector3(0, 0, 5)));
        Joker.StartCoroutine(Joker.LookAt(Alice.transform.position, Vector3.up));

        while (Joker.IsMoving) yield return new WaitForEndOfFrame();

        MainCamera.StartCoroutine(MainCamera.Follow(Alice.gameObject, PositionOffsets[0]));
        MainCamera.StartCoroutine(MainCamera.Track(Alice.gameObject, LookPositionOffsets[0]));

        State = SceneState.Active;
        StartCoroutine(Game());
    }

    private IEnumerator Game()
    {
        SpawnCard(3, Joker.transform.position + Vector3.right * 5);
        SpawnCard(7, Joker.transform.position + Vector3.left * 5);

        while (Card.Complete != true) yield return new WaitForEndOfFrame();
        Card.Reset();

        GradientSkybox.ChangeColor(1, Sky, 3);
        GradientSkybox.ChangeColor(2, Ground, 3);

        SpawnCard(2, Joker.transform.position + Vector3.right * 5 + Vector3.up * 0);
        SpawnCard(3, Joker.transform.position + Vector3.down * 5 + Vector3.up * 0);
        SpawnCard(4, Joker.transform.position + Vector3.up * 5 + Vector3.up * 0);
        SpawnCard(5, Joker.transform.position + Vector3.down * 5 + Vector3.left * 5);
        SpawnCard(6, Joker.transform.position + Vector3.down * 5 + Vector3.right * 5);
        SpawnCard(7, Joker.transform.position + Vector3.left * 5 + Vector3.up * 0);
        SpawnCard(8, Joker.transform.position + Vector3.right * 5 + Vector3.up * 5);
        SpawnCard(9, Joker.transform.position + Vector3.left * 5 + Vector3.up * 5);

        while (Card.Complete != true) yield return new WaitForEndOfFrame();
        Card.Reset();

        GradientSkybox.ChangeColor(1, Ground, 3);
        GradientSkybox.ChangeColor(2, Sky, 3);

        Joker.StartCoroutine(Joker.LookAt(Joker.transform.position + Vector3.forward, Vector3.up));
        Joker.StartCoroutine(Joker.MoveTo(Alice.transform.position + Vector3.forward * 10));

        while (Joker.IsMoving) yield return new WaitForEndOfFrame();

        Joker.transform.Find("Card").Find("FrontFace").GetComponent<Renderer>().material = CardMaterials[11];
        Joker.StartCoroutine(Joker.LookAt(Alice.transform.position + Vector3.back, Vector3.up));

        var scale = 4f;
        for (var i = 0f; i < 1; i+= Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            Joker.transform.localScale = Vector3.Lerp(Joker.transform.localScale,
                new Vector3(1.8f * scale, 2.5f * scale, 1 * scale), i);
        }

        GradientSkybox.DoTunnel();
        GradientSkybox.ChangeIntensity(100, 25);

        Star.SetActive(true);
        Joker.StartCoroutine(Joker.LookAt(Alice.transform.position,Vector3.up));
        Joker.Animator.Play("Run");
        Joker.Animator.speed = 2;
        MainCamera.StartCoroutine(MainCamera.MoveTo(PositionOffsets[1]));
        MainCamera.StartCoroutine(MainCamera.Track(Alice.gameObject, LookPositionOffsets[1]));
        Alice.StartCoroutine(Alice.LookAt(Alice.transform.position + Vector3.back, Vector3.up));
        Alice.Animator.SetBool("IsMoving", true);
        Alice.Animator.speed = 2;
    }

    private IEnumerator TransitionToEnding()
    {
        State = SceneState.TransitioningAway;

        MainCamera.StartCoroutine(MainCamera.MoveTo(Star.transform.position));
        MainCamera.StartCoroutine(MainCamera.LookAt(Star.transform.position, Vector3.up));
        GradientSkybox.ChangeColor(2, Color.black, 5);
        GradientSkybox.ChangeColor(1, Color.black, 5);
        MainCamera.MovementSpeed = 15;

        while (MainCamera.IsMoving) yield return new WaitForEndOfFrame();
        GradientSkybox.DoSky();
        MainCamera.MovementSpeed = 2;

        DeactivateObjects();

        State = SceneState.Complete;
    }

    private void SpawnCard(int num, Vector3 pos)
    {
        var card = Instantiate(CardPrefab, Joker.transform.position, Joker.transform.rotation).GetComponent<Card>();
        card.Number = num;
        Card.Cards.Add(card);

        var movement = card.transform.GetComponent<Movement>();
        movement.StartCoroutine(movement.MoveTo(pos));
        movement.StartCoroutine(movement.LookAt(Alice.transform.position + Vector3.back, Vector3.up));

        card.transform.Find("FrontFace").GetComponent<Renderer>().material = CardMaterials[num-1];
    }

}
