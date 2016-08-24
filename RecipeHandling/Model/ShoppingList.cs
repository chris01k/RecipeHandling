using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jamie.Model
{
    public class ShoppingList
    {
        public string Name { get; set; }


        private ShoppingListItemSet _Items;

        public ShoppingListItemSet Items
        {
            get { return _Items; }
            set { _Items = value; }
        }
        
        public ShoppingList()
        {
            _Items = new ShoppingListItemSet();
        }

        // Fügt ein Rezept sowie die Zutaten der Shopping Liste hinzu
        public void addRecipe(Recipe mRecipe)
        {
            foreach (var ingredients in mRecipe.Ingredients)
            {
                _Items.Add(new ShoppingListItem
                {
                    SpecificIngredient = ingredients.SpecificIngredient,
                    Quantity = ingredients.Quantity,
                    Unit = ingredients.Unit,
                });
            }
        }

        // Löscht ein Rezept aus der Shopping List und die in der Liste hinterlegten Zutaten
        public void deleteRecipe(Recipe mRecipe)
        {
            throw new NotImplementedException();
        }

        // Fügt ein Ingredient zur ShoppingList ohne Bezug zu einem Rezept.
        public void addIngredient(Ingredient mIngredient)
        {
            throw new NotImplementedException();
        }


    }

    public class ShoppingListItemSet : ObservableCollection<ShoppingListItem>
    {
    }


    public class ShoppingListItem
    {
        public string Unit { get; set; }
        public float? Quantity { get; set; }
        public Ingredient SpecificIngredient { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public RecipeSet RecipesPortion { get; set; }

    }

}
