using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeGen;

public class Example2Script : MonoBehaviour {

    public int NumberOfTrees = 100;
    List<GameObject> trees;

    void Start()
    {
        trees = new List<GameObject>();

        for (int i = 0; i < Mathf.RoundToInt(NumberOfTrees / 2); i++)
        {
            for (int y = 0; y < Mathf.RoundToInt(NumberOfTrees / 2); y++)
            {
                TreeData data = ScriptableObject.CreateInstance<TreeData>();
                data.RandomiseParameters(1);
                TreeGeneratorManager.instance.RequestTree(data, new Vector3(i * 10, 0, y * 10), callback);
            }
        }
    }

    public void callback(GameObject g)
    {
        trees.Add(g);
    }
}
