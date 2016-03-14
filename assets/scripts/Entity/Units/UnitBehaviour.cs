using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBehaviour : EntityBehaviour {

	public new UnitStats stats;

	// navigation
	NavMeshAgent navMeshAgent;

	// Use this for initialization
	protected override void Start () {
		
		base.Start ();

		SetupNavMeshAgent ();
	}

	/// <summary>
	/// setups basic stats, common for all the units
	/// </summary>
	protected override void SetupStats () {

		stats = new UnitStats ();
		stats.basicType = EntityStats.BasicType.BasicTypeUnit;
	}

	/// <summary>
	/// setups default available actions, common for all the units 
	/// </summary>
	protected override void SetupAvailableActions () {

		base.SetupAvailableActions ();

		// basic Move action
		EntityAction moveAction = new EntityAction ();
		moveAction.title = "Move";
		moveAction.isTargetRequired = true;
		moveAction.actionMethod = new EntityActionMethod (MoveToTarget);

		availableActions.Add (moveAction);
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

	// ########## Action Methods ###########

	void MoveToTarget(ActionTarget target, out bool isFinished) {

		float distance = Vector3.Distance (gameObject.transform.position, target.Position);
		if (distance > StopDistance ()) {
			
			isFinished = false;
			if (navMeshAgent.destination != target.Position) {
				navMeshAgent.SetDestination (target.Position);
			}

		} else {

			isFinished = true;
		}
	}

	float StopDistance () {

		return stats.size / 2.0f + 0.5f;
	}

	// ########## Update methods ###########

	protected override void UpdateRealTime () {

		base.UpdateRealTime ();

		navMeshAgent.Resume ();
	}

	protected override void UpdateFrozenTime () {

		base.UpdateFrozenTime ();

		navMeshAgent.Stop ();
	}
}
