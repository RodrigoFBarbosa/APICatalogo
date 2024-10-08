﻿using APICatalogo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalogo.DTOs;

public class ProductDTOUpdateResponse
{
    public int ProductId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public float Inventory { get; set; }
    public DateTime RegistrationDate { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
