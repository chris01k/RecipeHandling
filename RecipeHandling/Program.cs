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
 * Version 0.16 - 2016-08-26: Kosmetik am Code
 *                            - RecipeHandlingList nach RecipeHandlingSet umbenannt.
 *                                          
 * Offene Fragen: 
 *                - 

 */
using RecipeHandling;

using Jamie.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace RecipeHandling
{
    class Program
    {   
        static void Main(string[] args)
        {
            string MenuInput = "";
            RecipeHandlingSetHandler RHLH = new RecipeHandlingSetHandler(true);
//            RecipeHandlingSetHandler RHLH = new RecipeHandlingSetHandler();

            while (MenuInput!="Q")
            {
                Console.WriteLine();
                Console.WriteLine("\nMenü");
                Console.WriteLine("----");

                Console.WriteLine("C  Clear Unit List");
                Console.WriteLine("O  Open Unit List");
                Console.WriteLine("S  Save Unit List");
                Console.WriteLine("UA  Add Unit");
                Console.WriteLine("UR  Remove Unit");
                Console.WriteLine("US Select Unit");
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
                        RHLH.RHL.ClearLists();
                        break;
                    case "O":
                        RHLH.OpenList();
                        break;
                    case "S":
                        RHLH.SaveList();
                        break;
                    case "UA":
                        RHLH.RHL.AddUnit();
                        break;
                    case "UR":
                        RHLH.RHL.RemoveUnit();
                        break;
                    case "US":
                        Console.WriteLine(RHLH.RHL.SelectUnit());
                        break;
                    case "V":
                        RHLH.RHL.ShowSet();
                        break;
                    case "X":
                        RHLH.RHL.ViewXML();
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }
                
            }


        }

    }
}

