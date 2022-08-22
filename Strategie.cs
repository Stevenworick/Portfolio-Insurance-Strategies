using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetAssurancePortefeuille.Strategie
{
    abstract class Strategie
    {
        public List<Data> risky_returns;
        public List<Data> riskless_returns;
        public double floor;
        public double cushion;
        public double PerfConstraint;
        public double Mise_De_Depart;
        public double Maturity;
        public string Period;

        // Constructor
        public Strategie(List<Data> risky_returns, List<Data> riskless_returns, double floor, double cushion, double PerfConstraint, double mise_de_depart, double maturity, string period)
        {
            this.cushion = cushion;
            this.floor = floor;
            this.PerfConstraint = PerfConstraint; // mettre la value du Userform
            this.riskless_returns = riskless_returns;
            this.risky_returns = risky_returns;
            this.Mise_De_Depart = mise_de_depart;
            this.Maturity = maturity;
            this.Period = period;
        }


        public abstract void weights();

        ~Strategie()
        {
            Console.WriteLine("Destruction de la Stratégie");
        }
    }
}
