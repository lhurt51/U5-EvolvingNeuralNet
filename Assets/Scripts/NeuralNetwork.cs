using System;
using System.Collections.Generic;

/// <Summary>
/// Nerual Network C# (Unsupervised)
/// </Summary>
public class NeuralNetwork : IComparable<NeuralNetwork> {

	private int[] layers; // All neural network layers
	private float[][] neurons; // Neuron matrix
	private float [][][] weights; // Weight matrix
	private float fitness;

	/// <Summary>
	/// Initializes the neural network with random weights
	/// </Summary>
	/// <param name="layers"> layers to the neural network </param>
	public NeuralNetwork(int[] layers) {
		// Deep copy of layers of this network
		this.layers = new int[layers.Length];

		for (int i = 0; i < layers.Length; i++) {
			this.layers[i] = layers[i];
		}

		// Generate matrices
		InitNeurons();
		InitWeights();
	}

	/// <summary>
	/// Deep copy constructor
	/// </summary>
	/// <param name="copyNetwork">Network to deep copy</param>
	public NeuralNetwork(NeuralNetwork copyNetwork) {
		this.layers = new int[copyNetwork.layers.Length];

		for (int i = 0; i < copyNetwork.layers.Length; i++) {
			this.layers[i] = copyNetwork.layers[i];
		}

		InitNeurons();
		InitWeights();
		CopyWeights(copyNetwork.weights);
	}

	private void CopyWeights(float[][][] copyWeights) {
		for (int i = 0; i < weights.Length; i++) {
			for (int j = 0; j < weights[i].Length; j++) {
				for (int k = 0; k < weights[i][j].Length; k++) {
					weights[i][j][k] = copyWeights[i][j][k];
				}
			}
		}
	}

	/// <summary>
	/// Create the neuron matrix
	/// </summary>
	private void InitNeurons() {
		// Neuron Initialization
		List<float[]> neuronList = new List<float[]>();

		// Run through all layers
		for (int i = 0; i < layers.Length; i++) {
			neuronList.Add(new float[layers [i]]); // Add each layer to the neuron list
		}

		neurons = neuronList.ToArray(); // Convert the list to an array
	}

	/// <summary>
	/// Creates the weights matrix
	/// </summary>
	private void InitWeights() {
		// Weights list which will later be converted into a 3D array
		List<float[][]> weightsList = new List<float[][]>();

		// Itterate over all the neurons that have a weight connection
		for (int i = 1; i < layers.Length; i++) {
			// layer weight list for the current layer (converted to a 2D array)
			List<float[]> layerWeightsList = new List<float[]>();
			int neuronsInPreviousLayer = layers[i - 1];

			// Itterate over all neurons in this current layer
			for (int j = 0; j < neurons[i].Length; j++) {
				float[] neuronsWeights = new float[neuronsInPreviousLayer]; // Neurons weights

				// Itterate over all neurons in the previous layer and set the weights randomly between 0.5 and -0.5
				for (int k = 0; k < neuronsInPreviousLayer; k++) {
					// Give random weights to neuron weights
					neuronsWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
				}

				// Add neurons weights of this current layer to layer weights
				layerWeightsList.Add(neuronsWeights);
			}
			// Add this layers weights and convert the layer weights list to a 2D array
			weightsList.Add(layerWeightsList.ToArray());
		}
		// Convert to a 3D array
		weights = weightsList.ToArray();
	}

	/// <summary>
	/// Feed forward this neural network with a given input array
	/// </summary>
	/// <param name="inputs">Inputs to the network</param>
	/// <returns>The output layer</returns>
	public float[] FeedForward(float[] inputs) {
		// Adds inputs to the neuron matrix
		for (int i = 0; i < inputs.Length; i++) {
			neurons[0][i] = inputs[i];
		}

		// Itterate over all neurons and compute feedforward values
		for (int i = 1; i < layers.Length; i++) {
			for (int j = 0; j < neurons[i].Length; j++) {
				float value = 0.0f;

				for (int k = 0; k < neurons[i - 1].Length; k++) {
					// Sum of all weights connections of this neuron weight their values in the previous layer
					value += weights[i - 1][j][k] * neurons[i - 1][k];
				}

				// Hyperbolic tangent activation
				neurons[i][j] = (float)Math.Tanh (value);
			}
		}

		return neurons[neurons.Length - 1]; // Return output layer
	}

	/// <summary>
	/// Mutate the neural network weights
	/// </summary>
	public void Mutate() {
		for (int i = 0; i < weights.Length; i++) {
			for (int j = 0; j < weights[i].Length; j++) {
				for (int k = 0; k < weights[i][j].Length; k++) {
					float weight = weights[i][j][k];
					// Mutate weight value
					float randomNum = UnityEngine.Random.Range(0.0f, 100.0f);

					if (randomNum <= 2.0f)
						weight *= -1.0f; // Flip sign of weight
					else if (randomNum <= 4.0f)
						weight = UnityEngine.Random.Range(-0.5f, 0.5f); // Pick a different random weight
					else if (randomNum <= 6.0f)
						weight *= UnityEngine.Random.Range(0.0f, 1.0f) + 1.0f; // Randomly increase by 0% to 100%
					else if (randomNum <= 8.0f)
						weight *= UnityEngine.Random.Range(0.0f, 1.0f);
					weights[i][j][k] = weight;
				}
			}
		}
	}

	public void AddFitness(float fit) {
		fitness += fit;
	}

	public void SetFitness(float fit) {
		fitness = fit;
	}

	public float GetFitness() {
		return fitness;
	}

	/// <summary>
	/// Compares two neural networks base on their level of fitness
	/// </summary>
	/// <param name="other">Network to be compared to</param>
	/// <returns>1 if greater, -1 if less than, and 0 if equal</returns>
	public int CompareTo(NeuralNetwork other) {
		if (other == null) return 1;

		if (fitness > other.fitness)
			return 1;
		else if (fitness < other.fitness)
			return -1;
		else
			return 0;
	}
}
