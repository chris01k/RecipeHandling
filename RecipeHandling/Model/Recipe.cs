﻿using System;
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

        private IngredientItemSet _Ingredients;
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
            _Ingredients = new IngredientItemSet(IngredientSetData, UnitSetData, UnitTranslationSetData, this);
        }
        public Recipe(bool ToBePopulated)
        {
            _Ingredients = new IngredientItemSet(IngredientSetData, UnitSetData, UnitTranslationSetData, this);
            if (ToBePopulated) PopulateObject();
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
        public IngredientItemSet Ingredients
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
            return ID.Equals(ItemToCompare.ID) | EqualKey(ItemToCompare);
        }
        public bool EqualKey(Recipe ItemToCompare)
        {
            return Name.ToUpper().Equals(ItemToCompare.Name.ToUpper());
        }
        public void PopulateObject()
        {
            string InputString;
            int ParsedIntValue;

            Console.Write("Name : "); Name = Console.ReadLine();
            do
            {
                Console.Write("PortionQuantity : "); InputString = Console.ReadLine();
            } while (!int.TryParse(InputString, out ParsedIntValue));
            PortionQuantity = ParsedIntValue;
            Console.Write("Summary         : "); Summary = Console.ReadLine();
            Console.Write("Source          : "); Source = Console.ReadLine();
            Console.Write("SourcePage      : "); SourcePage = Console.ReadLine();
            Console.Write("SourceISBN      : "); SourceISBN = Console.ReadLine();

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
        private const string FileExtension = ".recp";

        //Variables
        private static long _MaxID = 0;
        private static IngredientSet _IngredientSetData;
        private static UnitSet _UnitSetData;
        private static UnitTranslationSet _UnitTranslationSetData;
        private static Recipe _SelectedItem;

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
            //set
            //{
            //    _UnitSetData = value;
            //}
        } //Readonly
        public static IngredientSet IngredientSetData
        {
            get
            {
                return _IngredientSetData;
            }

            //set
            //{
            //    _IngredientSetData = value;
            //}
        } //Readonly
        public static Recipe SelectedItem
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
        public void AddItem()
        {
            Recipe NewRecipe = new Recipe(true);

            if (Count == 0)
            {
                NewRecipe.SetDataReference(_IngredientSetData, _UnitSetData, _UnitTranslationSetData);
            }
            AddItem(NewRecipe);
        }
        public void AddItem(Recipe ItemToBeAdded)
        {
            if (!Contains(ItemToBeAdded))
            {
                Add(ItemToBeAdded);
                ItemToBeAdded.ID = ++_MaxID;
            }
            else Console.WriteLine("Das Rezept ist bereits vorhanden: \n {0}", ItemToBeAdded);
            _SelectedItem = SelectItem(ItemToBeAdded);
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
//            var maxIDFromFile = (from s in this select s.ID).Max();

            var maxIDFromFile = this
                                .Select(s => s.ID).Max();

            if (maxIDFromFile == null) _MaxID = 0;
            else _MaxID = (long)maxIDFromFile;
        }
        public void Menu()
        {
            string MenuInput = "";

            _SelectedItem = SelectItem(false);
            while (MenuInput != "Q")
            {

                ViewSet();
                Console.WriteLine();
                Console.WriteLine("\nRecipe Menü");
                Console.WriteLine(SelectedItem);
                Console.WriteLine("---------------");
                Console.WriteLine("A  Add Recipe");
                Console.WriteLine("D  Delete Selected Recipe");
                Console.WriteLine("I  Add Ingredient");
                Console.WriteLine("R  View Recipe");
                Console.WriteLine("S  Select Recipe");
                Console.WriteLine("V  View Set");
                Console.WriteLine("--------------------");
                Console.WriteLine("Q  Quit");
                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();
                switch (MenuInput)
                {
                    case "A":
                        AddItem();
                        break;
                    case "D":
                        DeleteSelectedItem();
                        break;
                    case "I":
                        SelectedItem.Ingredients.AddItem();
                        Console.WriteLine(SelectedItem.ToString());
                        break;
                    case "R":
                        Console.WriteLine(SelectedItem.ToString());
                        break;
                    case "S":
                        _SelectedItem = SelectItem(true);
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

        }
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

        }
        public void SetDataReference(IngredientSet IngredientSetData, UnitSet UnitSetData, 
                                     UnitTranslationSet UnitTranslationSetData)
        {
            _IngredientSetData = IngredientSetData;
            _UnitSetData = UnitSetData;
            _UnitTranslationSetData = UnitTranslationSetData;
            if (Count > 0) this.ElementAt(0).SetDataReference(IngredientSetData, UnitSetData, UnitTranslationSetData);
        }
        public Recipe SelectItem(bool ByRequest)
        {
            Recipe ReturnValue = null;

            if (ByRequest)
            {
                Recipe RequestedItem = new Recipe();

                Console.WriteLine("Recipe suchen:");
                Console.WriteLine("--------------");
                Console.WriteLine();
                Console.Write("Recipe Name: "); RequestedItem.Name = Console.ReadLine();

                ReturnValue = SelectItem(RequestedItem);
            }
            else if (Count != 0) ReturnValue = this[0];

            return ReturnValue;

        }
        public Recipe SelectItem(Recipe ItemToBeSelected)
        {
            Recipe ReturnValue = null;

            int IndexOfSelectedItem = IndexOf(ItemToBeSelected);
            if (IndexOfSelectedItem != -1) ReturnValue = this[IndexOfSelectedItem];
            return ReturnValue;
        }

        public void ViewSet()
        {
            Console.WriteLine(ToString());
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
