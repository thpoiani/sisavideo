using System;
using System.Configuration;
using Google.GData.Calendar;
using Google.GData.Client;
using Google.GData.Extensions;

namespace SisAVideo.Google
{
    class Calendario
    {
        private Uri calendarUri;
        private string username;
        private string password;

        private CalendarService service = null;

        private string title;
        private DateTime start;
        private DateTime end;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public DateTime Start
        {
            get { return start; }
            set { start = value; }
        }

        public DateTime End
        {
            get { return end; }
            set { end = value; }
        }

        public Calendario(string username, string password)
        {
            this.username = username;
            this.password = password;

            string calendarUrl = "https://www.google.com/calendar/feeds/" + username + "/private/full";

            calendarUri = new Uri(calendarUrl);
        }

        public void GoogleCalendar()
        {
            service = new CalendarService("Google Calendar");
            service.setUserCredentials(username, password);
        }

        private DateTime DateSet(string date, string time)
        {
            // date = DD/MM/YYYY
            // xTime = hh:mm

            int dia = Int32.Parse(date.Substring(0, 2));
            int mes = Int32.Parse(date.Substring(3, 2));
            int ano = Int32.Parse(date.Substring(6, 4));
            int hora = Int32.Parse(time.Substring(0, 2));
            int minuto = Int32.Parse(time.Substring(3, 2));

            return new DateTime(day: dia, month: mes, year: ano, hour: hora, minute: minuto, second: 0);
        }

        public void Event(string title, string date, string startTime, string endTime)
        {
            Title = title;
            Start = DateSet(date, startTime);
            End = DateSet(date, endTime);
        }

        public void Insert()
        {
            EventEntry entry = new EventEntry();

            entry.Title.Text = Title;

            When eventTime = new When();
            eventTime.StartTime = Start;
            eventTime.EndTime = End;
            entry.Times.Add(eventTime);

            service.Insert(calendarUri, entry);
        }

        public void Delete()
        {
            EventQuery entry = new EventQuery(calendarUri.ToString());

            entry.Query = Title;
            entry.StartTime = Start;
            entry.EndTime = End;

            EventFeed feed = service.Query(entry);

            foreach (EventEntry entries in feed.Entries)
            {
                entries.Delete();
                break;
            }
        }
    }
}
