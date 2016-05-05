using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CommanderType { LocalCommanderType, RemoteCommanderType, CPUCommanderType };

public class Commander {

	public int commanderId;
	public CommanderType commanderType;

	List<UnitBehaviour> allUnits;
	List<BuildingBehaviour> allBuildings;

	public Commander() {

		allBuildings = new List<BuildingBehaviour> ();
		allUnits = new List<UnitBehaviour> ();
	}

	public void UpdateAliveEntitiesLists() {

		// updating buildings
		foreach (BuildingBehaviour aBuilding in allBuildings) {

			if (aBuilding == null) {

				allBuildings.Remove (aBuilding);
			} else {

				if (!aBuilding.IsAlive ()) {
					allBuildings.Remove (aBuilding);
				}
			}
		}

		// updating units
		foreach (UnitBehaviour aUnit in allUnits) {

			if (aUnit == null) {

				allUnits.Remove (aUnit);
			} else {

				if (!aUnit.IsAlive ()) {
					allUnits.Remove (aUnit);
				}
			}
		}
	}

}
