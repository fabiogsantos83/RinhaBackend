using FluentValidation;
using RinhaBackend.Domain.Commands;
using System.Globalization;

namespace RinhaBackend.Domain.Validators
{
    public class AddPessoaRequestValidator : AbstractValidator<AddPessoaRequest>
    {
        public AddPessoaRequestValidator()
        {
            RuleFor(x => x.Apelido)
                .NotNull()
                .NotEmpty()
                .MaximumLength(32);

            RuleFor(x => x.Nome)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Nascimento)
                .Must(ValidateNascimento);

            RuleFor(x => x.Stack)
                .Must(ValidateStack);


        }

        private bool ValidateStack(IEnumerable<string> arg)
        {
            if (arg == null || arg.Count() == 0) return true;

            if (arg.Any(x => x.Length > 32)) return false;

            return true;
        }

        private bool ValidateNascimento(string arg)
        {
            DateTime dataNascimentoValida;

            if (!DateTime.TryParseExact(arg, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dataNascimentoValida))
            {
                return false;
            }

            return true;

        }
    }
}
