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
            ///     <para>Represents a <see cref="Mathematics.Matrix4x4"/> whose main-diagonal components are set to one and whose remaining components are all set to zero.</para>
            /// </summary>
            public static readonly Matrix4x4 Identity = new Matrix4x4(Vector4D.UnitX, Vector4D.UnitY, Vector4D.UnitZ, Vector4D.UnitW);
        #endregion

        #region Fields
            private Vector4D x;
            private Vector4D y;
            private Vector4D z;
            private Vector4D w;
        #endregion

        #region Constructors
            /// <summary>
            ///     <para>Initializes a new instance of the <see cref="Mathematics.Matrix4x4"/> struct.</para>
            /// </summary>
            /// <param name="x">The initial value for the x-component of the matrix.</param>
            /// <param name="y">The initial value for the y-component of the matrix.</param>
            /// <param name="z">The initial value for the z-component of the matrix.</param>
            /// <param name="w">The initial value for the w-component of the matrix.</param>
            public Matrix4x4(Vector4D x, Vector4D y, Vector4D z, Vector4D w)
            {
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }
        #endregion

        #region Operators
            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Matrix4x4"/> instances to determine equality.</para>
            /// </summary>
            /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
            /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
            public static bool operator ==(Matrix4x4 left, Matrix4x4 right)
            {
                return (left.x == right.x) &&
                       (left.y == right.y) &&
                       (left.z == right.z) &&
                       (left.w == right.w);
            }

            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Matrix4x4"/> instances to determine equality.</para>
            /// </summary>
            /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
            /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <c>false</c>.</returns>
            public static bool operator !=(Matrix4x4 left, Matrix4x4 right)
            {
                return !(left == right);
            }

            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Matrix4x4"/> instances to determine sort order.</para>
            /// </summary>
            /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
            /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
            public static bool operator <(Matrix4x4 left, Matrix4x4 right)
            {
                return (left.Determinant < right.Determinant);
            }


            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Matrix4x4"/> instances to determine sort order.</para>
            /// </summary>
            /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
            /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
            public static bool operator <=(Matrix4x4 left, Matrix4x4 right)
            {
                return (left.Determinant <= right.Determinant);
            }

            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Matrix4x4"/> instances to determine sort order.</para>
            /// </summary>
            /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
            /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
            public static bool operator >(Matrix4x4 left, Matrix4x4 right)
            {
                return (left.Determinant > right.Determinant);
            }

            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Matrix4x4"/> instances to determine sort order.</para>
            /// </summary>
            /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
            /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
            public static bool operator >=(Matrix4x4 left, Matrix4x4 right)
            {
                return (left.Determinant >= right.Determinant);
            }

            /// <summary>
            ///     <para>Negates a <see cref="Mathematics.Matrix4x4"/> instance to determine its additive inverse.</para>
            /// </summary>
            /// <param name="right">The matrix to negate.</param>
            /// <returns>The additive inverse of <paramref name="right"/>.</returns>
            public static Matrix4x4 operator -(Matrix4x4 right)
            {
                return (right * (-1.0f));
            }

            /// <summary>
            ///     <para>Adds two <see cref="Mathematics.Matrix4x4"/> instances to determine their sum.</para>
            /// </summary>
            /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
            /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
            /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
            public static Matrix4x4 operator +(Matrix4x4 left, Matrix4x4 right)
            {
                return new Matrix4x4((left.x + right.x),
                                     (left.y + right.y),
                                     (left.z + right.z),
                                     (left.w + right.w));
            }

            /// <summary>
            ///     <para>Subtracts two <see cref="Mathematics.Matrix4x4"/> instances to determine their difference.</para>
            /// </summary>
            /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
            /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
            /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
            public static Matrix4x4 operator -(Matrix4x4 left, Matrix4x4 right)
            {
                return (left + (-right));
            }

            /// <summary>
            ///     <para>Multiplies two <see cref="Mathematics.Matrix4x4"/> instances to determine their product.</para>
            /// </summary>
            /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
            /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
            /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
            public static Matrix4x4 operator *(Matrix4x4 left, Matrix4x4 right)
            {
                Vector4D x = new Vector4D((left.X.X * right.X.X) + (left.X.Y * right.Y.X) + (left.X.Z * right.Z.X) + (left.X.W * right.W.X),
                                          (left.X.X * right.X.Y) + (left.X.Y * right.Y.Y) + (left.X.Z * right.Z.Y) + (left.X.W * right.W.Y),
                                          (left.X.X * right.X.Z) + (left.X.Y * right.Y.Z) + (left.X.Z * right.Z.Z) + (left.X.W * right.W.Z),
                                          (left.X.X * right.X.W) + (left.X.Y * right.Y.W) + (left.X.Z * right.Z.W) + (left.X.W * right.W.W));

                Vector4D y = new Vector4D((left.Y.X * right.X.X) + (left.Y.Y * right.Y.X) + (left.Y.Z * right.Z.X) + (left.Y.W * right.W.X),
                                          (left.Y.X * right.X.Y) + (left.Y.Y * right.Y.Y) + (left.Y.Z * right.Z.Y) + (left.Y.W * right.W.Y),
                                          (left.Y.X * right.X.Z) + (left.Y.Y * right.Y.Z) + (left.Y.Z * right.Z.Z) + (left.Y.W * right.W.Z),
                                          (left.Y.X * right.X.W) + (left.Y.Y * right.Y.W) + (left.Y.Z * right.Z.W) + (left.Y.W * right.W.W));

                Vector4D z = new Vector4D((left.Z.X * right.X.X) + (left.Z.Y * right.Y.X) + (left.Z.Z * right.Z.X) + (left.Z.W * right.W.X),
                                          (left.Z.X * right.X.Y) + (left.Z.Y * right.Y.Y) + (left.Z.Z * right.Z.Y) + (left.Z.W * right.W.Y),
                                          (left.Z.X * right.X.Z) + (left.Z.Y * right.Y.Z) + (left.Z.Z * right.Z.Z) + (left.Z.W * right.W.Z),
                                          (left.Z.X * right.X.W) + (left.Z.Y * right.Y.W) + (left.Z.Z * right.Z.W) + (left.Z.W * right.W.W));

                Vector4D w = new Vector4D((left.W.X * right.X.X) + (left.W.Y * right.Y.X) + (left.W.Z * right.Z.X) + (left.W.W * right.W.X),
                                          (left.W.X * right.X.Y) + (left.W.Y * right.Y.Y) + (left.W.Z * right.Z.Y) + (left.W.W * right.W.Y),
                                          (left.W.X * right.X.Z) + (left.W.Y * right.Y.Z) + (left.W.Z * right.Z.Z) + (left.W.W * right.W.Z),
                                          (left.W.X * right.X.W) + (left.W.Y * right.Y.W) + (left.W.Z * right.Z.W) + (left.W.W * right.W.W));

                return new Matrix4x4(x, y, z, w);
            }

            /// <summary>
            ///     <para>Multiples a <see cref="Mathematics.Matrix4x4"/> and <see cref="System.Single"/> instance to determine their product.</para>
            /// </summary>
            /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
            /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
            /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
            public static Matrix4x4 operator *(Matrix4x4 left, float right)
            {
                return new Matrix4x4((left.x * right),
                                     (left.y * right),
                                     (left.z * right),
                                     (left.w * right));
            }

            /// <summary>
            ///     <para>Divides a <see cref="Mathematics.Matrix4x4"/> and <see cref="System.Single"/> instance to determine their quotient.</para>
            /// </summary>
            /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
            /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
            /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
            public static Matrix4x4 operator /(Matrix4x4 left, float right)
            {
                return (left * (1.0f / right));
            }
        #endregion

        #region Friendly Operators
            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Matrix4x4"/> instances to determine equality.</para>
            /// </summary>
            /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
            /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
            public static bool Equals(Matrix4x4 left, Matrix4x4 right)
            {
                return (left == right);
            }

            /// <summary>
            ///     <para>Compares two <see cref="Mathematics.Matrix4x4"/> instances to determine sort order.</para>
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
            ///     <para>Negates a <see cref="Mathematics.Matrix4x4"/> instance to determine its additive inverse.</para>
            /// </summary>
            /// <param name="right">The matrix to negate.</param>
            /// <returns>The additive inverse of <paramref name="right"/>.</returns>
            public static Matrix4x4 Negate(Matrix4x4 right)
            {
                return -right;
            }

            /// <summary>
            ///     <para>Adds two <see cref="Mathematics.Matrix4x4"/> instances to determine their sum.</para>
            /// </summary>
            /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
            /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
            /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
            public static Matrix4x4 Add(Matrix4x4 left, Matrix4x4 right)
            {
                return (left + right);
            }

            /// <summary>
            ///     <para>Subtracts two <see cref="Mathematics.Matrix4x4"/> instances to determine their difference.</para>
            /// </summary>
            /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
            /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
            /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
            public static Matrix4x4 Subtract(Matrix4x4 left, Matrix4x4 right)
            {
                return (left - right);
            }

            /// <summary>
            ///     <para>Multiplies two <see cref="Mathematics.Matrix4x4"/> instances to determine their product.</para>
            /// </summary>
            /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
            /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
            /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
            public static Matrix4x4 Multiply(Matrix4x4 left, Matrix4x4 right)
            {
                return (left * right);
            }

            /// <summary>
            ///     <para>Multiples a <see cref="Mathematics.Matrix4x4"/> and <see cref="System.Single"/> instance to determine their product.</para>
            /// </summary>
            /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
            /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
            /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
            public static Matrix4x4 Multiply(Matrix4x4 left, float right)
            {
                return (left * right);
            }

            /// <summary>
            ///     <para>Divides a <see cref="Mathematics.Matrix4x4"/> and <see cref="System.Single"/> instance to determine their quotient.</para>
            /// </summary>
            /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
            /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
            /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
            public static Matrix4x4 Divide(Matrix4x4 left, float right)
            {
                return (left / right);
            }

            /// <summary>
            ///     <para>Determines the transpose of a <see cref="Mathematics.Matrix4x4"/> instance.</para>
            /// </summary>
            /// <param name="right">The matrix to transpose.</param>
            /// <returns>The transpose of <paramref name="right"/>.</returns>
            public static Matrix4x4 Transpose(Matrix4x4 right)
            {
                Vector4D x = new Vector4D(right.x.X, right.y.X, right.z.X, right.w.X);
                Vector4D y = new Vector4D(right.x.Y, right.y.Y, right.z.Y, right.w.Y);
                Vector4D z = new Vector4D(right.x.Z, right.y.Z, right.z.Z, right.w.Z);
                Vector4D w = new Vector4D(right.x.W, right.y.W, right.z.W, right.w.W);

                return new Matrix4x4(x, y, z, w);
            }
        #endregion

        #region Methods
            /// <summary>
            ///     <para>Compares a <see cref="System.Object"/> to the current instance to determine sort order.</para>
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
            /// <exception cref="ArgumentException"><paramref name="obj"/> is not an instance of <see cref="Mathematics.Matrix4x4"/>.</exception>
            public int CompareTo(object obj)
            {
                if ((obj is Matrix4x4) == false)
                {
                    throw new ArgumentException("obj is not an instance of Mathematics.Matrix4x4", "obj");
                }
                Contract.EndContractBlock();

                return this.CompareTo((Matrix4x4)(obj));
            }

            /// <summary>
            ///     <para>Compares a <see cref="Mathematics.Matrix4x4"/> to the current instance to determine sort order.</para>
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
                return Matrix4x4.Compare(this, other);
            }

            /// <summary>
            ///     <para>Compares a <see cref="System.Object"/> to the current instance to determine equality.</para>
            /// </summary>
            /// <param name="obj">The object to compare to the current instance.</param>
            /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Mathematics.Matrix4x4"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
            public override bool Equals(object obj)
            {
                return (obj is Matrix4x4) &&
                       this.Equals((Matrix4x4)(obj));
            }

            /// <summary>
            ///     <para>Compares a <see cref="Mathematics.Matrix4x4"/> to the current instance to determine equality.</para>
            /// </summary>
            /// <param name="other">The matrix to compare to the current instance.</param>
            /// <returns><c>true</c> if <paramref name="other"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
            public bool Equals(Matrix4x4 other)
            {
                return Matrix4x4.Equals(this, other);
            }
        
            /// <summary>
            ///     <para>Generates a hash code for the value of the current instance.</para>
            /// </summary>
            /// <returns>A hash code for the value of the current instance.</returns>
            public override int GetHashCode()
            {
                return this.Determinant.GetHashCode();
            }

            /// <summary>
            ///     <para>Converts the current instance into a string that represents its value.</para>
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents the value of the current instance.</returns>
            public override string ToString()
            {
                return string.Format(CultureInfo.CurrentCulture.NumberFormat, "[{0} {1} {2} {3}]", this.x, this.y, this.z, this.w);
            }
        #endregion

        #region Properties
            /// <summary>
            ///     <para>Gets or sets the value of the component at the specified index for the current instance.</para>
            /// </summary>
            /// <param name="index">The index of the component to get or set.</param>
            unsafe public Vector4D this[int index]
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
                        return ((Vector4D*)(pVector))[index];
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
                        ((Vector4D*)(pVector))[index] = value;
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
                    return this.x;
                }

                set
                {
                    this.x = value;
                }
            }

            /// <summary>
            ///     <para>Gets or sets the value of the y-component for the current instance.</para>
            /// </summary>
            public Vector4D Y
            {
                get
                {
                    return this.y;
                }

                set
                {
                    this.y = value;
                }
            }

            /// <summary>
            ///     <para>Gets or sets the value of the z-component for the current instance.</para>
            /// </summary>
            public Vector4D Z
            {
                get
                {
                    return this.z;
                }

                set
                {
                    this.z = value;
                }
            }

            /// <summary>
            ///     <para>Gets or sets the value of the w-component for the current instance.</para>
            /// </summary>
            public Vector4D W
            {
                get
                {
                    return this.w;
                }

                set
                {
                    this.w = value;
                }
            }

            /// <summary>
            ///     <para>Gets the determinant of the current instance.</para>
            /// </summary>
            public float Determinant
            {
                get
                {
                    return (this.X.X * this.Y.Y * this.Z.Z * this.W.W) +
                           (this.X.Y * this.Y.Z * this.Z.W * this.W.X) +
                           (this.X.Z * this.Y.W * this.Z.X * this.W.Y) +
                           (this.X.W * this.Y.X * this.Z.Y * this.W.Z) -
                           (this.X.W * this.Y.Z * this.Z.Y * this.W.X) -
                           (this.X.Z * this.Y.Y * this.Z.X * this.W.W) -
                           (this.X.Y * this.Y.X * this.Z.W * this.W.Z) -
                           (this.X.X * this.Y.W * this.Z.Z * this.W.Y);
                }
            }
        #endregion
    }
}
