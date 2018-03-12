using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerGarden : Scene {

    [HideInInspector]
    public Movement Alice;
    [HideInInspector]
    public Movement MainCamera;
    [HideInInspector]
    public SkyboxController GradientSkybox;

    public Color Sky;
    public Color Ground;

    public GameObject[] FlowerPrefabs;

    public Movement Joker;

    public override bool Construct(string pastSceneName)
    {
        switch (pastSceneName)
        {
            case "DownTheRabbitHole":
                StartCoroutine(TransitionFromDownTheRabbitHole(SceneChanger.CurrentScene as DownTheRabbitHole));
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

        var triggerFlower = SceneObjects[0].GetComponent<Flower>();
        triggerFlower.gameObject.SetActive(true);
        triggerFlower.StartCoroutine(triggerFlower.Shrink());

        GradientSkybox.ChangeColor(1, Ground);
        GradientSkybox.ChangeColor(2, Sky);
        GradientSkybox.ChangeIntensity(1, 2);

        MainCamera.StartCoroutine(MainCamera.MoveTo(Alice.transform.position + Vector3.up * 5));
        MainCamera.StartCoroutine(MainCamera.Track(Alice.gameObject, LookPositionOffsets[0]));


        while (GradientSkybox.Intensity < 1) yield return new WaitForEndOfFrame();

        ActivateObjects();

        while (Alice.IsMoving) yield return new WaitForEndOfFrame();

        MainCamera.StartCoroutine(MainCamera.RotateAround(Alice.transform.position, PositionOffsets[0]));

        State = SceneState.Active;
        StartCoroutine(Game());
    }

    private IEnumerator Game()
    {
        var k = 0;
        var positions = new List<Vector2>();
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                positions.Add(new Vector2(i, j));
            }
        }
        positions.Sort((a, b) => 1 - 2 * Random.Range(0, 1));
        for (int i = 0; i < 6*6; i++)
        {
            SpawnFlower(i%FlowerPrefabs.Length,positions[i]);
            yield return new WaitForSeconds(0.2f);
        }

        MainCamera.MovementSpeed = 1;
        while (!Flower.BedCleared()) yield return new WaitForEndOfFrame();
        MainCamera.MovementSpeed = 5;
        MainCamera.RotationSpeed = 3;
        MainCamera.StartCoroutine(MainCamera.MoveTo(Alice.transform.position + PositionOffsets[1]));
        MainCamera.StartCoroutine(MainCamera.LookAt(Alice.transform.position + LookPositionOffsets[1], Vector3.up));
        DeactivateObjects();

        while (MainCamera.IsMoving) yield return new WaitForEndOfFrame();

        Joker.gameObject.SetActive(true);
        Joker.RotationSpeed = 1;
        Joker.StartCoroutine(Joker.LookAt(Joker.transform.position + new Vector3(0,0,1),Vector3.up));

    }

    private IEnumerator TransitionToQueenOfHearts()
    {
        State = SceneState.TransitioningAway;

        Alice.MovementSpeed = 2;
        //Alice.StartCoroutine(Alice.LookAt(Joker.transform.position, Vector3.up));
        Alice.StartCoroutine(Alice.MoveTo(Joker.transform.position + new Vector3(0, 0, -1)));
        Alice.Animator.SetBool("IsMoving", true);

        while (Alice.IsMoving) yield return new WaitForEndOfFrame();

        Joker.RotationSpeed = 10;
        State = SceneState.Complete;
    }

    private void SpawnFlower(int num, Vector2 pos)
    {
        var flower = Instantiate(FlowerPrefabs[num], PositionInFlowerBed(pos) , Quaternion.identity).GetComponent<Flower>();
        flower.transform.localScale = new Vector3(0,0,0);
        flower.StartCoroutine(flower.Grow());
        Flower.Flowers.Add(flower);
    }

    private Vector3 PositionInFlowerBed(Vector2 cell)
    {
        var pos = Alice.transform.position;
        cell -= new Vector2(3,3);
        if (cell.x >= 0)
            cell.x++;
        if (cell.y >= 0)
            cell.y++;
        return pos + new Vector3(cell.x * 5, 0, cell.y * 5);
    }
}
