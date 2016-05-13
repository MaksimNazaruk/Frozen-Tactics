using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitBehaviour : EntityBehaviour {

	public UnitStats unitStats;

	// navigation
	NavMeshAgent navMeshAgent;
	NavMeshObstacle navMeshObstacle;

	// Use this for initialization
	protected override void Awake () {
		
		base.Awake ();

		SetupNavMeshAgent ();
	}

	#region Life cycle and setup methods

	/// <summary>
	/// setups basic stats, common for all the units
	/// </summary>
	protected override void SetupStats () {

		base.SetupStats ();
		stats.basicType = EntityStats.BasicType.BasicTypeUnit;

		// unit specific stats
		unitStats = new UnitStats ();
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

		navMeshAgent = gameObject.AddComponent <NavMeshAgent> ();
		navMeshAgent.acceleration = 100.0f;                                                                                                                                                                           
		navMeshAgent.speed = unitStats.speed;
		navMeshAgent.angularSpeed = 1000.0f; // high speed for instant turns
		navMeshAgent.radius = stats.size / 2.0f;
		navMeshAgent.enabled = false;

		navMeshObstacle = gameObject.AddComponent<NavMeshObstacle> ();
		navMeshObstacle.carving = true;
		navMeshObstacle.size = new Vector3 (1.5f, 1.0f, 1.5f);
		navMeshObstacle.enabled = true;
	}

	protected void UpdateNavMeshAgentParameters () {

		navMeshAgent.speed = unitStats.speed;
		navMeshAgent.radius = stats.size / 2.0f;
	}

	public override void Destroy() {

		// custom logic for destroy

		base.Destroy ();
	}

	#endregion

	#region Action getter methods

	public EntityAction GetMoveAction () {

		EntityAction moveAction = GetActionWithTitle ("Move");
		return moveAction;
	}

	#endregion

	#region Action Methods

	void MoveToTarget(ActionTarget target, out bool isFinished) {

		float distance = Vector3.Distance (gameObject.transform.position, target.Position);
		bool isTargetReachable = true;

		if (navMeshAgent.enabled && !navMeshAgent.pathPending) {

			isTargetReachable = (navMeshAgent.pathStatus == NavMeshPathStatus.PathComplete) && navMeshAgent.hasPath;
		}

		if (distance > StopDistance () && isTargetReachable) {

			navMeshObstacle.enabled = false;
			navMeshAgent.enabled = true;

			isFinished = false;
			if (navMeshAgent.destination != target.Position) {
				navMeshAgent.SetDestination (target.Position);
			}

		} else {

			navMeshAgent.enabled = false;
			navMeshObstacle.enabled = true;
			isFinished = true;
		}
	}

	#endregion

	#region Helper methods

	float StopDistance () {

		return stats.size / 2.0f + 0.5f;
	}

	#endregion

	#region Update methods

	protected override void UpdateRealTime () {

		base.UpdateRealTime ();

		if (navMeshAgent.isActiveAndEnabled) {
			navMeshAgent.Resume ();
		}
	}

	protected override void UpdateFrozenTime () {

		base.UpdateFrozenTime ();

		if (navMeshAgent.isActiveAndEnabled) {
			navMeshAgent.Stop ();
		}
	}

	#endregion
}
