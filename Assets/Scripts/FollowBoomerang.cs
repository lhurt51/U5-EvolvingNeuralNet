using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBoomerang : MonoBehaviour {

	private bool initialized = false;
	private Transform player;
	private NeuralNetwork neurNet;
	private Rigidbody2D ridBody;
	private Material[] mats;

	public void Init(NeuralNetwork neurNet, Transform player) {
		this.neurNet = neurNet;
		this.player = player;
		initialized = true;
	}

	// Use this for initialization
	void Start () {
		ridBody = GetComponent<Rigidbody2D>();
		mats = new Material[transform.childCount];
		for (int i = 0; i < mats.Length; i++) {
			mats [i] = transform.GetChild (i).GetComponent<Renderer> ().material;
		}
	}

	void FixedUpdate () {
		if (initialized == true) {
			float[] inputs = new float[1];
			float dist = Vector2.Distance (transform.position, player.position);
			float angle = transform.eulerAngles.z % 360.0f;
			Vector2 dVector = (player.position - transform.position).normalized;
			float rad = Mathf.Atan2 (dVector.y, dVector.x);

			if (dist > 20.0f) dist = 20.0f;
			if (angle < 0.0f) angle += 360.0f;

			for (int i = 0; i < mats.Length; i++) {
				mats[i].color = new Color (dist / 20.0f, (1.0f - (dist / 20.0f)), (1.0f - (dist / 20.0f)));
			}

			rad *= Mathf.Rad2Deg;

			rad %= 360.0f;
			if (rad < 0.0f) rad += 360.0f;

			rad = 90.0f - rad;
			if (rad < 0.0f) rad += 360.0f;

			rad = 360.0f - rad;
			rad -= angle;
			if (rad < 0.0f) rad += 360.0f;

			if (rad >= 180.0f) {
				rad = 360.0f - rad;
				rad *= -1.0f;
			}

			rad *= Mathf.Deg2Rad;
			inputs [0] = rad / (Mathf.PI);

			float[] output = neurNet.FeedForward (inputs);

			ridBody.velocity = 2.5f * transform.up;
			ridBody.angularVelocity = 500.0f * output[0];

			neurNet.AddFitness (1.0f - Mathf.Abs (inputs [0]));
		}
	}
}
