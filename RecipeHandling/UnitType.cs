using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeHandling
{
    public class UnitType
    {
        private static int maxUnitTypeID = 0;
        private int unitTypeID;
        //private int parentUnitType;

        internal UnitType()
        {
            unitTypeID = ++maxUnitTypeID;
        }




    }
}