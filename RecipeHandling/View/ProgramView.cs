using Jamie.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jamie.View
{
    public class ProgramView
    {
        private EmbeddedDataSet _EmbeddedSetData;

        public ProgramView()
        {
            _EmbeddedSetData = new EmbeddedDataSet();
            //_FullSetHandler = new RecipeHandlingSetHandler(true);
        }

        public void ShowMainMenu()
        {
                {
                string MenuInput = "";

                while (MenuInput != "Q")
                {
                    Console.WriteLine();
                    Console.WriteLine("\nMenü");
                    Console.WriteLine("----");

                    Console.WriteLine("C  Clear Lists");
                    Console.WriteLine("O  Open Lists");
                    Console.WriteLine("S  Save Lists");
                    Console.WriteLine();

                    Console.WriteLine("I   Ingredient");
                    Console.WriteLine("FP  FoodPlanItem");
                    Console.WriteLine("R   Recipe");
                    Console.WriteLine("U   Unit");
                    Console.WriteLine("UT  Unit Translation");
                    Console.WriteLine("SL  Shopping List");
                    Console.WriteLine();
                    Console.WriteLine("T  Transfer to Foodplan to Shopping List");
                    Console.WriteLine("V  View Lists");
                    Console.WriteLine("X  View XML File");
                    Console.WriteLine("--------------------");
                    Console.WriteLine("Q  Quit");

                    Console.WriteLine();
                    Console.Write("Ihre Eingabe:");
                    MenuInput = Console.ReadLine().ToUpper();

                    switch (MenuInput)
                    {
                        case "C":
                            _EmbeddedSetData.ClearLists();
                            break;
                        case "O":
                            _EmbeddedSetData.OpenLists();
                            break;
                        case "S":
                            _EmbeddedSetData.SaveLists();
                            break;
                        case "I":
                            _EmbeddedSetData.AllSetData.Ingredients.Menu();
                            break;
                        case "FP":
                            _EmbeddedSetData.AllSetData.FoodPlanItems.Menu();
                            break;
                        case "R":
                            _EmbeddedSetData.AllSetData.Recipes.Menu();
                            break;
                        case "U":
                            _EmbeddedSetData.AllSetData.Units.Menu();
                            break;
                        case "UT":
                            _EmbeddedSetData.AllSetData.UnitTranslations.Menu();
                            break;
                        case "SL":
                            _EmbeddedSetData.AllSetData.ShoppingListItems.Menu();
                            break;
                        case "T":
                            _EmbeddedSetData.AllSetData.FoodPlanItems.TransferToShoppingList(_EmbeddedSetData.AllSetData.ShoppingListItems);
                            _EmbeddedSetData.AllSetData.ShoppingListItems.Menu();
                            break;
                        case "V":
                            _EmbeddedSetData.AllSetData.ViewSet();
                            break;
                        case "X":
                            _EmbeddedSetData.AllSetData.ViewXML();
                            break;
                        default:
                            Console.WriteLine();
                            break;
                    }

                }


            }

        }

    }

}