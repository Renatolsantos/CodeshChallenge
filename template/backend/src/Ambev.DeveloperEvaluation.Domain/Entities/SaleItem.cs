using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item within a sale.
/// </summary>
public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets or sets the sale this item belongs to.
    /// </summary>
    public Guid SaleId { get; set; }

    /// <summary>
    /// Gets or sets the product for this sale item.
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Gets or sets the quantity of the product.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets the unit price of the product.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Gets or sets the discount percentage applied to this item.
    /// </summary>
    public decimal DiscountPercentage { get; set; }

    /// <summary>
    /// Gets or sets the total price after discount.
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Gets or sets whether this item is cancelled.
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Initializes a new instance of the SaleItem class.
    /// </summary>
    public SaleItem()
    {
    }

    /// <summary>
    /// Initializes a new instance of the SaleItem class with specified product and quantity.
    /// </summary>
    /// <param name="product">The product being sold.</param>
    /// <param name="quantity">The quantity of the product.</param>
    public SaleItem(Product product, int quantity)
    {
        Product = product;
        UnitPrice = product.Price;
        Quantity = ValidateAndGetQuantity(quantity);
        DiscountPercentage = CalculateDiscountPercentage(Quantity);
        CalculateTotalPrice();
    }

    /// <summary>
    /// Validates the quantity against business rules and returns the valid quantity.
    /// </summary>
    /// <param name="quantity">The quantity to validate.</param>
    /// <returns>The validated quantity.</returns>
    /// <exception cref="ArgumentException">Thrown when quantity exceeds the maximum limit.</exception>
    private int ValidateAndGetQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");
        
        if (quantity > 20)
            throw new ArgumentException("Cannot sell more than 20 identical items.");
            
        return quantity;
    }

    /// <summary>
    /// Calculates the discount percentage based on the quantity.
    /// </summary>
    /// <param name="quantity">The quantity of items.</param>
    /// <returns>The discount percentage.</returns>
    private decimal CalculateDiscountPercentage(int quantity)
    {
        if (quantity < 4)
            return 0;
            
        if (quantity >= 10 && quantity <= 20)
            return 0.2m; // 20% discount
            
        return 0.1m; // 10% discount
    }

    /// <summary>
    /// Calculates the total price after discount.
    /// </summary>
    public void CalculateTotalPrice()
    {
        if (IsCancelled)
        {
            TotalPrice = 0;
            return;
        }
        
        var subtotal = UnitPrice * Quantity;
        var discount = subtotal * DiscountPercentage;
        TotalPrice = subtotal - discount;
    }

    /// <summary>
    /// Updates the quantity of this item and recalculates the discount and total price.
    /// </summary>
    /// <param name="quantity">The new quantity.</param>
    public void UpdateQuantity(int quantity)
    {
        Quantity = ValidateAndGetQuantity(quantity);
        DiscountPercentage = CalculateDiscountPercentage(Quantity);
        CalculateTotalPrice();
    }

    /// <summary>
    /// Cancels this item.
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
        CalculateTotalPrice();
    }
}