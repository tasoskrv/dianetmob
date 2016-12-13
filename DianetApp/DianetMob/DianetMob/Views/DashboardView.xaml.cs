﻿using DianetMob.DB;
using DianetMob.DB.Entities;
using DianetMob.Service;
using DianetMob.TableMapping;
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
        private Dictionary<int, double> DashboardDic = new Dictionary<int, double>(); 
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

        public void FillPieContent(IEnumerable<MapLogData> logrecords)
        {
            DashboardDic.Clear();
            DashboardDic.Add(1, 0);
            DashboardDic.Add(2, 0);
            DashboardDic.Add(3, 0);
            DashboardDic.Add(4, 0);
            foreach (MapLogData logrecord in logrecords)
            {
                DashboardDic[logrecord.IDCategory] += logrecord.Calories;
            }
            string data = "";
            string label = "";
            foreach (KeyValuePair<int, Double> entry in DashboardDic) {
                data += entry.Value + ",";
                label += GetLabelCategory(entry.Key) + ",";
            }
            data =data.Remove(data.Length - 1);
            label = label.Remove(label.Length - 1);

            webview.Html=
            "<!doctype html>" +
            "<html>" +
                "<head> " +
                    "<script src=\"file:///android_asset/Chart.bundle.js\"></script>" +
                    "<script src=\"file:///android_asset/utils.js\"></script>" +
                "</head>" +
                "<body>" +
                    "<div id=\"canvas - holder\" style=\"height: 20%\">" +
                        "<canvas id=\"chart - area\" />" +
                    "</div>" +
                    "<div style=\"width: 100%; \">" +
                        "<canvas id=\"canvas\"></canvas> " +
                    "</div>" +
                    "<script>" +
                        "var data = [" + data + "];" +
                        "var config = {" +
                        " type: 'pie', data: { datasets: [{ " +
                        " data: data, backgroundColor: [window.chartColors.blue, window.chartColors.yellow, window.chartColors.orange, window.chartColors.green], " +
                        " label: " +
                        " 'Calories'  }]," +
                        " labels: ["+ label + "] },  options: {responsive: true  }  }; " +
                        "var color = Chart.helpers.color; " +
                        "var barChartData = { " +
                        "labels: [\"Monday\", \"Tuesday\", \"Wednesday\", \"Thursday\", \"Friday\", \"Saturday\", \"Sunday\"], " +
                        "datasets: [{ type: \"bar\", label: \"Week Points\", backgroundColor: color(window.chartColors.blue).alpha(0.4).rgbString(), " +
                        "borderColor: window.chartColors.blue, data: [ 10,20,6,8,14,17,11]}] }; " +
                        "window.onload = function() {" +
                        "var ctx1 = document.getElementById(\"chart - area\").getContext(\"2d\"); " +
                        "window.myPie = new Chart(ctx1, config); " +
                        "var ctx = document.getElementById(\"canvas\").getContext(\"2d\"); " +
                        "window.myBar = new Chart(ctx, { type: \"bar\", data: barChartData, options: { responsive: true} }); }; " +
                    "</script>" +
                "</body>" +
            "</html>";
        }

    }
}
