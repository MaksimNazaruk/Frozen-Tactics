using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityBehaviour : MonoBehaviour {

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

	protected virtual void Start () {

		gameObject.layer = LayerMask.NameToLayer ("Entities");

		SetupStats ();
		SetupAvailableActions ();
		SetupCommands ();
		SetupEntityUI ();
	}

	protected virtual void SetupStats () {

		Debug.LogError ("SetupStats() method should be implemented in derived classes");
	}

	protected virtual void SetupAvailableActions () {

		availableActions = new List<EntityAction> ();
	}

	protected virtual void SetupCommands () {

		commandsToPerform = new List<EntityCommand> ();
	}

	/// <summary>
	/// Setups in-world UI - a selection ring, maybe healthbars, path line, etc
	/// </summary>
	protected virtual void SetupEntityUI () {

		selectionObject = Instantiate (Resources.Load("Prefabs\\EntityUI\\UnitCursor")) as GameObject;
		selectionObject.transform.SetParent (gameObject.transform, false);
		selectionObject.transform.localPosition = new Vector3 (0, 0, 0);
		selectionObject.transform.localRotation = Quaternion.Euler (0, 0, 0);

		MeshRenderer selectionRenderer = selectionObject.GetComponent<MeshRenderer> ();
		selectionRenderer.enabled = false;
	}

	/// <summary>
	/// Updates the entity's in-world UI. Typically is used to show\hide in-world UI elements
	/// </summary>
	protected virtual void UpdateEntityUI () {

		MeshRenderer selectionRenderer = selectionObject.GetComponent<MeshRenderer> ();
		selectionRenderer.enabled = IsSelected;
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

		if (commandsToPerform.Count > 0) {

			EntityCommand currentCommand = commandsToPerform [0];
			bool isFinished;
			currentCommand.action.actionMethod (currentCommand.target, out isFinished);

			if (isFinished) {
				commandsToPerform.Remove (currentCommand);
			}
		}

	}

	/// <summary>
	/// Update callback that is called only when GameManager is set to frozen mode
	/// </summary>
	protected virtual void UpdateFrozenTime () {

	}
}
