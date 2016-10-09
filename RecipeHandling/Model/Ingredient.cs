
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;



namespace Jamie.Model
{

    /* IngredientFlags, IngredientType im namespace:
     * können von mehreren Klassen verwendet werden (z.B. Ingredient, Recipe)
     */
    [Flags] public enum IngredientFlags : int
    { IsVegetarian = 1, IsVegan = 2, IsLowCarb = 4, IsLowFat = 8 } 
    public enum IngredientType : int { IsFluid, IsSolid, IsCrystal, IsPowder, IsHerb, IsGranular, NotInitialized = 999 }

    /* Eine Zutat beschreibt ein Produkt, welches in einem Rezept verarbeitet werden kann. Zutaten werden im Gegensatz zu Werkzeugen verbraucht. 
     * Hat Eigenschaften: x kcal/100g, Ernährungsampel (rot, gelb, grün)
     * länderspezifische Zuordnung?
     */

    public class Ingredient : IEquatable<Ingredient> //: ObservableObject
    {
        //Constants

        //static Variables
        private static UnitSet _UnitSetData;

        //Variables
        private long? _ID;
        private IngredientFlags _Flags;
        private IngredientType _Type;
        private string _Name;
        private Unit _TargetUnit;  

        //Constructors
        public Ingredient()
        {
        }
        public Ingredient(UnitSet UnitSetData)
        {
            _UnitSetData = UnitSetData;
        }
        public Ingredient(string Name, IngredientFlags IngredientType, UnitSet UnitSetData)
        {
            _UnitSetData = UnitSetData;
            _Name = Name;
            _Flags = IngredientType;
        }

        //Properties
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
        public IngredientFlags Flags
        {
            get
            {
                return _Flags;
            }

            set
            {
                _Flags = value;
            }
        }
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value)
                    return;

                _Name = value;
                //                RaisePropertyChanged(() => Name);
            }
        }
        public Unit TargetUnit
        {
            get
            {
                return _TargetUnit;
            }

            set
            {
                _TargetUnit = value;
            }
        }
        public IngredientType Type
        {
            get
            {
                return _Type;
            }

            set
            {
                _Type = value;
            }
        }
        public UnitSet UnitSetData
        {
            get
            {
                return _UnitSetData;
            }
        } //ReadOnly


        // Methods
        public bool Equals(Ingredient ItemToCompare)
        {
            if (ItemToCompare == null) return false;
            return ID.Equals(ItemToCompare.ID) || EqualKey(ItemToCompare);
        }
        public bool EqualKey(Ingredient ItemToCompare)
        {
            return Name.ToUpper().Equals(ItemToCompare.Name.ToUpper());
        }
        public static byte GetmaxIngredientFlag()
        {
            byte ReturnValue = 0;

            foreach (IngredientFlags i in Enum.GetValues(typeof(IngredientFlags)))
            {
                ReturnValue += (byte)i;
            }

            return ReturnValue;
        }
        public override string ToString()
        {
            return string.Format("{0,6}-Name: {1,15} Type: {2} Flags: {3}\n\t TargetUnit{4,5}", ID, Name, Type, Flags, TargetUnit);
        }
    }
    public class IngredientSet : ObservableCollection<Ingredient>
    {
        //Constants
        private const string FileExtension = ".ingr"; // --> Data

        //Variables
        private static long _MaxID = 0;
        private Ingredient _SelectedItem; 
        private static UnitSet _UnitSetData; 

        //Constructors
        public IngredientSet(UnitSet UnitSetData)
        {
            _UnitSetData = UnitSetData;
        }

        //Properties
        public Ingredient SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
        }//Readonly
        public static UnitSet UnitSetData
        {
            get
            {
                return _UnitSetData;
            }
        }  //Readonly

        //Methods
        public bool AddItem(Ingredient ItemToBeAdded) 
        {
            bool ReturnValue = true;

            if (!Contains(ItemToBeAdded))
            {
                Add(ItemToBeAdded);
                ItemToBeAdded.ID = ++_MaxID;
                _SelectedItem = SelectItem(ItemToBeAdded.Name);
            }
            else ReturnValue = false;

            return ReturnValue;
        }
        public void DeleteSelectedItem()
        {
            int NewSelectedIndex;

            if ((Count == 0) || (SelectedItem == null)) return;

            if (Count > 1)
            {
                NewSelectedIndex = IndexOf(SelectedItem) - 1;
                if (NewSelectedIndex < 0) NewSelectedIndex = 0;
            }
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
        public Ingredient GetItem(string ItemTextToBeSelected)
        {
            Ingredient LocalItemToSelect = new Ingredient(ItemTextToBeSelected, 0, UnitSetData);

            int IndexOfSelectedUnit = IndexOf(LocalItemToSelect);

            if (IndexOfSelectedUnit == -1) return null;
            else return this[IndexOfSelectedUnit];
        }
        public void PopulateSetWithDefaults()
        {
            IngredientFlags FlagsTobeSet;

            FlagsTobeSet = 0;
            FlagsTobeSet |= IngredientFlags.IsVegan;
            AddItem(new Ingredient("Zwiebeln", IngredientFlags.IsVegetarian
                                             | IngredientFlags.IsVegan
                                             | IngredientFlags.IsLowCarb
                                             | IngredientFlags.IsLowFat,UnitSetData));
            AddItem(new Ingredient("Tomaten",  IngredientFlags.IsVegetarian
                                             | IngredientFlags.IsVegan
                                             | IngredientFlags.IsLowCarb
                                             | IngredientFlags.IsLowFat,UnitSetData));
            AddItem(new Ingredient("Rinderfilet", IngredientFlags.IsLowCarb,UnitSetData));
            AddItem(new Ingredient("Quinoa", IngredientFlags.IsVegetarian
                                           | IngredientFlags.IsVegan
                                           | IngredientFlags.IsLowFat, UnitSetData));
        }
        public void SaveSet(string FileName)
        {
            FileName += FileExtension;
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(GetType());
                x.Serialize(fs, this);
            }

        } // --> Data
        public void SetDataReference(UnitSet UnitSetData)
        {
            _UnitSetData = UnitSetData;
        }
        public Ingredient SelectItem(int ItemPos)
        {
            Ingredient ReturnItem = null;
            if ((ItemPos > -1) && (ItemPos <= Count - 1))
            {
                ReturnItem = this[ItemPos];
                _SelectedItem = ReturnItem;
            }
            return ReturnItem;
        }
        public Ingredient SelectItem(string ItemTextToBeSelected)
        {
            Ingredient ReturnItem = null;
            Ingredient LocalItemToSelect = new Ingredient(ItemTextToBeSelected, 0,UnitSetData);

            int IndexOfSelectedItem = IndexOf(LocalItemToSelect);
            if (IndexOfSelectedItem > -1) ReturnItem = SelectItem(IndexOfSelectedItem);

            return ReturnItem;
        }
        public override string ToString()
        {
            string ReturnString = "";

            ReturnString += string.Format("\nListe der Zutaten: MaxID {0}\n", _MaxID);
            if (Count == 0) ReturnString += "-------> leer <-------\n";
            else
            {
                foreach (Ingredient ListItem in this)
                    ReturnString += ListItem.ToString() + "\n";
            }
            ReturnString += "\n";
            return ReturnString;
        }
    }

}

