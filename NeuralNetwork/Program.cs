using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            NeuralNetwork network = new NeuralNetwork();

            NeuralLayer inputLayer = new NeuralLayer(4, 0);
            inputLayer.Neurons[0].OutputPulse = 0.5;
            inputLayer.Neurons[1].OutputPulse = 0.2;
            inputLayer.Neurons[2].OutputPulse = 0.3;
            inputLayer.Neurons[3].OutputPulse = 0.07;
            NeuralLayer hiddenLayer = new NeuralLayer(5, 1);
            NeuralLayer outputLayer = new NeuralLayer(3, 2);

            network.AddLayer(inputLayer);
            network.AddLayer(hiddenLayer);
            network.AddLayer(outputLayer);

            network.Build();
            network.Print();

            Console.Read();
        }
    }

    class Neuron
    {
        public double OutputPulse { get; set; }
        public List<Connection> Connections { get; set; }

        public Neuron()
        {
            Connections = new List<Connection>();
            OutputPulse = new double();
        }

        public void Fire()
        {
            OutputPulse = Sum();
            OutputPulse = Activation(OutputPulse);
            OutputPulse = Math.Round(OutputPulse, 3);
        }

        public double Sum()
        {
            double value = 0;
            foreach (var C in Connections)
            {
                value += C.InputPulse * C.Weight;
            }
            return value;
        }

        public double Activation(double pulse)
        {
            pulse = 1 / (1 + Math.Pow(Math.E, -pulse));         //Sigmoida jako funkcja aktywacji
            return pulse;
        }


    }

    class Connection
    {
        public double Weight { get; set; }
        public double InputPulse { get; set; }
    }

    class NeuralLayer
    { 
        public List<Neuron> Neurons { get; set; }
        public int Id { get; set; }

        public NeuralLayer(int n, int Id)
        {
            Neurons = new List<Neuron>();

            for (int i = 0; i < n; i++)
            {
                Neurons.Add(new Neuron());
            }
            this.Id = Id;
        }

    }

    class NeuralNetwork
    {
        public List<NeuralLayer> Layers { get; set; }
        public NeuralNetwork()
        {
            Layers = new List<NeuralLayer>();
        }

        public void AddLayer(NeuralLayer newLayer)
        {
            int connectionCount = 1;

            if (Layers.Count > 0)
            {
                connectionCount = Layers.Last<NeuralLayer>().Neurons.Count;
            }
            /*
            foreach (var neuron in newLayer.Neurons)
            {
                for (int i = 0; i < connectionCount; i++)
                {
                   neuron.Connections.Add(new Connection());
                }
            }
            */
            Layers.Add(newLayer);
        }

        public void Build()
        {
            int i = 0;
            foreach (var layer in Layers)
            {
                if (i>=Layers.Count-1)
                {
                    break;
                }
                NeuralLayer nextLayer = Layers[i + 1];
                Create(layer, nextLayer);
                i++;
            }
        }

        public void Create(NeuralLayer fromLayer, NeuralLayer toLayer)
        {
            Random rand = new Random();
            foreach (var toNeuron in toLayer.Neurons)
            {
                foreach (var fromNeuron in fromLayer.Neurons)
                {
                    toNeuron.Connections.Add(new Connection() { InputPulse = fromNeuron.OutputPulse, Weight = rand.NextDouble() * 0.5+0.001 });  //inicjowanie połączeń losowymi wartościami i przekazywanie pulsu z poprzedzającego neurona
                    toNeuron.Fire();
                }
            }
        }

        public void Print()
        {
            for (int i = 0; i < Layers.Count; i++)
            {
                    Console.Write("LAYER: " + Layers[i].Id + "  ");
                    for (int j = 0; j < Layers[i].Neurons.Count; j++)
                    {
                    Console.Write(" " + Layers[i].Neurons[j].OutputPulse);
                    }

                Console.WriteLine();


                if (i < Layers.Count - 1)
                {
                    Console.Write("CONNECTIONS: ");
                    for (int j = 0; j < Layers[i + 1].Neurons.Count; j++)
                    {
                        foreach (var connection in Layers[i + 1].Neurons[j].Connections)
                        {
                            
                            Console.Write(" " + Math.Round(connection.Weight,3));
                        }
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
