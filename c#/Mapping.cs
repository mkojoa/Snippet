public static class Mapping
    {
        public static TTarget onMap<TSource, TTarget>(TSource value)
            where TSource : class
            where TTarget : class
        {
            var config = new MapperConfiguration(cfg =>
                cfg.CreateMap<TSource, TTarget>());

            var mapper = new Mapper(config);
            var result = mapper.Map<TSource, TTarget>(value);

            return result;
        }

        public static T MapTo<T>(this DbDataReader dr) where T : new()
        {
            var MaptoListObj = MapToList<T>(dr);
            if (MaptoListObj != null)
            {
                return MaptoListObj.FirstOrDefault();
            }
            return default;
        }

        public static List<T> MapToList<T>(this DbDataReader dr) where T : new()
        {
            if (dr != null && dr.HasRows)
            {
                var entity = typeof(T);
                var entities = new List<T>();
                var propDict = new Dictionary<string, PropertyInfo>();
                var props = entity.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                propDict = props.ToDictionary(p => p.Name.ToUpper(), p => p);
                while (dr.Read())
                {
                    T newObject = new T();
                    for (int index = 0; index < dr.FieldCount; index++)
                    {
                        if (propDict.ContainsKey(dr.GetName(index).ToUpper()))
                        {
                            var info = propDict[dr.GetName(index).ToUpper()];
                            if ((info != null) && info.CanWrite)
                            {
                                var val = dr.GetValue(index);
                                info.SetValue(newObject, (val == DBNull.Value) ? null : val, null);
                            }
                        }
                    }
                    entities.Add(newObject);
                }
                return entities;
            }
            return null;
        }
    }
