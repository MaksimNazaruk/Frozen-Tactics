using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TouchUIBehaviour : MonoBehaviour {

	public Canvas touchUICanvas;
	public LocalCommander activeCommander;

	EntityBehaviour selectedEntity;
	/// <summary>
	/// Currently selected action. Only used when target is required
	/// </summary>
	EntityAction activeAction; 

	// GUI elements

	// TODO: make dynamic GUI
	public Button button0;
	public Button button1;
	public Button button2;
	public Button button3;
	public Button button4;
	public Button button5;

	Button[] allButtons;

	// Use this for initialization
	void Start () {

		allButtons = new Button[] { button0, button1, button2, button3, button4, button5 };
	}
	
	// Update is called once per frame
	void Update () {
	
		// GUI update
		if (activeCommander != null) {
			if (activeCommander.selectedEntity != null && activeCommander.selectedEntity != selectedEntity) {

				selectedEntity = activeCommander.selectedEntity;
				UpdateUIForSelectedEntity ();
			}
		}

		// selection / targeting

		if (isTargetRequired) {
			RaycastForTarget ();
		} else {
			RaycastForSeleciton ();
		}
	}

	void UpdateUIForSelectedEntity () {

		// clean all the buttons
		foreach (Button aButton in allButtons) {
			aButton.GetComponentInChildren<Text> ().text = "---";
			aButton.onClick.RemoveAllListeners;
		}

		if (selectedEntity != null) {
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
				button.GetComponentInChildren<Text> ().text = action.title;
				button.onClick.AddListener (() => AddAction (action));
			}
		}
	}

	void AddAction(EntityAction action) {

		if (action.isTargetRequired) {
			activeAction = action;
		} else {
			activeAction = null;
			activeCommander.AddCommandForSelectedEntityWithActionAndTarget (action, null);
		}
	}

	// ###### Raycast ########

	void RaycastForSeleciton () {


	}

	void RaycastForTarget () {


	}
}
