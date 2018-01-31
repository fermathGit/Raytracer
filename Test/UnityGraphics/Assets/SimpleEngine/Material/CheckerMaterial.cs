using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEngine {
    public class CheckerMaterial: Material {
        public double Scale;

        public CheckerMaterial( double Scale ,double reflectiveness ) {
            this.Scale = Scale;
            this.Reflectiveness = reflectiveness;
        }

        public override Color Sample( Ray3 ray, Vector3 postion, Vector3 normal ) {
            return Math.Abs( ( Math.Floor( postion.x * 0.1 ) + Math.Floor( postion.z * Scale ) ) % 2 ) < 1 ? Color.white : Color.black;
        } 
    }
}
