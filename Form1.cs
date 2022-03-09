using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;


//
// COP 4365 - Spring 2022
//
// Homework #2: A Smarter Stoplight Problem
//
// Description: This program is designed to simulate the working system of a 4-way stoplight: North, South, East, and West. This
//              utilizes the system library of System.Diagnostics to make use of a stopwatch, which allows the program to automatically
//              run without human intervention. This program is a Windows Forms application with a GUI that shows the stoplights and correspondent
//              color changes when the stoplights change. The GUI aslo contains a button that is handled by a method that triggers the whole system and
//              calls other helper methods to carry the system out.
//
// File Name: Smarter_Stoplight_Problem
//
// By: Amir Aslamov
//
//

namespace Smarter_Stoplight_Problem
{
    public partial class Form1 : Form
    {
        // Method Name: Form1
        // Description: this is a constructor that initializes the form application
        public Form1()
        {
            InitializeComponent();
        }

        //we create a stopwatch object that will determine the overall running time
        //of the program we will create a stopwatch object and a timespan object
        //outside to make them global
        Stopwatch stopwatch_overall = new Stopwatch();
        TimeSpan time_span_overall;

        //we also create 4 stoplight objects
        Stoplight north_stoplight = new Stoplight();
        Stoplight south_stoplight = new Stoplight();
        Stoplight east_stoplight = new Stoplight();
        Stoplight west_stoplight = new Stoplight();
        

        // Method Name: CycleNorth
        // Description: this method simulates the cycle of the colors of the north stoplight:
        // it creates a seperate stopwatch object and correspondingly changes the labels, color properties
        // of the picture boxes and the color attribute of the north stoplight object
        public void CycleNorth()
        {
            //we create a seperate timer for this stoplight
            Stopwatch north_watch = new Stopwatch();
            //we start the timer
            north_watch.Start();
            //we get the timspan of seconds elapsed
            TimeSpan ts_north = north_watch.Elapsed;

            //first the stoplight has to be green for 9 seconds
            //so we change the backcolor of the picture box
            North_Green_PX.BackColor = Color.Green;
            //the rest of the lights will turn to gray color
            North_Red_PX.BackColor = Color.Gray;
            North_Yellow_PX.BackColor = Color.Gray;

            //refresh the GUI
            Refresh();

            //we also change the color property of the north stopligh object
            north_stoplight.ChangeColor("Green");

            //we print the changes to the console
            Console.WriteLine("{0, 3} {1, -2} {2, 9} {3, -13} {4, -14} {5, -16} {6, -13}", " ", 
                time_span_overall.Seconds.ToString(), " ", north_stoplight.GetColour(), south_stoplight.GetColour(),
                east_stoplight.GetColour(), west_stoplight.GetColour());

            //this boolean variable will help us to determine if there has been an mergency state request from this method
            bool emergency_requested = false;

            //green for 9 seconds, yellow for 3 seconds, the rest is red, we technically need 
            //the timing for the green and yellow colors, and the red color is just a default color
            //after a total of 12 seconds
            //but after 6 seconds this function calls the function for the south stoplight cycle
            //we will change the north light color inside that south stoplight cycle function
            while (ts_north.Seconds < 6)
            {
                //we keep getting the seconds elapsed from the overall timer
                time_span_overall = stopwatch_overall.Elapsed;
                //if 1 minute has passed in the overall stopwatch, we terminate the program
                if (time_span_overall.Minutes == 1)
                {
                    Environment.Exit(0);
                }

                //if 35 seconds have passed in the overall stopwatch, we call the emergency support method
                if (time_span_overall.Seconds == 35)
                {
                    emergency_requested = true;
                    EmergencySupport();
                }

                //we keep dynamically changing the text value of the timer label
                Timer_LBL.Text = time_span_overall.Seconds.ToString();
                Refresh();

                //we get the seconds elapsed in the timer for the north stoplight
                ts_north = north_watch.Elapsed;

                //if an emergency has been called, we break out of this loop
                if (emergency_requested)
                {
                    break;
                }
            }
            //we stop the watch for the north light
            north_watch.Stop();

            //if there has been an emergency request, we do not continue with the prevous cycle of this method:
            //we need to carry up from the state: Red Red Green Red, which is the state of system when East light is green 
            //for 6 seconds and after that it calls the west light cycle making it green too
            //if there has been an emergency, we, instead of completing the prevous cycle, will call the east cycle method
            if (emergency_requested)
            {
                CycleEast();
            }
            //if no emergency has been called, we proceed with the current cycle
            else
            {
                //if 6 seconds have passed, we trigger the south stoplight cycle
                CycleSouth();
            }
        }


