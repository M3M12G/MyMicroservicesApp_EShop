﻿using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        //Task 2. -> OrderRepository:GetOrdersByUsername - completed
        public async Task<IEnumerable<Order>> GetOrdersByUsername(string username)
        {
            return await this.GetAsync(o => o.Username.Equals(username));
        }
    }
}
