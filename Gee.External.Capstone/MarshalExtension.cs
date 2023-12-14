using System;
using System.Runtime.InteropServices;

namespace Gee.External.Capstone {
    /// <summary>
    ///     Marshal Extension.
    /// </summary>
    internal static class MarshalExtension {
        /// <summary>
        ///     Allocate Memory For a Structure.
        /// </summary>
        /// <typeparam name="T">
        ///     The structure's type.
        /// </typeparam>
        /// <returns>
        ///     A pointer to the allocated memory.
        /// </returns>
        internal static IntPtr AllocHGlobal<T>()
        {
            var nType = MarshalExtension.SizeOf<T>();
            var pType = Marshal.AllocHGlobal(nType);

            return pType;
        }

        /// <summary>
        ///     Allocate Memory For a Structure.
        /// </summary>
        /// <param name="size">
        ///     The collection's size.
        /// </param>
        /// <typeparam name="T">
        ///     The structure's type.
        /// </typeparam>
        /// <returns>
        ///     A pointer to the allocated memory.
        /// </returns>
        internal static IntPtr AllocHGlobal<T>(int size)
        {
            var nType = MarshalExtension.SizeOf<T>() * size;
            var pType = Marshal.AllocHGlobal(nType);

            return pType;
        }

        /// <summary>
        ///     Marshal a Pointer to a Structure and Free Memory.
        /// </summary>
        /// <typeparam name="T">
        ///     The destination structure's type.
        /// </typeparam>
        /// <param name="p">
        ///     The pointer to marshal.
        /// </param>
        /// <returns>
        ///     The destination structure.
        /// </returns>
        internal static T FreePtrToStructure<T>(IntPtr p)
        {
            var @struct = MarshalExtension.PtrToStructure<T>(p);
            Marshal.FreeHGlobal(p);

            return @struct;
        }

        /// <summary>
        ///     Returns the field offset of the unmanaged form of the managed class.
        /// </summary>
        /// <typeparam name="T">
        ///     A value type or formatted reference type that specifies the managed class. You
        ///     must apply the System.Runtime.InteropServices.StructLayoutAttribute to the class.
        /// </typeparam>
        /// <param name="fieldName">
        ///     The field within the t parameter.
        /// </param>
        /// <returns>
        ///     The offset, in bytes, for the fieldName parameter within the specified class
        ///     that is declared by platform invoke.
        /// </returns>
        public static IntPtr OffsetOf<T>(string fieldName)
        {
#if NETSTANDARD
            return Marshal.OffsetOf<T>(fieldName);
#else
            return Marshal.OffsetOf(typeof(T), fieldName);
#endif
        }

        /// <summary>
        ///     Marshal a Pointer to a Structure.
        /// </summary>
        /// <typeparam name="T">
        ///     The destination structure's type.
        /// </typeparam>
        /// <param name="p">
        ///     The pointer to marshal.
        /// </param>
        /// <returns>
        ///     The destination structure.
        /// </returns>
        internal static T PtrToStructure<T>(IntPtr p) {
#if NETSTANDARD
            return Marshal.PtrToStructure<T>(p);
#else
            var @struct = Marshal.PtrToStructure(p, typeof(T));
            return (T) @struct;
#endif
        }

        /// <summary>
        ///     Marshal a Pointer to a Collection of Structures.
        /// </summary>
        /// <typeparam name="T">
        ///     The collection's type.
        /// </typeparam>
        /// <param name="p">
        ///     A pointer to a collection. The pointer should be initialized to the collection's starting address.
        /// </param>
        /// <param name="size">
        ///     The collection's size.
        /// </param>
        /// <returns>
        ///     The destination collection.
        /// </returns>
        internal static T[] PtrToStructure<T>(IntPtr p, int size)
        {
            var array = new T[size];
            var index = p;
            for (var i = 0; i < size; i++) {
                var element = MarshalExtension.PtrToStructure<T>(index);
                array[i] = element;

                index += MarshalExtension.SizeOf<T>();
            }

            return array;
        }

        /// <summary>
        ///     Get a Type's Size.
        /// </summary>
        /// <typeparam name="T">
        ///     The type.
        /// </typeparam>
        /// <returns>
        ///     The type's size, in bytes.
        /// </returns>
        internal static int SizeOf<T>() {
#if NETSTANDARD
            return Marshal.SizeOf<T>();
#else
            var size = Marshal.SizeOf(typeof(T));
            return size;
#endif
        }
    }
}