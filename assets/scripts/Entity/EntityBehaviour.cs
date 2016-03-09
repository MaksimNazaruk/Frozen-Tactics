using UnityEngine;
using System.Collections;

public class EntityBehaviour : MonoBehaviour {

	public EntityStats stats;

	bool isSelected = false;
	public bool IsSelected {
		get {
			return isSelected;
		}
		set {
			MeshRenderer selectionRenderer = selectionObject.GetComponent<MeshRenderer> ();
			selectionRenderer.enabled = value;
		}
	}
	GameObject selectionObject;

	protected virtual void Start () {
	
		SetupStats ();
		SetupEntityUI ();
	}

	protected virtual void SetupStats () {

		Debug.LogError ("SetupStats() method should be implemented in derived classes");
	}
		
	// setups selection ring, maybe healthbars, path line, etc
	void SetupEntityUI () {

		selectionObject = Instantiate (Resources.Load("Prefabs\\UnitCursor")) as GameObject;
		selectionObject.transform.SetParent (gameObject.transform, false);
		selectionObject.transform.localPosition = new Vector3 (0, 0, 0);
		selectionObject.transform.localRotation = Quaternion.Euler (0, 0, 0);

		MeshRenderer selectionRenderer = selectionObject.GetComponent<MeshRenderer> ();
		selectionRenderer.enabled = false;
	}

	protected virtual void Update () {

		Debug.Log ("entity Update");

		if (GameplayManager.SharedInstance ().isRealtime) {
			UpdateRealTime ();
		} else {
			UpdateFrozenTime ();
		}
	}

	protected virtual void UpdateRealTime () {

		Debug.Log("UpdateRealTime Entity");
	}

	protected virtual void UpdateFrozenTime () {

		Debug.Log("UpdateFrozenTime Entity");
	}
}
