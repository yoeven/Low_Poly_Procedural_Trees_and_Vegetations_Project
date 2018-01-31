using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeGen;

public class Test : MonoBehaviour {

    public TreeData data;
    public Shader s;
    GameObject tree;

 
    void Start () {
        data = new TreeData();
        data.RandomiseParameters();
        tree = TreeGenerator.Build(transform.position,data,s);

	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (tree != null) Destroy(tree);
            data.RandomiseParameters();
            tree = TreeGenerator.Build(transform.position,data, s);
        }
    }
}
