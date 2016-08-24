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
        public Recipe()
        {
            _Ingredients = new IngredientRecipeSet();
        }

        public string Name { get; set; }
        public string Summary { get; set; } // Summary
        public int PortionQuantity { get; set; } // Portion min max berücksichtigen
        public string Source {get; set; }
        public string Page { get; set; }
        public bool ToTakeAway { get; set; }

        private IngredientRecipeSet _Ingredients;

        public IngredientRecipeSet Ingredients
        {
            get { return _Ingredients; }
            set { _Ingredients = value; }
        }

    }
}
