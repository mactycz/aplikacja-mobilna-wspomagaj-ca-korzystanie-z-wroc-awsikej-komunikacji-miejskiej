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
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button wybierzlinie;
        private Button wybierzprzystanek;
        private Button znajdznajlepsze;
        private Button ok;
        private Button powrot;
        private Button powrotdowynikow;

        private TextView szczegoly;
        private TextView wybrane;
        private TextView wynik;
        private Button wybierzgodzine;

        ListView lista_linii;
        ListView lista_dni;
        ListView lista_kierunkow;
        ListView lista_wynikow1;
        ListView lista_przystankow;
        List<string> odjazd_id = new List<string>();
        List<string> dojazd_id = new List<string>();

        public int i = 0;
        public int j = 0;
        public int lineCount;
        int poprawnywariant;
        string[] dzien = new string[7];
        string[,] trips;
        string[,] calendar;
        string[,] stop_times;
        string[,] stops;
        int trips_dlugosc;
        int calendar_dlugosc;
        int stop_times_dlugosc;
        int stops_dlugosc;
        string linia;
        string kierunek;
        string przystanek;
        string odjazd;
        string dojazd;
        TimeSpan godzina;


        List<string> kierunki = new List<string>();
        List<string> linie = new List<string>();
        List<string> tymczasowe_wyniki = new List<string>();
        List<string> przystanki = new List<string>();
        List<string> wyniki = new List<string>();

        List<string> dojazdy = new List<string>();
        List<string> wyniki_godzin = new List<string>();
        List<string> godziny_doj = new List<string>();
        List<string> ostatecznewyniki = new List<string>();


        Wariant day = new Wariant();


        protected override void OnCreate(Bundle savedInstanceState)
        {
            Android.Content.Res.AssetManager assets = this.Assets;
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            readgtfs start = new readgtfs();
            trips = start.ReadFile("trips", 37876, 9);
            trips_dlugosc = trips.GetLength(0);
            trips = start.ResizeArray<string>(trips, new int[] { 0, 1, 2, 3 });


            calendar = start.ReadFile("calendar", 6, 10);
            calendar_dlugosc = calendar.GetLength(0);
            calendar = start.ResizeArray<string>(calendar, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });


            stop_times = start.ReadFile("stop_times", 936984, 7);
            stop_times_dlugosc = stop_times.GetLength(0);
            stop_times = start.ResizeArray<string>(stop_times, new int[] { 0, 1, 3 });

            stops = start.ReadFile("stops", 2161, 5);
            stops_dlugosc = stops.GetLength(0);
            stops = start.ResizeArray<string>(stops, new int[] { 0, 2 });


            dzien[0] = "Poniedziałek";
            dzien[1] = "Wtorek";
            dzien[2] = "Środa";
            dzien[3] = "Czwartek";
            dzien[4] = "Piątek";
            dzien[5] = "Sobota";
            dzien[6] = "Niedziela";

            godzina = DateTime.Now.TimeOfDay;

            godzina = new TimeSpan(godzina.Hours, godzina.Minutes, 00);

            wybierzlinie = FindViewById<Button>(Resource.Id.linia);
            wybierzlinie.Click += Wybierzlinie_Click1;

            wybierzprzystanek = FindViewById<Button>(Resource.Id.przystanek);
            wybierzprzystanek.Click += Wybierzprzystanek_Click2;

            znajdznajlepsze = FindViewById<Button>(Resource.Id.znajdz);
            znajdznajlepsze.Click += Znajdznajlepsze_Click3;


        }
        private void Wybierzgodzine_Click(object sender, EventArgs e)
        {
            WybierzGodzine frag = WybierzGodzine.NewInstance(
                delegate (DateTime time)
                {
                    godzina = time.TimeOfDay;
                });

            frag.Show(FragmentManager, WybierzGodzine.TAG);
        }

        private void Wybierzlinie_Click1(object sender, System.EventArgs e)
        {



            SetContentView(Resource.Layout.wybor_linii);
            for (int i = 1; i < trips_dlugosc; i++)
            {
                if (i > 1 && trips[i, 0] != trips[i - 1, 0])
                {
                    linie.Add(trips[i, 0]);
                }
                else if (i == 1)
                {
                    linie.Add(trips[i, 0]);
                }
            }




            lista_linii = (ListView)FindViewById<ListView>(Resource.Id.mainlistview);
            lista_linii.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, linie);

            ok = FindViewById<Button>(Resource.Id.btnOk);
            ok.Click += ok_Click1;
            powrot = FindViewById<Button>(Resource.Id.btnCancel);
            powrot.Click += Powrot_click;
        }
        private void ok_Click1(object sender, System.EventArgs e)
        {


            var editinput = FindViewById<EditText>(Resource.Id.input);


            if (editinput.Text.Length < 1)
            {
                SetContentView(Resource.Layout.activity_main);
                Powrot_click(sender, e);

            }
            else
            {
                linia = editinput.Text.ToString().ToUpper();

                InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
                inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);

                SetContentView(Resource.Layout.dzien_tygodnia);

                lista_dni = (ListView)FindViewById<ListView>(Resource.Id.tydzien);
                lista_dni.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, dzien);

                powrot = FindViewById<Button>(Resource.Id.powrot);
                powrot.Click += Powrot_click;




                lista_dni.ItemClick += lista_dni_click1;
            }

        }
        private void lista_dni_click1(object sender, AdapterView.ItemClickEventArgs e)
        {

            SetContentView(Resource.Layout.kierunki);

            poprawnywariant = day.liczeniewariantu(e, calendar, calendar_dlugosc, poprawnywariant);


            for (int p = 0; p < trips_dlugosc; p++)
            {

                if (linia == trips[p, 0].ToLower() || linia == trips[p, 0])
                {
                    if (Int32.Parse(trips[p, 1]) == poprawnywariant)
                    {

                        if (!kierunki.Contains(trips[p, 3]))
                        {
                            kierunki.Add(trips[p, 3]);
                        }

                    }
                }
            }


            powrot = FindViewById<Button>(Resource.Id.powrot2);
            powrot.Click += Powrot_click;


            lista_kierunkow = (ListView)FindViewById<ListView>(Resource.Id.kierunki);

            lista_kierunkow.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, kierunki);
            lista_kierunkow.ItemClick += Lista_kierunkow_ItemClick1;

        }
        private void Lista_kierunkow_ItemClick1(object sender, AdapterView.ItemClickEventArgs e)
        {
            SetContentView(Resource.Layout.przystanki1);

            kierunek = kierunki[e.Position];

            for (int p = 0; p < trips_dlugosc; p++)
            {

                if (linia == trips[p, 0].ToLower() || linia == trips[p, 0])
                {
                    if (trips[p, 3] == kierunek || trips[p, 3].ToLower() == kierunek)
                    {



                        for (int w = 1; w < stop_times_dlugosc; w++)
                        {
                            if (trips[p, 2] == stop_times[w, 0])
                            {
                                for (int r = 1; r < stops_dlugosc; r++)
                                {
                                    if (stop_times[w, 2] == stops[r, 0])
                                    {
                                        if (!przystanki.Contains(stops[r, 1]))
                                        {
                                            przystanki.Add(stops[r, 1]);
                                        }

                                    }
                                }
                            }
                        }

                    }

                }
            }

            lista_przystankow = (ListView)FindViewById<ListView>(Resource.Id.przystanki_list);
            lista_przystankow.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, przystanki);

            lista_przystankow.ItemClick += Lista_przystankow_ItemClick1;

            powrot = FindViewById<Button>(Resource.Id.powrot4);
            powrot.Click += Powrot_click;

        }

        private void Lista_przystankow_ItemClick1(object sender, AdapterView.ItemClickEventArgs e)
        {
            SetContentView(Resource.Layout.wyniki_opcji1);

            przystanek = przystanki[e.Position];
            for (int i = 0; i < trips_dlugosc; i++)
            {
                if ((trips[i, 0] == linia || trips[i, 0].ToLower() == linia) && (trips[i, 3] == kierunek || trips[i, 3].ToLower() == kierunek) && Int32.Parse(trips[i, 1]) == poprawnywariant)
                {
                    for (int j = 0; j < stop_times_dlugosc; j++)
                    {
                        if (trips[i, 2] == stop_times[j, 0])
                        {
                            for (int k = 0; k < stops_dlugosc - 1; k++)
                            {
                                if (przystanek == stops[k, 1].ToLower() || przystanek == stops[k, 1])
                                {
                                    if (stops[k, 0] == stop_times[j, 2])
                                    {
                                        wyniki.Add(stop_times[j, 1]);
                                    }

                                }
                            }

                        }
                    }
                }
            }
            wyniki.Sort();
            szczegoly = FindViewById<TextView>(Resource.Id.text_szczegol);
            szczegoly.Text = linia + "(" + kierunek + ") " + przystanek;
            lista_wynikow1 = (ListView)FindViewById<ListView>(Resource.Id.wyniki1);
            lista_wynikow1.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, wyniki);

            powrot = FindViewById<Button>(Resource.Id.powrot3);
            powrot.Click += Powrot_click;


            przystanki.Clear();
            linie.Clear();
            kierunki.Clear();

            lista_wynikow1.ItemClick += Lista_wynikow1_ItemClick;


        }
        private void Powrot_click(object sender, System.EventArgs e)
        {

            SetContentView(Resource.Layout.activity_main);
            wybierzlinie = FindViewById<Button>(Resource.Id.linia);
            wybierzlinie.Click += Wybierzlinie_Click1;

            wybierzprzystanek = FindViewById<Button>(Resource.Id.przystanek);
            wybierzprzystanek.Click += Wybierzprzystanek_Click2;

            znajdznajlepsze = FindViewById<Button>(Resource.Id.znajdz);
            znajdznajlepsze.Click += Znajdznajlepsze_Click3;

            wyniki.Clear();
            przystanki.Clear();
            linie.Clear();
            kierunki.Clear();
        }

      
        private void Powrot_click3(object sender, EventArgs e)
        {

            string text = wynik.Text;
            

            SetContentView(Resource.Layout.activity_main);
            wybrane = FindViewById<TextView>(Resource.Id.textView1);
            wybrane.Text = text;
            wybierzlinie = FindViewById<Button>(Resource.Id.linia);
            wybierzlinie.Click += Wybierzlinie_Click1;

            wybierzprzystanek = FindViewById<Button>(Resource.Id.przystanek);
            wybierzprzystanek.Click += Wybierzprzystanek_Click2;

            znajdznajlepsze = FindViewById<Button>(Resource.Id.znajdz);
            znajdznajlepsze.Click += Znajdznajlepsze_Click3;

            ostatecznewyniki.Clear();


            tymczasowe_wyniki.Clear();
            wyniki_godzin.Clear();
            dojazdy.Clear();
            godziny_doj.Clear();
            wyniki.Clear();
            przystanki.Clear();
            linie.Clear();
            kierunki.Clear();
        }

     


        private void Wybierzprzystanek_Click2(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.dzien_tygodnia);


            lista_dni = (ListView)FindViewById<ListView>(Resource.Id.tydzien);
            lista_dni.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, dzien);


            lista_dni.ItemClick += lista_dni_click2;


            powrot = FindViewById<Button>(Resource.Id.powrot);
            powrot.Click += Powrot_click;




        }


        private void lista_dni_click2(object sender, AdapterView.ItemClickEventArgs e)
        {

            SetContentView(Resource.Layout.wpisanie_przystanku);
            poprawnywariant = day.liczeniewariantu(e, calendar, calendar_dlugosc, poprawnywariant);
            ok = FindViewById<Button>(Resource.Id.btnOk2);
            ok.Click += ok_Click2;
            powrot = FindViewById<Button>(Resource.Id.btnCancel2);
            powrot.Click += Powrot_click;
        }

        private void ok_Click2(object sender, EventArgs e)
        {

            var editinput = FindViewById<EditText>(Resource.Id.input2);


            if (editinput.Text.Length < 1)
            {
                SetContentView(Resource.Layout.activity_main);
                Powrot_click(sender, e);

            }
            else
            {
                InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
                inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
                SetContentView(Resource.Layout.przystanki1);

                przystanek = editinput.Text.ToString();
                for (int i = 1; i < stops_dlugosc - 1; i++)
                {
                    if ("\"" + przystanek + "\"" == stops[i, 1].ToLower() || "\"" + przystanek + "\"" == stops[i, 1])
                    {
                        for (int j = 1; j < stop_times_dlugosc; j++)
                        {
                            if (stops[i, 0] == stop_times[j, 2])
                            {
                                for (int k = 1; k < trips_dlugosc; k++)
                                {
                                    if (stop_times[j, 0] == trips[k, 2] && Int32.Parse(trips[k, 1]) == poprawnywariant)
                                    {

                                        if (!linie.Contains(trips[k, 0] + " " + trips[k, 3]))
                                        {
                                            linie.Add(trips[k, 0] + " " + trips[k, 3]);
                                        }


                                    }
                                }
                            }
                        }
                    }

                }
                linie.Sort();

                lista_przystankow = (ListView)FindViewById<ListView>(Resource.Id.przystanki_list);
                lista_przystankow.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, linie);

                lista_przystankow.ItemClick += Lista_przystankow_ItemClick2;

                powrot = FindViewById<Button>(Resource.Id.powrot4);
                powrot.Click += Powrot_click;

            }




        }

        private void Lista_przystankow_ItemClick2(object sender, AdapterView.ItemClickEventArgs e)
        {
            SetContentView(Resource.Layout.wyniki_opcji1);

            kierunek = linie[e.Position].Substring(linie[e.Position].IndexOf(" ") + 1, linie[e.Position].Length - linie[e.Position].IndexOf(" ") - 1);
            linia = linie[e.Position].Substring(0, linie[e.Position].IndexOf(" "));


            for (int i = 1; i < trips_dlugosc; i++)
            {
                if ((trips[i, 0] == linia || trips[i, 0].ToLower() == linia) && (trips[i, 3] == kierunek || trips[i, 3].ToLower() == kierunek) && Int32.Parse(trips[i, 1]) == poprawnywariant)
                {
                    for (int j = 1; j < stop_times_dlugosc; j++)
                    {
                        if (trips[i, 2] == stop_times[j, 0])
                        {
                            for (int k = 1; k < stops_dlugosc - 1; k++)
                            {
                                if ("\"" + przystanek + "\"" == stops[k, 1].ToLower() || "\"" + przystanek + "\"" == stops[k, 1])
                                {
                                    if (stops[k, 0] == stop_times[j, 2])
                                    {
                                        wyniki.Add(stop_times[j, 1]);
                                    }

                                }
                            }

                        }
                    }
                }

            }

            wyniki.Sort();
            lista_wynikow1 = (ListView)FindViewById<ListView>(Resource.Id.wyniki1);
            lista_wynikow1.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, wyniki);

            szczegoly = FindViewById<TextView>(Resource.Id.text_szczegol);
            szczegoly.Text = linia + "(" + kierunek + ") " + przystanek;



            powrot = FindViewById<Button>(Resource.Id.powrot3);
            powrot.Click += Powrot_click;

            lista_wynikow1.ItemClick += Lista_wynikow1_ItemClick;

            przystanki.Clear();
            linie.Clear();
            kierunki.Clear();

        }

     

      

        

     
        private void Lista_wynikow1_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            int ind = e.Position;
            SetContentView(Resource.Layout.activity_main);
            wybierzlinie = FindViewById<Button>(Resource.Id.linia);
            wybierzlinie.Click += Wybierzlinie_Click1;

            wybierzprzystanek = FindViewById<Button>(Resource.Id.przystanek);
            wybierzprzystanek.Click += Wybierzprzystanek_Click2;

            znajdznajlepsze = FindViewById<Button>(Resource.Id.znajdz);
            znajdznajlepsze.Click += Znajdznajlepsze_Click3;

            wybrane = FindViewById<TextView>(Resource.Id.textView1);
            wybrane.Text = wyniki[ind] + " " + szczegoly.Text;




            wyniki.Clear();
            przystanki.Clear();
            linie.Clear();
            kierunki.Clear();
        }

        private void Znajdznajlepsze_Click3(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.dzien_tygodnia);


            lista_dni = (ListView)FindViewById<ListView>(Resource.Id.tydzien);
            lista_dni.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, dzien);


            lista_dni.ItemClick += lista_dni_click3;


            powrot = FindViewById<Button>(Resource.Id.powrot);
            powrot.Click += Powrot_click;

        }

        private void lista_dni_click3(object sender, AdapterView.ItemClickEventArgs e)
        {

            poprawnywariant = day.liczeniewariantu(e, calendar, calendar_dlugosc, poprawnywariant);
            SetContentView(Resource.Layout.znajdznajlepsze);
            wybierzgodzine = FindViewById<Button>(Resource.Id.button1);


            wybierzgodzine.Click += Wybierzgodzine_Click;

            TextView nic = FindViewById<TextView>(Resource.Id.allocate_collector);
            nic.Text = "";

            powrot = FindViewById<Button>(Resource.Id.btnCancel);
            powrot.Click += Powrot_click;

            ok = FindViewById<Button>(Resource.Id.btnOk);
            ok.Click += ok_Click3;
        }


        private void ok_Click3(object sender, EventArgs e)
        {

            var editinput = FindViewById<EditText>(Resource.Id.input_1);
            var editinput2 = FindViewById<EditText>(Resource.Id.input_2);
            if (editinput.Text.Length < 1 || editinput2.Text.Length < 1)
            {
                SetContentView(Resource.Layout.activity_main);
                Powrot_click(sender, e);

            }
            else
            {
                InputMethodManager inputManager = (InputMethodManager)this.GetSystemService(Context.InputMethodService);
                inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);


                odjazd = editinput.Text.ToString();
                dojazd = editinput2.Text.ToString();
                SetContentView(Resource.Layout.wyniki_znalezienia);
                for (int i = 1; i < stops_dlugosc - 1; i++)
                {
                    if (stops[i, 1].ToLower() == "\"" + dojazd + "\"" || stops[i, 1] == "\"" + dojazd + "\"")
                    {
                        dojazd_id.Add(stops[i, 0]);

                    }
                    else if (stops[i, 1].ToLower() == "\"" + odjazd + "\"" || stops[i, 1] == "\"" + odjazd + "\"")
                    {
                        odjazd_id.Add(stops[i, 0]);
                    }

                }

                Znajdz1 find1 = new Znajdz1();
                wyniki = find1.szukajv1(stops, stops_dlugosc, stop_times, stop_times_dlugosc, trips, trips_dlugosc, odjazd, dojazd, poprawnywariant, godzina, dojazd_id, odjazd_id);

                if (wyniki.Count > 0)
                {
                    lista_przystankow = (ListView)FindViewById<ListView>(Resource.Id.wynikiznalezienia);
                    lista_przystankow.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, wyniki);
                    lista_przystankow.ItemClick += Lista_wynikow1_ItemClick;
                    szczegoly = FindViewById<TextView>(Resource.Id.text_szczegol);
                    szczegoly.Text = "połączenie z " + odjazd + " do " + dojazd + " od godziny " + godzina;

                }


                else
                {
                    Znajdz2 find2 = new Znajdz2();

                    ostatecznewyniki = find2.szukajv2(stops, stops_dlugosc, stop_times, stop_times_dlugosc, trips, trips_dlugosc, odjazd, dojazd, poprawnywariant, godzina, dojazd_id, odjazd_id);


                    for (int i = 0; i < ostatecznewyniki.Count; i++)
                    {
                        dojazdy.Add("dojazd o " + ostatecznewyniki[i].Substring(12 + dojazd.Length, 7));
                    }




                    if (ostatecznewyniki.Count > 0)
                    {

                        ekranwynikow_Click3(sender, e);
                    }
                    else
                    {



                        szczegoly = FindViewById<TextView>(Resource.Id.text_szczegol);
                        szczegoly.Text = "nie znaleziono połączenia";

                    }
                    powrot = FindViewById<Button>(Resource.Id.powrot3);
                    powrot.Click += Powrot_click;


                }
            }


        }

        private void Lista_przystankow_ItemClick3(object sender, AdapterView.ItemClickEventArgs e)
        {
            SetContentView(Resource.Layout.wyniki_lay);


            wynik = FindViewById<TextView>(Resource.Id.textViewwynik);
            wynik.Text = ostatecznewyniki[e.Position].Substring(ostatecznewyniki[e.Position].IndexOf("|") + 1, ostatecznewyniki[e.Position].Length - ostatecznewyniki[e.Position].IndexOf("|") - 1);
            powrot = FindViewById<Button>(Resource.Id.powrot5);

            powrot.Click += Powrot_click3;
            powrotdowynikow = FindViewById<Button>(Resource.Id.powrotdowynikow);
            powrotdowynikow.Click += ekranwynikow_Click3;


        }

        private void ekranwynikow_Click3(object sender, EventArgs e)
        {
            SetContentView(Resource.Layout.wyniki_znalezienia);

            szczegoly = FindViewById<TextView>(Resource.Id.text_szczegol);
            szczegoly.Text = odjazd + " do " + dojazd + " o " + godzina;

            lista_przystankow = (ListView)FindViewById<ListView>(Resource.Id.wynikiznalezienia);
            lista_przystankow.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, dojazdy);

            lista_przystankow.ItemClick += Lista_przystankow_ItemClick3;
        }

      
    }
}