using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace LogsVaivoa.Models
{
    public class Log
    {
        public string Nome { get; set; }
        public string Mensagem { get; set; }
        public string Detalhe { get; set; }
        private List<ValidationFailure> _errors; 
        
        public List<ValidationFailure> GetErrors() => _errors ??= new LogModelValidation().Validate(this).Errors;
        public bool IsValid() => !GetErrors().Any();
    }

    public class LogModelValidation : AbstractValidator<Log>
    {
        public LogModelValidation()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .MaximumLength(50).WithMessage("O campo {PropertyName} deve ter no maximo 50 caracteres");

            RuleFor(c => c.Mensagem)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .MaximumLength(250).WithMessage("O campo {PropertyName} deve ter no maximo 50 caracteres");

            RuleFor(c => c.Detalhe)
                .MaximumLength(1000).WithMessage("O campo {PropertyName} deve ter no maximo 50 caracteres");

        }
    }
    
}