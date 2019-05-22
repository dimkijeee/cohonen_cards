using System;
using System.Collections.Generic;
using System.Drawing;

namespace COHONEN_CARTS
{
    public class Network
    {
        public Neuron[,] Matrix { get; set; }

        public int Size { get; set; }

        public List<List<double>> InputArray { get; set; }  

        public List<double> Input { get; set; }

        public int InputVectorToProcess { get; set; } = 0;

        public int Step { get; set; }

        public double InitialSigma { get; set; }

        public double Tau { get; set; }

        public double InitialLearningSpeed { get; set; }

        public Network(List<double> input, int countOfNeurons)
        {
            Size = countOfNeurons;

            Random random = new Random();

            //Fill neurons
            Matrix = new Neuron[countOfNeurons, countOfNeurons];
            for (int k = 0; k < countOfNeurons; k++)
            {
                for (int i = 0; i < countOfNeurons; ++i)
                {
                    List<double> weights = new List<double>(input.Count);

                    for (int j = 0; j < input.Count; ++j)
                    {
                        weights.Add(random.Next(0, 255));
                    }

                    Matrix[k, i] = new Neuron(input.Count)
                    {
                        Weights = weights
                    };
                }
            }

            Input = input;

            InitialLearningSpeed = 0.1;

            InitialSigma = Math.Sqrt(countOfNeurons * countOfNeurons * 2) / 2;
            //InitialSigma = countOfNeurons * 4;

            Tau = 1000 / Math.Log(InitialSigma);

            Step = 0;
        }

        public Network(List<List<double>> inputArray, int countOfNeurons)
            : this(inputArray[0], countOfNeurons)
        {
            InputArray = inputArray;
        }

        public Point Competition()
        {
            int iIndexToReturn = 0;
            int jIndexToReturn = 0;

            double euqlidNorm = 1000000;

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    double sum = 0;

                    for (int k = 0; k < Input.Count; ++k)
                    {
                        sum += (Input[k] - Matrix[i, j].Weights[k]) * (Input[k] - Matrix[i, j].Weights[k]);
                    }

                    double tempEuqlidNorm = Math.Sqrt(sum);

                    if (tempEuqlidNorm < euqlidNorm)
                    {
                        euqlidNorm = tempEuqlidNorm;

                        iIndexToReturn = i;
                        jIndexToReturn = j;
                    }
                }
            }

            return new Point(iIndexToReturn, jIndexToReturn);
        }

        public double Cooperation(int iExcitedNeuron, int jExcitedNeuron, int iWinnerNeuron, int jWinnerNeuron, double sigma)
        {
            if (sigma < 1)
            {
                sigma = 1;
            }

            Point vector = new Point(iExcitedNeuron - iWinnerNeuron, jExcitedNeuron - jWinnerNeuron);

            double d_ji = vector.X * vector.X + vector.Y * vector.Y;

            double h_ji = Math.Exp(-1 * (d_ji / (2 * sigma * sigma)));

            if (Step % 100 == 0 && Step != 0 && iExcitedNeuron == 0 && jExcitedNeuron == 0)
            {
                Console.WriteLine("SIGMA = " + sigma);
                Console.WriteLine("H_ji = " + h_ji);
                Console.WriteLine("POSITION OF WINNER NEURON = " + iWinnerNeuron + " - " + jWinnerNeuron);
            }

            return h_ji;
        }

        public void Adaptation(double h_ji, int iExcitedNeuron, int jExcitedNeuron, double learningSpeed)
        {
            if (learningSpeed < 0.01)
            {
                learningSpeed = 0.01;
            }

            if (Step % 100 == 0 && Step != 0 && iExcitedNeuron == 0 && jExcitedNeuron == 0)
            {
                Console.WriteLine("LEARNING SPEED = " + learningSpeed);
            }

            for (int i = 0; i < Input.Count; ++i)
            {
                Matrix[iExcitedNeuron, jExcitedNeuron].Weights[i] +=
                    learningSpeed * h_ji * (Input[i] - Matrix[iExcitedNeuron, jExcitedNeuron].Weights[i]);
            }
        }

        public void Training(int countOfSteps)
        {
            while (Step <= countOfSteps)
            {
                double learningSpeed = InitialLearningSpeed * Math.Exp(-1 * (double)(Step / 1000));

                double sigma = InitialSigma * Math.Exp(-1 * Step / Tau);

                Point winnerNeuronPosition = Competition();

                for (int i = 0; i < Size; ++i)
                {
                    for (int j = 0; j < Size; ++j)
                    {
                        double h_ji = Cooperation(i, j, winnerNeuronPosition.X, winnerNeuronPosition.Y, sigma);

                        Adaptation(h_ji, i, j, learningSpeed);
                    }
                }

                Step++;
            }
        }

        public int TrainingStepByStep()
        {
            double learningSpeed = InitialLearningSpeed * Math.Exp(-1 * (double)(Step / 1000));

            double sigma = InitialSigma * Math.Exp(-1 * Step / Tau);

            Point winnerNeuronPosition = Competition();

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    double h_ji = Cooperation(i, j, winnerNeuronPosition.X, winnerNeuronPosition.Y, sigma);

                    Adaptation(h_ji, i, j, learningSpeed);
                }
            }

            return ++Step;
        }

        public int TrainingStepByStepWithInputArray()
        {
            Input = InputArray[InputVectorToProcess++ % InputArray.Count];

            double learningSpeed = InitialLearningSpeed * Math.Exp(-1 * (double)(Step / 1000));

            double sigma = InitialSigma * Math.Exp(-1 * Step / Tau);

            Point winnerNeuronPosition = Competition();

            for (int i = 0; i < Size; ++i)
            {
                for (int j = 0; j < Size; ++j)
                {
                    double h_ji = Cooperation(i, j, winnerNeuronPosition.X, winnerNeuronPosition.Y, sigma);

                    Adaptation(h_ji, i, j, learningSpeed);
                }
            }

            return ++Step;
        }
    }
}
