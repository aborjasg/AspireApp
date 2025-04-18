using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace csmatio.types
{
	/// <summary>
	/// This class represents Matlab's Structure object (structure array).
	/// 
	/// Note: An array of structures can contain only structures of the same type,
	/// meaning structures that have the same field names.
	/// </summary>
	/// <author>David Zier (david.zier@gmail.com)</author>
	public class MLStructure : MLArray
	{
		/// <summary>
		/// A Hashtable that keeps structure field names
		/// </summary>
		readonly List<string> _keys;

		/// <summary>
		/// Array of structures
		/// </summary>
		readonly List<Dictionary<string,MLArray>> _mlStructArray;

		/// <summary>
		/// Current structure pointer for bulk insert
		/// </summary>
		int _currentIndex;

		/// <summary>
		/// Create an <c>MLStructure</c> class object.
		/// </summary>
		/// <param name="Name">The name of the <c>MLStructure</c></param>
		/// <param name="Dims">The array dimensions of the <c>MLStructure</c></param>
		public MLStructure( string Name, int[] Dims ) :
			this( Name, Dims, mxSTRUCT_CLASS, 0 ){}

		/// <summary>
		/// Create a new <c>MLStructure</c> class object.
		/// </summary>
		/// <param name="Name">The name of the structure</param>
		/// <param name="Dims">The dimensions of the structure</param>
		/// <param name="Type">The Matlab array type</param>
		/// <param name="Attributes">The array attributes</param>
		public MLStructure( string Name, int[] Dims, int Type, int Attributes ) :
			base( Name, Dims, Type, Attributes )
		{
            _mlStructArray = new List<Dictionary<string, MLArray>>(Dims[0] * Dims[1]);
            _keys = new List<string>();
		}

		/// <summary>
		/// Public accessor to the field desribed by <c>Name</c> from the current structure.
		/// </summary>
		public MLArray this[ string Name ]
		{
			set => this[Name, _currentIndex ] = value;
			get => this[Name, _currentIndex];
		}

		/// <summary>
		/// Public accessor to the field described by <c>Name</c> from the (m,n)'th structure
		/// in the structure array.
		/// </summary>
		public MLArray this[ string Name, int M, int N ]
		{
			set => this[Name, GetIndex( M, N ) ] = value;
			get => this[ Name, GetIndex( M, N ) ];
		}

		/// <summary>
		/// Public accessor to the field described by <c>Name</c> from the index'th structure
		/// in the structure array.
		/// </summary>
		public MLArray this[ string Name, int Index ]
		{
			set
			{
				if( !_keys.Contains( Name ) )
				{
					_keys.Add( Name );
				}
				_currentIndex = Index;

				if( _mlStructArray.Count == 0 || _mlStructArray.Count <= Index )
				{
					_mlStructArray.Insert( Index, new Dictionary<string,MLArray>() );
				}
				_mlStructArray[Index].Add(Name,value);
			}
			get => _mlStructArray[Index][Name];
		}

		/// <summary>
		/// Gets the field names
		/// </summary>
		public List<string> Keys => _keys;

		/// <summary>
		/// Gets the maximum length of field descriptor
		/// </summary>
		public int MaxFieldLength
		{
			get
			{
				var maxLen = 0;
				foreach( var s in _keys )
				{
					maxLen = s.Length > maxLen ? s.Length : maxLen;
				}
				return maxLen+1;
			}
		}

		/// <summary>
		/// Dumps field names to byte array.  Field names are written as Zero End Strings.
		/// </summary>
		/// <returns>A <c>byte</c> array for all the field names</returns>
		public byte[] GetKeySetToByteArray()
		{
			var memstrm = new MemoryStream();
			var bw = new BinaryWriter(memstrm);
			var buffer = new char[ MaxFieldLength ];

			try
			{
				foreach( var s in _keys )
				{
					for( var i = 0; i < buffer.Length; i++ ) buffer[i] = (char)0;
					Array.Copy( s.ToCharArray(), 0, buffer, 0, s.Length );
					bw.Write( buffer );
				}
			}
			catch( IOException e )
			{
				Console.WriteLine("Could not write Structure key set to byte array: " + e );
				return new byte[0];
			}
			
			return memstrm.ToArray();
		}

		/// <summary>
		/// Gets all the fields from the struct array as a flat list of fields.
		/// </summary>
		public List<MLArray> AllFields
		{
			get
			{
				var fields = new List<MLArray>();

				foreach( var st in _mlStructArray )
					fields.AddRange( st.Values );

				return fields;
			}
		}

		/// <summary>
		/// Get a string representation for the content of the array.
		/// See <see cref="csmatio.types.MLArray.ContentToString()"/>
		/// </summary>
		/// <returns>A string representation.</returns>
		public override string ContentToString()
		{
			var sb = new StringBuilder();
			sb.Append( Name + " = \n" );

			if( M*N == 1 )
			{
				foreach( var key in _keys )
				{
					sb.Append("\t" + key + " : " + this[key].ContentToString() + "\n" );
				}
			}
			else
			{
				sb.Append("\n");
				sb.Append( M + "x" + N );
				sb.Append(" struct array with fields: \n");
				foreach( var key in _keys )
				{
					sb.Append("\t" + key + "\n");
				}
			}
			return sb.ToString();
		}

	}
}
