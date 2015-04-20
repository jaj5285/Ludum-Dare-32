using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

	public float speed = 25.0f;
	public string myCreator;	
	public Vector3 myDirection;
	public GameObject explosionImage;
	public bool isScreenShaker = false;
	public Transform myTransform;
	public bool isDamagingExplosion = false;
	
	void Start () {
		myTransform = transform;
	}

	void Update () {		
		//GameObject = projectile (lazer), make projectile go up		
		myTransform.Translate (myDirection * speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider col){
		if(!col.gameObject.CompareTag(myCreator) && !col.gameObject.tag.Contains("Checkpoint") && !col.gameObject.tag.Contains("Explosion")){

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
		ExplosionController ec = explosionImage.GetComponent<ExplosionController>();
		ec.init(myCreator, isDamagingExplosion);	
		Instantiate(explosionImage, myTransform.position, Quaternion.identity);
		
		//shake camera
		if(isScreenShaker){
			CameraController cc = Camera.main.GetComponent<CameraController> ();
			cc.shakeCamera ();
		}
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
