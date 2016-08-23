﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeHandling
{
    public class UnitTranslation:IEquatable<UnitTranslation>
    {
        [Flags]
        public enum TranslationIndepedenceType
        {IsStandard = 0x0, IsDepedent = 0x1, IsDefault =0x2}

        public string BaseUnitSymbol { get; set; }
        public string TargetUnitSymbol { get; set; }
        public double TranslationFactor { get; set; }
        public TranslationIndepedenceType IngredientDependent { get; set; }
//        private bool UnitTypeTranslation;


        internal UnitTranslation()
        {
            IngredientDependent = (TranslationIndepedenceType) 3;
        }

        internal UnitTranslation(bool ToBePopulated)
        {
            if (ToBePopulated) PopulateData();
        }

        internal UnitTranslation(string BaseUnitSymbol, string TargetUnitSymbol, double TranslationFactor, int IngredientDependent)
        {
            this.BaseUnitSymbol = BaseUnitSymbol;
            this.TargetUnitSymbol = TargetUnitSymbol;
            this.TranslationFactor = TranslationFactor;
            this.IngredientDependent = (TranslationIndepedenceType) IngredientDependent;
         
        }
        
        public bool Equals(UnitTranslation UnitTranslationToCompare)
        {
            return (BaseUnitSymbol.Equals(UnitTranslationToCompare.BaseUnitSymbol) &&
                    TargetUnitSymbol.Equals(UnitTranslationToCompare.TargetUnitSymbol));
        }
        
        public void PopulateData()
        {
            string InputString;
            double ParsedValue;

            Console.WriteLine("Eingabe neue Einheit:" );
            Console.WriteLine("---------------------");
            Console.WriteLine();
            Console.Write("BaseUnitSymbol   : "); BaseUnitSymbol = Console.ReadLine();
            Console.Write("TargetUnitSymbol : "); TargetUnitSymbol = Console.ReadLine();
            Console.Write("TranslationFactor: "); InputString = Console.ReadLine();
            do
            {
                Console.Write("TranslationFactor: "); InputString = Console.ReadLine();
            } while (double.TryParse(InputString, out ParsedValue));
            TranslationFactor = ParsedValue;

        }
        
        public override string ToString()
        {
            return String.Format("UnitTranslation: {0,5} =  {1,10:F3} {2,-5} {3}", BaseUnitSymbol, TranslationFactor, TargetUnitSymbol,
                                  IngredientDependent);
        }

    }

}