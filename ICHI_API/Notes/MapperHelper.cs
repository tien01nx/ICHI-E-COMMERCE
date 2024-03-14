using AutoMapper;

namespace ICHI_CORE.Helpers
{
    public static class MapperHelper
    {
        /// <summary>
        /// Map voi object moi
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T2 Map<T1, T2>(T1 obj)
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<T1, T2>().ReverseMap());
            var autoMapper = mapperConfig.CreateMapper();
            return autoMapper.Map<T1, T2>(obj);
        }

        public static List<T2> MapList<T1, T2>(List<T1> obj)
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<T1, T2>().ReverseMap());
            var autoMapper = mapperConfig.CreateMapper();
            return autoMapper.Map<List<T1>, List<T2>>(obj);
        }
        /// <summary>
        /// Map voi object co san
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="obj"></param>
        /// <param name="desObj"></param>
        public static void Map<T1, T2>(T1 obj, T2 desObj)
        {
            var mapperConfig = new MapperConfiguration(cfg => cfg.CreateMap<T1, T2>().ReverseMap());
            var autoMapper = mapperConfig.CreateMapper();
            autoMapper.Map<T1, T2>(obj, desObj);
        }
    }
}
