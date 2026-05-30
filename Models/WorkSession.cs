using System;

namespace WorkTrackLite.Models
{
    namespace WorkTrackLite.Models
    {
        public class WorkSession
        {
            public int SerialNo { get; set; }
            public string? MachineName { get; set; }
            public string? UserName { get; set; }            

            public DateTime? CheckInTime { get; set; }

            public DateTime ?CheckOutTime { get; set; }

            public string ?Status { get; set; }

            public bool ?Synced { get; set; }
            //private bool _synced;

            //public bool Synced
            //{
            //    get => _synced;
            //    set => SetProperty(ref _synced, value);
            //}
        }
    }
}
