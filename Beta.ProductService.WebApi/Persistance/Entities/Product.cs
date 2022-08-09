using Beta.ProductService.WebApi.Common;
using Beta.ProductService.WebApi.Exceptions;

namespace Beta.ProductService.WebApi.Persistance.Entities;

public class Product : BaseEntity<long>
{
    public string Name { get; private set; }

    #region Constructors

    private Product() { }

    public Product(string name)
    {
        this.Name = ValidateName(name);
    }

    #endregion


    #region Validation

    private string ValidateName(string name)
    {
        if (String.IsNullOrWhiteSpace(name))
        {
            throw new BadRequestException("Name of the product is required");
        }

        return name;
    }

    #endregion

    #region Setters

    public void SetName(string name)
    {
        this.Name = ValidateName(name);
    }

    #endregion
}