        // Method Name: CycleSouth
        // Description: this method simulates the cycle of the colors of the south stoplight:
        // it creates a seperate stopwatch object and correspondingly changes the labels, color properties
        // of the picture boxes and the color attribute of the south stoplight object
        public void CycleSouth()
        {
            //we create a seperate timer for this stoplight
            Stopwatch south_watch = new Stopwatch();
            //we start the timer
            south_watch.Start();
            //we get the timspan of seconds elapsed
            TimeSpan ts_south = south_watch.Elapsed;

            //first the stoplight has to be green for 9 seconds
            //so we change the backcolor of the picture box
            South_Green_PX.BackColor = Color.Green;
            //the rest of the lights will turn to gray color
            South_Red_PX.BackColor = Color.Gray;
            South_Yellow_PX.BackColor = Color.Gray;

            //refresh the GUI
            Refresh();
            //we also change the color property of the south stopligh object
            south_stoplight.ChangeColor("Green");

            //we print the changes to the console
            Console.WriteLine("{0, 3} {1, -2} {2, 9} {3, -13} {4, -14} {5, -16} {6, -13}", " ", 
                time_span_overall.Seconds.ToString(), " ", north_stoplight.GetColour(), south_stoplight.GetColour(),
                east_stoplight.GetColour(), west_stoplight.GetColour());

            //because the stopwatch is slower than the execution time of the program, we need to make sure we don't repeatedly print 
            //the same output inisde the while loop, so we will create helper boolean variables to help us with printing the changes once
            bool north_light_changed_yellow = false;
            bool north_light_changed_red = false;
            bool south_light_changed = false;

            //this boolean variable will help us determine if an emergency support has been called
            bool emergency_called = false;

            //green for 9 seconds, yellow for 3 seconds, the rest is red, we technically need 
            //the timing for the green and yellow colors, and the red color is just a default color
            //after a total of 12 seconds
            while (ts_south.Seconds < 12)
            {
                //we keep getting the seconds elapsed from the overall timer
                time_span_overall = stopwatch_overall.Elapsed;
                //if 1 minute has passed in the overall stopwatch, we terminate the program
                if (time_span_overall.Minutes == 1)
                {
                    Environment.Exit(0);
                }

                //if 35 seconds have passed in the overall stopwatch, we call the emergency support method
                if (time_span_overall.Seconds == 35)
                {
                    emergency_called = true;
                    EmergencySupport();
                }

                //we keep dynamically changing the text value of the timer label
                Timer_LBL.Text = time_span_overall.Seconds.ToString();
                Refresh();

                //if the emergency has been called, we stop the prevous cycle and thus break the loop
                if (emergency_called)
                {
                    break;
                }

                //we get the seconds elapsed in the timer for the north stoplight
                ts_south = south_watch.Elapsed;

                //if the timer for the south stoplight is 2, it means 3 seconds have passed, which means
                //a total of 9 seconds have passed for the north stoplight stopwatch: we change the color to yellow
                if (ts_south.Seconds == 3)
                {
                    if(!north_light_changed_yellow)
                    {
                        north_stoplight.ChangeColor("Yellow");
                        //we turn on the yellow light
                        North_Yellow_PX.BackColor = Color.Yellow;
                        //the rest of the lights will turn to gray color
                        North_Red_PX.BackColor = Color.Gray;
                        North_Green_PX.BackColor = Color.Gray;
                        Refresh();

                        //we print the changes to the console
                        Console.WriteLine("{0, 3} {1, -2} {2, 9} {3, -13} {4, -14} {5, -16} {6, -13}", " ",
                            time_span_overall.Seconds.ToString(), " ", north_stoplight.GetColour(), south_stoplight.GetColour(),
                            east_stoplight.GetColour(), west_stoplight.GetColour());
                        //set the boolean
                        north_light_changed_yellow = true;
                    }
                }
                //else if it is 5, 6 seconds have passed - total of 12 seconds for the north stoplight stopwatch: 
                //we change north light color to red
                else if (ts_south.Seconds == 6)
                {                  
                    if (!north_light_changed_red)
                    {
                        //we leave the stoplight at the default color of red
                        north_stoplight.ChangeColor("Red");
                        North_Red_PX.BackColor = Color.Red;
                        //we turn the rest of the light to the gray color
                        North_Green_PX.BackColor = Color.Gray;
                        North_Yellow_PX.BackColor = Color.Gray;
                        Refresh();

                        //we print the changes to the console
                        Console.WriteLine("{0, 3} {1, -2} {2, 9} {3, -13} {4, -14} {5, -16} {6, -13}", " ", 
                            time_span_overall.Seconds.ToString(), " ", north_stoplight.GetColour(), south_stoplight.GetColour(),
                            east_stoplight.GetColour(), west_stoplight.GetColour());
                        //set the boolean
                        north_light_changed_red = true;
                    }
                }
                //if 9 seconds have passed, we change the stoplight color to yellow
                else if (ts_south.Seconds == 9)
                {                
                    if (!south_light_changed)
                    {
                        south_stoplight.ChangeColor("Yellow");
                        //we turn on the yellow light
                        South_Yellow_PX.BackColor = Color.Yellow;
                        //we turn the rest of the colors to gray color
                        South_Red_PX.BackColor = Color.Gray;
                        South_Green_PX.BackColor = Color.Gray;
                        Refresh();

                        //we print the changes to the console
                        Console.WriteLine("{0, 3} {1, -2} {2, 9} {3, -13} {4, -14} {5, -16} {6, -13}", " ",
                            time_span_overall.Seconds.ToString(), " ", north_stoplight.GetColour(), south_stoplight.GetColour(),
                            east_stoplight.GetColour(), west_stoplight.GetColour());
                        //set the boolean
                        south_light_changed = true;
                    }
                }
            }

            //if the emergency has been called, we just finish with this method, because anyways after this cycle, east light cycle will get triggered in the start button method
            if (emergency_called)
            {
                ;
            } 
            else
            {
                //we leave the stoplight at the default color of red
                south_stoplight.ChangeColor("Red");
                South_Red_PX.BackColor = Color.Red;
                //we turn the rest of the light to gray color
                South_Green_PX.BackColor = Color.Gray;
                South_Yellow_PX.BackColor = Color.Gray;
                Refresh();

                //stop the stopwatch for the south light
                south_watch.Stop();
            }
        }


