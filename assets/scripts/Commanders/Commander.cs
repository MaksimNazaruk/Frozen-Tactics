using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum CommanderType { LocalCommanderType, RemoteCommanderType, CPUCommanderType };

public class Commander {

	public int commanderId;
	public CommanderType commanderType;

	protected List<WeakReference> entities;

	public Commander() {

		entities = new List<WeakReference> ();
	}

	#region Entities management

	protected bool IsEntityRegistered(EntityBehaviour entity) {

		return entities.Exists (x => x.Target as EntityBehaviour == entity);
	}

	public void RegisterEntity(EntityBehaviour entity) {

		if (!IsEntityRegistered(entity)) {

			entities.Add (new WeakReference (entity));
		}
	}

	public void ForgetEntity(EntityBehaviour entity) {

		if (IsEntityRegistered(entity)) {

			entities.RemoveAll (x => x.Target as EntityBehaviour == entity);
		}
	}

	public int EntitiesCount() {

		return entities.Count;
	}

	// TODO: consider count only on register/forget to avoid processing all the entities every time
	public int BuildingsCount () {

		int count = 0;

		foreach (WeakReference anEntityReference in entities) {

			EntityBehaviour entityBehaviour = anEntityReference.Target as EntityBehaviour;
			if (entityBehaviour.stats.basicType == EntityStats.BasicType.BasicTypeBuilding) {

				count++;
			}
		}

		return count;
	}

	// TODO: consider count only on register/forget to avoid processing all the entities every time
	public int UnitsCount () {

		int count = 0;

		foreach (WeakReference anEntityReference in entities) {

			EntityBehaviour entityBehaviour = anEntityReference.Target as EntityBehaviour;
			if (entityBehaviour.stats.basicType == EntityStats.BasicType.BasicTypeUnit) {

				count++;
			}
		}

		return count;
	}

	#endregion
}
