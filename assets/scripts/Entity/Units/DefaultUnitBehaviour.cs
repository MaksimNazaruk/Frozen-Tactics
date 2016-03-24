using UnityEngine;
using System.Collections;

public class DefaultUnitBehaviour : UnitBehaviour {

	// Use this for initialization
	protected override void Awake () {
	
		base.Awake ();
	}

	protected override void SetupStats () {

		base.SetupStats ();

		stats.title = "Default Unit";
		stats.fullHealth = 100.0f;
		stats.currentHealth = stats.fullHealth;
		stats.size = 1.0f;
		stats.attackRange = 7.0f;
		stats.visionRange = 10.0f;

		unitStats.speed = 10.0f;
	}

	protected override void SetupAvailableActions () {

		base.SetupAvailableActions ();
	}

	protected override void UpdateRealTime () {

		base.UpdateRealTime ();
	}

	protected override void UpdateFrozenTime () {

		base.UpdateFrozenTime ();
	}
}
