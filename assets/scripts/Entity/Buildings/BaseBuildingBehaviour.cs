using UnityEngine;
using System.Collections;

public class BaseBuildingBehaviour : BuildingBehaviour {

	// Use this for initialization
	protected override void Awake () {

		base.Awake ();
	}

	protected override void SetupStats () {

		base.SetupStats ();

		stats.title = "Main Base";
		stats.fullHealth = 2000.0f;
		stats.currentHealth = stats.fullHealth;
		stats.size = 5.0f;
		stats.attackRange = 0.0f;
		stats.visionRange = 10.0f;
	}

	protected override void SetupAvailableActions ()
	{
		base.SetupAvailableActions ();

		EntityAction buildDefaultUnitAction = new EntityAction ();
		buildDefaultUnitAction.title = "Build Default Unit";
		buildDefaultUnitAction.isTargetRequired = false;
		buildDefaultUnitAction.actionMethod = new EntityActionMethod (BuildDefaultUnit);

		availableActions.Add (buildDefaultUnitAction);
	}

	// ########### Actions ############

	void BuildDefaultUnit (ActionTarget target, out bool isFinished) {

		BuildUnit (EntityLibrary.DefaultUnit (), out isFinished);
	}

	// ########### Updates ############
		
	protected override void UpdateRealTime () {

		base.UpdateRealTime ();
	}

	protected override void UpdateFrozenTime () {

		base.UpdateFrozenTime ();
	}
}
