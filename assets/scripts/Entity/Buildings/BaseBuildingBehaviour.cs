using UnityEngine;
using System.Collections;

public class BaseBuildingBehaviour : BuildingBehaviour {
	
	// Use this for initialization
	protected override void Start () {

		base.Start ();
	}

	protected override void SetupStats () {

		base.SetupStats ();

		stats.title = "Main Base";
		stats.buildTime = 20.0f;
		stats.fullHealth = 2000.0f;
		stats.currentHealth = stats.fullHealth;
		stats.size = 1.0f;
		stats.attackRange = 0.0f;
		stats.visionRange = 10.0f;
	}

	protected override void UpdateRealTime () {

		base.UpdateRealTime ();

		Debug.Log ("UpdateRealTime Base Building");
	}

	protected override void UpdateFrozenTime () {

		base.UpdateFrozenTime ();

		Debug.Log ("UpdateFrozenTime Base Building");
	}
}
