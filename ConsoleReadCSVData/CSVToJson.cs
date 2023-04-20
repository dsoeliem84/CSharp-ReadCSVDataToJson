using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;


namespace ConsoleReadCSVData
{
    class CSVToJson
    {
        private string sJSONText = string.Empty;


        /// <summary>
        ///  Return json data for all columns
        /// </summary>
        /// <param name="fileName">CSV File (.csv)</param>
        public CSVToJson(string fileName)
        {
            var csv = new List<string[]>();
            bool bIsFirstIndex = true;
            string[] rowText;
            DataTable dt = new DataTable();

            try
            {
                var lines = System.IO.File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    //get text per row
                    rowText = line.ToString().Replace("\",", "|").Replace("\"", "").Split('|');

                    if (bIsFirstIndex)
                    {
                        //add column on data table
                        for (int i = 0; i < rowText.Length; i++)
                            dt.Columns.Add(rowText[i].ToString().Trim().Replace("\"", ""), typeof(string));
                        bIsFirstIndex = false;

                    }
                    else
                    {
                        //add rows on data table
                        DataRow dr = dt.NewRow();

                        for (int i = 0; i < rowText.Length; i++)
                        {
                            dr[i] = rowText[i].ToString().Trim().Replace("\"", "");
                        }

                        dt.Rows.Add(dr);

                    }
                }

                if (dt.Rows.Count > 0)
                    this.sJSONText = JsonConvert.SerializeObject(dt);

            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }            
        }


        /// <summary>
        ///  Return json data for for selected column [columnInput] and the output header will based on [columnOutput]
        /// </summary>
        /// <param name="fileName">CSV File (.csv)</param>
        /// <param name="columnsInput">List<string>Selected column(s)</param>
        /// <param name="columnOutput">List<string>Column(s) header for the output JSON data</param>
        public CSVToJson(string fileName, List<string> columnsInput, List<string> columnOutput)
        {

            if (columnsInput.Count != columnOutput.Count)
                throw new ArgumentException("Constructor Parameters are Invalid: ColumnsInput total index is not equal with columnOutput total index ");


            var csv = new List<string[]>();
            bool bIsFirstIndex = true;
            string[] rowText;
            DataTable dt = new DataTable();
            List<int> SelectedColumnIndex = new List<int>();
            string sColumnName = string.Empty;
            int iIndexColumnOutput = 0;

            try
            {
                var lines = System.IO.File.ReadAllLines(fileName);
                foreach (string line in lines)
                {
                    //get text per row
                    rowText = line.ToString().Replace("\",", "|").Replace("\"", "").Split('|');

                    if (bIsFirstIndex)
                    {
                        //add column on data table
                        for (int i = 0; i < rowText.Length; i++) {
                            sColumnName = rowText[i].ToString().Trim().Replace("\"", "");

                            if (columnsInput.Contains(sColumnName))
                            {
                                SelectedColumnIndex.Add(i);
                                dt.Columns.Add(columnOutput[iIndexColumnOutput], typeof(string));
                                iIndexColumnOutput += 1;
                            }
                        }
                        bIsFirstIndex = false;

                    }
                    else
                    {
                        if (SelectedColumnIndex.Count > 0) {
                            //add rows on data table
                            DataRow dr = dt.NewRow();
                            int incrementIdx = 0;

                            foreach (int ColIndex in SelectedColumnIndex) {
                                dr[incrementIdx] = rowText[ColIndex].ToString().Trim().Replace("\"", "");
                                incrementIdx += 1;
                            }

                            dt.Rows.Add(dr);
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                    this.sJSONText = JsonConvert.SerializeObject(dt);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

        }

        /// <summary>
        ///  Return JSON data
        /// </summary>
        public string GetJson()
        {
            return this.sJSONText;
        }
    }
}
