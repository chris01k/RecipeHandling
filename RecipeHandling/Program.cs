/* RecipeHandling: Testprogramm zur Vorarbeit im Projekt "Jamie". Erste Version ist eine
 * Kommandozeilen-Anwendung mit rudimentärem Menü zum Test der Unit-Listen-Funktionen und
 * möglicher Algoritmen zur Umrechnung von Units
 * Autor: Klaus Christochowitz  08-2016
 * Version 0.1  - 2016-08-17: Menü ist implementiert - RecipeHandlingList Methoden (ClearList,  
 *                            OpenList, ShowList, ViewList und ViewXML) wurden implementiert.
 * Version 0.11 - 2016-08-20: RecipeHandlingList.Units umgebaut auf ObservableCollection. 
 *                            RecipeHandlingList Methoden hinzu: AddUnit, RemoveUnit
 * Version 0.12 - 2016-08-21: Unit.maxUnitID enfernt, RecipeHandlingList.UnitTranslation hinzu
 *                            RecipeHandlingList Methoden hinzu: SelectUnit , ClearLists
 *                            RecipeHandlingListHandler hinzu: Methoden OpenList, SaveList 
 * Version 0.13 - 2016-08-22: Flags UnitTranslation.TranslationIndependenceType hinzu
 * Version 0.14 - 2016-08-23: Projektpflege - class UnitList umbenannt nach RecipeHandlingList
 *                                          - Unterordner Model angelegt
 *                                          - Übernahme aus Jamie: Ingredient, Recipe, Shoppinglist
 * Version 0.15 - 2016-08-24: Namespaces bereinigt: Jamie.Model
 *                            - ObservableObject und ObservableObject.RaisePropertyChanged in Ingredient 
 *                              auskommentiert
 * Version 0.16 - 2016-08-26: Refactoring: Kosmetik am Code
 *                            - RecipeHandlingList nach RecipeHandlingSet umbenannt.
 * Version 0.17 - 2016-08-28: Liste der Zutaten hinzugefügt - IngredientSet        
 *                            - Menüstruktur überarbeitet (Untermenüs je Objekt)
 *                            - Jeder Objektliste <Objekt>Set die Methoden hinzu (AddItem, Menu,ViewSet)
 * Version 0.18 - 2016-08-29: Klassen Ingredient, UnitTranslation und Unit standardisiert
 *                            - ViewSet / ToString überarbeitet und vereinheitlicht.
 *                            - PopulateSetWithDefault überarbeitet und vereinheitlicht.
 * Version 0.19 - 2016-08-30: Recipe und RecipeSet eingebunden
 * Version 0.20 - 2016-09-01: Unique ID einführt: Ingredient, Recipe, Unit, UnitSet - IngredientFlag im Namespace
 *                            Unique ID : MaxID zum Set, ID im <Objekt> kann einmal geschrieben werden
 *                            IngredientFlags im namespace kann von mehreren Klassen verwendet werden (z.B. Ingredient, Recipe)
 * Version 0.21 - 2016-09-01: Ersatz - Weitergabe von RecipeDataSet an <Object>Set & <Object> durch spezifisch erforderliche Sets:
 *                            - Übergabe des/der spezifischen Sets an die <Object>Set
 *                            - Umbau des <Object> Konstruktors auf Konstruktor([Set1[, Set2[...]]])
 *                            - Einbindung der Prüfroutine in PopulateObject: Units ()
 *                              noch offen UnitTranslation (Unit), Ingredient(), Recipe(Unit,Ingredient)
 * Version 0.22 - 2016-09-03: Recipe eingebunden
 *                            - Menu(), AddItem(), SelectItem() angepasst
 * Version 0.23 - 2016-09-03: Recipe inclusive IngredientItemSet eingefügt
 * Version 0.24 - 2016-09-04: Sicherung der Listen in einzelnen XML-Files implementiert.
 *                            - Grundsätze zur UnitTranslation beschrieben
 *                            - enum Jamie.Model.UnitType hinzu
 *                            - Flags UnitTranslation.TranslationIndependenceType überarbeitet
 *                            - Use Cases TranslationIndependenceType beschrieben
 * Version 0.25 - 2016-09-06: ListHelper hinzu, Ingredients erweitert 
 *                            - ListHelper ist eine statische Klasse für Listen Helper Methoden
 *                              ListHelper.ChangeIngredientFlagField hinzu
 *                              ListHelper.ChangeStringFlagField hinzu
 *                              ListHelper.ChangeUnitField hinzu
 *                            - Ingredients erweitert: _SelectedItem: Menu
 * Version 0.26 - 2016-09-07: UnitTranslation überarbeitet
 *                            - EditSelectedItem hinzu
 *                            - Umbau: BaseUnitSymbol --> BaseUnit, TargetUnitSymbol --> TargetUnit                         
 * Version 0.27 - 2016-09-13: UnitTranslations und div. Sets überarbeitet
 *                            - UnitTranslations: Einhaltung Regel "1. Es sollte alle Umrechnungen geben vom Typ "Immer gültig""
 *                              --> Immer gültig = Fall 0 = "Unabhängig von der Zutat, UnitTypen sind gleich"
 *                            - UnitSet, UnitTranslationSet, IngredientSet, RecipeSet: EvaluateMaxID
 *                              --> MaxID ermitteln nach Deserialize
 * Version 0.28 - 2016-09-17: Verweis auf IngredientSet implementiert (Zur Auswahl von AffectdIngredient)
 *                            - bei UnitTranslations: SetDataReference Methode hinzu, 
 *                            - Ingredients: Type hinzu
 * Version 0.29 - 2016-09-18: UnitTranslationSet: Eingabe aller UnitTranslation-Fälle implementiert
 * Version 0.30 - 2016-09-21: Klassen FoodPlanItem, FoodPlanItemSet, ShoppingListItem, ShoppingListItemSet hinzu
 * Version 0.31 - 2016-09-22: Bugfix: ShoppingListItemSet: UnitSetData und IngredientSetData bleiben bei Programmstart leer (Count == 0)
 * Version 0.32 - 2016-09-22: Foodplan.TransferToShoppingList(ShoppingListItemSet ShoppingListItems) hinzu
 *                            
 */

