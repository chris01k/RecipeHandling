/* RecipeHandling: Testprogramm zur Vorarbeit im Projekt "Jamie". Erste Version ist eine
 * Kommandozeilen-Anwendung mit rudimentärem Menü zum Test der Unit-Listen-Funktionen und
 * möglicher Algoritmen zur Umrechnung von Units
 * Autor: Klaus Christochowitz  08-2016
 * Version 0.1  - 2016-08-17: Menü ist implementiert - UnitList Methoden (ClearList,  
 *                            OpenList, ShowList, ViewList und ViewXML) wurden implementiert.
 * Version 0.11 - 2016-08-20: UnitList umgebaut auf ObservableCollection - UnitList Methoden hinzu
 *                            (AddUnit, RemoveUnit, SaveList )
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
            UnitList UL = new UnitList();


            while (MenuInput!="Q")
            {
                Console.WriteLine();
                Console.WriteLine("\nMenü");
                Console.WriteLine("----");

                Console.WriteLine("A Add Unit");
                Console.WriteLine("C Clear Unit List");
                Console.WriteLine("O Open Unit List");
                Console.WriteLine("R Remove Unit");
                Console.WriteLine("S Save Unit List");
                Console.WriteLine("V View Unit List");
                Console.WriteLine("X View XML File");
                Console.WriteLine();
                Console.WriteLine("Q Quit");

                Console.WriteLine();
                Console.Write("Ihre Eingabe:");
                MenuInput = Console.ReadLine().ToUpper();

                switch (MenuInput)
                {
                    case "A":
                        UL.AddUnit();
                        break;
                    case "C":
                        UL.Units.Clear();
                        break;
                    case "O":
                        UL.OpenList();
                        break;
                    case "R":
                        UL.RemoveUnit();
                        break;
                    case "S":
                        UL.SaveList();
                        break;
                    case "V":
                        UL.ShowList();
                        break;
                    case "X":
                        UL.ViewXML();
                        break;
                    default:
                        Console.WriteLine();
                        break;
                }
                
            }


            Console.WriteLine("UnitList");


            Console.WriteLine();
            foreach (Unit ListItem in UL.Units)
                Console.WriteLine(ListItem);

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(UL.GetType());
            x.Serialize(Console.Out, UL);




        }
    }
}

