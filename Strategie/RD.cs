using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetAssurancePortefeuille.Strategie
{
    class RD : Strategie
    {
        private double multiplicator;
        public Double Multiplicator
        {
            get { return multiplicator; }
            set { multiplicator = value; }
        }

        // Constructor
        public RD(List<Data> risky_returns, List<Data> riskless_returns, double floor, double cushion, double PerfConstraint, double mise_de_depart, double multiplicator, double maturity, string period) : base(risky_returns, riskless_returns, floor, cushion, PerfConstraint, mise_de_depart, maturity, period)
        {
            this.multiplicator = multiplicator;

        }
        public override void weights()
        {

            // Dans cette stratégie le benchmark est le riskless asset

            // Initialisation
            double Portfolio_value = this.Mise_De_Depart;
            double Z = Portfolio_value / Convert.ToDouble(this.riskless_returns[0].Price); // Z est le maximum de PV/Bench
            this.floor = PerfConstraint * Z * Convert.ToDouble(this.riskless_returns[0].Price);
            this.cushion = Math.Max(Portfolio_value - this.floor, 0);
            this.risky_returns[0].Weights = this.multiplicator * this.cushion / Portfolio_value;
            this.riskless_returns[0].Weights = 1 - this.risky_returns[0].Weights;

            var periodicite = this.Period == "DAILY" ? 252 : this.Period == "WEEKLY" ? 52 : this.Period == "MONTHLY" ? 12 : this.Period == "YEARLY" ? 1 : 0;



            for (int i = 1; i < periodicite * this.Maturity; i++)
            {
                Portfolio_value = (this.risky_returns[i - 1].Weights * (1 + Convert.ToDouble(this.risky_returns[i].Returns)) + this.riskless_returns[i - 1].Weights * (1 + Convert.ToDouble(this.riskless_returns[i].Returns))) * Portfolio_value;
                Z = Math.Max(Z, Portfolio_value / Convert.ToDouble(this.risky_returns[i].Price));
                this.floor = PerfConstraint * Z * Convert.ToDouble(this.riskless_returns[i].Price);
                this.cushion = Math.Max(Portfolio_value - this.floor, 0);
                this.risky_returns[i].Weights = this.multiplicator * this.cushion / Portfolio_value;
                this.riskless_returns[i].Weights = 1 - this.risky_returns[i].Weights; ;

                risky_returns[i].Portfolio_Value = Portfolio_value;
                riskless_returns[i].Portfolio_Value = Portfolio_value;

            }

        }
    }
}
