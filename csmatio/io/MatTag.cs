using csmatio.common;

namespace csmatio.io
{
	/// <summary>
	/// Used to create a MAT-file style tag
	/// </summary>
	/// <author>David Zier (david.zier@gmail.com)</author>
	public class MatTag
	{
		/// <summary>
		/// The array type for this tag.
		/// </summary>
		protected int _type=0;
		/// <summary>
		/// The size of the tag.
		/// </summary>
		protected int _size=0;

		/// <summary>
		/// Create a new MAT-file tag object.
		/// </summary>
		/// <param name="Type">The type of the array for this tag</param>
		/// <param name="Size">The size of the tag</param>
		public MatTag( int Type, int Size )
		{
			_type = Type;
			_size = Size;
		}

		/// <summary>
		/// Calculate the padding for the element.
		/// </summary>
		/// <param name="size">The size of the element.</param>
		/// <param name="compressed">Is the tag compressed?</param>
		/// <returns>The number of padding bytes</returns>
		protected int GetPadding( int size, bool compressed )
		{
			int padding = 0;
			if (SizeOf() != 0)// Added by Gyomei
			{
				// data not packed in the tag
				if (!compressed)
				{
					int b;
					padding = (b = ((size / SizeOf() % (8 / SizeOf())) * SizeOf())) != 0 ? 8 - b : 0;
				}
				else
				{
					int b;
					padding = (b = (((size / SizeOf()) % (4 / SizeOf())) * SizeOf())) != 0 ? 4 - b : 0;
				}
			}
			return padding;
		}

		/// <summary>
		/// <see cref="object.ToString()"/>
		/// </summary>
		public override string ToString()
		{
			string s;
			s = "[tag: " + MatDataTypes.TypeToString(_type) + " size: " + _size + "]";
			return s;
		}

		/// <summary>
		/// Get size of single data in this tag.
		/// </summary>
		/// <returns>The number of bytes for single data</returns>
		public int SizeOf() => MatDataTypes.SizeOf( _type );

		/// <summary>
        /// Get the type of the MatTag
		/// add set by Gyomei 2022/6/23
        /// </summary>
        public int Type
		{
			get { return _type; }
			set { _type = value; }
		}

		/// <summary>
		/// Get the number of bytes for the MAT-Data object
		/// add set by Gyomei 2022/6/23
		/// </summary>
		public int Size
		{
			get { return _size; }
			set { _size = value; }
		}
	}
}
