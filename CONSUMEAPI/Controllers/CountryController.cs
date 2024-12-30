using Microsoft.AspNetCore.Mvc;
using CONSUMEAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace CONSUMEAPI.Controllers;


public class CountryController : Controller
{
    private Uri baseAddress = new Uri("http://localhost:5097/api");
    private readonly HttpClient _client;

    public CountryController()
    {
        _client = new HttpClient();
        _client.BaseAddress = baseAddress;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region GetAllCountry
    [HttpGet]
    public IActionResult CountryList()
    {
        List<CountryModel> countries = new List<CountryModel>();
        HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/Country/GetAllCountry").Result;
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            dynamic jsonobject = JsonConvert.DeserializeObject(data);
            var dataofObject = jsonobject;
            var extractedDataJson = JsonConvert.SerializeObject(dataofObject, Formatting.Indented);
            countries = JsonConvert.DeserializeObject<List<CountryModel>>(extractedDataJson); 
        }
        return View("CountryList", countries);
    }
    #endregion

    #region Delete

    [HttpGet]

    public IActionResult Delete(int CountryID)
    {
        HttpResponseMessage response =
            _client.DeleteAsync($"{_client.BaseAddress}/Country/DeleteCountry/{CountryID}").Result;
        if (response.IsSuccessStatusCode)
        {
            TempData["Message"] = "Country Deleted Successfully!";
        }

        return RedirectToAction("CountryList");
    }

    #endregion

    #region AddCountry

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> CountryAddEdit([FromForm] CountryModel modelCountry)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(modelCountry);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                if (modelCountry.CountryID == 0) // Insert operation
                {
                    response = await _client.PostAsync($"{_client.BaseAddress}/Country/InsertCountry", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Country Inserted Successfully!";
                        return RedirectToAction("CountryList");
                    }
                }
                else // Update operation
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/Country/UpdateCountry/{modelCountry.CountryID}", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Country Updated Successfully!";
                        return RedirectToAction("CountryList");
                    }
                    else if (response.StatusCode == HttpStatusCode.NoContent) // Handle 204 No Content response
                    {
                        TempData["Message"] = "Country Updated Successfully! No content returned.";
                        return RedirectToAction("CountryList");
                    }
                }
            }
        }
        catch (Exception e)
        {
            TempData["Error"] = "An error occurred: " + e.Message;
        }

        return View("CountryForm", modelCountry);
    }
    #endregion
    
    [HttpGet]
    public IActionResult CountryAdd(int CountryID)
    {
        CountryModel countryModel = new CountryModel();
        HttpResponseMessage response =
            _client.GetAsync($"{_client.BaseAddress}/Country/GetByPK/{CountryID}").Result;
        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(data);
            // var dataOfObject = jsonObject.data;
            var extractedData = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            countryModel = JsonConvert.DeserializeObject<CountryModel>(extractedData);
        }

        return View("CountryForm", countryModel);
    }
   
}
