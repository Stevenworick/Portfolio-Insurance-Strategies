using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetAssurancePortefeuille.Strategie
{
    class CPPI : Strategie
    {
        private double multiplicator;
        public Double Multiplicator
        {
            get { return multiplicator; }
            set { multiplicator = value; }
        }

        // Constructor
        public CPPI(List<Data> risky_returns, List<Data> riskless_returns, double floor, double cushion, double PerfConstraint, double mise_de_depart, double multiplicator, double maturity, string period) : base(risky_returns, riskless_returns, floor, cushion, PerfConstraint, mise_de_depart, maturity, period)
        {
            this.multiplicator = multiplicator;

        }
        public override void weights()
        {
            double CPPI_E = Convert.ToDouble(this.risky_returns[0].Price);
            double CPPI_M = Convert.ToDouble(this.riskless_returns[0].Price);
            double Portfolio_value = this.Mise_De_Depart;

            var periodicite = this.Period == "DAILY" ? 252 : this.Period == "WEEKLY" ? 52 : this.Period == "MONTHLY" ? 12 : this.Period == "YEARLY" ? 1 : 0;
            //this.floor = this.Mise_De_Depart * this.floor;


            for (int i = 0; i < periodicite * this.Maturity; i++)
            {
                
                CPPI_E = CPPI_E * Convert.ToDouble(risky_returns[i].Returns);
                CPPI_M = CPPI_M * Convert.ToDouble(riskless_returns[i].Returns);
                Portfolio_value = Portfolio_value + CPPI_E + CPPI_M;
                //saving dans portfolio value in Datas 
                risky_returns[i].Portfolio_Value = Portfolio_value;
                riskless_returns[i].Portfolio_Value = Portfolio_value;

                this.cushion = Math.Max((Portfolio_value - this.floor), 0);
                CPPI_E = Math.Min(this.multiplicator * this.cushion, Portfolio_value);
                CPPI_M = Math.Max((Portfolio_value - CPPI_E), 0);

                risky_returns[i].Weights = CPPI_E / Portfolio_value;
                riskless_returns[i].Weights = CPPI_M / Portfolio_value;

            }

        }
    }
}
