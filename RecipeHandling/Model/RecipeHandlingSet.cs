using System;
using System.Linq;

namespace Jamie.Model
{
    public class RecipeDataSets
    {
        private IngredientSet _Ingredients; //related to <no other Set>
        private RecipeSet _Recipes; // related to IngredientSet, UnitSet
        private UnitSet _Units; // related to <no other Set>
        private UnitTranslationSet _UnitTranslations; //related to UnitSet

        //Constructors
        public RecipeDataSets()
        {
            _Units = new UnitSet();
            _UnitTranslations = new UnitTranslationSet(_Units);
            _Ingredients = new IngredientSet(_Units);
            _Recipes = new RecipeSet(_Units, _Ingredients);
        }
        public RecipeDataSets(bool ToBePopulatedWithDefaults)
        {
            _Units = new UnitSet();
            _UnitTranslations = new UnitTranslationSet(_Units);
            _Ingredients = new IngredientSet(_Units);
            _Recipes = new RecipeSet(_Units, _Ingredients);

            if (ToBePopulatedWithDefaults) PopulateSetWithDefaults();

        }

        //Properties
        public IngredientSet Ingredients
        {
            get
            {
                return _Ingredients;
            }

            set
            {
                _Ingredients = value;
            }
        }
        public RecipeSet Recipes
        {
            get { return _Recipes; }
            set { _Recipes = value; }
        }
        public UnitSet Units
        {
            get { return _Units; }
            set { _Units = value; }
        }
        public UnitTranslationSet UnitTranslations
        {
            get { return _UnitTranslations; }
            set { _UnitTranslations = value; }
        }

        //Methods
        public void ClearList()
        {
            Ingredients.Clear();
            Recipes.Clear();
            Units.Clear();
            UnitTranslations.Clear();
        }
        public int Count()
        {
            int ReturnValue = 0;

            ReturnValue += Ingredients.Count();
            ReturnValue += Recipes.Count();
            ReturnValue += Units.Count();
            ReturnValue += UnitTranslations.Count();

            return ReturnValue;
        }
        public void PopulateSetWithDefaults()
        {
            Units.PopulateSetWithDefaults();
            UnitTranslations.PopulateSetWithDefaults();
            Ingredients.PopulateSetWithDefaults();
        }
        public void RemoveUnit()
        {
            string LocalUnitSymbol = "";

            Console.WriteLine("Unit Entfernen:");
            Console.WriteLine("---------------");
            Console.WriteLine();
            Console.Write("UnitSymbol: "); LocalUnitSymbol = Console.ReadLine();

            RemoveUnit(LocalUnitSymbol);

        }
        public void RemoveUnit(string UnitSymbolToBeRemoved)
        {

            Unit UnitToBeRemoved = new Unit();
            UnitToBeRemoved.Symbol = UnitSymbolToBeRemoved;

            if (Units.Contains(UnitToBeRemoved)) Units.Remove(UnitToBeRemoved);
            else Console.WriteLine("UnitSymbol {0} konnte nicht gefunden werden", UnitSymbolToBeRemoved);
                       

        }
        public void SaveSet(string FileName)
        {
            Units.SaveSet(FileName);
            UnitTranslations.SaveSet(FileName);
            Ingredients.SaveSet(FileName);
            Recipes.SaveSet(FileName);
        }
        public void ViewSet()
        {
            Console.WriteLine();
            Console.WriteLine(ToString());
        }
        public override string ToString()
        {
            string ReturnString = "";

            ReturnString += Ingredients.ToString();
            ReturnString += Recipes.ToString();
            ReturnString += Units.ToString();
            ReturnString += UnitTranslations.ToString();
            return ReturnString;

        }      
        public void ViewXML()
        {
            Console.WriteLine();
            Console.WriteLine("XML Ausgabe der Listen:");
            if (Units.Count == 0) Console.WriteLine("-------> Alle Listen leer <-------");
            else
            {
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(GetType());
                x.Serialize(Console.Out, this);

            }
        }

    }
}