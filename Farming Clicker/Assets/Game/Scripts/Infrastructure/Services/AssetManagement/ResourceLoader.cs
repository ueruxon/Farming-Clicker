using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Scripts.Infrastructure.Services.AssetManagement
{
    public static class ResourceLoader
    {
        public static T Load<T>(string filepath) where T : Object
        {
            var data = Resources.Load<T>(filepath);
            if(data is null)
                throw new NullReferenceException($"Asset of type {typeof(T)} from {filepath} can't be loaded");

            return data;
        }
        
        public static T[] LoadAll<T>(string path) where T : Object
        {
            var data = Resources.LoadAll<T>(path);
            if(data is null)
                throw new NullReferenceException($"Assets of type {typeof(T)} from {path} can't be loaded");

            return data;
        }
    }
}