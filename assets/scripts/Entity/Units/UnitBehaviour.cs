using UnityEngine;
using System.Collections;

public class UnitBehaviour : EntityBehaviour {

	public new UnitStats stats;

	// navigation
	NavMeshAgent navMeshAgent;
	Vector3 destinationPoint;
	public void SetDestinationPoint (Vector3 newDestinationPoint) {
		destinationPoint = newDestinationPoint;
	}

	// Use this for initialization
	protected override void Start () {
		
		base.Start ();

		SetupNavMeshAgent ();
	}

	protected override void SetupStats () {

		stats = new UnitStats ();
		stats.basicType = EntityStats.BasicType.BasicTypeUnit;
	}

	void SetupNavMeshAgent () {

		// TODO: remove NavMeshAgent in case we have one???

		navMeshAgent = gameObject.AddComponent <NavMeshAgent>();
		navMeshAgent.acceleration = 100.0f;
		navMeshAgent.speed = stats.speed;
		navMeshAgent.angularSpeed = 1000.0f; // high speed for instant turns
		navMeshAgent.radius = stats.size / 2.0f;
		navMeshAgent.Stop ();
	}

	protected void UpdateNavMeshAgentParameters () {

		navMeshAgent.speed = stats.speed;
		navMeshAgent.radius = stats.size / 2.0f;
	}

	protected override void UpdateRealTime () {

		base.UpdateRealTime ();

		// TODO: set destination only once. maybe set it to null when there's no destination
		navMeshAgent.destination = destinationPoint;
		navMeshAgent.Resume ();
	}

	protected override void UpdateFrozenTime () {

		base.UpdateFrozenTime ();

		navMeshAgent.Stop ();
	}
}
