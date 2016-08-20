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

        private static int maxUnitID = 0;
        private int unitID;
        //private int parentUnit;

        internal Unit()
        {
            unitID = ++maxUnitID;
        }

        internal Unit(bool ToBePopulated)
        {
            unitID = ++maxUnitID;
            if (ToBePopulated) PopulateData();
        }

        internal Unit(string UnitName, string UnitSymbol, string UnitType)
        {
            unitID = ++maxUnitID;
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
            Console.WriteLine("Eingabe neue Einheit: {0,3}", unitID);
            Console.WriteLine("-------------------------");
            Console.WriteLine();
            Console.Write("UnitName  : "); UnitName = Console.ReadLine();
            Console.Write("UnitSymbol: "); UnitSymbol = Console.ReadLine();
            Console.Write("UnitType  : "); UnitType = Console.ReadLine();
        }
        


        public override string ToString()
        {
            return String.Format("Unit {0,5} Name: {1,10}  Symbol: {2,5} Type: {3,10}", unitID, UnitName, UnitSymbol, UnitType);
        }


    }

}



