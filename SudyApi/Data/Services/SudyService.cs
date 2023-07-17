using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StudandoApi.Data.Contexts;
using StudandoApi.Data.Interfaces;
using SudyApi.Data.Interfaces;
using SudyApi.Data.Repositories;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SudyApi.Data.Services
{
    public class SudyService : ISudyService
    {
        #region Field

        private readonly SudyContext _sudyContext;
        private readonly ICacheService _cacheService;

        #endregion

        #region Constructor

        public SudyService(SudyContext sudyContext, ICacheService cacheService)
        {
            _sudyContext = sudyContext;
            _cacheService = cacheService;
        }

        #endregion

        #region Repositories

        public IUserRepository UserRepository
        {
            get
            {
                return new UserRepository(_sudyContext, _cacheService);
            }
        }

        public IChapterRepository ChapterRepository
        {
            get
            {
                return new ChapterRepository(_sudyContext, _cacheService);
            }
        }

        public ISubjectRepository SubjectRepository
        {
            get
            {
                return new SubjectRepository(_sudyContext, _cacheService);
            }
        }

        public IUserInformationRepository UserInformationRepository
        {
            get
            {
                return new UserInformationRepository(_sudyContext, _cacheService);
            }
        }

        #endregion

        #region Methods

        public async Task Update<T>(T obj, bool removeCache = false, bool manualDesactiveCache = false)
        {
            bool cacheIsActivated = manualDesactiveCache ? false : bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            foreach (PropertyInfo item in obj.GetType().GetProperties())
            {
                if (item.PropertyType.IsClass && Type.GetTypeCode(item.PropertyType) != TypeCode.String)
                {
                    var p = typeof(T).GetProperty(item.Name).GetValue(obj);

                    if (p != null)
                        _sudyContext.Entry(p).State = EntityState.Modified;
                }

                if (cacheIsActivated && !item.PropertyType.IsClass)
                {
                    KeyAttribute attribute = Attribute.GetCustomAttribute(item, typeof(KeyAttribute)) as KeyAttribute;

                    if (attribute != null)
                    {
                        var p = typeof(T).GetProperty(item.Name).GetValue(obj);

                        _cacheService.Remove(item.ReflectedType.Name + p);

                        if (!removeCache)
                            _cacheService.Set(item.ReflectedType.Name + p, JsonConvert.SerializeObject(obj));
                    }
                }
            }

            _sudyContext.Entry(obj).State = EntityState.Modified;

            await _sudyContext.SaveChangesAsync();
        }

        public async Task UpdateMany<T>(List<T> obj, bool removeCache = false, bool manualDesactiveCache = false)
        {
            bool cacheIsActivated = manualDesactiveCache ? false : bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            foreach (T objItem in obj)
            {

                foreach (PropertyInfo item in objItem.GetType().GetProperties())
                {
                    if (item.PropertyType.IsClass && Type.GetTypeCode(item.PropertyType) != TypeCode.String)
                    {
                        var p = typeof(T).GetProperty(item.Name).GetValue(objItem);

                        if (p != null)
                            _sudyContext.Entry(p).State = EntityState.Modified;
                    }

                    if (cacheIsActivated && !item.PropertyType.IsClass)
                    {
                        KeyAttribute attribute = Attribute.GetCustomAttribute(item, typeof(KeyAttribute)) as KeyAttribute;

                        if (attribute != null)
                        {
                            var p = typeof(T).GetProperty(item.Name).GetValue(objItem);

                            _cacheService.Remove(item.ReflectedType.Name + p);

                            if (!removeCache)
                                _cacheService.Set(item.ReflectedType.Name + p, JsonConvert.SerializeObject(obj));
                        }
                    }
                }

                _sudyContext.Entry(objItem).State = EntityState.Modified;
            }

            await _sudyContext.SaveChangesAsync();
        }

        public async Task Create<T>(T obj, bool removeCache = false, bool manualDesactiveCache = false)
        {
            bool cacheIsActivated = manualDesactiveCache ? false : bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            _sudyContext.Add(obj);

            await _sudyContext.SaveChangesAsync();

            if (cacheIsActivated)
            {
                foreach (PropertyInfo item in obj.GetType().GetProperties())
                {
                    if (item.PropertyType.IsClass)
                        continue;

                    KeyAttribute attribute = Attribute.GetCustomAttribute(item, typeof(KeyAttribute)) as KeyAttribute;

                    if (attribute != null)
                    {
                        var p = typeof(T).GetProperty(item.Name).GetValue(obj);

                        _cacheService.Remove(item.ReflectedType.Name + p);

                        if (!removeCache)
                            _cacheService.Set(item.ReflectedType.Name + p, JsonConvert.SerializeObject(obj));
                    }

                }
            }
        }

        public async Task CreateMany<T>(List<T> obj, bool removeCache = false, bool manualDesactiveCache = false)
        {
            bool cacheIsActivated = manualDesactiveCache ? false : bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            foreach (T objItem in obj)
            {
                _sudyContext.Add(objItem);

                await _sudyContext.SaveChangesAsync();

                if (cacheIsActivated)
                {
                    foreach (PropertyInfo item in objItem.GetType().GetProperties())
                    {
                        if (item.PropertyType.IsClass)
                            continue;

                        KeyAttribute attribute = Attribute.GetCustomAttribute(item, typeof(KeyAttribute)) as KeyAttribute;

                        if (attribute != null)
                        {
                            var p = typeof(T).GetProperty(item.Name).GetValue(objItem);

                            _cacheService.Remove(item.ReflectedType.Name + p);

                            if (!removeCache)
                                _cacheService.Set(item.ReflectedType.Name + p, JsonConvert.SerializeObject(obj));
                        }

                    }
                }
            }
        }

        public async Task Delete<T>(T obj, bool manualDesactiveCache = false)
        {
            bool cacheIsActivated = manualDesactiveCache ? false : bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            foreach (PropertyInfo item in obj.GetType().GetProperties())
            {
                if (item.PropertyType.IsClass && Type.GetTypeCode(item.PropertyType) != TypeCode.String)
                {
                    var p = typeof(T).GetProperty(item.Name).GetValue(obj);

                    if (p != null)
                    {
                        item.SetValue(obj, null, null);
                        _sudyContext.Entry(obj).State = EntityState.Modified;
                    }
                }

                if (cacheIsActivated && !item.PropertyType.IsClass)
                {
                    KeyAttribute attribute = Attribute.GetCustomAttribute(item, typeof(KeyAttribute)) as KeyAttribute;

                    if (attribute != null)
                    {
                        var p = typeof(T).GetProperty(item.Name).GetValue(obj);

                        _cacheService.Remove(item.ReflectedType.Name + p);
                    }
                }
            }

            _sudyContext.Remove(obj);

            await _sudyContext.SaveChangesAsync();

        }

        public async Task DeleteMany<T>(List<T> objList, bool manualDesactiveCache = false)
        {
            bool cacheIsActivated = manualDesactiveCache ? false : bool.Parse(AppSettings.GetKey(ConfigKeys.RedisCache));

            foreach (T objItem in objList)
            {
                foreach (PropertyInfo item in objItem.GetType().GetProperties())
                {
                    if (item.PropertyType.IsClass && Type.GetTypeCode(item.PropertyType) != TypeCode.String)
                    {
                        var p = typeof(T).GetProperty(item.Name).GetValue(objItem);

                        if (p != null)
                        {
                            item.SetValue(objItem, null, null);
                            _sudyContext.Entry(objItem).State = EntityState.Modified;
                        }
                    }

                    if (cacheIsActivated && !item.PropertyType.IsClass)
                    {
                        KeyAttribute attribute = Attribute.GetCustomAttribute(item, typeof(KeyAttribute)) as KeyAttribute;

                        if (attribute != null)
                        {
                            var p = typeof(T).GetProperty(item.Name).GetValue(objItem);

                            _cacheService.Remove(item.ReflectedType.Name + p);
                        }
                    }
                }

                _sudyContext.Remove(objItem);
            }

            await _sudyContext.SaveChangesAsync();

        }

        #endregion
    }
}
