using MvcContrib.UI.Grid;
using POS.Models;
using System.Collections.Generic;
using System.Linq;

namespace POS.Controllers
{
    internal class GridModel : GridModel<object>
    {
        private List<DeeperLookViewModel> model;

        public GridModel(List<DeeperLookViewModel> model)
        {
            this.model = model;
        }

        public GridModel(IQueryable<MasterViewModel> model1)
        {
            Model = model1;
        }

        public IQueryable<MasterViewModel> Model { get; }
    }
}