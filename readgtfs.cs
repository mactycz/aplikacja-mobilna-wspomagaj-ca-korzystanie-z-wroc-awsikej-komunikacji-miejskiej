using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace praca_inz_mobilna
{
    
    class readgtfs
    {
        TimeSpan godzina = new TimeSpan();
        public T[,] ResizeArray<T>(T[,] original, int[] indices)
        {
            var newArray = new T[original.GetLength(0), indices.Length];
            int minRows = original.GetLength(0);
            int minCols = Math.Min(indices.Length, original.GetLength(1));
            for (int i = 0; i < minRows; i++)
            {
                for (int j = 0; j < minCols; j++)
                    newArray[i, j] = original[i, indices[j]];
            }
            return newArray;
        }

        public string[,] ReadFile(string name, int lineCount, int columns)
        {
            var stream = Android.App.Application.Context.Assets.Open(name + ".txt");

            StreamReader sr = new StreamReader(stream);
            string input = sr.ReadToEnd();

            string[,] wynik = new string[lineCount, columns];
            int i = 0, j = 0;

            foreach (var row in input.Split('\n'))
            {
                j = 0;
                foreach (var col in row.Trim().Split(','))
                {
                    wynik[i, j] = col.Trim();
                    j++;
                }
                i++;
            }

            sr.Close();
            return wynik;


        }

       
    }
}