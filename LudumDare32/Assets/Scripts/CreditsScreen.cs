using UnityEngine;
using System.Collections;

public class CreditsScreen : MonoBehaviour {

	void Update () {
		if(Input.GetKeyUp (KeyCode.LeftShift) || Input.GetKeyUp (KeyCode.RightShift) ) {
			Application.LoadLevel("main");
		}
	}
}
