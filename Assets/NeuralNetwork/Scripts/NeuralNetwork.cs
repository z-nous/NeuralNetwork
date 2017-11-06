using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeuralNetwork{

    //List that contains a list of neuron.s this is the whole neural network
    private List<List<Neuron>> neuronlist = new List<List<Neuron>>();
    //list of lists containing calculated results of neurons
    private List<List<double>> layerResults = new List<List<double>>();
    private double totalError = 0f;

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


    //processes the neural network and returns a list of doubles that contains the answers from the network
    public List<double> calculate(List<double> inputvalues, bool EnableVerbose = false)
    {
        //clear layerresults list before doing things to it
        layerResults.Clear();
        layerResults.TrimExcess();
        //initialize layerResults List

        for (int j = 0; j < neuronlist.Count; j++)
        {
            layerResults.Add(new List<double>());
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
            foreach (List<double> layerlist in layerResults)
            {
                Debug.Log("Layer " + index + " results");
                foreach (double neuronvalue in layerlist)
                {
                    Debug.Log(neuronvalue);
                }
                index++;
            }
        }
        
        return layerResults[layerResults.Count - 1];
    }
	
    public double train_backpropagation(List<List<double>> trainingdata, List<List<double>> desiredoutput,int epoch,double trainingspeed = 1f)
    {
        
        //check that trainingdata and desired output lists have the same amount of values
        if(trainingdata.Count == desiredoutput.Count)
        {
            int j = 0;

            while (j < epoch)
            {
                j++;

                /*
                foreach (List<Neuron> neurolist in neuronlist)
                {
                    foreach (Neuron neuron in neurolist)
                    {
                        neuron.error = 0.0;
                    }
                }*/
                
                //loop through the training data
                for (int i = 0; i < trainingdata.Count; i++)
                {
                    //###########################################forward propagation and error calculation#############################

                    //calculate outputs of the network
                    calculate(trainingdata[i]);

                    //add errors to the output neurons

                    int desiredOutputIndex = 0;
                    foreach (Neuron neuron in neuronlist[neuronlist.Count - 1])
                    {
                        neuron.error += (sigmoid.derivative(neuron.output) * (desiredoutput[i][desiredOutputIndex] - neuron.output)) * trainingspeed;
                        desiredOutputIndex++;
                    }
                    
                }

                //###########################################adjust the output layer weights#################################

                foreach (Neuron neuron in neuronlist[neuronlist.Count - 1])
                {
                    neuron.adjustWeights();
                }

                //###########################################adjust the hidden layers weights#################################

                //go through all hidden layers neurons and adjust their weights and biases

                int k = 0;
                for (int l = 0; l < neuronlist.Count - 2; l++)
                {
                    foreach (Neuron neuron in neuronlist[neuronlist.Count - 2 - l])
                    {
                        double weights = 1f;
                        //*calculate the sum of next layers weights affecting current neuron
                        foreach (Neuron neuron2 in neuronlist[neuronlist.Count - 1 - l])
                        {
                            weights = weights * neuron2.weigths[k];
                        }

                        neuron.error = (sigmoid.derivative(neuron.output) * weights) * trainingspeed;
                        neuron.adjustWeights();
                        k++;

                    }
                    k = 0;
                }
            }

        }
        //return if there is mismatch between trainingdata and desired results
        else
        {
            Debug.Log("Traningdata & desiredoutput mismatch");
            return 1.0;
        }

        return totalError;
    }


    //Mutates the network with given variables
    //neuronstomutate = what percentage of neurons to mutate 1 = all 0 none
    //weightstomutate = what percentage of neurons weights are to be mutated
    //variance = how much to change the values. for example if 0.5 is given, a value can go higher or lower by that amount
    //if given the values neuronstomutate = 0.1, weightstomutate = 0.1 and variance 0.1 = 10 percent of neurons are going through mutation and 10 percent of their weights are mutated by +- 0.1
    public void mutateNetwork(double neuronstomutate, double weightstomutate, double variance)
    {
        //Loop through all neurons
        foreach (List<Neuron> neuronlayer in neuronlist)
        {
            foreach (Neuron neuron in neuronlayer)
            {
                if (UnityEngine.Random.Range(0f, 1f) <= neuronstomutate)
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

    //randomizes all values
    public void randomize()
    {
        foreach (List<Neuron> neuronlayer in neuronlist)
        {
            foreach (Neuron neuron in neuronlayer)
            {
                neuron.randomize();
            }
        }
    }

    //#########################################functions to get infomration about neurons#######################################################
    public double getNeuronOutput(int layer, int neuronNumber)
    {
        return neuronlist[layer][neuronNumber].output;
    }

    public double getNeuronerror(int layer, int neuronNumber)
    {
        return neuronlist[layer][neuronNumber].error;
    }

    public double getNeuronbias(int layer, int neuronNumber)
    {
        return neuronlist[layer][neuronNumber].bias;
    }
}
/*Storage for code
 

       public double train_backpropagation(List<List<double>> trainingdata, List<List<double>> desiredoutput,int epoch,double trainingspeed = 1f)
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
                   

                    //###########################################adjust the output layer weights#################################

                    //go through the output layer neurons and adjust their weights and biases
                    int k = 0; //used to get the result of the right neuron
                    foreach (Neuron neuron in neuronlist[neuronlist.Count - 1])
                    {
                        //Debug.Log("neuroninfo");
                        //Debug.Log("neuron error " + neuron.error);
                        //Debug.Log("neuron output " + neuron.output);
                        neuron.error = (sigmoid.derivative(neuron.output) * (desiredoutput[i][k] - neuron.output)) * trainingspeed;
                        neuron.adjustWeights();
                        k++;
                    }
                    
                    //###########################################adjust the hidden layers weights#################################

                    //go through all hidden layers neurons and adjust their weights and biases
                    
                    k = 0;
                    //Debug.Log(neuronlist.Count - 3);
                    for (int l = 0; l < neuronlist.Count - 2; l++)
                    {
                        foreach (Neuron neuron in neuronlist[neuronlist.Count - 2 - l])
                        {
                            double weights = 1f;

                            //*calculate the sum of next layers weights affecting current neuron
                            foreach (Neuron neuron2 in neuronlist[neuronlist.Count - 1 - l])
                            {
                                //Debug.Log(neuron2.weigths[k]);
                                weights = weights * neuron2.weigths[k];
                            }

                            neuron.error = (sigmoid.derivative(neuron.output) * weights)*trainingspeed;
                            neuron.adjustWeights();
                            k++;
                            
                        }
                        k = 0;
                    }

                }
            }
            //####################################################calulate network squared errorr##########################################
            calculate(trainingdata[1]);
            totalError = 0;
            int f = 0;
            foreach (Neuron neuron in neuronlist[neuronlist.Count - 1])
            {
                //Debug.Log(Mathf.Pow(0.5f * (trainingdata[1][f] - neuron.output), 2f));
                totalError += (Math.Exp(0.5 * (trainingdata[1][f] - neuron.output)));
                f++;
            }

            Debug.Log("Network total error " + totalError);
        }
        //return if there is mismatch between trainingdata and desired results
        else
        {
            Debug.Log("Traningdata & desiredoutput mismatch");
            return 1.0;
        }

        return totalError;
    }


 */
