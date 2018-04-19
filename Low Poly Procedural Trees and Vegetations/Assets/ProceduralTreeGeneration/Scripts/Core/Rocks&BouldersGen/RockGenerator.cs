using System.Collections;
using System.Collections.Generic;
using ProceduralNoiseProject;
using ProceduralToolkit;
using UnityEngine;

namespace Gen.Rock {
	public class RockGenerator {

		static float NoiseGen (INoise noise, int oct, float x, float y, float z) {
			float value = 0.0f;
			int i;
			for (i = 0; i < oct; i++) {
				value += noise.Sample3D (
					x * Mathf.Pow (2, i),
					y * Mathf.Pow (2, i),
					z * Mathf.Pow (2, i)
				);
			}
			return value;
		}

		public static void Build (RockThreadReturnData returnData) {
			RockData data = returnData.RockData;
			MeshDraft MD;

			switch (data.RockBasePrimitiveShape) {
				case RockData.BasePrimitiveShapes.Dodecahedron:
					MD = MeshDraft.Dodecahedron (0.5f);
					break;
				case RockData.BasePrimitiveShapes.Icosahedron:
					MD = MeshDraft.Icosahedron (0.5f, false);
					break;
				case RockData.BasePrimitiveShapes.Prism:
					MD = MeshDraft.Prism (0.5f, data.Segments, 1f, false);
					break;
				case RockData.BasePrimitiveShapes.Pyramid:
					MD = MeshDraft.Pyramid (0.5f, data.Segments, 1f, false);
					break;
				default:
					MD = MeshDraft.Sphere (0.5f, data.Segments, data.Segments, false);
					break;
			};
			MeshObjectData rock = new MeshObjectData ();
			rock.vertices = MD.vertices.ToArray ();
			rock.triangles = MD.triangles.ToArray ();
			rock.tangents = MD.tangents.ToArray ();
			rock.AutoWeldMesh (0.0001f, 0.4f);
			Vector3[] verts = rock.vertices;
			INoise noise = new SimplexNoise (data.RockSeed, 0.3f, 0.2f);
			Rand r = new Rand (data.RockSeed);

			for (int i = 0; i < verts.Length; i++) {
				float currentNoise = NoiseGen (noise, 3, verts[i].x / 0.5f, verts[i].y / 0.5f, verts[i].z / 0.5f);
				//currentNoise*=2;
				Vector3 norm = verts[i].normalized;
				verts[i].x += currentNoise * norm.x;
				verts[i].y += currentNoise * norm.y;
				verts[i].z += currentNoise * norm.z;

				verts[i].x *= 3;
				verts[i].y *= 1.2f;
				verts[i].z *= 1.5f;
			}
			rock.vertices = verts;
			rock.flatShade();

			Color[] vertexColor = new Color[rock.vertices.Length];
				
			for (int i = 0; i < rock.vertices.Length; i++) {
				vertexColor[i] = data.RockGradientColor.Color.Evaluate(1-rock.vertices[i].y);
			}

			rock.colors = vertexColor;
			returnData.RockBuildData = rock;
			returnData.ManagerCallBack (returnData);
		}
	}
}