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
        data.Setup();
        data.RandomiseParameters();
        TreeGenManager.instance.GetTree(transform.position, data, s, GetTreecallBack);

	}

    public void GetTreecallBack(GameObject tree)
    {
        this.tree = tree;
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            if (tree != null) Destroy(tree);
            data.RandomiseParameters();
            TreeGenManager.instance.GetTree(transform.position, data, s, GetTreecallBack);
        }
    }
}
