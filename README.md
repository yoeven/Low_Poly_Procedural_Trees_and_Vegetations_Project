


A simple low poly procedural tree builder.

![](https://raw.githubusercontent.com/yoeven/Low_Poly_Procedural_Trees_and_Vegetations_Project/master/Screenshots/Example1.jpg)

![](https://raw.githubusercontent.com/yoeven/Low_Poly_Procedural_Trees_and_Vegetations_Project/master/Screenshots/Example2.jpg)

![10000 Trees](https://raw.githubusercontent.com/yoeven/Low_Poly_Procedural_Trees_and_Vegetations_Project/master/Screenshots/Example2%2810000%29.jpg)

# Features

  - Safe threading support for fast creation.
  - Scriptable Object support for saving Tree Data.
  
 ![enter image description here](https://raw.githubusercontent.com/yoeven/Low_Poly_Procedural_Trees_and_Vegetations_Project/master/Screenshots/TreeData.jpg)
  
  - Infinite possibilities of trees.

# Usage Example

  - Drop "TreeGenerationManager" Prefab into scene.
  - Create a MonoBehaviour script using namespcace "TreeGen".
 
  ```D
    public int NumberOfTrees = 100;
    List<GameObject> trees;

    void Start()
    {
        trees = new List<GameObject>();

        for (int i = 0; i < Mathf.RoundToInt(NumberOfTrees / 2); i++)
        {
            for (int y = 0; y < Mathf.RoundToInt(NumberOfTrees / 2); y++)
            {
                TreeData data = ScriptableObject.CreateInstance<TreeData>();
                data.RandomiseParameters();
                TreeGeneratorManager.instance.RequestTree(data, new Vector3(i * 10, 0, y * 10), callback);
            }
        }
    }

    public void callback(GameObject g)
    {
        trees.Add(g);
    }
 ```



### Todos

 - Add a pooling system.
 - Create vegetation system. (e.g. flowers, plants)

### Resources Credits
- Project is based of https://github.com/mattatz/unity-procedural-tree
- Procedural Sphere creation and shaders https://github.com/Syomus/ProceduralToolkit
- Video that helped with threading: https://www.youtube.com/watch?v=f0m73RsBik4

License
----

MIT



<!--stackedit_data:
eyJoaXN0b3J5IjpbOTk5NDU1Mjc0XX0=
-->