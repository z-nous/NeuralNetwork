using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Neuron {
    
    private double bias;
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
        if (isFirstLayer == false) bias = NextGaussian();
        else bias = 0;
        numberOfInputs = numberofinputs;

        //Randomize wieghts
        for (int i = 0; i < numberofinputs; i++)
        {
            weigths.Add(NextGaussian());
        }
    }

    //used to randomizes all neuron values
    public void randomize()
    {
        //If there are weights, randomize them
        if(weigths.Count > 0)
        {
            for (int i = 0; i < weigths.Count; i++)
            {
                weigths[i] = NextGaussian();
            }
        }

        //randomize bias
        if (isFirstLayer == false) bias = NextGaussian();
        else bias = 0;

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
        output = 1.0 / (1.0 + Math.Exp(inputSum - bias));
        return output;
    }

    //Activation function for single double input
    public double activate(double input)
    {
        output = 1 / (1 + Math.Exp(input * weigths[0] - bias)); ;
        return output;
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

    public void adjustWeights()
    {
        //Debug.Log(error);
        for (int i = 0; i < weigths.Count - 1; i++)
        {
            weigths[i] += error * inputvalues[i];
        }
        bias += error;
        return;
    }

    // gaussian distribution with mean 0 and variance 1
    //Source https://www.alanzucconi.com/2015/09/16/how-to-sample-from-a-gaussian-distribution/
    private static double NextGaussian()
    {
        double v1, v2, s;
        do
        {
            v1 = 2.0f * UnityEngine.Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * UnityEngine.Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Math.Sqrt((-2.0f * Math.Log(s)) / s);

        return v1 * s;
    }
}
