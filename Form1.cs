using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjetAssurancePortefeuille
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Rebalance_Frequency_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var Donnee = Utility.GetDatas(Rebalance_Frequency.SelectedItem.ToString());
            var dataAsset1 = Donnee.Where(x => x.Asset == "1").ToList();
            var dataAsset2 = Donnee.Where(x => x.Asset == "2").ToList();
            //var gbm = Utility.gbm(Convert.ToDouble(dataAsset1[0].Price), 5, 200, 0.5, 0.015, 10, 100, true); 

            
            //Calcul Returns
            Utility.calcul_returns(dataAsset2);
            Utility.calcul_returns(dataAsset1);
          
            var investment = textBox1.Text!=""  ? Convert.ToDouble(textBox1.Text) : 0;
            var maturity = textBox2.Text != "" ? Convert.ToDouble(textBox2.Text) : 0;
        
            var multiplicator = textBox4.Text != "" ? Convert.ToDouble(textBox4.Text) : 0;
            var perfConstr = textBox5.Text != "" ? Convert.ToDouble(textBox5.Text) : 0;
            
            //Strategies
            if(comboBox1.SelectedItem.ToString()== "Relative Drawdown")
            {
                var resultat = new Strategie.RD(dataAsset1, dataAsset2, investment * perfConstr, 0, perfConstr, investment, multiplicator, maturity, Rebalance_Frequency.SelectedItem.ToString());
                resultat.weights();
            }
            else if (comboBox1.SelectedItem.ToString() == "CPPI")
            {
                var resultat = new Strategie.CPPI(dataAsset1, dataAsset2, investment * perfConstr, 0, perfConstr, investment, multiplicator, maturity, Rebalance_Frequency.SelectedItem.ToString());
                resultat.weights();
            }
            else
            {
                var resultat = new Strategie.TIPP(dataAsset1, dataAsset2, investment * perfConstr, 0, perfConstr, investment, multiplicator, maturity, Rebalance_Frequency.SelectedItem.ToString());
                resultat.weights();
            }
            chart.ChartAreas[0].AxisY.Maximum = dataAsset1.Where(x => x.Portfolio_Value != 0).Select(x => Math.Round(x.Portfolio_Value, 0)).ToList().Max() + 10;
            chart.ChartAreas[0].AxisY.Minimum = dataAsset1.Where(x => x.Portfolio_Value != 0).Select(x => Math.Round(x.Portfolio_Value, 0)).ToList().Min() -10;
            chart.Series[0].Points.DataBindXY(dataAsset1.Where(x=>x.Portfolio_Value!=0).Select(x=>x.Date.ToShortDateString()).ToList(), dataAsset1.Where(x => x.Portfolio_Value != 0).Select(x => Math.Round(x.Portfolio_Value,2)).ToList());

            var returns = dataAsset1.Where(x => x.Portfolio_Value != 0).Select(x => x.Returns).ToList();
            var prix = dataAsset1.Where(x => x.Portfolio_Value != 0).ToList();
            Utility.calcul_returns(prix,true);
            var rendement= prix.Where(x => x.Portfolio_Value != 0).Select(x => x.Returns).ToList();
            var overall = (prix[prix.Count-1].Portfolio_Value / prix[0].Portfolio_Value) - 1 ;
            var Rendement_order = prix.OrderBy(x => x.Returns).ToList();
            var VaR = Rendement_order[Convert.ToInt32(Rendement_order.Count * 0.05)].Returns;
            textBox3.Text = Convert.ToString(Math.Round(VaR * 100, 2)) + " %";
            textBox6.Text = Convert.ToString(Math.Round(overall * 100, 2)) + " % "; 
            

        }

    }
}
