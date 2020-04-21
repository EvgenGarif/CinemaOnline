using System.Web;
using System.Web.Mvc;
using Cinema.Interfaces;
using Cinema.Services;
using LightInject;

namespace Cinema.Attributes
{
    public class PopulateHallsListAttribute : ActionFilterAttribute
    {
        [Inject]
        public ITicketsService TicketsService { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.Controller.ViewData["HallsList"] =
                TicketsService.GetAllHalls();
            base.OnActionExecuting(filterContext);
        }
    }
}