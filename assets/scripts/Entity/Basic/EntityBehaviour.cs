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
			MeshRenderer selectionRenderer = selectionObject.GetComponent<MeshRenderer> ();
			selectionRenderer.enabled = value;
		}
	}
	GameObject selectionObject;

	protected virtual void Start () {
	
		SetupStats ();
		SetupAvailableActions ();
		SetupCommands ();
		SetupEntityUI ();
	}

	protected virtual void SetupStats () {

		Debug.LogError ("SetupStats() method should be implemented in derived classes");
	}

	protected virtual void SetupAvailableActions () {

		Debug.LogError ("SetupActions() method should be implemented in derived classes");
	}

	protected virtual void SetupCommands () {

		commandsToPerform = new List<EntityCommand> ();
	}
		
	// setups selection ring, maybe healthbars, path line, etc
	void SetupEntityUI () {

		selectionObject = Instantiate (Resources.Load("Prefabs\\UnitCursor")) as GameObject;
		selectionObject.transform.SetParent (gameObject.transform, false);
		selectionObject.transform.localPosition = new Vector3 (0, 0, 0);
		selectionObject.transform.localRotation = Quaternion.Euler (0, 0, 0);

		MeshRenderer selectionRenderer = selectionObject.GetComponent<MeshRenderer> ();
		selectionRenderer.enabled = false;
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

	}

	/// <summary>
	/// Update callback that is called only when GameManager is set to frozen mode
	/// </summary>
	protected virtual void UpdateFrozenTime () {

	}
}