/* Version 0.33 - 2016-09-xx: x
 *                            - 
 *                            
 *                            
 *                            - offen: 
 *                              
 *                              --> checken, ob SelecedItem static sein muss
 *                              
 *                              
 *                              --> UnitTranslationSet.AddItem(UnitTranslation ItemToBeAdded): Reduzierung der Fälle
 *                              --> ShoppingList hinzu
 *                              --> Berechnung der Bedarfe in der Shopping List
 *                              
 *                             
 * 
 * Offene Fragen: 
 *
 * 
 * Checklisten : 1. Neue Objektliste einfügen....
 *                  - Interface IEquatable implementieren: Equals zufügen
 *                  - Methoden zufügen 
 *                  - Reihenfolge: <Object>ToString, <Set>ToString, <Set>ViewSet, <Set>Menu
 */

using Jamie.Model;
using System;


namespace Jamie.Main
{

    class Program
    {
        static void Main(string[] args)
        {
            string MenuInput = "";
            RecipeHandlingSetHandler JamieData = new RecipeHandlingSetHandler();
//            RecipeHandlingSetHandler JamieData = new RecipeHandlingSetHandler(true);

            while (MenuInput!="Q")
            {
                Console.WriteLine();
                Console.WriteLine("\nMenü");
                Console.WriteLine("----");

                Console.WriteLine("C  Clear Lists");
                Console.WriteLine("O  Open Lists");
                Console.WriteLine("S  Save Lists");
                Console.WriteLine();

                Console.WriteLine("I   Ingredient");
                Console.WriteLine("FP  FoodPlanItem");
                Console.WriteLine("R   Recipe");
                Console.WriteLine("U   Unit");
                Console.WriteLine("UT  Unit Translation");
                Console.WriteLine("SL  Shopping List");
                Console.WriteLine();
                Console.WriteLine("T  Transfer to Foodplan to Shopping List");
                Console.WriteLine("V  View Lists");
                Console.WriteLine("X  View XML File");
                Console.WriteLine("--------------------");
                Console.WriteLine("Q  Quit");

                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "C":
                        JamieData.ClearLists();
                        break;
                    case "O":
                        JamieData.OpenLists();
                        break;
                    case "S":
                        JamieData.SaveLists();
                        break;
                    case "I":
                        JamieData.JamieDataSet.Ingredients.Menu();
                        break;
                    case "FP":
                        JamieData.JamieDataSet.FoodPlanItems.Menu();
                        break;
                    case "R":
                        JamieData.JamieDataSet.Recipes.Menu();
                        break;
                    case "U":
                        JamieData.JamieDataSet.Units.Menu();
                        break;
                    case "UT":
                        JamieData.JamieDataSet.UnitTranslations.Menu();
                        break;
                    case "SL":
                        JamieData.JamieDataSet.ShoppingListItems.Menu();
                        break;
                    case "T":
                        JamieData.JamieDataSet.FoodPlanItems.TransferToShoppingList(JamieData.JamieDataSet.ShoppingListItems);
                        JamieData.JamieDataSet.ShoppingListItems.Menu();
                        break;
                    case "V":
                        JamieData.JamieDataSet.ViewSet();
                        break;
                    case "X":
                        JamieData.JamieDataSet.ViewXML();
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }
                
            }


        }

    }
}

