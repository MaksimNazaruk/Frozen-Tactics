using UnityEngine;
using System.Collections;

public class BuildingBehaviour : EntityBehaviour {

	public new BuildingStats stats;

	NavMeshObstacle navMeshObstacle;

	Vector3 rallyPoint;
	public void SetRallyPoint (Vector3 newDestinationPoint) {
		rallyPoint = newDestinationPoint;
	}

	// Use this for initialization
	protected override void Start () {

		base.Start ();

		SetupNavMeshObstacle ();
		SetupDefaultRallyPoint ();
	}

	protected override void SetupStats () {

		stats = new BuildingStats ();
		stats.basicType = EntityStats.BasicType.BasicTypeBuilding;
	}

	protected override void SetupAvailableActions ()
	{
		base.SetupAvailableActions ();

		EntityAction setRallyPointAction = new EntityAction ();
		setRallyPointAction.title = "Set Rally Point";
		setRallyPointAction.isTargetRequired = true;
		setRallyPointAction.actionMethod = new EntityActionMethod (SetRallyPointAction);

		availableActions.Add (setRallyPointAction);
	}

	void SetupNavMeshObstacle () {

		navMeshObstacle = gameObject.AddComponent<NavMeshObstacle> ();
		navMeshObstacle.carving = true;
		navMeshObstacle.size = new Vector3 (1.5f, 1.0f, 1.5f);
	}

	void SetupDefaultRallyPoint () {

		Vector3 defaultRallyPoint = gameObject.transform.position;
		defaultRallyPoint.x += stats.size / 2.0f + 1.0f;
		SetRallyPoint (defaultRallyPoint);
	}


	// ######### Actions #########

	void SetRallyPointAction(ActionTarget target, out bool isFinished) {

		SetRallyPoint (target.Position);
		isFinished = true;
	}


	protected override void UpdateRealTime () {

		base.UpdateRealTime ();


	}

	protected override void UpdateFrozenTime () {

		base.UpdateFrozenTime ();
	}

}
