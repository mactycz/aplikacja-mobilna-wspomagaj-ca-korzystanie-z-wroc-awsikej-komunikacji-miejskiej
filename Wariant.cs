using Android.App;
using Android.Content;

using Android.OS;
using Android.Support.V7.App;
using Android.Text.Format;
using Android.Util;
using Android.Views.InputMethods;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;


namespace praca_inz_mobilna
{
    class Wariant
    {
        public int liczeniewariantu(AdapterView.ItemClickEventArgs e,string[,] calendar,int calendar_length,int poprawnywariant)
        {
            
            for (int v = 1; v < calendar_length - 1; v++)
            {

                if (Int32.Parse(calendar[v, e.Position + 1]) == 1)
                {

                    poprawnywariant = Int32.Parse(calendar[v, 0]);
                }

            }
            return poprawnywariant;
        }
       
    }
}