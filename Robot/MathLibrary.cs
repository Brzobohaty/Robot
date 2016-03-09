using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
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
        /// Pokud je bod uvnitř kružnice, tak vrátí tento bod a pokud ne, tak vrátí nebližsí bod na kružnici
        /// </summary>
        /// <param name="x">souřadnice x bodu</param>
        /// <param name="y">souřadnice y bodu</param>
        /// <param name="xS">souřadnice x středu kružnice</param>
        /// <param name="yS">souřadnice y středu kružnice</param>
        /// <param name="r">poloměr kružnice</param>
        /// <returns>nejbližší bod uvnitř kružnice</returns>
        public static System.Drawing.Point convertPointToCircle(int x, int y, int xS, int yS, int r)
        {
            if(isPointInCircle(x,y,xS, yS, r))
            {
                return new System.Drawing.Point(x, y);    
            }else
            {
                double vX = x - xS;
                double vY = y - yS;
                double magV = Math.Sqrt(vX * vX + vY * vY);
                int aX = (int) (xS + vX / magV * r);
                int aY = (int) (yS + vY / magV * r);
                return new System.Drawing.Point(aX, aY);
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
        /// Vrátí levotočivý úhel určený třemi body
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
            double position = Math.Sign((xB - xA) * (yV - yA) - (yB - yA) * (xV - xA));
            if (position > 0) {
                degreeAngle = 360- degreeAngle;
            }
            return degreeAngle;
        }

        /// <summary>
        /// Vrátí druhý bod úsečky, který s osu x svírá daný úhel a je od prvního bodu vzdálen o danou hodnotu
        /// </summary>
        /// <param name="x">x souřadnice počátku úsečky</param>
        /// <param name="y">y souřadnice počátku úsečky</param>
        /// <param name="angle">úhel mezi úsečky a osou x</param>
        /// <param name="distance">vzdálenost bodu od prvního bodu úsečky</param>
        /// <returns>souřadnice drhého bodu úsečky</returns>
        public static System.Drawing.Point getPointOnLine(int x, int y, int angle, int distance) {
            double anglee = (angle * (Math.PI / 180));
            return new System.Drawing.Point((int)Math.Round((x + (distance * Math.Cos(anglee)))), (int)Math.Round((y + (distance * Math.Sin(anglee)))));
        }

        /// <summary>
        /// Přepočítá hodnotu z jednoho číselného rozsahu do druhého
        /// </summary>
        /// <param name="oldValue">hodnota ve starém rozsahu, kterou chceme přepočítat do nového</param>
        /// <param name="oldMin">minimum starého rozsahu</param>
        /// <param name="oldMax">maximum starého rozsahu</param>
        /// <param name="newMin">minimum nového rozsahu</param>
        /// <param name="newMax">maximum nového rozsahu</param>
        /// <returns></returns>
        public static int changeScale(int oldValue,int oldMin,int oldMax,int newMin,int newMax)
        {
            int oldRange = (oldMax - oldMin);
            if (oldRange == 0)
                return newMin;
            else
            {
                int newRange = (newMax - newMin);
                int newValue = (((oldValue - oldMin) * newRange) / oldRange) + newMin;
                return newValue;
            }
        }

        /// <summary>
        /// Přepočítá hodnotu z jednoho číselného rozsahu do druhého logaritmického rozsahu
        /// </summary>
        /// <param name="oldValue">hodnota ve starém rozsahu, kterou chceme přepočítat do nového</param>
        /// <param name="oldMin">minimum starého rozsahu</param>
        /// <param name="oldMax">maximum starého rozsahu</param>
        /// <param name="newMin">minimum nového rozsahu</param>
        /// <param name="newMax">maximum nového rozsahu</param>
        /// <returns>hodnotu v novém rozsahu</returns>
        public static double changeScaleLog(double oldValue, double oldMin, double oldMax, double newMin, double newMax)
        {
            double division = newMax / newMin;
            double shift = 0;
            if (division < 0) {
                shift = 1 - newMin;
                newMax = newMax + shift;
                newMin = newMin + shift;
            }
            //y = a exp bx
            double b = Math.Log(newMax / newMin ) / (oldMax - oldMin);
            double a = newMax / Math.Exp(b * oldMax);
            double newValue = a * Math.Exp(b * oldValue);
            if (division < 0) {
                return newValue - shift;
            }
            return newValue;
        }

        /// <summary>
        /// Vrátí bod přímce, který od daného bodu na přímce vzdálenou o danou velikost
        /// </summary>
        /// <param name="line">přímka</param>
        /// <param name="origin">počáteční bod</param>
        /// <param name="distance">vzdálenost mezi body</param>
        /// <returns></returns>
        public static Point getPointOnLineInDistance(Line line, Point origin, double distance)
        {
            Point endPoint = new Point();

            //distance^2 = (x1-x2)^2 + (y1-y2)^2
            Vector<double> leftSide = Vector<double>.Build.Dense(new double[] { 0, 0, distance * distance }); // [0]x^2 + [1]x + [2]
            Vector<double> xPow = Vector<double>.Build.Dense(new double[] { 1, -2 * origin.X, origin.X * origin.X }); // [0]x^2 + [1]x + [2]
            Vector<double> yPow = Vector<double>.Build.Dense(new double[] { line.k * line.k, 2 * (line.q - origin.Y) * line.k, (line.q - origin.Y) * (line.q - origin.Y) }); // [0]x^2 + [1]x + [2]
            Vector<double> rightSide = xPow + yPow;
            Vector<double> quadraticEqutation = rightSide - leftSide;
            Tuple<Complex, Complex> roots = MathNet.Numerics.FindRoots.Quadratic(quadraticEqutation.ElementAt(2), quadraticEqutation.ElementAt(1), quadraticEqutation.ElementAt(0));

            //výběr toho bodu, který je vzdálenější od středu
            endPoint.X = roots.Item1.Real;
            if (line.vertical)
            {
                endPoint.Y = origin.Y + Math.Sign(origin.Y) * distance;
            }
            else
            {
                Point endPointTemp = new Point(roots.Item2.Real, 0);
                Point trueOrigin = new Point(0, 0);
                endPoint.Y = line.getY(roots.Item1.Real);
                endPointTemp.Y = line.getY(roots.Item2.Real);

                if (getDistance(trueOrigin, endPoint) < getDistance(trueOrigin, endPointTemp))
                {
                    endPoint = endPointTemp;
                }
            }

            return endPoint;
        }

        /// <summary>
        /// Vrátí vzdálenost dvou bodů
        /// </summary>
        /// <param name="point1">bod 1</param>
        /// <param name="point2">bod 2</param>
        /// <returns>vzdálenost</returns>
        public static double getDistance(Point point1, Point point2)
        {
            double[] a = new double[2] { point1.X, point1.Y };
            double[] b = new double[2] { point2.X, point2.Y };
            return MathNet.Numerics.Distance.Euclidean(a, b);
        }

        /// <summary>
        /// Vrátí odchylku dvou přímek
        /// </summary>
        /// <param name="line1">přímka 1</param>
        /// <param name="line2">přímka 2</param>
        /// <returns>Odchylka dvou přímek ve stupních (0 - 180)</returns>
        public static double getDeviation(Line line1, Line line2)
        {
            //uv = | u |⋅| v | cos φ
            Vector<double> u = Vector<double>.Build.Dense(new double[] { line1.b, -line1.a });
            Vector<double> v = Vector<double>.Build.Dense(new double[] { line2.b, -line2.a });
            double uv = u.DotProduct(v);
            double angle = Math.Acos(uv / (u.L2Norm() * v.L2Norm()));
            return radiansToDegrees(angle);
        }

        /// <summary>
        /// Převede stupně na radiány
        /// </summary>
        /// <param name="angle">úhel ve stupních</param>
        /// <returns>úhel v radiánech</returns>
        public static double degreeToRadians(double angle)
        {
            return angle * Math.PI / 180;
        }

        /// <summary>
        /// Převede radiány na stupně
        /// </summary>
        /// <param name="angle">úhel v radiánech</param>
        /// <returns>úhel ve stupních</returns>
        public static double radiansToDegrees(double angle)
        {
            return angle * 180 / Math.PI;
        }

        /// <summary>
        /// vypočítá délku oblouku daného úhlem a poloměrem
        /// </summary>
        /// <param name="radius">poloměr</param>
        /// <param name="angle">úhel (ve stupních)</param>
        /// <returns></returns>
        public static double getArcLength(double radius,double angle) {
            return 2 * angle * Math.PI * radius / 360;
        }

        /// <summary>
        /// Spočítá lineární interpolaci bodu na dané přímce
        /// </summary>
        /// <param name="x">x souřadnice bodu</param>
        /// <param name="x0">x souřadnice jednoho bodu na přímce</param>
        /// <param name="x1">x souřadnice druhého bodu na přímce</param>
        /// <param name="y0">y souřadnice jednoho bodu na přímce</param>
        /// <param name="y1">y souřadnice druhého bodu na přímce</param>
        /// <returns></returns>
        public static double linearInterpolation(double x, double x0, double x1, double y0, double y1)
        {
            if ((x1 - x0) == 0)
            {
                return (y0 + y1) / 2;
            }
            return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        }

        ///*********************CLASSES*********************************************************************************************///

        /// <summary>
        /// Bod
        /// </summary>
        public class Point
        {
            public double X { get; set; } //x souřadnice bodu
            public double Y { get; set; } //y souřadnice bodu

            /// <summary>
            /// Vytvoří bod v počátku
            /// </summary>
            public Point()
            {
                X = 0;
                Y = 0;
            }

            /// <summary>
            /// Vytvoří bod s danými souřadnicemi
            /// </summary>
            /// <param name="x">x souřadnice bodu</param>
            /// <param name="y">y souřadnice bodu</param>
            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }
        }

        /// <summary>
        /// Přímka
        /// </summary>
        public class Line
        {
            public bool vertical { get; } = false; //příznak, že se jedná o vertikální přímku
            //koeficienty směrnicového tvaru rovnice y=kx+q
            public double k { get; }
            public double q { get; }
            //koeficienty obecného tvaru rovnice ax + by + c = 0
            public double a { get; }
            public double b { get; }
            public double c { get; }

            /// <summary>
            /// Vytvoří přímku z bodu a úhlu k ose x
            /// </summary>
            /// <param name="x">x souřadnice bodu na přímce</param>
            /// <param name="y">y souřadnice bodu na přímce</param>
            /// <param name="angle">úhel relativně k ose x</param>
            public Line(double x, double y, double angle)
            {
                if (angle == 90 || angle == 270 || angle == -90 || angle == -270)
                {
                    vertical = true;
                }
                k = Math.Tan(degreeToRadians(angle));
                q = y - (Math.Round(k, 5) * x);

                a = k;
                b = -1;
                c = q;
            }

            /// <summary>
            /// Vytvoří přímku zadanou koeficienty obecné rovnice (ax+by+c=0)
            /// </summary>
            /// <param name="a">koeficient u x</param>
            /// <param name="b">koeficient u y</param>
            /// <param name="c">koeficient konstanty</param>
            /// <param name="general">pouze příznak odlišující konstruktor</param>
            public Line(double a, double b, double c, bool general)
            {
                this.a = a;
                this.b = b;
                this.c = c;

                k = -a / b;
                q = -c / b;

                double angle = Math.Atan(k);
                if (Math.Abs(angle - 1.57) < 0.2 || Math.Abs(angle - 4.71) < 0.2 || Math.Abs(angle + 1.57) < 0.2 || Math.Abs(angle + 4.71) < 0.2)
                {
                    vertical = true;
                }
            }

            /// <summary>
            /// Vrátí y souřadnici bodu na přímce, který má danou souřadnici x
            /// </summary>
            /// <param name="x">x souřadnice bodu</param>
            /// <returns>y souřadnice bodu</returns>
            public double getY(double x)
            {
                return k * x + q;
            }

            /// <summary>
            /// Vrátí kolmici v daném bodě
            /// </summary>
            /// <param name="point">bod</param>
            /// <returns>kolmici</returns>
            public Line getNormal(Point point)
            {
                return new Line(b, -a, -((b * point.X) - (a * point.Y)), true);
            }
        }

        /// <summary>
        /// Kružnice
        /// </summary>
        public class Circle
        {
            public Point S { get; } //střed kružnice
            public double sX { get; } //x souřadnice středu
            public double sY { get; } //y souřadnice středu
            public double r { get; } //poloměr

            /// <summary>
            /// Vytvoří kružnici danou středem a bodem jenž na ní leží
            /// </summary>
            /// <param name="S">střed kružnice</param>
            /// <param name="P">bod na kru6nici</param>
            public Circle(Point S, Point P)
            {
                this.sX = S.X;
                this.sY = S.Y;
                this.S = S;
                r = getDistance(S, P);
            }

            /// <summary>
            /// Vrátí tečnu ke kružnici v daném bodě
            /// </summary>
            /// <param name="point">bod dotyku</param>
            /// <returns></returns>
            public Line getTangent(Point point)
            {
                //(x0-m)(x-m)+(y0-n)(y-n) = r^2
                double bracked1 = point.X - sX;
                Vector<double> bracked2 = Vector<double>.Build.Dense(new double[] { 1, 0, -sX }); // [0]x + [1]y + [2]
                double bracked3 = point.Y - sY;
                Vector<double> bracked4 = Vector<double>.Build.Dense(new double[] { 0, 1, -sY }); // [0]x + [1]y + [2]
                Vector<double> rightSide = Vector<double>.Build.Dense(new double[] { 0, 0, r * r }); // [0]x + [1]y + [2];
                Vector<double> line = (bracked1 * bracked2) + (bracked3 * bracked4) - rightSide;
                return new Line(line.ElementAt(0), line.ElementAt(1), line.ElementAt(2), true);
            }
        }
    }
}
