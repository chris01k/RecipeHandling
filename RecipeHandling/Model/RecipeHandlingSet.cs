using System;
using System.Linq;

namespace Jamie.Model
{
    public class RecipeDataSets
    {
        private IngredientSet _Ingredients;
        private RecipeSet _Recipes;
        private UnitSet _Units;
        private UnitTranslationSet _UnitTranslations;

        //Constructors
        public RecipeDataSets()
        {
            _Ingredients = new IngredientSet(this);
            _Recipes = new RecipeSet(this);
            _Units = new UnitSet(this);
            _UnitTranslations = new UnitTranslationSet(this);
        }
        public RecipeDataSets(bool ToBePopulatedWithDefaults)
        {
            _Ingredients = new IngredientSet(this);
            _Recipes = new RecipeSet(this);
            _Units = new UnitSet(this);
            _UnitTranslations = new UnitTranslationSet(this);

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
        public void AddUnit()
        {
            Unit UnitToBeAdded = new Unit(true);
            AddUnit(UnitToBeAdded);
        }
        public void AddUnit(Unit UnitToBeAdded)
        {
            if (!Units.Contains(UnitToBeAdded)) Units.Add(UnitToBeAdded);
            else Console.WriteLine("Die Unit ist bereits vorhanden: \n {0}", UnitToBeAdded);
        }
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
            Ingredients.PopulateSetWithDefaults();
            Units.PopulateSetWithDefaults();
            UnitTranslations.PopulateSetWithDefaults();
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
            UnitToBeRemoved.UnitSymbol = UnitSymbolToBeRemoved;

            if (Units.Contains(UnitToBeRemoved)) Units.Remove(UnitToBeRemoved);
            else Console.WriteLine("UnitSymbol {0} konnte nicht gefunden werden", UnitSymbolToBeRemoved);
                       

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