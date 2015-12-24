using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	
	public GameObject parentObj;
	UnitController unit;
	public Transform healthObj;
	public Transform backgroundObj;
	float startScale;
	Transform self;
	Transform target;

	void Start () {
		unit = parentObj.GetComponent<UnitController>();
		startScale = healthObj.localScale.x;
		self = transform;
		target = GameObject.Find("Player Manager").transform;
	}
	
	void FixedUpdate () {
		healthObj.localScale = new Vector3(startScale*((float)unit.health/unit.maxHealth) , healthObj.localScale.y, healthObj.localScale.z);
		self.LookAt(target.position);
	}
}
