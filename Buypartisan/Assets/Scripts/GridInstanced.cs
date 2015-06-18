using UnityEngine;
using System.Collections;

public class GridInstanced : MonoBehaviour {
	
	public GameObject grid;
	
	//public int maxX = 3, maxY = 3, maxZ = 3;
	
	
	public void GridInstantiate (int max) {
		
		
		for (int x = 0; x < max; x++)
		{
			for (int y = 0; y < max; y++)
			{
				for (int z = 0; z < max; z++)
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
