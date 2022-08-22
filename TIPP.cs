using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetAssurancePortefeuille.Strategie
{
    class TIPP : Strategie
    {
        private double floor_tipp;
        private double multiplicator;
        public Double Multiplicator
        {
            get { return multiplicator; }
            set { multiplicator = value; }
        }
        public Double Floor_tipp
        {
            get { return floor_tipp; }
            set { floor_tipp = value; }
        }

        // Constructor
        public TIPP(List<Data> risky_returns, List<Data> riskless_returns, double floor, double cushion, double PerfConstraint, double mise_de_depart, double multiplicator, double maturity, string period) : base(risky_returns, riskless_returns, floor, cushion, PerfConstraint, mise_de_depart, maturity, period)
        {
            this.floor_tipp = this.PerfConstraint;
            this.multiplicator = multiplicator;

        }
        public override void weights()
        {
            double TIPP_E = Convert.ToDouble(this.risky_returns[0].Price);
            double TIPP_M = Convert.ToDouble(this.riskless_returns[0].Price);
            double Portfolio_value = this.Mise_De_Depart;

            //F0
            var periodicite = this.Period == "DAILY" ? 252 : this.Period == "WEEKLY" ? 52 : this.Period == "MONTHLY" ? 12 : this.Period == "YEARLY" ? 1 : 0;
            //this.floor = this.Mise_De_Depart * this.floor;


            for (int i = 0; i < periodicite * this.Maturity; i++)
            {
                /// demander à youssef si le premier return est égale à zero
                TIPP_E = TIPP_E * Convert.ToDouble(risky_returns[i].Returns);
                TIPP_M = TIPP_M * Convert.ToDouble(riskless_returns[i].Returns);
                Portfolio_value = Portfolio_value + TIPP_E + TIPP_M;
                //saving dans portfolio value in Datas 
                risky_returns[i].Portfolio_Value = Portfolio_value;
                riskless_returns[i].Portfolio_Value = Portfolio_value;

                // Floor dynamique 
                if (this.floor_tipp * Portfolio_value > this.floor)
                {
                    this.floor = this.floor_tipp * Portfolio_value;
                }

                this.cushion = Math.Max((Portfolio_value - this.floor), 0);
                TIPP_E = this.multiplicator * this.cushion;
                TIPP_M = Portfolio_value - TIPP_E;

                risky_returns[i].Weights = TIPP_E / Portfolio_value;
                riskless_returns[i].Weights = TIPP_M / Portfolio_value;

            }

        }
    }
}
