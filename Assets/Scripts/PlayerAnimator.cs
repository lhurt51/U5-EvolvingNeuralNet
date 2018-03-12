using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
	
	private bool increaseSize = true;
	private Material playerMat;

	public float maxScale = 2.0f;
	public float minScale = 1.0f;


	// Use this for initialization
	void Start () {
		playerMat = GetComponent<Renderer>().material;
		playerMat.color = Color.yellow;
	}
	
	// Update is called once per frame
	void Update () {
		float delta = Time.deltaTime;
		Vector3 angles = transform.eulerAngles;
		Vector3 addedScale = new Vector3 (delta, delta, 0.0f);
		Vector3 localScale = transform.localScale;

		angles.z += delta * 50.0f;
		transform.eulerAngles = angles;

		if (increaseSize == true) {
			localScale += addedScale;
			if (localScale.x >= maxScale) increaseSize = false;
		} else {
			localScale -= addedScale;
			if (localScale.x <= minScale) increaseSize = true;
		}

		transform.localScale = localScale;
	}
}
