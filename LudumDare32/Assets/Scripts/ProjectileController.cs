using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

	public float speed = 25.0f;
	public string myCreator;	
	public Vector3 myDirection;
	public GameObject explosionImage;
	private Transform myTransform;
	
	void Start () {
		myTransform = transform;
	}

	void Update () {		
		//GameObject = projectile (lazer), make projectile go up		
		myTransform.Translate (myDirection * speed * Time.deltaTime);
		
		Vector3 cameraView = Camera.main.WorldToScreenPoint (myTransform.position);
		if (cameraView.x > Screen.width) {
			DestroyObject(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider col){
		if(!col.gameObject.CompareTag(myCreator)){

			if(col.gameObject.tag.Contains("Player")){
				//if hit other player, explode only if other player is not respawing
				if(!col.gameObject.GetComponent<PlayerController>().isRespawing){
					explode();
				}
			} else{
				explode();
			}
		}
	}

	void explode(){		
		//destroy obj
		Destroy(this.gameObject);
		
		//instantate explosion
		Instantiate(explosionImage, myTransform.position, Quaternion.identity);
		
		//shake camera
		CameraController cc = Camera.main.GetComponent<CameraController> ();
		cc.shakeCamera ();
	}

	public void init(bool isRightDirection, string playerTag){
		myCreator = playerTag;

		//set tag
		gameObject.tag = "Projectile_" + playerTag;

		//set projectile's direction
		if(isRightDirection){
			myDirection = Vector3.right;
		}else{
			myDirection = Vector3.left;
		}
	}
}
