using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

namespace Gen.Rock
{
    public class RockGeneratorManager : MonoBehaviour
    {
        public static RockGeneratorManager instance;
        public int MaxThreads = 2;
        public Shader vertexShader;
        private List<Thread> ThreadsAlive;
        private Queue<RockThreadReturnData> RocksToBeBuilt;
        private Queue<Action> ThreadMethodsToStart;

        private void Awake()
        {
            instance = this;
            ThreadsAlive = new List<Thread>();
            ThreadMethodsToStart = new Queue<Action>();
            RocksToBeBuilt = new Queue<RockThreadReturnData>();
        }

        private void Update()
        {
            while (RocksToBeBuilt.Count > 0)
            {
                BuildRock(RocksToBeBuilt.Dequeue());
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

        private void BuildRock(RockThreadReturnData ReturnData)
        {
          //build rock stuff
          	GameObject rockObject = new GameObject ("Rock");
            Mesh m = ReturnData.RockBuildData.ToMesh();
            m.RecalculateBounds();
            m.RecalculateNormals();
		    rockObject.AddComponent<MeshFilter> ().mesh = m;
		    rockObject.AddComponent<MeshRenderer> ();
            Material mat = new Material(vertexShader);
            mat.SetFloat("_Smoothness",0f);
		    rockObject.GetComponent<Renderer> ().material = mat;
            rockObject.transform.position = ReturnData.RockPos;
            ReturnData.OriginalCallBack(rockObject);
        }

        public void RequestRock(RockData rockData,Vector3 Pos ,Action<GameObject> callback)
        {
            if(ThreadsAlive.Count>=MaxThreads)
            {
                ThreadMethodsToStart.Enqueue(() => RequestRock(rockData, Pos, callback));
                return;
            }

            RockThreadReturnData ReturnData = new RockThreadReturnData();
            //rockData.Setup();
            ReturnData.RockPos = Pos;
            ReturnData.RockData = rockData;
            ReturnData.ManagerCallBack = ManagerCallBack;
            ReturnData.OriginalCallBack = callback;


            Thread t = new Thread(delegate ()
            {
                RockGenerator.Build(ReturnData);
            }); 
            t.IsBackground = true;
            t.Start();
            ThreadsAlive.Add(t);
        }

        public void ManagerCallBack(RockThreadReturnData returndata)
        {
            lock (RocksToBeBuilt)
            {      
                RocksToBeBuilt.Enqueue(returndata);
            }
        }
    }

    public class RockThreadReturnData
    {
        public Vector3 RockPos;
        public RockData RockData;
        public MeshObjectData RockBuildData;
        public Action<RockThreadReturnData> ManagerCallBack;
        public Action<GameObject> OriginalCallBack;
    }
}