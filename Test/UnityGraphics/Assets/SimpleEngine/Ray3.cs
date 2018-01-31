using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEngine {
    public class Ray3 {
        public Vector3 origin;
        public Vector3 direction;

        public Ray3( Vector3 origin, Vector3 direction ) {
            this.origin = origin;
            this.direction = direction;
        }

        public Vector3 GetPoint( double t ) {
            return origin.Add( direction.Multiply( t ) );
        }
    }
}
