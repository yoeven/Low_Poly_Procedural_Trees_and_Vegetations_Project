using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TreeGen
{
    [CreateAssetMenu(fileName = "TreeData", menuName = "Procedural Generation/Tree Data", order = 1)]
    public class TreeData : ScriptableObject
    {
        public string TreeName = "";
        public int randomSeed = 0;
        [Space]
        [Header("Branch Parameters")]
        [Range(0.25f, 0.95f)] public float lengthAttenuation = 0.8f;
        [Range(0.25f, 0.95f)] public float radiusAttenuation = 0.5f;
        [Range(1, 3)] public int branchesMin = 1;
        [Range(1, 3)] public int branchesMax = 2;
        [Range(-45f, 0f)] public float growthAngleMin = -15f;
        [Range(0f, 45f)] public float growthAngleMax = 15f;
        [Range(1f, 10f)] public float growthAngleScale = 4f;
        [Range(4, 20)] public int heightSegments = 10, radialSegments = 8;
        [Range(0.0f, 0.35f)] public float bendDegree = 0.1f;
        [Space]
        [Header("Root Parameters")]
        [Range(1, 4)] public int generations = 4;
        [Range(0.5f, 5f)] public float length = 1f;
        [Range(0.1f, 2f)] public float radius = 0.15f;
        [Space]
        [Header("Foliage Parameters")]
        [Range(0f, 100f)]public float foliageChance = 100f;
        [Range(5, 30)]public int foliageSegments = 10;
        [Range(0.0f, 0.2f)] public float noise = 0.2f;
        [Range(0.1f, 3f)] public float foliageScaleMin = 1;
        [Range(0.1f, 3f)] public float foliageScaleMax = 3;
        public Color branchColor;
        public Color[] foliageColors;


        [HideInInspector] public int RandomBranches;
        [HideInInspector] public float RandomGrowthAngle;
        [HideInInspector] public float RandomBendDegree;


        public void Setup()
        {
            Random.InitState(randomSeed);
            GetRandomBranches();
            GetRandomGrowthAngle();
            GetRandomBendDegree();
        }

        public void GetRandomBranches()
        {
            RandomBranches =  Random.Range(branchesMin, branchesMax + 1);
        }

        public void GetRandomGrowthAngle()
        {
            RandomGrowthAngle =  Random.Range(growthAngleMin, growthAngleMax);
        }

        public void GetRandomBendDegree()
        {
            RandomBendDegree =  Random.Range(-bendDegree, bendDegree);
        }

        public void RandomiseParameters()
        {
            randomSeed = Random.Range(0, int.MaxValue);
            Random.InitState(randomSeed);
            randomiseParameters();
        }

        public void RandomiseParameters(int seed)
        {
            randomSeed = seed;
            Random.InitState(randomSeed);
            randomiseParameters();
        }

        private void randomiseParameters()
        {
            lengthAttenuation = Random.Range(0.25f,0.95f);
            radiusAttenuation = Random.Range(0.25f, 0.95f);
            branchesMin = Random.Range(1,4);
            branchesMax = Random.Range(1,4);
            growthAngleMin = Random.Range(-45f, 0f);
            growthAngleMax = Random.Range(0f, 45f);
            growthAngleScale = Random.Range(1f, 10f);
            heightSegments = Random.Range(4, 21);
            radialSegments = Random.Range(4, 21);
            bendDegree = Random.Range(0f, 0.35f);
            generations = Random.Range(1, 5);
            length = Random.Range(0.5f, 5f);
            radius = Random.Range(0.1f, 2f);
            foliageChance = Random.Range(0f, 100f);
            foliageSegments = Random.Range(5,31);
            noise = Random.Range(0f, 0.2f);
            foliageScaleMin = Random.Range(0.1f, 3f);
            foliageScaleMax = Random.Range(0.1f, 3f);
            branchColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
            int NumOfRandomColor = Random.Range(1, 6);
            foliageColors = new Color[NumOfRandomColor];
            for(int i =0;i< NumOfRandomColor;i++)
            {
                foliageColors[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
            }
        }

    }
}
