using UnityEngine;
using System.Collections;

public class GridInstanced : MonoBehaviour {

    public GameObject grid;

    public int maxX = 3, maxY = 3, maxZ = 3;


	void Start () {

     

        for (int x = 0; x < maxX; x++)
        {
            for (int y = 0; y < maxY; y++)
            {
                for (int z = 0; z < maxZ; z++)
                {
                    Instantiate(grid, new Vector3(x, y, z), Quaternion.identity);
                }
            }
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
