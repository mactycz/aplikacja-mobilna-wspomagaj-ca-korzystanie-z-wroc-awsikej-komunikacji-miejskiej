using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace praca_inz_mobilna
{
    class Znajdz1
    {
        List<string> tymczasowe_wyniki = new List<string>();
        List<string> dojazdy = new List<string>();
        List<string> godziny = new List<string>();
        List<string> wyniki = new List<string>();
        List<string> godziny_doj = new List<string>();
        TimeSpan zero = new TimeSpan(0, 0, 0);
        public List<string> szukajv1(string[,]stops,int stops_dlugosc,string [,]stop_times,int stop_times_dlugosc,string[,] trips,int trips_dlugosc,string odjazd,string dojazd,int poprawnyWariant,TimeSpan godzina, List<string> dojazd_id , List<string> odjazd_id)
        {
            
            for (int j = 0; j < dojazd_id.Count; j++)
            {
                for (int i = 1; i < stop_times_dlugosc; i++)
                {


                    if (stop_times[i, 2] == dojazd_id[j])
                    {
                        if (!tymczasowe_wyniki.Contains(stop_times[i, 0]))
                        {
                            tymczasowe_wyniki.Add(stop_times[i, 0]);
                            dojazdy.Add(stop_times[i, 1]);
                        }
                    }

                }
            }
            for (int i = 1; i < stop_times_dlugosc; i++)
            {
                for (int j = 0; j < odjazd_id.Count; j++)
                {

                    if (stop_times[i, 2] == odjazd_id[j])
                    {
                        for (int k = 0; k < tymczasowe_wyniki.Count; k++)
                        {
                            TimeSpan odj = new TimeSpan(Int32.Parse(stop_times[i, 1].Substring(0, 2)), Int32.Parse(stop_times[i, 1].Substring(3, 2)), 00);
                            TimeSpan doj = new TimeSpan(Int32.Parse(dojazdy[k].Substring(0, 2)), Int32.Parse(dojazdy[k].Substring(3, 2)), 00);
                            if (tymczasowe_wyniki[k] == stop_times[i, 0] && odj < doj)
                            {
                                wyniki.Add(stop_times[i, 0]);
                                godziny.Add(stop_times[i, 1]);
                                godziny_doj.Add(dojazdy[k]);

                            }
                        }
                    }

                }
            }
            tymczasowe_wyniki.Clear();
            for (int j = 1; j < trips_dlugosc; j++)
            {
                for (int k = 0; k < wyniki.Count; k++)
                {
                    if (trips[j, 2] == wyniki[k] && poprawnyWariant == Int32.Parse(trips[j, 1]))
                    {
                        tymczasowe_wyniki.Add(godziny[k] + "  " + trips[j, 0] + "  " + trips[j, 3] + "dojazd o " + godziny_doj[k]);
                    }
                }
            }
            wyniki.Clear();

            for (int k = 0; k < tymczasowe_wyniki.Count; k++)
            {
                TimeSpan time = new TimeSpan(Int32.Parse(tymczasowe_wyniki[k].Substring(0, 2)), Int32.Parse(tymczasowe_wyniki[k].Substring(3, 2)), 00);

                if (time - godzina > zero)
                {
                    wyniki.Add("odjazd za: " + (time - godzina).ToString() + "   " + tymczasowe_wyniki[k]);
                }


            }

            wyniki.Sort();
            return wyniki;
        }

    }
}