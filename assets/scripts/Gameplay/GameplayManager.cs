using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameplayManager {

	static GameplayManager sharedInstance;
	public static GameplayManager SharedInstance () {

		if (sharedInstance == null) {
			sharedInstance = new GameplayManager ();
		}

		return sharedInstance;
	}

	public bool isRealtime;

	Dictionary<Commander, int> lastEntityIdForCommanders;

	GameplayManager () {

		commanders = new List <Commander> ();
		isRealtime = true;
		lastEntityIdForCommanders = new Dictionary<Commander, int>();
	}	

	#region Commanders

	protected List<Commander> commanders;

	public void UpdateCommanders() {

		foreach (Commander aCommander in commanders) {

			Debug.Log ("Commander " + aCommander.commanderId.ToString() + " has " + aCommander.EntitiesCount() + " units");
		}
	}

	public Commander CommanderForId (int commanderId) {

		Commander commander = null;

		foreach (Commander aCommander in commanders) {

			if (aCommander.commanderId == commanderId) {
				commander = aCommander;
				break;
			}
		}

		return commander;

	}

	protected int currentLocalCommanderId;
	public LocalCommander CurrentLocalCommander () {
		return CommanderForId (currentLocalCommanderId) as LocalCommander;
	}

	public void AddCommanderWithType(CommanderType commanderType) {

		Commander commander = null;

		switch (commanderType) {
		case CommanderType.LocalCommanderType:
			commander = new LocalCommander ();
			break;
		case CommanderType.RemoteCommanderType:
			commander = new LocalCommander (); // TODO: make RemoteCommander
			break;
		case CommanderType.CPUCommanderType:
			commander = new CPUCommander ();
			break;
		default:
			break;
		}

		if (commander != null) {
			commander.commanderId = commanders.Count;
			commander.commanderType = commanderType;
			commanders.Add (commander);

			// TODO: make a proper logic for selecting current local commander, for now it's just the last created local commander
			if (commanderType == CommanderType.LocalCommanderType) {
				currentLocalCommanderId = commander.commanderId;
			}
		}
	}

	/// <summary>
	/// Returns a color for commander with provided identifier. Currently hardcoded, consider chosing colors in the menu
	/// </summary>
	/// <returns>Color for commander with identifier.</returns>
	/// <param name="commanderId">Commander identifier.</param>
	public Color ColorForCommanderWithId(int commanderId) {

		Color[] availableColors = new Color[] { Color.blue, Color.red, Color.yellow, Color.green };

		Color commanderColor = Color.black;
		if (commanderId >= 0 && commanderId < availableColors.Length) {
			commanderColor = availableColors [commanderId];
		}

		return commanderColor;
	}

	#endregion

	#region Creating Entities

	// ####### Entity ID logic #########

	public int NextEntityIdForCommanderId(int commanderId) {

		Commander commander = CommanderForId (commanderId);

		int nextId = -1;

		if (commander != null) {

			int lastId = -1;
			if (lastEntityIdForCommanders.ContainsKey (commander)) {
				lastId = lastEntityIdForCommanders [commander];
				nextId = lastId + commanders.Count;
			} else {
				nextId = commander.commanderId + commanders.Count;
			}

			lastEntityIdForCommanders [commander] = nextId;
		}

		return nextId; 
	}

	public int NextEntityIdForCurrentLocalCommander () {

		return NextEntityIdForCommanderId (currentLocalCommanderId);
	}

	public GameObject CreateEntity(EntityBlueprint blueprint,  int commanderId, Vector3 position) {

		// TODO: maybe RegisterEntity() method that calls some Commander's method that adds this entity to the All list?
		// then the opposite can be done within Destroy()

		GameObject entityObject = Object.Instantiate (Resources.Load(blueprint.prefabName)) as GameObject;
		entityObject.transform.position = position;
		EntityBehaviour entityBehaviour = entityObject.GetComponent<EntityBehaviour> ();

		entityBehaviour.stats.commanderId = commanderId;
		entityBehaviour.stats.id = GameplayManager.SharedInstance ().NextEntityIdForCommanderId (commanderId);

		CommanderForId (commanderId).RegisterEntity (entityBehaviour);

		return entityObject;
	}

	public void DestroyEntity(EntityBehaviour entityBehaviour) {

		CommanderForId (entityBehaviour.stats.commanderId).ForgetEntity (entityBehaviour);

		GameObject entityObject = entityBehaviour.gameObject;
		Object.Destroy (entityObject);
	}

	#endregion
}
