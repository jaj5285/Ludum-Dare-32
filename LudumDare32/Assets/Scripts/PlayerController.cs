using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public int health = 3;
	public Text healthText;

	public float speed = 5;
	public float jumpHeight = 5;
	public KeyCode leftBtn;
	public KeyCode rightBtn;
	public KeyCode upBtn;
	public KeyCode downBtn;
	public KeyCode actionBtn;
	public AudioClip shootSound;
	public AudioClip deathSound;

	public bool isRespawing = false;
	public GameObject defaultProjectileFab;
	public GameObject explosiveProjectileFab;	
	public GameObject multiProjectileFab;

	public string currentProjectile = "default";

	public Transform respawnPoint;
	
	private bool isFacingRight = true;
	private int directionMultiplier = 1; //1 = right, -1 = left
	private Transform myTransform;
	private Rigidbody rb;
	private Vector3 myDirection;
	private Transform mySprite;
	private string myTag;
	private int currentProjectileCount;
	private Renderer renderer;
	private AudioSource audioSource;
	private float volLowRange = .5f;
	private float volHighRange = 1.0f;

	private float timer;
	private float flashDuration = 3.0f;

	void Awake () {
		myTransform = transform;
		rb = GetComponent<Rigidbody> ();
		myTag = this.gameObject.tag;
		timer = flashDuration;
		audioSource = GetComponent<AudioSource>();

		foreach(Transform child in transform){
			if(child.gameObject.tag == "Sprite"){
				mySprite = child;
				renderer = child.gameObject.GetComponent<Renderer> ();
				break;
			}
		}
	}
	
	void FixedUpdate () {
		movePlayer();
		
		//GetDirection
		if(isFacingRight) {
			directionMultiplier = 1;
			myDirection = myTransform.TransformDirection(Vector3.right);

			//flip sprite to facing direction
			if(mySprite != null){
				mySprite.eulerAngles = new Vector3(mySprite.eulerAngles.x, 0, mySprite.eulerAngles.z);
			}
		}
		else {
			directionMultiplier = -1;
			myDirection = myTransform.TransformDirection(Vector3.left);

			//flip sprite to facing direction
			if(mySprite != null){
				mySprite.eulerAngles = new Vector3(mySprite.eulerAngles.x, 180, mySprite.eulerAngles.z);
			}
		}

		//Debug.DrawLine(myTransform.position, new Vector3(myTransform.position.x + (directionMultiplier * 5), myTransform.position.y, myTransform.position.z), Color.red);
	}

	void Update () {
		//raycast test
		//if (Physics.Raycast (myTransform.position, myDirection, 5)) {
		//	print ("There is something in front of the object!");
		//}
		
		currentProjectileCount = GameObject.FindGameObjectsWithTag ("Projectile_" + myTag).Length;
		shootProjectile();
	}

	void movePlayer(){
		if(Input.GetKey(leftBtn)){
			isFacingRight = false;
			myTransform.position += Vector3.left * speed * Time.deltaTime;
		}
		if(Input.GetKey(rightBtn)){
			isFacingRight = true;
			myTransform.position += Vector3.right * speed * Time.deltaTime;
		}
		if(Input.GetKeyDown(upBtn)){
			rb.AddForce (new Vector3(0.0f, jumpHeight * 10, 0.0f) * speed);
		}
		if(Input.GetKey(downBtn)){
			rb.AddForce (new Vector3(0.0f, -1 * jumpHeight, 0.0f) * speed);
		}
	}

	void shootProjectile(){	

		if(Input.GetKeyDown(actionBtn) && currentProjectileCount < 3){
			//play shooting sound
			float vol = Random.Range (volLowRange, volHighRange);
			audioSource.PlayOneShot(shootSound,vol);

			//find which projectile to shoot
			switch(currentProjectile)
			{
			case "explosive":
				ExplosiveProjectileController epc = explosiveProjectileFab.GetComponent<ExplosiveProjectileController>();
				epc.init(isFacingRight, gameObject.tag);				
				Instantiate(explosiveProjectileFab, myTransform.position, Quaternion.identity);
				break;
			case "multi":
				ProjectileController mpc = multiProjectileFab.GetComponent<ProjectileController>();
				mpc.init(isFacingRight, gameObject.tag);				
				Instantiate(defaultProjectileFab, myTransform.position, Quaternion.identity);
				break;
			default:
				ProjectileController pc = defaultProjectileFab.GetComponent<ProjectileController>();
				pc.init(isFacingRight, gameObject.tag);				
				Instantiate(defaultProjectileFab, myTransform.position, Quaternion.identity);
				break;
			}
		}
	}

	IEnumerator setSpecialProjectileTime(float waitTime) {
		if(currentProjectile != "default"){
			yield return new WaitForSeconds(waitTime);
			Debug.Log ("Time's up!");
			currentProjectile = "default";
		}
	}
	
	IEnumerator ColorFlash(float time, float intervalTime, Color flashColor)
	{
		isRespawing = true;
		Color[] colors = {flashColor, Color.white};
		float elapsedTime = 0f;
		int index = 0;
		while(elapsedTime < time )
		{
			renderer.material.color = colors[index % 2];
			elapsedTime += intervalTime;
			index++;
			yield return new WaitForSeconds(intervalTime);
		}	
		renderer.material.color = Color.white;
		isRespawing = false;
	}
	
	void OnTriggerEnter(Collider col){	
		string colTag = col.gameObject.tag;

		if (colTag != null) {

		//PICKUP ITEM
			if(colTag.Contains("Item_")) {
				switch(colTag)
				{
					case "Item_Explosive":
						currentProjectile = "explosive";
						break;
					case "Item_Multi":
						currentProjectile = "multi";
						break;
					default:
						currentProjectile = "default";
						break;
				}
				float tempProjectileDuration = 5.0f;
				StartCoroutine(setSpecialProjectileTime(tempProjectileDuration));
				StartCoroutine(ColorFlash(tempProjectileDuration, 0.3f, Color.blue));
			}

		//HIT PROJECTILE
			if(colTag.Contains("Projectile_") && !isRespawing && !colTag.Contains(myTag)){
				ProjectileController pc = col.gameObject.GetComponent<ProjectileController>();

				KillSelf(pc.myCreator);
			}

		//HIT EXPLOSION
			if(colTag.Contains("Explosion_Damaging") && !isRespawing && !colTag.Contains(myTag)){
				KillSelf(colTag);
			}
		}
	}

	void KillSelf(string colTag){
		//decrease player health
		health--;

		//play death sound
		float vol = Random.Range (volLowRange, volHighRange);
		audioSource.PlayOneShot(deathSound,vol);
		
		if (health <= 0) {
			healthText.text = myTag + ": " + health;
			Destroy (this.gameObject);
			WorldController.EndGame (colTag, "Win by Kill");
		} else {
			//move to respawn point
			myTransform.position = respawnPoint.position;
		
			//Make player flash and be invincible for a while
			StartCoroutine (ColorFlash (5.0f, 0.3f, Color.red));
		}

	}
	
	//GUI text
	void OnGUI(){
		healthText.text = "x " + health;
	}
}
