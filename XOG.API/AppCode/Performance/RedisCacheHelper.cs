﻿namespace XOG.Performance
{
    public static class RedisCacheHelper
    {
    //    static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
    //      {
    //          //return ConnectionMultiplexer.Connect("shopyzone.redis.cache.windows.net,ssl=true,password=MbeBeEQYMvBZNznKGPPy+ZZeHCJEJMQT92LWZ/VGO3Q=");
    //          return ConnectionMultiplexer.Connect(string.Format("{0},ssl={1},password={2}", AppConfig.RedisHost, AppConfig.RedisIsSsl, AppConfig.RedisPassword));
    //      });

    //    public static ConnectionMultiplexer Connection
    //    {
    //        get
    //        {
    //            return lazyConnection.Value;
    //        }
    //    }

    //    /// <summary>
    //    /// Insert value into the application state using
    //    /// appropriate name/value pairs
    //    /// </summary>
    //    /// <typeparam name="T">Type of application state item</typeparam>
    //    /// <param name="o">Item to be application state</param>
    //    /// <param name="key">Name of item</param>
    //    public static void Add<T>(string key, T o)
    //    {
    //        var cache = Connection.GetDatabase();

    //        cache.Set(key, o);
    //    }

    //    /// <summary>
    //    /// Remove item from application state
    //    /// </summary>
    //    /// <param name="key">Name of application state item</param>
    //    public static void Clear(string key)
    //    {
    //        var cache = Connection.GetDatabase();

    //        cache.KeyDelete(key);
    //    }

    //    /// <summary>
    //    /// Check for item in application state
    //    /// </summary>
    //    /// <param name="key">Name of application state item</param>
    //    /// <returns></returns>
    //    public static bool Exists(string key)
    //    {
    //        var cache = Connection.GetDatabase();

    //        return cache.KeyExists(key);
    //    }

    //    /// <summary>
    //    /// Retrieve application state item
    //    /// </summary>
    //    /// <typeparam name="T">Type of application state item</typeparam>
    //    /// <param name="key">Name of application state item</param>
    //    /// <param name="value">Cached value. Default(T) if item doesn't exist.</param>
    //    /// <returns>Cached item as type</returns>
    //    public static bool Get<T>(string key, out T value)
    //    {
    //        try
    //        {
    //            if (!Exists(key))
    //            {
    //                value = default(T);
    //                return false;
    //            }

    //            var cache = Connection.GetDatabase();

    //            value = (T)cache.Get(key);
    //        }
    //        catch (Exception)
    //        {
    //            value = default(T);
    //            return false;
    //        }

    //        return true;
    //    }

    //    public static T SetOrGet<T>(string key, T value)
    //    {
    //        if (!Exists(key))
    //        {
    //            Add(key, value);
    //        }
    //        else
    //        {
    //            var cache = Connection.GetDatabase();

    //            value = (T)cache.Get(key);
    //        }

    //        return value;
    //    }

    //    static T Deserialize<T>(byte[] stream)
    //    {
    //        if (stream == null)
    //        {
    //            return default(T);
    //        }

    //        var binaryFormatter = new BinaryFormatter();
    //        using (var memoryStream = new MemoryStream(stream))
    //        {
    //            return (T)binaryFormatter.Deserialize(memoryStream);
    //        }
    //    }

    //    static T Get<T>(this IDatabase cache, string key)
    //    {
    //        return Deserialize<T>(cache.StringGet(key));
    //    }

    //    static object Get(this IDatabase cache, string key)
    //    {
    //        return Deserialize<object>(cache.StringGet(key));
    //    }

    //    static byte[] Serialize(object o)
    //    {
    //        if (o == null)
    //        {
    //            return null;
    //        }

    //        var binaryFormatter = new BinaryFormatter();
    //        using (var memoryStream = new MemoryStream())
    //        {
    //            binaryFormatter.Serialize(memoryStream, o);
    //            return memoryStream.ToArray();
    //        }
    //    }

    //    static void Set(this IDatabase cache, string key, object value)
    //    {
    //        cache.StringSet(key, Serialize(value));
    //    }
   }
}