        // Method Name: CycleEast
        // Description: this method simulates the cycle of the colors of the east stoplight:
        // it creates a seperate stopwatch object and correspondingly changes the labels, color properties
        // of the picture boxes and the color attribute of the south stoplight object
        public void CycleEast()
        {
            //we create a seperate timer for this stoplight
            Stopwatch east_watch = new Stopwatch();
            //we start the timer
            east_watch.Start();
            //we get the timspan of seconds elapsed
            TimeSpan ts_east = east_watch.Elapsed;

            //first the stoplight has to be green for 9 seconds
            //so we change the backcolor of the picture box
            East_Green_PX.BackColor = Color.Green;
            //the rest of the colors of the stoplight have to be gray
            East_Red_PX.BackColor = Color.Gray;
            East_Yellow_PX.BackColor = Color.Gray;

            //refresh the GUI
            Refresh();

            //we also change the color property of the south stopligh object
            east_stoplight.ChangeColor("Green");

            //we print the changes to the console
            Console.WriteLine("{0, 3} {1, -2} {2, 9} {3, -13} {4, -14} {5, -16} {6, -13}", " ", 
                time_span_overall.Seconds.ToString(), " ", north_stoplight.GetColour(), south_stoplight.GetColour(), 
                east_stoplight.GetColour(), west_stoplight.GetColour());

            //this boolean variable will help us determine if the emergency support method has been called
            bool emergency_called = false;

            //green for 9 seconds, yellow for 3 seconds, the rest is red, we technically need 
            //the timing for the green and yellow colors, and the red color is just a default color
            //after a total of 12 seconds
            //but after 6 seconds this function calls the function for the west stoplight cycle
            //we will change the east light color inside that west stoplight cycle function
            while (ts_east.Seconds < 6)
            {
                //we keep getting the seconds elapsed from the overall timer
                time_span_overall = stopwatch_overall.Elapsed;
                //if 1 minute has passed in the overall stopwatch, we terminate the program
                if (time_span_overall.Minutes == 1)
                {
                    Environment.Exit(0);
                }

                //if 35 seconds have passed in the overall stopwatch, we call the emergency support method
                if (time_span_overall.Seconds == 35)
                {
                    emergency_called = true;
                    EmergencySupport();
                }

                //we keep dynamically changing the text value of the timer label
                Timer_LBL.Text = time_span_overall.Seconds.ToString();
                Refresh();

                //we get the seconds elapsed in the timer for the north stoplight
                ts_east = east_watch.Elapsed;

                //if the emergency method has been called, we break out of this loop
                if (emergency_called)
                {
                    break;
                }
            }

            //if emergency method has been called, we recursively call the east cycle method
            if (emergency_called)
            {
                CycleEast();
            }
            else
            {
                //we stop the stopwatch for the east light
                east_watch.Stop();
                //if 6 seconds have passed, we trigger the west stoplight cycle
                CycleWest();
            }
        }


