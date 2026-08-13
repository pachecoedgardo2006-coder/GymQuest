using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GymQuest.Api.Filters;

/// <summary>
/// Filtro global de acción: antes de ejecutar cualquier método de un Controller,
/// busca si alguno de sus parámetros (los DTOs de entrada, ej. CrearUsuarioDto)
/// tiene un IValidator&lt;T&gt; registrado en Application y, si lo tiene, lo ejecuta.
///
/// Si la validación falla, corta la ejecución y responde 400 con los errores,
/// sin que el método del Controller siquiera llegue a ejecutarse.
///
/// Se registra una sola vez en Program.cs (AddControllers(options =>
/// options.Filters.Add&lt;ValidationFilter&gt;())) y aplica a TODOS los controllers,
/// sin tener que repetir código de validación en cada uno.
/// </summary>
public class ValidationFilter : IAsyncActionFilter
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationFilter(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        foreach (var argumento in context.ActionArguments.Values)
        {
            if (argumento is null)
            {
                continue;
            }

            var tipoValidator = typeof(IValidator<>).MakeGenericType(argumento.GetType());

            if (_serviceProvider.GetService(tipoValidator) is not IValidator validator)
            {
                continue; // Este DTO no tiene validator registrado (ej. tipos primitivos, Guid, etc.)
            }

            var contexto = new ValidationContext<object>(argumento);
            var resultado = await validator.ValidateAsync(contexto);

            if (!resultado.IsValid)
            {
                foreach (var error in resultado.Errors)
                {
                    context.ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
        }

        if (!context.ModelState.IsValid)
        {
            context.Result = new BadRequestObjectResult(context.ModelState);
            return;
        }

        await next();
    }
}