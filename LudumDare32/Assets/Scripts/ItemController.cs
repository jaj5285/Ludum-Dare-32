using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {
		
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag.Contains("Player")) {
			Destroy (this.gameObject);
		}
	}
}
