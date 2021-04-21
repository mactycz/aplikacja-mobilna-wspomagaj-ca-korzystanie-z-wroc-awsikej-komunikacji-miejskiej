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
    class Znajdz2
    {
       
        List<string> tymczasowe_wyniki = new List<string>();
        List<string> dojazdy = new List<string>();
        List<string> godziny = new List<string>();
        List<string> wyniki = new List<string>();
        List<string> godziny_doj = new List<string>();
        TimeSpan zero = new TimeSpan(0, 0, 0);
        List<string> ostatecznewyniki = new List<string>();

        List<string> tymczasowe_wyniki2 = new List<string>();
        List<string> przystankiwspolne = new List<string>();

        List<string> przystankiwspolne3 = new List<string>();
        List<string> przystankiwspolne4 = new List<string>();
        List<string> przystankiwspolne_id = new List<string>();
        List<string> przesiadkalinia2 = new List<string>();
        public List<string> szukajv2(string[,] stops, int stops_dlugosc, string[,] stop_times, int stop_times_dlugosc, string[,] trips, int trips_dlugosc, string odjazd, string dojazd, int poprawnyWariant, TimeSpan godzina,List<string> dojazd_id, List<string> odjazd_id)
        {
            
            
                string[,] stop_timeslocal = stop_times;
                string[,] stopslocal = stops;
                string[,] tripslocal = trips;

                List<string> opcje2 = new List<string>();
                List<TimeSpan> godzinaodjazdu = new List<TimeSpan>();
                List<string> wyniki_nazwa = new List<string>();
                List<string> przesiadka_nazwa = new List<string>();
                List<string> wyniki_id = new List<string>();
                List<string> przesiadka_id = new List<string>();
                List<string> dojazdy2 = new List<string>();
                List<string> godzinadojazdu = new List<string>();
                List<string> dojazdy_trip = new List<string>();

                TimeSpan limit = new TimeSpan(2, 00, 00);
                TimeSpan zero = new TimeSpan(0, 0, 0);
                TimeSpan godzinalini;
            for (int j = 0; j < dojazd_id.Count; j++)
                {
                    for (int i = 1; i < stop_times_dlugosc - 2; i++)
                    {

                        godzinalini = new TimeSpan(Int32.Parse(stop_timeslocal[i, 1].Substring(0, 2)), Int32.Parse(stop_timeslocal[i, 1].Substring(3, 2)), 00);
                        if (stop_times[i, 2] == dojazd_id[j] && godzinalini - godzina > zero && godzinalini - godzina < limit)
                        {
                            for (int z = 1; z < trips_dlugosc - 1; z++)
                            {
                                if (poprawnyWariant == Int32.Parse(tripslocal[z, 1]) && tripslocal[z, 2] == stop_timeslocal[i, 0])
                                {
                                    if (!tymczasowe_wyniki.Contains(stop_timeslocal[i, 0]))
                                    {
                                        tymczasowe_wyniki.Add(stop_timeslocal[i, 0]);//linia 2
                                        godziny_doj.Add(stop_timeslocal[i, 1]);//godzina dojazdu linii 2
                                    }

                                }
                            }
                        }
                    }
                }



                for (int j = 0; j < tymczasowe_wyniki.Count; j++)
                {

                    for (int i = 1; i < stop_times_dlugosc; i++)
                    {
                        if (tymczasowe_wyniki[j] == stop_timeslocal[i, 0])
                        {
                            if (new TimeSpan(Int32.Parse(godziny_doj[j].Substring(0, 2)), Int32.Parse(godziny_doj[j].Substring(3, 2)), 00) > new TimeSpan(Int32.Parse(stop_timeslocal[i, 1].Substring(0, 2)), Int32.Parse(stop_timeslocal[i, 1].Substring(3, 2)), 00))
                            {
                                if (!przesiadkalinia2.Contains(stop_timeslocal[i, 2]))
                                {
                                    przesiadkalinia2.Add(stop_timeslocal[i, 2]);//przesiadka od lini1
                                }
                            }

                        }
                    }
                }
                for (int i = 0; i < odjazd_id.Count; i++)
                {
                    for (int j = 1; j < stop_times_dlugosc - 2; j++)
                    {
                        godzinalini = new TimeSpan(Int32.Parse(stop_timeslocal[j, 1].Substring(0, 2)), Int32.Parse(stop_timeslocal[j, 1].Substring(3, 2)), 00);

                        if (odjazd_id[i] == stop_timeslocal[j, 2] && godzinalini - godzina > zero && godzinalini - godzina < limit)
                        {
                            for (int k = 1; k < trips_dlugosc - 1; k++)
                            {
                                if (poprawnyWariant == Int32.Parse(tripslocal[k, 1]) && trips[k, 2] == stop_timeslocal[j, 0])
                                {
                                    if (!dojazdy.Contains(stop_timeslocal[j, 0]))
                                    {
                                        godziny.Add(stop_timeslocal[j, 1]);//godzina odjazdu linii 1
                                        dojazdy.Add(stop_timeslocal[j, 0]);//linia 1

                                    }
                                }

                            }
                        }
                    }
                }

                for (int j = 0; j < dojazdy.Count; j++)
                {

                    for (int i = 1; i < stop_times_dlugosc; i++)
                    {
                        if (dojazdy[j] == stop_timeslocal[i, 0])
                        {
                            if (new TimeSpan(Int32.Parse(godziny[j].Substring(0, 2)), Int32.Parse(godziny[j].Substring(3, 2)), 00) < new TimeSpan(Int32.Parse(stop_timeslocal[i, 1].Substring(0, 2)), Int32.Parse(stop_timeslocal[i, 1].Substring(3, 2)), 00))
                            {
                                if (!wyniki.Contains(stop_timeslocal[i, 2]))
                                {
                                    wyniki.Add(stop_timeslocal[i, 2]);//przesiadka od lini1
                                }

                            }
                        }
                    }
                }

                for (int i = 0; i < wyniki.Count; i++)
                {
                    for (int j = 0; j < przesiadkalinia2.Count; j++)
                    {
                        for (int k = 1; k < stops_dlugosc - 1; k++)
                        {
                            if (przesiadkalinia2[j] == stopslocal[k, 0])
                            {
                                if (!przesiadka_nazwa.Contains(stopslocal[k, 1]))
                                {
                                    przesiadka_nazwa.Add(stopslocal[k, 1]);
                                    przesiadka_id.Add(stopslocal[k, 0]);
                                }

                            }
                            if (wyniki[i] == stopslocal[k, 0])
                            {
                                if (!wyniki_nazwa.Contains(stopslocal[k, 1]))
                                {
                                    wyniki_nazwa.Add(stopslocal[k, 1]);
                                    wyniki_id.Add(stopslocal[k, 0]);
                                }

                            }
                        }
                    }
                }


                for (int i = 0; i < przesiadka_nazwa.Count; i++)
                {
                    for (int j = 0; j < wyniki_nazwa.Count; j++)
                    {
                        if (przesiadka_nazwa[i] == wyniki_nazwa[j])
                        {
                            if (!przystankiwspolne.Contains(wyniki_nazwa[j]))
                            {
                                przystankiwspolne.Add(wyniki_nazwa[j]);
                            }
                        }
                    }
                }

                for (int i = 0; i < przystankiwspolne.Count; i++)
                {
                    for (int j = 0; j < stops_dlugosc - 1; j++)
                    {
                        if (przystankiwspolne[i] == stopslocal[j, 1])
                        {
                            if (!przystankiwspolne_id.Contains(stopslocal[j, 0]))
                            {
                                przystankiwspolne_id.Add(stopslocal[j, 0]);

                            }
                        }
                    }
                }



                int f = -1;


                for (int i = 1; i < stop_times_dlugosc; i++)
                {
                    for (int p = 1; p < tymczasowe_wyniki.Count; p++)
                    {
                        if (stop_timeslocal[i, 0] == tymczasowe_wyniki[p])
                        {
                            for (int j = 0; j < przystankiwspolne_id.Count; j++)
                            {
                                if (stop_timeslocal[i, 2] == przystankiwspolne_id[j])
                                {

                                    if (new TimeSpan(Int32.Parse(godziny_doj[p].Substring(0, 2)), Int32.Parse(godziny_doj[p].Substring(3, 2)), 00) > new TimeSpan(Int32.Parse(stop_timeslocal[i, 1].Substring(0, 2)), Int32.Parse(stop_timeslocal[i, 1].Substring(3, 2)), 00))
                                    {
                                        if (new TimeSpan(Int32.Parse(stop_timeslocal[i, 1].Substring(0, 2)), Int32.Parse(stop_timeslocal[i, 1].Substring(3, 2)), 00) > godzina)
                                        {




                                            for (int l = 1; l < trips_dlugosc; l++)
                                            {
                                                if (tymczasowe_wyniki[p] == tripslocal[l, 2])
                                                {
                                                    tymczasowe_wyniki2.Add(tripslocal[l, 0] + "(" + tripslocal[l, 3] + ")");
                                                    f++;
                                                }
                                            }
                                            for (int k = 1; k < stops_dlugosc - 1; k++)
                                            {
                                                if (przystankiwspolne_id[j] == stopslocal[k, 0])
                                                {
                                                    przystankiwspolne3.Add(stopslocal[k, 1]);
                                                }
                                            }

                                            opcje2.Add(stop_timeslocal[i, 1] + "  -> " + tymczasowe_wyniki2[f] + "  " + " -> " + dojazd + " dojedzie o " + godziny_doj[p]);
                                            godzinaodjazdu.Add(new TimeSpan(Int32.Parse(stop_times[i, 1].Substring(0, 2)), Int32.Parse(stop_times[i, 1].Substring(3, 2)), 00));
                                            godzinadojazdu.Add(godziny_doj[p]);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }

                int g = -1;
                tymczasowe_wyniki.Clear();
                tymczasowe_wyniki2.Clear();

                for (int i = 1; i < stop_times_dlugosc; i++)
                {
                    for (int k = 0; k < dojazdy.Count; k++)
                    {
                        if (stop_timeslocal[i, 0] == dojazdy[k])
                        {
                            for (int m = 0; m < przystankiwspolne_id.Count; m++)
                            {

                                if (stop_timeslocal[i, 2] == przystankiwspolne_id[m])
                                {
                                    for (int j = 0; j < opcje2.Count; j++)
                                    {
                                        if (godzina < godzinaodjazdu[j])
                                        {
                                            for (int l = 1; l < trips_dlugosc; l++)
                                            {
                                                if (dojazdy[k] == tripslocal[l, 2])
                                                {
                                                    dojazdy2.Add(tripslocal[l, 0] + "(" + tripslocal[l, 3] + ")");
                                                    dojazdy_trip.Add(tripslocal[l, 2]);

                                                    g++;
                                                }
                                            }
                                            for (int r = 1; r < stops_dlugosc - 1; r++)
                                            {
                                                if (przystankiwspolne_id[m] == stopslocal[r, 0])
                                                {
                                                    przystankiwspolne4.Add(stopslocal[r, 1]);
                                                }
                                            }
                                            if (przystankiwspolne4[g] == przystankiwspolne3[j])
                                            {

                                                if (new TimeSpan(Int32.Parse(godziny[k].Substring(0, 2)), Int32.Parse(godziny[k].Substring(3, 2)), 00) < new TimeSpan(Int32.Parse(opcje2[j].Substring(0, 2)), Int32.Parse(opcje2[j].Substring(3, 2)), 00))
                                                {
                                                    if (new TimeSpan(Int32.Parse(stop_timeslocal[i, 1].Substring(0, 2)), Int32.Parse(stop_timeslocal[i, 1].Substring(3, 2)), 00) < new TimeSpan(Int32.Parse(opcje2[j].Substring(0, 2)), Int32.Parse(opcje2[j].Substring(3, 2)), 00))
                                                    {
                                                        if (new TimeSpan(Int32.Parse(stop_timeslocal[i, 1].Substring(0, 2)), Int32.Parse(stop_timeslocal[i, 1].Substring(3, 2)), 00) > new TimeSpan(Int32.Parse(godziny[k].Substring(0, 2)), Int32.Parse(godziny[k].Substring(3, 2)), 00))
                                                        {

                                                            ostatecznewyniki.Add("dojazd do " + dojazd + " o  " + godzinadojazdu[j] + " | " + odjazd + " o godzinie: " + godziny[k] + "  " + " -> \n" + dojazdy2[g] + "dojedzie o " + stop_times[i, 1] + " na  " + przystankiwspolne4[g] + " -> " + "\n" + opcje2[j]);

                                                        }
                                                    }
                                                }

                                            }

                                        }

                                    }

                                }
                            }
                        }

                    }
                    
                }
            ostatecznewyniki.Sort();
            for (int i = 0; i < ostatecznewyniki.Count; i++)
            {
                if (i > 0 && new TimeSpan(Int32.Parse(ostatecznewyniki[i].Substring(ostatecznewyniki[i].IndexOf("nie: ") + 5, 2)), Int32.Parse(ostatecznewyniki[i].Substring(ostatecznewyniki[i].IndexOf("nie: ") + 8, 2)), 00) <= new TimeSpan(Int32.Parse(ostatecznewyniki[i - 1].Substring(ostatecznewyniki[i - 1].IndexOf("nie: ") + 5, 2)), Int32.Parse(ostatecznewyniki[i - 1].Substring(ostatecznewyniki[i - 1].IndexOf("nie: ") + 8, 2)), 00))
                {
                    ostatecznewyniki.RemoveAt(i);

                    i = 0;
                }
            }


            for (int i = 0; i < ostatecznewyniki.Count; i++)
            {


                if (i > 0 && ostatecznewyniki[i].Substring(0, 43 + dojazd.Length) == ostatecznewyniki[i - 1].Substring(0, 43 + dojazd.Length))
                {

                    ostatecznewyniki.RemoveAt(i);

                    i = 0;
                }
            }


            
            
            return ostatecznewyniki;
            }
    }
}