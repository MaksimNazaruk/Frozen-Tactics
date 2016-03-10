using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CommanderType { LocalCommanderType, RemoteCommanderType, CPUCommanderType };

public class Commander {

	public int commanderId;
	public CommanderType commanderType;

	List<UnitBehaviour> allUnits;
	List<BuildingBehaviour> allBuildings;



}
