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

	public bool isRespawing = false;
	public GameObject projectileFab;
	public Transform respawnPoint;
	
	private bool isFacingRight = true;
	private int directionMultiplier = 1; //1 = right, -1 = left
	private Transform myTransform;
	private Rigidbody rb;
	private float xRotation;
	private Vector3 myDirection;
	private Transform mySprite;
	private string myTag;
	private int currentProjectileCount;
	private Renderer renderer;

	private float timer;
	private float flashDuration = 3.0f;

	
	private int Countdown = 3;

	void Start () {
		myTransform = transform;
		rb = GetComponent<Rigidbody> ();
		xRotation = myTransform.eulerAngles.x;
		myTag = this.gameObject.tag;
		timer = flashDuration;

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

			ProjectileController pc = projectileFab.GetComponent<ProjectileController>();
			pc.init(isFacingRight, gameObject.tag);

			Instantiate(projectileFab, myTransform.position, Quaternion.identity);

			//push back player
			//rb.AddForce(new Vector3(-1 * directionMultiplier * 2.0f, 0.0f, 0.0f) * 100);
		}
	}
	
	IEnumerator InvincibleFlash(float time, float intervalTime)
	{
		isRespawing = true;
		Color[] colors = {Color.red, Color.white};
		float elapsedTime = 0f;
		int index = 0;
		while(elapsedTime < time )
		{
			renderer.material.color = colors[index % 2];

			elapsedTime += 0.5f;
			index++;
			yield return new WaitForSeconds(intervalTime);
		}	
		renderer.material.color = Color.white;
		isRespawing = false;
	}
	
	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag != null) {
			if(col.gameObject.tag.Contains("Projectile_") && !isRespawing){
				ProjectileController pc = col.gameObject.GetComponent<ProjectileController> ();

				//only die if colliding with other player's projectiles
				if (pc.myCreator != myTag && col.gameObject.tag != "Projectile_" + myTag) {	
					//decrease player health
					health--;

					//move to respawn point
					myTransform.position = respawnPoint.position;

					//Make player flash and be invincible for a while
					StartCoroutine(InvincibleFlash(5.0f, 0.2f));

					if(health <= 0){
						healthText.text = myTag + ": " + health;
						Destroy(this.gameObject);
					}
				}
			}
		}
	}
	
	//GUI text
	void OnGUI(){
		healthText.text = myTag + ": " + health;
	}
}
