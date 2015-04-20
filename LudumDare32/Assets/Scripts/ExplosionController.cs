using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {
		
	private Transform myTransform;
	public float timer;
	public float xyGrowth = 0.1f;
	public float defaultGrowth = 0.1f;
	public float giantGrowth = 0.4f;
	public string myCreator;
	public AudioClip kaboomSound;


	private AudioSource audioSource;
	private float volLowRange = .5f;
	private float volHighRange = 1.0f;

	void Awake(){		
		audioSource = GetComponent<AudioSource>();
	}

	void Start () {
		//play shooting sound
		if(audioSource != null){
			float vol = Random.Range (volLowRange, volHighRange);
			audioSource.PlayOneShot(kaboomSound,vol);
		}
		myTransform = transform;
		timer = 2f;
	}

	void Update () {	
		timer -= Time.deltaTime;		
		if (timer > 0)
		{	//fade
			StartCoroutine("Fade");
			grow ();
		}else {
			Destroy(this.gameObject);
		}
	}
	
	IEnumerator Fade() {
		Renderer renderer = GetComponent<Renderer> ();
		for (float f = 1f; f >= 0; f -= 0.01f) {
			Color c = renderer.material.color;

			c.a = f;
			renderer.material.color = c;
			yield return new WaitForSeconds(0.01f);
		}
	}

	void grow () {
		myTransform.localScale += new Vector3 (xyGrowth, xyGrowth, 0);
	}

	public void init(string playerTag, bool isDamagingExplosion){
		if (isDamagingExplosion) {
			gameObject.tag = "Explosion_Damaging_" + playerTag;
			xyGrowth = giantGrowth;
			myCreator = playerTag;
		} else {			
			gameObject.tag = "Explosion";
			xyGrowth = defaultGrowth;
			myCreator = playerTag;
		}
	}
}
