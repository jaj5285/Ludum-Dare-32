using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	void Update () {
		if(Input.GetKeyUp (KeyCode.Space) ) {
			Application.LoadLevel("main");
		}
		if(Input.GetKeyUp (KeyCode.A) ) {
			Application.LoadLevel("credits");
		}
	}
}
