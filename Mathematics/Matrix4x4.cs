using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Mathematics
{
    /// <summary>
    ///     <para>Represents a four-by-four dimension matrix.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 16, Size = 64)]
    public struct Matrix4x4 : IComparable, IComparable<Matrix4x4>, IEquatable<Matrix4x4>
    {
        #region Default Instances
        /// <summary>
        ///     <para>Represents a <see cref="Matrix4x4"/> whose main-diagonal components are set to one and whose remaining components are all set to zero.</para>
        /// </summary>
        public static readonly Matrix4x4 Identity = new Matrix4x4(Vector4D.UnitX, Vector4D.UnitY, Vector4D.UnitZ, Vector4D.UnitW);
        #endregion

        #region Fields
            private Vector4D _x;
            private Vector4D _y;
            private Vector4D _z;
            private Vector4D _w;
        #endregion

        #region Constructors
        /// <summary>
        ///     <para>Initializes a new instance of the <see cref="Matrix4x4"/> struct.</para>
        /// </summary>
        /// <param name="x">The initial value for the x-component of the matrix.</param>
        /// <param name="y">The initial value for the y-component of the matrix.</param>
        /// <param name="z">The initial value for the z-component of the matrix.</param>
        /// <param name="w">The initial value for the w-component of the matrix.</param>
        public Matrix4x4(Vector4D x, Vector4D y, Vector4D z, Vector4D w)
            {
                _x = x;
                _y = y;
                _z = z;
                _w = w;
            }
        #endregion

        #region Operators
        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine equality.</para>
        /// </summary>
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

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine equality.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix4x4 left, Matrix4x4 right)
            {
                return !(left == right);
            }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <(Matrix4x4 left, Matrix4x4 right)
            {
                return left.Determinant < right.Determinant;
            }


        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Matrix4x4 left, Matrix4x4 right)
            {
                return left.Determinant <= right.Determinant;
            }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >(Matrix4x4 left, Matrix4x4 right)
            {
                return left.Determinant > right.Determinant;
            }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Matrix4x4 left, Matrix4x4 right)
            {
                return left.Determinant >= right.Determinant;
            }

        /// <summary>
        ///     <para>Negates a <see cref="Matrix4x4"/> instance to determine its additive inverse.</para>
        /// </summary>
        /// <param name="right">The matrix to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Matrix4x4 operator -(Matrix4x4 right)
            {
                return right * (-1.0f);
            }

        /// <summary>
        ///     <para>Adds two <see cref="Matrix4x4"/> instances to determine their sum.</para>
        /// </summary>
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

        /// <summary>
        ///     <para>Subtracts two <see cref="Matrix4x4"/> instances to determine their difference.</para>
        /// </summary>
        /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Matrix4x4 operator -(Matrix4x4 left, Matrix4x4 right)
            {
                return left + (-right);
            }

        /// <summary>
        ///     <para>Multiplies two <see cref="Matrix4x4"/> instances to determine their product.</para>
        /// </summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
            {
                var x = new Vector4D((left.X.X * right.X.X) + (left.X.Y * right.Y.X) + (left.X.Z * right.Z.X) + (left.X.W * right.W.X),
                                          (left.X.X * right.X.Y) + (left.X.Y * right.Y.Y) + (left.X.Z * right.Z.Y) + (left.X.W * right.W.Y),
                                          (left.X.X * right.X.Z) + (left.X.Y * right.Y.Z) + (left.X.Z * right.Z.Z) + (left.X.W * right.W.Z),
                                          (left.X.X * right.X.W) + (left.X.Y * right.Y.W) + (left.X.Z * right.Z.W) + (left.X.W * right.W.W));

                var y = new Vector4D((left.Y.X * right.X.X) + (left.Y.Y * right.Y.X) + (left.Y.Z * right.Z.X) + (left.Y.W * right.W.X),
                                          (left.Y.X * right.X.Y) + (left.Y.Y * right.Y.Y) + (left.Y.Z * right.Z.Y) + (left.Y.W * right.W.Y),
                                          (left.Y.X * right.X.Z) + (left.Y.Y * right.Y.Z) + (left.Y.Z * right.Z.Z) + (left.Y.W * right.W.Z),
                                          (left.Y.X * right.X.W) + (left.Y.Y * right.Y.W) + (left.Y.Z * right.Z.W) + (left.Y.W * right.W.W));

                var z = new Vector4D((left.Z.X * right.X.X) + (left.Z.Y * right.Y.X) + (left.Z.Z * right.Z.X) + (left.Z.W * right.W.X),
                                          (left.Z.X * right.X.Y) + (left.Z.Y * right.Y.Y) + (left.Z.Z * right.Z.Y) + (left.Z.W * right.W.Y),
                                          (left.Z.X * right.X.Z) + (left.Z.Y * right.Y.Z) + (left.Z.Z * right.Z.Z) + (left.Z.W * right.W.Z),
                                          (left.Z.X * right.X.W) + (left.Z.Y * right.Y.W) + (left.Z.Z * right.Z.W) + (left.Z.W * right.W.W));

                var w = new Vector4D((left.W.X * right.X.X) + (left.W.Y * right.Y.X) + (left.W.Z * right.Z.X) + (left.W.W * right.W.X),
                                          (left.W.X * right.X.Y) + (left.W.Y * right.Y.Y) + (left.W.Z * right.Z.Y) + (left.W.W * right.W.Y),
                                          (left.W.X * right.X.Z) + (left.W.Y * right.Y.Z) + (left.W.Z * right.Z.Z) + (left.W.W * right.W.Z),
                                          (left.W.X * right.X.W) + (left.W.Y * right.Y.W) + (left.W.Z * right.Z.W) + (left.W.W * right.W.W));

                return new Matrix4x4(x, y, z, w);
            }

        /// <summary>
        ///     <para>Multiples a <see cref="Matrix4x4"/> and <see cref="float"/> instance to determine their product.</para>
        /// </summary>
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

        /// <summary>
        ///     <para>Divides a <see cref="Matrix4x4"/> and <see cref="float"/> instance to determine their quotient.</para>
        /// </summary>
        /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Matrix4x4 operator /(Matrix4x4 left, float right)
            {
                return left * (1.0f / right);
            }
        #endregion

        #region Friendly Operators
        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix4x4"/> instances to determine equality.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool Equals(Matrix4x4 left, Matrix4x4 right)
            {
                return left == right;
            }

        /// <summary>
        ///     <para>Compares two <see cref="Matrix4x4"/> instances to determine sort order.</para>
        /// </summary>
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
        public static int Compare(Matrix4x4 left, Matrix4x4 right)
            {
                return (int)(left.Determinant - right.Determinant);
            }

        /// <summary>
        ///     <para>Negates a <see cref="Matrix4x4"/> instance to determine its additive inverse.</para>
        /// </summary>
        /// <param name="right">The matrix to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Matrix4x4 Negate(Matrix4x4 right)
            {
                return -right;
            }

        /// <summary>
        ///     <para>Adds two <see cref="Matrix4x4"/> instances to determine their sum.</para>
        /// </summary>
        /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Matrix4x4 Add(Matrix4x4 left, Matrix4x4 right)
            {
                return left + right;
            }

        /// <summary>
        ///     <para>Subtracts two <see cref="Matrix4x4"/> instances to determine their difference.</para>
        /// </summary>
        /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Matrix4x4 Subtract(Matrix4x4 left, Matrix4x4 right)
            {
                return left - right;
            }

        /// <summary>
        ///     <para>Multiplies two <see cref="Matrix4x4"/> instances to determine their product.</para>
        /// </summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix4x4 Multiply(Matrix4x4 left, Matrix4x4 right)
            {
                return left * right;
            }

        /// <summary>
        ///     <para>Multiples a <see cref="Matrix4x4"/> and <see cref="float"/> instance to determine their product.</para>
        /// </summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix4x4 Multiply(Matrix4x4 left, float right)
            {
                return left * right;
            }

        /// <summary>
        ///     <para>Divides a <see cref="Matrix4x4"/> and <see cref="float"/> instance to determine their quotient.</para>
        /// </summary>
        /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Matrix4x4 Divide(Matrix4x4 left, float right)
            {
                return left / right;
            }

        /// <summary>
        ///     <para>Determines the transpose of a <see cref="Matrix4x4"/> instance.</para>
        /// </summary>
        /// <param name="right">The matrix to transpose.</param>
        /// <returns>The transpose of <paramref name="right"/>.</returns>
        public static Matrix4x4 Transpose(Matrix4x4 right)
            {
                var x = new Vector4D(right._x.X, right._y.X, right._z.X, right._w.X);
                var y = new Vector4D(right._x.Y, right._y.Y, right._z.Y, right._w.Y);
                var z = new Vector4D(right._x.Z, right._y.Z, right._z.Z, right._w.Z);
                var w = new Vector4D(right._x.W, right._y.W, right._z.W, right._w.W);

                return new Matrix4x4(x, y, z, w);
            }
        #endregion

        #region Methods
        /// <summary>
        ///     <para>Compares a <see cref="object"/> to the current instance to determine sort order.</para>
        /// </summary>
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
        public int CompareTo(object obj)
            {
                if ((obj is Matrix4x4) == false)
                {
                    throw new ArgumentException("obj is not an instance of Mathematics.Matrix4x4", "obj");
                }
                Contract.EndContractBlock();

                return CompareTo((Matrix4x4)obj);
            }

        /// <summary>
        ///     <para>Compares a <see cref="Matrix4x4"/> to the current instance to determine sort order.</para>
        /// </summary>
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
        public int CompareTo(Matrix4x4 other)
            {
                return Compare(this, other);
            }

        /// <summary>
        ///     <para>Compares a <see cref="object"/> to the current instance to determine equality.</para>
        /// </summary>
        /// <param name="obj">The object to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Matrix4x4"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
            {
                return (obj is Matrix4x4) &&
                       Equals((Matrix4x4)obj);
            }

        /// <summary>
        ///     <para>Compares a <see cref="Matrix4x4"/> to the current instance to determine equality.</para>
        /// </summary>
        /// <param name="other">The matrix to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix4x4 other)
            {
                return Equals(this, other);
            }
        
            /// <summary>
            ///     <para>Generates a hash code for the value of the current instance.</para>
            /// </summary>
            /// <returns>A hash code for the value of the current instance.</returns>
            public override int GetHashCode()
            {
                return Determinant.GetHashCode();
            }

        /// <summary>
        ///     <para>Converts the current instance into a string that represents its value.</para>
        /// </summary>
        /// <returns>A <see cref="string"/> that represents the value of the current instance.</returns>
        public override string ToString()
            {
                return string.Format(CultureInfo.CurrentCulture.NumberFormat, "[{0} {1} {2} {3}]", _x, _y, _z, _w);
            }
        #endregion

        #region Properties
            /// <summary>
            ///     <para>Gets or sets the value of the component at the specified index for the current instance.</para>
            /// </summary>
            /// <param name="index">The index of the component to get or set.</param>
            public unsafe Vector4D this[int index]
            {
                get
                {
                    if ((index < 0) || (index > 3))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    Contract.EndContractBlock();

                    fixed (Matrix4x4* pVector = &this)
                    {
                        return ((Vector4D*)pVector)[index];
                    }
                }

                set
                {
                    if ((index < 0) || (index > 3))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    Contract.EndContractBlock();

                    fixed (Matrix4x4* pVector = &this)
                    {
                        ((Vector4D*)pVector)[index] = value;
                    }
                }
            }

            /// <summary>
            ///     <para>Gets or sets the value of the x-component for the current instance.</para>
            /// </summary>
            public Vector4D X
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

            /// <summary>
            ///     <para>Gets or sets the value of the y-component for the current instance.</para>
            /// </summary>
            public Vector4D Y
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

            /// <summary>
            ///     <para>Gets or sets the value of the z-component for the current instance.</para>
            /// </summary>
            public Vector4D Z
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

            /// <summary>
            ///     <para>Gets or sets the value of the w-component for the current instance.</para>
            /// </summary>
            public Vector4D W
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

            /// <summary>
            ///     <para>Gets the determinant of the current instance.</para>
            /// </summary>
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
        #endregion
    }
}
