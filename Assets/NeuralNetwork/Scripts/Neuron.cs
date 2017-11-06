using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron {
    
    public double bias;
    public List<double> weigths = new List<double>();
    private List<double> inputvalues = new List<double>();
    private int numberOfInputs = 0;
    public double error = 0f;
    public double output = 0f;
    private bool isFirstLayer = false;

    //Constructor for Neuron
    //number of inputs defines how many inputs the neuron has for first layer there is only 1 input
    //while creating neurons in the NeuralNetwork, the isfirstlayer is set to true
	public Neuron(int numberofinputs, bool isfirstlayer = false)
    {

        isFirstLayer = isfirstlayer;
        //randomize bias for the neuron
        if (isFirstLayer == false) bias = gaussian.NextGaussian();
        else bias = 0;
        numberOfInputs = numberofinputs;

        //Randomize wieghts
        for (int i = 0; i < numberofinputs; i++)
        {
            weigths.Add(gaussian.NextGaussian());
        }
    }


    //Activation function for list input
    public double activate(List<double> inputs)
    {
        inputvalues.Clear();
        inputvalues = inputs;
        //calculate sum of inputs multiplied by weights
        double inputSum = 0;
        for (int i = 0; i < numberOfInputs; i++)
        {
            inputSum += weigths[i] * inputs[i];
        }

        //return sigmoid function value
        output = sigmoid.output(inputSum + bias);
        return output;
    }

    //Activation function for single double input
    public double activate(double input)
    {
        output = sigmoid.output(input * weigths[0] + bias);
        return output;
    }

    public void adjustWeights()
    {
        
        for (int i = 0; i < weigths.Count - 1; i++)
        {
            weigths[i] += error * inputvalues[i];
        }
        bias += error;
        return;
    }

    //used to randomizes all neuron values
    public void randomize()
    {
        //If there are weights, randomize them
        if (weigths.Count > 0)
        {
            for (int i = 0; i < weigths.Count; i++)
            {
                weigths[i] = gaussian.NextGaussian();
            }
        }

        //randomize bias
        if (isFirstLayer == false) bias = gaussian.NextGaussian();
        else bias = 0;

    }

    public void mutate(double amounttomutate, double variance)
    {
        for (int i = 0; i < weigths.Count; i++)
        {
            if (UnityEngine.Random.Range(0f, 1f) < amounttomutate)
            {

                weigths[i] += UnityEngine.Random.Range((float)-variance, (float)variance);
            }
        }

    }
}
