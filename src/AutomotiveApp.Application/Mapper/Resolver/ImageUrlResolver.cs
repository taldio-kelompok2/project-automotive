using AutoMapper;
using AutomotiveApp.Application.Helpers;
using AutomotiveApp.Base.Entities;

namespace AutomotiveApp.Application.Mapper.Resolver
{
    public class ImageUrlResolver<TSource, TDestination>(UrlGeneratorHelper urlHelper)
    : IValueResolver<TSource, TDestination, string?>
    where TSource : BaseEntity
    {
        public string? Resolve(TSource source, TDestination destination, string? destMember, ResolutionContext context)
        {
            var prop = source.GetType().GetProperty("ImageFileName");

            if (prop == null) return null;

            return prop.GetValue(source) is string filename ? urlHelper.GeneratePublicUrl<TSource>(filename) : null;
        }
    }
}