﻿using Grand.Business.Core.Extensions;
using Grand.Business.Core.Interfaces.Storage;
using Grand.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Widgets.Slider.Domain;
using Widgets.Slider.Models;
using Widgets.Slider.Services;

namespace Widgets.Slider.Components;

[ViewComponent(Name = "WidgetSlider")]
public class WidgetSliderComponent : ViewComponent
{
    private readonly IPictureService _pictureService;
    private readonly ISliderService _sliderService;
    private readonly IContextAccessor _contextAccessor;

    public WidgetSliderComponent(
        IPictureService pictureService,
        ISliderService sliderService,
        IContextAccessor contextAccessor)
    {
        _pictureService = pictureService;
        _sliderService = sliderService;
        _contextAccessor = contextAccessor;
    }

    private async Task<string> GetPictureUrl(string pictureId)
    {
        var url = await _pictureService.GetPictureUrl(pictureId, showDefaultPicture: false) ?? "";

        return url;
    }

    private async Task PrepareModel(IList<PictureSlider> sliders, PublicInfoModel model)
    {
        var i = 1;
        foreach (var item in sliders.OrderBy(x => x.DisplayOrder))
        {
            if ((item.StartDateUtc.HasValue && item.StartDateUtc > DateTime.UtcNow) ||
                (item.EndDateUtc.HasValue && item.EndDateUtc < DateTime.UtcNow))
                continue;

            model.Slide.Add(new PublicInfoModel.Slider {
                Link = item.Link,
                PictureUrl = await GetPictureUrl(item.PictureId),
                Name = item.GetTranslation(x => x.Name, _contextAccessor.WorkContext.WorkingLanguage.Id),
                Description = item.GetTranslation(x => x.Description, _contextAccessor.WorkContext.WorkingLanguage.Id),
                FullWidth = item.FullWidth,
                CssClass = i == 1 ? "active" : ""
            });
            i++;
        }
    }

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData = null)
    {
        var model = new PublicInfoModel();
        if (widgetZone == SliderWidgetDefaults.WidgetZoneHomePage)
        {
            var slides = await _sliderService.GetPictureSliders(SliderType.HomePage);
            await PrepareModel(slides, model);
        }

        if (widgetZone == SliderWidgetDefaults.WidgetZoneCategoryPage)
        {
            var slides = await _sliderService.GetPictureSliders(SliderType.Category, additionalData?.ToString());
            await PrepareModel(slides, model);
        }

        if (widgetZone == SliderWidgetDefaults.WidgetZoneCollectionPage)
        {
            var slides = await _sliderService.GetPictureSliders(SliderType.Collection, additionalData?.ToString());
            await PrepareModel(slides, model);
        }

        if (widgetZone == SliderWidgetDefaults.WidgetZoneBrandPage)
        {
            var slides = await _sliderService.GetPictureSliders(SliderType.Brand, additionalData?.ToString());
            await PrepareModel(slides, model);
        }

        if (!model.Slide.Any())
            return Content("");

        return View(model);
    }
}