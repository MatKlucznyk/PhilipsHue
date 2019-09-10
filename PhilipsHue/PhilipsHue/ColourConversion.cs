using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Crestron.SimplSharp;

namespace PhilipsHue
{  
    internal static class ColourConverter
    {      
        public struct RGBColour
        {
            public readonly double red;
            public readonly double green;
            public readonly double blue;

            public RGBColour(double red, double green, double blue)
            {
                this.red = red;
                this.green = green;
                this.blue = blue;
            }
        }

        internal struct Point
        {
            public static readonly Point D65White = new Point(0.312713, 0.329016);

            public static readonly Point PhilipsWhite = new Point(0.322727, 0.32902);

            public Point(double x, double y)
            {
                this.x = x;
                this.y = y;
                this.z = 1.0 - x - y;
            }

            public readonly double x;
            public readonly double y;
            public readonly double z;
        }
         

        internal struct Gamut
        {
            public readonly Point Red;
            public readonly Point Green;
            public readonly Point Blue;

            public Gamut(Point red, Point green, Point blue)
            {
                this.Red = red;
                this.Green = green;
                this.Blue = blue;
            }

            public static readonly Gamut PhilipsWideGamut = new Gamut(
				new Point(0.700607, 0.299301),
				new Point(0.172416, 0.746797),
			    new Point(0.135503, 0.039879)
				);

            public static Gamut ForModel(string modelId)
		    {
			    // Details from http://www.developers.meethue.com/documentation/supported-lights

			    List<string> gamutA = new List<string>() {
				    "LLC001" /* Monet, Renoir, Mondriaan (gen II) */,
				    "LLC005" /* Bloom (gen II) */,
				    "LLC006" /* Iris (gen III) */,
				    "LLC007" /* Bloom, Aura (gen III) */,
				    "LLC010" /* Iris */,
				    "LLC011" /* Hue Bloom */,
				    "LLC012" /* Hue Bloom */,
				    "LLC013" /* Storylight */,
				    "LST001" /* Light Strips */
                };

			    List<string> gamutB = new List<string>() {
				    "LCT001" /* Hue A19 */,
				    "LCT007" /* Hue A19 */,
				    "LCT002" /* Hue BR30 */,
				    "LCT003" /* Hue GU10 */,
				    "LLM001" /* colour Light Module */
                };

			    List<string> gamutC = new List<string>() {
				    "LLC020" /* Hue Go */,
				    "LST002" /* Hue LightStrips Plus */
                };

			    if (gamutA.Contains(modelId))
			    {
				    return new Gamut(
                        new Point(0.704, 0.296),
                        new Point(0.2151, 0.7106),
                        new Point(0.138, 0.08)
                    );

			    }
			    else if (gamutB.Contains(modelId))
			    {
                    return new Gamut(
                        new Point(0.675, 0.322),
                        new Point(0.409, 0.518),
                        new Point(0.167, 0.04)
                    );
			    }
			    else if (gamutC.Contains(modelId))
			    {
				    return new Gamut(
					      new Point(0.692, 0.308),
					      new Point(0.17, 0.7),
					      new Point(0.153, 0.048)
				      );
			    }
			    else
			    {
				    // A gamut containing all colours
				    return new Gamut(
					    new Point(1.0F, 0.0F),
					    new Point(0.0F, 1.0F),
					    new Point(0.0F, 0.0F)
				    );
			    }
		    }

            public bool Contains(Point point)
            {
                return IsBelow(Blue, Green, point) &&
                    IsBelow(Green, Red, point) &&
                    IsAbove(Red, Blue, point);
            }

            private static bool IsBelow(Point a, Point b, Point point)
            {
                double slope = (a.y - b.y) / (a.x - b.x);
                double intercept = a.y - slope * a.x;

                double maxY = point.x * slope + intercept;
                return point.y <= maxY;
            }

            private static bool IsAbove(Point blue, Point red, Point point)
            {
                double slope = (blue.y - red.y) / (blue.x - red.x);
                double intercept = blue.y - slope * blue.x;

                double minY = point.x * slope + intercept;
                return point.y >= minY;
            }

            public Point NearestContainedPoint(Point point)
            {
                if (Contains(point)) return point;

                //find closest points on each line
                Point pAB = GetClosestPointOnLine(Red, Green, point);
                Point pAC = GetClosestPointOnLine(Red, Blue, point);
                Point pBC = GetClosestPointOnLine(Green, Blue, point);

                //get distances per point and see which point is closes to point
                double dAB = GetDistanceBetweenTwoPoints(point, pAB);
                double dAC = GetDistanceBetweenTwoPoints(point, pAC);
                double dBC = GetDistanceBetweenTwoPoints(point, pBC);

                double lowest = dAB;
                Point closestPoint = pAB;

                if (dAC < lowest)
                {
                    lowest = dAC;
                    closestPoint = pAC;
                }

                if (dBC < lowest)
                {
                    lowest = dBC;
                    closestPoint = pBC;
                }

                return closestPoint;
            }

