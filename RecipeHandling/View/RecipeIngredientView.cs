using Jamie.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jamie.View
{
    public class RecipeIngredientView
    {
        //Variables
        private RecipeIngredientSet _RecipeIngredients;

        //Constructors
        public RecipeIngredientView()
        {
          
        }

        //Parameter
        public RecipeIngredientSet RecipeIngredients
        {
            get
            {
                return _RecipeIngredients;
            }
        }//Readonly

        //Methods
        public bool AddItem()
        {
            bool ReturnValue = false;

            RecipeIngredient newItem = NewPopulatedObject();
            if ((newItem != null) && RecipeIngredients.AddItem(newItem)) ReturnValue = true;

            return ReturnValue;
        }
        public void Menu()
        {
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                Console.WriteLine();
                Console.WriteLine("\nRezept-Zutaten: {0}", _RecipeIngredients.RelatedRecipe);
                Console.WriteLine("-----------------");
                Console.WriteLine("A  Add Ingredient");
                Console.WriteLine("V  View Set");
                Console.WriteLine("-----------------");
                Console.WriteLine("Q  Quit");

                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "A":
                        if (!AddItem())
                        {
                            Console.WriteLine("Fehler: Rezept-Zutat konnte nicht angelegt werden");
                            Console.ReadLine();
                        }
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
        public RecipeIngredient NewPopulatedObject()
        {
            string InputString;
            float ParsedDoubleValue;

            RecipeIngredient ReturnItem = new RecipeIngredient();

            Console.WriteLine("Eingabe neue Zutat zum Rezept:");
            Console.WriteLine("------------------------------");
            Console.WriteLine();
            do
            {
                Console.Write("Menge (Quantity): "); InputString = Console.ReadLine();
            } while (!float.TryParse(InputString, out ParsedDoubleValue));
            ReturnItem.Quantity = ParsedDoubleValue;

            ReturnItem.Unit = ListHelper.ChangeUnitField("Unit:");
            ReturnItem.Ingredient = ListHelper.ChangeIngredientField("Ingredient:");

            return ReturnItem;

        }
        public void SetRecipeIngredientList(RecipeIngredientSet RecipeIngredients)
        {
            _RecipeIngredients = RecipeIngredients;
        }
        public void ViewSet()
        {
            Console.WriteLine(ToString());
        }





    }
}