﻿using Ledger.Activations.Application.AppServices;
using Ledger.Activations.Domain.Aggregates.ActivationAggregate;
using Ledger.Shared.Notifications;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Ledger.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/foods")]
    public class ActivationController : BaseController
    {
        private readonly IActivationApplicationService _activationAppService;

        public ActivationController(IDomainNotificationHandler domainNotificationHandler, IActivationApplicationService activationAppServiceService) 
                                                                                                                : base(domainNotificationHandler)
        {
            _activationAppService = activationAppServiceService;
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            Activation activation = _activationAppService.GetByCompanyId(id);

            return Ok(activation);
        }
    }
}
