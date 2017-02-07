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
            IEnumerable<Weight> wghts = conn.Query<Weight>("SELECT IDWeight, WValue, WeightDate FROM Weight WHERE deleted=0 and IDUser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString()+ " order by WeightDate desc");
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
            ListViewWeight.SelectedItem = null;
            await Navigation.PushAsync(myWeightDt);
        }

        async void OnAddWeightClicked(object sender, EventArgs e)
        {
            
            myWeightDt.LoadData(0);
            await Navigation.PushAsync(myWeightDt);
        }

        async void OnAddPlanClicked(object sender, EventArgs e)
        {
            planPageDt.LoadData();
            await Navigation.PushAsync(planPageDt);
        }

        private void FillContent()
        {
                     
            string query = "SELECT * FROM Weight where deleted=0 and IDuser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString()+ " order by WeightDate desc limit 10";
            List<Weight> weightRecords = conn.Query<Weight>(query);

            string queryGoal = "SELECT * FROM Plan where deleted=0 and IDuser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString() + " limit 1";
            List<Plan> planRecords = conn.Query<Plan>(queryGoal);
            double percentage = 0;
            double goalValue = 0;
            if (planRecords.Count > 0)
            {
                goalValue = planRecords[0].Goal;
                string q = "SELECT * FROM Weight where deleted=0 and IDuser=" + StorageManager.GetConnectionInfo().LoginUser.IDUser.ToString() + " and WeightDate>=" + planRecords[0].StartGoal.Ticks + " order by WeightDate asc limit 1";
                List<Weight> Firstweight = conn.Query<Weight>(q);
                if (Firstweight.Count > 0)
                {
                    percentage = Math.Round(((Firstweight[0].WValue - recordsWgt.First<Weight>().WValue) / (Firstweight[0].WValue - goalValue)) * 100,1);
                }
            }
            double [] data = new double [weightRecords.Count];
            double[] goal = new double[weightRecords.Count];
            string [] label = new string[weightRecords.Count];
            int i = 0;
            foreach (Weight wRecord in weightRecords)
            {
                goal[i] = goalValue;
                data[weightRecords.Count-1-i] = wRecord.WValue;
                label[weightRecords.Count-1-i] = '"' + wRecord.WeightDate.ToString("dd-MMM ") + '"';
                i++; 
            }
            
            webview.Html = "<!doctype html>" + 
                    "<html>" + 
                    "   <head>" + 
                    "       <script src=\"file:///android_asset/Chart.bundle.js\"></script>" + 
                    "       <script src=\"file:///android_asset/utils.js\"></script>" +
                    "       <script src=\"file:///android_asset/progressbar.js\" ></script>" +
                    "       <style>" +
                    "	    	 .progressbar__label{" +
                    "	    		font-weight:bold;" +
                    "	    		font-size:20px;" +
                    "               font - family: \"'Helvetica Neue', 'Helvetica', 'Arial', sans-serif\"; " +
                    "	    	 }" +
                    "	    	 #container{" +
                    "	    		margin-left:30%;" +
                    "	    		margin-right:30%;" +
                    "	    		margin-top:1%;" +
                    "	    		margin-bottom:2%;" +
                    "	    	 }" +
                    "	    	 #container-text{" +
                    "	    		vertical-align:\"center\";" +
                    "	    		color:#666;" +
                    "	    		font-weight:bold;" +
                    "	    		font-size:12px;" +
                    "	    		font-family: \"'Helvetica Neue', 'Helvetica', 'Arial', sans - serif\";" +
                    "	    	 }" +
                    "       </style>" +
                    "   </head>" +
                    "<body>" +
                    "   <div id=\"container-text\" align=\"Center\" >Completion rate</div>" +
                    "   <div id=\"container\"></div>" +
                    "   <script>" +
                    "      var circle = new ProgressBar.Circle('#container', {" +
                    "      	color: '#36A2EB'," +
                    "      	strokeWidth: 10," +
                    "      	trailColor: '#D7D3E7'," +
                    "      	trailWidth: 10," +
                    "      	text: {" +
                    "              value: '"+percentage+"%'," +
                    "              className: 'progressbar__label'," +
                    "              style: {" +
                    "      			color: '#666'," +
                    "                  position: 'absolute'," +
                    "                  left: '50%'," +
                    "                  top: '50%'," +
                    "                  padding: 0," +
                    "                  margin: 0," +
                    "                  transform: {" +
                    "                      prefix: true," +
                    "                      value: 'translate(-50%, -50%)'" +
                    "                  }" +
                    "              }," +
                    "           autoStyleContainer: true," +
                    "      		alignToBottom: true" +
                    "          }," +
                    "      	   duration: 1200," +
                    "          easing: 'easeOut'," +
                    "      	   warnings: false" +
                    "       }" +
                    "      );" +
                    "      circle.animate("+percentage/100+");  " +
                    "   </script>" +
                    "   <div><canvas id=\"canvas\"></canvas></div>" + 
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
                    "                 },{ " + 
                    "                   type  : 'line', " +
                    "                   label : 'Goal', " +
                    "                   backgroundColor : color(window.chartColors.red).alpha(0.2).rgbString(),  " +
                    "                   borderColor : window.chartColors.red, " +
                    "                   data : [" + String.Join(",", goal)  + "] " +
                    "                  "+
                    "               }] " + 
                    "           }; " +
                    "           window.onload = function() { var ctx = document.getElementById(\"canvas\").getContext(\"2d\");  window.myBar = new Chart(ctx, {" +
                    "           type: 'bar', data: barChartData, options: { title: { display: true, text: 'Weight per date' },legend: {position: 'bottom'},} });}; " + 
                    "       </script>" + 
                    "   </body>" + 
                    "</html>  ";                
        }
    } 
}
