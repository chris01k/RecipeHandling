/* RecipeHandling: Testprogramm zur Vorarbeit im Projekt "Jamie". Erste Version ist eine
 * Kommandozeilen-Anwendung mit rudimentärem Menü zum Test der Unit-Listen-Funktionen und
 * möglicher Algoritmen zur Umrechnung von Units
 * Autor: Klaus Christochowitz  08-2016
 * Version 0.1  - 2016-08-17: Menü ist implementiert - UnitList Methoden (ClearList,  
 *                            OpenList, ShowList, ViewList und ViewXML) wurden implementiert.
 * Version 0.11 - 2016-08-20: UnitList.Units umgebaut auf ObservableCollection. 
 *                            UnitList Methoden hinzu: AddUnit, RemoveUnit
 * Version 0.12 - 2016-08-21: Unit.maxUnitID enfernt, UnitList.UnitTranslation hinzu
 *                            UnitList Methoden hinzu: SelectUnit , ClearLists
 *                            UnitListHandler hinzu: Methoden OpenList, SaveList 
 * Version 0.13 - 2016-08-22: Flags UnitTranslation.TranslationIndependenceType hinzu
 *                            
 */
using RecipeHandling;
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
            UnitListHandler ULH = new UnitListHandler(true);
//            UnitListHandler ULH = new UnitListHandler();

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
                        ULH.UL.ClearLists();
                        break;
                    case "O":
                        ULH.OpenList();
                        break;
                    case "S":
                        ULH.SaveList();
                        break;
                    case "UA":
                        ULH.UL.AddUnit();
                        break;
                    case "UR":
                        ULH.UL.RemoveUnit();
                        break;
                    case "US":
                        Console.WriteLine(ULH.UL.SelectUnit());
                        break;
                    case "V":
                        ULH.UL.ShowList();
                        break;
                    case "X":
                        ULH.UL.ViewXML();
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }
                
            }


        }

    }
}

