// Copyright © Tanner Gooding and Contributors. Licensed under the MIT License (MIT). See License.md in the repository root for more information.

using System;
using System.Runtime.CompilerServices;

namespace Mathematics;

/// <summary>Represents a two-dimensional geometric vector.</summary>
public struct Vector2 : IComparable, IComparable<Vector2>, IEquatable<Vector2>
{
    /// <summary>Represents a <see cref="Vector2"/> whose components are all set to zero.</summary>
    public static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);

    /// <summary>Represents a <see cref="Vector2"/> whose x-component is set to one and whose remaining components are all set to zero.</summary>
    public static readonly Vector2 UnitX = new Vector2(1.0f, 0.0f);

    /// <summary>Represents a <see cref="Vector2"/> whose y-component is set to one and whose remaining components are all set to zero.</summary>
    public static readonly Vector2 UnitY = new Vector2(0.0f, 1.0f);

    /// <summary>Represents a <see cref="Vector2"/> whose components are all set to one.</summary>
    public static readonly Vector2 One = new Vector2(1.0f, 1.0f);

    private float _x;
    private float _y;

    public Vector2(float value)
    {
        _x = value;
        _y = value;
    }

    /// <summary>Initializes a new instance of the <see cref="Vector2"/> struct.</summary>
    /// <param name="x">The initial value for the x-component of the vector.</param>
    /// <param name="y">The initial value for the y-component of the vector.</param>
    public Vector2(float x, float y)
    {
        _x = x;
        _y = y;
    }

    /// <summary>Gets or sets the value of the component at the specified index for the current instance.</summary>
    /// <param name="index">The index of the component to get or set.</param>
    public unsafe float this[int index]
    {
        get
        {
            return (uint)index > 1 ? throw new IndexOutOfRangeException() : Unsafe.Add(ref _x, index);
        }

        set
        {
            if ((uint)index > 1)
            {
                throw new IndexOutOfRangeException();
            }
            Unsafe.Add(ref _x, index) = value;
        }
    }

    /// <summary>Gets the length for the values of the current instance.</summary>
    public readonly float Length => MathF.Sqrt(LengthSquared);

    /// <summary>Gets the length-squared for the values of the current instance.</summary>
    public readonly float LengthSquared => DotProduct(this, this);

    /// <summary>Gets or sets the value of the x-component for the current instance.</summary>
    public float X
    {
        readonly get
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
        readonly get
        {
            return _y;
        }

        set
        {
            _y = value;
        }
    }

    /// <summary>Performs a comparison between two <see cref="Vector2"/> instances to determine equality.</summary>
    /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
    /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
    /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(Vector2 left, Vector2 right)
    {
        return (left._x == right._x) &&
               (left._y == right._y);
    }

    /// <summary>Performs a comparison between two <see cref="Vector2"/> instances to determine equality.</summary>
    /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
    /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
    /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(Vector2 left, Vector2 right) => !(left == right);

    /// <summary>Performs a comparison between two <see cref="Vector2"/> instances to determine sort order.</summary>
    /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
    /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
    /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
    public static bool operator <(Vector2 left, Vector2 right) => left.LengthSquared < right.LengthSquared;

    /// <summary>Performs a comparison between two <see cref="Vector2"/> instances to determine sort order.</summary>
    /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
    /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
    /// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
    public static bool operator <=(Vector2 left, Vector2 right) => left.LengthSquared <= right.LengthSquared;

    /// <summary>Performs a comparison between two <see cref="Vector2"/> instances to determine sort order.</summary>
    /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
    /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
    /// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
    public static bool operator >(Vector2 left, Vector2 right) => left.LengthSquared > right.LengthSquared;

    /// <summary>Performs a comparison between two <see cref="Vector2"/> instances to determine sort order.</summary>
    /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
    /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
    /// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
    public static bool operator >=(Vector2 left, Vector2 right) => left.LengthSquared >= right.LengthSquared;

    /// <summary>Negates a <see cref="Vector2"/> instance to determine its additive inverse.</summary>
    /// <param name="right">The vector to negate.</param>
    /// <returns>The additive inverse of <paramref name="right"/>.</returns>
    public static Vector2 operator -(Vector2 right) => right * (-1.0f);

    /// <summary>Adds two <see cref="Vector2"/> instances to determine their sum.</summary>
    /// <param name="left">The vector to add to <paramref name="right"/>.</param>
    /// <param name="right">The vector to add to <paramref name="left"/>.</param>
    /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
    public static Vector2 operator +(Vector2 left, Vector2 right)
    {
        return new Vector2(left._x + right._x,
                           left._y + right._y);
    }

    /// <summary>Subtracts two <see cref="Vector2"/> instances to determine their difference.</summary>
    /// <param name="left">The vector to subtract <paramref name="right"/> from.</param>
    /// <param name="right">The vector to subtract from <paramref name="left"/>.</param>
    /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
    public static Vector2 operator -(Vector2 left, Vector2 right) => left + (-right);

    /// <summary>Multiples a <see cref="Vector2"/> and <see cref="float"/> instance to determine their product.</summary>
    /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
    /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
    /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
    public static Vector2 operator *(Vector2 left, float right)
    {
        return new Vector2(left._x * right,
                           left._y * right);
    }

    /// <summary>Divides a <see cref="Vector2"/> and <see cref="float"/> instance to determine their quotient.</summary>
    /// <param name="left">The vector to be divided by <paramref name="right"/>.</param>
    /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
    /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
    public static Vector2 operator /(Vector2 left, float right) => left * (1.0f / right);

    /// <summary>Performs a comparison between two <see cref="Vector2"/> instances to determine equality.</summary>
    /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
    /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
    /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
    public static bool Equals(Vector2 left, Vector2 right) => left == right;

    /// <summary>Compares two <see cref="Vector2"/> instances to determine sort order.</summary>
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
    public static int Compare(Vector2 left, Vector2 right) => left.LengthSquared.CompareTo(right.LengthSquared);

    /// <summary>Negates a <see cref="Vector2"/> instance to determine its additive inverse.</summary>
    /// <param name="right">The vector to negate.</param>
    /// <returns>The additive inverse of <paramref name="right"/>.</returns>
    public static Vector2 Negate(Vector2 right) => -right;

    /// <summary>Adds two <see cref="Vector2"/> instances to determine their sum.</summary>
    /// <param name="left">The vector to add to <paramref name="right"/>.</param>
    /// <param name="right">The vector to add to <paramref name="left"/>.</param>
    /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
    public static Vector2 Add(Vector2 left, Vector2 right) => left + right;

    /// <summary>Subtracts two <see cref="Vector2"/> instances to determine their difference.</summary>
    /// <param name="left">The vector to subtract <paramref name="right"/> from.</param>
    /// <param name="right">The vector to subtract from <paramref name="left"/>.</param>
    /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
    public static Vector2 Subtract(Vector2 left, Vector2 right) => left - right;

    /// <summary>Multiples a <see cref="Vector2"/> and <see cref="float"/> instance to determine their product.</summary>
    /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
    /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
    /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
    public static Vector2 Multiply(Vector2 left, float right) => left * right;

    /// <summary>Divides a <see cref="Vector2"/> and <see cref="float"/> instance to determine their quotient.</summary>
    /// <param name="left">The vector to be divided by <paramref name="right"/>.</param>
    /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
    /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
    public static Vector2 Divide(Vector2 left, float right) => left / right;

    /// <summary>Multiples two <see cref="Vector2"/> instances to determine their scalar-product.</summary>
    /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
    /// <param name="right">The vector to multiply <paramref name="left"/> by.</param>
    /// <returns>The scalar-product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
    public static float DotProduct(Vector2 left, Vector2 right)
    {
        return (left._x * right._x) +
               (left._y * right._y);
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
    /// <exception cref="ArgumentException"><paramref name="obj"/> is not an instance of <see cref="Vector2"/>.</exception>
    public readonly int CompareTo(object? obj)
    {
        return obj is Vector2 other
            ? CompareTo(other)
            : (obj is null) ? 1 : throw new ArgumentException($"{nameof(obj)} is not an instance of {nameof(Vector2)}", nameof(obj));
    }

    /// <summary>Compares a <see cref="Vector2"/> to the current instance to determine sort order.</summary>
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
    public readonly int CompareTo(Vector2 other) => Compare(this, other);

    /// <summary>Compares a <see cref="object"/> to the current instance to determine equality.</summary>
    /// <param name="obj">The object to compare to the current instance.</param>
    /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Vector2"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
    public override readonly bool Equals(object? obj) => (obj is Vector2 other) && Equals(other);

    /// <summary>Compares a <see cref="Vector2"/> to the current instance to determine equality.</summary>
    /// <param name="other">The vector to compare to the current instance.</param>
    /// <returns><c>true</c> if <paramref name="other"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
    public readonly bool Equals(Vector2 other) => Equals(this, other);

    /// <summary>Generates a hash code for the value of the current instance.</summary>
    /// <returns>A hash code for the value of the current instance.</returns>
    public override readonly int GetHashCode() => LengthSquared.GetHashCode();

    public readonly Vector2 Normalize() => this / Length;

    /// <summary>Converts the current instance into a string that represents its value.</summary>
    /// <returns>A <see cref="string"/> that represents the value of the current instance.</returns>
    public override readonly string ToString() => $"[{_x}, {_y}]";

    public readonly Vector2 Transform(Matrix2x2 transformation)
    {
        return new Vector2((_x * transformation.X.X) + (_y * transformation.Y.X),
                           (_x * transformation.X.Y) + (_y * transformation.Y.Y));
    }
}
