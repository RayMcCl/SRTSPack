using UnityEngine;
using System.Collections;

public class SeedObjects : MonoBehaviour {

	public Seed[] obj;
	UGrid grid;
	public int gridI;
	public bool generate = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void OnDrawGizmos () {
		if(!grid)
			grid = GameObject.Find("UGrid").GetComponent<UGrid>();
			
		if(generate){
			for(int x = 0; x < obj.Length; x++){
				GameObject folder = new GameObject();
				folder.name = "Folder";
				for(int z = 0; z < obj[x].amount; z++){
					int loc = 0;
					while(grid.grids[gridI].grid[loc].state == false){
						loc = Random.Range(0, grid.grids[gridI].grid.Length);
					}
					GameObject clone = Instantiate(obj[x].obj, grid.grids[gridI].grid[loc].loc, Quaternion.identity) as GameObject;
					clone.transform.parent = folder.transform;
					clone.name = obj[x].obj.name;
					grid.grids[gridI].grid[loc].state = false;
				}
			}
			generate = false;
		}
	}
}