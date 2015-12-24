using UnityEngine;
using System.Collections;

public class CursorObject : MonoBehaviour {
	
	CursorManager mang;
	public string cursorTag;
	
	// Assign the Cursor manager on start
	void Start () {
		mang = GameObject.Find("Cursor Manager").GetComponent<CursorManager>();
	}

	// Upon the cursor moving over this object, call the CursorSet function on the Cursor Manager object with this tag
	void OnMouseOver () {
		if(this.enabled)
			mang.SendMessage("CursorSet", cursorTag);
	}
}
