using UnityEngine;
using System.Collections;

public class GameplayManager {

	static GameplayManager sharedInstance;
	public static GameplayManager SharedInstance () {

		if (sharedInstance == null) {
			sharedInstance = new GameplayManager ();
		}

		return sharedInstance;
	}

	public bool isRealtime;

	GameplayManager () {

		isRealtime = true;
	}

}
