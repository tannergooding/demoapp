// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;

namespace Mathematics
{
    /// <summary>Represents a four-dimensional geometric vector.</summary>
    public struct Vector4 : IComparable, IComparable<Vector4>, IEquatable<Vector4>
    {
        /// <summary>Represents a <see cref="Vector4"/> whose components are all set to zero.</summary>
        public static readonly Vector4 Zero = new Vector4(0.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>Represents a <see cref="Vector4"/> whose x-component is set to one and whose remaining components are all set to zero.</summary>
        public static readonly Vector4 UnitX = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>Represents a <see cref="Vector4"/> whose y-component is set to one and whose remaining components are all set to zero.</summary>
        public static readonly Vector4 UnitY = new Vector4(0.0f, 1.0f, 0.0f, 0.0f);

        /// <summary>Represents a <see cref="Vector4"/> whose z-component is set to one and whose remaining components are all set to zero.</summary>
        public static readonly Vector4 UnitZ = new Vector4(0.0f, 0.0f, 1.0f, 0.0f);

        /// <summary>Represents a <see cref="Vector4"/> whose w-component is set to one and whose remaining components are all set to zero.</summary>
        public static readonly Vector4 UnitW = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>Represents a <see cref="Vector4"/> whose components are all set to one.</summary>
        public static readonly Vector4 One = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);

        private float _x;
        private float _y;
        private float _z;
        private float _w;

        public Vector4(float value)
        {
            _x = value;
            _y = value;
            _z = value;
            _w = value;
        }

        public Vector4(Vector3 value, float w)
        {
            _x = value.X;
            _y = value.Y;
            _z = value.Z;
            _w = w;
        }

        /// <summary>Initializes a new instance of the <see cref="Vector4"/> struct.</summary>
        /// <param name="x">The initial value for the x-component of the vector.</param>
        /// <param name="y">The initial value for the y-component of the vector.</param>
        /// <param name="z">The initial value for the z-component of the vector.</param>
        /// <param name="w">The initial value for the w-component of the vector.</param>
        public Vector4(float x, float y, float z, float w)
        {
            _x = x;
            _y = y;
            _z = z;
            _w = w;
        }

        /// <summary>Gets or sets the value of the component at the specified index for the current instance.</summary>
        /// <param name="index">The index of the component to get or set.</param>
        public unsafe float this[int index]
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

        /// <summary>Gets the length for the values of the current instance.</summary>
        public float Length => MathF.Sqrt(LengthSquared);

        /// <summary>Gets the length-squared for the values of the current instance.</summary>
        public float LengthSquared => DotProduct(this, this);

        /// <summary>Gets or sets the value of the x-component for the current instance.</summary>
        public float X
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
        public float Y
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
        public float Z
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
        public float W
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

        /// <summary>Performs a comparison between two <see cref="Vector4"/> instances to determine equality.</summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector4 left, Vector4 right)
        {
            return (left._x == right._x) &&
                   (left._y == right._y) &&
                   (left._z == right._z) &&
                   (left._w == right._w);
        }

        /// <summary>Performs a comparison between two <see cref="Vector4"/> instances to determine equality.</summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector4 left, Vector4 right) => !(left == right);

        /// <summary>Performs a comparison between two <see cref="Vector4"/> instances to determine sort order.</summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <(Vector4 left, Vector4 right) => left.LengthSquared < right.LengthSquared;

        /// <summary>Performs a comparison between two <see cref="Vector4"/> instances to determine sort order.</summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Vector4 left, Vector4 right) => left.LengthSquared <= right.LengthSquared;

        /// <summary>Performs a comparison between two <see cref="Vector4"/> instances to determine sort order.</summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >(Vector4 left, Vector4 right) => left.LengthSquared > right.LengthSquared;

        /// <summary>Performs a comparison between two <see cref="Vector4"/> instances to determine sort order.</summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Vector4 left, Vector4 right) => left.LengthSquared >= right.LengthSquared;

        /// <summary>Negates a <see cref="Vector4"/> instance to determine its additive inverse.</summary>
        /// <param name="right">The vector to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Vector4 operator -(Vector4 right) => right * (-1.0f);

        /// <summary>Adds two <see cref="Vector4"/> instances to determine their sum.</summary>
        /// <param name="left">The vector to add to <paramref name="right"/>.</param>
        /// <param name="right">The vector to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Vector4 operator +(Vector4 left, Vector4 right)
        {
            return new Vector4(left._x + right._x,
                               left._y + right._y,
                               left._z + right._z,
                               left._w + right._w);
        }

        /// <summary>Subtracts two <see cref="Vector4"/> instances to determine their difference.</summary>
        /// <param name="left">The vector to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The vector to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Vector4 operator -(Vector4 left, Vector4 right) => left + (-right);

        /// <summary>Multiples a <see cref="Vector4"/> and <see cref="float"/> instance to determine their product.</summary>
        /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Vector4 operator *(Vector4 left, float right)
        {
            return new Vector4(left._x * right,
                               left._y * right,
                               left._z * right,
                               left._w * right);
        }

