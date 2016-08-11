using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeHandling
{
    public class Unit : IEquatable<Unit>
    {
        private static int maxUnitID=0;
        private int unitID;
        private int parentUnit;

        internal string UnitName { get; set; }
        internal string UnitSymbol { get; set; }
        internal string UnitType { get; set; }
     

        internal Unit()
        {
            unitID = ++maxUnitID;
        }

        internal Unit(string UnitName, string UnitSymbol, string UnitType)
        {
            unitID = ++maxUnitID;
            this.UnitName = UnitName;
            this.UnitSymbol = UnitSymbol;
            this.UnitType = UnitType;
        }

        public  bool Equals(Unit obj)
        {
            //return (UnitName ==(((Unit)obj).UnitName));
            return UnitSymbol.Equals(((Unit)obj).UnitSymbol);
        }

        public override int GetHashCode()
        {
            return unitID * 17;
        }

        public override string ToString()
        {
            return String.Format("Unit {3,5} Name: {0,10}  Symbol: {1,5} Type: {2,10}", UnitName, UnitSymbol, UnitType, unitID);
        }

        
    }

}