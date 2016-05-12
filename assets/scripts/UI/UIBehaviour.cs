using UnityEngine;
using System.Collections;

public class UIBehaviour : MonoBehaviour {

	public LocalCommander activeCommander;

	protected EntityBehaviour selectedEntity;
	/// <summary>
	/// Currently selected action. Only used when target is required
	/// </summary>
	protected EntityAction activeAction; 

	protected void EndTurn () {

		GameplayManager.SharedInstance ().EndTurn ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
}
