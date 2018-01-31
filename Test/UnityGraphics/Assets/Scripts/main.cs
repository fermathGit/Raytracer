using System;
using System.Collections;
using System.Collections.Generic;
using SimpleEngine;
using UnityEngine;

public class main : MonoBehaviour {
    #region member
    public GameObject PixelGo = null;
    public GameObject Parent = null;

    int c_side = 100;

    bool m_isChange = false;
    int m_left = 0;
    int m_right = 0;
    int m_up = 0;
    int m_down = 0;
    #endregion


    void Start() {
        if ( PixelGo != null ) {
            for ( int i = 0, imax = c_side; i < imax; ++i ) {
                for ( int j = 0, jmax = c_side; j < jmax; ++j ) {
                    var go = Instantiate( PixelGo, Parent.transform );
                    if ( go != null ) {
                        go.transform.localPosition = new UnityEngine.Vector3( i, j, 0 );

                        var render = go.GetComponent<Renderer>();
                        DrowRender_white( i, j, render );
                        //DrowRender_shade( i, j, render );
                        //DrowRender_chess( i, j, render );
                    }
                }
            }
        }

        //Client_Depth();
        //Client_Trace();
        Client_Reflection();
    }

    private void Client_Depth() {
        RenderDepth(
                    Parent,
                    new Sphere( new SimpleEngine.Vector3( 0, 10, -10 ), 10 ),
                    new PerspectiveCamera( new SimpleEngine.Vector3( 0, 10, 10 ), new SimpleEngine.Vector3( 0, 0, -1 ), new SimpleEngine.Vector3( 0, 1, 0 ), 90 ),
                    20 );
    }

    private void Client_Trace() {
        var plane = new SimpleEngine.Plane( new SimpleEngine.Vector3( 0, 1, 0 ), 0 );
        var sphere1 = new Sphere( new SimpleEngine.Vector3( -10, 10, -10 ), 10 );
        var sphere2 = new Sphere( new SimpleEngine.Vector3( 10, 10, -10 ), 10 );
        plane.material = new CheckerMaterial( 0.1, 0.5 );
        sphere1.material = new PhongMaterial( SimpleEngine.Color.red, SimpleEngine.Color.white, 16, 0.25 );
        sphere2.material = new PhongMaterial( SimpleEngine.Color.white, SimpleEngine.Color.white, 16, 0.25 );

        Union scene = new Union( new SimpleEngine.Object[] { plane, sphere1, sphere2 } );

        RayTrace(
            Parent,
            scene,
            new PerspectiveCamera( new SimpleEngine.Vector3( 0, 10, 10 ), new SimpleEngine.Vector3( 0, 0, -1 ), new SimpleEngine.Vector3( -1, 0, 0 ), 90 ) );
    }

    private void Client_Reflection() {
        var plane = new SimpleEngine.Plane( new SimpleEngine.Vector3( 0, 1, 0 ), 0 );
        var sphere1 = new Sphere( new SimpleEngine.Vector3( -10, 10, -10 ), 10 );
        var sphere2 = new Sphere( new SimpleEngine.Vector3( 10, 10, -10 ), 10 );
        plane.material = new CheckerMaterial( 0.1, 0.5 );
        sphere1.material = new PhongMaterial( SimpleEngine.Color.red, SimpleEngine.Color.white, 16 ,0.25);
        sphere2.material = new PhongMaterial( SimpleEngine.Color.blue, SimpleEngine.Color.white, 16, 0.25 );

        Union scene = new Union( new SimpleEngine.Object[] { plane, sphere1, sphere2 } );

        double maxReflect = 3;
        RayTraceReflection(
            Parent,
            scene,
            new PerspectiveCamera( new SimpleEngine.Vector3( 0, 5, 15 ), new SimpleEngine.Vector3( 0, 0, -1 ), new SimpleEngine.Vector3( -1, 0, 0 ), 90 ),
            maxReflect
            );
    }

