using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Service;
using DianetMob.TableMapping;
using DianetMob.Utils;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DianetMob.Views
{
    public partial class DashboardView : ContentView
    {
        private SQLiteConnection conn = null;
        private HtmlWebViewSource webview = null;
        

        public DashboardView()
        {
            InitializeComponent();
            conn = StorageManager.GetConnection();


            //IEnumerable<MapLogData> logrecords = logrecords = StorageManager.LoadDataByDate(datePick.Date, datePick.Date);

            
            webview = new HtmlWebViewSource();
      
            webview1.Source = webview;
        }
        public string GetLabelCategory(int id) {
            switch (id) {
                case 1:
                    return "\"Breakfast\"";
                case 2:
                    return "\"Lunch\"";
                case 3:
                    return "\"Dinner\"";
                case 4:
                    return "\"Snack\"";
                default:
                    return "\"\"";
            } 
        }

        public void FillPieContent(Dictionary<int, double> DashboardDic, double Total, IEnumerable<MapLogData> maplogs)
        { 
            string data = "";
            string label = "";
            foreach (KeyValuePair<int, Double> entry in DashboardDic) {
                data += PointSystem.PointCalculate(entry.Value) + ",";
                label += GetLabelCategory(entry.Key) + ",";
            }
            data =data.Remove(data.Length - 1);
            label = label.Remove(label.Length - 1);

            string Wdata = "";
            string Wlabel = "";
            foreach (MapLogData maplog in maplogs) {
                Wdata += PointSystem.PointCalculate(maplog.Calories) + ",";
                Wlabel += "\""+maplog.MealDate.DayOfWeek +"\""+ ",";
            }
            if (Wdata.Length > 0)
            {
                Wdata = Wdata.Remove(Wdata.Length - 1);
                Wlabel = Wlabel.Remove(Wlabel.Length - 1);
            }
            string web1 = "<!doctype html>" +
            "<html>" +
                "<head> " +
                    "<script src=\"file:///android_asset/Chart.bundle.js\"></script>" +
                    "<script src=\"file:///android_asset/utils.js\"></script>" +
                    
            "</head>" +
                "<body>" +
                    "<div style=\"margin:0 auto;width: 70%\" id=\"canvas - holder\">" +
                        "<canvas id=\"chart - area\" />" +
                    "</div>" +
                    "<div style=\"width: 100%; \">" +
                        "<canvas id=\"canvas\"></canvas> " +
                    "</div>" +
                    "<script>" +
                        "var data = [" + data + "];" +
                        "var config = {" +
                        " type: 'pie',  data: { datasets: [{ " +
                        " data: data,  backgroundColor: [window.chartColors.blue, window.chartColors.yellow, window.chartColors.orange, window.chartColors.green], " +
                        " label: " +
                        " 'Calories'  }]," +
                        " labels: [" + label + "] },  options: {legend: {position: 'bottom'} ,title: {display: true, text: 'Points per meal' } }}; " +
                        "var color = Chart.helpers.color; " +
                        "var barChartData = { " +
                        "labels: [" + Wlabel + "], " +
                        "datasets: [{ type: \"bar\", label: \"Week Points\", backgroundColor: color(window.chartColors.blue).alpha(0.4).rgbString(), ";
                        
                        
                        
                        
                        string web2= "borderColor: window.chartColors.blue, data: [ " + Wdata + "]}] }; " +
                        "window.onload = function() {" +
                        "var ctx1 = document.getElementById(\"chart - area\").getContext(\"2d\"); " +
                        "window.myPie = new Chart(ctx1, config); " +
                        "var ctx = document.getElementById(\"canvas\").getContext(\"2d\"); " +
                        "window.myBar = new Chart(ctx, { type: \"bar\", data: barChartData,  options: {legend: {position: 'bottom'} ,title: {display: true, text: 'Points per day' } }}); }; " +
                    "</script>" +
                "</body>" +
            "</html>";

            webview.Html = web1 + web2;
        }

    }
}
