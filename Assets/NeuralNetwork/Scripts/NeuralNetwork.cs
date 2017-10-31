using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NeuralNetwork{

    //List that contains a list of neurons this is the whole neural network
    private List<List<Neuron>> neuronlist = new List<List<Neuron>>();
    //list of lists containing calculated results of neurons
    private List<List<float>> layerResults = new List<List<float>>();

    //Constructor for neural network
    //inputs are pretty self explanatory
    public NeuralNetwork(int numberofinputs,int numberofoutputs, params int[] numberofneuronsinlayer)
    {
        //Add input layer
        neuronlist.Add(new List<Neuron>());
        for (int i = 1; i <= numberofinputs; i++)
        {
            neuronlist[0].Add(new Neuron(1,true));
        }

        //add the hidden layers
        int layerindex = 1;
        int neuronsInPreviousLayer = 0;
        foreach (int numberofneurons in numberofneuronsinlayer)
        {
            neuronlist.Add(new List<Neuron>());
            neuronsInPreviousLayer = neuronlist[layerindex - 1].Count;
            for (int i = 0; i < numberofneurons; i++)
            {
                neuronlist[layerindex].Add(new Neuron(neuronsInPreviousLayer));
            }
            layerindex++;
        }

        //Add output layer
        neuronlist.Add(new List<Neuron>());
        for (int i = 0; i < numberofoutputs; i++)
        {
            neuronlist[neuronlist.Count - 1].Add(new Neuron(neuronlist[neuronlist.Count - 2].Count));
        }
    }


    //processes the neural network and returns a list of floats that contains the answers from the network
    public List<float> calculate(List<float> inputvalues, bool EnableVerbose = false)
    {
        //clear layerresults list before doing things to it
        layerResults.Clear();

        //initialize layerResults List

        for (int j = 0; j < neuronlist.Count; j++)
        {
            layerResults.Add(new List<float>());
        }

        //is there correct amount of input values if not, return null
        if (inputvalues.Count != neuronlist[0].Count)
        {
            
            Debug.Log("wrong number of inputs");
            return null;
        }

        int layer = 0;
        int inputIndex = 0;
        //go through each network layer
        foreach (List<Neuron> neuronLayer in neuronlist)
        {
            //and each neuron in that layer
            foreach (Neuron neuron in neuronLayer)
            {

                //If first layer, add input values
                if (layer == 0)
                {
                    //put input values into a neuron and add the output of the neuron to layerResults first layers neuron output
                    layerResults[layer].Add(neuron.activate(inputvalues[inputIndex]));
                    inputIndex++;
                }

                else
                {
                    //input results from last layers neuron results to next layers neurons and add the results to lauerResults list of lists
                    layerResults[layer].Add(neuron.activate(layerResults[layer - 1]));
                }

            }
            layer++;
        }

        //print layer results if verbose enabled
        if(EnableVerbose == true)
        {
            int index = 0;
            foreach (List<float> layerlist in layerResults)
            {
                Debug.Log("Layer " + index + " results");
                foreach (float neuronvalue in layerlist)
                {
                    Debug.Log(neuronvalue);
                }
                index++;
            }
        }
        
        return layerResults[layerResults.Count - 1];
    }
	
    //randomizes all values
    public void randomize()
    {
        foreach (List<Neuron> neuronlayer in neuronlist)
        {
            foreach(Neuron neuron in neuronlayer)
            {
                neuron.randomize();
            }
        }
    }

    //Mutates the network with given variables
    //neuronstomutate = what percentage of neurons to mutate 1 = all 0 none
    //weightstomutate = what percentage of neurons weights are to be mutated
    //variance = how much to change the values. for example if 0.5 is given, a value can go higher or lower by that amount
    //if given the values neuronstomutate = 0.1, weightstomutate = 0.1 and variance 0.1 = 10 percent of neurons are going through mutation and 10 percent of their weights are mutated by +- 0.1
    public void mutateNetwork(float neuronstomutate,float weightstomutate, float variance)
    {
        //Loop through all neurons
        foreach (List<Neuron> neuronlayer in neuronlist)
        {
            foreach (Neuron neuron in neuronlayer)
            {
                if (Random.Range(0f, 1f) <= neuronstomutate)
                {
                    neuron.mutate(weightstomutate, variance);
                }
            }
        }


    }

    //outputs the structure of the network as a list of ints containing the amount of neurons in each layer
    public List<int> getNetworkStructure(bool verbose = false)
    {
        List<int> structure = new List<int>();

        int layer = 0;
        foreach (List<Neuron> neuronlayer in neuronlist)
        {
            structure.Add(0);
            foreach (Neuron neuron in neuronlayer)
            {
                structure[layer]++;
            }
            layer++;
        }

        layer = 0;
        if (verbose == true)
        {
            Debug.Log("the network consist of " + structure.Count + " layers");
            foreach (int value in structure)
            {
                Debug.Log("Layer " + layer + " has " + structure[layer] + " neurons");
                    layer++;
            }
        }

        return structure;
    }

    public void train_backpropagation(List<List<float>> trainingdata, List<List<float>> desiredoutput,int epoch)
    {
        
        //check that trainingdata and desired output lists have the same amount of values
        if(trainingdata.Count == desiredoutput.Count)
        {
            int j = 0;
            while (j < epoch)
            {
                j++;
                //loop through the training data
                for (int i = 0; i < trainingdata.Count; i++)
                {
                    //###########################################forward propagation###########################################

                    //calculate outputs of the network
                    calculate(trainingdata[i]);

                    //###########################################adjust the output layer error#################################

                    //go through the output layer neurons and adjust their weights and biases
                    int k = 0; //used to get the result of the right neuron
                    foreach (Neuron neuron in neuronlist[neuronlist.Count - 1])
                    {
                        neuron.error = sigmoidDerivative(neuron.output) * (desiredoutput[i][k] - neuron.output);
                        k++;
                        neuron.adjustWeights();
                    }

                    //###########################################adjust the hidden layers error#################################

                    //go through all hidden layers neurons and adjust their weights and biases
                    k = 0;
                    for (int l = 0; l < neuronlist.Count - 1; l++)
                    {
                        foreach (Neuron neuron in neuronlist[neuronlist.Count - 2 - l])
                        {
                            float weights = 0f;
                            //calculate the sum of next layers weights affecting current neuron
                            for (int m = 0; i < neuronlist[neuronlist.Count - 1 - l].Count; m++)
                            {
                                weights *= neuronlist[neuronlist.Count - 1 - l][m].weigths[k];
                            }

                            neuron.error = sigmoidDerivative(neuron.output) * weights;
                            neuron.adjustWeights();
                            k++;
                        }
                    }

                }
            }
            

        }
        //return if there is mismatch between trainingdata and desired results
        else
        {
            Debug.Log("Traningdata & desiredoutput mismatch");
            return;
        }

        
    }

    private float sigmoidDerivative(float x)
    {
        return x*(1-x);
    }
}
