// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;

namespace Mathematics
{
    /// <summary>Represents a four-by-four dimension matrix.</summary>
    public struct Matrix2x2 : IComparable, IComparable<Matrix2x2>, IEquatable<Matrix2x2>
    {
        /// <summary>Represents a <see cref="Matrix2x2"/> whose main-diagonal components are set to one and whose remaining components are all set to zero.</summary>
        public static readonly Matrix2x2 Identity = new Matrix2x2(Vector2.UnitX, Vector2.UnitY);

        private Vector2 _x;
        private Vector2 _y;

        /// <summary>Gets or sets the value of the component at the specified index for the current instance.</summary>
        /// <param name="index">The index of the component to get or set.</param>
        public unsafe Vector2 this[int index]
        {
            get
            {
                if ((uint)(index) > 1)
                {
                    throw new IndexOutOfRangeException();
                }
                return Unsafe.Add(ref _x, index);
            }

            set
            {
                if ((uint)(index) > 1)
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
                return (X.X * Y.Y) -
                       (X.Y * Y.X);
            }
        }

        /// <summary>Gets or sets the value of the x-component for the current instance.</summary>
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

        /// <summary>Gets or sets the value of the y-component for the current instance.</summary>
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

        /// <summary>Initializes a new instance of the <see cref="Matrix2x2"/> struct.</summary>
        /// <param name="x">The initial value for the x-component of the matrix.</param>
        /// <param name="y">The initial value for the y-component of the matrix.</param>
        public Matrix2x2(Vector2 x, Vector2 y)
        {
            _x = x;
            _y = y;
        }

        /// <summary>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine equality.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Matrix2x2 left, Matrix2x2 right)
        {
            return (left._x == right._x) &&
                   (left._y == right._y);
        }

        /// <summary>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine equality.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix2x2 left, Matrix2x2 right) => !(left == right);

        /// <summary>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <(Matrix2x2 left, Matrix2x2 right) => left.Determinant < right.Determinant;

        /// <summary>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Matrix2x2 left, Matrix2x2 right) => left.Determinant <= right.Determinant;

        /// <summary>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >(Matrix2x2 left, Matrix2x2 right) => left.Determinant > right.Determinant;

        /// <summary>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Matrix2x2 left, Matrix2x2 right) => left.Determinant >= right.Determinant;

        /// <summary>Negates a <see cref="Matrix2x2"/> instance to determine its additive inverse.</summary>
        /// <param name="right">The matrix to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Matrix2x2 operator -(Matrix2x2 right) => right * (-1.0f);

        /// <summary>Adds two <see cref="Matrix2x2"/> instances to determine their sum.</summary>
        /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Matrix2x2 operator +(Matrix2x2 left, Matrix2x2 right)
        {
            return new Matrix2x2(left._x + right._x,
                                 left._y + right._y);
        }

        /// <summary>Subtracts two <see cref="Matrix2x2"/> instances to determine their difference.</summary>
        /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Matrix2x2 operator -(Matrix2x2 left, Matrix2x2 right) => left + (-right);

        /// <summary>Multiplies two <see cref="Matrix2x2"/> instances to determine their product.</summary>
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

        /// <summary>Multiples a <see cref="Matrix2x2"/> and <see cref="float"/> instance to determine their product.</summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix2x2 operator *(Matrix2x2 left, float right)
        {
            return new Matrix2x2(left._x * right,
                                 left._y * right);
        }

        /// <summary>Divides a <see cref="Matrix2x2"/> and <see cref="float"/> instance to determine their quotient.</summary>
        /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Matrix2x2 operator /(Matrix2x2 left, float right) => left * (1.0f / right);

        /// <summary>Performs a comparison between two <see cref="Matrix2x2"/> instances to determine equality.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool Equals(Matrix2x2 left, Matrix2x2 right) => left == right;

        /// <summary>Compares two <see cref="Matrix2x2"/> instances to determine sort order.</summary>
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
        public static int Compare(Matrix2x2 left, Matrix2x2 right) => left.Determinant.CompareTo(right.Determinant);

        /// <summary>Negates a <see cref="Matrix2x2"/> instance to determine its additive inverse.</summary>
        /// <param name="right">The matrix to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Matrix2x2 Negate(Matrix2x2 right) => -right;

        /// <summary>Adds two <see cref="Matrix2x2"/> instances to determine their sum.</summary>
        /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Matrix2x2 Add(Matrix2x2 left, Matrix2x2 right) => left + right;

        /// <summary>Subtracts two <see cref="Matrix2x2"/> instances to determine their difference.</summary>
        /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Matrix2x2 Subtract(Matrix2x2 left, Matrix2x2 right) => left - right;

        /// <summary>Multiplies two <see cref="Matrix2x2"/> instances to determine their product.</summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix2x2 Multiply(Matrix2x2 left, Matrix2x2 right) => left * right;

        /// <summary>Multiples a <see cref="Matrix2x2"/> and <see cref="float"/> instance to determine their product.</summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix2x2 Multiply(Matrix2x2 left, float right) => left * right;

        /// <summary>Divides a <see cref="Matrix2x2"/> and <see cref="float"/> instance to determine their quotient.</summary>
        /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Matrix2x2 Divide(Matrix2x2 left, float right) => left / right;

        /// <summary>Determines the transpose of a <see cref="Matrix2x2"/> instance.</summary>
        /// <param name="right">The matrix to transpose.</param>
        /// <returns>The transpose of <paramref name="right"/>.</returns>
        public static Matrix2x2 Transpose(Matrix2x2 right)
        {
            var x = new Vector2(right._x.X, right._y.X);
            var y = new Vector2(right._x.Y, right._y.Y);

            return new Matrix2x2(x, y);
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
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not an instance of <see cref="Matrix2x2"/>.</exception>
        public int CompareTo(object? obj)
        {
            if (obj is Matrix2x2 other)
            {
                return CompareTo(other);
            }
            return (obj is null) ? 1 : throw new ArgumentException($"{nameof(obj)} is not an instance of {nameof(Matrix2x2)}", nameof(obj));
        }

        /// <summary>Compares a <see cref="Matrix2x2"/> to the current instance to determine sort order.</summary>
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
        public int CompareTo(Matrix2x2 other) => Compare(this, other);

        /// <summary>Compares a <see cref="object"/> to the current instance to determine equality.</summary>
        /// <param name="obj">The object to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Matrix2x2"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj) => (obj is Matrix2x2 other) && Equals(other);

        /// <summary>Compares a <see cref="Matrix2x2"/> to the current instance to determine equality.</summary>
        /// <param name="other">The matrix to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix2x2 other) => Equals(this, other);

        /// <summary>Generates a hash code for the value of the current instance.</summary>
        /// <returns>A hash code for the value of the current instance.</returns>
        public override int GetHashCode() => Determinant.GetHashCode();

        /// <summary>Converts the current instance into a string that represents its value.</summary>
        /// <returns>A <see cref="string"/> that represents the value of the current instance.</returns>
        public override string ToString() => $"[{_x}, {_y}]";
    }
}
