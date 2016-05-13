using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class TouchUIBehaviour : UIBehaviour {

	public Canvas touchUICanvas;

	//Raycasting
	int camRayLength = 1000;
	int groundMask;
	int entityMask;

	// GUI elements

	public Button endTurnButton;

	// TODO: make dynamic GUI
	public Button button0;
	public Button button1;
	public Button button2;
	public Button button3;
	public Button button4;
	public Button button5;

	// info labels
	public Text commanderStatsText;
	public Text commanderInfoText;
	public Text idInfoText;
	public Text classInfoText;
	public Text currentCommandInfoText;
	public Text hpInfoText;

	Button[] allButtons;

	// Use this for initialization
	void Start () {

		allButtons = new Button[] { button0, button1, button2, button3, button4, button5 };

		groundMask = LayerMask.GetMask ("Environment");
		entityMask = LayerMask.GetMask ("Entities");

		SetupStaticUI ();
	}

	protected void SetupStaticUI () {

		endTurnButton.onClick.AddListener (() => EndTurn ());
	}
	
	// Update is called once per frame
	void Update () {

		// GUI update
		if (activeCommander != null) {

			selectedEntity = activeCommander.selectedEntity;
			UpdateUIForSelectedEntity ();

			if (!IsPointerOverUI () && Input.GetMouseButtonDown (0)) {
				
				if (activeAction != null) {

					// selection / targeting
					if (activeAction.isTargetRequired) {
						
						ActionTarget target = RaycastForTarget ();
						if (target != null) {

							AddAction (activeAction, target);
						}

					} else {
						
						// should not happen since activeAction is stored only for getting a target
						RaycastForSelection ();
					}

				} else {
				
					RaycastForSelection ();
				}
			}
		}
	}

	void UpdateTurnUI () {

		if (GameplayManager.SharedInstance ().IsRealtime ()) {

			float activePhaseTimeLeft = GameplayManager.SharedInstance ().ActivePhaseTimeLeft ();
			endTurnButton.GetComponentInChildren<Text> ().text = activePhaseTimeLeft.ToString ("0") + " s left";

		} else {

			endTurnButton.GetComponentInChildren<Text> ().text = "End turn";
		}

		endTurnButton.interactable = !GameplayManager.SharedInstance ().IsRealtime ();
	}

	void UpdateUIForSelectedEntity () {

		UpdateTurnUI ();
		UpdateCommanderStats ();
		UpdateSelectedEntityCommands ();
		UpdateSelectedEntityInfo ();
	}

	void UpdateSelectedEntityCommands () {

		// clean all the buttons
		foreach (Button aButton in allButtons) {
			aButton.enabled = false;
			aButton.gameObject.SetActive (false);
			aButton.GetComponentInChildren<Text> ().text = "---";
			aButton.onClick.RemoveAllListeners();
		}

		if (selectedEntity != null && !GameplayManager.SharedInstance ().IsRealtime()) {

			if (selectedEntity.stats.commanderId == activeCommander.commanderId) {

				List<EntityAction> availableActions = selectedEntity.availableActions;

				// to be safe if we have more actions then buttons
				// TODO: dynamic GUI!!!
				int actionsCount = availableActions.Count;
				if (availableActions.Count > allButtons.Length) {
					actionsCount = allButtons.Length;
				}

				for (int i = 0; i < actionsCount; i++) {
					EntityAction action = availableActions [i];
					Button button = allButtons [i];
					button.enabled = true;
					button.gameObject.SetActive (true);
					button.GetComponentInChildren<Text> ().text = action.title;
					button.onClick.AddListener (() => AddAction (action, null));
				}
			}
		}
	}

	void UpdateCommanderStats() {

		commanderStatsText.text = "B: " + activeCommander.BuildingsCount ().ToString () + " U: " + activeCommander.UnitsCount ().ToString ();
	}

	void UpdateSelectedEntityInfo () {

		if (selectedEntity != null) {

			commanderInfoText.text = "Commander: " + selectedEntity.stats.commanderId.ToString ();
			idInfoText.text = "ID: " + selectedEntity.stats.id.ToString();
			classInfoText.text = "Class: " + selectedEntity.stats.title.ToString ();
			hpInfoText.text = "HP: " + selectedEntity.stats.currentHealth.ToString () + "/" + selectedEntity.stats.fullHealth.ToString ();

			if (selectedEntity.stats.commanderId == activeCommander.commanderId) {
				if (selectedEntity.commandsToPerform.Count > 0) {
					currentCommandInfoText.text = "Commands:\n";
					foreach (EntityCommand command in selectedEntity.commandsToPerform) {
						currentCommandInfoText.text += command.action.title.ToString() + "\n";
					}
				} else {
					currentCommandInfoText.text = "Commands: -";
				}
			} else {
				currentCommandInfoText.text = "Commands: N/A";
			}
		} else {

			commanderInfoText.text = "Commander: -";
			idInfoText.text = "ID: -";
			classInfoText.text = "Class: -";
			hpInfoText.text = "HP: -";
			currentCommandInfoText.text = "Commands: -";
		}
	}

	void AddAction(EntityAction action, ActionTarget target) {

		if (action.isTargetRequired && target == null) {
			activeAction = action;
		} else {
			activeAction = null;
			activeCommander.AddCommandForSelectedEntityWithActionAndTarget (action, target);
		}
	}

	#region Raycast

	void RaycastForSelection () {

		GameObject selectedGameObject;

		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit raycastHit;

		// Trying to select any unit
		if (Physics.Raycast (camRay, out raycastHit, camRayLength, entityMask)) {

			// getting unit object
			if (raycastHit.transform.gameObject != null) {
				selectedGameObject = raycastHit.transform.gameObject;
				activeCommander.SelectEntity(selectedGameObject.GetComponent<EntityBehaviour> ());
			}
		} else {

			selectedGameObject = null;
			activeCommander.SelectEntity(null);
		}
	}

	ActionTarget RaycastForTarget () {

		ActionTarget target = null;

		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit raycastHit;

		// Trying to select any unit
		if (Physics.Raycast (camRay, out raycastHit, camRayLength, entityMask)) {

			// getting entity object
			if (raycastHit.transform.gameObject != null) {
				
				target = new ActionTarget ();
				GameObject targetedGameObject = raycastHit.transform.gameObject;
				target.Entity = targetedGameObject.GetComponent<EntityBehaviour> ();
			}

		} else { // no object was selected as a target. trying to get a point on the surface to target

			if (Physics.Raycast (camRay, out raycastHit, camRayLength, groundMask)) {

				target = new ActionTarget ();
				target.Position = raycastHit.point;
			}
		}

		return target;
	}

	bool IsPointerOverUI () {

		bool isPointerOverGameObject = false;

		#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER

		isPointerOverGameObject = EventSystem.current.IsPointerOverGameObject();

		#else

		foreach(Touch touch in Input.touches) {
			if(touch.phase != TouchPhase.Canceled && touch.phase != TouchPhase.Ended) {
				if(EventSystem.current.IsPointerOverGameObject(touch.fingerId)) {
					isPointerOverGameObject = true;
					break;
				}
			}
		}


		#endif

		return isPointerOverGameObject;
	}

	#endregion
}
