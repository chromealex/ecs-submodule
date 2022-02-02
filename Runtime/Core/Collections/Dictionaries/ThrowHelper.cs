namespace ME.ECS.Collections {
    // This file defines an internal class used to throw exceptions in BCL code.
    // The main purpose is to reduce code size. 
    // 
    // The old way to throw an exception generates quite a lot IL code and assembly code.
    // Following is an example:
    //     C# source
    //          throw new ArgumentNullException("key", Environment.GetResourceString("ArgumentNull_Key"));
    //     IL code:
    //          IL_0003:  ldstr      "key"
    //          IL_0008:  ldstr      "ArgumentNull_Key"
    //          IL_000d:  call       string System.Environment::GetResourceString(string)
    //          IL_0012:  newobj     instance void System.ArgumentNullException::.ctor(string,string)
    //          IL_0017:  throw
    //    which is 21bytes in IL.
    // 
    // So we want to get rid of the ldstr and call to Environment.GetResource in IL.
    // In order to do that, I created two enums: ExceptionResource, ExceptionArgument to represent the
    // argument name and resource name in a small integer. The source code will be changed to 
    //    ThrowHelper.ThrowArgumentNullException(ExceptionArgument.key, ExceptionResource.ArgumentNull_Key);
    //
    // The IL code will be 7 bytes.
    //    IL_0008:  ldc.i4.4
    //    IL_0009:  ldc.i4.4
    //    IL_000a:  call       void System.ThrowHelper::ThrowArgumentNullException(valuetype System.ExceptionArgument)
    //    IL_000f:  ldarg.0
    //
    // This will also reduce the Jitted code size a lot. 
    //
    // It is very important we do this for generic classes because we can easily generate the same code 
    // multiple times for different instantiation. 
    // 
    // <
 
 
 
 
 
 
 
    
    using System;
    using System.Runtime.CompilerServices;        
    using System.Runtime.Serialization;
    using System.Diagnostics.Contracts;
 
    [Pure]
    internal static class ThrowHelper {    
        internal static void ThrowArgumentOutOfRangeException() {        
            ThrowArgumentOutOfRangeException(ExceptionArgument.index, ExceptionResource.ArgumentOutOfRange_Index);            
        }
 
        internal static void ThrowWrongKeyTypeArgumentException(object key, Type targetType) {
            throw new ArgumentException();
        }
 
        internal static void ThrowWrongValueTypeArgumentException(object value, Type targetType) {
            throw new ArgumentException();
        }
 
        internal static void ThrowKeyNotFoundException() {
            throw new System.Collections.Generic.KeyNotFoundException();
        }
        
        internal static void ThrowArgumentException(ExceptionResource resource) {
            throw new ArgumentException();
        }
 
        internal static void ThrowArgumentException(ExceptionResource resource, ExceptionArgument argument) {
            throw new ArgumentException();
        }
 
        internal static void ThrowArgumentNullException(ExceptionArgument argument) {
            throw new ArgumentNullException(GetArgumentName(argument));
        }
 
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument) {
            throw new ArgumentOutOfRangeException(GetArgumentName(argument));
        }
 
        internal static void ThrowArgumentOutOfRangeException(ExceptionArgument argument, ExceptionResource resource) {
                
            throw new ArgumentOutOfRangeException(GetArgumentName(argument));         
        }
 
        internal static void ThrowInvalidOperationException(ExceptionResource resource) {
            throw new InvalidOperationException();
        }
 
        internal static void ThrowSerializationException(ExceptionResource resource) {
            throw new SerializationException();
        }
 
        internal static void  ThrowSecurityException(ExceptionResource resource) {
            throw new System.Security.SecurityException();            
        }
 
        internal static void ThrowNotSupportedException(ExceptionResource resource) {
            throw new NotSupportedException();
        }
 
        internal static void ThrowUnauthorizedAccessException(ExceptionResource resource) {
            throw new UnauthorizedAccessException();
        }
 
        internal static void ThrowObjectDisposedException(string objectName, ExceptionResource resource) {
            throw new ObjectDisposedException(objectName);
        }
 
        // Allow nulls for reference types and Nullable<U>, but not for value types.
        internal static void IfNullAndNullsAreIllegalThenThrow<T>(object value, ExceptionArgument argName) {
            // Note that default(T) is not equal to null for value types except when T is Nullable<U>. 
            if (value == null && !(default(T) == null))
                ThrowHelper.ThrowArgumentNullException(argName);
        }
 
        //
        // This function will convert an ExceptionArgument enum value to the argument name string.
        //
        internal static string GetArgumentName(ExceptionArgument argument) {
            string argumentName = null;
 
            switch (argument) {
                case ExceptionArgument.array:
                    argumentName = "array";
                    break;
 
                case ExceptionArgument.arrayIndex:
                    argumentName = "arrayIndex";
                    break;
 
                case ExceptionArgument.capacity:
                    argumentName = "capacity";
                    break;
 
                case ExceptionArgument.collection:
                    argumentName = "collection";
                    break;
 
                case ExceptionArgument.list:
                    argumentName = "list";
                    break;
 
                case ExceptionArgument.converter:
                    argumentName = "converter";
                    break;
 
                case ExceptionArgument.count:
                    argumentName = "count";
                    break;
 
                case ExceptionArgument.dictionary:
                    argumentName = "dictionary";
                    break;
 
                case ExceptionArgument.dictionaryCreationThreshold:
                    argumentName = "dictionaryCreationThreshold";
                    break;
 
                case ExceptionArgument.index:
                    argumentName = "index";
                    break;
 
                case ExceptionArgument.info:
                    argumentName = "info";
                    break;
 
                case ExceptionArgument.key:
                    argumentName = "key";
                    break;
 
                case ExceptionArgument.match:
                    argumentName = "match";
                    break;
 
                case ExceptionArgument.obj:
                    argumentName = "obj";
                    break;
 
                case ExceptionArgument.queue:
                    argumentName = "queue";
                    break;
 
                case ExceptionArgument.stack:
                    argumentName = "stack";
                    break;
 
                case ExceptionArgument.startIndex:
                    argumentName = "startIndex";
                    break;
 
                case ExceptionArgument.value:
                    argumentName = "value";
                    break;
 
                case ExceptionArgument.name:
                    argumentName = "name";
                    break;
 
                case ExceptionArgument.mode:
                    argumentName = "mode";
                    break;
 
                case ExceptionArgument.item:
                    argumentName = "item";
                    break;
 
                case ExceptionArgument.options:
                    argumentName = "options";
                    break;
 
                case ExceptionArgument.view:
                    argumentName = "view";
                    break;
 
               case ExceptionArgument.sourceBytesToCopy:
                    argumentName = "sourceBytesToCopy";
                    break;
 
                default:
                    Contract.Assert(false, "The enum value is not defined, please checked ExceptionArgumentName Enum.");
                    return string.Empty;
            }
 
            return argumentName;
        }
 
        //
        // This function will convert an ExceptionResource enum value to the resource string.
        //
        internal static string GetResourceName(ExceptionResource resource) {
            string resourceName = null;
 
            switch (resource) {
                case ExceptionResource.Argument_ImplementIComparable:
                    resourceName = "Argument_ImplementIComparable";
                    break;
 
                case ExceptionResource.Argument_AddingDuplicate:
                    resourceName = "Argument_AddingDuplicate";
                    break;
 
                case ExceptionResource.ArgumentOutOfRange_BiggerThanCollection:
                    resourceName = "ArgumentOutOfRange_BiggerThanCollection";
                    break;
 
                case ExceptionResource.ArgumentOutOfRange_Count:
                    resourceName = "ArgumentOutOfRange_Count";
                    break;
 
                case ExceptionResource.ArgumentOutOfRange_Index:
                    resourceName = "ArgumentOutOfRange_Index";
                    break;
 
                case ExceptionResource.ArgumentOutOfRange_InvalidThreshold:
                    resourceName = "ArgumentOutOfRange_InvalidThreshold";
                    break;
 
                case ExceptionResource.ArgumentOutOfRange_ListInsert:
                    resourceName = "ArgumentOutOfRange_ListInsert";
                    break;
 
                case ExceptionResource.ArgumentOutOfRange_NeedNonNegNum:
                    resourceName = "ArgumentOutOfRange_NeedNonNegNum";
                    break;
 
                case ExceptionResource.ArgumentOutOfRange_SmallCapacity:
                    resourceName = "ArgumentOutOfRange_SmallCapacity";
                    break;
 
                case ExceptionResource.Arg_ArrayPlusOffTooSmall:
                    resourceName = "Arg_ArrayPlusOffTooSmall";
                    break;
 
                case ExceptionResource.Arg_RankMultiDimNotSupported:
                    resourceName = "Arg_RankMultiDimNotSupported";
                    break;
 
                case ExceptionResource.Arg_NonZeroLowerBound:
                    resourceName = "Arg_NonZeroLowerBound";
                    break;
 
                case ExceptionResource.Argument_InvalidArrayType:
                    resourceName = "Argument_InvalidArrayType";
                    break;
 
                case ExceptionResource.Argument_InvalidOffLen:
                    resourceName = "Argument_InvalidOffLen";
                    break;
 
                case ExceptionResource.Argument_ItemNotExist:
                    resourceName = "Argument_ItemNotExist";
                    break;                    
 
                case ExceptionResource.InvalidOperation_CannotRemoveFromStackOrQueue:
                    resourceName = "InvalidOperation_CannotRemoveFromStackOrQueue";
                    break;
 
                case ExceptionResource.InvalidOperation_EmptyQueue:
                    resourceName = "InvalidOperation_EmptyQueue";
                    break;
 
                case ExceptionResource.InvalidOperation_EnumOpCantHappen:
                    resourceName = "InvalidOperation_EnumOpCantHappen";
                    break;
 
                case ExceptionResource.InvalidOperation_EnumFailedVersion:
                    resourceName = "InvalidOperation_EnumFailedVersion";
                    break;
 
                case ExceptionResource.InvalidOperation_EmptyStack:
                    resourceName = "InvalidOperation_EmptyStack";
                    break;
 
                case ExceptionResource.InvalidOperation_EnumNotStarted:
                    resourceName = "InvalidOperation_EnumNotStarted";
                    break;
 
                case ExceptionResource.InvalidOperation_EnumEnded:
                    resourceName = "InvalidOperation_EnumEnded";
                    break;
 
                case ExceptionResource.NotSupported_KeyCollectionSet:
                    resourceName = "NotSupported_KeyCollectionSet";
                    break;
 
                case ExceptionResource.NotSupported_ReadOnlyCollection:
                    resourceName = "NotSupported_ReadOnlyCollection";
                    break;
 
                case ExceptionResource.NotSupported_ValueCollectionSet:
                    resourceName = "NotSupported_ValueCollectionSet";
                    break;
 
 
                case ExceptionResource.NotSupported_SortedListNestedWrite:
                    resourceName = "NotSupported_SortedListNestedWrite";
                    break;
 
 
                case ExceptionResource.Serialization_InvalidOnDeser:
                    resourceName = "Serialization_InvalidOnDeser";
                    break;
 
                case ExceptionResource.Serialization_MissingKeys:
                    resourceName = "Serialization_MissingKeys";
                    break;
 
                case ExceptionResource.Serialization_NullKey:
                    resourceName = "Serialization_NullKey";
                    break;
 
                case ExceptionResource.Argument_InvalidType:
                    resourceName = "Argument_InvalidType";
                    break;
 
                case ExceptionResource.Argument_InvalidArgumentForComparison:
                    resourceName = "Argument_InvalidArgumentForComparison";                    
                    break;
 
                case ExceptionResource.InvalidOperation_NoValue:
                    resourceName = "InvalidOperation_NoValue";                    
                    break;
 
                case ExceptionResource.InvalidOperation_RegRemoveSubKey:
                    resourceName = "InvalidOperation_RegRemoveSubKey";                    
                    break;
 
                case ExceptionResource.Arg_RegSubKeyAbsent:
                    resourceName = "Arg_RegSubKeyAbsent";                    
                    break;
 
                case ExceptionResource.Arg_RegSubKeyValueAbsent:
                    resourceName = "Arg_RegSubKeyValueAbsent";                    
                    break;
                    
                case ExceptionResource.Arg_RegKeyDelHive:
                    resourceName = "Arg_RegKeyDelHive";                    
                    break;
 
                case ExceptionResource.Security_RegistryPermission:
                    resourceName = "Security_RegistryPermission";                    
                    break;
 
                case ExceptionResource.Arg_RegSetStrArrNull:
                    resourceName = "Arg_RegSetStrArrNull";                    
                    break;
 
                case ExceptionResource.Arg_RegSetMismatchedKind:
                    resourceName = "Arg_RegSetMismatchedKind";                    
                    break;
 
                case ExceptionResource.UnauthorizedAccess_RegistryNoWrite:
                    resourceName = "UnauthorizedAccess_RegistryNoWrite";
                    break;
 
                case ExceptionResource.ObjectDisposed_RegKeyClosed:
                    resourceName = "ObjectDisposed_RegKeyClosed";
                    break;
 
                case ExceptionResource.Arg_RegKeyStrLenBug:
                    resourceName = "Arg_RegKeyStrLenBug";
                    break;
 
                case ExceptionResource.Argument_InvalidRegistryKeyPermissionCheck:
                    resourceName = "Argument_InvalidRegistryKeyPermissionCheck";
                    break;
 
                case ExceptionResource.NotSupported_InComparableType:
                    resourceName = "NotSupported_InComparableType";
                    break;
 
                case ExceptionResource.Argument_InvalidRegistryOptionsCheck:
                    resourceName = "Argument_InvalidRegistryOptionsCheck";
                    break;
 
                case ExceptionResource.Argument_InvalidRegistryViewCheck:
                    resourceName = "Argument_InvalidRegistryViewCheck";
                    break;
 
                default:
                    Contract.Assert( false, "The enum value is not defined, please checked ExceptionArgumentName Enum.");
                    return string.Empty;
            }
 
            return resourceName;
        }
 
    }
 
    //
    // The convention for this enum is using the argument name as the enum name
    // 
    internal enum ExceptionArgument {
        obj,
        dictionary,
        dictionaryCreationThreshold,
        array,
        info,
        key,
        collection,
        list,
        match,
        converter,
        queue,
        stack,
        capacity,
        index,
        startIndex,
        value,
        count,
        arrayIndex,
        name,
        mode,
        item,
        options,
        view,
        sourceBytesToCopy,
    }
 
    //
    // The convention for this enum is using the resource name as the enum name
    // 
    internal enum ExceptionResource {
        Argument_ImplementIComparable,
        Argument_InvalidType,     
        Argument_InvalidArgumentForComparison,
        Argument_InvalidRegistryKeyPermissionCheck,        
        ArgumentOutOfRange_NeedNonNegNum,
        
        Arg_ArrayPlusOffTooSmall,
        Arg_NonZeroLowerBound,        
        Arg_RankMultiDimNotSupported,        
        Arg_RegKeyDelHive,
        Arg_RegKeyStrLenBug,  
        Arg_RegSetStrArrNull,
        Arg_RegSetMismatchedKind,
        Arg_RegSubKeyAbsent,        
        Arg_RegSubKeyValueAbsent,
        
        Argument_AddingDuplicate,
        Serialization_InvalidOnDeser,
        Serialization_MissingKeys,
        Serialization_NullKey,
        Argument_InvalidArrayType,
        NotSupported_KeyCollectionSet,
        NotSupported_ValueCollectionSet,
        ArgumentOutOfRange_SmallCapacity,
        ArgumentOutOfRange_Index,
        Argument_InvalidOffLen,
        Argument_ItemNotExist,
        ArgumentOutOfRange_Count,
        ArgumentOutOfRange_InvalidThreshold,
        ArgumentOutOfRange_ListInsert,
        NotSupported_ReadOnlyCollection,
        InvalidOperation_CannotRemoveFromStackOrQueue,
        InvalidOperation_EmptyQueue,
        InvalidOperation_EnumOpCantHappen,
        InvalidOperation_EnumFailedVersion,
        InvalidOperation_EmptyStack,
        ArgumentOutOfRange_BiggerThanCollection,
        InvalidOperation_EnumNotStarted,
        InvalidOperation_EnumEnded,
        NotSupported_SortedListNestedWrite,
        InvalidOperation_NoValue,
        InvalidOperation_RegRemoveSubKey,
        Security_RegistryPermission,
        UnauthorizedAccess_RegistryNoWrite,
        ObjectDisposed_RegKeyClosed,
        NotSupported_InComparableType,
        Argument_InvalidRegistryOptionsCheck,
        Argument_InvalidRegistryViewCheck
    }
    
}

