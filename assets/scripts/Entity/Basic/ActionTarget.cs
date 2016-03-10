using UnityEngine;
using System.Collections;

public class ActionTarget {

	EntityBehaviour entity;
	Vector3 position;

	public EntityBehaviour Entity {
		get {
			return entity;
		}
		set {
			position = Vector3.zero; // doesnt actualy nulls the position, but at least resets it
			entity = value;
		}
	}

	public Vector3 Position {
		get {
			if (entity != null) {
				return entity.gameObject.transform.position;
			} else {
				return position;
			}
		}
		set {
			entity = null;
			position = value;
		}
	}
}
