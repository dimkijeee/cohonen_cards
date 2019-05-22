using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COHONEN_CARTS
{
    public class Neuron
    {
        public List<double> Weights { get; set; }

        public Neuron(int inputSize)
        {
            Weights = new List<double>(inputSize);
        }
    }
}
