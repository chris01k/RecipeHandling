using System;

namespace Jamie.Model
{
    public enum ListEntryStatus
    { IsOK = 0x1, IsCalculated, IsNotConfirmed }

    public static class ListHelper
    {

        //Mathods
        public static IngredientFlags ChangeIngredientFlagField(string DisplayFieldToBeChanged)
        {
            IngredientFlags FieldToBeChanged;
            IngredientFlags ReturnValueFlag = 0;
            string InputString;

            Console.Write("Neuer Eintrag für {0}: ", DisplayFieldToBeChanged);
            for (int i = 1; i <= Ingredient.maxIngredientFlag; i = (i * 2))
            {
                Console.Write("{0,15}:", (IngredientFlags)i); InputString = Console.ReadLine();
                if (InputString.Length > 0) ReturnValueFlag = (ReturnValueFlag | (IngredientFlags)i);
            }
            FieldToBeChanged = ReturnValueFlag;



            return FieldToBeChanged;
        }
        public static IngredientType ChangeIngredientTypeField(string DisplayFieldToBeChanged)
        {
            IngredientType FieldToBeChanged;
            IngredientType ReturnTypeValue;
            string InputString;

            do
            {
                Console.Write("Neuer Eintrag für {0}: ", DisplayFieldToBeChanged);
                InputString = Console.ReadLine();
                try
                {
                    ReturnTypeValue = (IngredientType)Enum.Parse(typeof(IngredientType), InputString);
                }
                catch
                {
                    continue;
                }
                break;
            } while (true);


            FieldToBeChanged = ReturnTypeValue;



            return FieldToBeChanged;
        }
        public static double ChangeDoubleField(string DisplayFieldToBeChanged)
        {
            string InputString;
            double FieldToBeChanged;
            do
            {
                Console.Write("Neuer Eintrag für {0}: ", DisplayFieldToBeChanged);
                InputString = Console.ReadLine();
            } while (!double.TryParse(InputString, out FieldToBeChanged));


            return FieldToBeChanged;
        }
        public static string ChangeStringField(string DisplayFieldToBeChanged)
        {
            string FieldToBeChanged;
            Console.Write("Neuer Eintrag für {0}: ", DisplayFieldToBeChanged);
            FieldToBeChanged = Console.ReadLine();
            return FieldToBeChanged;
        }
        public static Unit ChangeUnitField(string DisplayFieldToBeChanged, UnitSet UnitSelection)
        {
            Unit ReturnValueUnit = null;
            string InputString;

            Console.Write("Neuer Eintrag für {0} - Symbol: ", DisplayFieldToBeChanged);
            InputString = Console.ReadLine();
            while (ReturnValueUnit == null)
            {
                ReturnValueUnit = UnitSelection.SelectItem(InputString);
            };
            return ReturnValueUnit;
        }
        public static string SelectField(string[] Fields)
        {
            string InputString ="";

            Console.WriteLine();
            foreach (string str in Fields)
            {
                Console.Write(str+" "); 
            }
            Console.WriteLine();


            Console.Write("Auswahl Feld: ");
            InputString = Console.ReadLine();

            foreach (string str in Fields)
            {
                if (str == InputString) return InputString;
            }
            return "";
        }
        public static void ResetString(string DisplayFieldToBeChanged)
        {
            DisplayFieldToBeChanged = "";
            Console.Write("{0} ist leer: ", DisplayFieldToBeChanged);
        }
    }
}