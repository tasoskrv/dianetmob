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
        public static ObservableCollection<Weight> recordsWgt = new ObservableCollection<Weight>();
        private MyWeightDetail myWeightDt = new MyWeightDetail();
        private PlanPageDetail planPageDt = new PlanPageDetail();
        private HtmlWebViewSource webview = null;

        public MyWeight()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();            
            webview = new HtmlWebViewSource();
            webview2.Source = webview;
            setRecords();
        }

        public void setRecords()
        {
            ListViewWeight.ItemsSource = null;
            recordsWgt.Clear();
            IEnumerable<Weight> wghts = conn.Query<Weight>("SELECT IDWeight, WValue, WeightDate FROM Weight WHERE IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString());
            foreach (Weight wght in wghts)
            {
                recordsWgt.Add(new Weight { IDWeight = wght.IDWeight, WValue = wght.WValue, WeightDate = wght.WeightDate });
            }
            ListViewWeight.ItemsSource = recordsWgt; 
        }

        protected override void OnAppearing()
        {
            FillContent();
        }

        public void OnDeleted(object sender, EventArgs e)
        {
            var selectedItem = (MenuItem)sender;
            var selectedWeight = selectedItem.CommandParameter as Weight;

            if (selectedWeight.IDServer == 0)
            {
                recordsWgt.Remove(selectedWeight);
                StorageManager.DeleteData(selectedWeight);
            }
            else
            {
                selectedWeight.Deleted = 1;
                StorageManager.UpdateData(selectedWeight);
                setRecords();
            }
            FillContent();
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

        private void FillContent()
        {

            string query = "SELECT * FROM Weight where IDuser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString();
            List<Weight> weightRecords = conn.Query<Weight>(query);

            string queryGoal = "SELECT * FROM Plan where IDuser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString();
            List<Plan> planRecords = conn.Query<Plan>(queryGoal);

            double goalValue = planRecords[0].Goal;

            double [] data = new double [weightRecords.Count];
            double[] goal = new double[weightRecords.Count];
            string [] label = new string[weightRecords.Count];
            int i = 0;
            foreach (Weight wRecord in weightRecords)
            {
                data[i] = wRecord.WValue;
                goal[i] = goalValue;
                label[i] = '"' + wRecord.WeightDate.ToString("dd/MM/yyyy") + '"';
                i++;
            }

            webview.Html = "<!doctype html>" + 
                    "<html>" + 
                    "   <head>" + 
                    "       <script src=\"file:///android_asset/Chart.bundle.js\"></script>" + 
                    "       <script src=\"file:///android_asset/utils.js\"></script>" + 
                    "       <style>" +
                    "           canvas { -moz-user-select: none; -webkit-user-select: none; -ms-user-select: none; } " + 
                    "       </style>" +
                    "   </head>" +
                    "   <body>" + 
                    "       <div>" + 
                    "           <canvas id=\"canvas\" height= \"260% \"></canvas>" + 
                    "       </div>" + 
                    "       <script> " + 
                    "           var color = Chart.helpers.color;" +
                    "           var barChartData = { " +
                    "               labels   :  [" + String.Join(",", label) + "]," +
                    "               datasets :  [{ " +
                    "                   type  : 'line', " + 
                    "                   label : 'Weight', " + 
                    "                   backgroundColor : color(window.chartColors.blue).alpha(0.2).rgbString(), " + 
                    "                   borderColor : window.chartColors.blue," +
                    "                   data : [" + String.Join(",", data) + "] " +
                    "               },{ " + 
                    "                   type  : 'line', " + 
                    "                   label : 'Goal', " + 
                    "                   backgroundColor : color(window.chartColors.red).alpha(0.2).rgbString(),  " +
                    "                   borderColor : window.chartColors.red, " +
                    "                   data : [" + String.Join(",", goal) + "] " + 
                    "               }] " + 
                    "           }; " +
                    "           Chart.plugins.register({ afterDatasetsDraw: function(chartInstance, easing) { var ctx = chartInstance.chart.ctx;" +
                    "           chartInstance.data.datasets.forEach(function (dataset, i) {var meta = chartInstance.getDatasetMeta(i);" +
                    "           if (!meta.hidden) {  " + 
                    "               meta.data.forEach(function(element, index) { " + 
                    "                   ctx.fillStyle = 'rgb(0, 0, 0)'; " +
                    "                   var fontSize = 10; " + 
                    "                   var fontStyle = 'normal'; " +
                    "                   var fontFamily = 'Helvetica Neue'; " +
                    "                   ctx.font = Chart.helpers.fontString(fontSize, fontStyle, fontFamily); " +
                    "               var dataString = dataset.data[index].toString(); " + 
                    "               ctx.textAlign = 'center'; " + 
                    "               ctx.textBaseline = 'middle'; " + 
                    "               var padding = 5; " + 
                    "               var position = element.tooltipPosition();" +
                    "               ctx.fillText(dataString, position.x, position.y - (fontSize / 2) - padding);  " +
                    "           }); " +
                    "           } }); }}); " +
                    "           window.onload = function() { var ctx = document.getElementById(\"canvas\").getContext(\"2d\");  window.myBar = new Chart(ctx, {" +
                    "           type: 'bar', data: barChartData, options: { title: { display: true },} });}; " + 
                    "       </script>" + 
                    "   </body>" + 
                    "</html>  ";                
        }
    } 
}
