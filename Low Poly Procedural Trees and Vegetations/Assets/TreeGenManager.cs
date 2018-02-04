using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TreeGen;
using System;
using System.Threading;

public class TreeGenManager : MonoBehaviour
{
    public static TreeGenManager instance;

    private Action<GameObject> tempcallback;


    void Awake () {
        instance = this;
    }

    public void GetTree(Vector3 Position, TreeData data, Shader s,Action<GameObject> callback)
    {
        tempcallback = callback;
        Thread threadStart = new Thread( delegate()
        {
            TreeGenerator.Build(Position, data, s, FinishedProcessingTree);
        });

        threadStart.Start();
        
    }

    public void FinishedProcessingTree(GameObject tree)
    {
        lock(tree)
        {
            tempcallback(tree);
        }
    }
}
