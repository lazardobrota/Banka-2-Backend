using Bank.Application.Domain;
using Bank.UserService.Database;
using Bank.UserService.Models;

using Microsoft.EntityFrameworkCore;

namespace Bank.UserService.Repositories;

public interface ICountryRepository
{
    Task<Page<Country>> FindAll(Pageable pageable);

    Task<Country?> FindById(Guid id);

    Task<Country> Add(Country country);

    Task<Country> Update(Country oldCountry, Country country);
}

public class CountryRepository(ApplicationContext context) : ICountryRepository
{
    private readonly ApplicationContext m_Context = context;

    public async Task<Page<Country>> FindAll(Pageable pageable)
    {
        var countryQuery = m_Context.Countries.AsQueryable();

        int totalElements = await countryQuery.CountAsync();

        var countries = await countryQuery.Skip((pageable.Page - 1) * pageable.Size)
                                          .Take(pageable.Size)
                                          .ToListAsync();

        return new Page<Country>(countries, pageable.Page, pageable.Size, totalElements);
    }

    public async Task<Country?> FindById(Guid id)
    {
        return await m_Context.Countries.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Country> Add(Country country)
    {
        var addedCountry = await m_Context.Countries.AddAsync(country);

        await m_Context.SaveChangesAsync();

        return addedCountry.Entity;
    }

    public async Task<Country> Update(Country oldCountry, Country country)
    {
        m_Context.Countries.Entry(oldCountry)
                 .State = EntityState.Detached;

        var updatedCountry = m_Context.Countries.Update(country);

        await m_Context.SaveChangesAsync();

        return updatedCountry.Entity;
    }
}
