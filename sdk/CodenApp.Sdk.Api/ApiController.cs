using CodenApp.Sdk.Domain.Notifications;
using CodenApp.Sdk.Infrastructure.Abstraction.Bus;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CodenApp.Sdk.Api
{
    [Authorize]
    public class ApiController : ControllerBase
    {

        private readonly IMediatorHandler mediator;
        private readonly DomainNotificationHandler domainNotification;

        public ApiController(
            INotificationHandler<DomainNotification> domainNotification,
            IMediatorHandler mediator)
        {
            this.mediator = mediator;
            this.domainNotification = (DomainNotificationHandler)domainNotification;
        }

        #region [IActionResult] sync

        protected IActionResult RestResult<TData>(TData data) 
            where TData : class
        {
            if (this.IsValidOperation)
            {
                return RestResultOk(data);
            }

            return RestResultErro(data, this.domainNotification.GetNotifications().Select(s => s.Value));
        }


        protected IActionResult Result<TData>(TData data)
            where TData : class
        {
            if (this.IsValidOperation)
            {
                return ResultOk(data);
            }

            return ResultErro(data, this.domainNotification.GetNotifications().Select(s => s.Value));
        }

        protected IActionResult Result()
        {
            if (this.IsValidOperation)
            {
                return ResultOk();
            }

            return ResultErro(this.domainNotification.GetNotifications().Select(s => s.Value));
        }

        protected static IActionResult ResultOk()
        {
            return ResultOk("Sucesso!");
        }

        protected static IActionResult ResultOk<TData>(TData data, IEnumerable<string> messages)
            where TData : class
        {
            return new JsonResult(new Response<TData>(data, true, HttpStatusCode.OK, messages));
        }

        protected static IActionResult ResultOk<TData>(TData data)
            where TData : class
        {
            return new JsonResult(new Response<TData>(data, true, HttpStatusCode.OK));
        }

        protected static IActionResult RestResultOk<TData>(TData data)
            where TData : class
        {
            return new JsonResult(new ResultJson(true, data));
        }

        private IActionResult RestResultErro(IEnumerable<string> messages)
        {
            return RestResultErro("Erro!", messages);
        }
        private IActionResult ResultErro(IEnumerable<string> messages)
        {
            return ResultErro("Erro!", messages);
        }

        private IActionResult RestResultErro<TData>(TData data, IEnumerable<string> messages)
            where TData : class
        {
            var result = new JsonResult(new ResultJson(false, data, messages.Distinct()));
            result.StatusCode = (int)HttpStatusCode.BadRequest;
            return result;
        }

        private IActionResult ResultErro<TData>(TData data, IEnumerable<string> messages)
            where TData : class
        {
            return new JsonResult(new Response<TData>(data, false, HttpStatusCode.BadRequest, messages.Distinct()));
            // return BadRequest(new JsonResult(new Response<TData>(data, false, HttpStatusCode.BadRequest, messages.Distinct())));
        }

        #endregion

        #region [IActionResult] async 

        protected static Task<IActionResult> ResultOkAsync()
        {
            return Task.FromResult(ResultOk());
        }

        protected static Task<IActionResult> ResultOkAsync<TData>(TData data, IEnumerable<string> messages)
            where TData : class
        {
            return Task.FromResult(ResultOk(data, messages));
        }

        protected static Task<IActionResult> ResultOkAsync<TData>(TData data)
            where TData : class
        {
            return Task.FromResult(ResultOk(data));
        }

        protected Task<IActionResult> ResultErroAsync(IEnumerable<string> messages)
        {
            return Task.FromResult(ResultErro(messages));
        }

        protected Task<IActionResult> ResultErroAsync<TData>(TData data, IEnumerable<string> messages)
            where TData : class
        {
            return Task.FromResult(ResultErro(data, messages));
        }

        #endregion

        #region protected

        protected IEnumerable<DomainNotification> Notifications => this.domainNotification.GetNotifications();

        protected bool IsValidOperation => !this.domainNotification.HasNotification();

        protected void NotifyError(string code, string message)
        {
            this.mediator.RaiseEventAsync(new DomainNotification(code, message));
        }

        #endregion
    }
}
