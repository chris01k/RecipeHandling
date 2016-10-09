using Jamie.Model;
using System;

namespace Jamie.View
{
    
    public class ShoppingListView
    {

        //Variables
        private ShoppingListItemSet _ShoppingListItems;

        //Constructors
        public ShoppingListView(ShoppingListItemSet ShoppingListItems)
        {
            _ShoppingListItems = ShoppingListItems;
        }

        //Properties
        public ShoppingListItemSet ShoppingListItems
        {
            get
            {
                return _ShoppingListItems;
            }
        }//Readonly

        //Methods

/*      public bool AddItem()
        //{
        //    ShoppingListItem newItem;

        //    newItem = new ShoppingListItem();
        //    if (ShoppingListItems.Count == 0) newItem.SetDataReference(IngredientSetData, UnitSetData, UnitTranslationSetData);
        //    newItem.PopulateObject();
        //    return AddItem(newItem);
        }
*/
        public void Menu()
    {
        int HowManyItemsInSet = ShoppingListItems.Count;

        if (HowManyItemsInSet > 0) ShoppingListItems.SelectItem(HowManyItemsInSet - 1);
        string MenuInput = "";

        while (MenuInput != "Q")
        {
            ViewSet();
            Console.WriteLine();
            Console.WriteLine("\nShoppingListItem Menü");
            Console.WriteLine("---------\nSelected ShoppingLisstItem: {0}\n", ShoppingListItems.SelectedItem);
            Console.WriteLine("A  Add ShoppingListItem");
            Console.WriteLine("D  Delete Selected ShoppingListItem");
            Console.WriteLine("S  Select ShoppingListItem");
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
                    break;
                case "D":
                    //DeleteSelectedItem();
                    break;
                case "S":
                    //SelectItem();
                    break;
                case "V":
                    break;
                default:
                    Console.WriteLine();
                    break;
            }

        }
    }
        public ShoppingListItem NewPopulatedObject()
        {
            string InputString;
            double ParsedDoubleValue;

            ShoppingListItem ReturnItem = new ShoppingListItem();

            do
            {
                Console.Write("Quantity  : "); InputString = Console.ReadLine();
            } while (!double.TryParse(InputString, out ParsedDoubleValue));
            ReturnItem.Quantity = ParsedDoubleValue;

            ReturnItem.Unit = ListHelper.ChangeUnitField("Unit:");
            ReturnItem.Ingredient = ListHelper.ChangeIngredientField("Ingredient:");

            return ReturnItem;

        }
    //    public void PopulateObject()
    //{
    //    string InputString;

    //    do
    //    {
    //        Console.Write("Shoppinglist Date: "); InputString = Console.ReadLine();
    //        try
    //        {
    //            DueDate = DateTime.Parse(InputString);
    //        }
    //        catch
    //        {
    //            continue;
    //        }
    //        break;
    //    } while (true);

    //    Console.Write("Name            : "); Name = Console.ReadLine();
    //    Console.Write("Responsible     : "); Responsible = Console.ReadLine();
    //    Console.Write("Shop            : "); Shop = Console.ReadLine();



    //}
        public ShoppingListItem SelectItem()
    {
        int ParsedIntValue;
        long SelectedID;
        string InputString;

        do
        {
            Console.Write("FoodPlan ID:"); InputString = Console.ReadLine();
        } while (!int.TryParse(InputString, out ParsedIntValue));
        SelectedID = ParsedIntValue;

        return ShoppingListItems.SelectItem(SelectedID);

    }
        public void ViewSet()
    {
        Console.WriteLine(ToString());
    }
    }
}