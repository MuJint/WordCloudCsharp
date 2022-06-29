using System;

namespace WordCloudCsharp
{
    /// <summary>
    /// singletion service
    /// <para>单例服务</para>
    /// </summary>
    public interface ISingletion<T> where T : class
    {
        private static T? _instance;
        /// <summary>
        /// get instance
        /// <para>获取单例</para>
        /// </summary>
        static T Instance
        {
            get
            {
                if(_instance is null)
                {
                    lock (typeof(T))
                    {
                        _instance ??= Activator.CreateInstance<T>();
                    }
                }
                return _instance;
            }
        }
    }
}
