using UnityEngine;
using System.Collections;

public class EntityBehaviour : MonoBehaviour {

	public EntityStats stats;

	protected virtual void Start () {
	
		SetupStats ();
	}

	protected virtual void SetupStats () {

		Debug.LogError ("SetupStats() method should be implemented in derived classes");
	}
		
	protected virtual void Update () {

		Debug.Log ("entity Update");

		if (GameplayManager.SharedInstance ().isRealtime) {
			UpdateRealTime ();
		} else {
			UpdateFrozenTime ();
		}
	}

	protected virtual void UpdateRealTime () {

		Debug.Log("UpdateRealTime Entity");
	}

	protected virtual void UpdateFrozenTime () {

		Debug.Log("UpdateFrozenTime Entity");
	}
}
