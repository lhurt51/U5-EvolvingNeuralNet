using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

	public GameObject followerPrefab;
	public GameObject player;
	public float traingingTime = 15.0f;

	private bool leftMouseDown = false;
	private bool isTraining = false;
	private int popSize = 50;
	private int genNum = 0;
	private int[] layers = new int[] { 1, 10, 10 ,1 }; // 1 input and 1 output
	private List<NeuralNetwork> nets;
	private List<FollowBoomerang> followerList = null;

	void InitFollowerNueralNets() {
		// Population size must be even!
		if (popSize % 2 != 0) popSize = 20;

		nets = new List<NeuralNetwork>();

		for (int i = 0; i < popSize; i++) {
			NeuralNetwork net = new NeuralNetwork(layers);
			net.Mutate();
			nets.Add(net);
		}
	}

	void CreateFollowerBodies() {
		if (followerList != null) {
			for (int i = 0; i < followerList.Count; i++) {
				GameObject.Destroy(followerList [i].gameObject);
			}
		}

		followerList = new List<FollowBoomerang>();

		for (int i = 0; i < popSize; i++) {
			FollowBoomerang follower = ((GameObject)Instantiate (followerPrefab, new Vector3 (UnityEngine.Random.Range (-10.0f, 10.0f), UnityEngine.Random.Range (-10.0f, 10.0f), 0.0f), followerPrefab.transform.rotation)).GetComponent<FollowBoomerang>();
			follower.Init(nets [i], player.transform);
			followerList.Add (follower);
		}
	}

	void Timer() {
		isTraining = false;
	}

	// Update is called once per frame
	void Update () {
		if (isTraining == false) {
			if (genNum == 0) {
				InitFollowerNueralNets ();
			} else {
				int halfPopSize = popSize / 2;

				nets.Sort ();
				for (int i = 0; i < halfPopSize; i++) {
					nets [i] = new NeuralNetwork(nets[i + halfPopSize]);
					nets [i].Mutate ();
					nets [i + halfPopSize] = new NeuralNetwork (nets [i + halfPopSize]);
				}

				for (int i = 0; i < popSize; i++) {
					nets [i].SetFitness (0.0f);
				}
			}

			genNum++;

			isTraining = true;
			Invoke("Timer", traingingTime);
			CreateFollowerBodies();
		}

		if (Input.GetMouseButtonDown (0))
			leftMouseDown = true;
		else if (Input.GetMouseButtonUp (0))
			leftMouseDown = false;

		if (leftMouseDown == true) {
			Vector2 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			player.transform.position = mousePos;
		}
	}
}
