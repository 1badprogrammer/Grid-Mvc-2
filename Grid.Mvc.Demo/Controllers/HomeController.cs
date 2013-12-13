using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Grid.Mvc.Demo.Models;
using Grid.Mvc.Ajax.GridExtensions;

namespace Grid.Mvc.Demo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            var cars = Data();
            var grid = (AjaxGrid<GridDataRow>)new AjaxGridFactory().CreateAjaxGrid(cars, 1, false);
            return View(new GridData(){ Grid = grid});
        }

        [HttpGet]
        public ActionResult Grid(int? page)
        {
            var cars = Data();

            var grid = new AjaxGridFactory().CreateAjaxGrid(cars, page.HasValue ? page.Value : 1, page.HasValue);

            return Json(new { Html = grid.ToJson("_CarGrid", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
        }

        private IQueryable<GridDataRow> Data()
        {
            var cars = new List<GridDataRow>()
                {
                    new GridDataRow() {CarMake = "Chevy", CarModel = "Cavalier", Condition = "Poor", Year = 2002},
                    new GridDataRow() {CarMake = "Ford", CarModel = "F150", Condition = "Good", Year = 2011},
                    new GridDataRow() {CarMake = "Toyota", CarModel = "Corolla", Condition = "Excellent", Year = 2012},
                    new GridDataRow() {CarMake = "Honda", CarModel = "Civic", Condition = "Poor", Year = 2004},
                    new GridDataRow() {CarMake = "Tesla", CarModel = "Model S", Condition = "Excellent", Year = 2012},
                    new GridDataRow() {CarMake = "Ford", CarModel = "F250", Condition = "Good", Year = 2009},
                    new GridDataRow() {CarMake = "Chevy", CarModel = "1500", Condition = "Good", Year = 2009},
                    new GridDataRow() {CarMake = "Ford", CarModel = "Raptor", Condition = "Excellent", Year = 2011},
                    new GridDataRow() {CarMake = "Chevy", CarModel = "Cavalier", Condition = "Good", Year = 2003},
                    new GridDataRow() {CarMake = "Chevy", CarModel = "Cobalt", Condition = "Excellent", Year = 2006},
                }.AsQueryable();

            return cars;
        }

    }
}
