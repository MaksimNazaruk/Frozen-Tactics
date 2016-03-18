using UnityEngine;
using System.Collections;

// TODO: consider making separate libraries for units and buildings
public class EntityLibrary {

//	static EntityLibrary sharedInstance;
//	public static EntityLibrary SharedInstance () {
//
//		if (sharedInstance == null) {
//			sharedInstance = new EntityLibrary ();
//		}
//
//		return sharedInstance;
//	}

	#region Units

	// TODO: consider making static instances to avoid creating new instances each time
	public static EntityBlueprint DefaultUnit () {

		EntityBlueprint defaultUnitBlueprint = new EntityBlueprint ();
		defaultUnitBlueprint.title = "Default unit";
		defaultUnitBlueprint.prefabName = "Prefabs\\Units\\DefaultUnit";
		defaultUnitBlueprint.buildTime = 3.0f;
		defaultUnitBlueprint.buildCost = 50.0f;

		return defaultUnitBlueprint;
	}

	#endregion

	#region Buildings

	public static EntityBlueprint MainBase () {

		EntityBlueprint mainBaseBlueprint = new EntityBlueprint ();
		mainBaseBlueprint.title = "Main base";
		mainBaseBlueprint.prefabName = "Prefabs\\Buildings\\MainBase";
		mainBaseBlueprint.buildTime = 10.0f;
		mainBaseBlueprint.buildCost = 500.0f;

		return mainBaseBlueprint;
	}

	#endregion
}
