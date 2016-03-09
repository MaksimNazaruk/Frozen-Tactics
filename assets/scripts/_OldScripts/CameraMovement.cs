using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	Rect screenRect;
	public int margin = 50;
	public float scrollSpeed = 5f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		KeyboardCameraMovement ();
	}

	void KeyboardCameraMovement () {

		float verticalInput = Input.GetAxis ("Vertical");
		float horizontalInput = Input.GetAxis ("Horizontal");

		Vector3 translationVector = new Vector3 ();
		translationVector.x -= verticalInput * scrollSpeed * Time.deltaTime;
		translationVector.z += verticalInput * scrollSpeed * Time.deltaTime;
		translationVector.x += horizontalInput * scrollSpeed * Time.deltaTime;
		translationVector.z += horizontalInput * scrollSpeed * Time.deltaTime;

		gameObject.transform.Translate (translationVector, Space.World);
	}

	void MouseCameraMovement () {

		if (Camera.current != null && screenRect.width == 0) {
			screenRect = Camera.current.pixelRect;
		}

		Vector3 translationVector = new Vector3 ();

		if (Input.mousePosition.x < margin) {
			translationVector.x -= scrollSpeed * Time.deltaTime;
			translationVector.z -= scrollSpeed * Time.deltaTime;
		}

		if (Input.mousePosition.x > screenRect.width - margin) {
			translationVector.x += scrollSpeed * Time.deltaTime;
			translationVector.z += scrollSpeed * Time.deltaTime;
		}

		if (Input.mousePosition.y > screenRect.height - margin) {
			translationVector.z += scrollSpeed * Time.deltaTime;
			translationVector.x -= scrollSpeed * Time.deltaTime;
		}

		if (Input.mousePosition.y < margin) {
			translationVector.z -= scrollSpeed * Time.deltaTime;
			translationVector.x += scrollSpeed * Time.deltaTime;
		}

		transform.Translate (translationVector, Space.World);
	}
}