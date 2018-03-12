using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateSceneOnClick : MonoBehaviour
{
    public Scene SceneToActivate;

	void OnClick () {
		SceneChanger.ChangeScene(SceneToActivate);
	}
}
