using UnityEngine;
using System.Collections;

public class LocalCommander : Commander {

	public EntityBehaviour selectedEntity = null; // TODO: consider multiple selection

	public void SelectEntity (EntityBehaviour entity) {

		if (entity == selectedEntity) {
			return;
		}

		selectedEntity.IsSelected = false;
		selectedEntity = entity;
		if (selectedEntity != null) {
			selectedEntity.IsSelected = true;
		}
	}

	public EntityAction[] AvailableActionsOfSelectedEntity () {

		if (selectedEntity == null) {
			return null;
		} else {
			return selectedEntity.availableActions.ToArray ();
		}
	}

	public void AddCommandForSelectedEntityWithActionAndTarget(EntityAction action, ActionTarget target) {

		if (selectedEntity == null) {
			return;
		} 

		selectedEntity.AddCommandWithActionAndTarget (action, target);
	}
}
