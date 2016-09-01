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
 *                            
 *                             
 * 
 * Offene Fragen: - Globale Enums in einer Klasse zusammenfassen?
 *                - Konstruktoren internal oder public?
 *                - Bei Properties im Konstuktor: _Property = Property oder this.Property = Property
 *                
 * To Dos       : - InitMaxID nach Deserialize (für den Fall, dass Program mit leeren Listen beginnt)
 *                - Recipe einbinden....
 *                - IngredientType Definition globalisieren????
 *                - Variable ID kapseln
 *                - IngredientRecipeSet zufügen.
 * 
 * 
 * Checklisten : 1. Neue Objektliste einfügen....
 *                  - Interface IEquatable implementieren: Equals zufügen
 *                  - Methoden zufügen - Reihenfolge: <Object>ToString, <Set>ToString, <Set>ViewSet, <Set>Menu, 
 */

using Jamie.Model;
using System;




namespace Main
{

    class Program
    {
        static void Main(string[] args)
        {
            string MenuInput = "";
            RecipeHandlingSetHandler JamieData = new RecipeHandlingSetHandler(true);

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
                Console.WriteLine("R   Recipe");
                Console.WriteLine("U   Unit");
                Console.WriteLine("UT  Unit Translation");
                //                Console.WriteLine("UA  Add Unit");
                //                Console.WriteLine("UR  Remove Unit");
                //                Console.WriteLine("US Select Unit");
                Console.WriteLine();
                Console.WriteLine("V  View Unit List");
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
                    case "R":
                        JamieData.JamieDataSet.Recipes.Menu();
                        break;
                    case "U":
                        JamieData.JamieDataSet.Units.Menu();
                        break;
                    case "UT":
                        JamieData.JamieDataSet.UnitTranslations.Menu();
                        break;
/*                    case "UR":
                        JamieSetHandler.JamieDataSet.RemoveUnit();
                        break;
                    case "US":
                        Console.WriteLine(JamieSetHandler.JamieDataSet.SelectUnit());
                        break;
*/                    case "V":
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

