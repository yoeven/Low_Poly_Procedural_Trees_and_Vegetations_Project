using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

namespace Gen.Tree
{
    public class TreeGeneratorManager : MonoBehaviour
    {
        public static TreeGeneratorManager instance;
        public int MaxThreads = 2;
        public Shader vertexShader;
        private List<Thread> ThreadsAlive;
        private Queue<TreeThreadReturnData> TreesToBeBuilt;
        private Queue<Action> ThreadMethodsToStart;

        private void Awake()
        {
            instance = this;
            ThreadsAlive = new List<Thread>();
            ThreadMethodsToStart = new Queue<Action>();
            TreesToBeBuilt = new Queue<TreeThreadReturnData>();
        }

        private void Update()
        {
            while (TreesToBeBuilt.Count > 0)
            {
                BuildTree(TreesToBeBuilt.Dequeue());
            }

            for(int i =0;i< ThreadsAlive.Count;i++)
            {
                if(!ThreadsAlive[i].IsAlive)
                {
                    ThreadsAlive[i].Abort();
                    ThreadsAlive.Remove(ThreadsAlive[i]);
                }
            }

            if(ThreadsAlive.Count < MaxThreads && ThreadMethodsToStart.Count>0)
            {
                Action a = ThreadMethodsToStart.Dequeue();
                a();
            }
        }

        private void BuildTree(TreeThreadReturnData ReturnData)
        {
            MeshObjectData TreeBuildData = ReturnData.TreeBuildData;
            Mesh TreeMesh = TreeBuildData.ToMesh();
            TreeMesh.RecalculateBounds();
            TreeMesh.RecalculateNormals();

            string ObjectName = ReturnData.TreeData.TreeName != null && ReturnData.TreeData.TreeName != "" ? ReturnData.TreeData.TreeName : "Woah!, Some kind of random Tree";
            GameObject treeObject = new GameObject(ObjectName);
            treeObject.AddComponent<MeshFilter>().mesh = TreeMesh;
            treeObject.AddComponent<MeshRenderer>();
            Material mat = new Material(vertexShader);
            mat.SetFloat("_Smoothness",0f);
            treeObject.GetComponent<Renderer>().material = mat;
            MeshObjectData[] PlantBuildData = ReturnData.FoliagesBuildData;

            for (int i = 0; i < PlantBuildData.Length; i++)
            {

                Mesh plantMesh = PlantBuildData[i].ToMesh();
                plantMesh.RecalculateBounds();
                plantMesh.RecalculateNormals();

                GameObject Foliage = new GameObject("Foliage");
                Foliage.AddComponent<MeshFilter>().mesh = plantMesh;
                Foliage.AddComponent<MeshRenderer>();
                Foliage.GetComponent<Renderer>().material = new Material(mat);
                UnityEngine.Random.InitState(ReturnData.TreeData.TreeSeed + i);
                Foliage.transform.localScale *= UnityEngine.Random.Range(ReturnData.TreeData.foliageScaleMin, ReturnData.TreeData.foliageScaleMax);
                Foliage.transform.position = PlantBuildData[i].position;
                Foliage.transform.SetParent(treeObject.transform);
            }

            treeObject.transform.position = TreeBuildData.position;

            ReturnData.OriginalCallBack(treeObject);
        }

        public void RequestTree(TreeData treeData,Vector3 Pos ,Action<GameObject> callback)
        {
            if(ThreadsAlive.Count>=MaxThreads)
            {
                ThreadMethodsToStart.Enqueue(() => RequestTree(treeData, Pos, callback));
                return;
            }

            TreeThreadReturnData ReturnData = new TreeThreadReturnData();
            treeData.Setup();
            ReturnData.TreePos = Pos;
            ReturnData.TreeData = treeData;
            ReturnData.ManagerCallBack = ManagerCallBack;
            ReturnData.OriginalCallBack = callback;


            Thread t = new Thread(delegate ()
            {
                TreeGenerator.Build(ReturnData);
            }); 
            t.IsBackground = true;
            t.Start();
            ThreadsAlive.Add(t);
        }

        public void ManagerCallBack(TreeThreadReturnData returndata)
        {
            lock (TreesToBeBuilt)
            {      
                TreesToBeBuilt.Enqueue(returndata);
            }
        }
    }

    public class TreeThreadReturnData
    {
        public Vector3 TreePos;
        public TreeData TreeData;
        public MeshObjectData TreeBuildData;
        public MeshObjectData[] FoliagesBuildData;
        public Action<TreeThreadReturnData> ManagerCallBack;
        public Action<GameObject> OriginalCallBack;
    }
}