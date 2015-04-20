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
	public AudioClip shootSound;
	
	private Transform mySprite;
	private Renderer renderer;
	private int directionMultiplier = 1; //1 = right, -1 = left
	private Rigidbody rb;
	private AudioSource audioSource;
	private float volLowRange = .5f;
	private float volHighRange = 1.0f;

	void Awake() {		
		audioSource = GetComponent<AudioSource>();
		//play shooting sound
		float vol = Random.Range (volLowRange, volHighRange);
		audioSource.PlayOneShot(shootSound,vol);
	}

	void Start(){		
		myTransform = transform;
	}

	void Update () {		
		//GameObject = projectile (lazer), make projectile go up		
		myTransform.Translate (myDirection * speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag != null) {
			if (!col.gameObject.CompareTag (myCreator) && !col.gameObject.tag.Contains ("Checkpoint") && !col.gameObject.tag.Contains ("Explosion") && !col.gameObject.tag.Contains ("Projectile")) {

				if (col.gameObject.tag.Contains ("Player")) {
					//if hit other player, explode only if other player is not respawing
					if (!col.gameObject.GetComponent<PlayerController> ().isRespawing) {
						explode ();
					}
				} else {
					explode ();
				}
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

	
	void SetSpriteDirection(bool isFacingRight){
		//Get the child sprite
		foreach(Transform child in transform){
			if(child.gameObject.tag == "Sprite"){
				mySprite = child;
				renderer = child.gameObject.GetComponent<Renderer> ();
				break;
			}
		}

		if(isFacingRight) {
			directionMultiplier = 1;
			myDirection = transform.TransformDirection(Vector3.right);
			
			//flip sprite to facing direction
			if(mySprite != null){
				mySprite.eulerAngles = new Vector3(mySprite.eulerAngles.x, 0, mySprite.eulerAngles.z);
			}
		}
		else {
			directionMultiplier = -1;
			myDirection = transform.TransformDirection(Vector3.left);
			
			//flip sprite to facing direction
			if(mySprite != null){
				mySprite.eulerAngles = new Vector3(mySprite.eulerAngles.x, 180, mySprite.eulerAngles.z);
			}
		}
	}

	public void init(bool isRightDirection, string playerTag){
		myCreator = playerTag;

		//set tag
		gameObject.tag = "Projectile_" + playerTag;

		//set projectile's direction
		if(isRightDirection){
			myDirection = Vector3.right;
			SetSpriteDirection(true);
		}else{
			myDirection = Vector3.left;
			SetSpriteDirection(false);
		}
	}
}
