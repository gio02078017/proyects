using System;
using System.Collections.Generic;
using GrupoExito.Entities.Entiites;

namespace GrupoExito.Models.ViewModels.Payments
{
    public class ScheduleHourViewModel : BaseViewModel
    {
        private ScheduleHours hour;
        public ScheduleHours Hour
        {
            get { return hour; }
            set { SetProperty(ref hour, value); }
        }

        public EventHandler CellSelected { get; set; }

        public ScheduleHourViewModel(ScheduleHours hour)
        {
            this.hour = hour;
        }
    }
}
