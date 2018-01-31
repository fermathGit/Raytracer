using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEngine {
    public class IntersectResult {
        public Object geometry;
        public double distance;
        public Vector3 position;
        public Vector3 normal;

        public static IntersectResult noHit = new IntersectResult();

        public IntersectResult() {
            geometry = null;
            distance = 0;
            position = Vector3.zero;
            normal = Vector3.zero;
        }
    }
}
