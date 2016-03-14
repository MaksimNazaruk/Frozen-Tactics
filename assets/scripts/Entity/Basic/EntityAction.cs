using UnityEngine;
using System.Collections;

public delegate void EntityActionMethod (ActionTarget target, out bool isFinished);

public class EntityAction {

	public string title;
	public bool isTargetRequired = false;
	public EntityActionMethod actionMethod;
}
