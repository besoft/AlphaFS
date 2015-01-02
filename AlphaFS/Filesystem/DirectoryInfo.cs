/* Copyright (c) 2008-2014 Peter Palotas, Jeffrey Jangli, Normalex
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.AccessControl;

namespace Alphaleonis.Win32.Filesystem
{
   /// <summary>Exposes instance methods for creating, moving, and enumerating through directories and subdirectories. This class cannot be inherited.</summary>
   [SerializableAttribute]
   public sealed class DirectoryInfo : FileSystemInfo
   {
      #region Constructors

      #region DirectoryInfo

      #region .NET

      /// <summary>Initializes a new instance of the <see cref="Alphaleonis.Win32.Filesystem.DirectoryInfo"/> class on the specified path.</summary>
      /// <param name="path">The path on which to create the <see cref="Alphaleonis.Win32.Filesystem.DirectoryInfo"/>.</param>
      /// <remarks>
      /// This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.
      /// The path parameter can be a file name, including a file on a Universal Naming Convention (UNC) share.
      /// </remarks>
      public DirectoryInfo(string path) : this(null, path, false)
      {
      }
      
      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="Alphaleonis.Win32.Filesystem.DirectoryInfo"/> class on the specified path.</summary>
      /// <param name="path">The path on which to create the <see cref="Alphaleonis.Win32.Filesystem.DirectoryInfo"/>.</param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="path"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="path"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="path"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <remarks>This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.</remarks>
      public DirectoryInfo(string path, bool? isFullPath) : this(null, path, isFullPath)
      {
      }

      /// <summary>[AlphaFS] Special internal implementation.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="fullPath">The full path on which to create the <see cref="Alphaleonis.Win32.Filesystem.DirectoryInfo"/>.</param>
      /// <param name="junk1">Not used.</param>
      /// <param name="junk2">Not used.</param>
      /// <remarks>This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.</remarks>
      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "junk1")]
      [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "junk2")]
      private DirectoryInfo(KernelTransaction transaction, string fullPath, bool junk1, bool junk2)
      {
         IsDirectory = true;
         Transaction = transaction;

         LongFullName = Path.GetLongPathInternal(fullPath, false, false, false, false);

         OriginalPath = Path.GetFileName(fullPath, true);

         FullPath = fullPath;

         DisplayPath = GetDisplayName(OriginalPath);
      }

      #region Transacted

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="Alphaleonis.Win32.Filesystem.DirectoryInfo"/> class on the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path on which to create the <see cref="Alphaleonis.Win32.Filesystem.DirectoryInfo"/>.</param>
      /// <remarks>This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.</remarks>
      public DirectoryInfo(KernelTransaction transaction, string path) : this(transaction, path, false)
      {
      }

      /// <summary>[AlphaFS] Initializes a new instance of the <see cref="Alphaleonis.Win32.Filesystem.DirectoryInfo"/> class on the specified path.</summary>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path on which to create the <see cref="Alphaleonis.Win32.Filesystem.DirectoryInfo"/>.</param>
      /// <param name="isFullPath">
      /// <para><c>true</c> <paramref name="path"/> is an absolute path. Unicode prefix is applied.</para>
      /// <para><c>false</c> <paramref name="path"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      /// <para><c>null</c> <paramref name="path"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      /// <remarks>This constructor does not check if a directory exists. This constructor is a placeholder for a string that is used to access the disk in subsequent operations.</remarks>
      public DirectoryInfo(KernelTransaction transaction, string path, bool? isFullPath)
      {
         InitializeInternal(true, transaction, path, isFullPath);
      }

      #endregion // Transacted

      #endregion // AlphaFS

      #endregion // DirectoryInfo

      #endregion // Constructors

      #region Methods

      #region .NET

      #region Create

      #region .NET

      /// <summary>Creates a directory.</summary>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Create()
      {
         Directory.CreateDirectoryInternal(Transaction, LongFullName, null, null, false, null);
      }

      /// <summary>Creates a directory using a <see cref="System.Security.AccessControl.DirectorySecurity"/> object.</summary>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void Create(DirectorySecurity directorySecurity)
      {
         Directory.CreateDirectoryInternal(Transaction, LongFullName, null, directorySecurity, false, null);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Creates a directory using a <see cref="System.Security.AccessControl.DirectorySecurity"/> object.</summary>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void Create(bool compress)
      {
         Directory.CreateDirectoryInternal(Transaction, LongFullName, null, null, compress, null);
      }

      /// <summary>[AlphaFS] Creates a directory using a <see cref="System.Security.AccessControl.DirectorySecurity"/> object.</summary>
      /// <param name="directorySecurity">The access control to apply to the directory.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <remarks>If the directory already exists, this method does nothing.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void Create(DirectorySecurity directorySecurity, bool compress)
      {
         Directory.CreateDirectoryInternal(Transaction, LongFullName, null, directorySecurity, compress, null);
      }

      #endregion // AlphaFS

      #endregion // Create

      #region CreateSubdirectory

      #region .NET

      /// <summary>Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="DirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path)
      {
         return CreateSubdirectoryInternal(path, null, null, false);
      }

      /// <summary>Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="DirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="directorySecurity">The <see cref="DirectorySecurity"/> security to apply.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity)
      {
         return CreateSubdirectoryInternal(path, null, directorySecurity, false);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="DirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path, bool compress)
      {
         return CreateSubdirectoryInternal(path, null, null, compress);
      }

      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="DirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path, string templatePath, bool compress)
      {
         return CreateSubdirectoryInternal(path, templatePath, null, compress);
      }


      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="DirectoryInfo"/> class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="directorySecurity">The <see cref="DirectorySecurity"/> security to apply.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateSubdirectoryInternal(path, null, directorySecurity, compress);
      }

      /// <summary>[AlphaFS] Creates a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the <see cref="DirectoryInfo"/> class.</summary>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="path">The specified path. This cannot be a different disk volume.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <param name="directorySecurity">The <see cref="DirectorySecurity"/> security to apply.</param>
      /// <returns>The last directory specified in <paramref name="path"/>.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectoryInfo CreateSubdirectory(string path, string templatePath, DirectorySecurity directorySecurity, bool compress)
      {
         return CreateSubdirectoryInternal(path, templatePath, directorySecurity, compress);
      }

      #endregion // AlphaFS

      #endregion // CreateSubdirectory

      #region Delete

      #region .NET

      /// <summary>Deletes this <see cref="DirectoryInfo"/> if it is empty.</summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <c>null</c>.</exception>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public override void Delete()
      {
         Directory.DeleteDirectoryInternal(null, Transaction, LongFullName, false, false, true, false, null);
      }

      /// <summary>Deletes this instance of a <see cref="DirectoryInfo"/>, specifying whether to delete subdirectories and files.</summary>
      /// <param name="recursive"><c>true</c> to delete this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
      /// <remarks>
      /// If the <see cref="DirectoryInfo"/> has no files or subdirectories, this method deletes the <see cref="DirectoryInfo"/> even if <paramref name="recursive"/> is <c>false</c>.
      /// Attempting to delete a <see cref="DirectoryInfo"/> that is not empty when <paramref name="recursive"/> is <c>false</c> throws an <see cref="IOException"/>.
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException ">path is <c>null</c>.</exception>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Delete(bool recursive)
      {
         Directory.DeleteDirectoryInternal(null, Transaction, LongFullName, recursive, false, !recursive, false, null);
      }

      #endregion // .NET

      #region AlphaFS

      /// <summary>[AlphaFS] Deletes this instance of a <see cref="DirectoryInfo"/>, specifying whether to delete files and subdirectories.</summary>
      /// <param name="recursive"><c>true</c> to delete this directory, its subdirectories, and all files; otherwise, <c>false</c>.</param>
      /// <param name="ignoreReadOnly"><c>true</c> ignores read only attribute of files and directories.</param>
      /// <remarks>
      /// If the <see cref="DirectoryInfo"/> has no files or subdirectories, this method deletes the <see cref="DirectoryInfo"/> even if recursive is <c>false</c>.
      /// Attempting to delete a <see cref="DirectoryInfo"/> that is not empty when recursive is false throws an <see cref="IOException"/>.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Delete(bool recursive, bool ignoreReadOnly)
      {
         Directory.DeleteDirectoryInternal(null, Transaction, LongFullName, recursive, ignoreReadOnly, !recursive, false, null);
      }

      #endregion // AlphaFS

      #endregion // Delete

      #region EnumerateDirectories

      #region .NET

      /// <summary>Returns an enumerable collection of directory information in the current directory.</summary>
      /// <returns>An enumerable collection of directories in the current directory.</returns>
      [SecurityCritical]
      public IEnumerable<DirectoryInfo> EnumerateDirectories()
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<DirectoryInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Folders, null);
      }

      /// <summary>Returns an enumerable collection of directory information that matches a specified search pattern.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern)
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<DirectoryInfo>(Transaction, LongFullName, searchPattern, DirectoryEnumerationOptions.Folders, null);
      }

      /// <summary>Returns an enumerable collection of directory information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of directories that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public IEnumerable<DirectoryInfo> EnumerateDirectories(string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.Folders;

         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;
            
         return Directory.EnumerateFileSystemEntryInfosInternal<DirectoryInfo>(Transaction, LongFullName, searchPattern, enumOptions, null);
      }

      #endregion // .NET
      
      #endregion // EnumerateDirectories

      #region EnumerateFiles

      #region .NET

      /// <summary>Returns an enumerable collection of file information in the current directory.</summary>
      /// <returns>An enumerable collection of the files in the current directory.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles()
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<FileInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Files, null);
      }

      /// <summary>Returns an enumerable collection of file information that matches a search pattern.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/>.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles(string searchPattern)
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<FileInfo>(Transaction, LongFullName, searchPattern, DirectoryEnumerationOptions.Files, null);
      }

      /// <summary>Returns an enumerable collection of file information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of files that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SecurityCritical]
      public IEnumerable<FileInfo> EnumerateFiles(string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.Files;

         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return Directory.EnumerateFileSystemEntryInfosInternal<FileInfo>(Transaction, LongFullName, searchPattern, enumOptions, null);
      }

      #endregion // .NET

      #endregion // EnumerateFiles

      #region EnumerateFileSystemInfos

      #region .NET

      /// <summary>Returns an enumerable collection of file system information in the current directory.</summary>
      /// <returns>An enumerable collection of file system information in the current directory. </returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos()
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<FileSystemInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, null);
      }

      /// <summary>Returns an enumerable collection of file system information that matches a specified search pattern.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern)
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<FileSystemInfo>(Transaction, LongFullName, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, null);
      }

      /// <summary>Returns an enumerable collection of file system information that matches a specified search pattern and search subdirectory option.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An enumerable collection of file system information objects that matches <paramref name="searchPattern"/> and <paramref name="searchOption"/>.</returns>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public IEnumerable<FileSystemInfo> EnumerateFileSystemInfos(string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.FilesAndFolders;

         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return Directory.EnumerateFileSystemEntryInfosInternal<FileSystemInfo>(Transaction, LongFullName, searchPattern, enumOptions, null);
      }

      #endregion // .NET

      #endregion // EnumerateFileSystemInfos

      #region GetAccessControl

      #region .NET

      /// <summary>Gets a <see cref="DirectorySecurity"/> object that encapsulates the access control list (ACL) entries for the directory described by the current DirectoryInfo object.</summary>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the directory.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectorySecurity GetAccessControl()
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, LongFullName, AccessControlSections.Access | AccessControlSections.Group | AccessControlSections.Owner, null);
      }

      /// <summary>Gets a <see cref="DirectorySecurity"/> object that encapsulates the specified type of access control list (ACL) entries for the directory described by the current <see cref="DirectoryInfo"/> object.</summary>
      /// <param name="includeSections">One of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to receive.</param>
      /// <returns>A <see cref="DirectorySecurity"/> object that encapsulates the access control rules for the file described by the path parameter.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public DirectorySecurity GetAccessControl(AccessControlSections includeSections)
      {
         return File.GetAccessControlInternal<DirectorySecurity>(true, LongFullName, includeSections, null);
      }

      #endregion // .NET

      #endregion // GetAccessControl

      #region GetDirectories

      #region .NET

      /// <summary>Returns the subdirectories of the current directory.</summary>
      /// <returns>An array of <see cref="DirectoryInfo"/> objects.</returns>
      /// <remarks>If there are no subdirectories, this method returns an empty array. This method is not recursive.</remarks>
      /// <remarks>
      /// The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.
      /// </remarks>
      [SecurityCritical]
      public DirectoryInfo[] GetDirectories()
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<DirectoryInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Folders, null).ToArray();
      }

      /// <summary>Returns an array of directories in the current <see cref="DirectoryInfo"/> matching the given search criteria.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <returns>An array of type <see cref="DirectoryInfo"/> matching <paramref name="searchPattern"/>.</returns>
      /// <remarks>
      /// The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.
      /// </remarks>
      [SecurityCritical]
      public DirectoryInfo[] GetDirectories(string searchPattern)
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<DirectoryInfo>(Transaction, LongFullName, searchPattern, DirectoryEnumerationOptions.Folders, null).ToArray();
      }

      /// <summary>Returns an array of directories in the current <see cref="DirectoryInfo"/> matching the given search criteria and using a value to determine whether to search subdirectories.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An array of type <see cref="DirectoryInfo"/> matching <paramref name="searchPattern"/>.</returns>
      /// <remarks>If there are no subdirectories, or no subdirectories match the searchPattern parameter, this method returns an empty array.</remarks>
      /// <remarks>
      /// The EnumerateDirectories and GetDirectories methods differ as follows: When you use EnumerateDirectories, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetDirectories, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateDirectories can be more efficient.
      /// </remarks>
      [SecurityCritical]
      public DirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.Folders;

         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return Directory.EnumerateFileSystemEntryInfosInternal<DirectoryInfo>(Transaction, LongFullName, searchPattern, enumOptions, null).ToArray();
      }

      #endregion // .NET

      #endregion // GetDirectories
      
      #region GetFiles

      #region .NET

      /// <summary>Returns a file list from the current directory.</summary>
      /// <returns>An array of type <see cref="FileInfo"/>.</returns>
      /// <remarks>The order of the returned file names is not guaranteed; use the Sort() method if a specific sort order is required.</remarks>
      /// <remarks>If there are no files in the <see cref="DirectoryInfo"/>, this method returns an empty array.</remarks>
      /// <remarks>
      /// The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      [SecurityCritical]
      public FileInfo[] GetFiles()
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<FileInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.Files, null).ToArray();
      }

      /// <summary>Returns a file list from the current directory matching the given search pattern.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <returns>An array of type <see cref="FileInfo"/>.</returns>
      /// <remarks>The order of the returned file names is not guaranteed; use the Sort() method if a specific sort order is required.</remarks>
      /// <remarks>If there are no files in the <see cref="DirectoryInfo"/>, this method returns an empty array.</remarks>
      /// <remarks>
      /// The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      [SecurityCritical]
      public FileInfo[] GetFiles(string searchPattern)
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<FileInfo>(Transaction, LongFullName, searchPattern, DirectoryEnumerationOptions.Files, null).ToArray();
      }

      /// <summary>Returns a file list from the current directory matching the given search pattern and using a value to determine whether to search subdirectories.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An array of type <see cref="FileInfo"/>.</returns>
      /// <remarks>The order of the returned file names is not guaranteed; use the Sort() method if a specific sort order is required.</remarks>
      /// <remarks>If there are no files in the <see cref="DirectoryInfo"/>, this method returns an empty array.</remarks>
      /// <remarks>
      /// The EnumerateFiles and GetFiles methods differ as follows: When you use EnumerateFiles, you can start enumerating the collection of names
      /// before the whole collection is returned; when you use GetFiles, you must wait for the whole array of names to be returned before you can access the array.
      /// Therefore, when you are working with many files and directories, EnumerateFiles can be more efficient.
      /// </remarks>
      [SecurityCritical]
      public FileInfo[] GetFiles(string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.Files;

         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return Directory.EnumerateFileSystemEntryInfosInternal<FileInfo>(Transaction, LongFullName, searchPattern, enumOptions, null).ToArray();
      }

      #endregion // .NET

      #endregion // GetFiles

      #region GetFileSystemInfos

      #region .NET

      /// <summary>Returns an array of strongly typed <see cref="FileSystemInfo"/> entries representing all the files and subdirectories in a directory.</summary>
      /// <returns>An array of strongly typed <see cref="FileSystemInfo"/> entries.</returns>
      /// <remarks>
      /// For subdirectories, the <see cref="FileSystemInfo"/> objects returned by this method can be cast to the derived class <see cref="DirectoryInfo"/>.
      /// Use the <see cref="FileAttributes"/> value returned by the <see cref="FileSystemInfo.Attributes"/> property to determine whether the <see cref="FileSystemInfo"/> represents a file or a directory.
      /// </remarks>
      /// <remarks>
      /// If there are no files or directories in the DirectoryInfo, this method returns an empty array. This method is not recursive.
      /// For subdirectories, the FileSystemInfo objects returned by this method can be cast to the derived class DirectoryInfo.
      /// Use the FileAttributes value returned by the Attributes property to determine whether the FileSystemInfo represents a file or a directory.
      /// </remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public FileSystemInfo[] GetFileSystemInfos()
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<FileSystemInfo>(Transaction, LongFullName, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, null).ToArray();
      }

      /// <summary>Retrieves an array of strongly typed <see cref="FileSystemInfo"/> objects representing the files and subdirectories that match the specified search criteria.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <returns>An array of strongly typed <see cref="FileSystemInfo"/> entries.</returns>
      /// <remarks>
      /// For subdirectories, the <see cref="FileSystemInfo"/> objects returned by this method can be cast to the derived class <see cref="DirectoryInfo"/>.
      /// Use the <see cref="FileAttributes"/> value returned by the <see cref="FileSystemInfo.Attributes"/> property to determine whether the <see cref="FileSystemInfo"/> represents a file or a directory.
      /// </remarks>
      /// <remarks>
      /// If there are no files or directories in the DirectoryInfo, this method returns an empty array. This method is not recursive.
      /// For subdirectories, the FileSystemInfo objects returned by this method can be cast to the derived class DirectoryInfo.
      /// Use the FileAttributes value returned by the Attributes property to determine whether the FileSystemInfo represents a file or a directory.
      /// </remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public FileSystemInfo[] GetFileSystemInfos(string searchPattern)
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<FileSystemInfo>(Transaction, LongFullName, searchPattern, DirectoryEnumerationOptions.FilesAndFolders, null).ToArray();
      }

      /// <summary>Retrieves an array of strongly typed <see cref="FileSystemInfo"/> objects representing the files and subdirectories that match the specified search criteria.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <param name="searchOption">
      /// <para>One of the <see cref="SearchOption"/> enumeration values that specifies whether the <paramref name="searchOption"/></para>
      /// <para> should include only the current directory or should include all subdirectories.</para>
      /// </param>
      /// <returns>An array of strongly typed <see cref="FileSystemInfo"/> entries.</returns>
      /// <remarks>
      /// For subdirectories, the <see cref="FileSystemInfo"/> objects returned by this method can be cast to the derived class <see cref="DirectoryInfo"/>.
      /// Use the <see cref="FileAttributes"/> value returned by the <see cref="FileSystemInfo.Attributes"/> property to determine whether the <see cref="FileSystemInfo"/> represents a file or a directory.
      /// </remarks>
      /// <remarks>
      /// If there are no files or directories in the DirectoryInfo, this method returns an empty array. This method is not recursive.
      /// For subdirectories, the FileSystemInfo objects returned by this method can be cast to the derived class DirectoryInfo.
      /// Use the FileAttributes value returned by the Attributes property to determine whether the FileSystemInfo represents a file or a directory.
      /// </remarks>
      [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Infos")]
      [SecurityCritical]
      public FileSystemInfo[] GetFileSystemInfos(string searchPattern, SearchOption searchOption)
      {
         DirectoryEnumerationOptions enumOptions = DirectoryEnumerationOptions.FilesAndFolders;

         if (searchOption == SearchOption.AllDirectories)
            enumOptions |= DirectoryEnumerationOptions.Recursive;

         return Directory.EnumerateFileSystemEntryInfosInternal<FileSystemInfo>(Transaction, LongFullName, searchPattern, enumOptions, null).ToArray();
      }

      #endregion // .NET

      #endregion // GetFileSystemInfos

      #region MoveTo

      /// <summary>Moves a <see cref="DirectoryInfo"/> instance and its contents to a new path.
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing directory by default.</para>
      /// <para>This method does not work across disk volumes.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationFullPath">
      /// <para>The name and path to which to move this directory.</para>
      /// <para>The destination cannot be another disk volume or a directory with the identical name.</para>
      /// <para>It can be an existing directory to which you want to add this directory as a subdirectory.</para>
      /// </param>
      [SecurityCritical]
      public void MoveTo(string destinationFullPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationFullPath, null, MoveOptions.None, null, null, out destinationPathLp, true);
         CopyToMoveToInternalRefresh(destinationFullPath, destinationPathLp);
      }

      #endregion // MoveTo

      #region Refresh

      #region .NET

      /// <summary>Refreshes the state of the object.</summary>
      [SecurityCritical]
      public new void Refresh()
      {
         base.Refresh();
      }

      #endregion // .NET

      #endregion // Refresh

      #region SetAccessControl

      #region .NET

      /// <summary>Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the directory described by the current DirectoryInfo object.</summary>
      /// <param name="directorySecurity">A <see cref="DirectorySecurity"/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void SetAccessControl(DirectorySecurity directorySecurity)
      {
         File.SetAccessControlInternal(LongFullName, null, directorySecurity, AccessControlSections.All, null);
      }

      /// <summary>Applies access control list (ACL) entries described by a <see cref="DirectorySecurity"/> object to the directory described by the current DirectoryInfo object.</summary>
      /// <param name="directorySecurity">A <see cref="DirectorySecurity"/> object that describes an ACL entry to apply to the directory described by the path parameter.</param>
      /// <param name="includeSections">One or more of the <see cref="AccessControlSections"/> values that specifies the type of access control list (ACL) information to set.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
      [SecurityCritical]
      public void SetAccessControl(DirectorySecurity directorySecurity, AccessControlSections includeSections)
      {
         File.SetAccessControlInternal(LongFullName, null, directorySecurity, includeSections, null);
      }

      #endregion // .NET

      #endregion // SetAccessControl

      #region ToString

      #region .NET

      /// <summary>Returns the original path that was passed by the user.</summary>
      public override string ToString()
      {
         return DisplayPath;
      }

      #endregion // .NET

      #endregion // ToString

      #endregion // .NET

      #region AlphaFS

      #region AddStream

      /// <summary>[AlphaFS] Adds an alternate data stream (NTFS ADS) to the directory.</summary>
      /// <param name="name">The name for the stream. If a stream with <paramref name="name"/> already exists, it will be overwritten.</param>
      /// <param name="contents">The lines to add to the stream.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void AddStream(string name, string[] contents)
      {
         AlternateDataStreamInfo.AddStreamInternal(true, Transaction, LongFullName, name, contents, null);
      }

      #endregion // AddStream

      #region CopyTo1

      #region IsFullPath

      #region DirectoryInfo

      /// <summary>[AlphaFS] Copies a <see cref="DirectoryInfo"/> instance and its contents to a new path.
      /// <para>&#160;</para>
      /// <returns>Returns a new <see cref="DirectoryInfo"/> instance if the directory was completely copied.</returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing directory by default.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public DirectoryInfo CopyTo1(string destinationPath, bool? isFullPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, CopyOptions.FailIfExists, null, null, null, out destinationPathLp, isFullPath);
         return new DirectoryInfo(Transaction, destinationPathLp, null);
      }

      /// <summary>[AlphaFS] Copies a <see cref="DirectoryInfo"/> instance and its contents to a new path.
      /// <para>&#160;</para>
      /// <returns>Returns a new <see cref="DirectoryInfo"/> instance if the directory was completely copied.</returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public DirectoryInfo CopyTo1(string destinationPath, CopyOptions copyOptions, bool? isFullPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, copyOptions, null, null, null, out destinationPathLp, isFullPath);
         return new DirectoryInfo(Transaction, destinationPathLp, null);
      }

      #endregion // DirectoryInfo

      #region CopyMoveResult

      /// <summary>[AlphaFS] Copies a <see cref="DirectoryInfo"/> instance and its contents to a new path, <see cref="CopyOptions"/> can be specified,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing directory by default.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public CopyMoveResult CopyTo1(string destinationPath, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, CopyOptions.FailIfExists, null, progressHandler, userProgressData, out destinationPathLp, isFullPath);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      /// <summary>[AlphaFS] Copies a <see cref="DirectoryInfo"/> instance and its contents to a new path, <see cref="CopyOptions"/> can be specified,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <c>null</c>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public CopyMoveResult CopyTo1(string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, isFullPath);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      #endregion // CopyMoveResult

      #endregion // IsFullPath

      #region DirectoryInfo

      /// <summary>[AlphaFS] Copies a <see cref="DirectoryInfo"/> instance and its contents to a new path.
      /// <para>&#160;</para>
      /// <returns>Returns a new <see cref="DirectoryInfo"/> instance if the directory was completely copied.</returns>
      /// <para>&#160;</para>
      /// </summary>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing directory by default.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The destination directory path.</param>
      [SecurityCritical]
      public DirectoryInfo CopyTo1(string destinationPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, CopyOptions.FailIfExists, null, null, null, out destinationPathLp, false);
         return new DirectoryInfo(Transaction, destinationPathLp, null);
      }

      /// <summary>[AlphaFS] Copies a <see cref="DirectoryInfo"/> instance and its contents to a new path.
      /// <para>&#160;</para>
      /// <returns>Returns a new <see cref="DirectoryInfo"/> instance if the directory was completely copied.</returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <c>null</c>.</param>
      [SecurityCritical]
      public DirectoryInfo CopyTo1(string destinationPath, CopyOptions copyOptions)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationPath, copyOptions, null, null, null, out destinationPathLp, false);
         return new DirectoryInfo(Transaction, destinationPathLp, null);
      }

      #endregion // DirectoryInfo

      #region CopyMoveResult

      /// <summary>[AlphaFS] Copies a <see cref="DirectoryInfo"/> instance and its contents to a new path, <see cref="CopyOptions"/> can be specified,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to prevent overwriting of an existing directory by default.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo1(string destinationPath, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, CopyOptions.FailIfExists, null, progressHandler, userProgressData, out destinationPathLp, false);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      /// <summary>[AlphaFS] Copies a <see cref="DirectoryInfo"/> instance and its contents to a new path, <see cref="CopyOptions"/> can be specified,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy action.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the directory is to be copied. This parameter can be <c>null</c>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      [SecurityCritical]
      public CopyMoveResult CopyTo1(string destinationPath, CopyOptions copyOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationPath, copyOptions, null, progressHandler, userProgressData, out destinationPathLp, false);
         CopyToMoveToInternalRefresh(destinationPath, destinationPathLp);
         return cmr;
      }

      #endregion // CopyMoveResult

      #endregion // CopyTo

      #region CountFileSystemObjects

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public long CountFileSystemObjects(DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<string>(Transaction, LongFullName, Path.WildcardStarMatchAll, directoryEnumerationOptions, null).Count();
      }

      /// <summary>[AlphaFS] Counts file system objects: files, folders or both) in a given directory.</summary>
      /// <param name="searchPattern">
      /// <para>The search string to match against the names of directories in path. This parameter can contain a</para>
      /// <para>combination of valid literal path and wildcard (<see cref="Path.WildcardStarMatchAll"/> and <see cref="Path.WildcardQuestion"/>)</para>
      /// <para>characters, but doesn't support regular expressions.</para>
      /// </param>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <returns>The counted number of file system objects.</returns>
      /// <exception cref="System.UnauthorizedAccessException">An exception is thrown case of access errors.</exception>
      [SecurityCritical]
      public long CountFileSystemObjects(string searchPattern, DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         return Directory.EnumerateFileSystemEntryInfosInternal<string>(Transaction, LongFullName, searchPattern, directoryEnumerationOptions, null).Count();
      }

      #endregion // CountFileSystemObjects
      
      #region Compress

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <remarks>This will only compress the root items, non recursive.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Compress()
      {
         Directory.CompressDecompressInternal(Transaction, LongFullName, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, true, null);
      }

      /// <summary>[AlphaFS] Compresses a directory using NTFS compression.</summary>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Compress(DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         Directory.CompressDecompressInternal(Transaction, LongFullName, Path.WildcardStarMatchAll, directoryEnumerationOptions, true, null);
      }

      #endregion // Compress

      #region DisableCompression

      /// <summary>[AlphaFS] Disables compression of the specified directory and the files in it.</summary>
      /// <remarks>
      /// This method disables the directory-compression attribute. It will not decompress the current contents of the directory.
      /// However, newly created files and directories will be uncompressed.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void DisableCompression()
      {
         Device.ToggleCompressionInternal(true, Transaction, LongFullName, false, null);
      }

      #endregion // DisableCompression

      #region DisableEncryption

      /// <summary>[AlphaFS] Disables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=0"</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void DisableEncryption()
      {
         Directory.EnableDisableEncryptionInternal(LongFullName, false, null);
      }

      #endregion // DisableEncryption

      #region Decompress

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <remarks>This will only decompress the root items, non recursive.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Decompress()
      {
         Directory.CompressDecompressInternal(Transaction, LongFullName, Path.WildcardStarMatchAll, DirectoryEnumerationOptions.FilesAndFolders, false, null);
      }

      /// <summary>[AlphaFS] Decompresses an NTFS compressed directory.</summary>
      /// <param name="directoryEnumerationOptions"><see cref="DirectoryEnumerationOptions"/> flags that specify how the directory is to be enumerated.</param>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void Decompress(DirectoryEnumerationOptions directoryEnumerationOptions)
      {
         Directory.CompressDecompressInternal(Transaction, LongFullName, Path.WildcardStarMatchAll, directoryEnumerationOptions, false, null);
      }

      #endregion // Decompress

      #region Decrypt

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      [SecurityCritical]
      public void Decrypt()
      {
         Directory.EncryptDecryptDirectoryInternal(LongFullName, false, false, null);
      }

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="recursive"><c>true</c> to decrypt the directory recursively. <c>false</c> only decrypt files and directories in the root of the directory.</param>
      [SecurityCritical]
      public void Decrypt(bool recursive)
      {
         Directory.EncryptDecryptDirectoryInternal(LongFullName, false, recursive, null);
      }

      #endregion // Decrypt
      
      #region DeleteEmpty

      /// <summary>[AlphaFS] Deletes empty subdirectores from the <see cref="DirectoryInfo"/> instance.</summary>
      [SecurityCritical]
      public void DeleteEmpty()
      {
         Directory.DeleteEmptyDirectoryInternal(null, Transaction, LongFullName, false, false, true, null);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectores from the <see cref="DirectoryInfo"/> instance.</summary>
      /// <param name="recursive"><c>true</c> deletes empty subdirectories from this directory and its subdirectories.</param>
      [SecurityCritical]
      public void DeleteEmpty(bool recursive)
      {
         Directory.DeleteEmptyDirectoryInternal(null, Transaction, LongFullName, recursive, false, true, null);
      }

      /// <summary>[AlphaFS] Deletes empty subdirectores from the <see cref="DirectoryInfo"/> instance.</summary>
      /// <param name="recursive"><c>true</c> deletes empty subdirectories from this directory and its subdirectories.</param>
      /// <param name="ignoreReadOnly"><c>true</c> overrides read only <see cref="FileAttributes"/> of empty directories.</param>
      [SecurityCritical]
      public void DeleteEmpty(bool recursive, bool ignoreReadOnly)
      {
         Directory.DeleteEmptyDirectoryInternal(null, Transaction, LongFullName, recursive, ignoreReadOnly, true, null);
      }

      #endregion // DeleteEmpty

      #region EnableCompression

      /// <summary>[AlphaFS] Enables compression of the specified directory and the files in it.</summary>
      /// <remarks>
      /// This method enables the directory-compression attribute. It will not compress the current contents of the directory.
      /// However, newly created files and directories will be compressed.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void EnableCompression()
      {
         Device.ToggleCompressionInternal(true, Transaction, LongFullName, true, null);
      }

      #endregion // EnableCompression
      
      #region EnableEncryption

      /// <summary>[AlphaFS] Enables encryption of the specified directory and the files in it. It does not affect encryption of subdirectories below the indicated directory.</summary>
      /// <returns><c>true</c> on success, <c>false</c> otherwise.</returns>
      /// <remarks>This method will create/change the file "Desktop.ini" and wil set Encryption value: "Disable=1"</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void EnableEncryption()
      {
         Directory.EnableDisableEncryptionInternal(LongFullName, true, null);
      }

      #endregion // EnableEncryption

      #region Encrypt

      /// <summary>[AlphaFS] Encrypts a directory so that only the account used to encrypt the directory can decrypt it.</summary>
      [SecurityCritical]
      public void Encrypt()
      {
         Directory.EncryptDecryptDirectoryInternal(LongFullName, true, false, null);
      }

      /// <summary>[AlphaFS] Decrypts a directory that was encrypted by the current account using the Encrypt method.</summary>
      /// <param name="recursive"><c>true</c> to encrypt the directory recursively. <c>false</c> only encrypt files and directories in the root of the directory.</param>
      [SecurityCritical]
      public void Encrypt(bool recursive)
      {
         Directory.EncryptDecryptDirectoryInternal(LongFullName, true, recursive, null);
      }

      #endregion // Encrypt

      #region EnumerateStreams

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory.</summary>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public IEnumerable<AlternateDataStreamInfo> EnumerateStreams()
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(true, Transaction, null, LongFullName, null, null, null);
      }

      /// <summary>[AlphaFS] Returns an enumerable collection of <see cref="AlternateDataStreamInfo"/> instances for the directory.</summary>
      /// <returns>An enumerable collection of <see cref="AlternateDataStreamInfo"/> of type <see cref="StreamType"/> instances for the directory.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public IEnumerable<AlternateDataStreamInfo> EnumerateStreams(StreamType streamType)
      {
         return AlternateDataStreamInfo.EnumerateStreamsInternal(true, Transaction, null, LongFullName, null, streamType, null);
      }

      #endregion // EnumerateStreams

      #region GetDirName

      private static string GetDirName(string path)
      {
         return path.Length > 3 ? Path.GetFileName(Path.RemoveDirectorySeparator(path, false), true) : path;
      }

      #endregion // GetDirName

      #region GetDisplayName

      private static string GetDisplayName(string path)
      {
         return path.Length != 2 || (path[1] != Path.VolumeSeparatorChar) ? path : Path.CurrentDirectoryPrefix;
      }

      #endregion // GetDisplayName

      #region GetStreamSize

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by all data streams (NTFS ADS).</summary>
      /// <returns>The number of bytes used by all data streams.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public long GetStreamSize()
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, Transaction, null, LongFullName, null, null, null);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a named data streams (NTFS ADS).</summary>
      /// <param name="name">The name of the stream to retrieve.</param>
      /// <returns>The number of bytes used by a named stream.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public long GetStreamSize(string name)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, Transaction, null, LongFullName, name, StreamType.Data, null);
      }

      /// <summary>[AlphaFS] Retrieves the actual number of bytes of disk storage used by a <see cref="StreamType"/> data streams (NTFS ADS).</summary>
      /// <param name="type">The <see cref="StreamType"/> of the stream to retrieve.</param>
      /// <returns>The number of bytes used by stream of type <see cref="StreamType"/>.</returns>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public long GetStreamSize(StreamType type)
      {
         return AlternateDataStreamInfo.GetStreamSizeInternal(true, Transaction, null, LongFullName, null, type, null);
      }

      #endregion GetStreamSize

      #region MoveTo1

      #region IsFullPath

      #region DirectoryInfo

      /// <summary>[AlphaFS] Moves a <see cref="DirectoryInfo"/> instance and its contents to a new path, <see cref="MoveOptions"/> can be specified,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a new <see cref="DirectoryInfo"/> instance if the directory was completely moved.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      /// <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationFullPath">
      /// <para>The name and path to which to move this directory.</para>
      /// <para>The destination cannot be another disk volume unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>, or a directory with the identical name.</para>
      /// <para>It can be an existing directory to which you want to add this directory as a subdirectory.</para>
      /// </param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationFullPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationFullPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationFullPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public DirectoryInfo MoveTo1(string destinationFullPath, MoveOptions moveOptions, bool? isFullPath)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationFullPath, null, moveOptions, null, null, out destinationPathLp, isFullPath);
         return new DirectoryInfo(Transaction, destinationPathLp, null);
      }

      #endregion // DirectoryInfo

      #region CopyMoveResult

      /// <summary>[AlphaFS] Moves a <see cref="DirectoryInfo"/> instance and its contents to a new path, <see cref="MoveOptions"/> can be specified,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      /// <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationFullPath">
      /// <para>The name and path to which to move this directory.</para>
      /// <para>The destination cannot be another disk volume unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>, or a directory with the identical name.</para>
      /// <para>It can be an existing directory to which you want to add this directory as a subdirectory.</para>
      /// </param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <c>null</c>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationFullPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationFullPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationFullPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      public CopyMoveResult MoveTo1(string destinationFullPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, bool? isFullPath)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationFullPath, null, moveOptions, progressHandler, userProgressData, out destinationPathLp, isFullPath);
         CopyToMoveToInternalRefresh(destinationFullPath, destinationPathLp);
         return cmr;
      }

      #endregion // CopyMoveResult

      #endregion // IsFullPath

      #region DirectoryInfo

      /// <summary>[AlphaFS] Moves a <see cref="DirectoryInfo"/> instance and its contents to a new path, <see cref="MoveOptions"/> can be specified,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a new <see cref="DirectoryInfo"/> instance if the directory was completely moved.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      /// <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationFullPath">
      /// <para>The name and path to which to move this directory.</para>
      /// <para>The destination cannot be another disk volume unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>, or a directory with the identical name.</para>
      /// <para>It can be an existing directory to which you want to add this directory as a subdirectory.</para>
      /// </param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <c>null</c>.</param>
      [SecurityCritical]
      public DirectoryInfo MoveTo1(string destinationFullPath, MoveOptions moveOptions)
      {
         string destinationPathLp;
         CopyToMoveToInternal(destinationFullPath, null, moveOptions, null, null, out destinationPathLp, true);
         return new DirectoryInfo(Transaction, destinationPathLp, null);
      }

      #endregion // DirectoryInfo

      #region CopyMoveResult

      /// <summary>[AlphaFS] Moves a <see cref="DirectoryInfo"/> instance and its contents to a new path, <see cref="MoveOptions"/> can be specified,
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>
      /// <para>Returns a <see cref="CopyMoveResult"/> class with the status of the Move action.</para>
      /// </returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Use this method to allow or prevent overwriting of an existing directory.</para>
      /// <para>This method does not work across disk volumes unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>.</para>
      /// <para>Whenever possible, avoid using short file names (such as XXXXXX~1.XXX) with this method.</para>
      /// <para>If two directories have equivalent short file names then this method may fail and raise an exception and/or result in undesirable behavior.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationFullPath">
      /// <para>The name and path to which to move this directory.</para>
      /// <para>The destination cannot be another disk volume unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.CopyAllowed"/>, or a directory with the identical name.</para>
      /// <para>It can be an existing directory to which you want to add this directory as a subdirectory.</para>
      /// </param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the directory is to be moved. This parameter can be <c>null</c>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the directory has been moved. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      [SecurityCritical]
      public CopyMoveResult MoveTo1(string destinationFullPath, MoveOptions moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData)
      {
         string destinationPathLp;
         CopyMoveResult cmr = CopyToMoveToInternal(destinationFullPath, null, moveOptions, progressHandler, userProgressData, out destinationPathLp, true);
         CopyToMoveToInternalRefresh(destinationFullPath, destinationPathLp);
         return cmr;
      }

      #endregion // CopyMoveResult

      #endregion // MoveTo1

      #region RefreshEntryInfo

      /// <summary>Refreshes the state of the <see cref="FileSystemEntryInfo"/> EntryInfo instance.</summary>
      [SecurityCritical]
      public new void RefreshEntryInfo()
      {
         base.RefreshEntryInfo();
      }

      #endregion // RefreshEntryInfo

      #region RemoveStream

      /// <summary>[AlphaFS] Removes all alternate data streams (NTFS ADS) from the directory.</summary>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void RemoveStream()
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, Transaction, LongFullName, null, null);
      }

      /// <summary>[AlphaFS] Removes an alternate data stream (NTFS ADS) from the directory.</summary>
      /// <param name="name">The name of the stream to remove.</param>
      /// <remarks>This method only removes streams of type <see cref="StreamType.AlternateData"/>.</remarks>
      /// <remarks>No Exception is thrown if the stream does not exist.</remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      public void RemoveStream(string name)
      {
         AlternateDataStreamInfo.RemoveStreamInternal(true, Transaction, LongFullName, name, null);
      }

      #endregion // RemoveStream


      #region Unified Internals

      #region CopyToMoveToInternal

      /// <summary>[AlphaFS] Unified method CopyMoveInternal() to copy/move a Non-/Transacted file or directory including its children to a new location,
      /// <para><see cref="CopyOptions"/> or <see cref="MoveOptions"/> can be specified,</para>
      /// <para>and the possibility of notifying the application of its progress through a callback function.</para>
      /// <para>&#160;</para>
      /// <returns>Returns a <see cref="CopyMoveResult"/> class with the status of the Copy or Move action.</returns>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>Option <see cref="CopyOptions.NoBuffering"/> is recommended for very large file transfers.</para>
      /// <para>You cannot use the Move method to overwrite an existing file, unless <paramref name="moveOptions"/> contains <see cref="MoveOptions.ReplaceExisting"/>.</para>
      /// <para>This Move method works across disk volumes, and it does not throw an exception if the source and destination are the same. </para>
      /// <para>Note that if you attempt to replace a file by moving a file of the same name into that directory, you get an IOException.</para>
      /// </remarks>
      /// </summary>
      /// <exception cref="ArgumentException">The path parameter contains invalid characters, is empty, or contains only white spaces.</exception>
      /// <exception cref="ArgumentNullException">path is <c>null</c>.</exception>
      /// <exception cref="DirectoryNotFoundException"/>
      /// <exception cref="IOException"/>
      /// <exception cref="NotSupportedException"/>
      /// <exception cref="UnauthorizedAccessException"/>
      /// <exception cref="NativeError.ThrowException()"/>
      /// <param name="destinationPath">The destination directory path.</param>
      /// <param name="copyOptions"><see cref="CopyOptions"/> that specify how the file is to be copied. This parameter can be <c>null</c>.</param>
      /// <param name="moveOptions"><see cref="MoveOptions"/> that specify how the file is to be moved. This parameter can be <c>null</c>.</param>
      /// <param name="progressHandler">A callback function that is called each time another portion of the file has been copied. This parameter can be <c>null</c>.</param>
      /// <param name="userProgressData">The argument to be passed to the callback function. This parameter can be <c>null</c>.</param>
      /// <param name="longFullPath">Returns the retrieved long full path.</param>
      /// <param name="isFullPath">
      ///    <para><c>true</c> <paramref name="destinationPath"/> is an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>false</c> <paramref name="destinationPath"/> will be checked and resolved to an absolute path. Unicode prefix is applied.</para>
      ///    <para><c>null</c> <paramref name="destinationPath"/> is already an absolute path with Unicode prefix. Use as is.</para>
      /// </param>
      [SecurityCritical]
      private CopyMoveResult CopyToMoveToInternal(string destinationPath, CopyOptions? copyOptions, MoveOptions? moveOptions, CopyMoveProgressRoutine progressHandler, object userProgressData, out string longFullPath, bool? isFullPath)
      {
         string destinationPathLp = isFullPath == null
            ? destinationPath
            : (bool) isFullPath
               ? Path.GetLongPathInternal(destinationPath, false, false, false, false)
#if NET35
               : Path.GetFullPathInternal(Transaction, destinationPath, true, false, false, true, false, true, true);
#else
               : Path.GetFullPathInternal(Transaction, destinationPath, true, true, false, true, false, true, true);
#endif

         longFullPath = destinationPathLp;

         // Returns false when CopyMoveProgressResult is PROGRESS_CANCEL or PROGRESS_STOP.
         return Directory.CopyMoveInternal(Transaction, LongFullName, destinationPathLp, copyOptions, moveOptions, progressHandler, userProgressData, null);
      }

      private void CopyToMoveToInternalRefresh(string destinationPath, string destinationPathLp)
      {
         LongFullName = destinationPathLp;
         FullPath = Path.GetRegularPathInternal(destinationPathLp, false, false, false, false);

         OriginalPath = destinationPath;
         DisplayPath = OriginalPath;

         // Flush any cached information about the directory.
         Reset();
      }

      #endregion // CopyToMoveToInternal

      #region CreateSubdirectoryInternal

      /// <summary>[AlphaFS] Unified method CreateSubdirectory() to create a subdirectory or subdirectories on the specified path. The specified path can be relative to this instance of the DirectoryInfo class.</summary>
      /// <param name="path">The specified path. This cannot be a different disk volume or Universal Naming Convention (UNC) name.</param>
      /// <param name="templatePath">The path of the directory to use as a template when creating the new directory.</param>
      /// <param name="directorySecurity">The <see cref="DirectorySecurity"/> security to apply.</param>
      /// <param name="compress">When <c>true</c> compresses the directory.</param>
      /// <returns>The last directory specified in path as an <see cref="DirectoryInfo"/> object.</returns>
      /// <remarks>
      /// Any and all directories specified in path are created, unless some part of path is invalid.
      /// The path parameter specifies a directory path, not a file path.
      /// If the subdirectory already exists, this method does nothing.
      /// </remarks>
      /// <exception cref="NativeError.ThrowException()"/>
      [SecurityCritical]
      private DirectoryInfo CreateSubdirectoryInternal(string path, string templatePath, DirectorySecurity directorySecurity, bool compress)
      {
         string pathLp = Path.CombineInternal(false, LongFullName, path);

         if (string.Compare(LongFullName, 0, pathLp, 0, LongFullName.Length, StringComparison.OrdinalIgnoreCase) != 0)
            throw new ArgumentException("Invalid SubPath", pathLp);

         return Directory.CreateDirectoryInternal(Transaction, pathLp, templatePath, directorySecurity, compress, true);
      }

      #endregion // CreateSubdirectoryInternal
      
      #endregion // Unified Internals

      #endregion // AlphaFS

      #endregion // Methods

      #region Properties

      #region .NET

      #region Exists

      /// <summary>Gets a value indicating whether the directory exists.
      /// <para>&#160;</para>
      /// <value><c>true</c> if the directory exists; otherwise, <c>false</c>.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>The <see cref="Exists"/> property returns <c>false</c> if any error occurs while trying to determine if the specified directory exists.</para>
      /// <para>This can occur in situations that raise exceptions such as passing a directory name with invalid characters or too many characters,</para>
      /// <para>a failing or missing disk, or if the caller does not have permission to read the directory.</para>
      /// </remarks>
      /// </summary>
      [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
      public override bool Exists
      {
         [SecurityCritical]
         get
         {
            try
            {
               if (DataInitialised == -1)
                  Refresh();

               FileAttributes attrs = Win32AttributeData.FileAttributes;
               return DataInitialised == 0 && (attrs != (FileAttributes) (-1) && (attrs & FileAttributes.Directory) != 0);
            }
            catch
            {
               return false;
            }
         }
      }

      #endregion // Exists

      #region Name

      /// <summary>Gets the name of this <see cref="DirectoryInfo"/> instance.
      /// <para>&#160;</para>
      /// <value>The directory name.</value>
      /// <para>&#160;</para>
      /// <remarks>
      /// <para>This Name property returns only the name of the directory, such as "Bin".</para>
      /// <para>To get the full path, such as "c:\public\Bin", use the FullName property.</para>
      /// </remarks>
      /// </summary>
      public override string Name
      {
         get { return GetDirName(FullPath); }
      }

      #endregion // Name

      #region Parent

      /// <summary>Gets the parent directory of a specified subdirectory.
      /// <para>&#160;</para>
      /// <value>The parent directory, or null if the path is null or if the file path denotes a root (such as "\", "C:", or * "\\server\share").</value>
      /// <para>&#160;</para>
      /// </summary>
      public DirectoryInfo Parent
      {
         [SecurityCritical]
         get
         {
            string path = FullPath;

            if (path.Length > 3)
               path = Path.RemoveDirectorySeparator(FullPath, false);

            string dirName = Path.GetDirectoryName(path, false);
            return dirName == null ? null : new DirectoryInfo(Transaction, dirName, true, true);
         }
      }

      #endregion // Parent

      #region Root

      /// <summary>Gets the root portion of the directory.
      /// <para>&#160;</para>
      /// <value>An object that represents the root of the directory.</value>
      /// <para>&#160;</para>
      /// </summary>
      public DirectoryInfo Root
      {
         [SecurityCritical] get { return new DirectoryInfo(Transaction, Path.GetPathRoot(FullPath, false), true); }
      }

      #endregion // Root

      #endregion // .NET

      #endregion // Properties
   }
}