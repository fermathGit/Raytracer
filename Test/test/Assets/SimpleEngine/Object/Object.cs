using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEngine {
    public abstract class Object {
        public Material material;

        public abstract void Initialize();
        public abstract IntersectResult Intersect( Ray3 ray );
    }
}
