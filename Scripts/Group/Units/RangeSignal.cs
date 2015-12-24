using UnityEngine;
using System.Collections;

public class RangeSignal : MonoBehaviour {

	public int type = 0;
	public UnitController cont;

	void OnTriggerEnter (Collider coll) {
		if (coll.gameObject.tag == "Unit") {
			UnitController script = coll.gameObject.GetComponent<UnitController>();
			int state = cont.DetermineRelations(script.group);
			if(state == 2){
				cont.SphereSignal(type, coll.gameObject);
			}
		}
	}
}
