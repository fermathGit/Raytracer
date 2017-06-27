using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEngine {
    public abstract class Material {
        public double Reflectiveness { get; internal set; }

        public abstract Color Sample( Ray3 ray, Vector3 postion, Vector3 normal );
    }
}
