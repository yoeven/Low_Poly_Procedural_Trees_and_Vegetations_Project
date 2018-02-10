using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

namespace TreeGen
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
            treeObject.GetComponent<Renderer>().material = new Material(vertexShader);

            MeshObjectData[] PlantBuildData = ReturnData.FoliagesBuildData;

            for (int i = 0; i < PlantBuildData.Length; i++)
            {

                Mesh plantMesh = PlantBuildData[i].ToMesh();
                plantMesh.RecalculateBounds();
                plantMesh.RecalculateNormals();

                GameObject Foliage = new GameObject("Foliage");
                Foliage.AddComponent<MeshFilter>().mesh = plantMesh;
                Foliage.AddComponent<MeshRenderer>();
                Foliage.GetComponent<Renderer>().material = new Material(vertexShader);
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

public class MeshObjectData
{
    public Vector3 position;
    public Vector3[] vertices;
    public Vector3[] normals;
    public Vector4[] tangents;
    public int[] triangles;
    public Vector2[] uv;
    public Vector2[] uv2;
    public Vector2[] uv3;
    public Vector2[] uv4;
    public Color[] colors;


    public Mesh ToMesh()
    {
        Mesh m = new Mesh();
        m.MarkDynamic();
        m.vertices = vertices;
        m.normals = normals;
        m.tangents = tangents;
        m.triangles = triangles;
        m.uv = uv;
        m.uv2 = uv2;
        m.uv3 = uv3;
        m.uv4 = uv4;
        m.colors = colors;
        return m;
    }
}