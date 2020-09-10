// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;

namespace Mathematics
{
    /// <summary>Represents a four-by-four dimension matrix.</summary>
    public struct Matrix3x3 : IComparable, IComparable<Matrix3x3>, IEquatable<Matrix3x3>
    {
        /// <summary>Represents a <see cref="Matrix3x3"/> whose main-diagonal components are set to one and whose remaining components are all set to zero.</summary>
        public static readonly Matrix3x3 Identity = new Matrix3x3(Vector3.UnitX, Vector3.UnitY, Vector3.UnitZ);

        private Vector3 _x;
        private Vector3 _y;
        private Vector3 _z;

        /// <summary>Initializes a new instance of the <see cref="Matrix3x3"/> struct.</summary>
        /// <param name="x">The initial value for the x-component of the matrix.</param>
        /// <param name="y">The initial value for the y-component of the matrix.</param>
        /// <param name="z">The initial value for the z-component of the matrix.</param>
        public Matrix3x3(Vector3 x, Vector3 y, Vector3 z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        /// <summary>Gets or sets the value of the component at the specified index for the current instance.</summary>
        /// <param name="index">The index of the component to get or set.</param>
        public unsafe Vector3 this[int index]
        {
            get
            {
                if ((uint)(index) > 2)
                {
                    throw new IndexOutOfRangeException();
                }
                return Unsafe.Add(ref _x, index);
            }

            set
            {
                if ((uint)(index) > 2)
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
                return (X.X * Y.Y * Z.Z) +
                       (X.Y * Y.Z * Z.X) +
                       (X.Z * Y.X * Z.Y) -
                       (X.Z * Y.Y * Z.X) -
                       (X.Y * Y.X * Z.Z) -
                       (X.X * Y.Z * Z.Y);
            }
        }

        /// <summary>Gets or sets the value of the x-component for the current instance.</summary>
        public Vector3 X
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
        public Vector3 Y
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
        public Vector3 Z
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

        /// <summary>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine equality.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Matrix3x3 left, Matrix3x3 right)
        {
            return (left._x == right._x) &&
                   (left._y == right._y) &&
                   (left._z == right._z);
        }

        /// <summary>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine equality.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Matrix3x3 left, Matrix3x3 right) => !(left == right);

        /// <summary>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <(Matrix3x3 left, Matrix3x3 right) => left.Determinant < right.Determinant;

        /// <summary>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Matrix3x3 left, Matrix3x3 right) => left.Determinant <= right.Determinant;

        /// <summary>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >(Matrix3x3 left, Matrix3x3 right) => left.Determinant > right.Determinant;

        /// <summary>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine sort order.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Matrix3x3 left, Matrix3x3 right) => left.Determinant >= right.Determinant;

        /// <summary>Negates a <see cref="Matrix3x3"/> instance to determine its additive inverse.</summary>
        /// <param name="right">The matrix to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Matrix3x3 operator -(Matrix3x3 right) => right * (-1.0f);

        /// <summary>Adds two <see cref="Matrix3x3"/> instances to determine their sum.</summary>
        /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Matrix3x3 operator +(Matrix3x3 left, Matrix3x3 right)
        {
            return new Matrix3x3(left._x + right._x,
                                 left._y + right._y,
                                 left._z + right._z);
        }

        /// <summary>Subtracts two <see cref="Matrix3x3"/> instances to determine their difference.</summary>
        /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Matrix3x3 operator -(Matrix3x3 left, Matrix3x3 right) => left + (-right);

        /// <summary>Multiplies two <see cref="Matrix3x3"/> instances to determine their product.</summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix3x3 operator *(Matrix3x3 left, Matrix3x3 right)
        {
            var x = new Vector3((left.X.X * right.X.X) + (left.X.Y * right.Y.X) + (left.X.Z * right.Z.X),
                                (left.X.X * right.X.Y) + (left.X.Y * right.Y.Y) + (left.X.Z * right.Z.Y),
                                (left.X.X * right.X.Z) + (left.X.Y * right.Y.Z) + (left.X.Z * right.Z.Z));

            var y = new Vector3((left.Y.X * right.X.X) + (left.Y.Y * right.Y.X) + (left.Y.Z * right.Z.X),
                                (left.Y.X * right.X.Y) + (left.Y.Y * right.Y.Y) + (left.Y.Z * right.Z.Y),
                                (left.Y.X * right.X.Z) + (left.Y.Y * right.Y.Z) + (left.Y.Z * right.Z.Z));

            var z = new Vector3((left.Z.X * right.X.X) + (left.Z.Y * right.Y.X) + (left.Z.Z * right.Z.X),
                                (left.Z.X * right.X.Y) + (left.Z.Y * right.Y.Y) + (left.Z.Z * right.Z.Y),
                                (left.Z.X * right.X.Z) + (left.Z.Y * right.Y.Z) + (left.Z.Z * right.Z.Z));

            return new Matrix3x3(x, y, z);
        }

        /// <summary>Multiples a <see cref="Matrix3x3"/> and <see cref="float"/> instance to determine their product.</summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix3x3 operator *(Matrix3x3 left, float right)
        {
            return new Matrix3x3(left._x * right,
                                 left._y * right,
                                 left._z * right);
        }

        /// <summary>Divides a <see cref="Matrix3x3"/> and <see cref="float"/> instance to determine their quotient.</summary>
        /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Matrix3x3 operator /(Matrix3x3 left, float right) => left * (1.0f / right);

        /// <summary>Performs a comparison between two <see cref="Matrix3x3"/> instances to determine equality.</summary>
        /// <param name="left">The matrix to compare to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool Equals(Matrix3x3 left, Matrix3x3 right) => left == right;

        /// <summary>Compares two <see cref="Matrix3x3"/> instances to determine sort order.</summary>
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
        public static int Compare(Matrix3x3 left, Matrix3x3 right) => left.Determinant.CompareTo(right.Determinant);

        /// <summary>Negates a <see cref="Matrix3x3"/> instance to determine its additive inverse.</summary>
        /// <param name="right">The matrix to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Matrix3x3 Negate(Matrix3x3 right) => -right;

        /// <summary>Adds two <see cref="Matrix3x3"/> instances to determine their sum.</summary>
        /// <param name="left">The matrix to add to <paramref name="right"/>.</param>
        /// <param name="right">The matrix to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Matrix3x3 Add(Matrix3x3 left, Matrix3x3 right) => left + right;

        /// <summary>Subtracts two <see cref="Matrix3x3"/> instances to determine their difference.</summary>
        /// <param name="left">The matrix to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The matrix to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Matrix3x3 Subtract(Matrix3x3 left, Matrix3x3 right) => left - right;

        /// <summary>Multiplies two <see cref="Matrix3x3"/> instances to determine their product.</summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix3x3 Multiply(Matrix3x3 left, Matrix3x3 right) => left * right;

        /// <summary>Multiples a <see cref="Matrix3x3"/> and <see cref="float"/> instance to determine their product.</summary>
        /// <param name="left">The matrix to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Matrix3x3 Multiply(Matrix3x3 left, float right) => left * right;

        /// <summary>Divides a <see cref="Matrix3x3"/> and <see cref="float"/> instance to determine their quotient.</summary>
        /// <param name="left">The matrix to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Matrix3x3 Divide(Matrix3x3 left, float right) => left / right;

        /// <summary>Determines the transpose of a <see cref="Matrix3x3"/> instance.</summary>
        /// <param name="right">The matrix to transpose.</param>
        /// <returns>The transpose of <paramref name="right"/>.</returns>
        public static Matrix3x3 Transpose(Matrix3x3 right)
        {
            var x = new Vector3(right._x.X, right._y.X, right._z.X);
            var y = new Vector3(right._x.Y, right._y.Y, right._z.Y);
            var z = new Vector3(right._x.Z, right._y.Z, right._z.Z);

            return new Matrix3x3(x, y, z);
        }

        public static Matrix3x3 CreateFrom(Quaternion rotation)
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

            return new Matrix3x3(new Vector3(1.0f - (2.0f * (yy + zz)), 2.0f * (xy + wz), 2.0f * (xz - wy)),
                                 new Vector3(2.0f * (xy - wz), 1.0f - (2.0f * (zz + xx)), 2.0f * (yz + wx)),
                                 new Vector3(2.0f * (xz + wy), 2.0f * (yz - wx), 1.0f - (2.0f * (yy + xx))));
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
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not an instance of <see cref="Matrix3x3"/>.</exception>
        public int CompareTo(object? obj)
        {
            if (obj is Matrix3x3 other)
            {
                return CompareTo(other);
            }
            return (obj is null) ? 1 : throw new ArgumentException($"{nameof(obj)} is not an instance of {nameof(Matrix3x3)}", nameof(obj));
        }

        /// <summary>Compares a <see cref="Matrix3x3"/> to the current instance to determine sort order.</summary>
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
        public int CompareTo(Matrix3x3 other) => Compare(this, other);

        /// <summary>Compares a <see cref="object"/> to the current instance to determine equality.</summary>
        /// <param name="obj">The object to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Matrix3x3"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj) => (obj is Matrix3x3 other) && Equals(other);

        /// <summary>Compares a <see cref="Matrix3x3"/> to the current instance to determine equality.</summary>
        /// <param name="other">The matrix to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Matrix3x3 other) => Equals(this, other);

        /// <summary>Generates a hash code for the value of the current instance.</summary>
        /// <returns>A hash code for the value of the current instance.</returns>
        public override int GetHashCode() => Determinant.GetHashCode();

        /// <summary>Converts the current instance into a string that represents its value.</summary>
        /// <returns>A <see cref="string"/> that represents the value of the current instance.</returns>
        public override string ToString() => $"[{_x}, {_y}, {_z}]";
    }
}
