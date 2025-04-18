using System;
using csmatio.common;

namespace csmatio.types
{
	/// <summary>
	/// This class represents an UInt16 (ushort) array (matrix)
	/// </summary>
	/// <author>David Zier (david.zier@gmail.com)</author>
	public class MLUInt16 : MLNumericArray<ushort>
	{
		#region Constructors

		/// <summary>
		/// Normally this constructor is used only by <c>MatFileReader</c> and <c>MatFileWriter</c>
		/// </summary>
		/// <param name="Name">Array name</param>
		/// <param name="Dims">Array dimensions</param>
		/// <param name="Type">Array type: here <c>mxUINT16_CLASS</c></param>
		/// <param name="Attributes">Array flags</param>
		public MLUInt16(string Name, int[] Dims, int Type, int Attributes)
			: base(Name, Dims, Type, Attributes) { }

		/// <summary>
		/// Create a <c>MLUInt16</c> array with given name and dimensions.
		/// </summary>
		/// <param name="Name">Array name</param>
		/// <param name="Dims">Array dimensions</param>
		public MLUInt16(string Name, int[] Dims)
			: base(Name, Dims, mxUINT16_CLASS, 0) { }

		/// <summary>
		/// <a href="http://math.nist.gov/javanumerics/jama/">Jama</a> [math.nist.gov] style:
		/// construct a 2D real matrix from a one-dimensional packed array.
		/// </summary>
		/// <param name="Name">Array name</param>
		/// <param name="vals">One-dimensional array of <c>ushort</c>, packed by columns</param>
		/// <param name="m">Number of rows</param>
		public MLUInt16(string Name, ushort[] vals, int m)
			: base(Name, mxUINT16_CLASS, vals, m) { }

		/// <summary>
		/// <a href="http://math.nist.gov/javanumerics/jama/">Jama</a> [math.nist.gov] style:
		/// construct a 2D real matrix from <c>ushort[][]</c>.
		/// </summary>
		/// <remarks>Note: Array is converted to <c>ushort[]</c></remarks>
		/// <param name="Name">Array name</param>
		/// <param name="vals">Two-dimensional array of values</param>
		public MLUInt16(string Name, ushort[][] vals)
			: this(Name, Helpers.Array2DTo1D(vals), vals.Length) { }

		/// <summary>
		/// <a href="http://math.nist.gov/javanumerics/jama/">Jama</a> [math.nist.gov] style:
		/// construct a 2D imaginary matrix from a one-dimensional packed array.
		/// </summary>
		/// <param name="Name">Array name</param>
		/// <param name="Real">One-dimensional array of <c>ushort</c> for <i>real</i> values, packed by columns</param>
		/// <param name="Imag">One-dimensional array of <c>ushort</c> for <i>imaginary</i> values, packed by columns</param>
		/// <param name="M">Number of rows</param>
		public MLUInt16(string Name, ushort[] Real, ushort[] Imag, int M)
			: base(Name, mxUINT16_CLASS, Real, Imag, M) { }


		/// <summary>
		/// <a href="http://math.nist.gov/javanumerics/jama/">Jama</a> [math.nist.gov] style:
		/// construct a 2D imaginary matrix from a one-dimensional packed array.
		/// </summary>
		/// <param name="Name">Array name</param>
		/// <param name="Real">One-dimensional array of <c>ushort</c> for <i>real</i> values, packed by columns</param>
		/// <param name="Imag">One-dimensional array of <c>ushort</c> for <i>imaginary</i> values, packed by columns</param>
		public MLUInt16(string Name, ushort[][] Real, ushort[][] Imag)
			: this(Name, Helpers.Array2DTo1D(Real), Helpers.Array2DTo1D(Imag), Real.Length) { }

		#endregion

		/// <summary>
		/// Builds a numeric object from a byte array.
		/// </summary>
		/// <param name="bytes">A byte array containing the data.</param>
		/// <returns>A numeric object</returns>
		protected override object BuildFromBytes2(byte[] bytes) => BitConverter.ToUInt16(bytes, 0);

		/// <summary>
		/// Gets a byte array from a numeric object.
		/// </summary>
		/// <param name="val">The numeric object to convert into a byte array.</param>
		public override byte[] GetByteArray(object val) => BitConverter.GetBytes((ushort)val);
	}
}
