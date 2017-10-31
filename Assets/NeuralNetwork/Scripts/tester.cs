using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : MonoBehaviour {

    NeuralNetwork testNetwork;
    List<float> results = new List<float>();
    private List<float> testvaules = new List<float>();

    private List<List<float>> trainingdata = new List<List<float>>();
    private List<List<float>> desiredOutput = new List<List<float>>();

    // Use this for initialization
    void Start () {
        for (int i = 0; i < 512; i++)
        {
            testvaules.Add(Random.Range(0f, 1f));
        }

        testNetwork = new NeuralNetwork(512, 9, 16, 16);

        results = testNetwork.calculate(testvaules,true);

        print("Network output:");
        foreach  (float value in results)
        {
            print(value);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("space"))
        {

            testNetwork.mutateNetwork(0.2f, 1f, 0.1f);
            results = testNetwork.calculate(testvaules);
            print("results from network");
            foreach (float value in results)
            {
                print(value);
            }

        }
	}

}
