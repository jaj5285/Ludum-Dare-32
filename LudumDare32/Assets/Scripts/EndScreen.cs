using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreen : MonoBehaviour {

	public Text winnerText;
	public Text winConditionText;

	void Update () {
		if(Input.GetKeyUp (KeyCode.Space) ) {
			Application.LoadLevel("main");
		}
		if(Input.GetKeyUp (KeyCode.A) ) {
			Application.LoadLevel("credits");
		}
	}

	void OnGUI(){
		winnerText.text = WorldController.winner +" Wins!";
		winConditionText.text = "Won by " + WorldController.winCondition;
	}
}
