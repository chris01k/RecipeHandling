using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.IO;


namespace Jamie.Model
{
    public enum UnitType : int
    { IsCount = 1, IsLength, IsArea, IsVolume, IsWeight, IsTime };

    public class Unit : IEquatable<Unit>
    {

        //Variables
        private long? _ID;
        private string _Symbol; //Key
        private string _Name;
        private UnitType _Type; //Anzahl, Länge, (Fläche), Volumen, Masse, Zeit


        // Constructors
        public Unit()
        {
        }
        public Unit(string Symbol, string Name, UnitType Type)
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
            if (ItemToCompare == null) return false;
            return ID.Equals(ItemToCompare.ID) || EqualKey(ItemToCompare);
        }
        public bool EqualKey(Unit ItemToCompare)
        {
            return Symbol.Equals(ItemToCompare.Symbol);
        }
        public override string ToString()
        {
            return string.Format("{0,6} {1,5} - Name: {2,10}   Type: {3,10}", ID, Symbol, Name, Type);
        }
    }

    public class UnitSet: ObservableCollection<Unit>
    {
        //Constants
        private const string FileExtension = ".unit"; // -> Data

        //static Variables
        private static long _MaxID = 0;
        private Unit _SelectedItem;

            //Variables

            //Constructors
        public UnitSet()
        {
        }

        //Properties
        public static long MaxID //Readonly
        {
            get
            {
                return _MaxID;
            }
        }
        public Unit SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
        } //Readonly

        //Methods
        public bool AddItem(Unit ItemToBeAdded)
        {
            if (!Contains(ItemToBeAdded)) 
            {
                ItemToBeAdded.ID = ++_MaxID;
                Add(ItemToBeAdded);
                _SelectedItem = ItemToBeAdded;
                return true;
            }
            else return false;
        } 
        public void DeleteSelectedItem()
        {
            int NewSelectedIndex;

            if ((Count == 0) || (SelectedItem == null)) return;
            if (Count > 1) NewSelectedIndex = IndexOf(_SelectedItem) - 1;
            else NewSelectedIndex = 1;
            Remove(SelectedItem);
            if (Count > 0) _SelectedItem = this[NewSelectedIndex];
            else _SelectedItem = null;


        }
        public void EvaluateMaxID()
        {
            var maxIDFromFile = this.Select(s => s.ID).Max();

            if (maxIDFromFile == null) _MaxID = 0;
            else _MaxID = (long)maxIDFromFile;
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

            EvaluateMaxID();
           return ReturnUnitSet;

        } // --> Data
        public void SaveSet(string FileName)
        {
            FileName += FileExtension;
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(GetType());
                x.Serialize(fs, this);
            }

        } // --> Data
        public Unit SelectItem(int ItemPos)
        {
            Unit ReturnItem = null;
            if ((ItemPos>-1) && (ItemPos <= Count - 1))
            {
                ReturnItem = this[ItemPos];
                _SelectedItem = ReturnItem;
            }
            return ReturnItem;
        }
        public Unit SelectItem(string ItemTextToBeSelected)
        {
            Unit ReturnItem = null;
            Unit LocalUnitToSelect = new Unit(ItemTextToBeSelected,"", 0);

            int IndexOfSelectedUnit = IndexOf(LocalUnitToSelect);
            if (IndexOfSelectedUnit > -1)  ReturnItem = SelectItem(IndexOfSelectedUnit);
            return ReturnItem;
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
}



