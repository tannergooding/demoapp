using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Mathematics
{
    /// <summary>
    ///     <para>Represents a four-by-four dimension matrix.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 16, Size = 36)]
    public struct Matrix3x3 : IComparable, IComparable<Matrix3x3>, IEquatable<Matrix3x3>
    {
        #region Default Instances
        /// <summary>
        ///     <para>Represents a <see cref="Matrix3x3"/> whose main-diagonal components are set to one and whose remaining components are all set to zero.</para>
        /// </summary>
        public static readonly Matrix3x3 Identity = new Matrix3x3(Vector3D.UnitX, Vector3D.UnitY, Vector3D.UnitZ);
        #endregion

        #region Fields
            private Vector3D _x;
            private Vector3D _y;
            private Vector3D _z;
        #endregion

        #region Constructors
        /// <summary>
        ///     <para>Initializes a new instance of the <see cref="Matrix3x3"/> struct.</para>
        /// </summary>
        /// <param name="x">The initial value for the x-component of the matrix.</param>
        /// <param name="y">The initial value for the y-component of the matrix.</param>
        /// <param name="z">The initial value for the z-component of the matrix.</param>
        public Matrix3x3(Vector3D x, Vector3D y, Vector3D z)
            {
                _x = x;
                _y = y;
                _z = z;
            }
        #endregion

        #region Operators
        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine equality.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Matrix3x3 left, Matrix3x3 right)
            {
                return (left._x == right._x) &&
                       (left._y == right._y) &&
                       (left._z == right._z);
            }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine equality.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix3x3 left, Matrix3x3 right)
            {
                return !(left == right);
            }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <(Matrix3x3 left, Matrix3x3 right)
            {
                return left.Determinant < right.Determinant;
            }


        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Matrix3x3 left, Matrix3x3 right)
            {
                return left.Determinant <= right.Determinant;
            }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >(Matrix3x3 left, Matrix3x3 right)
            {
                return left.Determinant > right.Determinant;
            }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Matrix3x3 left, Matrix3x3 right)
            {
                return left.Determinant >= right.Determinant;
            }

        /// <summary>
        ///     <para>Negates a <see cref="Matrix3x3"/> instance to determine its additive inverse.</para>
        /// </summary>
        /// <param name="right">The matrix to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Matrix3x3 operator -(Matrix3x3 right)
            {
                return right * (-1.0f);
            }

        /// <summary>
        ///     <para>Adds two <see cref="Matrix3x3"/> instances to determine their sum.</para>
        /// </summary>
        /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Matrix3x3 operator +(Matrix3x3 left, Matrix3x3 right)
            {
                return new Matrix3x3(left._x + right._x,
                                     left._y + right._y,
                                     left._z + right._z);
            }

        /// <summary>
        ///     <para>Subtracts two <see cref="Matrix3x3"/> instances to determine their difference.</para>
        /// </summary>
        /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Matrix3x3 operator -(Matrix3x3 left, Matrix3x3 right)
            {
                return left + (-right);
            }

        /// <summary>
        ///     <para>Multiplies two <see cref="Matrix3x3"/> instances to determine their product.</para>
        /// </summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix3x3 operator *(Matrix3x3 left, Matrix3x3 right)
            {
                var x = new Vector3D((left.X.X * right.X.X) + (left.X.Y * right.Y.X) + (left.X.Z * right.Z.X),
                                          (left.X.X * right.X.Y) + (left.X.Y * right.Y.Y) + (left.X.Z * right.Z.Y),
                                          (left.X.X * right.X.Z) + (left.X.Y * right.Y.Z) + (left.X.Z * right.Z.Z));

                var y = new Vector3D((left.Y.X * right.X.X) + (left.Y.Y * right.Y.X) + (left.Y.Z * right.Z.X),
                                          (left.Y.X * right.X.Y) + (left.Y.Y * right.Y.Y) + (left.Y.Z * right.Z.Y),
                                          (left.Y.X * right.X.Z) + (left.Y.Y * right.Y.Z) + (left.Y.Z * right.Z.Z));

                var z = new Vector3D((left.Z.X * right.X.X) + (left.Z.Y * right.Y.X) + (left.Z.Z * right.Z.X),
                                          (left.Z.X * right.X.Y) + (left.Z.Y * right.Y.Y) + (left.Z.Z * right.Z.Y),
                                          (left.Z.X * right.X.Z) + (left.Z.Y * right.Y.Z) + (left.Z.Z * right.Z.Z));

                return new Matrix3x3(x, y, z);
            }

        /// <summary>
        ///     <para>Multiples a <see cref="Matrix3x3"/> and <see cref="float"/> instance to determine their product.</para>
        /// </summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix3x3 operator *(Matrix3x3 left, float right)
            {
                return new Matrix3x3(left._x * right,
                                     left._y * right,
                                     left._z * right);
            }

        /// <summary>
        ///     <para>Divides a <see cref="Matrix3x3"/> and <see cref="float"/> instance to determine their quotient.</para>
        /// </summary>
        /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Matrix3x3 operator /(Matrix3x3 left, float right)
            {
                return left * (1.0f / right);
            }
        #endregion

        #region Friendly Operators
        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine equality.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool Equals(Matrix3x3 left, Matrix3x3 right)
            {
                return left == right;
            }

        /// <summary>
        ///     <para>Compares two <see cref="Matrix3x3"/> instances to determine sort order.</para>
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
        public static int Compare(Matrix3x3 left, Matrix3x3 right)
            {
                return (int)(left.Determinant - right.Determinant);
            }

        /// <summary>
        ///     <para>Negates a <see cref="Matrix3x3"/> instance to determine its additive inverse.</para>
        /// </summary>
        /// <param name="right">The matrix to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Matrix3x3 Negate(Matrix3x3 right)
            {
                return -right;
            }

        /// <summary>
        ///     <para>Adds two <see cref="Matrix3x3"/> instances to determine their sum.</para>
        /// </summary>
        /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Matrix3x3 Add(Matrix3x3 left, Matrix3x3 right)
            {
                return left + right;
            }

        /// <summary>
        ///     <para>Subtracts two <see cref="Matrix3x3"/> instances to determine their difference.</para>
        /// </summary>
        /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Matrix3x3 Subtract(Matrix3x3 left, Matrix3x3 right)
            {
                return left - right;
            }

        /// <summary>
        ///     <para>Multiplies two <see cref="Matrix3x3"/> instances to determine their product.</para>
        /// </summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix3x3 Multiply(Matrix3x3 left, Matrix3x3 right)
            {
                return left * right;
            }

        /// <summary>
        ///     <para>Multiples a <see cref="Matrix3x3"/> and <see cref="float"/> instance to determine their product.</para>
        /// </summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix3x3 Multiply(Matrix3x3 left, float right)
            {
                return left * right;
            }

        /// <summary>
        ///     <para>Divides a <see cref="Matrix3x3"/> and <see cref="float"/> instance to determine their quotient.</para>
        /// </summary>
        /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Matrix3x3 Divide(Matrix3x3 left, float right)
            {
                return left / right;
            }

        /// <summary>
        ///     <para>Determines the transpose of a <see cref="Matrix3x3"/> instance.</para>
        /// </summary>
        /// <param name="right">The matrix to transpose.</param>
        /// <returns>The transpose of <paramref name="right"/>.</returns>
        public static Matrix3x3 Transpose(Matrix3x3 right)
            {
                var x = new Vector3D(right._x.X, right._y.X, right._z.X);
                var y = new Vector3D(right._x.Y, right._y.Y, right._z.Y);
                var z = new Vector3D(right._x.Z, right._y.Z, right._z.Z);
                
                return new Matrix3x3(x, y, z);
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
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not an instance of <see cref="Matrix3x3"/>.</exception>
        public int CompareTo(object obj)
            {
                if ((obj is Matrix3x3) == false)
                {
                    throw new ArgumentException("obj is not an instance of Mathematics.Matrix3x3", "obj");
                }
                Contract.EndContractBlock();

                return CompareTo((Matrix3x3)obj);
            }

        /// <summary>
        ///     <para>Compares a <see cref="Matrix3x3"/> to the current instance to determine sort order.</para>
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
        public int CompareTo(Matrix3x3 other)
            {
                return Compare(this, other);
            }

        /// <summary>
        ///     <para>Compares a <see cref="object"/> to the current instance to determine equality.</para>
        /// </summary>
        /// <param name="obj">The object to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Matrix3x3"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
            {
                return (obj is Matrix3x3) &&
                       Equals((Matrix3x3)obj);
            }

        /// <summary>
        ///     <para>Compares a <see cref="Matrix3x3"/> to the current instance to determine equality.</para>
        /// </summary>
        /// <param name="other">The matrix to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix3x3 other)
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
                return string.Format(CultureInfo.CurrentCulture.NumberFormat, "[{0} {1} {2}]", _x, _y, _z);
            }
        #endregion

        #region Properties
            /// <summary>
            ///     <para>Gets or sets the value of the component at the specified index for the current instance.</para>
            /// </summary>
            /// <param name="index">The index of the component to get or set.</param>
            public unsafe Vector3D this[int index]
            {
                get
                {
                    if ((index < 0) || (index > 2))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    Contract.EndContractBlock();

                    fixed (Matrix3x3* pVector = &this)
                    {
                        return ((Vector3D*)pVector)[index];
                    }
                }

                set
                {
                    if ((index < 0) || (index > 2))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    Contract.EndContractBlock();

                    fixed (Matrix3x3* pVector = &this)
                    {
                        ((Vector3D*)pVector)[index] = value;
                    }
                }
            }

            /// <summary>
            ///     <para>Gets or sets the value of the x-component for the current instance.</para>
            /// </summary>
            public Vector3D X
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
            public Vector3D Y
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
            public Vector3D Z
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
            ///     <para>Gets the determinant of the current instance.</para>
            /// </summary>
            public float Determinant
            {
                get
                {
                    return (X.X * Y.Y * Z.Z) +
                           (X.Y * Y.Z * Z.X) +
                           (X.Z * Y.X * Z.Y) -
                           (X.Z * Y.Y * Z.X) -
                           (X.Y * Y.X * Z.Z) -
                           (X.X * Y.Z * Z.Y);
                }
            }
        #endregion
    }
}
