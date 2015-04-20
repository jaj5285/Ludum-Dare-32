using UnityEngine;
using System.Collections;

public class ExplosiveProjectileController : ProjectileController {

	void Start () {		
		myTransform = transform;
		speed = 12.0f;
		isScreenShaker = true;
		isDamagingExplosion = true;
	}

}
