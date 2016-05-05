using UnityEngine;
using System.Collections;

public class GameplayManagerBehaviour : MonoBehaviour {

	public GameObject uiManagerObject;
	UIBehaviour uiBehaviour;

	// Use this for initialization
	void Awake () {
	
		uiBehaviour = uiManagerObject.GetComponent<UIBehaviour> ();
		SetupCommanders ();
	}
		
	void SetupCommanders () {

		// for now it's only one local commander
		GameplayManager.SharedInstance().AddCommanderWithType(CommanderType.LocalCommanderType);
		uiBehaviour.activeCommander = GameplayManager.SharedInstance ().CurrentLocalCommander ();
	}

	// Update is called once per frame
	void Update () {
	
		GameplayManager.SharedInstance ().UpdateCommanders ();
	}
}
