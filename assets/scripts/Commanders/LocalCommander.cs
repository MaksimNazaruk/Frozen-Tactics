using UnityEngine;
using System.Collections;

public class LocalCommander : Commander {

	public EntityBehaviour selectedEntity = null; // TODO: consider multiple selection

	public EntityAction[] AvailableActionsOfSelectedEntity () {

		if (selectedEntity == null) {
			return null;
		} else {
			return selectedEntity.availableActions.ToArray ();
		}
	}

	public void AddCommantForSelectedEntityWithActionAndTarget(EntityAction action, ActionTarget target) {

		if (selectedEntity == null) {
			return;
		} 

		selectedEntity.AddCommandWithActionAndTarget (action, target);
	}
}
