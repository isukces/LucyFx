using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lucy
{
    public class ModelViewModel<TModel, TViewModel>
    {
        public ModelViewModel(TModel model, TViewModel viewData)
        {
            Model = model;
            ViewData = viewData;
        }

        public TModel Model { get; private set; }
        public TViewModel ViewData { get; private set; }
    }
}