namespace System.Collections.Generic {

    internal static class HashHelpers {

        #if FEATURE_RANDOMIZED_STRING_HASHING
    public const int HashCollisionThreshold = 100;
    public static bool s_UseRandomizedStringHashing = String.UseRandomizedHashing();
        #endif

        internal const Int32 HashPrime = 101;
        // Table of prime numbers to use as hash table sizes. 
        // A typical resize algorithm would pick the smallest prime number in this array
        // that is larger than twice the previous capacity. 
        // Suppose our Hashtable currently has capacity x and enough elements are added 
        // such that a resize needs to occur. Resizing first computes 2x then finds the 
        // first prime in the table greater than 2x, i.e. if primes are ordered 
        // p_1, p_2, ..., p_i, ..., it finds p_n such that p_n-1 < 2x < p_n. 
        // Doubling is important for preserving the asymptotic complexity of the 
        // hashtable operations such as add.  Having a prime guarantees that double 
        // hashing does not lead to infinite loops.  IE, your hash function will be 
        // h1(key) + i*h2(key), 0 <= i < size.  h2 and the size must be relatively prime.
        public static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363, 156437,
            187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403, 968897, 1162687, 1395263,
            1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559, 5999471, 7199369
        };

        public static bool IsPrime(int candidate) {
            if ((candidate & 1) != 0) {
                int limit = (int)Math.Sqrt(candidate);
                for (int divisor = 3; divisor <= limit; divisor += 2) {
                    if ((candidate % divisor) == 0) return false;
                }

                return true;
            }

            return (candidate == 2);
        }

