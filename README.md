# NeuralNetwork
unity + Neural Network

trying my hand at programming c# neural network.

All the important stuff can be found at Assets/NeuralNetwork/Scripts

the neural network has two scirpts
NeuralNetwork.sc 
Neuron.cs

##Usage:
#to create a neural network
NeuralNetwork MyNeuralNetwork;
MyNeuralNetwork = new NeuralNetwork(int numberofinputs,int numberofoutputs, params int[] numberofneuronsinlayer)

##public functions:
#MyNeuralNetwork.calculate(myinputvalues)
#public List<float> calculate(List<float> inputvalues, bool EnableVerbose = false)
processes the network, takes list of float values as input and outputs a list of floats.
the number of values in inputvalues must match the number of inputneurons in the network
If EnableVerbose is set to true the output value of each neuron in each layer is outputted to debug.log
  
#MyNeuralNetwork.randomize()
#public void randomize()
Randmoizes every neurons weights and biases.

#MyNeuralNetwork.mutateNetwork(0.1f,0.1f.0.1f)
#public void mutateNetwork(float neuronstomutate,float weightstomutate, float variance)
Mutates the network with given variables
neuronstomutate = what percentage of neurons to mutate: 1 = all 0 none
weightstomutate = what percentage of neurons weights are to be mutated: 1 = all 0 none
variance = how much to change the values. for example if 0.5 is given, a random value is chosen between -0.5 and 0.5 and added to existing values
if given the values neuronstomutate = 0.1, weightstomutate = 0.1 and variance 0.1 = 10 percent of neurons are going through mutation and 10 percent of their weights are mutated by +- 0.1

#MyNeuralNetwork.getNetworkStructure(true)
#public List<int> getNetworkStructure(bool verbose = false)
outputs the structure of the network as a list of ints containing the amount of neurons in each layer
  
#MyNeuralNetwork.train_backpropagation()
#public void train_backpropagation(List<List<float>> trainingdata, List<List<float>> desiredoutput,int epoch) 
Still a work in progress. does nothing at the moment.
