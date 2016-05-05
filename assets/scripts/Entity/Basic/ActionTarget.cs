using UnityEngine;
using System;
//using System.Collections;

public class ActionTarget {

	WeakReference entityReference;
	Vector3 position;

	public ActionTarget() {

		entityReference = new WeakReference (null);
		position = Vector3.zero;
	}
		
	public EntityBehaviour Entity {
		
		get {
			EntityBehaviour entity = entityReference.Target as EntityBehaviour;
			return entity;
		}
		set {
			position = Vector3.zero; // doesnt actualy nulls the position, but at least resets it
			entityReference.Target = value;
		}
	}

	// TODO: consider denying getting position for not visible unit. may require storing reference to the action's owner or it's commander
	public Vector3 Position {
		
		get {
			EntityBehaviour entity = entityReference.Target as EntityBehaviour;
			if (entity != null) {
				position = entity.gameObject.transform.position;
			}
			return position;
		}
		set {
			entityReference.Target = null;
			position = value;
		}
	}
}
