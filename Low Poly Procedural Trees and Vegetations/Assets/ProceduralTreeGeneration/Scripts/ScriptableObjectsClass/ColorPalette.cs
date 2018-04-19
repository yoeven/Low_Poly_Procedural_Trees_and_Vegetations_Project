using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "Colorpalette", menuName = "Color Palette", order = 1)]
public class ColorPalette : ScriptableObject {
	public string ColorPaletteName;
	public Gradient Color;

	public void RandomiseColors () {
		int s = Random.Range (0, int.MaxValue);
		RandomiseColors (s);
	}

	public void RandomiseColors (int seed) 
	{
		Random.InitState (seed);
		int NumOfColors = Random.Range (1, 6);
		GradientColorKey[] colorKeys = new GradientColorKey[NumOfColors];
		float minTime= 0;
		for (int i = 0; i < NumOfColors; i++) 
		{
			int s = seed+i+1;
			Random.InitState (s+1);
			float r = Random.Range (0f, 1f);
			Random.InitState (s+2);
			float g = Random.Range (0f, 1f);
			Random.InitState (s+3);
			float b = Random.Range (0f, 1f);
			colorKeys[i].color = new Color (r, g, b, 1);

			Random.InitState (s+4);
			float tempT = Random.Range (minTime, 1f);
			minTime = tempT;
			colorKeys[i].time = minTime;
		}
		Gradient newGradient = new Gradient();
		newGradient.colorKeys = colorKeys;
		Color = newGradient;
	}
}