    void Update() {
        if ( Input.GetKey( KeyCode.LeftArrow ) ) {
            m_left++;
            m_isChange = true;
        }
        if ( Input.GetKey( KeyCode.RightArrow ) ) {
            m_right++;
            m_isChange = true;
        }
        if ( Input.GetKey( KeyCode.UpArrow ) ) {
            m_up++;
            m_isChange = true;
        }
        if ( Input.GetKey( KeyCode.DownArrow ) ) {
            m_down++;
            m_isChange = true;
        }

        if ( m_isChange ) {
            ClearScreen();
            RenderDepth(
                Parent,
                new Sphere( new SimpleEngine.Vector3( 0 +m_up-m_down, 10 + m_left - m_right, -10 ), 10 ),
                new PerspectiveCamera( new SimpleEngine.Vector3( 0, 10, 10 ), new SimpleEngine.Vector3( 0, 0, -1 ), new SimpleEngine.Vector3( 0, 1, 0 ), 90 ),
                20 );
            m_isChange = false;
        }
    }

    #region func

    void DrowRender_white( int i, int j, Renderer render ) {
        render.material.color = UnityEngine.Color.white;
    }

    void DrowRender_shade( int i, int j, Renderer render ) {
        render.material.color = new UnityEngine.Color( 1.0f * i / c_side, 1.0f * j / c_side, 0, 1 );
    }

    void DrowRender_chess( int i, int j, Renderer render ) {
        float value = ( ( i / 2 + j / 2 ) ) % 2;
        render.material.color = new UnityEngine.Color( value, value, value );
    }

    void RayTraceReflection( GameObject parent, Union scene, PerspectiveCamera camera, double maxReflect ) {
        scene.Initialize();
        camera.Initialize();
        
        for ( var y = 0; y < c_side; y++ ) {
            double sy = 1 - (double)y / c_side;
            for ( var x = 0; x < c_side; x++ ) {
                double sx = (double)x / c_side;
                var ray = camera.GenerateRay( sx, sy );
                SimpleEngine.Color color = RayTraceRecursive( scene, ray, maxReflect );

                var curGo = parent.transform.GetChild( y * c_side + x ).gameObject;
                var render = curGo.GetComponent<Renderer>();
                render.material.color = new UnityEngine.Color( (float)color.r, (float)color.g, (float)color.b );
            }
        }
    }

    SimpleEngine.Color RayTraceRecursive( Union scene, Ray3 ray, double maxReflect ) {
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
            return color;
        } else
            return SimpleEngine.Color.black;
    }

    void RayTrace( GameObject parent, Union scene, PerspectiveCamera camera ) {
        scene.Initialize();
        camera.Initialize();

        for ( var y = 0; y < c_side; y++ ) {
            double sy = 1 - (double)y / c_side;
            for ( var x = 0; x < c_side; x++ ) {
                double sx = (double)x / c_side;
                var ray = camera.GenerateRay( sx, sy );
                var result = scene.Intersect( ray );
                if ( result.geometry != null ) {
                    var color = result.geometry.material.Sample( ray, result.position, result.normal );

                    var curGo = parent.transform.GetChild( y * c_side + x ).gameObject;
                    var render = curGo.GetComponent<Renderer>();
                    render.material.color = new UnityEngine.Color( (float)color.r, (float)color.g, (float)color.b );
                }
            }
        }
    }

    void RenderDepth( GameObject parent, Sphere scene, PerspectiveCamera camera, int maxDepth ) {
        scene.Initialize();
        camera.Initialize();

        for ( var y = 0; y < c_side; y++ ) {
            double sy = 1 - (double)y / c_side;
            for ( var x = 0; x < c_side; x++ ) {
                double sx = (double)x / c_side;
                var ray = camera.GenerateRay( sx, sy );
                var result = scene.Intersect( ray );
                if ( result.geometry != null ) {
                    double depth = 1 - Math.Min( ( result.distance / maxDepth ) * 1, 1 );
                    var curGo = parent.transform.GetChild( y * c_side + x ).gameObject;
                    var render = curGo.GetComponent<Renderer>();
                    //render.material.color = new Color( (float)depth, (float)depth, (float)depth, 1 );
                    render.material.color = new UnityEngine.Color( (float)( result.normal.x + 1 ), (float)( result.normal.y + 1 ), (float)( result.normal.z + 1 ) );
                    Debug.Log( depth );
                }
            }
        }
    }

    void ClearScreen() {
        for ( int i = 0, imax = c_side; i < imax; ++i ) {
            for ( int j = 0, jmax = c_side; j < jmax; ++j ) {
                var curGo = Parent.transform.GetChild( i * c_side + j ).gameObject;
                var render = curGo.GetComponent<Renderer>();
                render.material.color = UnityEngine.Color.white;
            }
        }
    }

    #endregion
}
