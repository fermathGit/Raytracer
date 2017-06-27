using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEngine {
    public class Plane : Object {
        Vector3 normal;
        Vector3 position;
        double d;

        //n*x=d(d为空间原点至平面最短距离)
        public Plane( Vector3 normal, double d ) {
            this.normal = normal;
            this.d = d;
        }

        public Plane Copy() {
            return new Plane( normal, d );
        }

        public override void Initialize() {
            position = normal.Multiply( d );
        }

        public override IntersectResult Intersect( Ray3 ray ) {
            var a = ray.direction.Dot( normal );
            if ( a >= 0 ) {
                return IntersectResult.noHit;
            }

            var b = normal.Dot( ray.origin.Subtract( position ) );
            var result = new IntersectResult();
            result.geometry = this;
            result.distance = -b / a;
            result.position = ray.GetPoint( result.distance );
            result.normal = normal;
            return result;
        }
    }
}
