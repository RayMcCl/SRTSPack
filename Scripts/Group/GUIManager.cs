using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {
	
	public GUIElement[] elements;
	
	public Vector2 standardSize = new Vector2(1720, 1080);
	public Rect unitGraphic = new Rect (0, 0, 0, 0);
	public Vector2 UColumnsXRows = new Vector2(10, 5);
	public Vector2 unitBDisp = new Vector2(5, 10);

	public bool universalBuild = false;
	public Rect buildButtonSize = new Rect(50, 900, 75, 75);
	public Vector2 BColumnsXRows = new Vector2(10, 5);
	public Vector2 buildingBDisp = new Vector2(80, 100);
	
	public Rect resourceSize = new Rect(0, 0, 100, 50);
	public Vector2 RColumnsXRows = new Vector2(10, 5);
	public Vector2 resourceBDisp = new Vector2(80, 100);
	
	public Rect miniMapSize = new Rect(1520, 880, 200, 200);
	
	BuildingPlacement place;
	UnitSelection select;
	[HideInInspector]
	public Group group;
	ResourceManager resourceManager;
	Vector2 ratio;
	Vector2 lastWindowSize = Vector2.zero;
	
	void OnDrawGizmos () {
		if(gameObject.name != "Player Manager"){
			gameObject.name = "Player Manager";
		}
	}
	
	void Start () {
		if(place == null){
			place = gameObject.GetComponent<BuildingPlacement>();
		}
		if(select == null){
			select = gameObject.GetComponent<UnitSelection>();
		}
		if(resourceManager == null){
			resourceManager = gameObject.GetComponent<ResourceManager>();
		}
		ReconfigureWindows();
	}
	
	void ReconfigureWindows (){
		ratio = new Vector2 (Screen.width / standardSize.x,  Screen.height / standardSize.y);
		GameObject.Find("MiniMap").GetComponent<MiniMap>().localBounds = new Rect(ratio.x*miniMapSize.x, ratio.y*miniMapSize.y, ratio.x*miniMapSize.width, ratio.y*miniMapSize.height);
		GameObject.Find("MiniMap").GetComponent<MiniMap>().SetSize();
		select.ResizeSelectionWindow(ratio);
		lastWindowSize = new Vector2(Screen.width, Screen.height);
		Debug.Log("Reconfiguring Windows");
	}
	
	void Update () {
		Vector2 windowSize = new Vector2(Screen.width, Screen.height);
		if(lastWindowSize != windowSize){
			ReconfigureWindows();
		}
	}
	
	void OnGUI () {
		useGUILayout = false;
		GUI.depth = 10;
		for(int x = 0; x < elements.Length; x++){
			
			elements[x].Display(ratio);
		}
		// Create Building
		if(universalBuild){
			DisplayBuild();
		}
		else{
			bool canBuild = false;
			bool[] buildingCanBuild = new bool[group.buildingList.Length];
			for(int x = 0; x < select.curSelectedLength; x++){
				UnitController cont = select.curSelectedS[x];
				if(cont.build.builderUnit){
					for(int a = 0; a < cont.build.build.Length; a++){
						if(!buildingCanBuild[a]){
							if(cont.build.build[a].canBuild){
								buildingCanBuild[a] = true;
							}
						}
					}
					canBuild = true;
				}
			}
			if(canBuild){
				DisplayBuild(buildingCanBuild);
			}
		}	
		
		// Resource 
		
		int y = 0;
		int z = 0;
		for(int x = 0; x < resourceManager.resourceTypes.Length; x++){
			// Displays the Resource and the Amount
			GUI.Box(new Rect((resourceSize.x+y*resourceBDisp.x)*ratio.x, (resourceSize.y+z*resourceBDisp.y)*ratio.y, 
			(resourceSize.width)*ratio.x, (resourceSize.height)*ratio.y), 
			resourceManager.resourceTypes[x].name + " : " + resourceManager.resourceTypes[x].amount);
			y = y + 1;
			if(y >= RColumnsXRows.x){
				y = 0;
				z++;
				if(z >= RColumnsXRows.y){
					break;
				}
			}
		}
		
		y = 0;
		z = 0;
		
		// UnitController
		for(int x = 0; x < select.curSelectedLength; x++){
			if(GUI.Button(new Rect((unitGraphic.x+(y*unitBDisp.x))*ratio.x, (unitGraphic.y+(z*unitBDisp.y))*ratio.y, unitGraphic.width*ratio.x, unitGraphic.height*ratio.y), "")){
				select.Select(x, "Unit");
			}
			if(select.curSelectedS[x].gui.image){
				GUI.DrawTexture(new Rect((unitGraphic.x+(y*unitBDisp.x))*ratio.x, (unitGraphic.y+(z*unitBDisp.y))*ratio.y, unitGraphic.width*ratio.x, unitGraphic.height*ratio.y), select.curSelectedS[x].gui.image);
			}
			y++;
			if(y >= UColumnsXRows.x){
				y = 0;
				z++;
				if(z >= UColumnsXRows.y){
					break;
				}
			}
		}
		for(int x = 0; x < select.curBuildSelectedLength; x++){
			if(GUI.Button(new Rect((unitGraphic.x+(y*unitBDisp.x))*ratio.x, (unitGraphic.y+(z*unitBDisp.y))*ratio.y, unitGraphic.width*ratio.x, unitGraphic.height*ratio.y), "")){
				select.Select(x, "Build");
			}
			GUI.DrawTexture(new Rect((unitGraphic.x+(y*unitBDisp.x))*ratio.x, (unitGraphic.y+(z*unitBDisp.y))*ratio.y, unitGraphic.width*ratio.x, unitGraphic.height*ratio.y), select.curBuildSelectedS[x].gui.image);
			y++;
			if(y >= UColumnsXRows.x){
				y = 0;
				z++;
				if(z >= UColumnsXRows.y){
					break;
				}
			}
		}
	
		// Building
		
		if(select.curBuildSelectedLength > 0){
			select.curBuildSelectedS[0].DisplayGUI(ratio.x, ratio.y);
		}
	}
	
	public void DisplayBuild (){
		Vector2 ratio = new Vector2 (Screen.width / standardSize.x,  Screen.height / standardSize.y);
		int y = 0;
		int z = 0;
		for(int x = 0; x < group.buildingList.Length; x++){
			if(group.buildingList[x].obj){
				// Displays the Building Name
				if(GUI.Button(new Rect((buildButtonSize.x+y*buildingBDisp.x)*ratio.x, (buildButtonSize.y+z*buildingBDisp.y)*ratio.y, 
								   buildButtonSize.width*ratio.x, buildButtonSize.height*ratio.y), group.buildingList[x].obj.GetComponent<BuildingController>().name)){
					place.BeginPlace(group.buildingList[x]);
				}
				y = y + 1;
				if(y >= BColumnsXRows.x){
					y = 0;
					z++;
					if(z >= BColumnsXRows.y){
						break;
					}
				}
			}
		}
	}
	
	public void DisplayBuild (bool[] canBuild){
		Vector2 ratio = new Vector2 (Screen.width / standardSize.x,  Screen.height / standardSize.y);
		int y = 0;
		int z = 0;
		for(int x = 0; x < group.buildingList.Length; x++){
			if(group.buildingList[x].obj && canBuild[x]){
				// Displays the Building Name
				if(GUI.Button(new Rect((buildButtonSize.x+y*buildingBDisp.x)*ratio.x, (buildButtonSize.y+z*buildingBDisp.y)*ratio.y, 
								   buildButtonSize.width*ratio.x, buildButtonSize.height*ratio.y), group.buildingList[x].obj.GetComponent<BuildingController>().name)){
					place.BeginPlace(group.buildingList[x]);
				}
				y = y + 1;
				if(y >= BColumnsXRows.x){
					y = 0;
					z++;
					if(z >= BColumnsXRows.y){
						break;
					}
				}
			}
		}
	}
}
