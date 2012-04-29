using System;
using System.Collections.Generic;
using Ch0nkEngine.Cameras.Components;
using SlimDX;
using SlimDX.Direct3D11;
using System.Windows.Forms;
using Ch0nkEngine.Composition;

namespace Ch0nkEngine.Cameras
{
    public abstract class Camera : Container
    {
        internal Matrix projectionMatrix;
        internal Matrix viewMatrix;

        internal Vector3 upVector = Vector3.UnitY;
        internal Vector3 position = Vector3.Zero;
        internal Vector3 target = Vector3.UnitX;


        protected Camera()
        {
        }


        #region Getters and Setters


        public Matrix ProjectionMatrix
        {
            get { return projectionMatrix; }
            set { projectionMatrix = value; }
        }

        public Matrix ViewMatrix
        {
            get { return viewMatrix; }
            set { viewMatrix = value; }
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Target
        {
            get { return target; }
            set { target = value; }
        }

        public Vector3 UpVector
        {
            get { return upVector; }
            set { upVector = value; }
        }

        public Vector3 Direction
        {
            get { return target - position;}
        }

        #endregion
    }
}