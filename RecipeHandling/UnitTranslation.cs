using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeHandling
{
    public class UnitTranslation:IEquatable<UnitTranslation>
    {
        public string BaseUnitSymbol { get; set; }
        public string TargetUnitSymbol { get; set; }
        public double TranslationFactor { get; set; }
        public bool IngredientDependent { get; set; }

//        private bool UnitTypeTranslation;


        internal UnitTranslation()
        {
            
        }

        internal UnitTranslation(bool ToBePopulated)
        {
            if (ToBePopulated) PopulateData();
        }

        internal UnitTranslation(string BaseUnitSymbol, string TargetUnitSymbol, double TranslationFactor, bool IngredientDependent)
        {
            this.BaseUnitSymbol = BaseUnitSymbol;
            this.TargetUnitSymbol = TargetUnitSymbol;
            this.TranslationFactor = TranslationFactor;
            this.IngredientDependent = IngredientDependent;
         
        }



        public bool Equals(UnitTranslation UnitTranslationToCompare)
        {
            return (BaseUnitSymbol.Equals(UnitTranslationToCompare.BaseUnitSymbol) &&
                    TargetUnitSymbol.Equals(UnitTranslationToCompare.TargetUnitSymbol));
        }


        public void PopulateData()
        {
            string InputString;
            float ParsedValue;

            Console.WriteLine("Eingabe neue Einheit:" );
            Console.WriteLine("---------------------");
            Console.WriteLine();
            Console.Write("BaseUnitSymbol   : "); BaseUnitSymbol = Console.ReadLine();
            Console.Write("TargetUnitSymbol : "); TargetUnitSymbol = Console.ReadLine();
            Console.Write("TranslationFactor: "); InputString = Console.ReadLine();
            do
            {
                Console.Write("TranslationFactor: "); InputString = Console.ReadLine();
            } while (float.TryParse(InputString, out ParsedValue));
            TranslationFactor = ParsedValue;

        }



        public override string ToString()
        {
            return String.Format("UnitTranslation: {0,5} --> {1,5} with Factor {2,10} ", BaseUnitSymbol, TargetUnitSymbol, TranslationFactor);
        }

    }

}