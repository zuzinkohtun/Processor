﻿using Moq;
using Microsoft.Extensions.Logging;
using ProcessorApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ProcessorApp.Test;

public class HomeControllerTest
{
    private readonly HomeController _homeController;

    public HomeControllerTest()
    {
        _homeController = new HomeController(Mock.Of<ILogger<HomeController>>());
        var httpContext = new DefaultHttpContext();
        var tempDataProvider = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>());
        _homeController.TempData = tempDataProvider;
    }

    [Fact]
    public void Compute_OddNumber_ShouldReturnOddNumbersResult()
    {
        string input = "1,2,3,4,5,6,7,8,9,10,11,12";
        string compute = "ListOdd";

        var result = _homeController.Compute(input,compute);

        var redirectToActionResult  = Assert.IsType<RedirectToActionResult>(result);
        Assert.NotNull(redirectToActionResult );
        Assert.Equal("Index",redirectToActionResult.ActionName);
        Assert.Equal("Odd Numbers: 1, 3, 5, 7, 9, 11", _homeController.TempData["ResultMessage"]);
    }

    [Fact]
    public void Compute_OddNumber_ShouldReturnNoOddNumberResult()
    {
        string input = "2,4,6,8,10,12";
        string compute = "ListOdd";

        var result = _homeController.Compute(input,compute);

        var redirectToActionResult  = Assert.IsType<RedirectToActionResult>(result);
        Assert.NotNull(redirectToActionResult );
        Assert.Equal("Index",redirectToActionResult.ActionName);
        Assert.Equal("No odd number found.", _homeController.TempData["ResultMessage"]);
    }

    [Fact]
    public void Compute_Sum_ShouldReturnSumResult(){
        string input = "1,2,3,4,5";
        string compute = "Sum";

        var result = _homeController.Compute(input,compute);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.NotNull(redirectToActionResult );
        Assert.Equal("Index",redirectToActionResult.ActionName);
        Assert.Equal("Sum: 15", _homeController.TempData["ResultMessage"]);
    }

    [Fact]
    public void Compute_Sum_ShouldReturnNegativeSumResult(){
        string input = "1,3,-7";
        string compute = "Sum";

        var result = _homeController.Compute(input,compute);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.NotNull(redirectToActionResult );
        Assert.Equal("Index",redirectToActionResult.ActionName);
        Assert.Equal("Sum: -3", _homeController.TempData["ResultMessage"]);
    }

    [Fact]
    public void Compute_Duplicate_ShouldReturnDuplicateResult(){
        string input = "1,2,3,4,5,2,4,7,0,1,2,5,5";
        string compute = "CountDuplicate";

        var result = _homeController.Compute(input,compute);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.NotNull(redirectToActionResult );
        Assert.Equal("Index",redirectToActionResult.ActionName);
        Assert.Equal("Duplicates: 1 (2 times), 2 (3 times), 4 (2 times), 5 (3 times)", _homeController.TempData["ResultMessage"]);
    }

    [Fact]
    public void Compute_Duplicate_ShouldReturnNoDuplicateResult(){
        string input = "1,2,3,4,5,7,0,8,9";
        string compute = "CountDuplicate";

        var result = _homeController.Compute(input,compute);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.NotNull(redirectToActionResult );
        Assert.Equal("Index",redirectToActionResult.ActionName);
        Assert.Equal("No duplicates found.", _homeController.TempData["ResultMessage"]);
    }

    [Fact]
    public void Compute_InvalidInput_ShouldReturnInvalidMessageResult(){
        string input = " ";
        string compute = "Sum";

        var result = _homeController.Compute(input,compute);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.NotNull(redirectToActionResult );
        Assert.Equal("Index",redirectToActionResult.ActionName);
        Assert.Equal("Invalid input.", _homeController.TempData["ResultMessage"]);
    }

    [Fact]
    public void Compute_WithNoInvalidNumber_ShouldReturnInvalidMessageResult(){
        string input = "a,b,c,*,-";
        string compute = "Sum";

        var result = _homeController.Compute(input,compute);

        var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        Assert.NotNull(redirectToActionResult );
        Assert.Equal("Index",redirectToActionResult.ActionName);
        Assert.Equal("Invalid input.", _homeController.TempData["ResultMessage"]);
    }
}
