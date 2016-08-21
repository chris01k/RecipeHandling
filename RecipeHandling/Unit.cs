using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RecipeHandling
{
    public class Unit:IEquatable <Unit>
    {
        public string UnitName { get; set; }
        public string UnitSymbol { get; set; }
        public string UnitType { get; set; }


        internal Unit()
        {
        }

        internal Unit(bool ToBePopulated)
        {
            if (ToBePopulated) PopulateData();
        }

        internal Unit(string UnitName, string UnitSymbol, string UnitType)
        {
            this.UnitName = UnitName;
            this.UnitSymbol = UnitSymbol;
            this.UnitType = UnitType;
        }

        

        public bool Equals(Unit unitToCompare)
        {
            return UnitSymbol.Equals(unitToCompare.UnitSymbol);
        }


        public void PopulateData()
        {
            Console.WriteLine("Eingabe neue Einheit:");
            Console.WriteLine("---------------------");
            Console.WriteLine();
            Console.Write("UnitName  : "); UnitName = Console.ReadLine();
            Console.Write("UnitSymbol: "); UnitSymbol = Console.ReadLine();
            Console.Write("UnitType  : "); UnitType = Console.ReadLine();
        }
        


        public override string ToString()
        {
            return String.Format("Name: {0,10}  Symbol: {1,5} Type: {2,10}", UnitName, UnitSymbol, UnitType);
        }


    }

}



