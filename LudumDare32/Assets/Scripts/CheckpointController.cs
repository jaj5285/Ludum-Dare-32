using UnityEngine;
using System.Collections;

public class CheckpointController : MonoBehaviour {

	public Sprite defaultSprite;
	public Sprite player1Sprite;
	public Sprite player2Sprite;
	private Transform mySpriteContainer;
	public AudioClip activateSound;
	
	private AudioSource audioSource;
	private float volLowRange = .5f;
	private float volHighRange = 1.0f;


	void Awake(){		
		audioSource = GetComponent<AudioSource>();
	}

	void Start () {
		//get child Sprite Object
		foreach(Transform child in transform){
			if(child.gameObject.tag == "Sprite"){
				mySpriteContainer = child;
				break;
			}
		}
	}

	void OnTriggerEnter(Collider col){
		string colTag = col.gameObject.tag;
		if(colTag.Contains("Player") && !col.gameObject.tag.Contains("Projectile")&&  !col.gameObject.tag.Contains("Explosion")){

			//Do switch logic only if player did not already capture this checkpoint
			if(gameObject.tag != "Checkpoint_" + colTag) {
				//set tag to belong to Player (for win condition check)
				gameObject.tag = "Checkpoint_" + colTag;
				
				//change color		
				if(col.gameObject.CompareTag("Player1")){
					mySpriteContainer.GetComponent<SpriteRenderer> ().sprite = player1Sprite;
				} 
				else if(col.gameObject.CompareTag("Player2"))
				{				
					mySpriteContainer.GetComponent<SpriteRenderer> ().sprite = player2Sprite;
				}

				//play sound
				if(audioSource != null){
					float vol = Random.Range (volLowRange, volHighRange);
					audioSource.PlayOneShot(activateSound,vol);
				}

				WorldController.CheckCheckpointWinCondition(colTag);
			}
		}

	}
}