        // Method Name: CycleWest
        // Description: this method simulates the cycle of the colors of the west stoplight:
        // it creates a seperate stopwatch object and correspondingly changes the labels, color properties
        // of the picture boxes and the color attribute of the south stoplight object
        public void CycleWest()
        {
            //we create a seperate timer for this stoplight
            Stopwatch west_watch = new Stopwatch();
            //we start the timer
            west_watch.Start();
            //we get the timspan of seconds elapsed
            TimeSpan ts_west = west_watch.Elapsed;

            //first the stoplight has to be green for 9 seconds
            //so we change the backcolor of the picture box
            West_Green_PX.BackColor = Color.Green;
            //the rest of the colors of the stoplight have to be gray color
            West_Red_PX.BackColor = Color.Gray;
            West_Yellow_PX.BackColor = Color.Gray;

            //refresh the GUI
            Refresh();

            //we also change the color property of the south stopligh object
            west_stoplight.ChangeColor("Green");

            //we print the changes to the console
            Console.WriteLine("{0, 3} {1, -2} {2, 9} {3, -13} {4, -14} {5, -16} {6, -13}", " ", 
                time_span_overall.Seconds.ToString(), " ", north_stoplight.GetColour(), south_stoplight.GetColour(), 
                east_stoplight.GetColour(), west_stoplight.GetColour());

            //because the stopwatch is slower than the execution time of the program, we need to make sure we don't repeatedly print 
            //the same output inisde the while loop, so we will create helper boolean variables to help us with printing the changes once
            bool east_light_changed_yellow = false;
            bool east_light_changed_red = false;
            bool west_light_changed = false;

            //this boolean variable will help us determine if the emergency support method has been called
            bool emergency_called = false;

            //green for 9 seconds, yellow for 3 seconds, the rest is red, we technically need 
            //the timing for the green and yellow colors, and the red color is just a default color
            //after a total of 12 seconds
            while (ts_west.Seconds < 12)
            {
                //we keep getting the seconds elapsed from the overall timer
                time_span_overall = stopwatch_overall.Elapsed;
                //if 1 minute has passed in the overall stopwatch, we terminate the program
                if (time_span_overall.Minutes == 1)
                {
                    Environment.Exit(0);
                }

                //if 35 seconds have passed in the overall stopwatch, we call the emergency support method
                if (time_span_overall.Seconds == 35)
                {
                    emergency_called = true;
                    EmergencySupport();
                }

                //we keep dynamically changing the text value of the timer label
                Timer_LBL.Text = time_span_overall.Seconds.ToString();
                Refresh();

                //if the emergency method has been called, we break this loop
                if (emergency_called)
                {
                    break;
                }

                //we get the seconds elapsed in the timer for the north stoplight
                ts_west = west_watch.Elapsed;

                //if the timer for the west stoplight is 2, it means 3 seconds have passed, which means
                //a total of 9 seconds have passed for the east stoplight stopwatch: we change the color to yellow
                if (ts_west.Seconds == 3)
                {              
                    if (!east_light_changed_yellow)
                    {
                        east_stoplight.ChangeColor("Yellow");
                        //we change turn the yellow light
                        East_Yellow_PX.BackColor = Color.Yellow;
                        //the rest of the colors will be gray
                        East_Green_PX.BackColor = Color.Gray;
                        East_Red_PX.BackColor = Color.Gray;
                        Refresh();

                        //we print the changes to the console
                        Console.WriteLine("{0, 3} {1, -2} {2, 9} {3, -13} {4, -14} {5, -16} {6, -13}", " ", 
                            time_span_overall.Seconds.ToString(), " ", north_stoplight.GetColour(),
                            south_stoplight.GetColour(), east_stoplight.GetColour(), west_stoplight.GetColour());
                        //set the boolean
                        east_light_changed_yellow = true;
                    }
                }
                //else if it is 5, 6 seconds have passed - total of 12 seconds for the east stoplight stopwatch: 
                //we change east light color to red
                else if (ts_west.Seconds == 6)
                {
                    if (!east_light_changed_red)
                    {
                        //we leave the stoplight at the default color of red
                        east_stoplight.ChangeColor("Red");
                        East_Red_PX.BackColor = Color.Red;
                        //the rest turn to gray
                        East_Yellow_PX.BackColor = Color.Gray;
                        East_Green_PX.BackColor = Color.Gray;
                        Refresh();

                        //we print the changes to the console
                        Console.WriteLine("{0, 3} {1, -2} {2, 9} {3, -13} {4, -14} {5, -16} {6, -13}", " ",
                            time_span_overall.Seconds.ToString(), " ", north_stoplight.GetColour(),
                            south_stoplight.GetColour(), east_stoplight.GetColour(), west_stoplight.GetColour());
                        //set the boolean
                        east_light_changed_red = true;
                    }
                }
                //if 9 seconds have passed, we change the stoplight color to yellow
                else if (ts_west.Seconds == 9)
                {
                    if (!west_light_changed)
                    {
                        west_stoplight.ChangeColor("Yellow");
                        //we turn on the yellow light 
                        West_Yellow_PX.BackColor = Color.Yellow;
                        //the rest of the lights will turn to gray
                        West_Red_PX.BackColor = Color.Gray;
                        West_Green_PX.BackColor = Color.Gray;
                        Refresh();

                        //we print the changes to the console
                        Console.WriteLine("{0, 3} {1, -2} {2, 9} {3, -13} {4, -14} {5, -16} {6, -13}", " ", 
                            time_span_overall.Seconds.ToString(), " ", north_stoplight.GetColour(),
                            south_stoplight.GetColour(), east_stoplight.GetColour(), west_stoplight.GetColour());
                        //set the boolean
                        west_light_changed = true;
                    }
                }
            }

            //if the emergency method has been called, we reverse the cycle and call the east cycle method
            if (emergency_called)
            {
                CycleEast();
            }
            //otherwise, we carry on
            else
            {
                //we leave the stoplight at the default color of red
                west_stoplight.ChangeColor("Red");
                West_Red_PX.BackColor = Color.Red;
                //the rest of the lights till turn to gray color
                West_Green_PX.BackColor = Color.Gray;
                West_Yellow_PX.BackColor = Color.Gray;
                Refresh();

                //we stop the stopwatch for the west light
                west_watch.Stop();
            }
        }


