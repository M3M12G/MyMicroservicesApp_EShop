using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.DTOs;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        //Task 3. -> OrderController:GetOrdersByUsername - completed
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetOrdersByUsername(string username)
        {
            // check if username is empty or null
            if (string.IsNullOrEmpty(username)) {
                return BadRequest();
            }

            var ordersOfUser = await _orderRepository.GetOrdersByUsername(username);
            
            //check whether user with that username did any order
            if (ordersOfUser == null)
            {
                return NotFound();
            }
            // do mapping of orders to order response objects as enumerable
            var ordersToReturn = _mapper.Map<IEnumerable<OrderResponse>>(ordersOfUser);
            return Ok(ordersToReturn);
        }
    }
}
