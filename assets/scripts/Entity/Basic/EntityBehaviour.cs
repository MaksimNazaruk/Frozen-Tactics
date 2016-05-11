using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityBehaviour : MonoBehaviour {


	// temp(?) property
	public int editorCommanderId = -1;
	public GameObject teamFlagObject;

	public EntityStats stats;

	public List<EntityAction> availableActions;
	public List<EntityCommand> commandsToPerform;

	bool isSelected = false;
	public bool IsSelected {
		get {
			return isSelected;
		}
		set {
			isSelected = value;
			UpdateEntityUI ();
		}
	}
	GameObject selectionObject;

	protected virtual void Awake () {

		gameObject.layer = LayerMask.NameToLayer ("Entities");

		SetupStats ();
		SetupAvailableActions ();
		SetupCommands ();
		SetupEntityUI ();
	}

	protected virtual void Start () {

		SetupEntityUI ();
	}

	#region Setup

	protected virtual void SetupStats () {

		stats = new EntityStats (); // TODO: consider creating new stats only once. Though currently this code is called only once.

		if (editorCommanderId != -1) { // commanderId was set in the Editor
			
			stats.commanderId = editorCommanderId;
			stats.id = GameplayManager.SharedInstance ().NextEntityIdForCommanderId (stats.commanderId);
		}
	}

	protected virtual void SetupAvailableActions () {

		availableActions = new List<EntityAction> ();
	}

	protected virtual void SetupCommands () {

		commandsToPerform = new List<EntityCommand> ();

		SetupDefaultActions ();
	}
		
	void SetupDefaultActions() {

		// debug demage command
		EntityAction moveAction = new EntityAction ();
		moveAction.title = "Auto Damage";
		moveAction.isTargetRequired = false;
		moveAction.actionMethod = new EntityActionMethod (AutoDamage);

		availableActions.Add (moveAction);
	}

	/// <summary>
	/// Setups in-world UI - a selection ring, maybe healthbars, path line, etc
	/// </summary>
	protected virtual void SetupEntityUI () {

		selectionObject = Instantiate (Resources.Load("Prefabs\\EntityUI\\UnitCursor")) as GameObject;
		selectionObject.transform.SetParent (gameObject.transform, false);
		selectionObject.transform.localPosition = new Vector3 (0, 0.1f, 0);

		MeshRenderer selectionRenderer = selectionObject.GetComponent<MeshRenderer> ();
		selectionRenderer.enabled = false;

		// team color
		if (teamFlagObject != null && stats != null) {

			teamFlagObject.GetComponent<Renderer> ().material.color = GameplayManager.SharedInstance ().ColorForCommanderWithId (stats.commanderId);
		}
	}

	#endregion

	#region Actions

	void AutoDamage(ActionTarget target, out bool isFinished) {

		stats.currentHealth -= 100;
		isFinished = true;
	}

	#endregion

	#region Status

	public bool IsAlive() {

		return (stats.currentHealth > 0);
	}

	#endregion

	/// <summary>
	/// Updates the entity's in-world UI. Typically is used to show\hide in-world UI elements
	/// </summary>
	protected virtual void UpdateEntityUI () {

		MeshRenderer selectionRenderer = selectionObject.GetComponent<MeshRenderer> ();
		selectionRenderer.enabled = IsSelected;
	}

	// TODO: consider using action types instead to avoid using string comparisons
	/// <summary>
	/// Gets the action with title.
	/// </summary>
	/// <returns>Action with input title.</returns>
	/// <param name="actionTitle">Action title.</param>
	public EntityAction GetActionWithTitle(string actionTitle) {

		EntityAction foundAction = null;

		foreach (EntityAction action in availableActions) {

			if (action.title == actionTitle) {

				foundAction = action;
				break;
			}
		}

		return foundAction;
	}

	/// <summary>
	/// Adds the command with provided action.
	/// </summary>
	/// <param name="action">Action.</param>
	/// <param name="target">Target.</param>
	public void AddCommandWithActionAndTarget(EntityAction action, ActionTarget target) {

		EntityCommand command = new EntityCommand ();
		command.action = action;
		command.target = target;

		commandsToPerform.Add (command);
	}

	public virtual void Destroy() {

		commandsToPerform.Clear ();
		IsSelected = false;

		GameplayManager.SharedInstance ().DestroyEntity (this);
	}

//	protected virtual void AnimateDestruction() {
//
//	}

	/// <summary>
	/// Default Unity3D Update callback
	/// </summary>
	protected virtual void Update () {

		if (GameplayManager.SharedInstance ().isRealtime) {
			UpdateRealTime ();
		} else {
			UpdateFrozenTime ();
		}
	}

	/// <summary>
	/// Update callback that is called only when GameManager is set to realtime mode
	/// </summary>
	protected virtual void UpdateRealTime () {

		if (!IsAlive ()) {

			Destroy ();
		}

		if (commandsToPerform.Count > 0) {

//			// ##### debug ####
//			Renderer entityRenderer = gameObject.GetComponent<Renderer> ();
//			entityRenderer.material.color = Color.red;

			EntityCommand currentCommand = commandsToPerform [0];
			bool isFinished;
			currentCommand.action.actionMethod (currentCommand.target, out isFinished);

			if (isFinished) {
				commandsToPerform.Remove (currentCommand);
			}
		} else {

//			// ##### debug ####
//			Renderer entityRenderer = gameObject.GetComponent<Renderer> ();
//			entityRenderer.material.color = Color.green;
		}

	}

	/// <summary>
	/// Update callback that is called only when GameManager is set to frozen mode
	/// </summary>
	protected virtual void UpdateFrozenTime () {

	}
}
