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
                Name = "One hour from now",
                TimeSpan = new TimeSpan(1,0,0)
            },
            new FriendlyTimeSpan()
            {
                Name = "Two hours from now",
                TimeSpan = new TimeSpan(2,0,0)
            },
            new FriendlyTimeSpan()
            {
                Name = "Three hours from now",
                TimeSpan = new TimeSpan(3,0,0)
            },
            new FriendlyTimeSpan()
            {
                Name = "Four hours from now",
                TimeSpan = new TimeSpan(4,0,0)
            },
            new FriendlyTimeSpan()
            {
                Name = "After Noon",
                TimeSpan = DateTime.Today.AddHours((double)13)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                Name = "End of today",
                TimeSpan = DateTime.Today.AddHours((double)17)-DateTime.Now
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
                Name = "Until I manually clear it",
                TimeSpan = new TimeSpan(365, 0,0,0)
            },
        };

        private static DateTime FindNextMonday()
        {
            for (int x = 1; x<=8; x++)
            {
                DateTime adjustedDay = DateTime.Today.AddDays(1);
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