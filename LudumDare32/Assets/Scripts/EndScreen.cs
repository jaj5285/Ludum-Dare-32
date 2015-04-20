using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScreen : MonoBehaviour {

	public Text winnerText;
	public Text winConditionText;

	void Update () {
		if(Input.GetKeyUp (KeyCode.LeftShift) || Input.GetKeyUp (KeyCode.RightShift) ) {
			Application.LoadLevel("main");
		}
		if(Input.GetKeyUp (KeyCode.X) ) {
			Application.LoadLevel("credits");
		}
	}

	void OnGUI(){
		winnerText.text = WorldController.winner +" Wins!";
		winConditionText.text = WorldController.winCondition;
	}
}
