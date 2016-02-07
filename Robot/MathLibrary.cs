using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot
{
    /// <summary>
    /// Knihovna matematických funkcí
    /// </summary>
    public class MathLibrary
    {
        private MathLibrary(){}

        /// <summary>
        /// Zjistí, zda je bod uvnitř kružnice
        /// </summary>
        /// <param name="x">souřadnice x bodu</param>
        /// <param name="y">souřadnice y bodu</param>
        /// <param name="xS">souřadnice x středu kružnice</param>
        /// <param name="yS">souřadnice y středu kružnice</param>
        /// <param name="r">poloměr kružnice</param>
        /// <returns>true pokud je bod uvnitř kružnice</returns>
        public static bool isPointInCircle(int x, int y, int xS, int yS, int r){
            //mocnost bodu ke kružnici
            if (((x - xS) * (x - xS)) +((y - yS) * (y - yS)) - r * r < 0) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Pokud je bod uvnitř kružnice, tak vrátí tento bod a pokud ne tak vrátí nebližsí bod na kružnici
        /// </summary>
        /// <param name="x">souřadnice x bodu</param>
        /// <param name="y">souřadnice y bodu</param>
        /// <param name="xS">souřadnice x středu kružnice</param>
        /// <param name="yS">souřadnice y středu kružnice</param>
        /// <param name="r">poloměr kružnice</param>
        /// <returns>nejbližší bod uvnitř kružnice</returns>
        public static Point convertPointToCircle(int x, int y, int xS, int yS, int r)
        {
            if(isPointInCircle(x,y,xS, yS, r))
            {
                return new Point(x, y);    
            }else
            {
                double vX = x - xS;
                double vY = y - yS;
                double magV = Math.Sqrt(vX * vX + vY * vY);
                int aX = (int) (xS + vX / magV * r);
                int aY = (int) (yS + vY / magV * r);
                return new Point(aX, aY);
            }
        }

        /// <summary>
        /// Vrátí vzdálenost mezi body
        /// </summary>
        /// <param name="xA">x souřadnice bodu A</param>
        /// <param name="yA">y souřadnice bodu A</param>
        /// <param name="xB">x souřadnice bodu B</param>
        /// <param name="yB">y souřadnice bodu B</param>
        /// <returns></returns>
        public static double getPointsDistance(int xA, int yA, int xB, int yB)
        {
            return Math.Sqrt(((xA-xB)*(xA-xB))+((yA-yB)*(yA-yB)));
        }

        /// <summary>
        /// Vrátí úhel určený třemi body
        /// </summary>
        /// <param name="xV">x souřadnice vrcholu</param>
        /// <param name="yV">y souřadnice vrcholu</param>
        /// <param name="xA">x souřadnice bodu A</param>
        /// <param name="yA">y souřadnice bodu A</param>
        /// <param name="xB">x souřadnice bodu B</param>
        /// <param name="yB">y souřadnice bodu B</param>
        /// <returns>ůhel ve stupních</returns>
        public static double getAngle(int xV, int yV, int xA, int yA, int xB, int yB) {
            double VAdistance = getPointsDistance(xV, yV, xA, yA);
            double VBdistance = getPointsDistance(xV, yV, xB, yB);
            double ABdistance = getPointsDistance(xB, yB, xA, yA);
            double temp = 2 * VAdistance * VBdistance;
            if (temp == 0) {
                return 0;
            }
            double angle = Math.Acos(((VAdistance * VAdistance) + (VBdistance * VBdistance) - (ABdistance * ABdistance)) / temp);
            double degreeAngle = angle * 180 / Math.PI;
            return degreeAngle;
        }
    }
}
