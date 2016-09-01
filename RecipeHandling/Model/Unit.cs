using System;
using System.Collections.ObjectModel;

namespace Jamie.Model
{
    public class UnitSet: ObservableCollection<Unit>
    {
//        private static RecipeDataSets _Data;
//        auskommentiert weil umgebaut wird auf spezifische Set-Anforderungen 
//        (es ist nicht erforderlich, dass das RecipeDataSet übergeben wird - stattdessen spezifische Listen)
        private static long _MaxID = 0;

        //Constructors
        public UnitSet(RecipeDataSets Data)
        {
        }

        //Properties
        public static long MaxID
        {
            get
            {
                return _MaxID;
            }
        }

        //Methods
        public void AddItem()
        {
            AddItem(new Unit(true));
        }
        public void AddItem(Unit ItemToBeAdded)
        {
            if (!Contains(ItemToBeAdded)) 
            {
                Add(ItemToBeAdded);
                ItemToBeAdded.ID = ++_MaxID;
            }
            else Console.WriteLine("Die Unit ist bereits vorhanden: \n {0}", ItemToBeAdded);
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
        public Unit SelectUnit()
        {
            string LocalUnitSymbol = "";

            Console.WriteLine("Unit suchen:");
            Console.WriteLine("------------");
            Console.WriteLine();
            Console.Write("UnitSymbol: "); LocalUnitSymbol = Console.ReadLine();

            return SelectUnit(LocalUnitSymbol);

        }
        public Unit SelectUnit(string UnitSymbolToBeSelected)
        {
            string InputString;
            Unit LocalUnitToSelect = new Unit("", UnitSymbolToBeSelected, "");

            int IndexOfSelectedUnit = IndexOf(LocalUnitToSelect);
            if (IndexOfSelectedUnit == -1)
            {
                Console.WriteLine();
                Console.WriteLine("---------------> Unit {0} nicht bekannt <---------------", UnitSymbolToBeSelected);
                Console.WriteLine("Neue Unit eingeben (J/N)?"); InputString = Console.ReadLine();
                if (InputString.ToUpper() == "J")
                {
                    AddItem();
                    IndexOfSelectedUnit = Count - 1;
                }
                else return null;
            }
            return this[IndexOfSelectedUnit];

        }
        public void PopulateSetWithDefaults()
        {
            AddItem(new Unit("Kilogramm", "kg", "Masse"));
            AddItem(new Unit("Gramm", "g", "Masse"));
            AddItem(new Unit("Unze", "oz", "Masse"));
            AddItem(new Unit("Liter", "l", "Volumen"));
            AddItem(new Unit("Stück", "st", "Anzahl"));
            AddItem(new Unit("Milliliter", "ml", "Volumen"));
            AddItem(new Unit("Meter", "m", "Länge"));
        }
        public void ViewSet()
        {
            Console.WriteLine(ToString());
        }
        public override string ToString()
        {
            string ReturnString = "";

            ReturnString += string.Format("\nListe der Units - MaxID: {0}\n", MaxID);
            if (Count == 0) ReturnString += "-------> leer <-------\n";
            else
            {
                foreach (Unit ListItem in this)
                    ReturnString += ListItem.ToString() + "\n";
            }
            ReturnString += "\n";
            return ReturnString;
        }
    }

    public class Unit:IEquatable <Unit>
    {
        //Constants


        //Variables
        private long? _ID;
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
        public long? ID
        {
            get
            {
                return _ID;
            }
            set
            {
                if (_ID == null) _ID = value;
//                else throw exception;
            }
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


        //Methods
        public bool Equals(Unit ItemToCompare)
        {
            return ID.Equals(ItemToCompare.ID) | EqualKey(ItemToCompare);
        }
        public bool EqualKey(Unit ItemToCompare)
        {
            return UnitSymbol.Equals(ItemToCompare.UnitSymbol);
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
            return string.Format("{0,6}-Name: {1,10}  Symbol: {2,5} Type: {3,10}", ID, UnitName, UnitSymbol, UnitType);
        }
    }

}



