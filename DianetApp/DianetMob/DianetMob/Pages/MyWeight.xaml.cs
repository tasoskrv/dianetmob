using DianetMob.DB;
using DianetMob.DB.Entities;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Pages
{
    public partial class MyWeight : ContentPage
    {
        private SQLiteConnection conn = null;
        private ObservableCollection<Weight> records = new ObservableCollection<Weight>();
        private MyWeightDetail myWeightDt = new MyWeightDetail();
        private PlanPageDetail planPageDt = new PlanPageDetail();

        public MyWeight()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();
            string WeightContent;
            WeightContent = FillContent(5, 10, 15, 9);
           

            var html = new HtmlWebViewSource
            {
                Html = WeightContent
            };


            webview2.Source = html;
        }



        protected override void OnAppearing()
        {
            //records.Clear();
            IEnumerable<Weight> wghts = conn.Query<Weight>("SELECT IDWeight, WValue, InsertDate FROM Weight WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
            IEnumerable<Plan> plans = conn.Query<Plan>("SELECT IDPlan, Goal, GoalDate FROM Plan WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
            // prepei na kathorisoume poies times apo autes tou wghts k plans theloume na blepoume sto Chart

             
           /* foreach (Weight wght in wghts)
            {
                records.Add(new Weight { IDWeight = wght.IDWeight, WValue = wght.WValue, InsertDate = wght.InsertDate });
            }*/
        }

        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            Weight myWght = e.Item as Weight;
            myWeightDt.LoadData(myWght.IDWeight);
            await Navigation.PushAsync(myWeightDt);
        }

        async void OnAddWeightClicked(object sender, EventArgs e)
        {
            myWeightDt.LoadData(0);
            await Navigation.PushAsync(myWeightDt);
        }

        async void OnAddPlanClicked(object sender, EventArgs e)
        {
            planPageDt.LoadData(0);
            await Navigation.PushAsync(planPageDt);
        }


        private string FillContent(int v1, int v2, int v3, int v4)
    {

            return "<!doctype html><html><head> <script src=\"file:///android_asset/Chart.bundle.js\"></script><script src=\"file:///android_asset/utils.js\"></script><style>" +
                    "canvas { -moz-user-select: none; -webkit-user-select: none; -ms-user-select: none; } </style>" +
                    "</head><body><div><canvas id=\"canvas\" height= \"260% \"></canvas></div><script> var color = Chart.helpers.color;" +
                    "var barChartData = { labels: [\"17/12/2016\", \"25/12/2016\", \"25/12/ 2016\", \"25/12/2016\", \"25/12/2016\", \"25/12/2016\"]," +
                    "datasets: [{ type: 'bar', label: 'Weight', backgroundColor: color(window.chartColors.blue).alpha(0.2).rgbString(), borderColor: window.chartColors.blue," +
                    "data: [90,89,88,88,86,86]}, {type: 'line', label: 'Goal', backgroundColor: color(window.chartColors.red).alpha(0.2).rgbString(),  " +
                    "borderColor: window.chartColors.red, data: [ 90,  88, 86, 85, 84, 83] }]};   " +
                    "Chart.plugins.register({ afterDatasetsDraw: function(chartInstance, easing) { var ctx = chartInstance.chart.ctx;" +
                    "chartInstance.data.datasets.forEach(function (dataset, i) {var meta = chartInstance.getDatasetMeta(i);" +
                    "if (!meta.hidden) {  meta.data.forEach(function(element, index) { ctx.fillStyle = 'rgb(0, 0, 0)'; " +
                    "var fontSize = 10; var fontStyle = 'normal'; var fontFamily = 'Helvetica Neue'; ctx.font = Chart.helpers.fontString(fontSize, fontStyle, fontFamily); " +
                    "var dataString = dataset.data[index].toString(); ctx.textAlign = 'center'; ctx.textBaseline = 'middle'; var padding = 5; var position = element.tooltipPosition();" +
                    "ctx.fillText(dataString, position.x, position.y - (fontSize / 2) - padding);  });} }); }}); " +
                    "window.onload = function() { var ctx = document.getElementById(\"canvas\").getContext(\"2d\");  window.myBar = new Chart(ctx, {" +
                    "type: 'bar', data: barChartData, options: { title: { display: true },} });}; </script></body></html>  ";
                



        }

} 
}
