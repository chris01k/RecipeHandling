using System;
using System.Collections.ObjectModel;

namespace Jamie.Model
{
    public class RecipeSet: ObservableCollection<Recipe>
    {
        private static UnitSet _UnitSetData;
        private static IngredientSet _IngredientSetData;
        private static Recipe _SelectedRecipe;
        private static long _MaxID = 0;


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


        //Methods
        public void AddItem()
        {
            Recipe NewRecipe = new Recipe(true);

            if (Count == 0)
            {
                NewRecipe.SetDataReference(_UnitSetData, _IngredientSetData);
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
            _SelectedRecipe = SelectItem(ItemToBeAdded);
        }
        public bool IsEmpty()
        {
            return (Count == 0);
        }
        public void Menu()
        {
            string MenuInput = "";

            _SelectedRecipe = SelectItem(false);
            while (MenuInput != "Q")
            {

                ViewSet();
                Console.WriteLine();
                Console.WriteLine("\nRecipe Menü");
                Console.WriteLine(_SelectedRecipe);
                Console.WriteLine("---------------");
                Console.WriteLine("A  Add Recipe");
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
                    case "S":
                        _SelectedRecipe = SelectItem(true);
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
        //public void PopulateSetWithDefaults()
        //{
        //    Ingredient.IngredientFlags FlagsTobeSet;

        //    FlagsTobeSet = 0;
        //    FlagsTobeSet |= Ingredient.IngredientFlags.IsVegan;
        //    AddItem(new Ingredient("Zwiebeln", Ingredient.IngredientFlags.IsVegetarian
        //                                     | Ingredient.IngredientFlags.IsVegan
        //                                     | Ingredient.IngredientFlags.IsLowCarb
        //                                     | Ingredient.IngredientFlags.IsLowFat));
        //    AddItem(new Ingredient("Tomaten", Ingredient.IngredientFlags.IsVegetarian
        //                                     | Ingredient.IngredientFlags.IsVegan
        //                                     | Ingredient.IngredientFlags.IsLowCarb
        //                                     | Ingredient.IngredientFlags.IsLowFat));
        //    AddItem(new Ingredient("Rinderfilet", Ingredient.IngredientFlags.IsLowCarb));
        //    AddItem(new Ingredient("Quinoa", Ingredient.IngredientFlags.IsVegetarian
        //                                     | Ingredient.IngredientFlags.IsVegan
        //                                     | Ingredient.IngredientFlags.IsLowFat));
        //}
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
            string ReturnString = "";

            ReturnString += "\nListe der Rezepte:\n";
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

    /* Ein Rezept ist für eine Portionsanzahl ausgelegt.
     * Rezepte generieren ein Gesamtmerkmal aus den einzelnen Merkmalen von allen Zutaten
     */
    public class Recipe:IEquatable<Recipe>
    {
        //Variables
        private static UnitSet _UnitSetData;
        private static IngredientSet _IngredientSetData;
        private long? _ID;

        private IngredientItemSet _Ingredients;
        private string _Name;
        private int _PortionQuantity; // Portion min max berücksichtigen
        private string _Source; // Source: Cookbook the recipe is taken from 
        private string _SourceISBN;
        private string _SourcePage; // Page the recipe is found in the cookbook
        private string _Summary; // Summary
        //        private bool _ToTakeAway;

        
        //Constructors
        public Recipe()
        {
            _Ingredients = new IngredientItemSet(UnitSetData, IngredientSetData, this);
        }
        public Recipe(bool ToBePopulated)
        {
            _Ingredients = new IngredientItemSet(UnitSetData, IngredientSetData, this);
            if (ToBePopulated) PopulateObject();
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
        public static UnitSet UnitSetData //Readonly
        {
            get
            {
                return _UnitSetData;
            }

            //set
            //{
            //    _UnitSetData = value;
            //}
        }
        public static IngredientSet IngredientSetData //Readonly
        {
            get
            {
                return _IngredientSetData;
            }

            //set
            //{
            //    _IngredientSetData = value;
            //}
        }

        public IngredientItemSet Ingredients  // Maybe Readonly
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
        public void SetDataReference(UnitSet UnitSetData, IngredientSet IngredientSetData)
        {
            _UnitSetData = UnitSetData;
            _IngredientSetData = IngredientSetData;
//            Ingredients.SetDataReference(Data);
        }
        public override string ToString()
        {
            return string.Format("{0,6}-Name: {1,10}  Source: {2,5}  Seite: {3,5}  Summary {4,15}", ID, Name, Source, SourcePage, Summary);
        }

    }
}
