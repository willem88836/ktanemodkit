﻿using System.CodeDom.Compiler;
using UnityEngine;

namespace WillemMeijer.NMAirTrafficControl
{
    public class AirTrafficUtilities
	{
        private static string GeneratePlaneSerial(MonoRandom mRandom)
        {
            string serial = "";

            int q1 = mRandom.Next(0, 9);
            serial += q1.ToString();

            int q2 = mRandom.Next(65, 90);
            serial += (char)q2;

            double a = mRandom.NextDouble();
            if (a < 0.5f)
            {
                serial += "-";
            }

            int q3 = mRandom.Next(0, 9);
            serial += q3.ToString();

            if (a >= 0.5f)
            {
                serial += "-";
            }

            int q4 = mRandom.Next(65, 90);
            serial += (char)q4;

            int q5 = mRandom.Next(0, 9);
            serial += q5.ToString();

            return serial;
        }

        private static string GenerateGenericSerial(MonoRandom mRandom)
        {
            string serial = "";

            int q1 = mRandom.Next(65, 90);
            serial += (char)q1;

            int q2 = mRandom.Next(65, 90);
            serial += (char)q2;

            double a = mRandom.NextDouble();
            if (a < 0.5f)
            {
                serial += "-";
            }

            int q3 = mRandom.Next(0, 9);
            serial += q3.ToString();

            if (a >= 0.5f)
            {
                serial += "-";
            }

            int q4 = mRandom.Next(0, 9);
            serial += q4.ToString();

            int q5 = mRandom.Next(0, 9);
            serial += q5.ToString();

            return serial;
        }


        public static void GeneratePlaneSerials(int n, MonoRandom mRandom)
        {
            string[] serials = new string[n];

            for (int i = 0; i < n; i++)
            {
                string serial = GeneratePlaneSerial(mRandom);
                serials[i] = serial;
            }

            AirTrafficControlData.PlaneSerials = serials;
        }

        public static void GenerateOrigins(int n, MonoRandom mRandom)
        {
            string[] allCities = AirTrafficControlData.AllCities;
            string[] originCities = new string[n];
            
            for (int i = 0; i < n; i++)
            {
                string option = allCities[mRandom.Next(0, allCities.Length)];
                originCities[i] = option;
            }

            AirTrafficControlData.OriginCities = originCities;
        }
        
        public static void GenerateShuttleSerials(int n, MonoRandom mRandom)
        {
            string[] serials = new string[n];

            for (int i = 0; i < n; i++)
            {
                string serial = GenerateGenericSerial(mRandom);
                serials[i] = serial;
            }

            AirTrafficControlData.ShuttleSerials = serials;
        }

        public static void GenerateLuggageSerials(int n, MonoRandom mRandom)
        {
            string[] serials = new string[n];

            for (int i = 0; i < n; i++)
            {
                string serial = GenerateGenericSerial(mRandom);
                serials[i] = serial;
            }

            AirTrafficControlData.LuggageSerials = serials;
        }

        public static void GenerateHangars(int n, MonoRandom mRandom)
        {
            string[] hangars = new string[n];

            for (int i = 1; i <= n; i++)
            {
                hangars[i] = "Hangar " + i;
            }

            AirTrafficControlData.Hangars = hangars;
        }

        public static void GenerateCrossTable(MonoRandom mRandom)
        {
            int rowsCount = AirTrafficControlData.PlaneSerials.Length;
            int columnCount = AirTrafficControlData.OriginCities.Length;
            int hangarCount = AirTrafficControlData.Hangars.Length;

            int[,] crossTable = new int[rowsCount, columnCount];

            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    int k = mRandom.Next(0, hangarCount);
                    crossTable[i, j] = k;
                }
            }

            AirTrafficControlData.OriginSerialCrossTable = crossTable;
        }



        public static void PrintPlaneSerials()
        {
            string cs = "{\n";
            for (int i = 0; i < AirTrafficControlData.PlaneSerials.Length; i++)
            {
                cs += "\"" + AirTrafficControlData.PlaneSerials[i] + "\"";
                if (i < AirTrafficControlData.PlaneSerials.Length - 1)
                {
                    cs += ",\n";
                }
            }
            cs += "};";
            Debug.Log(cs);
        }

        public static void PrintOrigins()
        {
            string cs = "{\n";
            for (int i = 0; i < AirTrafficControlData.OriginCities.Length; i++)
            {
                cs += "\"" + AirTrafficControlData.OriginCities[i] + "\"";
                if (i < AirTrafficControlData.OriginCities.Length - 1)
                {
                    cs += ",\n";
                }
            }
            cs += "};";
            Debug.Log(cs);
        }

        public static void PrintShuttles()
        {
            string s = "";
            foreach(string shuttle in AirTrafficControlData.ShuttleSerials)
            {
                s += "\"";
                s += shuttle;
                s += "\",\n";
            }
            Debug.Log(s);
        }

        public static void PrintLuggage()
        {
            string l = "";
            foreach (string luggage in AirTrafficControlData.LuggageSerials)
            {
                l += "\"";
                l += luggage;
                l += "\",\n";
            }
            Debug.Log(l);
        }

        public static void PrintCrosstableAsCS()
        {
            int[,] data = AirTrafficControlData.OriginSerialCrossTable;

            string cs = "{\n";

            for (int i = 0; i < data.GetLength(0); i++)
            {
                cs += "{";
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    cs += data[i, j] ;
                    if (j < data.GetLength(1) - 1)
                    {
                        cs += ",";
                    }
                }
                cs += "}";
                if (i < data.GetLength(0) - 1)
                {
                    cs += ",\n";
                }
            }
            cs += "};";

            Debug.Log(cs);
        }

        public static void PrintCrossTableAsHTML()
        {
            int[,] data = AirTrafficControlData.OriginSerialCrossTable;

            string html = "                        <tr class=\"Serial-Origin-Labels\"><th></th>";
            
            // labels.
            for (int k = 0; k < data.GetLength(1); k++)
            {
                html += "<th><div>"
                    + AirTrafficControlData.OriginCities[k] 
                    + "</div></th>";
            }
            html += "</tr>\n";
            for(int i = 0; i < data.GetLength(0); i++)
            {
                html += "                        <tr>";
                html += "<th>" + AirTrafficControlData.PlaneSerials[i] + "</th>";
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    html += "<th>" + (data[i, j] + 1) + "</th>";
                }
                html += "</tr>\n";
            }

            Debug.Log(html);
        }
    }
}