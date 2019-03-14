using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Mathematics
{
    /// <summary>
    ///     <para>Represents a four-by-four dimension matrix.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 16, Size = 16)]
    public struct Matrix2x2 : IComparable, IComparable<Matrix2x2>, IEquatable<Matrix2x2>
    {
        #region Default Instances
        /// <summary>
        ///     <para>Represents a <see cref="Matrix2x2"/> whose main-diagonal components are set to one and whose remaining components are all set to zero.</para>
        /// </summary>
        public static readonly Matrix2x2 Identity = new Matrix2x2(Vector2.UnitX, Vector2.UnitY);
        #endregion

        #region Fields
        private Vector2 _x;
        private Vector2 _y;
        #endregion

        #region Properties
        /// <summary>
        ///     <para>Gets or sets the value of the component at the specified index for the current instance.</para>
        /// </summary>
        /// <param name="index">The index of the component to get or set.</param>
        public unsafe Vector2 this[int index]
        {
            get
            {
                if ((index < 0) || (index > 1))
                {
                    throw new IndexOutOfRangeException();
                }
                Contract.EndContractBlock();

                fixed (Matrix2x2* pVector = &this)
                {
                    return ((Vector2*)pVector)[index];
                }
            }

            set
            {
                if ((index < 0) || (index > 1))
                {
                    throw new IndexOutOfRangeException();
                }
                Contract.EndContractBlock();

                fixed (Matrix2x2* pVector = &this)
                {
                    ((Vector2*)pVector)[index] = value;
                }
            }
        }

        /// <summary>
        ///     <para>Gets the determinant of the current instance.</para>
        /// </summary>
        public float Determinant
        {
            get
            {
                return (X.X * Y.Y) -
                       (X.Y * Y.X);
            }
        }

        /// <summary>
        ///     <para>Gets or sets the value of the x-component for the current instance.</para>
        /// </summary>
        public Vector2 X
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
        public Vector2 Y
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
        #endregion

        #region Constructors
        /// <summary>
        ///     <para>Initializes a new instance of the <see cref="Matrix2x2"/> struct.</para>
        /// </summary>
        /// <param name="x">The initial value for the x-component of the matrix.</param>
        /// <param name="y">The initial value for the y-component of the matrix.</param>
        public Matrix2x2(Vector2 x, Vector2 y)
        {
            _x = x;
            _y = y;
        }
        #endregion

        #region Operators
        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine equality.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Matrix2x2 left, Matrix2x2 right)
        {
            return (left._x == right._x) &&
                   (left._y == right._y);
        }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine equality.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix2x2 left, Matrix2x2 right)
        {
            return !(left == right);
        }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <(Matrix2x2 left, Matrix2x2 right)
        {
            return left.Determinant < right.Determinant;
        }


        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Matrix2x2 left, Matrix2x2 right)
        {
            return left.Determinant <= right.Determinant;
        }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >(Matrix2x2 left, Matrix2x2 right)
        {
            return left.Determinant > right.Determinant;
        }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Matrix2x2 left, Matrix2x2 right)
        {
            return left.Determinant >= right.Determinant;
        }

        /// <summary>
        ///     <para>Negates a <see cref="Matrix2x2"/> instance to determine its additive inverse.</para>
        /// </summary>
        /// <param name="right">The matrix to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Matrix2x2 operator -(Matrix2x2 right)
        {
            return right * (-1.0f);
        }

        /// <summary>
        ///     <para>Adds two <see cref="Matrix2x2"/> instances to determine their sum.</para>
        /// </summary>
        /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Matrix2x2 operator +(Matrix2x2 left, Matrix2x2 right)
        {
            return new Matrix2x2(left._x + right._x,
                                 left._y + right._y);
        }

        /// <summary>
        ///     <para>Subtracts two <see cref="Matrix2x2"/> instances to determine their difference.</para>
        /// </summary>
        /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Matrix2x2 operator -(Matrix2x2 left, Matrix2x2 right)
        {
            return left + (-right);
        }

        /// <summary>
        ///     <para>Multiplies two <see cref="Matrix2x2"/> instances to determine their product.</para>
        /// </summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix2x2 operator *(Matrix2x2 left, Matrix2x2 right)
        {
            var x = new Vector2((left.X.X * right.X.X) + (left.X.Y * right.Y.X),
                                      (left.X.X * right.X.Y) + (left.X.Y * right.Y.Y));

            var y = new Vector2((left.Y.X * right.X.X) + (left.Y.Y * right.Y.X),
                                      (left.Y.X * right.X.Y) + (left.Y.Y * right.Y.Y));

            return new Matrix2x2(x, y);
        }

        /// <summary>
        ///     <para>Multiples a <see cref="Matrix2x2"/> and <see cref="float"/> instance to determine their product.</para>
        /// </summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix2x2 operator *(Matrix2x2 left, float right)
        {
            return new Matrix2x2(left._x * right,
                                 left._y * right);
        }

        /// <summary>
        ///     <para>Divides a <see cref="Matrix2x2"/> and <see cref="float"/> instance to determine their quotient.</para>
        /// </summary>
        /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Matrix2x2 operator /(Matrix2x2 left, float right)
        {
            return left * (1.0f / right);
        }
        #endregion

        #region Friendly Operators
        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine equality.</para>
        /// </summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool Equals(Matrix2x2 left, Matrix2x2 right)
        {
            return left == right;
        }

        /// <summary>
        ///     <para>Compares two <see cref="Matrix2x2"/> instances to determine sort order.</para>
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
        public static int Compare(Matrix2x2 left, Matrix2x2 right)
        {
            return (int)(left.Determinant - right.Determinant);
        }

        /// <summary>
        ///     <para>Negates a <see cref="Matrix2x2"/> instance to determine its additive inverse.</para>
        /// </summary>
        /// <param name="right">The matrix to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Matrix2x2 Negate(Matrix2x2 right)
        {
            return -right;
        }

        /// <summary>
        ///     <para>Adds two <see cref="Matrix2x2"/> instances to determine their sum.</para>
        /// </summary>
        /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Matrix2x2 Add(Matrix2x2 left, Matrix2x2 right)
        {
            return left + right;
        }

        /// <summary>
        ///     <para>Subtracts two <see cref="Matrix2x2"/> instances to determine their difference.</para>
        /// </summary>
        /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Matrix2x2 Subtract(Matrix2x2 left, Matrix2x2 right)
        {
            return left - right;
        }

        /// <summary>
        ///     <para>Multiplies two <see cref="Matrix2x2"/> instances to determine their product.</para>
        /// </summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix2x2 Multiply(Matrix2x2 left, Matrix2x2 right)
        {
            return left * right;
        }

        /// <summary>
        ///     <para>Multiples a <see cref="Matrix2x2"/> and <see cref="float"/> instance to determine their product.</para>
        /// </summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix2x2 Multiply(Matrix2x2 left, float right)
        {
            return left * right;
        }

        /// <summary>
        ///     <para>Divides a <see cref="Matrix2x2"/> and <see cref="float"/> instance to determine their quotient.</para>
        /// </summary>
        /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Matrix2x2 Divide(Matrix2x2 left, float right)
        {
            return left / right;
        }

        /// <summary>
        ///     <para>Determines the transpose of a <see cref="Matrix2x2"/> instance.</para>
        /// </summary>
        /// <param name="right">The matrix to transpose.</param>
        /// <returns>The transpose of <paramref name="right"/>.</returns>
        public static Matrix2x2 Transpose(Matrix2x2 right)
        {
            var x = new Vector2(right._x.X, right._y.X);
            var y = new Vector2(right._x.Y, right._y.Y);

            return new Matrix2x2(x, y);
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
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not an instance of <see cref="Matrix2x2"/>.</exception>
        public int CompareTo(object obj)
        {
            if ((obj is Matrix2x2) == false)
            {
                throw new ArgumentException("obj is not an instance of Mathematics.Matrix2x2", "obj");
            }
            Contract.EndContractBlock();

            return CompareTo((Matrix2x2)obj);
        }

        /// <summary>
        ///     <para>Compares a <see cref="Matrix2x2"/> to the current instance to determine sort order.</para>
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
        public int CompareTo(Matrix2x2 other)
        {
            return Compare(this, other);
        }

        /// <summary>
        ///     <para>Compares a <see cref="object"/> to the current instance to determine equality.</para>
        /// </summary>
        /// <param name="obj">The object to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Matrix2x2"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return (obj is Matrix2x2) &&
                   Equals((Matrix2x2)obj);
        }

        /// <summary>
        ///     <para>Compares a <see cref="Matrix2x2"/> to the current instance to determine equality.</para>
        /// </summary>
        /// <param name="other">The matrix to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix2x2 other)
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
            return string.Format(CultureInfo.CurrentCulture.NumberFormat, "[{0} {1}]", _x, _y);
        }
        #endregion
    }
}
