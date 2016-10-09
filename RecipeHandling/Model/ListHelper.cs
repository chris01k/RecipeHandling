using Jamie.Model;
using System;

namespace Jamie.View
{
    public static class ListHelper
    {
        //Variables
        private static IngredientSet _Ingredients;
        private static RecipeSet _Recipes;
        private static UnitSet _Units;

        //Properties
        public static IngredientSet Ingredients
        {
            get
            {
                return _Ingredients;
            }
        } //Readonly
        public static RecipeSet Recipes
        {
            get
            {
                return _Recipes;
            }
        } //Readonly
        public static UnitSet Units 
        {
            get
            {
                return _Units;
            }

        } //Readonly


        //Methods
        public static IngredientFlags ChangeIngredientFlagField(string DisplayFieldToBeChanged)
        {
            IngredientFlags FieldToBeChanged;
            IngredientFlags ReturnValueFlag = 0;
            string InputString;

            Console.Write("Neuer Eintrag für {0}: ", DisplayFieldToBeChanged);
            for (int i = 1; i <= Ingredient.GetmaxIngredientFlag(); i = (i * 2))
            {
                Console.Write("{0,15}:", (IngredientFlags)i); InputString = Console.ReadLine();
                if (InputString.Length > 0) ReturnValueFlag = (ReturnValueFlag | (IngredientFlags)i);
            }
            FieldToBeChanged = ReturnValueFlag;



            return FieldToBeChanged;
        }// --> View
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
        }// --> View
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
        }// --> View
        public static string ChangeStringField(string DisplayFieldToBeChanged)
        {
            string FieldToBeChanged;
            Console.Write("Neuer Eintrag für {0}: ", DisplayFieldToBeChanged);
            FieldToBeChanged = Console.ReadLine();
            return FieldToBeChanged;
        }// --> View
        public static Ingredient ChangeIngredientField(string DisplayFieldToBeChanged)
        {
            Ingredient ReturnValueUnit = null;
            string InputString;

            while (ReturnValueUnit == null)
            {
                Console.Write("Neuer Eintrag für {0}: ", DisplayFieldToBeChanged);
                InputString = Console.ReadLine();
                ReturnValueUnit = Ingredients.SelectItem(InputString);
            };
            return ReturnValueUnit;
        }
        public static Recipe ChangeRecipeField(string DisplayFieldToBeChanged)
        {
            Recipe ReturnValueUnit = null;
            string InputString;
            long InputValue;

            do
            {
                Console.Write("Neuer Eintrag für {0}: ", DisplayFieldToBeChanged);
                InputString = Console.ReadLine();
            } while(!long.TryParse(InputString, out InputValue));

            ReturnValueUnit = Recipes.SelectItemByID(InputValue);

            return ReturnValueUnit;
        }

        public static Unit ChangeUnitField(string DisplayFieldToBeChanged)
        {
            Unit ReturnValueUnit = null;
            string InputString;

            while (ReturnValueUnit == null)
            {
                Console.Write("Neuer Eintrag für {0} - Symbol: ", DisplayFieldToBeChanged);
                InputString = Console.ReadLine();
                ReturnValueUnit = Units.SelectItem(InputString);
            };
            return ReturnValueUnit;
        }// --> View
        public static void SetDataReference(IngredientSet Ingredients, RecipeSet Recipes, UnitSet Units)
        {
            _Ingredients = Ingredients;
            _Recipes = Recipes;
            _Units = Units;
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
        }// --> View
        public static void ResetString(string DisplayFieldToBeChanged)
        {
            DisplayFieldToBeChanged = "";
            Console.Write("{0} ist leer: ", DisplayFieldToBeChanged);
        }// --> View
    }
}