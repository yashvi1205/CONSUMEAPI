using System.Net;
using System.Text;
using CONSUMEAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CONSUMEAPI.Controllers;

public class CityController : Controller
{
    private Uri baseAddress = new Uri("http://localhost:5097/api");
    private readonly HttpClient _client;

    public CityController()
    {
        _client = new HttpClient();
        _client.BaseAddress = baseAddress;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region GetAllCities
    [HttpGet]
    public IActionResult CityList()
    {
        List<CityModel> city = new List<CityModel>();
        HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/City/GetAllCity").Result;
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            dynamic jsonobject = JsonConvert.DeserializeObject(data);
            var dataofObject = jsonobject;
            var extractedDataJson = JsonConvert.SerializeObject(dataofObject, Formatting.Indented);
            city = JsonConvert.DeserializeObject<List<CityModel>>(extractedDataJson); 
        }
        return View("CityList", city);
    }
    #endregion

    #region Delete

    [HttpGet]

    public IActionResult Delete(int? CityID)
    {
        HttpResponseMessage response =
            _client.DeleteAsync($"{_client.BaseAddress}/City/DeleteCity/{CityID}").Result;
        if (response.IsSuccessStatusCode)
        {
            TempData["Message"] = "State Deleted Successfully!";
        }

        return RedirectToAction("CityList");
    }

    #endregion

    #region AddCity
    
    [HttpPost]
    public async Task<IActionResult> CityAddEdit([FromForm] CityModel modelCity)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(modelCity);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                if (modelCity.CityID == 0) // Insert operation
                {
                    response = await _client.PostAsync($"{_client.BaseAddress}/City/InsertCity", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "City Inserted Successfully!";
                        ViewBag.CountryList = CountryDropDown();
                        // change
                        return RedirectToAction("CityList");
                    }
                }
                else // Update operation
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/City/UpdateCity/{modelCity.CityID}",
                        content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "City Updated Successfully!";
                        ViewBag.CountryList = CountryDropDown();
                        return RedirectToAction("CityList");
                    }
                    else if (response.StatusCode == HttpStatusCode.NoContent) // Handle 204 No Content response
                    {
                        TempData["Message"] = "State Updated Successfully! No content returned.";
                        ViewBag.CountryList = CountryDropDown();
                        return RedirectToAction("CityList");
                    }
                }
            }
            ViewBag.CountryList = CountryDropDown();
        }
        catch (Exception e)
        {
            TempData["Error"] = "An error occurred: " + e.Message;
        }
        // change
        return RedirectToAction("CityList");
    }
    #endregion
    
    [HttpGet]
    public async Task<IActionResult> CityAdd(int? CityID)
    {
        CityModel cityModel = new CityModel();
        HttpResponseMessage response =
            _client.GetAsync($"{_client.BaseAddress}/City/GetByPK/{CityID}").Result;
        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(data);
            var extractedData = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            cityModel = JsonConvert.DeserializeObject<CityModel>(extractedData);
        }
        ViewBag.CountryList = CountryDropDown();
        return View("CityForm", cityModel);
    }
    
    // change
    #region Country DropDown
    [HttpGet]
    public List<CountryDropDown> CountryDropDown()
    {
        List<CountryDropDown> cityDropDownModels = new List<CountryDropDown>();
        HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/City/GetCountries/countries").Result;
        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(data);
            var extractedData = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            cityDropDownModels = JsonConvert.DeserializeObject<List<CountryDropDown>>(extractedData);
        }
        return cityDropDownModels;
    }
    #endregion

    #region getstatebycountry

    [HttpPost]
    public async Task<JsonResult> GetStatesByCountry(int CountryID)
    {
        var states = await GetStatesByCountryID(CountryID);
        return Json(states);
    }
    
    private async Task<List<StateDropDownModel>> GetStatesByCountryID(int CountryID)
    {
        var response = await _client.GetAsync($"City/GetStatesByCountryID/states/{CountryID}");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<StateDropDownModel>>(data);
        }
        return new List<StateDropDownModel>();
    }

    #endregion
    
    
}