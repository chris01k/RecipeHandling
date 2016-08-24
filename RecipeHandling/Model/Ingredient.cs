
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using GalaSoft.MvvmLight;
using System.Text.RegularExpressions;


namespace Jamie.Model
{
    // Ein Rezept enthält eine Zutatenliste bestehend aus x Einträgen, wobei jeder Eintrag eine Zutat sowie die erforderliche Menge beschreibt.
    public class IngredientRecipeSet : ObservableCollection<IngredientItem>
    {
    }


    public class IngredientItem  //: ObservableObject
    {
        Ingredient _SpecificIngredient;

        public IngredientItem()
        {
            _SpecificIngredient = new Ingredient();
        }

        public Ingredient SpecificIngredient
        {
            get { return _SpecificIngredient; }
            set
            {
                if (_SpecificIngredient == value)
                    return;

                _SpecificIngredient = value;
//                RaisePropertyChanged(() => SpecificIngredient);
            }
        }

        public string Name
        {
            get { return _SpecificIngredient != null ? _SpecificIngredient.Name : ""; }
            set
            {
                if (_SpecificIngredient.Name == value)
                    return;

                _SpecificIngredient.Name = value;
                //RaisePropertyChanged(() => Name);
            }
        }

        private string _Unit;
        public string Unit
        {
            get { return _Unit; }
            set
            {
                if (_Unit == value)
                    return;

                _Unit = value;
//                RaisePropertyChanged(() => Unit);

            }
        }

        float? _Quantity;
        public float? Quantity
        {
            get { return _Quantity; }
            set
            {
                if (_Quantity == value)
                    return;

                _Quantity = value;
//                RaisePropertyChanged(() => Quantity);

            }
        }


    }



    /* Eine Zutat beschreibt ein Produkt, welches in einem Rezept verarbeitet werden kann. Zutaten werden im Gegensatz zu Werkzeugen verbraucht. 
     * Hat Eigenschaften: x kcal/100g, Ernährungsampel (rot, gelb, grün)
     * länderspezifische Zuordnung?
     */

    public class Ingredient //: ObservableObject
    {
        string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name == value)
                    return;

                _Name = value;
//                RaisePropertyChanged(() => Name);
            }
        }



        private bool? _IsVegetarian;

        public bool? IsVegetarian
        {
            get { return _IsVegetarian; }
            set
            {
                if (_IsVegetarian == value)
                    return;

                _IsVegetarian = value;
//                RaisePropertyChanged(() => IsVegetarian);

            }
        }

    }
}
