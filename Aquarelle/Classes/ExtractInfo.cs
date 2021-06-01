using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Aquarelle.Classes
{
    public static class ExtractInfo
    {
        private static readonly string pathToExtractedInfoFile = PATHS.EXTRACTED_INFO_FILE_PATH;

        public static void StartExtraction(string filePath)
        {
            PdfReader reader;
            int pageNum;
            string text;
            string[] lines;
            string currentLine;
            List<string> lstExtractedInfo;

            try
            {
                lstExtractedInfo = new List<string>();

                if (!String.IsNullOrEmpty(filePath))
                {
                    //save the pdf name
                    lstExtractedInfo.Add("FileName:"+filePath.Substring(filePath.LastIndexOf(@"\") + 1));

                    // initialise the pdf reader
                    reader = new PdfReader(filePath);
                    pageNum = reader.NumberOfPages;

                    // Start the extraction
                    for (int i = 1; i <= pageNum; i++)
                    {
                        text = PdfTextExtractor.GetTextFromPage(reader, i, new LocationTextExtractionStrategy());

                        lines = text.Split('\n');
                        for (int j = 0, len = lines.Length; j < len; j++)
                        {
                            currentLine = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(lines[j]));

                            if (currentLine.Contains("Date:"))
                                lstExtractedInfo.Add(currentLine);

                            if (currentLine.Contains("Customer Number:"))
                                lstExtractedInfo.Add(currentLine);

                            if (currentLine.Contains("Family name:"))
                                lstExtractedInfo.Add(currentLine);

                            if(currentLine == "Account Number Account Creation Date ")
                            {
                                lstExtractedInfo.Add(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(lines[j + 1])));
                                lstExtractedInfo.Add(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(lines[j + 2])));
                            }

                        }
                    }

                    string firstPattern = "", secondPattern = "", thirdPattern = "";

                    string[] accountOneDetails = lstExtractedInfo.ElementAt<string>(lstExtractedInfo.Count - 2).Split(' ');
                    string[] accountTwoDetails = lstExtractedInfo.ElementAt<string>(lstExtractedInfo.Count - 1).Split(' ');

                    string pdfName = lstExtractedInfo.Find(extractedInfo => extractedInfo.Contains("FileName"));
                    string date = lstExtractedInfo.Find(extractedInfo => extractedInfo.Contains("Date"));
                    string custNumber = lstExtractedInfo.Find(extractedInfo => extractedInfo.Contains("Customer Number"));
                    string familyName = lstExtractedInfo.Find(extractedInfo => extractedInfo.Contains("Family name"));
                    
                    firstPattern += pdfName.Substring(pdfName.LastIndexOf(":") + 1) + " |";
                    firstPattern += date.Substring(date.LastIndexOf(":") + 1) + " |";
                    firstPattern += custNumber.Substring(custNumber.LastIndexOf(":") + 1) + " |";
                    firstPattern += familyName.Substring(familyName.LastIndexOf(":") + 1);
                    secondPattern = accountOneDetails[0] + " | " + accountOneDetails[1];
                    thirdPattern =  accountTwoDetails[0] + " | " + accountTwoDetails[1];

                    string[] formattedExtractedInfo = { firstPattern, secondPattern, thirdPattern };
                    SaveExtractedInfo(formattedExtractedInfo);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private static void SaveExtractedInfo(string[] lines)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(pathToExtractedInfoFile, true))
                {
                    foreach (string line in lines)
                        outputFile.WriteLine(line);
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}