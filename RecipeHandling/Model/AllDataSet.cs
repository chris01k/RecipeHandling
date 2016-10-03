using System;
using System.Linq;

namespace Jamie.Model
{
    public class AllDataSets
    {
        private FoodPlanItemSet _FoodPlanItems; //related to Recipes
        private IngredientSet _Ingredients; //related to <no other Set>
        private RecipeSet _Recipes; // related to Ingredients, Units
        private ShoppingListItemSet _ShoppingListItems; //related to Ingredients, Units
        private UnitSet _Units; // related to <no other Set>
        private UnitTranslationSet _UnitTranslations; //related to Units


        //Constructors
        public AllDataSets()
        {
            _Units = new UnitSet();
            _UnitTranslations = new UnitTranslationSet(_Units);
            _Ingredients = new IngredientSet(_Units);
            _Recipes = new RecipeSet(_Units, _Ingredients);
            _FoodPlanItems = new FoodPlanItemSet();
            _ShoppingListItems = new ShoppingListItemSet();
            SetDataReference();
        }
        public AllDataSets(bool ToBePopulatedWithDefaults)
        {
            _Units = new UnitSet();
            _UnitTranslations = new UnitTranslationSet(_Units);
            _Ingredients = new IngredientSet(_Units);
            _Recipes = new RecipeSet(_Units, _Ingredients);
            _FoodPlanItems = new FoodPlanItemSet();
            _ShoppingListItems = new ShoppingListItemSet();
            SetDataReference();
            if (ToBePopulatedWithDefaults) PopulateSetWithDefaults();

        }

        //Properties
        public FoodPlanItemSet FoodPlanItems
        {
            get
            {
                return _FoodPlanItems;
            }

            set
            {
                _FoodPlanItems = value;
            }
        }
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
        public ShoppingListItemSet ShoppingListItems
        {
            get
            {
                return _ShoppingListItems;
            }

            set
            {
                _ShoppingListItems = value;
            }

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
            FoodPlanItems.Clear();
            Ingredients.Clear();
            Recipes.Clear();
            ShoppingListItems.Clear();
            Units.Clear();
            UnitTranslations.Clear();
            EvaluateMaxIDs();

        }
        public void EvaluateMaxIDs()
        {
            Ingredients.EvaluateMaxID();
            FoodPlanItems.EvaluateMaxID();
            Recipes.EvaluateMaxID();
            Units.EvaluateMaxID();
            UnitTranslations.EvaluateMaxID();
            ShoppingListItems.EvaluateMaxID();
        }
        public void PopulateSetWithDefaults()
        {
            Units.PopulateSetWithDefaults();
            UnitTranslations.PopulateSetWithDefaults();
            Ingredients.PopulateSetWithDefaults();
        }
        public void SetDataReference()
        {
            _UnitTranslations.SetDataReference(_Ingredients, _Units);
            _Ingredients.SetDataReference(_Units);
            _Recipes.SetDataReference(_Ingredients, _Units, _UnitTranslations);
            _FoodPlanItems.SetDataReference(_Recipes);
            _ShoppingListItems.SetDataReference(_Ingredients, _Units, _UnitTranslations);
        }
        public void SaveSet(string FileName) 
        {
            Units.SaveSet(FileName);
            UnitTranslations.SaveSet(FileName);
            Ingredients.SaveSet(FileName);
            Recipes.SaveSet(FileName);
        }// --> Data
        public void ViewSet()
        {
            Console.WriteLine();
            Console.WriteLine(ToString());
        }// --> View
        public override string ToString()
        {
            string ReturnString = "";

            ReturnString += FoodPlanItems.ToString();
            ReturnString += Ingredients.ToString();
            ReturnString += Recipes.ToString();
            ReturnString += Units.ToString();
            ReturnString += UnitTranslations.ToString();
            ReturnString += ShoppingListItems.ToString();

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
        }// --> View

    }
}