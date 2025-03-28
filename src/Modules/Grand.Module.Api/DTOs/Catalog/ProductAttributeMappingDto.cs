﻿using Grand.Module.Api.Models;
using Grand.Domain.Catalog;

namespace Grand.Module.Api.DTOs.Catalog;

public class ProductAttributeMappingDto : BaseApiEntityModel
{
    public string ProductAttributeId { get; set; }
    public string TextPrompt { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public int? ValidationMinLength { get; set; }
    public int? ValidationMaxLength { get; set; }
    public string ValidationFileAllowedExtensions { get; set; }
    public int? ValidationFileMaximumSize { get; set; }
    public string DefaultValue { get; set; }
    public string ConditionAttributeXml { get; set; }
    public AttributeControlType AttributeControlTypeId { get; set; }

    public IList<ProductAttributeValueDto> ProductAttributeValues { get; set; } = new List<ProductAttributeValueDto>();
}

public class ProductAttributeValueDto : BaseApiEntityModel
{
    public string AssociatedProductId { get; set; }
    public string Name { get; set; }
    public string ColorSquaresRgb { get; set; }
    public string ImageSquaresPictureId { get; set; }
    public double PriceAdjustment { get; set; }
    public double WeightAdjustment { get; set; }
    public double Cost { get; set; }
    public int Quantity { get; set; }
    public bool IsPreSelected { get; set; }
    public int DisplayOrder { get; set; }
    public string PictureId { get; set; }
    public AttributeValueType AttributeValueTypeId { get; set; }
}