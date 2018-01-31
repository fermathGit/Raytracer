using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleEngine;

namespace SimpleEngine {
    public class PerspectiveCamera {
        Vector3 eye;
        Vector3 front;//前
        Vector3 up;
        Vector3 right;
        Vector3 refUp;
        double fov;
        double fovScale;

        //左手法则
        public PerspectiveCamera( Vector3 eye, Vector3 front, Vector3 up, double fov ) {
            this.eye = eye;
            this.front = front;
            this.refUp = up;
            this.fov = fov;
        }

        public void Initialize() {
            this.right = this.front.Cross( this.refUp );
            this.up = this.right.Cross( this.front );
            this.fovScale = Math.Tan( this.fov * 0.5 * Math.PI / 180 ) * 2;
        }

        public Ray3 GenerateRay( double x, double y ) {
            var r = this.right.Multiply( ( x - 0.5 ) * this.fovScale );
            var u = this.up.Multiply( ( y - 0.5 ) * this.fovScale );
            return new Ray3( this.eye, this.front.Add( r ).Add( u ).Normalize() );
        }
    }
}

