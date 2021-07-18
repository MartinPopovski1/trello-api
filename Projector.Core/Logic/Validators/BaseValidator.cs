namespace Projector.Core.Logic.Validators
{
    public class BaseValidator
    {
        public BaseValidator()
        {
            ThrowException = true;
        }
        public bool ThrowException { get; set; }
    }
}