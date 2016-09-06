using System;
using System.Collections.ObjectModel;
using System.IO;


namespace Jamie.Model
{
    public enum UnitType : int
    { IsCount = 1, IsLength, IsArea, IsVolume, IsWeight, IsTime };

    public class UnitSet: ObservableCollection<Unit>
    {
        //Variables
        private static long _MaxID = 0;
        private const string FileExtension = ".unit";

        //Constructors
        public UnitSet()
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
        public bool AddItem()
        {
            Unit newItem = new Unit();
            newItem.PopulateObject();
            return AddItem(newItem);
        }
        public bool AddItem(Unit ItemToBeAdded)
        {
            if (!Contains(ItemToBeAdded)) 
            {
                Add(ItemToBeAdded);
                ItemToBeAdded.ID = ++_MaxID;
                return true;
            }
            else Console.WriteLine("Die Unit ist bereits vorhanden: \n {0}", ItemToBeAdded);
            return false;
        }
        public bool KeyItemExists(string KeyUnitSymbol)
        {
            Unit KeyUnit = new Unit("", KeyUnitSymbol, (UnitType) 0);
            return KeyItemExists(KeyUnit);
        }
        public bool KeyItemExists(Unit KeyUnit)
        {
            return (IndexOf(KeyUnit) != -1);
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
        public UnitSet OpenSet(string FileName)
        {
            UnitSet ReturnUnitSet = this;
            ReturnUnitSet.Clear();
            FileName += FileExtension;
            using (Stream fs = new FileStream(FileName, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(ReturnUnitSet.GetType());
                ReturnUnitSet = (UnitSet)x.Deserialize(fs);
            }
            return ReturnUnitSet;

        }
        public void SaveSet(string FileName)
        {
            FileName += FileExtension;
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(GetType());
                x.Serialize(fs, this);
            }

        }
        public Unit SelectItem()
        {
            string LocalUnitSymbol = "";

            Console.WriteLine("Unit suchen:");
            Console.WriteLine("------------");
            Console.WriteLine();
            Console.Write("UnitSymbol: "); LocalUnitSymbol = Console.ReadLine();

            return SelectItem(LocalUnitSymbol);

        }
        public Unit SelectItem(string ItemTextToBeSelected)
        {
            Unit LocalUnitToSelect = new Unit(ItemTextToBeSelected,"", (UnitType)0);

            int IndexOfSelectedUnit = IndexOf(LocalUnitToSelect);
            if (IndexOfSelectedUnit == -1)  return null;
            else return this[IndexOfSelectedUnit];

        }
        public void PopulateSetWithDefaults()
        {
            AddItem(new Unit("kg", "Kilogramm", UnitType.IsWeight));
            AddItem(new Unit("g", "Gramm", UnitType.IsWeight));
            AddItem(new Unit("oz", "Unzen", UnitType.IsWeight));
            AddItem(new Unit("l", "Liter", UnitType.IsVolume));
            AddItem(new Unit("st", "Stück", UnitType.IsCount));
            AddItem(new Unit("ml", "Milliliter", UnitType.IsVolume));
            AddItem(new Unit("m", "Meter", UnitType.IsLength));
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
        private string _Symbol; //Key
        private string _Name;
        private UnitType _Type; //Anzahl, Länge, (Fläche), Volumen, Masse, Zeit


        // Constructors
        internal Unit()
        {
        }
        internal Unit(bool ToBePopulated)
        {
            if (ToBePopulated) PopulateObject();
        }
        internal Unit(string Symbol, string Name,  UnitType Type)
        {
            _Symbol = Symbol;
            _Name = Name;
            _Type = Type;
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
        public string Symbol
        {
            get { return _Symbol; }
            set { _Symbol = value; }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public UnitType Type
        {
            get { return _Type; }
            set { _Type = value; }
        }


        //Methods
        public bool Equals(Unit ItemToCompare)
        {
            return ID.Equals(ItemToCompare.ID) | EqualKey(ItemToCompare);
        }
        public bool EqualKey(Unit ItemToCompare)
        {
            return Symbol.Equals(ItemToCompare.Symbol);
        }
        public void PopulateObject()
        {
            string InputString;

            Console.WriteLine("Eingabe neue Einheit:");
            Console.WriteLine("---------------------");
            Console.WriteLine();
            Console.Write("UnitName  : "); Name = Console.ReadLine();
            Console.Write("UnitSymbol: "); Symbol = Console.ReadLine();
            do
            {
                Console.Write("UnitType  : "); InputString = Console.ReadLine();
                try
                {
                    Type = (UnitType)Enum.Parse(typeof(UnitType), InputString);
                }
                catch
                {
                    continue;
                }
                break;
            } while (true);


        }
        public override string ToString()
        {
            return string.Format("{0,6} {1,5} - Name: {2,10}   Type: {3,10}", ID, Symbol, Name,  Type);
        }
    }

}



