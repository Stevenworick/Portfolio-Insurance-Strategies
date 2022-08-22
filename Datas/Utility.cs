using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeOpenXml;
using System.IO;
using MathNet.Numerics.Distributions;

namespace ProjetAssurancePortefeuille
{
    class Utility
    {
        public static List<Data> GetDatas(string period)
        {
            List<Data> list_data = new List<Data>();

            string pathbis = "C:/Users/steve/Downloads/DataBloom.xlsx";

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            FileInfo fileInfo = new FileInfo(pathbis);
            ExcelPackage package = new ExcelPackage(fileInfo);
            ExcelWorksheet worksheet = package.Workbook.Worksheets[period];
            for (int i = 7; i < worksheet.Dimension.Rows; i++)
            {
                list_data.Add(new Data { Date = Convert.ToDateTime(worksheet.Cells[i, 1].Value), Price = Convert.ToDecimal(worksheet.Cells[i, 2].Value), Volatility = worksheet.Cells[i, 3].Value.ToString(), Asset = "1" });
                list_data.Add(new Data { Date = Convert.ToDateTime(worksheet.Cells[i, 1].Value), Price = Convert.ToDecimal(worksheet.Cells[i, 8].Value), Volatility = worksheet.Cells[i, 9].Value.ToString(), Asset = "2" });
            }
            return list_data;
        }

        public static void calcul_returns(List<Data> liste, bool IsPortfolio = false)
        {
            var listorder = liste.OrderBy(x => x.Date).ToList();
            
            for (int j = 1; j < listorder.Count(); j++)
            {   
                if(IsPortfolio)
                {
                    listorder[j].Returns = Convert.ToDecimal((listorder[j].Portfolio_Value - listorder[j - 1].Portfolio_Value) / listorder[j - 1].Portfolio_Value);
                }

                else
                { listorder[j].Returns = (listorder[j].Price - listorder[j - 1].Price) / listorder[j - 1].Price;}
            }
        }


        // public static List<double> gbm(double risky_init ,  int n_years = 10, int n_scenarios = 1000, double mu = 0.07, double sigma = 0.15, int steps_per_year = 12, double s_0 = 100.0, bool prices = true)
        //{
        //    /*
        //    Evolution of Geometric Brownian Motion trajectories, such as for Stock Prices through Monte Carlo
        //    :param n_years:  The number of years to generate data for
        //    :param n_paths: The number of scenarios/trajectories
        //    :param mu: Annualized Drift, e.g.Market Return
        //    :param sigma: Annualized Volatility
        //    :param steps_per_year: granularity of the simulation
        //    :param s_0: initial value
        //    :return: a numpy array of n_paths columns and n_years*steps_per_year rows
        //    */

        //    // Derive per-step Model Parameters from User Specifications
        //    double dt = 1 / steps_per_year;
        //    int n_steps = (n_years * steps_per_year) + 1;


        //    // the standard way ...
        //    // rets_plus_1 = np.random.normal(loc=mu*dt+1, scale=sigma*np.sqrt(dt), size=(n_steps, n_scenarios))
        //    // without discretization error ...


        //    double mean = Math.Pow(1 + mu, dt);
        //    double stdDev = (sigma * Math.Sqrt(dt));
        //    List<double> rets_plus_1 = new List<double>();
        //    Normal normalDist = new Normal(mean, stdDev);
        //    for (int i = 0; i < n_steps * n_scenarios; i++)
        //    {

        //        double randomGaussianValue = normalDist.Sample();
        //        rets_plus_1.Add(randomGaussianValue);
        //    }

        //    rets_plus_1[0] = risky_init;
        //    List<double> cumprod = new List<double>();
        //    cumprod.Add(rets_plus_1[0]);
        //    for (int k = 1; k < rets_plus_1.Count + 1; k++)
        //    {
        //        cumprod.Add(cumprod[k - 1] * rets_plus_1[k]);
        //    }
        //    List<double> ret_val = new List<double>();
        //    if (prices)
        //    {
        //        for (int j = 0; j < cumprod.Count(); j++)
        //        {
        //            ret_val.Add(s_0 * cumprod[j]) ;
        //        }

        //    }
        //    else
        //    {
        //        for (int jackson = 0; jackson < rets_plus_1.Count(); jackson++)
        //        {
        //            rets_plus_1.Add( rets_plus_1[jackson] - 1);
        //        }

        //        ret_val = rets_plus_1;
        //    }

        //    return ret_val;

        //}

    }
}
