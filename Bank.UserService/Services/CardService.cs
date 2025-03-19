﻿using Bank.Application.Domain;
using Bank.Application.Endpoints;
using Bank.Application.Queries;
using Bank.Application.Requests;
using Bank.Application.Responses;
using Bank.UserService.Mappers;
using Bank.UserService.Repositories;

using Microsoft.EntityFrameworkCore;

using Npgsql;

namespace Bank.UserService.Services;

public interface ICardService
{
    Task<Result<Page<CardResponse>>> GetAll(CardFilterQuery userFilterQuery, Pageable pageable);

    Task<Result<CardResponse>> GetOne(Guid id);

    Task<Result<CardResponse>> Create(CardCreateRequest request);

    Task<Result<CardResponse>> Update(CardStatusUpdateRequest request, Guid id);

    Task<Result<CardResponse>> Update(CardLimitUpdateRequest request, Guid id);
}

public class CardService(ICardRepository repository, ICardTypeRepository cardTypeRepository, IAccountRepository accountRepository) : ICardService
{
    private readonly ICardRepository     m_CardRepository     = repository;
    private readonly ICardTypeRepository m_CardTypeRepository = cardTypeRepository;
    private readonly IAccountRepository  m_AccountRepository  = accountRepository;

    public async Task<Result<Page<CardResponse>>> GetAll(CardFilterQuery cardFilterQuery, Pageable pageable)
    {
        var cards = await m_CardRepository.FindAll(cardFilterQuery, pageable);

        var result = cards.Items.Select(card => card.ToResponse())
                          .ToList();

        return Result.Ok(new Page<CardResponse>(result, cards.PageNumber, cards.PageSize, cards.TotalElements));
    }

    public async Task<Result<CardResponse>> GetOne(Guid id)
    {
        var card = await m_CardRepository.FindById(id);

        if (card == null)
            return Result.NotFound<CardResponse>();

        return Result.Ok(card.ToResponse());
    }

    public async Task<Result<CardResponse>> Create(CardCreateRequest cardCreateRequest)
    {
        var cardType = await m_CardTypeRepository.FindById(cardCreateRequest.CardTypeId);

        if (cardType == null)
            return Result.BadRequest<CardResponse>();

        var account = await m_AccountRepository.FindById(cardCreateRequest.AccountId);

        if (account == null)
            return Result.BadRequest<CardResponse>();

        const int maxRetries = 5;

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                var cardToCreate = cardCreateRequest.ToCard(cardType, account);

                var card = await m_CardRepository.Add(cardToCreate);

                return Result.Ok(card.ToResponse());
            }
            catch (DbUpdateException ex) when (IsDuplicateKeyException(ex))
            {
                if (attempt >= maxRetries - 1)
                    return Result.BadRequest<CardResponse>("Failed to generate a unique card number after multiple attempts");

                await Task.Delay(50 * (attempt + 1));
            }
        }

        return Result.BadRequest<CardResponse>("Unexpected error creating card");
    }

    public async Task<Result<CardResponse>> Update(CardStatusUpdateRequest request, Guid id)
    {
        var oldCard = await m_CardRepository.FindById(id);

        if (oldCard is null)
            return Result.NotFound<CardResponse>($"No Card found with Id: {id}");

        var card = await m_CardRepository.Update(oldCard, request.ToCard(oldCard));

        return Result.Ok(card.ToResponse());
    }

    public async Task<Result<CardResponse>> Update(CardLimitUpdateRequest request, Guid id)
    {
        var oldCard = await m_CardRepository.FindById(id);

        if (oldCard is null)
            return Result.NotFound<CardResponse>($"No Card found with Id: {id}");

        var card = await m_CardRepository.Update(oldCard, request.ToCard(oldCard));

        return Result.Ok(card.ToResponse());
    }

    private bool IsDuplicateKeyException(DbUpdateException ex)
    {
        // For PostgreSQL
        return ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505";
    }
}
