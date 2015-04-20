using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {

	void Update () {
		if(Input.GetKeyUp (KeyCode.LeftShift) || Input.GetKeyUp (KeyCode.RightShift) ) {
			Application.LoadLevel("main");
		}
		if(Input.GetKeyUp (KeyCode.X) ) {
			Application.LoadLevel("credits");
		}
	}
}
