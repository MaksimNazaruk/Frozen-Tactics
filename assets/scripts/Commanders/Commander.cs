using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum CommanderType { LocalCommanderType, RemoteCommanderType, CPUCommanderType };

public class Commander {

	public int commanderId;
	public CommanderType commanderType;

	List<WeakReference> entities;

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

	#endregion
}
