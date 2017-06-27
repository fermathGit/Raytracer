using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleEngine {
    public class PhongMaterial : Material {
        public Color diffuse;
        public Color specular;
        public double shininess;

        public PhongMaterial( Color diffuse, Color specular, double shininess ,double reflectiveness ) {
            this.diffuse = diffuse;
            this.specular = specular;
            this.shininess = shininess;
            this.Reflectiveness = reflectiveness;
        }

        Vector3 lightDir = new Vector3( 1, 0, 0 ).Normalize();
        Color lightColor = Color.white;

        public override Color Sample( Ray3 ray, Vector3 position, Vector3 normal ) {
            var NdotL = normal.Dot( lightDir );
            var H = ( lightDir.Subtract( ray.direction ) ).Normalize();
            var NdotH = normal.Dot( H );
            var diffuseTerm = diffuse.Multiply( Math.Max( NdotL, 0 ) );
            var specularTermm = specular.Multiply( Math.Pow( Math.Max( NdotH, 0 ), shininess ) );
            return lightColor.Modulate( diffuseTerm.Add( specularTermm ) );
        }
    }
}
