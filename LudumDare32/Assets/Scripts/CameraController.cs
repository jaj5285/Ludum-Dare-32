using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	public bool isShaking = false;
	
	// How long the object should shake for.
	public float shakeDuration = 1f;
	
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.1f;
	public float decreaseFactor = 1.5f;
	
	Vector3 originalPos;
	
	void Awake()
	{
		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}
	
	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}
	
	void Update()
	{
		if(isShaking){
			if (shakeDuration > 0)
			{
				camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
				
				shakeDuration -= Time.deltaTime * decreaseFactor;
			}
			else
			{
				isShaking = false;
				shakeDuration = 1f; //default duration is 1 second
				camTransform.localPosition = originalPos;
			}
		}
	}

	public void shakeCamera (float duration = 1.0f){
		shakeDuration = duration;
		isShaking = true;
	}
}
