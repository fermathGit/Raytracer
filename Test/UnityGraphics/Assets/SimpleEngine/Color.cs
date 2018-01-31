using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEngine {
    public class Color {
        public double r;
        public double g;
        public double b;
        
        public Color( double r, double g, double b ) {
            this.r = r;
            this.g = g;
            this.b = b;
        }

        public Color Copy() {
            return new Color( r, g, b );
        }

        public Color Add( Color c ) {
            return new Color( r + c.r, g + c.g, b + c.b );
        }

        public Color Multiply( double s ) {
            return new Color( r * s, g * s, b * s );
        }

        public Color Modulate( Color c ) {
            return new Color( r * c.r, g * c.g, b * c.b );
        }

        static public Color black = new Color( 0, 0, 0 );
        static public Color white = new Color( 1, 1, 1 );
        static public Color red = new Color( 1, 0, 0 );
        static public Color green = new Color( 0, 1, 0 );
        static public Color blue = new Color( 0, 0, 1 );
    }
}
