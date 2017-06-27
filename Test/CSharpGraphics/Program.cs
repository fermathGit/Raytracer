using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using SimpleEngine;

namespace CSharpGraphics {
    class Program {
        static void Main( string[] args ) {
            var plane = new Plane( new Vector3( 0, 1, 0 ), 0 );
            var sphere1 = new Sphere( new Vector3( -10, 10, -10 ), 10 );
            var sphere2 = new Sphere( new Vector3( 10, 10, -10 ), 10 );
            plane.material = new CheckerMaterial( 0.1, 0.5 );
            sphere1.material = new PhongMaterial( SimpleEngine.Color.red, SimpleEngine.Color.white, 16, 0.25 );
            sphere2.material = new PhongMaterial( SimpleEngine.Color.blue, SimpleEngine.Color.white, 16, 0.25 );

            Union scene = new Union( new SimpleEngine.Object[] { plane, sphere1, sphere2 } );

            double maxReflect = 3;
            RayTraceReflection(
                scene,
                new PerspectiveCamera( new SimpleEngine.Vector3( 0, 5, 15 ), new SimpleEngine.Vector3( 0, 0, -1 ), new SimpleEngine.Vector3(1, 0, 0 ), 90 ),
                maxReflect
                );

        }

        static void RayTraceReflection(  Union scene, PerspectiveCamera camera, double maxReflect ) {
            scene.Initialize();
            camera.Initialize();

            int width = 800;
            int height = 800;
            Bitmap b1 = new Bitmap( width, height );     //新建位图b1

            for ( int i = 0, imax = width; i < imax; ++i ) {
                double sy = 1 - (double)i / imax;
                for ( int j = 0, jmax = height; j < jmax; ++j ) {
                    double sx = (double)j / jmax;
                    var ray = camera.GenerateRay( sx, sy );
                    SimpleEngine.Color color = RayTraceRecursive( scene, ray, maxReflect );

                    System.Drawing.Color c = System.Drawing.Color.FromArgb( (int)Math.Floor( color.r * 255), (int)Math.Floor( color.g * 255 ), (int)Math.Floor( color.b * 255 ) );
                    
                    b1.SetPixel( i, j, c );
                }
            }

            b1.Save( @"D:\p1.bmp" ); //把b1存到D盘
            
        }

        static SimpleEngine.Color RayTraceRecursive( Union scene, Ray3 ray, double maxReflect ) {
            var result = scene.Intersect( ray );
            if ( result.geometry != null ) {
                var reflectiveness = result.geometry.material.Reflectiveness;
                var color = result.geometry.material.Sample( ray, result.position, result.normal );
                color = color.Multiply( 1 - reflectiveness );

                if ( reflectiveness > 0 && maxReflect > 0 ) {
                    var r = result.normal.Multiply( -2 * result.normal.Dot( ray.direction ) ).Add( ray.direction );
                    ray = new Ray3( result.position, r );
                    var reflectedColor = RayTraceRecursive( scene, ray, maxReflect - 1 );
                    color = color.Add( reflectedColor.Multiply( reflectiveness ) );
                }

                color.r = color.r > 1 ? 1 : color.r;
                color.g = color.g > 1 ? 1 : color.g;
                color.b = color.b > 1 ? 1 : color.b;
                return color;
            } else
                return SimpleEngine.Color.black;
        }

    }
}
