﻿using Microsoft.EntityFrameworkCore;
using Domain.Carts;
using Infrasctructure.Cache;
using Infrasctructure.Database;
using Infrasctructure.Repositories.Interfaces;

namespace Infrasctructure.Repositories.Classes;

public class CartRepository(ApplicationDbContext dbContext, ICacheEntityService cache) :
    BaseRepository<Cart>(dbContext, cache), ICartRepository
{

    public async Task<Cart?> GetByUserIdAsync(int id) =>
        await _dbContext.Carts
            .Include(c => c.Items)
            .ThenInclude(p => p.Product)
            .ThenInclude(p => p.Category)
            .FirstOrDefaultAsync(c => c.UserId == id);

    public async Task<bool> IsExistByUserIdAsync(int id) =>
        await _dbContext.Carts.AnyAsync(c => c.UserId == id);

    public async Task<Cart?> GetWithItemsAsync(int id) =>
        await _dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id);

    public async Task RemoveItemsAsync(int cartId) =>
        await _dbContext.CartItems
            .Where(ci => ci.CartId == cartId)
            .ExecuteDeleteAsync();

    public async Task<Cart?> GetWithItemsAndProductsAsync(int cartId)
    {
        return await _dbContext.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId);


    }

}
