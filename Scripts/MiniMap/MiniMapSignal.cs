using UnityEngine;
using System.Collections;

public class MiniMapSignal : MonoBehaviour {

	public string miniMapTag;
	public bool display = false;
	public MeshRenderer rend;
	public int group = 0;
	public bool isStatic = false;

	// Use this for initialization
	void Awake () {
		if(GameObject.Find("MiniMap").GetComponent<MiniMap>().AddElement(gameObject, miniMapTag, this, group)){

		}
		else{
			Debug.Log("MiniMap : Unit Tag : " + miniMapTag + " Not Found");
		}
		rend = gameObject.GetComponent<MeshRenderer>();
	}
	
	//If the renderer is disabled, the minimap icon will not show up
	void FixedUpdate () {
		display = rend.enabled;
	}
}