        // Method Name: EmergencySupport
        // Description: this method will prioritze the east stoplight and turn it green while the rest
        // of the stoplights to red for a total of 10 seconds (from 35 seconds to 45 seconds of overall timer)
        public void EmergencySupport()
        {
            //we change the text value of the status label to let know the system is on emergency from the east
            Status_LBL.Text = "Emergency on East";

            //we turn the east traffic light green, while the rest are red
            east_stoplight.ChangeColor("Green");
            north_stoplight.ChangeColor("Red");
            south_stoplight.ChangeColor("Red");
            west_stoplight.ChangeColor("Red");

            //we change the GUI correspondingly
            East_Green_PX.BackColor = Color.Green;
            East_Red_PX.BackColor = Color.Gray;
            East_Yellow_PX.BackColor = Color.Gray;

            North_Green_PX.BackColor = Color.Gray;
            North_Red_PX.BackColor = Color.Red;
            North_Yellow_PX.BackColor = Color.Gray;

            South_Green_PX.BackColor = Color.Gray;
            South_Red_PX.BackColor = Color.Red;
            South_Yellow_PX.BackColor = Color.Gray;

            West_Green_PX.BackColor = Color.Gray;
            West_Red_PX.BackColor = Color.Red;
            West_Yellow_PX.BackColor = Color.Gray;

            Refresh();

            //we print the changes to the console
            Console.WriteLine("\nAn emergency vehicle has been detected coming from the East");
            Console.WriteLine("{0, 3} {1, -2} {2, 9} {3, -13} {4, -14} {5, -16} {6, -13}", " ", 
                time_span_overall.Seconds.ToString(), " ", north_stoplight.GetColour(), 
                south_stoplight.GetColour(), east_stoplight.GetColour(), west_stoplight.GetColour());

            //so we keep the state of colors above until 45 seconds have passed in the overall timer
            while (time_span_overall.Seconds < 45)
            {
                //we also keep getting the seconds elapsed from the overall timer
                time_span_overall = stopwatch_overall.Elapsed;
                //if 1 minute has passed in the overall stopwatch, we terminate the program
                if (time_span_overall.Minutes == 1)
                {
                    Environment.Exit(0);
                }
                //we keep dynamically changing the text value of the timer label
                Timer_LBL.Text = time_span_overall.Seconds.ToString();
                Refresh();
            }

            //we print the changes to the console
            Console.WriteLine("\nThe emergency vehicle has left the area.");

            //we change the text value of the status label back to regular on
            Status_LBL.Text = "On Regular Cycle";
        }


