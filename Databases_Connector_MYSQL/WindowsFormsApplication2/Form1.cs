using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Diagnostics;

 

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        //
        MySqlCommand cmd;
        VehicleDBMgr vdm = new VehicleDBMgr();
        DataRow[] result;
        DataSet locationinfo;
        Dictionary<int, List<obj>> dictionary;
        //

        MySqlCommand cmd2;
        DataSet locationinfo2;
        DataRow[] result2;

        //
        Dictionary<int,int> sno_convert;

        public Form1()
        {
            InitializeComponent();
        }

        private void button_getLocation_Click(object sender, EventArgs e)
        {
        cmd= new MySqlCommand("SELECT * FROM experiment_lab.driver_behavior_logs where time_stamp between '2015-12-11 11:54:00' and  '2015-12-13 12:10:00';");
         
           locationinfo= vdm.SelectQuery(cmd);
           
           dataGridView1.DataSource = locationinfo.Tables[0];

            /*

            dictionary = new Dictionary<int, List<obj>>();

            sno_convert = new Dictionary<int, int>();

            
            result = locationinfo.Tables[0].Select("");
            int number_loc = result.Length;
// No bugs here
            for (int i = 0; i < result.Length; i++)
            {
                 List<obj> temp = new List<obj>();
                for (int j = 0; j < result.Length; j++)
                {
                    obj d = new obj();
                    d.id = Int32.Parse(result[j]["Sno"].ToString());                   
                   d.radius = Int32.Parse(result[j]["Radious"].ToString());
                   double lon1= Double.Parse(result[i]["Longitude"].ToString());
                    double lat1 = Double.Parse(result[i]["Latitude"].ToString());
                    double lon2= Double.Parse(result[j]["Longitude"].ToString());
                    double lat2 = Double.Parse(result[j]["Latitude"].ToString());
                    if (i != j)
                        d.distance = DistanceAlgorithm.DistanceBetweenPlaces(lon1, lat1, lon2, lat2);
                    else
                        d.distance = 0;
                    temp.Add(d);
                }
                sno_convert.Add(Int32.Parse(result[i]["Sno"].ToString()), i);

                dictionary.Add(Int32.Parse(result[i]["Sno"].ToString()), temp);
            }
             
             
             */

            Console.WriteLine("Good");


   //    till here      



            
             



            

        }

        private void button1_Click(object sender, EventArgs e)
        {
          cmd2 = new MySqlCommand("select * from gpstrackvehiclelogs where UserID = 'dodla' and VehicleID = 'AP03TA0264' and (`DateTime` between '2015-10-01 00:00:00' and '2015-12-02 00:00:00')");
           
             locationinfo2 = vdm.SelectQuery(cmd2);

            dataGridView1.DataSource = locationinfo2.Tables[0];

            result2 = locationinfo2.Tables[0].Select("");


        }

        private void button2_Click(object sender, EventArgs e)
        {

            Stopwatch stopwatch = Stopwatch.StartNew();

            int dog = 0;
           int restart=0;
            int Sno_initial=0;
//
            for (int j = 0; j < result2.Length; j++)
            {
                double flon = Double.Parse(result2[j]["Longitude"].ToString());
                double flat = Double.Parse(result2[j]["Latitiude"].ToString());



                for (int i = 0; i < result.Length; i++)
                {
                    dog++;
                    double longi = Double.Parse(result[i]["Longitude"].ToString());
                    double lati = Double.Parse(result[i]["Latitude"].ToString());


                    if (DistanceAlgorithm.DistanceBetweenPlaces(flon, flat, longi, lati) < Int32.Parse(result[i]["Radious"].ToString()))
                    {
                        Sno_initial = Int32.Parse(result[i]["Sno"].ToString());
                        restart = j;
                        break;

                    }
                }

                if (  Sno_initial != 0)
                    break;
            }
// No bugs till here
            restart++;
            Console.WriteLine("HEllo");
            

            for (int i = restart; i < result2.Length;i++ )
            {
                DataRow row = result2[i];
                double longi = Double.Parse(row["Longitude"].ToString());
                double lati = Double.Parse(row["Latitiude"].ToString());
                double temp1 = Double.Parse(result[sno_convert[Sno_initial]]["Longitude"].ToString());
                double temp2 = Double.Parse(result[sno_convert[Sno_initial]]["Latitude"].ToString());

                double dist = DistanceAlgorithm.DistanceBetweenPlaces(longi, lati, temp1, temp2);

              

                List<obj> list = dictionary[Sno_initial];
              double mind=0;
                foreach(obj ob in list)
                {
                    dog++;
                    if (ob.distance != 0)
                    {
                        if (ob.distance - dist < ob.radius)
                        {
                            
                          double fin =  DistanceAlgorithm.DistanceBetweenPlaces(longi, lati, Double.Parse(result[sno_convert[ob.id]]["Longitude"].ToString()), Double.Parse(result[sno_convert[ob.id]]["Latitude"].ToString()));
                          if (fin < ob.radius)
                          {
                              
                              Sno_initial = ob.id;
                              
                              break;

                              
                          }
                          
                        }

                    }

                }
                
               

            }

         //   System.Threading.Thread.Sleep(500);
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);

            Console.WriteLine("Hello");





            

            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cmd = new MySqlCommand("select * from branchdata where UserName in ('dodla', 'Palamaner')");

            locationinfo = vdm.SelectQuery(cmd);
            DataRow[] resultp = locationinfo.Tables[0].Select("");

            for(int i=0; i< resultp.Length-1; i++)
            {
               
                if(  ( Int32.Parse(resultp[i]["direction"].ToString() ) - Int32.Parse(resultp[i+1]["direction"].ToString()) ) >200 || (Int32.Parse(resultp[i]["direction"].ToString()) - Int32.Parse(resultp[i+1]["direction"].ToString()) )< -200) 
            {


            }

                else
                {



                }




            }  



        }

       
    }

    class obj
    {
        public int id;
        public double distance;
        public int radius;
    };


   
}


class DistanceAlgorithm
{
    const double PIx = 3.141592653589793;
    const double RADIUS = 6378.16;

    /// <summary>
    /// This class cannot be instantiated.
    /// </summary>
    private DistanceAlgorithm() { }

    /// <summary>
    /// Convert degrees to Radians
    /// </summary>
    /// <param name="x">Degrees</param>
    /// <returns>The equivalent in radians</returns>
    public static double Radians(double x)
    {
        return x * PIx / 180;
    }

    /// <summary>
    /// Calculate the distance between two places.
    /// </summary>
    /// <param name="lon1"></param>
    /// <param name="lat1"></param>
    /// <param name="lon2"></param>
    /// <param name="lat2"></param>
    /// <returns></returns>
    public static double DistanceBetweenPlaces(
        double lon1,
        double lat1,
        double lon2,
        double lat2)
    {
        double dlon = Radians(lon2 - lon1);
        double dlat = Radians(lat2 - lat1);

        double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
        double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return angle * RADIUS * 1000;
    }


}
