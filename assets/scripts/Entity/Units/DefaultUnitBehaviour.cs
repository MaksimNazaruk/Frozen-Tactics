using UnityEngine;
using System.Collections;

public class DefaultUnitBehaviour : UnitBehaviour {

	// Use this for initialization
	protected override void Start () {
	
		base.Start ();
	}

	protected override void SetupStats () {

		base.SetupStats ();

		stats.title = "Dafault Unit";
		stats.buildTime = 3.0f;
		stats.fullHealth = 100.0f;
		stats.currentHealth = stats.fullHealth;
		stats.size = 1.0f;
		stats.speed = 10.0f;
		stats.attackRange = 7.0f;
		stats.visionRange = 10.0f;
	}

	protected override void UpdateRealTime () {

		base.UpdateRealTime ();

		Debug.Log ("UpdateRealTime Default Unit");
	}

	protected override void UpdateFrozenTime () {

		base.UpdateFrozenTime ();

		Debug.Log ("UpdateFrozenTime Default Unit");
	}
}
