using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace COHONEN_CARTS
{
    public partial class Form1 : Form
    {
        public Bitmap Image { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var random = new Random();

            int countOfNeurons = 128;

            List<double> input = new List<double>(3)
            {
                random.Next(0, 255),
                random.Next(0, 255),
                random.Next(0, 255)
            };

            int countOfTrainings = int.Parse(textBox1.Text);

            Network network = new Network(input, countOfNeurons);

            drawImage(network, true, countOfNeurons);

            while (network.Step <= countOfTrainings)
            {
                int step = network.TrainingStepByStep();

                if (step % 100 == 0)
                {
                    double percents = (double) step / countOfTrainings * 100;
                    Console.Clear();
                    Console.WriteLine("Progress: {0:0}%", percents);
                }
            }

            drawImage(network, false, countOfNeurons);
        }

        private void drawImage(Network network, bool beforeLearning, int countOfNeurons)
        {
            int boxSize = countOfNeurons * 2;

            pictureBox1.Size = new Size(boxSize, boxSize);
            this.Controls.Add(pictureBox1);

            Bitmap flag = new Bitmap(boxSize, boxSize);

            for (int i = 0; i < countOfNeurons; ++i)
            {
                for (int j = 0; j < countOfNeurons; ++j)
                {
                    //var componentA = Math.Abs((int)(network.Matrix[i, j].Weights[0] * 1000)) % 255;
                    //var componentB = Math.Abs((int)(network.Matrix[i, j].Weights[1] * 1000)) % 255;
                    //var componentC = Math.Abs((int)(network.Matrix[i, j].Weights[2] * 1000)) % 255;

                    int componentA = (int)network.Matrix[i, j].Weights[0] % 255;
                    int componentB = (int)network.Matrix[i, j].Weights[1] % 255;
                    int componentC = (int)network.Matrix[i, j].Weights[2] % 255;

                    Color color = Color.FromArgb(componentA, componentB, componentC);

                    flag.SetPixel(i * 2, j * 2, color);
                    flag.SetPixel(i * 2 + 1, j * 2, color);
                    flag.SetPixel(i * 2, j * 2 + 1, color);
                    flag.SetPixel(i * 2 + 1, j * 2 + 1, color);
                }
            }

            var resizedBitMap = new Bitmap(flag, new Size(boxSize * 2, boxSize * 2));

            if (beforeLearning)
            {
                pictureBox1.Image = resizedBitMap;
                pictureBox1.Image = resizedBitMap;
            }
            else
            {
                pictureBox2.Image = resizedBitMap;
            }

            Image = resizedBitMap;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int countOfNeurons = 100;

            List<List<double>> inputArray = new List<List<double>>();

            Random random = new Random();

            for (int i = 0; i < 32; ++i)
            {
                inputArray.Add(new List<double>
                {
                    random.Next(0, 255),
                    random.Next(0, 255),
                    random.Next(0, 255)
                });
            }

            int countOfTrainings = int.Parse(textBox1.Text);

            Network network = new Network(inputArray, countOfNeurons);

            drawImage(network, true, countOfNeurons);

            while (network.Step <= countOfTrainings)
            {
                int step = network.TrainingStepByStepWithInputArray();

                if (step % 100 == 0)
                {
                    double percents = (double)step / countOfTrainings * 100;
                    Console.Clear();
                    Console.WriteLine("Progress: {0:0}%", percents);
                }
            }

            drawImage(network, false, countOfNeurons);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Image.Save(@"C:\Users\dimkijeee\source\repos\COHONEN_CARTS\COHONEN_CARTS\Photos\ImageBitmap.bmp");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap image = new Bitmap(@"C:\Users\dimkijeee\source\repos\COHONEN_CARTS\COHONEN_CARTS\Photos\SophiaAndThree.jpg");

            int countOfNeurons = 1280;

            List<List<double>> inputArray = new List<List<double>>();

            Random random = new Random();

            //for (int i = 0; i < 128; ++i)
            //{
            //    inputArray.Add(new List<double>
            //    {
            //        random.Next(0, 255),
            //        random.Next(0, 255),
            //        random.Next(0, 255)
            //    });
            //}

            //for (int i = 0; i < 5; ++i)
            //{
            //    inputArray.Add(new List<double>
            //    {
            //        0 + i*10, 0 + i*10, 0 + i*10
            //    });

            //    inputArray.Add(new List<double>
            //    {
            //        255 - i*10, 255 - i*10, 255 - i*10
            //    });
            //}

            inputArray.Add(new List<double>
                {
                    243, 235, 12
                });
            inputArray.Add(new List<double>
            {
                152, 147, 2
            });
            inputArray.Add(new List<double>
            {
                193, 191, 110
            });

            int countOfTrainings = int.Parse(textBox1.Text);

            Network network = new Network(inputArray, countOfNeurons);

            for (int i = 0; i < 1280; ++i)
            {
                for (int j = 0; j < 1280; ++j)
                {
                    network.Matrix[i, j].Weights[0] = image.GetPixel(i, j).R;
                    network.Matrix[i, j].Weights[1] = image.GetPixel(i, j).G;
                    network.Matrix[i, j].Weights[2] = image.GetPixel(i, j).B;
                }
            }

            drawImage(network, true, countOfNeurons);

            while (network.Step <= countOfTrainings)
            {
                int step = network.TrainingStepByStepWithInputArray();

                if (step % 1 == 0)
                {
                    double percents = (double)step / countOfTrainings * 100;
                    Console.Clear();
                    Console.WriteLine("Progress: {0:0}%", percents);
                }
            }

            drawImage(network, false, countOfNeurons);
        }
    }
}
