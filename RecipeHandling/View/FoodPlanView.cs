using Jamie.Model;
using System;

namespace Jamie.View
{
    public class FoodPlanView
    {
        //Variables
        private FoodPlanItemSet _FoodPlanItems;
        private RecipeSet _Recipes;

        //Constructors
        public FoodPlanView(FoodPlanItemSet FoodPlanItems, RecipeSet Recipes)
        {
            _FoodPlanItems = FoodPlanItems;
            _Recipes = Recipes;
        }

        //Properties
        public FoodPlanItemSet FoodPlanItems
        {
            get
            {
                return _FoodPlanItems;
            }
        } //Readonly
        public RecipeSet Recipes
        {
            get
            {
                return _Recipes;
            }
        } //Readonly

        //Methods
        public void Menu()
        {
            int HowManyItemsInSet = FoodPlanItems.Count;

            if (HowManyItemsInSet > 0) FoodPlanItems.SelectItem(HowManyItemsInSet - 1);
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                ViewSet();
                Console.WriteLine();
                Console.WriteLine("\nFoodPlanItem Menü");
                Console.WriteLine("---------\nSelected FoodplanItem: {0}\n", FoodPlanItems.SelectedItem);
                Console.WriteLine("A  Add FoodplanItem");
                Console.WriteLine("D  Delete Selected FoodplanItem");
                Console.WriteLine("S  Select FoodplanItem");
                Console.WriteLine("V  View Set");
                Console.WriteLine("--------------------");
                Console.WriteLine("Q  Quit");

                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "A":
                        //AddItem();
                        FoodPlanItem newItem = NewPopulatedObject();
                        if ((newItem != null) && !FoodPlanItems.AddItem(newItem))
                        {
                            Console.WriteLine("Fehler: Food Plan Eintrag konnte nicht angelegt werden");
                            Console.ReadLine();
                        }

                        break;
                    case "D":
                        //DeleteSelectedItem();
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
        }// --> View
        public FoodPlanItem NewPopulatedObject()
        {
            DateTime InputDate;
            float InputFloat;
            string InputString;
            FoodPlanItem ReturnItem;


            ReturnItem = new FoodPlanItem();


            do
            {
                Console.Write("Date of Preparation: ");
                InputString = Console.ReadLine();
            } while (!DateTime.TryParse(InputString, out InputDate));
            ReturnItem.DateToStartPreparation = InputDate;

            do
            {
                Console.Write("Date to Consume: ");
                InputString = Console.ReadLine();
            } while (!DateTime.TryParse(InputString, out InputDate));
            ReturnItem.DateToConsume = InputDate;

            do
            {
                Console.Write("Date to Consume: ");
                InputString = Console.ReadLine();
            } while (!float.TryParse(InputString, out InputFloat));
            ReturnItem.TotalPortions = InputFloat;



            /*            do
                          {
                            Console.Write("Date of Preparation  : "); InputString = Console.ReadLine();
                            try
                            {
                            DateTime.Parse(InputString);
                                        }
                                        catch
                                        {
                                            continue;
                                        }
                                        break;
                                    } while (true);

                            do
                            {
                                Console.Write("Date to Consume      : "); InputString = Console.ReadLine();
                                try
                                {
                                    ReturnItem.DateToConsume = DateTime.Parse(InputString);
                                }
                                catch
                                {
                                    continue;
                                }
                                break;
                            } while (true);



                        do
                        {
                            Console.Write("Total Portions       : "); InputString = Console.ReadLine();
                            try
                            {
                                ReturnItem.TotalPortions = float.Parse(InputString);
                            }
                            catch
                            {
                                continue;
                            }
                            break;
                        } while (true);
            */
            Console.WriteLine(Recipes.ToString());
            ReturnItem.PlannedRecipe = ListHelper.ChangeRecipeField("Recipe: ");

            return ReturnItem;

        }// --> View
        public FoodPlanItem SelectItem()
        {
            int ParsedIntValue;
            long SelectedID;
            string InputString;

            do
            {
                Console.Write("FoodPlan ID:"); InputString = Console.ReadLine();
            } while (!int.TryParse(InputString, out ParsedIntValue));
            SelectedID = ParsedIntValue;

            return FoodPlanItems.SelectItemByID(SelectedID);

        }
        public void ViewSet()
        {
            Console.WriteLine(FoodPlanItems.ToString());
        }// --> View

    }
}