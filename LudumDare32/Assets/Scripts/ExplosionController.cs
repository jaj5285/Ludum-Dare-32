using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {
		
	private Transform myTransform;
	public float timer;
	public float xGrowth = 0.1f;
	public float yGrowth = 0.1f;


	void Start () {
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
		myTransform.localScale += new Vector3 (xGrowth, yGrowth, 0);
	}
}
