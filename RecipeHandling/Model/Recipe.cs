using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Jamie.Model
{

    /* Ein Rezept ist für eine Portionsanzahl ausgelegt.
     * Rezepte generieren ein Gesamtmerkmal aus den einzelnen Merkmalen von allen Zutaten
     */
    public class Recipe : IEquatable<Recipe>
    {
        //Variables
        private static IngredientSet _IngredientSetData;
        private static UnitSet _UnitSetData;
        private static UnitTranslationSet _UnitTranslationSetData;


        private long? _ID;

        private RecipeIngredientSet _Ingredients;
        private string _Name;
        private int _PortionQuantity; // Portion min max berücksichtigen
        private string _Source; // Source: Cookbook the recipe is taken from 
        private string _SourceISBN;
        private string _SourcePage; // Page the recipe is found in the cookbook
        private string _Summary; // Summary
                                 //private bool _ToTakeAway;


        //Constructors
        public Recipe()
        {
            _Ingredients = new RecipeIngredientSet(IngredientSetData, UnitSetData, UnitTranslationSetData, this);
        }

        //Properties
        public static IngredientSet IngredientSetData //Readonly
        {
            get
            {
                return _IngredientSetData;
            }
        }
        public static UnitSet UnitSetData //Readonly
        {
            get
            {
                return _UnitSetData;
            }
        }
        public static UnitTranslationSet UnitTranslationSetData
        {
            get
            {
                return _UnitTranslationSetData;
            }
        } //Readonly
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
        public RecipeIngredientSet Ingredients
        {
            get { return _Ingredients; }
            set { _Ingredients = value; }
        }
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
            }
        }
        public int PortionQuantity
        {
            get
            {
                return _PortionQuantity;
            }

            set
            {
                _PortionQuantity = value;
            }
        }
        public string Source
        {
            get
            {
                return _Source;
            }

            set
            {
                _Source = value;
            }
        }
        public string SourceISBN
        {
            get
            {
                return _SourceISBN;
            }

            set
            {
                _SourceISBN = value;
            }
        }
        public string SourcePage
        {
            get
            {
                return _SourcePage;
            }

            set
            {
                _SourcePage = value;
            }
        }
        public string Summary
        {
            get
            {
                return _Summary;
            }

            set
            {
                _Summary = value;
            }
        }

        //Methods
        public bool Equals(Recipe ItemToCompare)
        {
            if (ItemToCompare == null) return false;
            return ID.Equals(ItemToCompare.ID) || EqualKey(ItemToCompare);
        }
        public bool EqualKey(Recipe ItemToCompare)
        {
            return Name.ToUpper().Equals(ItemToCompare.Name.ToUpper());
        }
        public void SetDataReference(IngredientSet IngredientSetData, UnitSet UnitSetData, 
                                     UnitTranslationSet UnitTranslationSetData)
        {
            _IngredientSetData = IngredientSetData;
            _UnitSetData = UnitSetData;
            _UnitTranslationSetData = UnitTranslationSetData;
            _Ingredients.SetDataReference(UnitSetData, IngredientSetData, UnitTranslationSetData);
        }
        public override string ToString()
        {
            return string.Format("{0,6}-Name: {1,10} Portionen: {2,4} Source: {3,5}  Seite: {4,5}  Summary {5,15}", ID, Name, PortionQuantity, Source, SourcePage, Summary);
        }
    }

    public class RecipeSet: ObservableCollection<Recipe>
    {
        //Constants
        private const string FileExtension = ".recp";// --> Data

        //Variables
        private static long _MaxID = 0;
        private static IngredientSet _IngredientSetData;
        private static UnitSet _UnitSetData;
        private static UnitTranslationSet _UnitTranslationSetData;
        private Recipe _SelectedItem;

        //Constructors
        public RecipeSet(UnitSet UnitSetData, IngredientSet IngredientSetData)
        {
            _UnitSetData = UnitSetData;
            _IngredientSetData = IngredientSetData;
        }

        //Properties
        public static UnitSet UnitSetData
        {
            get
            {
                return _UnitSetData;
            }
        } //Readonly
        public static IngredientSet IngredientSetData
        {
            get
            {
                return _IngredientSetData;
            }
        } //Readonly
        public Recipe SelectedItem
        {
            get
            {
                return _SelectedItem;
            }
        } //Readonly
        public static UnitTranslationSet UnitTranslationSetData
        {
            get
            {
                return _UnitTranslationSetData;
            }
        } //Readonly

        //Methods
        public bool AddItem(Recipe ItemToBeAdded)
        {
            bool ReturnValue = true;

            if (!Contains(ItemToBeAdded))
            {
                Add(ItemToBeAdded);
                ItemToBeAdded.ID = ++_MaxID;
            }
            else ReturnValue = false;
            SelectItem(ItemToBeAdded);

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

            SelectedItem.Ingredients.Clear();
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
        public RecipeSet OpenSet(string FileName)
        {
            RecipeSet ReturnSet = this;
            ReturnSet.Clear();
            FileName += FileExtension;
            using (Stream fs = new FileStream(FileName, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(ReturnSet.GetType());
                ReturnSet = (RecipeSet)x.Deserialize(fs);
            }
            EvaluateMaxID();
            return ReturnSet;

        }// --> Data
        public void SaveSet(string BaseFileName)
        {
            string FileName = BaseFileName + FileExtension;
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(GetType());
                x.Serialize(fs, this);
            }

            foreach (Recipe Rcp in this)
            {
                FileName = BaseFileName + "." + Rcp.Name;
                Rcp.Ingredients.SaveSet(FileName);
            }

        }// --> Data
        public void SetDataReference(IngredientSet IngredientSetData, UnitSet UnitSetData, 
                                     UnitTranslationSet UnitTranslationSetData)
        {
            _IngredientSetData = IngredientSetData;
            _UnitSetData = UnitSetData;
            _UnitTranslationSetData = UnitTranslationSetData;
            if (Count > 0) this.ElementAt(0).SetDataReference(IngredientSetData, UnitSetData, UnitTranslationSetData);
        }
        public Recipe SelectItem(int ItemPos)
        {
            Recipe ReturnItem = null;
            if ((ItemPos > -1) && (ItemPos <= Count - 1))
            {
                ReturnItem = this[ItemPos];
                _SelectedItem = ReturnItem;
            }
            return ReturnItem;
        }
        public Recipe SelectItem(Recipe ItemToBeSelected)
        {
            Recipe ReturnValue = null;

            int IndexOfSelectedItem = IndexOf(ItemToBeSelected);
            if (IndexOfSelectedItem != -1) ReturnValue = SelectItem(IndexOfSelectedItem);
            return ReturnValue;
        }
        public Recipe SelectItemByID(long IDToSelect)
        {
            Recipe LocalItemToSelect = new Recipe();
            LocalItemToSelect.ID = IDToSelect;

            int IndexOfSelectedItem = IndexOf(LocalItemToSelect);
            if (IndexOfSelectedItem == -1) return null;
            else return this[IndexOfSelectedItem];

        }


        public override string ToString()
        {
            string ReturnString;

            ReturnString = string.Format("\nListe der Rezepte: MaxID {0}\n", _MaxID);
            if (Count == 0) ReturnString += "-------> leer <-------\n";
            else
            {
                foreach (Recipe ListItem in this)
                {
                    ReturnString += ListItem.ToString() + "\n";
                    ReturnString += ListItem.Ingredients.ToString() + "\n";
                }
            }
            ReturnString += "\n";
            return ReturnString;
        }
    }      
}