        public static int GetPrime(int min) {
            if (min < 0) throw new ArgumentException();
            System.Diagnostics.Contracts.Contract.EndContractBlock();

            for (int i = 0; i < HashHelpers.primes.Length; i++) {
                int prime = HashHelpers.primes[i];
                if (prime >= min) return prime;
            }

            //outside of our predefined table. 
            //compute the hard way. 
            for (int i = (min | 1); i < Int32.MaxValue; i += 2) {
                if (HashHelpers.IsPrime(i) && ((i - 1) % HashHelpers.HashPrime != 0)) return i;
            }

            return min;
        }

        public static int GetMinPrime() {
            return HashHelpers.primes[0];
        }

        // Returns size of hashtable to grow to.
        public static int ExpandPrime(int oldSize) {
            int newSize = 2 * oldSize;

            // Allow the hashtables to grow to maximum possible size (~2G elements) before encoutering capacity overflow.
            // Note that this check works even when _items.Length overflowed thanks to the (TKey) cast
            if ((uint)newSize > HashHelpers.MaxPrimeArrayLength && HashHelpers.MaxPrimeArrayLength > oldSize) {
                System.Diagnostics.Contracts.Contract.Assert(HashHelpers.MaxPrimeArrayLength == HashHelpers.GetPrime(HashHelpers.MaxPrimeArrayLength), "Invalid MaxPrimeArrayLength");
                return HashHelpers.MaxPrimeArrayLength;
            }

            return HashHelpers.GetPrime(newSize);
        }


