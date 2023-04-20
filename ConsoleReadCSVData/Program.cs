using System;
using System.Collections.Generic;

namespace ConsoleReadCSVData
{
    class Program
    {
        static void Main(string[] args)
        {
            string strFileName = "./foreign_net_buy.csv";


            CSVToJson oReadCSVDataAll = new CSVToJson(strFileName);
            string sJSONText = oReadCSVDataAll.GetJson();

            Console.WriteLine(sJSONText);

            /*{
                "dateData": "2023-04-10T10:36:20.690Z",
                "tickerCode": "string",
                "typeFlow": 0,
                "volumeTotal": 0,
                "valueTotal": 0,
                "volumeBuy": 0,
                "volumeSell": 0,
                "netRatioVolume": 0,
                "dominationRatio": 0
              }*/

            List<string> inputCols = new List<string> { "Code", "Volume", "Value", "Frg Buy", "Frg Sell" };
            List<string> outputCols = new List<string> { "tickerCode", "volumeTotal", "valueTotal", "volumeBuy", "volumeSell" };

            CSVToJson oReadCSVDataByColumn = new CSVToJson(strFileName, inputCols, outputCols);
            string sJSONText2 = oReadCSVDataByColumn.GetJson();

            Console.WriteLine(sJSONText2);

        }

    }
}
