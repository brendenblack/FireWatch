using Firewatch.Application.Common.Interfaces;
using Firewatch.Application.Investments.Commands.ParseAndImportTrades;
using Firewatch.Application.Investments.Queries.GetTradeExecutions;
using Firewatch.Application.Investments.Queries.GetTrades;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Firewatch.WebUI.Controllers
{

    [Authorize]
    public class InvestmentsController : ApiController
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IMediator _mediator;

        public InvestmentsController(ICurrentUserService currentUserService, IMediator mediator)
        {
            _currentUserService = currentUserService;
            _mediator = mediator;
        }

        public class ParseTradesModel
        {
            public string Content { get; set; }

            public string Format { get; set; }
        }

        [HttpPost]
        public async Task<ActionResult<ParseAndImportTradesResponse>> ImportTrades(ParseTradesModel model)
        {
            var command = new ParseAndImportTradesCommand
            {
                Contents = model.Content,
                Format = model.Format,
                OwnerId = _currentUserService.UserId,
                RequestorId = _currentUserService.UserId
            };

            return await _mediator.Send(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from">An optional date filter, limiting results to only values on or later than the date specified. In yyyyMMdd format.</param>
        /// <param name="to">An optional date filter, limiting results to only values on or earlier than the date specified. In yyyyMMdd format.</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<GetTradesVm>> GetTrades([FromQuery] string? from, [FromQuery] string? to)
        {
            var query = new GetTradesQuery
            {
                OwnerId = _currentUserService.UserId,
                RequestorId = _currentUserService.UserId,
                From = from.ToDate(),
                To = to.ToDate()
            };

            return await _mediator.Send(query);
        }

        [HttpGet("executions")]
        public async Task<ActionResult<TradeExecutionsVm>> GetExecutions([FromQuery] string? from = "", [FromQuery] string? to = "")
        {
            var query = new GetTradeExecutionsQuery
            {
                OwnerId = _currentUserService.UserId,
                RequestorId = _currentUserService.UserId,
                From = from.ToDate(),
                To = to.ToDate()
            };

            return await _mediator.Send(query);
        }

        [HttpGet("{accountNumber}/executions")]
        public async Task<ActionResult<TradeExecutionsVm>> GetExecutionsForAccount([FromRoute] string accountNumber, [FromQuery] string? from = "", [FromQuery] string? to = "")
        {
            var query = new GetTradeExecutionsQuery
            {
                OwnerId = _currentUserService.UserId,
                RequestorId = _currentUserService.UserId,
                From = from.ToDate(),
                To = to.ToDate(),
                AccountNumber = accountNumber
            };

            return await _mediator.Send(query);
        }
    }
}
