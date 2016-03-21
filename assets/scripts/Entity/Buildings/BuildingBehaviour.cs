using UnityEngine;
using System.Collections;

public class BuildingBehaviour : EntityBehaviour {

	public new BuildingStats stats;

	NavMeshObstacle navMeshObstacle;

	// Unit Building properties
	float buildingTimerCurrentValue = 0.0f;
	EntityBlueprint currentBlueprint;
	protected bool isUnitBuildingFinished;

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

	void SetupDefaultRallyPoint () {

		Vector3 defaultRallyPoint = gameObject.transform.position;
		defaultRallyPoint.x += stats.size / 2.0f + 15.0f;
		SetRallyPoint (defaultRallyPoint);
	}


	// ######### Actions #########

	void SetRallyPointAction(ActionTarget target, out bool isFinished) {

		SetRallyPoint (target.Position);
		isFinished = true;
	}

	// ########## Unit building common logic ###########

	protected void StartBuildingUnit (EntityBlueprint blueprint) {

		isUnitBuildingFinished = false;
		currentBlueprint = blueprint;
		buildingTimerCurrentValue = currentBlueprint.buildTime;
	}

	protected void FinishBuildingUnit () {

		GameObject unitObject = Instantiate (Resources.Load(currentBlueprint.prefabName)) as GameObject;
		Vector3 unitPosition = gameObject.transform.position;
		unitPosition.x += stats.size / 2.0f + 2.0f;
		unitObject.transform.position = unitPosition;
		UnitBehaviour unitBehaviour = unitObject.GetComponent<UnitBehaviour> ();

		// send new unit to the rally point
		if (unitBehaviour != null) {

			EntityAction moveAction = unitBehaviour.GetMoveAction ();
			ActionTarget moveTarget = new ActionTarget ();
			moveTarget.Position = rallyPoint;
			unitBehaviour.AddCommandWithActionAndTarget (moveAction, moveTarget);
		}

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

	// ########## Updates ########

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

}
