using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Jamie.Model
{
    public class UnitSet: ObservableCollection<Unit>
    {

        public void ShowSet()
        {
            Console.WriteLine("Liste der Units:");
            if (Count == 0) Console.WriteLine("-------> leer <-------");
            else
            {
                foreach (Unit ListItem in this)
                    Console.WriteLine(ListItem);
            }
            Console.WriteLine();
        }

    }



public class Unit:IEquatable <Unit>
    {
        private string _UnitName;
        private string _UnitSymbol;
        private string _UnitType;

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

        public string UnitName
        {
            get { return _UnitName; }
            set { _UnitName = value; }
        }
        public string UnitSymbol
        {
            get { return _UnitSymbol; }
            set { _UnitSymbol = value; }
        }
        public string UnitType
        {
            get { return _UnitType; }
            set { _UnitType = value; }
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



