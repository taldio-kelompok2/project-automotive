using AutoMapper;
using AutomotiveApp.Application.Helpers;
using AutomotiveApp.Base.Entities;

namespace AutomotiveApp.Application.Mapper.Resolver
{
    public class HeroImageUrlResolver<TSource, TDestination>(UrlGeneratorHelper urlHelper)
    : IValueResolver<TSource, TDestination, string?>
    where TSource : BaseEntity
    {
        public string? Resolve(TSource source, TDestination destination, string? destMember, ResolutionContext context)
        {
            var prop = source.GetType().GetProperty("HeroImageFileName");

            Console.WriteLine($"[HeroImageUrlResolver] type={typeof(TSource).Name}, filename={prop?.GetValue(source)}");

            if (prop == null) return null;

            return prop.GetValue(source) is string filename ? urlHelper.GeneratePublicUrl<TSource>(filename) : null;
        }
    }
}