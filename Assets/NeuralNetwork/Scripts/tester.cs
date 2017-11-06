using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tester : MonoBehaviour {

    private NeuralNetwork testNetwork;
    //private List<double> results = new List<double>();

    private List<List<double>> trainingdata = new List<List<double>>();
    private List<List<double>> desiredOutput = new List<List<double>>();

    private List<double> testdata11 = new List<double>();
    private List<double> testdata00 = new List<double>();

    // Use this for initialization
    void Start () {
        testNetwork = new NeuralNetwork(2, 1, 2);

        //Construct trainingdata for AND gate
        constructTrainingData();

        //test network
        print("network structure: ");
        testNetwork.getNetworkStructure(true);

        //print testdata
        for (int i = 0; i < trainingdata.Count; i++)
        {
            print("input:");
            print( trainingdata[i][0]);
            print(trainingdata[i][1]);
            print("output should be");
            print(desiredOutput[i][0]);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("space"))
        {
            //mixTraingindata();
            print("starting training");
            double error = testNetwork.train_backpropagation(trainingdata, desiredOutput, 5000, 10f);
            print("Total error after training: " + error);
            print("training finished");
            print("results for 0 0 : " + testNetwork.calculate(trainingdata[0])[0] + " should be 0");
            print("results for 0 1 : " + testNetwork.calculate(trainingdata[1])[0] + " should be 1");
            print("results for 1 0 : " + testNetwork.calculate(trainingdata[2])[0] + " should be 1");
            print("results for 1 1 : " + testNetwork.calculate(trainingdata[3])[0] + " should be 0");
        }
        
        if (Input.GetKey("r"))
        {
            testNetwork.randomize();
        }

    }

    private void constructTrainingData()
    {
        trainingdata.Add(new List<double>());
        desiredOutput.Add(new List<double>());
        trainingdata[0].Add(0f);
        trainingdata[0].Add(0f);
        desiredOutput[0].Add(0f);
        //desiredOutput[0].Add(0f);

        trainingdata.Add(new List<double>());
        desiredOutput.Add(new List<double>());
        trainingdata[1].Add(1f);
        trainingdata[1].Add(0f);
        desiredOutput[1].Add(1f);
        //desiredOutput[1].Add(0f);

        trainingdata.Add(new List<double>());
        desiredOutput.Add(new List<double>());
        trainingdata[2].Add(0f);
        trainingdata[2].Add(1f);
        desiredOutput[2].Add(1f);
        //desiredOutput[2].Add(1f);

        trainingdata.Add(new List<double>());
        desiredOutput.Add(new List<double>());
        trainingdata[3].Add(1f);
        trainingdata[3].Add(1f);
        desiredOutput[3].Add(0f);
        //desiredOutput[3].Add(1f);
    }

    private void mixTraingindata()
    {
        for (int i = 0; i < trainingdata.Count; i++)
        {
            List<double> temp = trainingdata[i];
            int randomindex = Random.Range(i, trainingdata.Count);
            trainingdata[i] = trainingdata[randomindex];
            trainingdata[randomindex] = temp;
        }
    }
}
