using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetAssurancePortefeuille
{
    class Data
    {
        public DateTime Date
        {
            get; set;
        }

        public decimal Price
        {
            get; set;
        }
        public string Volatility // Instancier des valeurs nulles
        {
            get; set;
        }

        public string Asset
        {
            get; set;
        }
        public decimal Returns
        {
            get; set;
        }

        public double Portfolio_Value
        {
            get; set;
        }
        public double Weights
        {
            get; set;
        }

    }
}
