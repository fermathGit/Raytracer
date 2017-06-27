using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEngine {
    public class Union {
        Object[] geometries;

        public Union( Object[] geometries ) {
            this.geometries = geometries;
        }

        public void Initialize() {
            //foreach ( var obj in geometries ) {
            //    obj.Initialize();
            //}
            if ( geometries != null && geometries.Length > 0 ) {
                for ( int i = 0, imax = geometries.Length; i < imax; ++i ) {
                    var child = geometries[i];
                    if ( child != null ) {
                        child.Initialize();
                    }
                }
            }
        }

        public IntersectResult Intersect( Ray3 ray ) {
            double minDistance = Double.MaxValue;
            var minResult = IntersectResult.noHit;
            //foreach ( var obj in this.geometries ) {
            //    var result = obj.Intersect( ray );
            //    if ( result.geometry!=null && result.distance < minDistance ) {
            //        minDistance = result.distance;
            //        minResult = result;
            //    }
            //}

            if ( geometries != null && geometries.Length > 0 ) {
                for ( int i = 0, imax = geometries.Length; i < imax; ++i ) {
                    var child = geometries[i];
                    if ( child != null ) {
                        var result = child.Intersect( ray );
                        if ( result.geometry != null && result.distance < minDistance ) {
                            minDistance = result.distance;
                            minResult = result;
                        }
                    }
                }
            }
            return minResult;
        }
    }
}
