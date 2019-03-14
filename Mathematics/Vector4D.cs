using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Mathematics
{
    /// <summary>
    ///     <para>Represents a four-dimensional geometric vector.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 16, Size = 16)]
    public struct Vector4D : IComparable, IComparable<Vector4D>, IEquatable<Vector4D>
    {
        #region Default Instances
        /// <summary>
        ///     <para>Represents a <see cref="Vector4D"/> whose components are all set to zero.</para>
        /// </summary>
        public static readonly Vector4D Zero = new Vector4D(0.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>
        ///     <para>Represents a <see cref="Vector4D"/> whose x-component is set to one and whose remaining components are all set to zero.</para>
        /// </summary>
        public static readonly Vector4D UnitX = new Vector4D(1.0f, 0.0f, 0.0f, 0.0f);

        /// <summary>
        ///     <para>Represents a <see cref="Vector4D"/> whose y-component is set to one and whose remaining components are all set to zero.</para>
        /// </summary>
        public static readonly Vector4D UnitY = new Vector4D(0.0f, 1.0f, 0.0f, 0.0f);

        /// <summary>
        ///     <para>Represents a <see cref="Vector4D"/> whose z-component is set to one and whose remaining components are all set to zero.</para>
        /// </summary>
        public static readonly Vector4D UnitZ = new Vector4D(0.0f, 0.0f, 1.0f, 0.0f);

        /// <summary>
        ///     <para>Represents a <see cref="Vector4D"/> whose w-component is set to one and whose remaining components are all set to zero.</para>
        /// </summary>
        public static readonly Vector4D UnitW = new Vector4D(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>
        ///     <para>Represents a <see cref="Vector4D"/> whose components are all set to one.</para>
        /// </summary>
        public static readonly Vector4D Unit = new Vector4D(1.0f, 1.0f, 1.0f, 1.0f);
        #endregion

        #region Fields
            private float _x;
            private float _y;
            private float _z;
            private float _w;
        #endregion

        #region Constructors
        /// <summary>
        ///     <para>Initializes a new instance of the <see cref="Vector4D"/> struct.</para>
        /// </summary>
        /// <param name="x">The initial value for the x-component of the vector.</param>
        /// <param name="y">The initial value for the y-component of the vector.</param>
        /// <param name="z">The initial value for the z-component of the vector.</param>
        /// <param name="w">The initial value for the w-component of the vector.</param>
        public Vector4D(float x, float y, float z, float w)
            {
                _x = x;
                _y = y;
                _z = z;
                _w = w;
            }
        #endregion

        #region Operators
        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Vector4D"/> instances to determine equality.</para>
        /// </summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(Vector4D left, Vector4D right)
            {
                return (left._x == right._x) &&
                       (left._y == right._y) &&
                       (left._z == right._z) &&
                       (left._w == right._w);
            }


        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Vector4D"/> instances to determine equality.</para>
        /// </summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(Vector4D left, Vector4D right)
            {
                return !(left == right);
            }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Vector4D"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <(Vector4D left, Vector4D right)
            {
                return left.LengthSquared < right.LengthSquared;
            }


        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Vector4D"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator <=(Vector4D left, Vector4D right)
            {
                return left.LengthSquared <= right.LengthSquared;
            }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Vector4D"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >(Vector4D left, Vector4D right)
            {
                return left.LengthSquared > right.LengthSquared;
            }

        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Vector4D"/> instances to determine sort order.</para>
        /// </summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
        public static bool operator >=(Vector4D left, Vector4D right)
            {
                return left.LengthSquared >= right.LengthSquared;
            }

        /// <summary>
        ///     <para>Negates a <see cref="Vector4D"/> instance to determine its additive inverse.</para>
        /// </summary>
        /// <param name="right">The vector to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Vector4D operator -(Vector4D right)
            {
                return right * (-1.0f);
            }

        /// <summary>
        ///     <para>Adds two <see cref="Vector4D"/> instances to determine their sum.</para>
        /// </summary>
        /// <param name="left">The vector to add to <paramref name="right"/>.</param>
        /// <param name="right">The vector to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Vector4D operator +(Vector4D left, Vector4D right)
            {
                return new Vector4D(left._x + right._x,
                                    left._y + right._y,
                                    left._z + right._z,
                                    left._w + right._w);
            }

        /// <summary>
        ///     <para>Subtracts two <see cref="Vector4D"/> instances to determine their difference.</para>
        /// </summary>
        /// <param name="left">The vector to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The vector to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Vector4D operator -(Vector4D left, Vector4D right)
            {
                return left + (-right);
            }

        /// <summary>
        ///     <para>Multiples a <see cref="Vector4D"/> and <see cref="Matrix4x4"/> instance to determine their product.</para>
        /// </summary>
        /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Vector4D operator *(Vector4D left, Matrix4x4 right)
            {
                return new Vector4D((left.X * right.X.X) + (left.Y * right.Y.X) + (left.Z * right.Z.X) + (left.W * right.W.X),
                                    (left.X * right.X.Y) + (left.Y * right.Y.Y) + (left.Z * right.Z.Y) + (left.W * right.W.Y),
                                    (left.X * right.X.Z) + (left.Y * right.Y.Z) + (left.Z * right.Z.Z) + (left.W * right.W.Z),
                                    (left.X * right.X.W) + (left.Y * right.Y.W) + (left.Z * right.Z.W) + (left.W * right.W.W));
            }

        /// <summary>
        ///     <para>Multiples a <see cref="Vector4D"/> and <see cref="float"/> instance to determine their product.</para>
        /// </summary>
        /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Vector4D operator *(Vector4D left, float right)
            {
                return new Vector4D(left._x * right,
                                    left._y * right,
                                    left._z * right,
                                    left._w * right);
            }

        /// <summary>
        ///     <para>Divides a <see cref="Vector4D"/> and <see cref="float"/> instance to determine their quotient.</para>
        /// </summary>
        /// <param name="left">The vector to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Vector4D operator /(Vector4D left, float right)
            {
                return left * (1.0f / right);
            }
        #endregion

        #region Friendly Operators
        /// <summary>
        ///     <para>Performs a comparison between two <see cref="Vector4D"/> instances to determine equality.</para>
        /// </summary>
        /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
        /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool Equals(Vector4D left, Vector4D right)
            {
                return left == right;
            }

        /// <summary>
        ///     <para>Compares two <see cref="Vector4D"/> instances to determine sort order.</para>
        /// </summary>
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
        public static int Compare(Vector4D left, Vector4D right)
            {
                return (int)(left.LengthSquared - right.LengthSquared);
            }

        /// <summary>
        ///     <para>Negates a <see cref="Vector4D"/> instance to determine its additive inverse.</para>
        /// </summary>
        /// <param name="right">The vector to negate.</param>
        /// <returns>The additive inverse of <paramref name="right"/>.</returns>
        public static Vector4D Negate(Vector4D right)
            {
                return -right;
            }

        /// <summary>
        ///     <para>Adds two <see cref="Vector4D"/> instances to determine their sum.</para>
        /// </summary>
        /// <param name="left">The vector to add to <paramref name="right"/>.</param>
        /// <param name="right">The vector to add to <paramref name="left"/>.</param>
        /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
        public static Vector4D Add(Vector4D left, Vector4D right)
            {
                return left + right;
            }

        /// <summary>
        ///     <para>Subtracts two <see cref="Vector4D"/> instances to determine their difference.</para>
        /// </summary>
        /// <param name="left">The vector to subtract <paramref name="right"/> from.</param>
        /// <param name="right">The vector to subtract from <paramref name="left"/>.</param>
        /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
        public static Vector4D Subtract(Vector4D left, Vector4D right)
            {
                return left - right;
            }

        /// <summary>
        ///     <para>Multiples a <see cref="Vector4D"/> and <see cref="Matrix4x4"/> instance to determine their product.</para>
        /// </summary>
        /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Vector4D Multiply(Vector4D left, Matrix4x4 right)
            {
                return left * right;
            }

        /// <summary>
        ///     <para>Multiples a <see cref="Vector4D"/> and <see cref="float"/> instance to determine their product.</para>
        /// </summary>
        /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
        /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static Vector4D Multiply(Vector4D left, float right)
            {
                return left * right;
            }

        /// <summary>
        ///     <para>Divides a <see cref="Vector4D"/> and <see cref="float"/> instance to determine their quotient.</para>
        /// </summary>
        /// <param name="left">The vector to be divided by <paramref name="right"/>.</param>
        /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
        /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
        public static Vector4D Divide(Vector4D left, float right)
            {
                return left / right;
            }

        /// <summary>
        ///     <para>Multiples two <see cref="Vector4D"/> instances to determine their scalar-product.</para>
        /// </summary>
        /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
        /// <param name="right">The vector to multiply <paramref name="left"/> by.</param>
        /// <returns>The scalar-product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
        public static float DotProduct(Vector4D left, Vector4D right)
            {
                return (left._x * right._x) +
                       (left._y * right._y) +
                       (left._z * right._z);
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
        /// <exception cref="ArgumentException"><paramref name="obj"/> is not an instance of <see cref="Vector4D"/>.</exception>
        public int CompareTo(object obj)
            {
                if ((obj is Vector4D) == false)
                {
                    throw new ArgumentException("obj is not an instance of Mathematics.Vector4D", "obj");
                }
                Contract.EndContractBlock();

                return CompareTo((Vector4D)obj);
            }

        /// <summary>
        ///     <para>Compares a <see cref="Vector4D"/> to the current instance to determine sort order.</para>
        /// </summary>
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
        public int CompareTo(Vector4D other)
            {
                return Compare(this, other);
            }

        /// <summary>
        ///     <para>Compares a <see cref="object"/> to the current instance to determine equality.</para>
        /// </summary>
        /// <param name="obj">The object to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Vector4D"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
            {
                return (obj is Vector4D) &&
                       Equals((Vector4D)obj);
            }

        /// <summary>
        ///     <para>Compares a <see cref="Vector4D"/> to the current instance to determine equality.</para>
        /// </summary>
        /// <param name="other">The vector to compare to the current instance.</param>
        /// <returns><c>true</c> if <paramref name="other"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
        public bool Equals(Vector4D other)
            {
                return Equals(this, other);
            }
        
            /// <summary>
            ///     <para>Generates a hash code for the value of the current instance.</para>
            /// </summary>
            /// <returns>A hash code for the value of the current instance.</returns>
            public override int GetHashCode()
            {
                return LengthSquared.GetHashCode();
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
            public unsafe float this[int index]
            {
                get
                {
                    if ((index < 0) || (index > 3))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    Contract.EndContractBlock();

                    fixed (Vector4D* pVector = &this)
                    {
                        return ((float*)pVector)[index];
                    }
                }

                set
                {
                    if ((index < 0) || (index > 3))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    Contract.EndContractBlock();

                    fixed (Vector4D* pVector = &this)
                    {
                        ((float*)pVector)[index] = value;
                    }
                }
            }

            /// <summary>
            ///     <para>Gets or sets the value of the x-component for the current instance.</para>
            /// </summary>
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

            /// <summary>
            ///     <para>Gets or sets the value of the y-component for the current instance.</para>
            /// </summary>
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

            /// <summary>
            ///     <para>Gets or sets the value of the z-component for the current instance.</para>
            /// </summary>
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

            /// <summary>
            ///     <para>Gets or sets the value of the w-component for the current instance.</para>
            /// </summary>
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

            /// <summary>
            ///     <para>Gets the length for the values of the current instance.</para>
            /// </summary>
            public float Length
            {
                get
                {
                    return (float)Math.Sqrt(LengthSquared);
                }
            }

            /// <summary>
            ///     <para>Gets the length-squared for the values of the current instance.</para>
            /// </summary>
            public float LengthSquared
            {
                get
                {
                    return DotProduct(this, this);
                }
            }
        #endregion
    }
}
