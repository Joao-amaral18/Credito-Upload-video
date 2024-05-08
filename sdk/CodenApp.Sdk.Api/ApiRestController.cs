using CodenApp.Sdk.Infrastructure.Abstraction.Bus;
using CodenApp.Sdk.Domain.Notifications;
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
    public class ApiRestController : ControllerBase
    {

        private readonly IMediatorHandler mediator;
        private readonly DomainNotificationHandler domainNotification;

        public ApiRestController(
            INotificationHandler<DomainNotification> domainNotification,
            IMediatorHandler mediator)
        {
            this.mediator = mediator;
            this.domainNotification = (DomainNotificationHandler)domainNotification;
        }

        #region [IActionResult] sync

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
            return new JsonResult(new ResultJson(true, data, messages));
        }

        protected static IActionResult ResultOk<TData>(TData data)
            where TData : class
        {
            return new JsonResult(new ResultJson(true, data));
        }

        private IActionResult ResultErro(IEnumerable<string> messages)
        {
            return ResultErro("Erro!", messages);
        }

        private IActionResult ResultErro<TData>(TData data, IEnumerable<string> messages)
            where TData : class
        {
            var result = new JsonResult(new ResultJson(false, data, messages.Distinct()));
            result.StatusCode = (int)HttpStatusCode.BadRequest;
            return result;
            //return BadRequest(new JsonResult(new Response<TData>(data, false, HttpStatusCode.BadRequest, messages.Distinct())));
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

    public class ResultJson
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public ResultJson(bool success, object data, IEnumerable<string> errors)
        {
            this.Success = success;
            this.Data = data;
            this.Errors = errors;
        }
        public ResultJson(bool success, object data)
        {
            this.Success = success;
            this.Data = data;
        }
    }
}
