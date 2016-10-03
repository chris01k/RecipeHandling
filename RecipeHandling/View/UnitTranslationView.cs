using Jamie.Model;
using System;


namespace Jamie.View
{
    public class UnitTranslationView
    {
        //Variables
        private UnitTranslationSet _UnitTranslations;
        private UnitSet _Units;

        //Constructors
        public UnitTranslationView(UnitTranslationSet UnitTranslations, UnitSet Units)
        {
            _UnitTranslations = UnitTranslations;
            _Units = Units;
        }

        //Properties
        public UnitTranslationSet UnitTranslations //Readonly
        {
            get
            {
                return _UnitTranslations;
            }
        }
        public UnitSet Units
        {
            get
            {
                return _Units;
            }
        } //Readonly

        //Methods
        public void Menu()
        {
            int HowManyItemsInSet = UnitTranslations.Count;

            if (HowManyItemsInSet > 0) UnitTranslations.SelectItem(HowManyItemsInSet - 1);
            string MenuInput = "";

            while (MenuInput != "Q")
            {
                ViewSet(); 
                Console.WriteLine();
                Console.WriteLine("\nUnit Translation Menü");
                Console.WriteLine("----------------------");
                Console.WriteLine("Selected Ingredient {0}\n", UnitTranslationSet.SelectedItem);
                Console.WriteLine();
                Console.WriteLine("A  Add Unit Translation");
                Console.WriteLine("D  Delete Unit Translation");
                Console.WriteLine("E  Edit Unit Translation");
                Console.WriteLine("S  Select Unit Translation");
                Console.WriteLine("V  View Set");
                Console.WriteLine("--------------------");
                Console.WriteLine("Q  Quit");

                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "A":
                        UnitTranslation newItem = PopulateObject();
                        if ((newItem != null) && !UnitTranslations.AddItem(newItem))
                        {
                            Console.WriteLine("Fehler: Einheit konnte nicht angelegt werden");
                            Console.ReadLine();
                        }
                        break;
                    case "D":
                        UnitTranslations.DeleteSelectedItem();
                        break;
                    case "E":
                        //EditSelectedItem();
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
        public void SelectItem()
        {
            string InputString;
            int ParsedIntValue;
            int SelectedID;
            UnitTranslation UT = new UnitTranslation();

            do
            {
                Console.Write("Unit Translation ID:"); InputString = Console.ReadLine();
            } while (!int.TryParse(InputString, out ParsedIntValue));
            SelectedID = ParsedIntValue;

            UT.ID = SelectedID;

            if (UnitTranslations.SelectItem(UT) == null)
            {
                Console.WriteLine("Fehler: UnitTranslation mit ID: {0} konnte nicht gefunden werden", SelectedID);
                Console.ReadLine();
            }

        }
        public UnitTranslation PopulateObject()
        {
            //UnitTranslation NewUnitTranslation = new UnitTranslation();


            string InputString;
            double ParsedDoubleValue;
            Ingredient ProcessedIngredient;
            Unit ProcessedUnit;
            UnitTranslation ReturnItem;

            ReturnItem = new UnitTranslation();


            Console.WriteLine();
            Console.WriteLine("Eingabe neue Einheit:");
            Console.WriteLine("---------------------");
            Console.WriteLine();
            ViewUnitSet();
            do
            {
                Console.Write("BaseUnitSymbol   : "); ProcessedUnit = Units.SelectItem(Console.ReadLine());
            } while (ProcessedUnit == null);
            ReturnItem.BaseUnit = ProcessedUnit;
            do
            {
                Console.Write("TargetUnitSymbol   : "); ProcessedUnit = Units.SelectItem(Console.ReadLine());
            } while (ProcessedUnit == null);
            ReturnItem.TargetUnit = ProcessedUnit;

            do
            {
                Console.Write("TranslationFactor: "); InputString = Console.ReadLine();
            } while (!double.TryParse(InputString, out ParsedDoubleValue));
            ReturnItem.TranslationFactor = ParsedDoubleValue;

            if (ReturnItem.BaseUnit.Type != ReturnItem.TargetUnit.Type)
            {
                ReturnItem.TranslationFlag |= TranslationType.IsTypeChange;
                Console.Write("IsIngredientDependent:"); InputString = Console.ReadLine();
                if (InputString.Length > 0) //Flag setzen, Ingredient eingeben
                {
                    ReturnItem.TranslationFlag |= TranslationType.IsIngredientDependent;
                    do
                    {
                        Console.Write("  Affeced Ingredient  : "); ProcessedIngredient = UnitTranslationSet.IngredientSetData.GetItem(Console.ReadLine());
                    } while (ProcessedIngredient == null);
                    ReturnItem.AffectedIngredient = ProcessedIngredient;
                }
                else //IngredientType eingeben
                {
                    do
                    {
                        Console.Write("Ingredient Type  : "); InputString = Console.ReadLine();
                        try
                        {
                            ReturnItem.IngredientType = (IngredientType)Enum.Parse(typeof(IngredientType), InputString);
                        }
                        catch
                        {
                            continue;
                        }
                        break;
                    } while (true);

                }

            }
            return ReturnItem;
        }
        public void ViewSet()
        {
            Console.WriteLine(UnitTranslations.ToString());
        }
        public void ViewUnitSet()
        {
            Console.WriteLine(Units.ToString());
        }


    }
}