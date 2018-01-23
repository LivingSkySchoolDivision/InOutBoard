using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYInOut
{
    public class FriendlyTimeSpanRepository
    {
        private List<FriendlyTimeSpan> _list = new List<FriendlyTimeSpan>()
        {
            new FriendlyTimeSpan()
            {
                Name = "Morning",
                TimeSpan = DateTime.Today.AddHours((double)13)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                Name = "Afternoon",
                TimeSpan = DateTime.Today.AddHours((double)17)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                Name = "End of today",
                TimeSpan = DateTime.Today.AddHours((double)17)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                Name = "Tomorrow afternoon",
                TimeSpan = DateTime.Today.AddHours((double)36)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                Name = "End of tomorrow",
                TimeSpan = DateTime.Today.AddHours((double)42)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                Name = "Next week",
                TimeSpan = FindNextMonday()-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                Name = "The week after next",
                TimeSpan = FindNextMonday().AddDays(7)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                Name = "1 hour",
                TimeSpan = new TimeSpan(1,0,0)
            },
            new FriendlyTimeSpan()
            {
                Name = "2 hours",
                TimeSpan = new TimeSpan(2,0,0)
            },
            new FriendlyTimeSpan()
            {
                Name = "3 hours",
                TimeSpan = new TimeSpan(3,0,0)
            },
            new FriendlyTimeSpan()
            {
                Name = "Until I manually clear it",
                TimeSpan = new TimeSpan(365, 0,0,0)
            },

        };

        private static DateTime FindNextMonday()
        {
            for (int x = 1; x<=8; x++)
            {
                DateTime adjustedDay = DateTime.Today.AddDays(x);
                if (adjustedDay.DayOfWeek == DayOfWeek.Monday)
                {
                    return adjustedDay;
                }
            }
            return DateTime.Today;
        }


        public List<FriendlyTimeSpan> GetAll()
        {
            return _list.ToList();
        }
    }
}