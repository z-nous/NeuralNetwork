using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Neuron {
    
    private float bias;
    private List<float> weigths = new List<float>(); 
    private int numberOfInputs = 0;
    private float error = 0f;
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
    public float activate(List<float> inputs)
    {
        //calculate sum of inputs multiplied by weights
        float inputSum = 0;
        for (int i = 0; i < numberOfInputs; i++)
        {
            inputSum += weigths[i] * inputs[i];
        }

        //return sigmoid function value
        return 1 / (1 + Mathf.Exp(inputSum - bias));
    }

    //Activation function for single float input
    public float activate(float input)
    {
        return 1 / (1 + Mathf.Exp(input*weigths[0] - bias));
    }


    public void mutate(float amounttomutate, float variance)
    {
        for (int i = 0; i < weigths.Count; i++)
        {
            if (Random.Range(0f, 1f) < amounttomutate)
            {
                weigths[i] += Random.Range(-variance, variance);
            }
        }

    }

    // gaussian distribution with mean 0 and variance 1
    //Source https://www.alanzucconi.com/2015/09/16/how-to-sample-from-a-gaussian-distribution/
    private static float NextGaussian()
    {
        float v1, v2, s;
        do
        {
            v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);

        s = Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s);

        return v1 * s;
    }
}