        /// <summary>Divides a <see cref="Vector4"/> and <see cref="float"/> instance to determine their quotient.</summary>
        /// <param name="left">The vector to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Vector4 operator /(Vector4 left, float right) => left * (1.0f / right);

        /// <summary>Performs a comparison between two <see cref="Vector4"/> instances to determine equality.</summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool Equals(Vector4 left, Vector4 right) => left == right;

        /// <summary>Compares two <see cref="Vector4"/> instances to determine sort order.</summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
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
        public static int Compare(Vector4 left, Vector4 right) => left.LengthSquared.CompareTo(right.LengthSquared);

        /// <summary>Negates a <see cref="Vector4"/> instance to determine its additive inverse.</summary>
        /// <param name="right">The vector to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Vector4 Negate(Vector4 right) => -right;

        /// <summary>Adds two <see cref="Vector4"/> instances to determine their sum.</summary>
        /// <param name="left">The vector to add to <paramref name="right"/>.</param>
        /// <param name="right">The vector to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Vector4 Add(Vector4 left, Vector4 right) => left + right;

        /// <summary>Subtracts two <see cref="Vector4"/> instances to determine their difference.</summary>
        /// <param name="left">The vector to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The vector to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Vector4 Subtract(Vector4 left, Vector4 right) => left - right;

        /// <summary>Multiples a <see cref="Vector4"/> and <see cref="float"/> instance to determine their product.</summary>
        /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Vector4 Multiply(Vector4 left, float right) => left * right;

        /// <summary>Divides a <see cref="Vector4"/> and <see cref="float"/> instance to determine their quotient.</summary>
        /// <param name="left">The vector to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Vector4 Divide(Vector4 left, float right) => left / right;

        /// <summary>Multiples two <see cref="Vector4"/> instances to determine their scalar-product.</summary>
        /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The vector to multiply <paramref name="left"/> by.</param>
        /// <returns>The scalar-product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static float DotProduct(Vector4 left, Vector4 right)
        {
            return (left._x * right._x) +
                   (left._y * right._y) +
                   (left._z * right._z);
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
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not an instance of <see cref="Vector4"/>.</exception>
        public int CompareTo(object? obj)
        {
            if (obj is Vector4 other)
            {
                return CompareTo(other);
            }
            return (obj is null) ? 1 : throw new ArgumentException($"{nameof(obj)} is not an instance of {nameof(Vector4)}", nameof(obj));
        }

        /// <summary>Compares a <see cref="Vector4"/> to the current instance to determine sort order.</summary>
        /// <param name="other">The vector to compare to the current instance.</param>
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
        public int CompareTo(Vector4 other) => Compare(this, other);

        /// <summary>Compares a <see cref="object"/> to the current instance to determine equality.</summary>
        /// <param name="obj">The object to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Vector4"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object? obj) => (obj is Vector4 other) && Equals(other);

        /// <summary>Compares a <see cref="Vector4"/> to the current instance to determine equality.</summary>
        /// <param name="other">The vector to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Vector4 other) => Equals(this, other);

        /// <summary>Generates a hash code for the value of the current instance.</summary>
        /// <returns>A hash code for the value of the current instance.</returns>
        public override int GetHashCode() => LengthSquared.GetHashCode();

        public Vector4 Normalize() => this / Length;

        /// <summary>Converts the current instance into a string that represents its value.</summary>
        /// <returns>A <see cref="string"/> that represents the value of the current instance.</returns>
        public override string ToString() => $"[{_x}, {_y}, {_z}, {_w}]";


        public Vector4 Transform(Matrix4x4 transformation)
        {
            return new Vector4((_x * transformation.X.X) + (_y * transformation.Y.X) + (_z * transformation.Z.X) + (_w * transformation.W.X),
                               (_x * transformation.X.Y) + (_y * transformation.Y.Y) + (_z * transformation.Z.Y) + (_w * transformation.W.Y),
                               (_x * transformation.X.Z) + (_y * transformation.Y.Z) + (_z * transformation.Z.Z) + (_w * transformation.W.Z),
                               (_x * transformation.X.W) + (_y * transformation.Y.W) + (_z * transformation.Z.W) + (_w * transformation.W.W));
        }

        public Vector4 Transform(Quaternion rotation)
        {
            var x2 = rotation.X + rotation.X;
            var y2 = rotation.Y + rotation.Y;
            var z2 = rotation.Z + rotation.Z;

            var wx2 = rotation.W * x2;
            var wy2 = rotation.W * y2;
            var wz2 = rotation.W * z2;
            var xx2 = rotation.X * x2;
            var xy2 = rotation.X * y2;
            var xz2 = rotation.X * z2;
            var yy2 = rotation.Y * y2;
            var yz2 = rotation.Y * z2;
            var zz2 = rotation.Z * z2;

            return new Vector4((_x * (1.0f - yy2 - zz2)) + (_y * (xy2 - wz2)) + (_z * (xz2 + wy2)),
                               (_x * (xy2 + wz2)) + (_y * (1.0f - xx2 - zz2)) + (_z * (yz2 - wx2)),
                               (_x * (xz2 - wy2)) + (_y * (yz2 + wx2)) + (_z * (1.0f - xx2 - yy2)),
                               _w);
        }
    }
}
