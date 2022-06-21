namespace ME.ECS.Mathematics {

    public static class fpmatrix3x3Ext {

        public static ref sfloat Get(this ref fpmatrix3x3 m, int row, int column) {

            var index = row + column * 3;
            switch (index) {
                case 0:
                    return ref m.m00;

                case 1:
                    return ref m.m10;

                case 2:
                    return ref m.m20;

                case 3:
                    return ref m.m01;

                case 4:
                    return ref m.m11;

                case 5:
                    return ref m.m21;

                case 6:
                    return ref m.m02;

                case 7:
                    return ref m.m12;

                case 8:
                    return ref m.m22;
            }

            throw new System.IndexOutOfRangeException("Invalid matrix index!");

        }

    }

    public struct fpmatrix3x3 {

        public sfloat m00;
        public sfloat m10;
        public sfloat m20;
        public sfloat m01;
        public sfloat m11;
        public sfloat m21;
        public sfloat m02;
        public sfloat m12;
        public sfloat m22;

        public sfloat this[int index] {
            get {
                switch (index) {
                    case 0:
                        return this.m00;

                    case 1:
                        return this.m10;

                    case 2:
                        return this.m20;

                    case 3:
                        return this.m01;

                    case 4:
                        return this.m11;

                    case 5:
                        return this.m21;

                    case 6:
                        return this.m02;

                    case 7:
                        return this.m12;

                    case 8:
                        return this.m22;

                    default:
                        throw new System.IndexOutOfRangeException("Invalid matrix index!");
                }
            }
            set {
                switch (index) {
                    case 0:
                        this.m00 = value;
                        break;

                    case 1:
                        this.m10 = value;
                        break;

                    case 2:
                        this.m20 = value;
                        break;

                    case 3:
                        this.m01 = value;
                        break;

                    case 4:
                        this.m11 = value;
                        break;

                    case 5:
                        this.m21 = value;
                        break;

                    case 6:
                        this.m02 = value;
                        break;

                    case 7:
                        this.m12 = value;
                        break;

                    case 8:
                        this.m22 = value;
                        break;

                    default:
                        throw new System.IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        public float3 MultiplyVector3(float3 v) {
            float3 res;
            res.x = this[0] * v.x + this[3] * v.y + this[6] * v.z;
            res.y = this[1] * v.x + this[4] * v.y + this[7] * v.z;
            res.z = this[2] * v.x + this[5] * v.y + this[8] * v.z;
            return res;
        }

        public void MultiplyVector3(float3 v, float3 output) {
            output.x = this[0] * v.x + this[3] * v.y + this[6] * v.z;
            output.y = this[1] * v.x + this[4] * v.y + this[7] * v.z;
            output.z = this[2] * v.x + this[5] * v.y + this[8] * v.z;
        }

        public float3 MultiplyVector3Transpose(float3 v) {
            float3 res;
            res.x = this.Get(0, 0) * v.x + this.Get(1, 0) * v.y + this.Get(2, 0) * v.z;
            res.y = this.Get(0, 1) * v.x + this.Get(1, 1) * v.y + this.Get(2, 1) * v.z;
            res.z = this.Get(0, 2) * v.x + this.Get(1, 2) * v.y + this.Get(2, 2) * v.z;
            return res;
        }

        private static fpmatrix3x3 CreateIdentityMatrix3x3f() {
            var temp = new fpmatrix3x3();
            temp.SetIdentity();
            return temp;
        }

        private static fpmatrix3x3 CreateZeroMatrix3x3f() {
            var temp = new fpmatrix3x3();
            temp.SetZero();
            return temp;
        }

        public static fpmatrix3x3 identity = fpmatrix3x3.CreateIdentityMatrix3x3f();
        public static fpmatrix3x3 zero = fpmatrix3x3.CreateZeroMatrix3x3f();

        public void SetIdentity() {
            this.Get(0, 0) = 1.0F;
            this.Get(0, 1) = 0.0F;
            this.Get(0, 2) = 0.0F;
            this.Get(1, 0) = 0.0F;
            this.Get(1, 1) = 1.0F;
            this.Get(1, 2) = 0.0F;
            this.Get(2, 0) = 0.0F;
            this.Get(2, 1) = 0.0F;
            this.Get(2, 2) = 1.0F;
        }

        public void SetZero() {
            this.Get(0, 0) = 0.0F;
            this.Get(0, 1) = 0.0F;
            this.Get(0, 2) = 0.0F;
            this.Get(1, 0) = 0.0F;
            this.Get(1, 1) = 0.0F;
            this.Get(1, 2) = 0.0F;
            this.Get(2, 0) = 0.0F;
            this.Get(2, 1) = 0.0F;
            this.Get(2, 2) = 0.0F;
        }

        public void SetOrthoNormalBasis(float3 inX, float3 inY, float3 inZ) {
            this.Get(0, 0) = inX[0];
            this.Get(0, 1) = inY[0];
            this.Get(0, 2) = inZ[0];
            this.Get(1, 0) = inX[1];
            this.Get(1, 1) = inY[1];
            this.Get(1, 2) = inZ[1];
            this.Get(2, 0) = inX[2];
            this.Get(2, 1) = inY[2];
            this.Get(2, 2) = inZ[2];
        }

        public void SetOrthoNormalBasisInverse(float3 inX, float3 inY, float3 inZ) {
            this.Get(0, 0) = inX[0];
            this.Get(1, 0) = inY[0];
            this.Get(2, 0) = inZ[0];
            this.Get(0, 1) = inX[1];
            this.Get(1, 1) = inY[1];
            this.Get(2, 1) = inZ[1];
            this.Get(0, 2) = inX[2];
            this.Get(1, 2) = inY[2];
            this.Get(2, 2) = inZ[2];
        }

        public void SetScale(float3 inScale) {
            this.Get(0, 0) = inScale[0];
            this.Get(0, 1) = 0.0F;
            this.Get(0, 2) = 0.0F;
            this.Get(1, 0) = 0.0F;
            this.Get(1, 1) = inScale[1];
            this.Get(1, 2) = 0.0F;
            this.Get(2, 0) = 0.0F;
            this.Get(2, 1) = 0.0F;
            this.Get(2, 2) = inScale[2];
        }

        public bool IsIdentity() {

            if (this.Get(0, 0) == 1f && this.Get(1, 0) == 0f && this.Get(2, 0) == 0f && this.Get(0, 1) == 0f && this.Get(1, 1) == 1f && this.Get(2, 1) == 0f &&
                this.Get(0, 2) == 0f && this.Get(1, 2) == 0f && this.Get(2, 2) == 1f) {
                return true;
            }

            return false;

        }

        public void Scale(float3 inScale) {
            this.Get(0, 0) *= inScale[0];
            this.Get(1, 0) *= inScale[0];
            this.Get(2, 0) *= inScale[0];

            this.Get(0, 1) *= inScale[1];
            this.Get(1, 1) *= inScale[1];
            this.Get(2, 1) *= inScale[1];

            this.Get(0, 2) *= inScale[2];
            this.Get(1, 2) *= inScale[2];
            this.Get(2, 2) *= inScale[2];

        }

        public sfloat GetDeterminant() {
            var fCofactor0 = this.Get(0, 0) * this.Get(1, 1) * this.Get(2, 2);
            var fCofactor1 = this.Get(0, 1) * this.Get(1, 2) * this.Get(2, 0);
            var fCofactor2 = this.Get(0, 2) * this.Get(1, 0) * this.Get(2, 1);

            var fCofactor3 = this.Get(0, 2) * this.Get(1, 1) * this.Get(2, 0);
            var fCofactor4 = this.Get(0, 1) * this.Get(1, 0) * this.Get(2, 2);
            var fCofactor5 = this.Get(0, 0) * this.Get(1, 2) * this.Get(2, 1);

            return fCofactor0 + fCofactor1 + fCofactor2 - fCofactor3 - fCofactor4 - fCofactor5;
        }

        private static void swap(ref sfloat lhs, ref sfloat rhs) {

            var temp = lhs;
            lhs = rhs;
            rhs = temp;

        }

        public void Transpose() {
            fpmatrix3x3.swap(ref this.Get(0, 1), ref this.Get(1, 0));
            fpmatrix3x3.swap(ref this.Get(0, 2), ref this.Get(2, 0));
            fpmatrix3x3.swap(ref this.Get(2, 1), ref this.Get(1, 2));
        }

        public bool Invert() {
            ////// THIS METHOD IS NUMERICALLY LESS ROBUST
            // Invert a 3x3 using cofactors.  This is faster than using a generic
            // Gaussian elimination because of the loop overhead of such a method.

            var kInverse = new fpmatrix3x3();
            kInverse.Get(0, 0) = this.Get(1, 1) * this.Get(2, 2) - this.Get(1, 2) * this.Get(2, 1);
            kInverse.Get(0, 1) = this.Get(0, 2) * this.Get(2, 1) - this.Get(0, 1) * this.Get(2, 2);
            kInverse.Get(0, 2) = this.Get(0, 1) * this.Get(1, 2) - this.Get(0, 2) * this.Get(1, 1);
            kInverse.Get(1, 0) = this.Get(1, 2) * this.Get(2, 0) - this.Get(1, 0) * this.Get(2, 2);
            kInverse.Get(1, 1) = this.Get(0, 0) * this.Get(2, 2) - this.Get(0, 2) * this.Get(2, 0);
            kInverse.Get(1, 2) = this.Get(0, 2) * this.Get(1, 0) - this.Get(0, 0) * this.Get(1, 2);
            kInverse.Get(2, 0) = this.Get(1, 0) * this.Get(2, 1) - this.Get(1, 1) * this.Get(2, 0);
            kInverse.Get(2, 1) = this.Get(0, 1) * this.Get(2, 0) - this.Get(0, 0) * this.Get(2, 1);
            kInverse.Get(2, 2) = this.Get(0, 0) * this.Get(1, 1) - this.Get(0, 1) * this.Get(1, 0);

            var fDet = this.Get(0, 0) * kInverse.Get(0, 0) + this.Get(0, 1) * kInverse.Get(1, 0) + this.Get(0, 2) * kInverse.Get(2, 0);

            if (math.abs(fDet) > VecMath.VECTOR3_EPSILON) {
                kInverse /= fDet;
                this = kInverse;
                return true;
            } else {
                this.SetZero();
                return false;
            }
        }

        public void InvertTranspose() {
            this.Invert();
            this.Transpose();
        }

        public static fpmatrix3x3 operator /(fpmatrix3x3 m, sfloat f) {
            m *= 1.0F / f;
            return m;
        }

        public static fpmatrix3x3 operator *(fpmatrix3x3 m, sfloat f) {
            m.m00 *= f;
            m.m10 *= f;
            m.m20 *= f;
            m.m01 *= f;
            m.m11 *= f;
            m.m21 *= f;
            m.m02 *= f;
            m.m12 *= f;
            m.m22 *= f;
            return m;
        }

        public static fpmatrix3x3 operator *(fpmatrix3x3 m, fpmatrix3x3 inM) {
            int i;
            for (i = 0; i < 3; i++) {
                float3 v = new float3(m.Get(i, 0), m.Get(i, 1), m.Get(i, 2));
                m.Get(i, 0) = v[0] * inM.Get(0, 0) + v[1] * inM.Get(1, 0) + v[2] * inM.Get(2, 0);
                m.Get(i, 1) = v[0] * inM.Get(0, 1) + v[1] * inM.Get(1, 1) + v[2] * inM.Get(2, 1);
                m.Get(i, 2) = v[0] * inM.Get(0, 2) + v[1] * inM.Get(1, 2) + v[2] * inM.Get(2, 2);
            }

            return m;
        }

        public void SetAxisAngle(float3 rotationAxis, sfloat radians) {
            this.GetRotMatrixNormVec(ref this, rotationAxis, radians);
        }

        public void SetFromToRotation(float3 from, float3 to) {
            var mtx = new fpmatrix3x3();
            this.fromToRotation(ref from, ref to, ref mtx);
            this.Get(0, 0) = mtx.Get(0, 0);
            this.Get(0, 1) = mtx.Get(0, 1);
            this.Get(0, 2) = mtx.Get(0, 2);
            this.Get(1, 0) = mtx.Get(1, 0);
            this.Get(1, 1) = mtx.Get(1, 1);
            this.Get(1, 2) = mtx.Get(1, 2);
            this.Get(2, 0) = mtx.Get(2, 0);
            this.Get(2, 1) = mtx.Get(2, 1);
            this.Get(2, 2) = mtx.Get(2, 2);
        }

        public void MakePositive(float3 euler) {
            sfloat negativeFlip = -0.0001F;
            sfloat positiveFlip = math.PI * 2.0F - 0.0001F;

            if (euler.x < negativeFlip) {
                euler.x += 2.0f * math.PI;
            } else if (euler.x > positiveFlip) {
                euler.x -= 2.0f * math.PI;
            }

            if (euler.y < negativeFlip) {
                euler.y += 2.0f * math.PI;
            } else if (euler.y > positiveFlip) {
                euler.y -= 2.0f * math.PI;
            }

            if (euler.z < negativeFlip) {
                euler.z += 2.0f * math.PI;
            } else if (euler.z > positiveFlip) {
                euler.z -= 2.0f * math.PI;
            }
        }

        public void SanitizeEuler(float3 euler) {
            this.MakePositive(euler);
        }

        public void EulerToMatrix(float3 v, fpmatrix3x3 matrix) {
            sfloat cx = math.cos(v.x);
            sfloat sx = math.sin(v.x);
            sfloat cy = math.cos(v.y);
            sfloat sy = math.sin(v.y);
            sfloat cz = math.cos(v.z);
            sfloat sz = math.sin(v.z);

            matrix.Get(0, 0) = cy * cz + sx * sy * sz;
            matrix.Get(0, 1) = cz * sx * sy - cy * sz;
            matrix.Get(0, 2) = cx * sy;

            matrix.Get(1, 0) = cx * sz;
            matrix.Get(1, 1) = cx * cz;
            matrix.Get(1, 2) = -sx;

            matrix.Get(2, 0) = -cz * sy + cy * sx * sz;
            matrix.Get(2, 1) = cy * cz * sx + sy * sz;
            matrix.Get(2, 2) = cx * cy;
        }

        /// This is YXZ euler conversion
        public bool MatrixToEuler(fpmatrix3x3 matrix, float3 v) {
            // from http://www.geometrictools.com/Documentation/EulerAngles.pdf
            // YXZ order
            if (matrix.Get(1, 2) < 0.999F) // some fudge for imprecision
            {
                if (matrix.Get(1, 2) > -0.999F) // some fudge for imprecision
                {
                    v.x = math.asin(-matrix.Get(1, 2));
                    v.y = math.atan2(matrix.Get(0, 2), matrix.Get(2, 2));
                    v.z = math.atan2(matrix.Get(1, 0), matrix.Get(1, 1));
                    this.SanitizeEuler(v);
                    return true;
                } else {
                    // WARNING.  Not unique.  YA - ZA = atan2(r01,r00)
                    v.x = math.PI * 0.5F;
                    v.y = math.atan2(matrix.Get(0, 1), matrix.Get(0, 0));
                    v.z = 0.0F;
                    this.SanitizeEuler(v);

                    return false;
                }
            } else {
                // WARNING.  Not unique.  YA + ZA = atan2(-r01,r00)
                v.x = -math.PI * 0.5F;
                v.y = math.atan2(-matrix.Get(0, 1), matrix.Get(0, 0));
                v.z = 0.0F;
                this.SanitizeEuler(v);
                return false;
            }
        }

        /*
         * A function for creating a rotation matrix that rotates a vector called
         * "from" into another vector called "to".
         * Input : from[3], to[3] which both must be *normalized* non-zero vectors
         * Output: mtx[3][3] -- a 3x3 matrix in colum-major form
         * Author: Tomas Mˆller, 1999
         */
        private void fromToRotation(ref float3 from, ref float3 to, ref fpmatrix3x3 mtx) {
            float3 v;
            sfloat e, h;
            v = math.cross(from, to);
            e = math.dot(from, to);
            if (e > 1.0f) /* "from" almost or equal to "to"-vector? */ {
                /* return identity */
                mtx.Get(0, 0) = 1.0f;
                mtx.Get(0, 1) = 0.0f;
                mtx.Get(0, 2) = 0.0f;
                mtx.Get(1, 0) = 0.0f;
                mtx.Get(1, 1) = 1.0f;
                mtx.Get(1, 2) = 0.0f;
                mtx.Get(2, 0) = 0.0f;
                mtx.Get(2, 1) = 0.0f;
                mtx.Get(2, 2) = 1.0f;
            } else if (e < -1.0f) /* "from" almost or equal to negated "to"? */ {
                float3 up, left = float3.zero;
                sfloat invlen;
                sfloat fxx, fyy, fzz, fxy, fxz, fyz;
                sfloat uxx, uyy, uzz, uxy, uxz, uyz;
                sfloat lxx, lyy, lzz, lxy, lxz, lyz;
                /* left=CROSS(from, (1,0,0)) */
                left[0] = 0.0f;
                left[1] = from[2];
                left[2] = -from[1];
                if (math.dot(left, left) < 0.00001f) /* was left=CROSS(from,(1,0,0)) a good choice? */ {
                    /* here we now that left = CROSS(from, (1,0,0)) will be a good choice */
                    left[0] = -from[2];
                    left[1] = 0.0f;
                    left[2] = from[0];
                }

                /* normalize "left" */
                invlen = 1.0f / math.sqrt(math.dot(left, left));
                left[0] *= invlen;
                left[1] *= invlen;
                left[2] *= invlen;
                up = math.cross(left, from);
                /* now we have a coordinate system, i.e., a basis;    */
                /* M=(from, up, left), and we want to rotate to:      */
                /* N=(-from, up, -left). This is done with the matrix:*/
                /* N*M^T where M^T is the transpose of M              */
                fxx = -from[0] * from[0];
                fyy = -from[1] * from[1];
                fzz = -from[2] * from[2];
                fxy = -from[0] * from[1];
                fxz = -from[0] * from[2];
                fyz = -from[1] * from[2];

                uxx = up[0] * up[0];
                uyy = up[1] * up[1];
                uzz = up[2] * up[2];
                uxy = up[0] * up[1];
                uxz = up[0] * up[2];
                uyz = up[1] * up[2];

                lxx = -left[0] * left[0];
                lyy = -left[1] * left[1];
                lzz = -left[2] * left[2];
                lxy = -left[0] * left[1];
                lxz = -left[0] * left[2];
                lyz = -left[1] * left[2];
                /* symmetric matrix */
                mtx.Get(0, 0) = fxx + uxx + lxx;
                mtx.Get(0, 1) = fxy + uxy + lxy;
                mtx.Get(0, 2) = fxz + uxz + lxz;
                mtx.Get(1, 0) = mtx.Get(0, 1);
                mtx.Get(1, 1) = fyy + uyy + lyy;
                mtx.Get(1, 2) = fyz + uyz + lyz;
                mtx.Get(2, 0) = mtx.Get(0, 2);
                mtx.Get(2, 1) = mtx.Get(1, 2);
                mtx.Get(2, 2) = fzz + uzz + lzz;
            } else /* the most common case, unless "from"="to", or "from"=-"to" */ {
                /* ...otherwise use this hand optimized version (9 mults less) */
                sfloat hvx, hvz, hvxy, hvxz, hvyz;
                h = (1.0f - e) / math.dot(v, v);
                hvx = h * v[0];
                hvz = h * v[2];
                hvxy = hvx * v[1];
                hvxz = hvx * v[2];
                hvyz = hvz * v[1];
                mtx.Get(0, 0) = e + hvx * v[0];
                mtx.Get(0, 1) = hvxy - v[2];
                mtx.Get(0, 2) = hvxz + v[1];
                mtx.Get(1, 0) = hvxy + v[2];
                mtx.Get(1, 1) = e + h * v[1] * v[1];
                mtx.Get(1, 2) = hvyz - v[0];
                mtx.Get(2, 0) = hvxz - v[1];
                mtx.Get(2, 1) = hvyz + v[0];
                mtx.Get(2, 2) = e + hvz * v[2];
            }
        }

        // Right handed
        private bool LookRotationToMatrix(float3 viewVec, float3 upVec, fpmatrix3x3 m) {
            var z = viewVec;
            // compute u0
            sfloat mag = math.length(z);
            if (mag < 0.00001f) {
                m.SetIdentity();
                return false;
            }

            z /= mag;

            var x = math.cross(upVec, z);
            mag = math.length(x);
            if (mag < 0.00001f) {
                m.SetIdentity();
                return false;
            }

            x /= mag;

            var y = math.cross(z, x);
            if (math.lengthsq(y) == 1.0F) {
                return false;
            }

            m.SetOrthoNormalBasis(x, y, z);
            return true;
        }

        private void GetRotMatrixNormVec(ref fpmatrix3x3 data, float3 inVec, sfloat radians) {
            /* This function contributed by Erich Boleyn (erich@uruk.org) */
            /* This function used from the Mesa OpenGL code (matrix.c)  */
            sfloat s, c;
            sfloat vx, vy, vz, xx, yy, zz, xy, yz, zx, xs, ys, zs, one_c;

            s = math.sin(radians);
            c = math.cos(radians);

            vx = inVec[0];
            vy = inVec[1];
            vz = inVec[2];

            //#define M(row,col)  out[row*3 + col]
            /*
            *     Arbitrary axis rotation matrix.
            *
            *  This is composed of 5 matrices, Rz, Ry, T, Ry', Rz', multiplied
            *  like so:  Rz * Ry * T * Ry' * Rz'.  T is the final rotation
            *  (which is about the X-axis), and the two composite transforms
            *  Ry' * Rz' and Rz * Ry are (respectively) the rotations necessary
            *  from the arbitrary axis to the X-axis then back.  They are
            *  all elementary rotations.
            *
            *  Rz' is a rotation about the Z-axis, to bring the axis vector
            *  into the x-z plane.  Then Ry' is applied, rotating about the
            *  Y-axis to bring the axis vector parallel with the X-axis.  The
            *  rotation about the X-axis is then performed.  Ry and Rz are
            *  simply the respective inverse transforms to bring the arbitrary
            *  axis back to its original orientation.  The first transforms
            *  Rz' and Ry' are considered inverses, since the data from the
            *  arbitrary axis gives you info on how to get to it, not how
            *  to get away from it, and an inverse must be applied.
            *
            *  The basic calculation used is to recognize that the arbitrary
            *  axis vector (x, y, z), since it is of unit length, actually
            *  represents the sines and cosines of the angles to rotate the
            *  X-axis to the same orientation, with theta being the angle about
            *  Z and phi the angle about Y (in the order described above)
            *  as follows:
            *
            *  cos ( theta ) = x / sqrt ( 1 - z^2 )
            *  sin ( theta ) = y / sqrt ( 1 - z^2 )
            *
            *  cos ( phi ) = sqrt ( 1 - z^2 )
            *  sin ( phi ) = z
            *
            *  Note that cos ( phi ) can further be inserted to the above
            *  formulas:
            *
            *  cos ( theta ) = x / cos ( phi )
            *  sin ( theta ) = y / cos ( phi )
            *
            *  ...etc.  Because of those relations and the standard trigonometric
            *  relations, it is pssible to reduce the transforms down to what
            *  is used below.  It may be that any primary axis chosen will give the
            *  same results (modulo a sign convention) using thie method.
            *
            *  Particularly nice is to notice that all divisions that might
            *  have caused trouble when parallel to certain planes or
            *  axis go away with care paid to reducing the expressions.
            *  After checking, it does perform correctly under all cases, since
            *  in all the cases of division where the denominator would have
            *  been zero, the numerator would have been zero as well, giving
            *  the expected result.
            */

            xx = vx * vx;
            yy = vy * vy;
            zz = vz * vz;
            xy = vx * vy;
            yz = vy * vz;
            zx = vz * vx;
            xs = vx * s;
            ys = vy * s;
            zs = vz * s;
            one_c = 1.0F - c;

            data[0 * 3 + 0] = one_c * xx + c;
            data[1 * 3 + 0] = one_c * xy - zs;
            data[2 * 3 + 0] = one_c * zx + ys;

            data[0 * 3 + 1] = one_c * xy + zs;
            data[1 * 3 + 1] = one_c * yy + c;
            data[2 * 3 + 1] = one_c * yz - xs;

            data[0 * 3 + 2] = one_c * zx - ys;
            data[1 * 3 + 2] = one_c * yz + xs;
            data[2 * 3 + 2] = one_c * zz + c;

        }

    }

}