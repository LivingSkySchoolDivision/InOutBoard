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
                ID = 1,
                Name = "This afternoon",
                TimeSpan = DateTime.Today.AddHours((double)13)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                ID = 2,
                Name = "End of today",
                TimeSpan = DateTime.Today.AddHours((double)17)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                ID = 11,
                Name = "Tomorrow",
                TimeSpan = DateTime.Today.AddHours((double)24)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                ID = 3,
                Name = "Tomorrow afternoon",
                TimeSpan = DateTime.Today.AddHours((double)36)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                ID = 4,
                Name = "End of tomorrow",
                TimeSpan = DateTime.Today.AddHours((double)42)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                ID = 5,
                Name = "Next week (Monday)",
                TimeSpan = FindNextMonday()-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                ID = 6,
                Name = "The week after next (Monday)",
                TimeSpan = FindNextMonday().AddDays(7)-DateTime.Now
            },
            new FriendlyTimeSpan()
            {
                ID = 7,
                Name = "1 hour",
                TimeSpan = new TimeSpan(1,0,0)
            },
            new FriendlyTimeSpan()
            {
                ID = 8,
                Name = "2 hours",
                TimeSpan = new TimeSpan(2,0,0)
            },
            new FriendlyTimeSpan()
            {
                ID = 9,
                Name = "3 hours",
                TimeSpan = new TimeSpan(3,0,0)
            },
            new FriendlyTimeSpan()
            {
                ID = 10,
                Name = "Until I manually clear it",
                TimeSpan = new TimeSpan(365*5, 0,0,0)
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

        public FriendlyTimeSpan Get(int id)
        {
            foreach (FriendlyTimeSpan t in this.GetAll())
            {
                if (t.ID == id)
                {
                    return t;
                }
            }

            return new FriendlyTimeSpan();
        }
    }
}