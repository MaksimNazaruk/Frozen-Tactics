using UnityEngine;
using System;

public class CPUCommander : Commander {

	public void UpdateAI () {

		// stubs
		if (EntitiesCount () < 10) {

			BaseBuildingBehaviour mainBaseBehaviour = MainBaseBuildingBehaviour ();

			Debug.Log("main base commander = " + mainBaseBehaviour.stats.commanderId);

			if (mainBaseBehaviour) {
				
				// adding build default unit action
				EntityAction buildDefaultUnitAction = mainBaseBehaviour.GetActionWithTitle("Build Default Unit");
				mainBaseBehaviour.AddCommandWithActionAndTarget (buildDefaultUnitAction, null);
			}
		}
	}

	protected BaseBuildingBehaviour MainBaseBuildingBehaviour () {

		BaseBuildingBehaviour baseBuildingBehaviour = null;

		foreach (WeakReference anEntityReference in entities) {

			EntityBehaviour entityBehaviour = anEntityReference.Target as EntityBehaviour;
			if (entityBehaviour is BaseBuildingBehaviour) {
				baseBuildingBehaviour = entityBehaviour as BaseBuildingBehaviour;
				break;
			}
		}

		return baseBuildingBehaviour;
	}
}
