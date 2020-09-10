// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;

namespace Mathematics
{
    /// <summary>Represents a four-by-four dimension matrix.</summary>
    public struct Matrix4x4 : IComparable, IComparable<Matrix4x4>, IEquatable<Matrix4x4>
    {
        /// <summary>Represents a <see cref="Matrix4x4"/> whose main-diagonal components are set to one and whose remaining components are all set to zero.</summary>
        public static readonly Matrix4x4 Identity = new Matrix4x4(Vector4.UnitX, Vector4.UnitY, Vector4.UnitZ, Vector4.UnitW);

        private Vector4 _x;
        private Vector4 _y;
        private Vector4 _z;
        private Vector4 _w;

        /// <summary>Initializes a new instance of the <see cref="Matrix4x4"/> struct.</summary>
        /// <param name="x">The initial value for the x-component of the matrix.</param>
        /// <param name="y">The initial value for the y-component of the matrix.</param>
        /// <param name="z">The initial value for the z-component of the matrix.</param>
        /// <param name="w">The initial value for the w-component of the matrix.</param>
        public Matrix4x4(Vector4 x, Vector4 y, Vector4 z, Vector4 w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        /// <summary>Gets or sets the value of the component at the specified index for the current instance.</summary>
        /// <param name="index">The index of the component to get or set.</param>
        public unsafe Vector4 this[int index]
        {
            get
            {
                if ((uint)(index) > 3)
                {
                    throw new IndexOutOfRangeException();
                }
                return Unsafe.Add(ref _x, index);
            }

            set
            {
                if ((uint)(index) > 3)
                {
                    throw new IndexOutOfRangeException();
                }
                Unsafe.Add(ref _x, index) = value;
            }
        }

        /// <summary>Gets the determinant of the current instance.</summary>
        public float Determinant
        {
            get
            {
                return (X.X * Y.Y * Z.Z * W.W) +
                       (X.Y * Y.Z * Z.W * W.X) +
                       (X.Z * Y.W * Z.X * W.Y) +
                       (X.W * Y.X * Z.Y * W.Z) -
                       (X.W * Y.Z * Z.Y * W.X) -
                       (X.Z * Y.Y * Z.X * W.W) -
                       (X.Y * Y.X * Z.W * W.Z) -
                       (X.X * Y.W * Z.Z * W.Y);
            }
        }

        /// <summary>Gets or sets the value of the x-component for the current instance.</summary>
        public Vector4 X
        {
            get
            {
                return _x;
            }

            set
            {
                _x = value;
            }
        }

        /// <summary>Gets or sets the value of the y-component for the current instance.</summary>
        public Vector4 Y
        {
            get
            {
                return _y;
            }

            set
            {
                _y = value;
            }
        }

        /// <summary>Gets or sets the value of the z-component for the current instance.</summary>
        public Vector4 Z
        {
            get
            {
                return _z;
            }

            set
            {
                _z = value;
            }
        }

        /// <summary>Gets or sets the value of the w-component for the current instance.</summary>
        public Vector4 W
        {
            get
            {
                return _w;
            }

            set
            {
                _w = value;
            }
        }

        /// <summary>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine equality.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Matrix4x4 left, Matrix4x4 right)
        {
            return (left._x == right._x) &&
                   (left._y == right._y) &&
                   (left._z == right._z) &&
                   (left._w == right._w);
        }

        /// <summary>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine equality.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix4x4 left, Matrix4x4 right) => !(left == right);

        /// <summary>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <(Matrix4x4 left, Matrix4x4 right) => left.Determinant < right.Determinant;

        /// <summary>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Matrix4x4 left, Matrix4x4 right) => left.Determinant <= right.Determinant;

        /// <summary>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >(Matrix4x4 left, Matrix4x4 right) => left.Determinant > right.Determinant;

        /// <summary>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Matrix4x4 left, Matrix4x4 right) => left.Determinant >= right.Determinant;

        /// <summary>Negates a <see cref="Matrix4x4"/> instance to determine its additive inverse.</summary>
        /// <param name="right">The matrix to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Matrix4x4 operator -(Matrix4x4 right) => right * (-1.0f);

        /// <summary>Adds two <see cref="Matrix4x4"/> instances to determine their sum.</summary>
        /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Matrix4x4 operator +(Matrix4x4 left, Matrix4x4 right)
        {
            return new Matrix4x4(left._x + right._x,
                                 left._y + right._y,
                                 left._z + right._z,
                                 left._w + right._w);
        }

        /// <summary>Subtracts two <see cref="Matrix4x4"/> instances to determine their difference.</summary>
        /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Matrix4x4 operator -(Matrix4x4 left, Matrix4x4 right) => left + (-right);

        /// <summary>Multiplies two <see cref="Matrix4x4"/> instances to determine their product.</summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
        {
            var x = new Vector4((left.X.X * right.X.X) + (left.X.Y * right.Y.X) + (left.X.Z * right.Z.X) + (left.X.W * right.W.X),
                                (left.X.X * right.X.Y) + (left.X.Y * right.Y.Y) + (left.X.Z * right.Z.Y) + (left.X.W * right.W.Y),
                                (left.X.X * right.X.Z) + (left.X.Y * right.Y.Z) + (left.X.Z * right.Z.Z) + (left.X.W * right.W.Z),
                                (left.X.X * right.X.W) + (left.X.Y * right.Y.W) + (left.X.Z * right.Z.W) + (left.X.W * right.W.W));

            var y = new Vector4((left.Y.X * right.X.X) + (left.Y.Y * right.Y.X) + (left.Y.Z * right.Z.X) + (left.Y.W * right.W.X),
                                (left.Y.X * right.X.Y) + (left.Y.Y * right.Y.Y) + (left.Y.Z * right.Z.Y) + (left.Y.W * right.W.Y),
                                (left.Y.X * right.X.Z) + (left.Y.Y * right.Y.Z) + (left.Y.Z * right.Z.Z) + (left.Y.W * right.W.Z),
                                (left.Y.X * right.X.W) + (left.Y.Y * right.Y.W) + (left.Y.Z * right.Z.W) + (left.Y.W * right.W.W));

            var z = new Vector4((left.Z.X * right.X.X) + (left.Z.Y * right.Y.X) + (left.Z.Z * right.Z.X) + (left.Z.W * right.W.X),
                                (left.Z.X * right.X.Y) + (left.Z.Y * right.Y.Y) + (left.Z.Z * right.Z.Y) + (left.Z.W * right.W.Y),
                                (left.Z.X * right.X.Z) + (left.Z.Y * right.Y.Z) + (left.Z.Z * right.Z.Z) + (left.Z.W * right.W.Z),
                                (left.Z.X * right.X.W) + (left.Z.Y * right.Y.W) + (left.Z.Z * right.Z.W) + (left.Z.W * right.W.W));

            var w = new Vector4((left.W.X * right.X.X) + (left.W.Y * right.Y.X) + (left.W.Z * right.Z.X) + (left.W.W * right.W.X),
                                (left.W.X * right.X.Y) + (left.W.Y * right.Y.Y) + (left.W.Z * right.Z.Y) + (left.W.W * right.W.Y),
                                (left.W.X * right.X.Z) + (left.W.Y * right.Y.Z) + (left.W.Z * right.Z.Z) + (left.W.W * right.W.Z),
                                (left.W.X * right.X.W) + (left.W.Y * right.Y.W) + (left.W.Z * right.Z.W) + (left.W.W * right.W.W));

            return new Matrix4x4(x, y, z, w);
        }

        /// <summary>Multiples a <see cref="Matrix4x4"/> and <see cref="float"/> instance to determine their product.</summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix4x4 operator *(Matrix4x4 left, float right)
        {
            return new Matrix4x4(left._x * right,
                                 left._y * right,
                                 left._z * right,
                                 left._w * right);
        }

        /// <summary>Divides a <see cref="Matrix4x4"/> and <see cref="float"/> instance to determine their quotient.</summary>
        /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Matrix4x4 operator /(Matrix4x4 left, float right) => left * (1.0f / right);

        /// <summary>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine equality.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool Equals(Matrix4x4 left, Matrix4x4 right) => left == right;

        /// <summary>Compares two <see cref="Matrix4x4"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns>
        ///     <para>A value that indicates the relative order of the objects being compared.</para>
        ///     <list type="table">
        ///         <item>
        ///             <term>Less Than Zero</term>
        ///             <description><paramref name="left"/> precedes <paramref name="right"/> in the sort order.</description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description><paramref name="left"/> is in the same sort position as <paramref name="right"/>.</description>
        ///         </item>
        ///         <item>
        ///             <term>Greater Than Zero</term>
        ///             <description><paramref name="left"/> follows <paramref name="right"/> in the sort order.</description>
        ///         </item>
        ///     </list>
        /// </returns>
        public static int Compare(Matrix4x4 left, Matrix4x4 right) => left.Determinant.CompareTo(right.Determinant);

        /// <summary>Negates a <see cref="Matrix4x4"/> instance to determine its additive inverse.</summary>
        /// <param name="right">The matrix to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Matrix4x4 Negate(Matrix4x4 right) => -right;

        /// <summary>Adds two <see cref="Matrix4x4"/> instances to determine their sum.</summary>
        /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Matrix4x4 Add(Matrix4x4 left, Matrix4x4 right) => left + right;

        /// <summary>Subtracts two <see cref="Matrix4x4"/> instances to determine their difference.</summary>
        /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Matrix4x4 Subtract(Matrix4x4 left, Matrix4x4 right) => left - right;

        /// <summary>Multiplies two <see cref="Matrix4x4"/> instances to determine their product.</summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix4x4 Multiply(Matrix4x4 left, Matrix4x4 right) => left * right;

        /// <summary>Multiples a <see cref="Matrix4x4"/> and <see cref="float"/> instance to determine their product.</summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix4x4 Multiply(Matrix4x4 left, float right) => left * right;

        /// <summary>Divides a <see cref="Matrix4x4"/> and <see cref="float"/> instance to determine their quotient.</summary>
        /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Matrix4x4 Divide(Matrix4x4 left, float right) => left / right;

        /// <summary>Determines the transpose of a <see cref="Matrix4x4"/> instance.</summary>
        /// <param name="right">The matrix to transpose.</param>
        /// <returns>The transpose of <paramref name="right"/>.</returns>
        public static Matrix4x4 Transpose(Matrix4x4 right)
        {
            var x = new Vector4(right._x.X, right._y.X, right._z.X, right._w.X);
            var y = new Vector4(right._x.Y, right._y.Y, right._z.Y, right._w.Y);
            var z = new Vector4(right._x.Z, right._y.Z, right._z.Z, right._w.Z);
            var w = new Vector4(right._x.W, right._y.W, right._z.W, right._w.W);

            return new Matrix4x4(x, y, z, w);
        }

        public static Matrix4x4 CreateFrom(Matrix3x3 value, Vector3 w)
        {
            return new Matrix4x4(new Vector4(value.X, 0.0f),
                                 new Vector4(value.Y, 0.0f),
                                 new Vector4(value.Z, 0.0f),
                                 new Vector4(w, 1.0f));
        }

        public static Matrix4x4 CreateFrom(OrthogonalTransform transform) => CreateFrom(Matrix3x3.CreateFrom(transform.Rotation), transform.Translation);

        public static Matrix4x4 CreateFrom(Quaternion rotation)
        {
            var xx = rotation.X * rotation.X;
            var yy = rotation.Y * rotation.Y;
            var zz = rotation.Z * rotation.Z;

            var xy = rotation.X * rotation.Y;
            var wz = rotation.Z * rotation.W;
            var xz = rotation.Z * rotation.X;
            var wy = rotation.Y * rotation.W;
            var yz = rotation.Y * rotation.Z;
            var wx = rotation.X * rotation.W;

            return new Matrix4x4(new Vector4(1.0f - (2.0f * (yy + zz)), 2.0f * (xy + wz), 2.0f * (xz - wy), 0.0f),
                                 new Vector4(2.0f * (xy - wz), 1.0f - (2.0f * (zz + xx)), 2.0f * (yz + wx), 0.0f),
                                 new Vector4(2.0f * (xz + wy), 2.0f * (yz - wx), 1.0f - (2.0f * (yy + xx)), 0.0f),
                                 Vector4.UnitW);
        }

        /// <summary>Compares a <see cref="object"/> to the current instance to determine sort order.</summary>
        /// <param name="obj">The object to compare to the current instance.</param>
        /// <returns>
        ///     <para>A value that indicates the relative order of the objects being compared.</para>
        ///     <list type="table">
        ///         <item>
        ///             <term>Less Than Zero</term>
        ///             <description>The current instance precedes <paramref name="obj"/> in the sort order.</description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>The current instance is in the same sort position as <paramref name="obj"/>.</description>
        ///         </item>
        ///         <item>
        ///             <term>Greater Than Zero</term>
        ///             <description>The current instance follows <paramref name="obj"/> in the sort order.</description>
        ///         </item>
        ///     </list>
        /// </returns>
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not an instance of <see cref="Matrix4x4"/>.</exception>
        public int CompareTo(object? obj)
        {
            if (obj is Matrix4x4 other)
            {
                return CompareTo(other);
            }
            return (obj is null) ? 1 : throw new ArgumentException($"{nameof(obj)} is not an instance of {nameof(Matrix4x4)}", nameof(obj));
        }

        /// <summary>Compares a <see cref="Matrix4x4"/> to the current instance to determine sort order.</summary>
        /// <param name="other">The matrix to compare to the current instance.</param>
        /// <returns>
        ///     <para>A value that indicates the relative order of the objects being compared.</para>
        ///     <list type="table">
        ///         <item>
        ///             <term>Less Than Zero</term>
        ///             <description>The current instance precedes <paramref name="other"/> in the sort order.</description>
        ///         </item>
        ///         <item>
        ///             <term>Zero</term>
        ///             <description>The current instance is in the same sort position as <paramref name="other"/>.</description>
        ///         </item>
        ///         <item>
        ///             <term>Greater Than Zero</term>
        ///             <description>The current instance follows <paramref name="other"/> in the sort order.</description>
        ///         </item>
        ///     </list>
        /// </returns>
        public int CompareTo(Matrix4x4 other) => Compare(this, other);

        /// <summary>Compares a <see cref="object"/> to the current instance to determine equality.</summary>
        /// <param name="obj">The object to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Matrix4x4"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj) => (obj is Matrix4x4 other) && Equals(other);

        /// <summary>Compares a <see cref="Matrix4x4"/> to the current instance to determine equality.</summary>
        /// <param name="other">The matrix to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix4x4 other) => Equals(this, other);

        /// <summary>Generates a hash code for the value of the current instance.</summary>
        /// <returns>A hash code for the value of the current instance.</returns>
        public override int GetHashCode() => Determinant.GetHashCode();

        public Matrix4x4 Invert()
        {
            var a = _x.X;
            var b = _x.Y;
            var c = _x.Z;
            var d = _x.W;

            var e = _y.X;
            var f = _y.Y;
            var g = _y.Z;
            var h = _y.W;

            var i = _z.X;
            var j = _z.Y;
            var k = _z.Z;
            var l = _z.W;

            var m = _w.X;
            var n = _w.Y;
            var o = _w.Z;
            var p = _w.W;


            var kp_lo = (k * p) - (l * o);
            var jp_ln = (j * p) - (l * n);
            var jo_kn = (j * o) - (k * n);
            var ip_lm = (i * p) - (l * m);
            var io_km = (i * o) - (k * m);
            var in_jm = (i * n) - (j * m);


            var a11 = +((f * kp_lo) - (g * jp_ln) + (h * jo_kn));
            var a12 = -((e * kp_lo) - (g * ip_lm) + (h * io_km));
            var a13 = +((e * jp_ln) - (f * ip_lm) + (h * in_jm));
            var a14 = -((e * jo_kn) - (f * io_km) + (g * in_jm));

            var det = (a * a11) + (b * a12) + (c * a13) + (d * a14);

            if (MathF.Abs(det) < float.Epsilon)
            {
                return new Matrix4x4(new Vector4(float.NaN),
                                     new Vector4(float.NaN),
                                     new Vector4(float.NaN),
                                     new Vector4(float.NaN));
            }
            else
            {
                var invDet = 1.0f / det;


                var (xx, yx, zx, wx) = (a11 * invDet,
                                        a12 * invDet,
                                        a13 * invDet,
                                        a14 * invDet);


                var (xy, yy, zy, wy) = (-((b * kp_lo) - (c * jp_ln) + (d * jo_kn)) * invDet,
                                        +((a * kp_lo) - (c * ip_lm) + (d * io_km)) * invDet,
                                        -((a * jp_ln) - (b * ip_lm) + (d * in_jm)) * invDet,
                                        +((a * jo_kn) - (b * io_km) + (c * in_jm)) * invDet);


                var gp_ho = (g * p) - (h * o);
                var fp_hn = (f * p) - (h * n);
                var fo_gn = (f * o) - (g * n);
                var ep_hm = (e * p) - (h * m);
                var eo_gm = (e * o) - (g * m);
                var en_fm = (e * n) - (f * m);


                var (xz, yz, zz, wz) = (+((b * gp_ho) - (c * fp_hn) + (d * fo_gn)) * invDet,
                                        -((a * gp_ho) - (c * ep_hm) + (d * eo_gm)) * invDet,
                                        +((a * fp_hn) - (b * ep_hm) + (d * en_fm)) * invDet,
                                        -((a * fo_gn) - (b * eo_gm) + (c * en_fm)) * invDet);



                var gl_hk = (g * l) - (h * k);
                var fl_hj = (f * l) - (h * j);
                var fk_gj = (f * k) - (g * j);
                var el_hi = (e * l) - (h * i);
                var ek_gi = (e * k) - (g * i);
                var ej_fi = (e * j) - (f * i);


                var (xw, yw, zw, ww) = (-((b * gl_hk) - (c * fl_hj) + (d * fk_gj)) * invDet,
                                        +((a * gl_hk) - (c * el_hi) + (d * ek_gi)) * invDet,
                                        -((a * fl_hj) - (b * el_hi) + (d * ej_fi)) * invDet,
                                        +((a * fk_gj) - (b * ek_gi) + (c * ej_fi)) * invDet);

                return new Matrix4x4(new Vector4(xx, xy, xz, xw),
                                     new Vector4(yx, yy, yz, yw),
                                     new Vector4(zx, zy, zz, zw),
                                     new Vector4(wx, wy, wz, ww));
            }
        }

        /// <summary>Converts the current instance into a string that represents its value.</summary>
        /// <returns>A <see cref="string"/> that represents the value of the current instance.</returns>
        public override string ToString() => $"[{_x}, {_y}, {_z}, {_w}]";
    }
}
