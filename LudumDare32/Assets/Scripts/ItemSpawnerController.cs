using UnityEngine;
using System.Collections;

public class ItemSpawnerController : MonoBehaviour {
	
	public Transform explosionItemFab;
	public Vector3 randRange;

	void Start () {
		InvokeRepeating ("spawnExplosiveItem", 10, 13.0f);	
	}

	void Update () {
		
		//Random range of the screen
		randRange = new Vector3(Random.Range(-4, 4), Random.Range(-3, 3), 0.0f);
	}

	void spawnExplosiveItem(){	
		float cameraZPosition = -1 * Camera.main.transform.position.z;
		Vector3 screenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0,Screen.width), Random.Range(0,Screen.height), cameraZPosition)); 
		Instantiate (explosionItemFab, screenPosition, Quaternion.identity);
	}
}
