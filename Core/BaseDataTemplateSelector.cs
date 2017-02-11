using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using System.Reflection;
using Windows.UI.Xaml.Data;

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
            //get required Type for DataTemplate
            FrameworkElement Element = container as FrameworkElement;
            Type RequiredTypeForDataTemplate = Element.DataContext.GetType();

            Controls = Helper.FindParents<FrameworkElement>(container);

            foreach (DataTemplate control in DataTemplates)
            {
                var yo = control.GetType().GetTypeInfo();
                //var test = yo.GetCustomAttribute(typeof(System.ComponentModel.DataAnnotations), true);
                var e = (control.LoadContent() as FrameworkElement).GetBindingExpressions().ToList();
                foreach (BindingExpression b in e)
                {
                    
                }
                var uor = control.LoadContent();

            }

            //TODO Get DataType of DataTemplate
            //http://stackoverflow.com/questions/39546479/get-datatype-datatemplatekey
            var key = new DataTemplateKey(item.GetType());
            Page Page = Controls.FirstOrDefault(p => p as Page != null) as Page;

            //var dataTemplate = (DataTemplate)this.FindResource(key);

            //var tc = dataTemplate.LoadContent().GetType();


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