        // This is the maximum prime smaller than Array.MaxArrayLength
        public const int MaxPrimeArrayLength = 0x7FEFFFFD;

        #if FEATURE_RANDOMIZED_STRING_HASHING
    public static bool IsWellKnownEqualityComparer(object comparer)
    {
        return (comparer == null || comparer == System.Collections.Generic.EqualityComparer<string>.Default || comparer is IWellKnownStringEqualityComparer); 
    }

    public static IEqualityComparer GetRandomizedEqualityComparer(object comparer)
    {
        Contract.Assert(comparer == null || comparer == System.Collections.Generic.EqualityComparer<string>.Default || comparer is IWellKnownStringEqualityComparer); 

        if(comparer == null) {
            return new System.Collections.Generic.RandomizedObjectEqualityComparer();
        } 

        if(comparer == System.Collections.Generic.EqualityComparer<string>.Default) {
            return new System.Collections.Generic.RandomizedStringEqualityComparer();
        }

        IWellKnownStringEqualityComparer cmp = comparer as IWellKnownStringEqualityComparer;

        if(cmp != null) {
            return cmp.GetRandomizedEqualityComparer();
        }

        Contract.Assert(false, "Missing case in GetRandomizedEqualityComparer!");

        return null;
    }

    public static object GetEqualityComparerForSerialization(object comparer)
    {
        if(comparer == null)
        {
            return null;
        }

        IWellKnownStringEqualityComparer cmp = comparer as IWellKnownStringEqualityComparer;

        if(cmp != null)
        {
            return cmp.GetEqualityComparerForSerialization();
        }

        return comparer;
    }

    private const int bufferSize = 1024;
    private static RandomNumberGenerator rng;
    private static byte[] data;
    private static int currentIndex = bufferSize;
    private static readonly object lockObj = new Object();

    internal static long GetEntropy() 
    {
        lock(lockObj) {
            long ret;

            if(currentIndex == bufferSize) 
            {
                if(null == rng)
                {
                    rng = RandomNumberGenerator.Create();
                    data = new byte[bufferSize];
                    Contract.Assert(bufferSize % 8 == 0, "We increment our current index by 8, so our buffer size must be a multiple of 8");
                }

                rng.GetBytes(data);
                currentIndex = 0;
            }

            ret = BitConverter.ToInt64(data, currentIndex);
            currentIndex += 8;

            return ret;
        }
    }
        #endif // FEATURE_RANDOMIZED_STRING_HASHING

    }

}