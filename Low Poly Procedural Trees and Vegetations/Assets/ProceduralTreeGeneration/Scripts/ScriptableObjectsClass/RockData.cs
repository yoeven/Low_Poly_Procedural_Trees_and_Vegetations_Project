using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gen.Rock
{
    [CreateAssetMenu(fileName = "RockData", menuName = "Procedural Generation/Rock Data", order = 2)]
    public class RockData : ScriptableObject
    {
        public enum BasePrimitiveShapes { Sphere, Dodecahedron, Icosahedron, Prism, Pyramid };
        public enum NoiseTypes {Simplex,Perlin,Voronoi,Value,Worley}
        [Header("Rock Details")]
        public string RockName = "";
        public int RockSeed = 0;
        [Range(3,50)] public int Segments = 20;
        public ColorPalette RockGradientColor;
        public BasePrimitiveShapes RockBasePrimitiveShape = BasePrimitiveShapes.Sphere;
        [Range(1,10)]public int octives = 3;
        public NoiseTypes NoiseType = NoiseTypes.Simplex;

        public void RandomiseParameters()
        {
            RandomiseSeed();
            Random.InitState(RockSeed);
            randomiseParameters();
        }

        public void RandomiseParameters(int seed)
        {
            RockSeed = seed;
            Random.InitState(RockSeed);
            randomiseParameters();
        }

        public void RandomiseSeed()
        {
            RockSeed = Random.Range(0, int.MaxValue);
        }

        private void randomiseParameters()
        {
            Segments = Random.Range(3,51);
            RockGradientColor = ScriptableObject.CreateInstance<ColorPalette>();
            RockGradientColor.RandomiseColors(RockSeed);
            RockBasePrimitiveShape =(BasePrimitiveShapes)Random.Range(0,6);
            octives = Random.Range(1,10);
            NoiseType = (NoiseTypes) Random.Range(0,6);
        }

    }
}
