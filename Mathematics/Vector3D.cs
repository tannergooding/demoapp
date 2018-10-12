using System;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Mathematics
{
    /// <summary>
    ///     <para>Represents a three-dimensional geometric vector.</para>
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 16, Size = 12)]
    public struct Vector3D : IComparable, IComparable<Vector3D>, IEquatable<Vector3D>
    {
        #region Default Instances
            /// <summary>
            ///     <para>Represents a <see cref="Mathematics.Vector3D"/> whose components are all set to zero.</para>
            /// </summary>
            public static readonly Vector3D Zero = new Vector3D(0.0f, 0.0f, 0.0f);

            /// <summary>
            ///     <para>Represents a <see cref="Mathematics.Vector3D"/> whose x-component is set to one and whose remaining components are all set to zero.</para>
            /// </summary>
            public static readonly Vector3D UnitX = new Vector3D(1.0f, 0.0f, 0.0f);

            /// <summary>
            ///     <para>Represents a <see cref="Mathematics.Vector3D"/> whose y-component is set to one and whose remaining components are all set to zero.</para>
            /// </summary>
            public static readonly Vector3D UnitY = new Vector3D(0.0f, 1.0f, 0.0f);

            /// <summary>
            ///     <para>Represents a <see cref="Mathematics.Vector3D"/> whose z-component is set to one and whose remaining components are all set to zero.</para>
            /// </summary>
            public static readonly Vector3D UnitZ = new Vector3D(0.0f, 0.0f, 1.0f);

            /// <summary>
            ///     <para>Represents a <see cref="Mathematics.Vector3D"/> whose components are all set to one.</para>
            /// </summary>
            public static readonly Vector3D Unit = new Vector3D(1.0f, 1.0f, 1.0f);
        #endregion

        #region Fields
            private float x;
            private float y;
            private float z;
        #endregion

        #region Constructors
            /// <summary>
            ///     <para>Initializes a new instance of the <see cref="Mathematics.Vector3D"/> struct.</para>
            /// </summary>
            /// <param name="x">The initial value for the x-component of the vector.</param>
            /// <param name="y">The initial value for the y-component of the vector.</param>
            /// <param name="z">The initial value for the z-component of the vector.</param>
            public Vector3D(float x, float y, float z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }
        #endregion

        #region Operators
            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Vector3D"/> instances to determine equality.</para>
            /// </summary>
            /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
            /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
            public static bool operator ==(Vector3D left, Vector3D right)
            {
                return (left.x == right.x) &&
                       (left.y == right.y) &&
                       (left.z == right.z);
            }


            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Vector3D"/> instances to determine equality.</para>
            /// </summary>
            /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
            /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are not equal; otherwise, <c>false</c>.</returns>
            public static bool operator !=(Vector3D left, Vector3D right)
            {
                return !(left == right);
            }

            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Vector3D"/> instances to determine sort order.</para>
            /// </summary>
            /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
            /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
            public static bool operator <(Vector3D left, Vector3D right)
            {
                return (left.LengthSquared < right.LengthSquared);
            }


            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Vector3D"/> instances to determine sort order.</para>
            /// </summary>
            /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
            /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
            public static bool operator <=(Vector3D left, Vector3D right)
            {
                return (left.LengthSquared <= right.LengthSquared);
            }

            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Vector3D"/> instances to determine sort order.</para>
            /// </summary>
            /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
            /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
            public static bool operator >(Vector3D left, Vector3D right)
            {
                return (left.LengthSquared > right.LengthSquared);
            }

            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Vector3D"/> instances to determine sort order.</para>
            /// </summary>
            /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
            /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
            public static bool operator >=(Vector3D left, Vector3D right)
            {
                return (left.LengthSquared >= right.LengthSquared);
            }

            /// <summary>
            ///     <para>Negates a <see cref="Mathematics.Vector3D"/> instance to determine its additive inverse.</para>
            /// </summary>
            /// <param name="right">The vector to negate.</param>
            /// <returns>The additive inverse of <paramref name="right"/>.</returns>
            public static Vector3D operator -(Vector3D right)
            {
                return (right * (-1.0f));
            }

            /// <summary>
            ///     <para>Adds two <see cref="Mathematics.Vector3D"/> instances to determine their sum.</para>
            /// </summary>
            /// <param name="left">The vector to add to <paramref name="right"/>.</param>
            /// <param name="right">The vector to add to <paramref name="left"/>.</param>
            /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
            public static Vector3D operator +(Vector3D left, Vector3D right)
            {
                return new Vector3D((left.x + right.x),
                                    (left.y + right.y),
                                    (left.z + right.z));
            }

            /// <summary>
            ///     <para>Subtracts two <see cref="Mathematics.Vector3D"/> instances to determine their difference.</para>
            /// </summary>
            /// <param name="left">The vector to subtract <paramref name="right"/> from.</param>
            /// <param name="right">The vector to subtract from <paramref name="left"/>.</param>
            /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
            public static Vector3D operator -(Vector3D left, Vector3D right)
            {
                return (left + (-right));
            }

            /// <summary>
            ///     <para>Multiples a <see cref="Mathematics.Vector3D"/> and <see cref="Mathematics.Matrix3x3"/> instance to determine their product.</para>
            /// </summary>
            /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
            /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
            /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
            public static Vector3D operator *(Vector3D left, Matrix3x3 right)
            {
                return new Vector3D((left.X * right.X.X) + (left.Y * right.Y.X) + (left.Z * right.Z.X),
                                    (left.X * right.X.Y) + (left.Y * right.Y.Y) + (left.Z * right.Z.Y),
                                    (left.X * right.X.Z) + (left.Y * right.Y.Z) + (left.Z * right.Z.Z));
            }


            /// <summary>
            ///     <para>Multiples a <see cref="Mathematics.Vector3D"/> and <see cref="System.Single"/> instance to determine their product.</para>
            /// </summary>
            /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
            /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
            /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
            public static Vector3D operator *(Vector3D left, float right)
            {
                return new Vector3D((left.x * right),
                                    (left.y * right),
                                    (left.z * right));
            }

            /// <summary>
            ///     <para>Divides a <see cref="Mathematics.Vector3D"/> and <see cref="System.Single"/> instance to determine their quotient.</para>
            /// </summary>
            /// <param name="left">The vector to be divided by <paramref name="right"/>.</param>
            /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
            /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
            public static Vector3D operator /(Vector3D left, float right)
            {
                return (left * (1.0f / right));
            }
        #endregion

        #region Friendly Operators
            /// <summary>
            ///     <para>Performs a comparison between two <see cref="Mathematics.Vector3D"/> instances to determine equality.</para>
            /// </summary>
            /// <param name="left">The vector to compare to <paramref name="right"/>.</param>
            /// <param name="right">The vector to compare to <paramref name="left"/>.</param>
            /// <returns><c>true</c> if <paramref name="left"/> and <paramref name="right"/> are equal; otherwise, <c>false</c>.</returns>
            public static bool Equals(Vector3D left, Vector3D right)
            {
                return (left == right);
            }

            /// <summary>
            ///     <para>Compares two <see cref="Mathematics.Vector3D"/> instances to determine sort order.</para>
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
            public static int Compare(Vector3D left, Vector3D right)
            {
                return (int)(left.LengthSquared - right.LengthSquared);
            }

            /// <summary>
            ///     <para>Negates a <see cref="Mathematics.Vector3D"/> instance to determine its additive inverse.</para>
            /// </summary>
            /// <param name="right">The vector to negate.</param>
            /// <returns>The additive inverse of <paramref name="right"/>.</returns>
            public static Vector3D Negate(Vector3D right)
            {
                return -right;
            }

            /// <summary>
            ///     <para>Adds two <see cref="Mathematics.Vector3D"/> instances to determine their sum.</para>
            /// </summary>
            /// <param name="left">The vector to add to <paramref name="right"/>.</param>
            /// <param name="right">The vector to add to <paramref name="left"/>.</param>
            /// <returns>The sum of <paramref name="right"/> added to <paramref name="left"/>.</returns>
            public static Vector3D Add(Vector3D left, Vector3D right)
            {
                return (left + right);
            }

            /// <summary>
            ///     <para>Subtracts two <see cref="Mathematics.Vector3D"/> instances to determine their difference.</para>
            /// </summary>
            /// <param name="left">The vector to subtract <paramref name="right"/> from.</param>
            /// <param name="right">The vector to subtract from <paramref name="left"/>.</param>
            /// <returns>The difference of <paramref name="right"/> subtracted from <paramref name="right"/>.</returns>
            public static Vector3D Subtract(Vector3D left, Vector3D right)
            {
                return (left - right);
            }

            /// <summary>
            ///     <para>Multiples a <see cref="Mathematics.Vector3D"/> and <see cref="Mathematics.Matrix3x3"/> instance to determine their product.</para>
            /// </summary>
            /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
            /// <param name="right">The matrix to multiply <paramref name="left"/> by.</param>
            /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
            public static Vector3D Multiply(Vector3D left, Matrix3x3 right)
            {
                return (left * right);
            }

            /// <summary>
            ///     <para>Multiples a <see cref="Mathematics.Vector3D"/> and <see cref="System.Single"/> instance to determine their product.</para>
            /// </summary>
            /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
            /// <param name="right">The scalar to multiply <paramref name="left"/> by.</param>
            /// <returns>The product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
            public static Vector3D Multiply(Vector3D left, float right)
            {
                return (left * right);
            }

            /// <summary>
            ///     <para>Divides a <see cref="Mathematics.Vector3D"/> and <see cref="System.Single"/> instance to determine their quotient.</para>
            /// </summary>
            /// <param name="left">The vector to be divided by <paramref name="right"/>.</param>
            /// <param name="right">The scalar to divide <paramref name="left"/> by.</param>
            /// <returns>The quotient of <paramref name="left"/> divided by <paramref name="right"/>.</returns>
            public static Vector3D Divide(Vector3D left, float right)
            {
                return (left / right);
            }

            /// <summary>
            ///     <para>Multiples two <see cref="Mathematics.Vector3D"/> instances to determine their scalar-product.</para>
            /// </summary>
            /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
            /// <param name="right">The vector to multiply <paramref name="left"/> by.</param>
            /// <returns>The scalar-product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
            public static float DotProduct(Vector3D left, Vector3D right)
            {
                return (left.x * right.x) +
                       (left.y * right.y) +
                       (left.z * right.z);
            }

            /// <summary>
            ///     <para>Multiples two <see cref="Mathematics.Vector3D"/> instances to determine their vector-product.</para>
            /// </summary>
            /// <param name="left">The vector to be multiplied by <paramref name="right"/>.</param>
            /// <param name="right">The vector to multiply <paramref name="left"/> by.</param>
            /// <returns>The vector-product of <paramref name="left"/> multiplied by <paramref name="right"/>.</returns>
            public static Vector3D CrossProduct(Vector3D left, Vector3D right)
            {
                return new Vector3D(((left.y * right.z) - (left.z * right.y)),
                                    ((left.z * right.x) - (left.x * right.z)),
                                    ((left.x * right.y) - (left.y * right.x)));
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
            /// <exception cref="ArgumentException"><paramref name="obj"/> is not an instance of <see cref="Mathematics.Vector3D"/>.</exception>
            public int CompareTo(object obj)
            {
                if ((obj is Vector3D) == false)
                {
                    throw new ArgumentException("obj is not an instance of Mathematics.Vector3D", "obj");
                }
                Contract.EndContractBlock();

                return this.CompareTo((Vector3D)(obj));
            }

            /// <summary>
            ///     <para>Compares a <see cref="Mathematics.Vector3D"/> to the current instance to determine sort order.</para>
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
            public int CompareTo(Vector3D other)
            {
                return Vector3D.Compare(this, other);
            }

            /// <summary>
            ///     <para>Compares a <see cref="System.Object"/> to the current instance to determine equality.</para>
            /// </summary>
            /// <param name="obj">The object to compare to the current instance.</param>
            /// <returns><c>true</c> if <paramref name="obj"/> is a <see cref="Mathematics.Vector3D"/> and is equal to the current instance; otherwise, <c>false</c>.</returns>
            public override bool Equals(object obj)
            {
                return (obj is Vector3D) &&
                       this.Equals((Vector3D)(obj));
            }

            /// <summary>
            ///     <para>Compares a <see cref="Mathematics.Vector3D"/> to the current instance to determine equality.</para>
            /// </summary>
            /// <param name="other">The vector to compare to the current instance.</param>
            /// <returns><c>true</c> if <paramref name="other"/> is equal to the current instance; otherwise, <c>false</c>.</returns>
            public bool Equals(Vector3D other)
            {
                return Vector3D.Equals(this, other);
            }
        
            /// <summary>
            ///     <para>Generates a hash code for the value of the current instance.</para>
            /// </summary>
            /// <returns>A hash code for the value of the current instance.</returns>
            public override int GetHashCode()
            {
                return this.LengthSquared.GetHashCode();
            }

            /// <summary>
            ///     <para>Converts the current instance into a string that represents its value.</para>
            /// </summary>
            /// <returns>A <see cref="System.String"/> that represents the value of the current instance.</returns>
            public override string ToString()
            {
                return string.Format(CultureInfo.CurrentCulture.NumberFormat, "[{0} {1} {2}]", this.x, this.y, this.z);
            }
        #endregion

        #region Properties
            /// <summary>
            ///     <para>Gets or sets the value of the component at the specified index for the current instance.</para>
            /// </summary>
            /// <param name="index">The index of the component to get or set.</param>
            unsafe public float this[int index]
            {
                get
                {
                    if ((index < 0) || (index > 2))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    Contract.EndContractBlock();

                    fixed (Vector3D* pVector = &this)
                    {
                        return ((float*)(pVector))[index];
                    }
                }

                set
                {
                    if ((index < 0) || (index > 2))
                    {
                        throw new IndexOutOfRangeException();
                    }
                    Contract.EndContractBlock();

                    fixed (Vector3D* pVector = &this)
                    {
                        ((float*)(pVector))[index] = value;
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
            public float Y
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
            public float Z
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
            ///     <para>Gets the length for the values of the current instance.</para>
            /// </summary>
            public float Length
            {
                get
                {
                    return (float)Math.Sqrt(this.LengthSquared);
                }
            }

            /// <summary>
            ///     <para>Gets the length-squared for the values of the current instance.</para>
            /// </summary>
            public float LengthSquared
            {
                get
                {
                    return Vector3D.DotProduct(this, this);
                }
            }
        #endregion
    }
}