            public static Point GetClosestPointOnLine(Point a, Point b, Point p)
            {
                Point AP = new Point(p.x - a.x, p.y - a.y);
                Point AB = new Point(b.x - a.x, b.y - a.y);

                double ab2 = AB.x * AB.x + AB.y * AB.y;
                double ap_ab = AP.x * AB.x + AP.y * AB.y;

                double t = ap_ab / ab2;

                if (t < 0.0f) t = 0.0f;

                else if (t > 1.0f) t = 1.0f;

                return new Point(a.x + AB.x * t, a.y + AB.y * t);
            }
        }

        private static double GetDistanceBetweenTwoPoints(Point one, Point two)
		{
			double dx = one.x - two.x; // horizontal difference
			double dy = one.y - two.y; // vertical difference

			return Math.Sqrt(dx * dx + dy * dy);
		}

        public static Point RgbToXY(RGBColour colour, string model)
        {
            double r = InverseGamma(colour.red);
            double g = InverseGamma(colour.green);
            double b = InverseGamma(colour.blue);

            double X = r * 0.664511f + g * 0.154324f + b * 0.162028f;
            double Y = r * 0.283881f + g * 0.668433f + b * 0.047685f;
            double Z = r * 0.000088f + g * 0.072310f + b * 0.986039f;

            Point xyPoint = new Point(0.0, 0.0);

            if ((X + Y + Z) > 0.0) xyPoint = new Point(X / (X + Y + Z), Y / (X + Y + Z));

            if (model != null)
            {
                Gamut gamut = Gamut.ForModel(model);

                return gamut.NearestContainedPoint(xyPoint);
            }

            return xyPoint;
        }

        public static Point PhilipsRGBtoXY(RGBColour colour, string model)
        {
            double r = InverseGamma(colour.red);
            double g = InverseGamma(colour.green);
            double b = InverseGamma(colour.blue);

            double X = r * 0.664511 + g * 0.0154324 + b * 0.162028;
            double Y = r * 0.283881 + g * 0.668433 + b * 0.047685;
            double Z = r * 0.000088 + g * 0.072310 + b * 0.986039;

            double x = X / (X + Y + Z);
            double y = Y / (X + Y + Z);

            Point xyPoint = new Point(x, y);

            return xyPoint;
        }

        public static RGBColour PhilipsXYtoRGB(Point point, string model)
        {
            double Y = 1.0f;
            double X = (Y / point.y) * point.x;
            double Z = (Y / point.y) * point.z;

            double r = X * 1.656492f - Y * 0.354851f - Z * 0.255038f;
            double g = -X * 0.707196f + Y * 1.655397f + Z * 0.036152f;
            double b =  X * 0.051713f - Y * 0.121364f + Z * 1.011530f;

            r = Gamma(r);
            g = Gamma(g);
            b = Gamma(b);

            RGBColour RGB = new RGBColour (r, g, b);

            return RGB;
        }
    
        
        public static RGBColour XYtoRGB(Point point, string model)
        {
            if (model != null)
            {
                Gamut gamut = Gamut.ForModel(model);
                point = gamut.NearestContainedPoint(point);
            }

            point = Gamut.PhilipsWideGamut.NearestContainedPoint(point);

            double Y = 1.0;
            double X = (Y / point.y) * point.x;
            double Z = (Y / point.y) * point.z;

            double r = X * 1.656492 - Y * 0.354851 - Z * 0.255038;
            double g = -X * 0.707196 + Y * 1.655397 + Z * 0.036152;
            double b = X * 0.051713 - Y * 0.121364 + Z * 1.011530;

            double maxComponent = Math.Max(Math.Max(r, g), b);

            if (maxComponent > 1.0)
            {
                r /= maxComponent;
                g /= maxComponent;
                b /= maxComponent;
            }

            r = Gamma(r);
            g = Gamma(g);
            b = Gamma(b);

            return new RGBColour(r, g, b);
        }

        

        private static double InverseGamma(double value)
        {
            double result;

            if (value > 0.04045) result = Math.Pow((value + 0.055) / (1.0 + 0.055), 2.4);

            else result = value / 12.92;

            return Bound(result);
        }

        private static double Gamma(double value)
        {
            double result;
            if (value <= 0.0031308) result = 12.92 * value;

            else result = (1.0 + 0.055) * Math.Pow(value, (1.0 / 2.4)) - 0.055;

            return Bound(result);
        }

        private static double Bound(double value)
        {
            return Math.Max(0.0, Math.Min(1.0, value));
        }
    }
}