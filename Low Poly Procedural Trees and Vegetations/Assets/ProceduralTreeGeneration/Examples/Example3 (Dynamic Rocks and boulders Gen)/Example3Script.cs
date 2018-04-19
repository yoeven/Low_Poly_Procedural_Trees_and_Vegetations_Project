using System.Collections;
using System.Collections.Generic;
using Gen.Rock;
using UnityEngine;

public class Example3Script : MonoBehaviour {
	public int NumberOfRocks = 100;
	List<GameObject> rocks;

	void Start () {
		rocks = new List<GameObject> ();

		for (int i = 0; i < Mathf.RoundToInt (NumberOfRocks / 2); i++) {
			for (int y = 0; y < Mathf.RoundToInt (NumberOfRocks / 2); y++) {
				RockData newdata = ScriptableObject.CreateInstance<RockData>();
				newdata.RandomiseParameters();
				RockGeneratorManager.instance.RequestRock (newdata, new Vector3 (i * 10, 0, y * 10), callback);
			}
		}
	}

	public void callback (GameObject g) {
		rocks.Add(g);
	}
}