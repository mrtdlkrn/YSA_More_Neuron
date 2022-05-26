using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YSA
{
    public partial class Form1 : Form
    {
        class Data
        {
            public double[] input { get; set; }
            public int output { get; set; }

            public Data(double[] input, int output)
            {
                this.input = input;
                this.output = output;
            }
        }

        class Neuron
        {
            public double bias { get; set; }
            public double[] w { get; set; }
            public Function function { get; set; }

            public Neuron(int dimension, double bias, Function function)
            {
                this.bias = bias;
                this.function = function;
                w = new double[dimension];
                for (int i = 0; i < dimension; i++)
                {
                    w[i] = new Random().NextDouble();
                }
            }
            public string getW()
            {
                string value = "";
                for (int i = 0; i < w.Length; i++)
                {
                    value += " " + w[i].ToString();
                }
                return value;
            }

            public double getClass(int classNumber,int classIndex) {
                if (classNumber == classIndex)
                {
                    return 1;
                }
                else
                    return -1;
            }
        }

        abstract class Function
        {
            public double net(double[] input, double[] w, double bias)
            {
                double sum = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    sum += input[i] * w[i];
                }
                return sum + w[w.Length - 1] * bias;
            }

            public abstract double calculate(double net);
        }

        class BinaryFunction : Function
        {
            public override double calculate(double net)
            {
                if (net > 0)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        class ContinousFunction : Function
        {
            public override double calculate(double net)
            {
                return 1 / (1 + Math.Pow(Math.E, -net));
            }
        }
        List<Data> dataList;
        int classCount;
        public Form1()
        {
            InitializeComponent();
            dataList = new List<Data>();
            
            double[] d1 = { 1, 2, 4, -1 };
            double[] d2 = { 2, 3, -1, 7 };
            double[] d3 = { -1, 0, 2, -3 };
            double[] d4 = { -1, -2, 4, 2 };
            double[] d5 = { 1, 2, -4, -2 };
            double[] d6 = { 4, 5, -2, 3 };
            dataList.Add(new Data(d1, 0));
            dataList.Add(new Data(d2, 2));
            dataList.Add(new Data(d3, 1));
            dataList.Add(new Data(d4, 2));
            dataList.Add(new Data(d5, 0));
            dataList.Add(new Data(d6, 1));
            HashSet<int> classArray = new HashSet<int>();
            for(int i = 0; i < dataList.Count; i++)
                classArray.Add(dataList[i].output);
            classCount = classArray.Count;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            singleLayerMultiNeuron(1.0000, 4, 1.0000);
        }

        public void singleLayerMultiNeuron(double c, int dimension, double bias)
        {
            List<Neuron> neuronList = new List<Neuron>();
            /*Neuron[] neuron = new Neuron[classCount];*/ // Diffirent option 
            /*for(int i = 0; i < classCount; i++) { 
                neuron[i] = new Neuron(dimension, bias, new BinaryFunction());
            }*/
            for(int i = 0; i < classCount; i++) { 
                neuronList.Add(new Neuron(dimension, bias, new BinaryFunction()));
            }
            int maxfes = 1000;
            int fes = 0;
            string str ="";
            double error = 0;
            while (fes<maxfes)
            {
                
                for (int i = 0; i < dataList.Count; i++)
                {
                    for(int j = 0; j < neuronList.Count; j++) { 
                        double net = neuronList[j].function.net(dataList[i].input, neuronList[j].w, neuronList[j].bias);
                        double fnet = neuronList[j].function.calculate(net);
                        for (int k = 0; k < (dimension - 1); k++)
                        {
                            neuronList[j].w[k] += c * (neuronList[j].getClass(dataList[i].output,j) - fnet) * dataList[i].input[k];
                        }
                        neuronList[j].w[dimension - 1] += c * (neuronList[j].getClass(dataList[i].output, j) - fnet) * neuronList[j].bias;
                        error += Math.Pow(neuronList[j].getClass(dataList[i].output, j) - fnet, 2) / 2;
                    }
                }
                fes++;
                if (error < 0.1 * classCount)
                    break;
            }
            for(int i = 0; i < neuronList.Count; i++)
            {
                str+=" Neuron"+(i+1)+ neuronList[i].getW()+"\n";
            }
            str += "error = " + error;
            MessageBox.Show(str);
        }
    }
}
