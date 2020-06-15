using System;
namespace GrupoExito.Models.ViewModels.Payments
{
    public class ScheduleTypeViewModel : BaseViewModel
    {
        public ScheduleTypeViewModel()
        {
        }

        public bool Express { get; set; }
        public int MinutesPromiseDelivery { get; set; }
        public string PricePromise { get; set; }

        public void ExpressAction()
        {

        }

        public void SheduleAction()
        {

        }
    }
}
