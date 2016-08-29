using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jamie.Model
{
    public class RecipeSet: ObservableCollection<Recipe>
    {

        //Methods
        public void AddItem()
        {
            AddItem(new Recipe(true));
        }
        public void AddItem(Recipe ItemToBeAdded)
        {
            if (!Contains(ItemToBeAdded)) Add(ItemToBeAdded);
            else Console.WriteLine("Die Zutat ist bereits vorhanden: \n {0}", ItemToBeAdded);
        }
        public void Menu()
        {
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                Console.WriteLine();
                Console.WriteLine("\nIngredient Menü");
                Console.WriteLine("----------------------");
                Console.WriteLine("A  Add Ingredient");
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
//                        AddItem();
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
                    ReturnString += ListItem.ToString() + "\n";
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

        private IngredientRecipeSet _Ingredients;
        private string _Name;
        private int _PortionQuantity; // Portion min max berücksichtigen
        private string _Source; // Source: Cookbook the recipe is taken from 
        private string _SourcePage; // Page the recipe is found in the cookbook
        private string _Summary; // Summary
//        private bool _ToTakeAway;

        //Constructors
        public Recipe()
        {
            _Ingredients = new IngredientRecipeSet();
        }
        public Recipe(bool ToBePopulated)
        {
            _Ingredients = new IngredientRecipeSet();
            if (ToBePopulated) PopulateObject();
        }

        //Properties
        public IngredientRecipeSet Ingredients
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
            } while (int.TryParse(InputString, out ParsedIntValue));
            PortionQuantity = ParsedIntValue;
            Console.Write("Summary : "); Summary = Console.ReadLine();
            Console.Write("Source : "); Source = Console.ReadLine();

        }
        public override string ToString()
        {
            return String.Format("Name: {0,10}  Source: {1,5}  Seite: {2,5}  Summary {3,15}", Name, Source, SourcePage, Summary);
        }

    }
}
