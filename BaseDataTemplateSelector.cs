using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using System.Reflection;

namespace Get.the.solution.UWP.XAML
{
    public class BaseDataTemplateSelector : DataTemplateSelector
    {
        public BaseDataTemplateSelector()
        {
            Controls = Enumerable.Empty<FrameworkElement>();
        }

        public object DataTemplateType
        {
            get;
            set;
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            Controls = Helper.FindParents<FrameworkElement>(container);

            foreach (DataTemplate control in DataTemplates)
            {
                var e = (control.LoadContent() as FrameworkElement).GetBindingExpressions().ToList();
            }

            //TODO Get DataType of DataTemplate



            return SelectTemplateCore(item);
        }

        protected IEnumerable<FrameworkElement> Controls { get; set; }

        protected IEnumerable<DataTemplate> DataTemplates
        {
            get
            {
                return Controls.SelectMany(a => a.Resources).Where(r => r.Value.GetType() == typeof(DataTemplate)).Select(d => d.Value).Cast<DataTemplate>();
            }
        }


    }
}
