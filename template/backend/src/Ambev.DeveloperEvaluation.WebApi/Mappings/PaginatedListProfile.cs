using AutoMapper;
using PaginationCommon = Ambev.DeveloperEvaluation.Common.Pagination;
using PaginationWebApi = Ambev.DeveloperEvaluation.WebApi.Common;

namespace Ambev.DeveloperEvaluation.WebApi.Mappings;

/// <summary>
/// Profile for mapping PaginatedList between Application and WebApi layers
/// </summary>
public class PaginatedListProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for PaginatedList
    /// </summary>
    public PaginatedListProfile()
    {
        CreateMap(typeof(PaginationCommon.PaginatedList<>), typeof(PaginationWebApi.PaginatedList<>))
            .ConvertUsing(typeof(PaginatedListConverter<,>));
    }
}

/// <summary>
/// Custom converter for PaginatedList mapping
/// </summary>
/// <typeparam name="TSource">Source type</typeparam>
/// <typeparam name="TDestination">Destination type</typeparam>
public class PaginatedListConverter<TSource, TDestination> : ITypeConverter<PaginationCommon.PaginatedList<TSource>, PaginationWebApi.PaginatedList<TDestination>>
{
    public PaginationWebApi.PaginatedList<TDestination> Convert(PaginationCommon.PaginatedList<TSource> source, PaginationWebApi.PaginatedList<TDestination> destination, ResolutionContext context)
    {
        var mappedItems = source.Select(item => context.Mapper.Map<TDestination>(item)).ToList();
        return new PaginationWebApi.PaginatedList<TDestination>(
            mappedItems,
            source.TotalCount,
            source.CurrentPage,
            source.PageSize
        );
    }
}

