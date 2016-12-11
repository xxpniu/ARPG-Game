using System;

namespace UMath
{
    /// <summary>
    /// transform.
    /// </summary>
    [Serializable]
    public class UTransform
    {
        public UTransform()
        {
            
        }

        #region fields
        private UVector3 _localPos = UVector3.zero;
        private UQuaternion _localRot = UQuaternion.LookRotation(UVector3.forward);
        private UVector3 _localScale =  UVector3.one;
        private UTransform _parent;
        private bool isdrity = true; 

        private UMatrix4x4 _matrix;
        private UMatrix4x4 _invmatrix;
        #endregion

        #region private
        private void Refresh()
        {
            if (!isdrity)
                return;
            isdrity = false;
            _matrix = UMatrix4x4.TRS(_localPos, _localRot, _localScale);
            _invmatrix = _matrix.Inverted();
        }

        private UMatrix4x4 Matrix
        {
            get
            {
                Refresh();
                return _matrix;
            }
        }

        private UMatrix4x4 InvMatrix
        {
            get{
                Refresh();
                return _invmatrix;
            }
        }
        #endregion

        #region public 
        public void setParent(UTransform parent,bool stayWorld= false)
        {
            var pos = this.position;
            var rot = this.rotation;
            //var scale = this.scale;

            this._parent = parent;
            if (stayWorld)
            {
                this.position = pos;
                this.rotation = rot;
                //this.scale = scale;
            }

        }

        /// <summary>
        /// Gets the local to world matrix.
        /// </summary>
        /// <value>The local to world matrix.</value>
        public UMatrix4x4 localToWorldMatrix
        {
            get
            {
                if (_parent != null)
                    return _parent.Matrix * _parent.localToWorldMatrix;
                return UMatrix4x4.identity;
            }
        }

        /// <summary>
        /// Gets the world to local matrix.
        /// </summary>
        /// <value>The world to local matrix.</value>
        public UMatrix4x4 worldToLocalMatrix
        {
            get{
                if (_parent != null)
                {
                    return _parent.worldToLocalMatrix * _parent.InvMatrix;
                }
                else
                {
                    return UMatrix4x4.identity;
                }
            }
        }

        /// <summary>
        /// Gets or sets the local position.
        /// </summary>
        /// <value>The local position.</value>
        public UVector3 localPosition
        {
            set
            {
                isdrity = true;
                _localPos = value;
            }

            get
            { 
                return _localPos;
            }
        }

        /// <summary>
        /// Gets or sets the local scale.
        /// </summary>
        /// <value>The local scale.</value>
        public UVector3 localScale{
            set
            {
                _localScale = value;
                isdrity = true;
            }
            get{ return _localScale; }
        }

        /// <summary>
        /// Gets or sets the local rotation.
        /// </summary>
        /// <value>The local rotation.</value>
        public UQuaternion localRotation
        {
            set{ 
                _localRot = value;
                isdrity = true;
            }

            get{ return _localRot;}
        }

        /// <summary>
        /// Gets or sets the world position.
        /// </summary>
        /// <value>The position.</value>
        public UVector3 position
        {
            set
            {
                var p = worldToLocalMatrix * new UVector4(value, 1);
                localPosition = p.Xyz;
            }
            get
            {
                var v = localToWorldMatrix * new UVector4(localPosition, 1);
                return v.Xyz;
            }
        }

        /// <summary>
        /// Gets or sets the world rotation.
        /// </summary>
        /// <value>The rotation.</value>
        public UQuaternion rotation
        {
            set
            {
                var r = worldToLocalMatrix.Rotation * value;
                localRotation = r;
            }

            get
            {
                return (localToWorldMatrix * Matrix).Rotation;
            }
        }

        /// <summary>
        /// Gets world scale.
        /// </summary>
        /// <value>The scale.</value>
        public UVector3 scale
        {
            get{ return  (localToWorldMatrix * Matrix).Scale ; }
        }

        /// <summary>
        /// Gets or sets the forward.
        /// </summary>
        /// <value>The forward.</value>
        public UVector3 forward
        {
            set{ var rot = UQuaternion.LookRotation(value); this.rotation = rot; }
            get{ return rotation*UVector3.forward ; }
        }

        /// <summary>
        /// Transforms the direction.
        /// </summary>
        /// <returns>The direction.</returns>
        /// <param name="dir">Dir.</param>
        public UVector3 TransformDirection(UVector3 dir)
        {
            return (localToWorldMatrix * Matrix * new UVector4(dir, 0)).Xyz;
        }
        /// <summary>
        /// Transforms the point.
        /// </summary>
        /// <returns>The point.</returns>
        /// <param name="point">Point.</param>

        public UVector3 TransformPoint(UVector3 point)
        {
            return (localToWorldMatrix * Matrix * new UVector4(point, 1)).Xyz;
        }

        /// <summary>
        /// Looks at target.
        /// </summary>
        /// <param name="target">Target.</param>
        public void LookAt(UTransform target)
        {
            var dir = target.position - this.position;
            if (dir.sqrMagnitude < MathHelper.Epsilon) return;
            rotation = UQuaternion.LookRotation(dir.normalized);
        }
        #endregion
    }
}

