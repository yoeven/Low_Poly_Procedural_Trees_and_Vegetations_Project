using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeGen;

public class Test : MonoBehaviour {

    public TreeData data;
    List<GameObject> trees;

    void Start () {
        trees = new List<GameObject>();

        for(int i = 0; i<10;i++)
        {
            for (int y = 0; y <10; y++)
            {
                TreeData newData = ScriptableObject.CreateInstance<TreeData>();
                newData.RandomiseParameters();
                TreeGeneratorManager.instance.RequestTree(newData, new Vector3(i * 10, 0, y * 10), callback);
            }
        }
	}

    public void callback(GameObject g)
    {
        trees.Add(g);
    }

 
}