        // Method Name: Start_BTN_Click
        // Description: this is an event handler method for the start button, when the user clicks on it
        // this method starts off the system, causes the stoplights to change colors and calls other helper functions
        // to cause the progression of the cycles inside the system
        private void Start_BTN_Click(object sender, EventArgs e)
        {
            //we change the text value of the status label to display the system is on
            Status_LBL.Text = "On Regular Cycle";

            //we trigger the global timer
            stopwatch_overall.Start();
            time_span_overall = stopwatch_overall.Elapsed;

            //we know that initially the north light's color is green, while that of the rest is red
            north_stoplight.ChangeColor("Green");
            south_stoplight.ChangeColor("Red");
            west_stoplight.ChangeColor("Red");
            east_stoplight.ChangeColor("Red");

            //we print the initial setup
            Console.WriteLine("\n\n{0} {1, 12} {2, 12} {3, 15} {4, 15}", "Current Time", "North Light", "South Light", 
                "East Light", "West Light");
            Console.WriteLine("{0} {1, 12} {1, 12} {2, 15} {3, 15}", "____________", "___________", "___________", 
                "__________", "__________");

            //The idea of the program:
            //CYCLE:
            //NORTH starts GREEN
            //After 6 seconds => SOUTH turns GREEN
            //After SOUTH turns RED => EAST turns GREEN
            //After 6 seconds => WEST turns GREEN
            //After WEST turns RED => NORTH starts GREEN

            //we will keep running the system while 60 seconds are not passed
            //so we keep running until 1 minute has not elapsed
            while (time_span_overall.Minutes < 1)
            {
                time_span_overall = stopwatch_overall.Elapsed;
                //we change the text value of the timer label to display the seconds elapsed dynamically
                Timer_LBL.Text = time_span_overall.Seconds.ToString();
                //refresh the GUI
                Refresh();

                //the cycle starts from the north stoplight
                //the north stoplight, after 6 seconds, triggers the south stoplight
                //once the control returns to this function after the 2 calls of the helper
                //functions, we know that the south stoplight finished its cycle, so we start
                //the east stoplight cycle, which in turn triggers the west stoplight after 6 seconds
                CycleNorth();

                //if 1 minute has passed, we don't want to call the CycleEast method
                if (time_span_overall.Minutes == 1)
                {
                    break;
                }
                CycleEast();
            }
            //we stop the global stopwatch of the overall program
            stopwatch_overall.Stop();
        }
    }
}
