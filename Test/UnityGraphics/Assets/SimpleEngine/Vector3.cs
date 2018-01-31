using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEngine {
    public class Vector3 {
        public double x;
        public double y;
        public double z;
        public static Vector3 zero = new Vector3( 0, 0, 0 );

        public Vector3( double x, double y, double z ) {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3 Copy() {
            return new Vector3( x, y, z );
        }

        public double Length() {
            return (double)Math.Sqrt( x * x + y * y + z * z );
        }

        public double SqrLength() {
            return x * x + y * y + z * z;
        }

        public Vector3 Normalize() {
            var inv = 1 / Length();
            return new Vector3( x * inv, y * inv, z * inv );
        }

        public Vector3 Negate() {
            return new Vector3( -x, -y, -z );
        }

        public Vector3 Add( Vector3 v ) {
            return new Vector3( x + v.x, y + v.y, z + v.z );
        }

        public Vector3 Subtract( Vector3 v ) {
            return new Vector3( x - v.x, y - v.y, z - v.z );
        }

        public Vector3 Multiply( double f ) {
            return new Vector3( x * f, y * f, z * f );
        }

        public Vector3 Divide( double f ) {
            try {
                var invf = 1 / f;
                return new Vector3( x * invf, y * invf, z * invf );
            }
            catch ( Exception e ) {
                throw e;
            }
        }

        public double Dot( Vector3 v ) {
            return x * v.x + y * v.y + z * v.z;
        }

        //右手定则(握手)
        public Vector3 Cross( Vector3 v ) {
            return new Vector3( -z * v.y + y * v.z, z * v.x - x * v.z, -y * v.x + x * v.y );//(-a3*b2+a2*b3,a3*b1-a1*b3,-a2*b1+a1*b2)
        }
    }
}
