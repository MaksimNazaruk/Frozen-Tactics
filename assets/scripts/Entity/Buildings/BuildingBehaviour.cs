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

	void SetupNavMeshObstacle () {

		navMeshObstacle = gameObject.AddComponent<NavMeshObstacle> ();
		navMeshObstacle.carving = true;
	}

	void SetupDefaultRallyPoint () {

		Vector3 defaultRallyPoint = gameObject.transform.position;
		defaultRallyPoint.x += stats.size / 2.0f + 1.0f;
		SetRallyPoint (defaultRallyPoint);
	}

	protected override void UpdateRealTime () {

		base.UpdateRealTime ();


	}

	protected override void UpdateFrozenTime () {

		base.UpdateFrozenTime ();
	}

}
