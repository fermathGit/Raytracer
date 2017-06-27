using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEngine {
    public class Sphere : Object {
        public Vector3 center;
        public double radius;
        public double sqrRadius;

        public Sphere( Vector3 center, double radius ) {
            this.center = center;
            this.radius = radius;
        }

        public override void Initialize() {
            sqrRadius = radius * radius;
        }

        public Sphere Copy() {
            return new Sphere( center.Copy(), radius );
        }

        public override IntersectResult Intersect( Ray3 ray ) {
            var v = ray.origin.Subtract( center );
            var a0 = v.SqrLength() - sqrRadius;
            var DdotV = ray.direction.Dot( v );

            if ( DdotV <= 0 ) {//可能是因为result.distance需要大于0
                var discr = DdotV * DdotV - a0;
                if ( discr >= 0 ) {
                    var result = new IntersectResult();
                    result.geometry = this;
                    result.distance = -DdotV - Math.Sqrt( discr );
                    result.position = ray.GetPoint( result.distance );
                    result.normal = result.position.Subtract( center ).Normalize();
                    return result;
                }
            }

            return IntersectResult.noHit;
        }
    }
}
