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
    
    public class WybierzGodzine : DialogFragment, TimePickerDialog.IOnTimeSetListener
    {
        TimeSpan godzina;

        public static readonly string TAG = "MyTimePickerFragment";
        Action<DateTime> timeSelectedHandler = delegate { };

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currentTime = DateTime.Now;
            bool is24HourFormat = DateFormat.Is24HourFormat(Activity);
            TimePickerDialog dialog = new TimePickerDialog
                (Activity, this, currentTime.Hour, currentTime.Minute, true);
            return dialog;
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            DateTime currentTime = DateTime.Now;
            DateTime selectedTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hourOfDay, minute, 0);
            Log.Debug(TAG, selectedTime.ToLongTimeString());
            timeSelectedHandler(selectedTime);
        }

        public static WybierzGodzine NewInstance(Action<DateTime> onTimeSelected)
        {
            WybierzGodzine frag = new WybierzGodzine();
            frag.timeSelectedHandler = onTimeSelected;
            return frag;
        }



        



    }
}