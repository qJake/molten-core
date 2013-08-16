using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace Molten.Core.Storage
{
    /// <summary>
    /// Manages saving and loading of .xml storage files in the user's profile.
    /// </summary>
    /// <remarks>TODO: Support encryption so that users cannot read/alter storage files manually.</remarks>
    public static class XmlStorageManager
    {
        /// <summary>
        /// Specifies the path to the local storage location.
        /// </summary>
        private static string storageLocation;

        /// <summary>
        /// Whether or not the storage manager has been initialized. No save/load operations are permitted without initializing first.
        /// </summary>
        private static bool isInitialized;

        /// <summary>
        /// Initializes the XmlStorageManager which will be the folder used to store information. The storage area will be named according to the executing assembly's name.
        /// </summary>
        public static void Initialize()
        {
            Initialize(Assembly.GetExecutingAssembly().GetName().Name);
        }

        /// <summary>
        /// Resets the XmlStorageManager to its initial state, before initialization.
        /// </summary>
        public static void Reset()
        {
            storageLocation = null;
            isInitialized = false;
        }

        /// <summary>
        /// Initializes the XmlStorageManager with the specified storage area name, which will be the folder used to store information.
        /// </summary>
        /// <param name="storageAreaName">The name of the folder to use for storage.</param>
        /// <remarks>TODO: Support other arbitrary locations instead of always using %appdata%.</remarks>
        public static void Initialize(string storageAreaName)
        {
            if (storageAreaName.Intersect(Path.GetInvalidPathChars()).Any())
            {
                throw new StorageInitializationException("Storage area name contains invalid directory characters.");
            }
            else
            {
                storageLocation = @"%appdata%\" + storageAreaName + @"\";

                InitializeStorageArea();
            }
        }

        /// <summary>
        /// Serializes an object and saves its current state to a storage location on the disk.
        /// </summary>
        /// <param name="name">An arbitrary name for the stored object (used to load it later).</param>
        /// <param name="o">The object to serialize and save. The object must be serializable. Non-serializable properties must be marked with the XmlIgnore attribute.</param>
        public static void Save(string name, object o)
        {
            if (isInitialized)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Name must not be null or empty.", "name");
                }
                if (name.Intersect(Path.GetInvalidFileNameChars()).Any())
                {
                    throw new ArgumentException("Name contains invalid characters.", "name");
                }
                if (o == null)
                {
                    throw new ArgumentException("Object must not be null.", "o");
                }
                if (!o.GetType().IsSerializable)
                {
                    throw new ArgumentException("Object must be marked Serializable.", "o");
                }

                File.WriteAllText(Path.Combine(Environment.ExpandEnvironmentVariables(storageLocation), name + ".xml"), Serialize(o));
            }
            else
            {
                throw new InvalidOperationException("Unable to save object: The XmlStorageManager has not been initialized. (Try calling XmlStorageManager.Initialize())");
            }
        }

        /// <summary>
        /// Loads an object's state from storage.
        /// </summary>
        /// <typeparam name="T">The type of object to load.</typeparam>
        /// <param name="name">The arbitrary name of the stored object that was used when saving it.</param>
        /// <returns>The loaded object's state from storage, or default(T) if the storage object was not found.</returns>
        public static T Load<T>(string name)
        {
            if (isInitialized)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Name must not be null or empty.", "name");
                }
                if (name.Intersect(Path.GetInvalidFileNameChars()).Any())
                {
                    throw new ArgumentException("Name contains invalid characters.", "name");
                }

                if (!File.Exists(Path.Combine(Environment.ExpandEnvironmentVariables(storageLocation), name + ".xml")))
                {
                    return default(T);
                }

                return Deserialize<T>(File.ReadAllText(Path.Combine(Environment.ExpandEnvironmentVariables(storageLocation), name + ".xml")));
            }
            else
            {
                throw new InvalidOperationException("Unable to load object: The XmlStorageManager has not been initialized. (Try calling XmlStorageManager.Initialize())");
            }
        }

        /// <summary>
        /// Checks to see if a storage object exists or not.
        /// </summary>
        /// <param name="name">The arbitrary name of the stored object to check for.</param>
        /// <returns>True if the storage object exists, false otherwise.</returns>
        public static bool Exists(string name)
        {
            if (isInitialized)
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new ArgumentException("Name must not be null or empty.", "name");
                }
                if (name.Intersect(Path.GetInvalidFileNameChars()).Any())
                {
                    throw new ArgumentException("Name contains invalid characters.", "name");
                }

                if (!File.Exists(Path.Combine(Environment.ExpandEnvironmentVariables(storageLocation), name + ".xml")))
                {
                    return false;
                }

                return true;
            }
            else
            {
                throw new InvalidOperationException("Unable to load object: The XmlStorageManager has not been initialized. (Try calling XmlStorageManager.Initialize())");
            }
        }

        /// <summary>
        ///  Clears the current storage area of all persisted settings and files. This operation cannot be undone.
        /// </summary>
        public static void DeleteAll()
        {
            if (isInitialized)
            {
                DirectoryInfo di = new DirectoryInfo(Environment.ExpandEnvironmentVariables(storageLocation));

                foreach (FileInfo f in di.GetFiles())
                {
                    f.Delete();
                }
            }
            else
            {
                throw new InvalidOperationException("Unable to delete storage area: The XmlStorageManager has not been initialized. (Try calling XmlStorageManager.Initialize())");
            }
        }

        /// <summary>
        /// Permanently deletes a storage object. This operation cannot be undone.
        /// </summary>
        /// <param name="name">The name of the storage object to delete.</param>
        /// <remarks>If the storage object does not exist, no error is thrown.</remarks>
        public static void Delete(string name)
        {
            if (isInitialized)
            {
                DirectoryInfo di = new DirectoryInfo(Environment.ExpandEnvironmentVariables(storageLocation));
                string file = Path.Combine(Environment.ExpandEnvironmentVariables(storageLocation), name + ".xml");

                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Helper function to ensure storage area is read/write and all folders are set up accordingly. After calling
        /// this function, it is safe to read and write to the storage folder(s).
        /// </summary>
        private static void InitializeStorageArea()
        {
            if (!isInitialized)
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(Environment.ExpandEnvironmentVariables(storageLocation));
                    if (!di.Exists)
                    {
                        di.Create();
                    }
                    var fs = File.Create(Path.Combine(di.FullName, ".test"));
                    fs.Close();
                    File.Delete(Path.Combine(di.FullName, ".test"));

                    isInitialized = true;
                }
                catch (Exception ex)
                {
                    throw new StorageInitializationException("Unable to initialize local app storage.", ex);
                }
            }
        }

        /// <summary>
        /// Deserializes the specified XML string into type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of object expected when deserializing.</typeparam>
        /// <param name="xml">The string of XML data to deseriailze.</param>
        /// <returns>An object of type <typeparamref name="T" /> that was deserialized.</returns>
        private static T Deserialize<T>(string xml)
        {
            return (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(xml));
        }

        /// <summary>
        /// Serializes and returns the XML representation of the specified object.
        /// </summary>
        /// <param name="o">The object to serialize.</param>
        /// <returns>A string of XML data representing the serialized object.</returns>
        private static string Serialize(object o)
        {
            StringWriter sw = new StringWriter();
            new XmlSerializer(o.GetType()).Serialize(sw, o);
            return sw.ToString();
        }
    }
}
