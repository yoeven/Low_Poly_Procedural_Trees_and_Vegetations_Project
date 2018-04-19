using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gen.Rock;

public class Example4Script : MonoBehaviour {
	public bool RandomSeed= true;
	public RockData[] Rocks;

	void Start() {
		for(int i = 0;i<Rocks.Length;i++)
		{
			RockData currentRock = Rocks[i];
			if(RandomSeed)
			{
				currentRock = Instantiate(Rocks[i]);
				currentRock.RandomiseSeed();
			}
			RockGeneratorManager.instance.RequestRock(currentRock,new Vector3(i*60,0,0),callback);
		}
	}

	void callback(GameObject rock)
	{
		rock.transform.localScale*=10;
	}

}
