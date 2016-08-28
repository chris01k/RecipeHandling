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

    }


    /* Ein Rezept ist für eine Portionsanzahl ausgelegt.
     * Rezepte generieren ein Gesamtmerkmal aus den einzelnen Merkmalen von allen Zutaten
     */

    public class Recipe
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
    }
}
