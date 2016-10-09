using Jamie.Model;
using System;

namespace Jamie.View
{
    public class RecipeView
    {
        //Variables
        private RecipeSet _Recipes;
        private RecipeIngredientView _RecipeIngredientUI;

        //Constructors
        public RecipeView(RecipeSet Recipes)
        {
            _Recipes = Recipes;
            _RecipeIngredientUI = new RecipeIngredientView();
            if (Recipes.SelectedItem != null) _RecipeIngredientUI.SetRecipeIngredientList(Recipes.SelectedItem.Ingredients);
        }

        //Parameter
        public RecipeSet Recipes
        {
            get
            {
                return _Recipes;
            }
        }
        public void Menu()
        {
            string MenuInput = "";

            int HowManyItemsInSet = Recipes.Count;
            if (HowManyItemsInSet > 0) Recipes.SelectItem(HowManyItemsInSet - 1);
            while (MenuInput != "Q")
            {
                ViewSet();
                if (Recipes.SelectedItem != null) _RecipeIngredientUI.SetRecipeIngredientList(Recipes.SelectedItem.Ingredients);
                Console.WriteLine();
                Console.WriteLine("\nRecipe Menü");
                Console.WriteLine(Recipes.SelectedItem);
                Console.WriteLine("---------------");
                Console.WriteLine("A  Add Recipe");
                Console.WriteLine("D  Delete Selected Recipe");
                Console.WriteLine("I  Add Ingredient (to selected Recipe)");
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
                        Recipe newItem = NewPopulatedObject();
                        if ((newItem != null) && !Recipes.AddItem(newItem))
                        {
                            Console.WriteLine("Fehler: Rezept konnte nicht angelegt werden");
                            Console.ReadLine();
                        }
                        break;
                    case "D":
                        Recipes.DeleteSelectedItem();
                        break;
                    case "I":

                        if (!_RecipeIngredientUI.AddItem())
                        {
                            Console.WriteLine("Fehler: Rezept-Zutat konnte nicht angelegt werden");
                            Console.ReadLine();
                        }
                        Console.WriteLine(Recipes.SelectedItem.ToString());
                        break;
                    case "R":
                        Console.WriteLine(Recipes.SelectedItem.ToString());
                        break;
                    case "S":
                        SelectItem();
                        break;
                    case "V":
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }

            }
        }
        public Recipe NewPopulatedObject()
        {
            string InputString;
            int ParsedIntValue;
            Recipe ReturnItem;


            ReturnItem = new Recipe();

            
            Console.Write("Name : "); ReturnItem.Name = Console.ReadLine();
            do
            {
                Console.Write("PortionQuantity : "); InputString = Console.ReadLine();
            } while (!int.TryParse(InputString, out ParsedIntValue));
            ReturnItem.PortionQuantity = ParsedIntValue;
            Console.Write("Summary         : "); ReturnItem.Summary = Console.ReadLine();
            Console.Write("Source          : "); ReturnItem.Source = Console.ReadLine();
            Console.Write("SourcePage      : "); ReturnItem.SourcePage = Console.ReadLine();
            Console.Write("SourceISBN      : "); ReturnItem.SourceISBN = Console.ReadLine();

            return ReturnItem;
        }
        public Recipe SelectItem()
        {
            Recipe ReturnValue = null;

            Recipe RequestedItem = new Recipe();

            Console.WriteLine("Recipe suchen:");
            Console.WriteLine("--------------");
            Console.WriteLine();
            Console.Write("Recipe Name: "); RequestedItem.Name = Console.ReadLine();
            ReturnValue = Recipes.SelectItem(RequestedItem);
            if (ReturnValue != null) _RecipeIngredientUI.SetRecipeIngredientList(ReturnValue.Ingredients);

            return ReturnValue;
        }
        public void ViewSet()
        {
            Console.WriteLine(Recipes.ToString());
        }
    }
}