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
        //Methods
        public void AddItem()
        {
            AddItem(new Unit(true));
        }
        public void AddItem(Unit UnitToBeAdded)
        {
            if (!Contains(UnitToBeAdded)) Add(UnitToBeAdded);
            else Console.WriteLine("Die Unit ist bereits vorhanden: \n {0}", UnitToBeAdded);
        }
        public void Menu()
        {
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                Console.WriteLine();
                Console.WriteLine("\nUnit Menü");
                Console.WriteLine("---------");
                Console.WriteLine("A  Add Unit");
                Console.WriteLine("V  View Set");
                Console.WriteLine("--------------------");
                Console.WriteLine("Q  Quit");

                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "A":
                        ViewSet();
                        AddItem();
                        ViewSet();
                        break;
                    case "V":
                        ViewSet();
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }

            }
        }
        public void ViewSet()
        {
            Console.WriteLine();
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


        // Constructors
        internal Unit()
        {
        }
        internal Unit(bool ToBePopulated)
        {
            if (ToBePopulated) PopulateObject();
        }
        internal Unit(string UnitName, string UnitSymbol, string UnitType)
        {
            this.UnitName = UnitName;
            this.UnitSymbol = UnitSymbol;
            this.UnitType = UnitType;
        }

        // Properties
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

        //Methods
        public bool Equals(Unit UnitToCompare)
        {
            return UnitSymbol.Equals(UnitToCompare.UnitSymbol);
        }
        public void PopulateObject()
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



