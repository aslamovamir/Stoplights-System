using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smarter_Stoplight_Problem
{
    //class for the stoplight objects
    class Stoplight
    {
        //private attribute for the color of the stoplight
        private string color;

        //Method Name: Stoplight
        //Description: this is a constructor that helps in the creation of the object of this class
        //initially, the color of the stoplight is default red
        public Stoplight()
        {
            this.color = "red";
        }

        //Method Name: ChangeColor
        //Description: this is a method to change the attrubute of color of the stoplight object
        public void ChangeColor(string color)
        {
            this.color = color;
        }

        //Method Name: GetColour
        //Description: a getter method to return the color attribute of the object
        public string GetColour()
        {
            return this.color;
        }
    }
}
