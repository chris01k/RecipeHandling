using Jamie.Model;
using System;


namespace Jamie.View
{
    public class IngredientView
    {
        //Variables
        private IngredientSet _Ingredients;
        private UnitSet _Units;

        //Constructors
        public IngredientView(IngredientSet Ingredients, UnitSet Units)
        {
            _Ingredients = Ingredients;
            _Units = Units;
        }

        //Properties
        public IngredientSet Ingredients
        {
            get
            {
                return _Ingredients;
            }
        } //Readonly
        public UnitSet Units
        {
            get
            {
                return _Units;
            }
        } //Readonly

        //Methods
        public void EditSelectedItem()
        {
            string InputString = "";
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                ViewSet();
                Console.WriteLine();
                Console.WriteLine("Edit Selected Ingredient: {0}\n", Ingredients.SelectedItem);
                Console.WriteLine("-------------------------\n");
                Console.WriteLine();
                Console.WriteLine("C  Change Field");
                //Console.WriteLine("R  Reset Field");
                Console.WriteLine("--------------------");
                Console.WriteLine("Q  Quit");
                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "C":
                        string[] FieldsToBeSelected = { "Name", "Flags", "TargetUnit", "Type" };

                        InputString = ListHelper.SelectField(FieldsToBeSelected);
                        if (InputString == "Name") Ingredients.SelectedItem.Name = ListHelper.ChangeStringField(InputString);
                        else if (InputString == "Flags") Ingredients.SelectedItem.Flags = ListHelper.ChangeIngredientFlagField(InputString);
                        else if (InputString == "TargetUnit") Ingredients.SelectedItem.TargetUnit = ListHelper.ChangeUnitField(InputString);
                        else if (InputString == "Type") Ingredients.SelectedItem.Type = ListHelper.ChangeIngredientTypeField(InputString);
                        break;

                    default:
                        Console.WriteLine();
                        break;
                }


            }
        }
        public void Menu()
        {
            int HowManyItemsInSet = Ingredients.Count;

            if (HowManyItemsInSet > 0) Ingredients.SelectItem(HowManyItemsInSet - 1);
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                ViewSet();
                Console.WriteLine();
                Console.WriteLine("\nIngredient Menü");
                Console.WriteLine("------------------------\n");
                Console.WriteLine("Selected Ingredient {0}\n", Ingredients.SelectedItem);
                Console.WriteLine();
                Console.WriteLine("A  Add Ingredient");
                Console.WriteLine("D  Delete Ingredient");
                Console.WriteLine("E  Edit Selected Ingredient");
                Console.WriteLine("S  Select Ingredient");
                Console.WriteLine("V  View Set");
                Console.WriteLine("--------------------");
                Console.WriteLine("Q  Quit");
                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "A":
                        Ingredient newItem = NewPopulatedObject();
                        if ((newItem != null) && !Ingredients.AddItem(newItem))
                        {
                            Console.WriteLine("Fehler: Zutat konnte nicht angelegt werden");
                            Console.ReadLine();
                        }
                        break;
                    case "D":
                        Ingredients.DeleteSelectedItem();
                        break;
                    case "E":
                        EditSelectedItem();
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
        public Ingredient NewPopulatedObject()
        {
            IngredientFlags FlagValue = 0;

            string InputString;
            Unit ProcessedUnit;
            Ingredient ReturnItem;

            ReturnItem = new Ingredient();

            Console.WriteLine("Eingabe neue Zutat:");
            Console.WriteLine("-------------------");
            Console.WriteLine();
            Console.Write("         Name  : "); ReturnItem.Name = Console.ReadLine();
            do
            {
                Console.Write("  Target Unit  : "); ProcessedUnit = Units.SelectItem(Console.ReadLine());
            } while (ProcessedUnit == null);
            ReturnItem.TargetUnit = ProcessedUnit;
            do
            {
                Console.Write("Ingredient Type  : "); InputString = Console.ReadLine();
                try
                {
                    ReturnItem.Type = (IngredientType)Enum.Parse(typeof(IngredientType), InputString);
                }
                catch
                {
                    continue;
                }
                break;
            } while (true);



            for (int i = 1; i <= Ingredient.GetmaxIngredientFlag(); i = (i * 2))
            {
                Console.Write("{0,15}:", (IngredientFlags)i); InputString = Console.ReadLine();
                if (InputString.Length > 0) FlagValue = (FlagValue | (IngredientFlags)i);
            }
            ReturnItem.Flags = FlagValue;

            return ReturnItem;

        }
        public Ingredient SelectItem()
        {
            string InputItemText = "";

            Console.WriteLine("Ingredient auswählen:");
            Console.WriteLine("---------------------");
            Console.WriteLine();
            Console.Write("IngredientName: "); InputItemText = Console.ReadLine();

            return Ingredients.SelectItem(InputItemText);

        }
        public void ViewSet()
        {
            Console.WriteLine(Ingredients.ToString());
        }
    }
}