﻿using UnityEngine;
using System.Collections;

public class BuildingBehaviour : EntityBehaviour {

	public BuildingStats buildingStats;

	NavMeshObstacle navMeshObstacle;

	// Unit Building properties
	float buildingTimerCurrentValue = 0.0f;
	EntityBlueprint currentBlueprint;
	bool isUnitBuildingFinished;

	// Rally point properties
	Vector3 rallyPoint;
	public void SetRallyPoint (Vector3 newDestinationPoint) {
		rallyPoint = newDestinationPoint;
		UpdateEntityUI ();
	}
	GameObject rallyPointMarkerObject;

	// Use this for initialization
	protected override void Awake () {

		base.Awake ();

		SetupNavMeshObstacle ();
		SetupDefaultRallyPoint ();
	}

	protected override void SetupStats () {
		
		base.SetupStats ();
		stats.basicType = EntityStats.BasicType.BasicTypeBuilding;

		// building specific stats
		buildingStats = new BuildingStats ();
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

	protected override void SetupEntityUI () {

		base.SetupEntityUI ();

		rallyPointMarkerObject = Instantiate (Resources.Load("Prefabs\\EntityUI\\RallyPoint")) as GameObject;
		rallyPointMarkerObject.transform.position = rallyPoint;

		MeshRenderer rallyPointMarkerRenderer = rallyPointMarkerObject.GetComponent<MeshRenderer> ();
		rallyPointMarkerRenderer.enabled = false;
	}

	protected override void UpdateEntityUI () {

		base.UpdateEntityUI ();

		// TODO: stub to compensate for a high origin point. Need to make correct origin point and get rid of this custom position
		Vector3 rallyPointMarkerPosition = rallyPoint;
		rallyPointMarkerPosition.y = 4.0f;

		rallyPointMarkerObject.transform.position = rallyPointMarkerPosition;

		MeshRenderer rallyPointMarkerRenderer = rallyPointMarkerObject.GetComponent<MeshRenderer> ();
		rallyPointMarkerRenderer.enabled = IsSelected;
	}

	void SetupNavMeshObstacle () {

		navMeshObstacle = gameObject.AddComponent<NavMeshObstacle> ();
		navMeshObstacle.carving = true;
		navMeshObstacle.size = new Vector3 (1.5f, 1.0f, 1.5f);
	}

	// TODO: update default rally point y position. looks like it's not reachable for the units. seems to be OK now o__O
	void SetupDefaultRallyPoint () {

		Vector3 defaultRallyPoint = gameObject.transform.position;
		defaultRallyPoint.z += -(stats.size / 2.0f + 10.0f);
		defaultRallyPoint.y = 0.0f;
		SetRallyPoint (defaultRallyPoint);
	}


	// ######### Actions #########

	void SetRallyPointAction(ActionTarget target, out bool isFinished) {

		SetRallyPoint (target.Position);
		isFinished = true;
	}


	// ########## Unit building common logic ###########
	#region Unit production logic

	protected void BuildUnit (EntityBlueprint unitBlueprint, out bool isFinished) {

		if (!IsBuildingUnit () && !isUnitBuildingFinished) {
			StartBuildingUnit (unitBlueprint);
		}
		isFinished = isUnitBuildingFinished;

		if (isUnitBuildingFinished) {
			isUnitBuildingFinished = false;
		}
	}

	protected void StartBuildingUnit (EntityBlueprint blueprint) {

		isUnitBuildingFinished = false;
		currentBlueprint = blueprint;
		buildingTimerCurrentValue = currentBlueprint.buildTime;
	}

	protected void FinishBuildingUnit () {
		
		// calculating unit position to be just outside of the building in derection of a rally point
		Vector3 unitOffset = rallyPoint - gameObject.transform.position;
		Vector3 unitPosition = gameObject.transform.position + unitOffset.normalized * (stats.size / 2.0f);

		// creating unit
		GameObject unitObject = GameplayManager.SharedInstance ().CreateEntity (currentBlueprint, stats.commanderId, unitPosition);

		UnitBehaviour unitBehaviour = unitObject.GetComponent<UnitBehaviour> ();

		// adding move action to the rally point
		EntityAction moveAction = unitBehaviour.GetMoveAction ();
		ActionTarget moveTarget = new ActionTarget ();
		moveTarget.Position = rallyPoint;
		unitBehaviour.AddCommandWithActionAndTarget (moveAction, moveTarget);

		// finishing
		currentBlueprint = null;
		isUnitBuildingFinished = true;
	}

	public bool IsBuildingUnit () {

		if ((currentBlueprint != null) && (buildingTimerCurrentValue > 0)) {
			return true;
		} else {
			return false;
		}
	}

	#endregion


	// ########## Updates ########
	#region Updates

	protected override void UpdateRealTime () {

		base.UpdateRealTime ();

		if (buildingTimerCurrentValue > 0) {
			
			buildingTimerCurrentValue -= Time.deltaTime;

			if (buildingTimerCurrentValue <= 0) {

				if (currentBlueprint != null) {
					FinishBuildingUnit ();
				}

				buildingTimerCurrentValue = 0.0f;
			}
		}
	}

	protected override void UpdateFrozenTime () {

		base.UpdateFrozenTime ();
	}

	#endregion

}
