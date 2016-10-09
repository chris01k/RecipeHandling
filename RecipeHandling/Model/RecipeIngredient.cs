using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Jamie.Model
{
    /* IngredientItem ist ein (Quanity, Unit, Ingredient) Triple, welches einem Rezept zugeordnet ist
*/
    public class RecipeIngredient  //: ObservableObject
    {
        //Variables
        private long? _ID;

        private double _Quantity;
        private Ingredient _Ingredient;
        private Unit _Unit;


        //Constructors
        public RecipeIngredient()
        {
        }
        public RecipeIngredient(double Quantity, Unit Unit, Ingredient Ingredient)
        {
            _Quantity = Quantity;
            _Unit = Unit;
            _Ingredient = Ingredient;
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
        public Ingredient Ingredient
        {
            get { return _Ingredient; }
            set
            {
                _Ingredient = value;
                //                RaisePropertyChanged(() => SpecificIngredient);
            }
        }
        public string Name
        {
            get { return _Ingredient != null ? _Ingredient.Name : ""; }
            set
            {
                if (_Ingredient.Name == value)
                    return;

                _Ingredient.Name = value;
                //RaisePropertyChanged(() => Name);
            }
        }
        public Unit Unit
        {
            get { return _Unit; }
            set
            {
                _Unit = value;
                //                RaisePropertyChanged(() => Unit);

            }
        }
        public double Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity == value)
                    return;

                _Quantity = value;
                //                RaisePropertyChanged(() => Quantity);

            }
        }


        //Methods
        public override string ToString()
        {
            string ReturnString = string.Format(" {0,8} {1} {2}", Quantity, Unit, Ingredient.Name);

            ReturnString += "\n";
            return ReturnString;
        }
    }

    /* IngredientItemSet ist eine Liste IngredientItems (Menge, Unit, Ingredient):
     * Ein Rezept enthält eine Zutatenliste bestehend aus x Einträgen, wobei jeder Eintrag eine Zutat sowie die 
     * erforderliche Menge beschreibt.
     */
    public class RecipeIngredientSet : ObservableCollection<RecipeIngredient>
    {
        //Constants
        private const string FileExtension = ".rcig"; // --> Data

        //Variables
        private static long _MaxID;

        private static IngredientSet _IngredientSetData;
        private Recipe _RelatedRecipe;
        private static UnitSet _UnitSetData;
        private static UnitTranslationSet _UnitTranslationSetData;


        //Constructors
        public RecipeIngredientSet(IngredientSet IngredientSetData, UnitSet UnitSetData,
                                 UnitTranslationSet UnitTranslationSetData, Recipe RelatedRecipe)
        {
            _IngredientSetData = IngredientSetData;
            _UnitSetData = UnitSetData;
            _UnitTranslationSetData = UnitTranslationSetData;
            _RelatedRecipe = RelatedRecipe;
        }

        //Properties
        public static IngredientSet IngredientSetData
        {
            get
            {
                return _IngredientSetData;
            }
        }//Readonly
        public static long MaxID
        {
            get
            {
                return _MaxID;
            }
        }
        public Recipe RelatedRecipe
        {
            get
            {
                return _RelatedRecipe;
            }
        }//Readonly
        public static UnitSet UnitSetData
        {
            get
            {
                return _UnitSetData;
            }
        }//Readonly
        public static UnitTranslationSet UnitTranslationSetData
        {
            get
            {
                return _UnitTranslationSetData;
            }
        }//Readonly

        //Methods
        public bool AddItem(RecipeIngredient ItemToBeAdded)
        {
            bool ReturnValue = true;

            if (!Contains(ItemToBeAdded))
            {
                Add(ItemToBeAdded);
                ItemToBeAdded.ID = ++_MaxID;
            }
            else ReturnValue = false;

            return ReturnValue;
        }
        public RecipeIngredientSet OpenSet(string FileName)
        {
            RecipeIngredientSet ReturnSet = this;
            ReturnSet.Clear();
            FileName += "." + RelatedRecipe.Name + FileExtension;
            using (Stream fs = new FileStream(FileName, FileMode.Open))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(ReturnSet.GetType());
                ReturnSet = (RecipeIngredientSet)x.Deserialize(fs);
            }
            return ReturnSet;

        }// --> Data
        public void SaveSet(string FileName)
        {
            FileName += "." + RelatedRecipe.Name + FileExtension;
            using (FileStream fs = new FileStream(FileName, FileMode.Create))
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(GetType());
                x.Serialize(fs, this);
            }

        }// --> Data
        public void SetDataReference(UnitSet UnitSetData, IngredientSet IngredientSetData,
                                     UnitTranslationSet UnitTranslationSetData)
        {
            _IngredientSetData = IngredientSetData;
            _UnitSetData = UnitSetData;
            _UnitTranslationSetData = UnitTranslationSetData;
        }
        public override string ToString()
        {
            string ReturnString = "";

            if (Count == 0) ReturnString += "         -------> leer <-------\n         ";
            else
            {
                foreach (RecipeIngredient ListItem in this)
                    //                    ReturnString += ListItem.ToString() + "\n      ";
                    ReturnString += string.Format("{0,6} {1,5} {2,20} {3}  \n", ListItem.Quantity, ListItem.Unit.Symbol, ListItem.Ingredient.Name, RelatedRecipe.Name);
            }
            ReturnString += "\n";
            return ReturnString;
        }
    }
}