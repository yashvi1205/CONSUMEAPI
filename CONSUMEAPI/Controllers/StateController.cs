using System.Net;
using System.Text;
using CONSUMEAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CONSUMEAPI.Controllers;

public class StateController : Controller
{
    private Uri baseAddress = new Uri("http://localhost:5097/api");
    private readonly HttpClient _client;

    public StateController()
    {
        _client = new HttpClient();
        _client.BaseAddress = baseAddress;
    }

    public IActionResult Index()
    {
        return View();
    }

    #region GetAllState
    [HttpGet]
    public IActionResult StateList()
    {
        List<StateModel> state = new List<StateModel>();
        HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/State/GetAllState").Result;
        if (response.IsSuccessStatusCode)
        {
            var data = response.Content.ReadAsStringAsync().Result;
            dynamic jsonobject = JsonConvert.DeserializeObject(data);
            var dataofObject = jsonobject;
            var extractedDataJson = JsonConvert.SerializeObject(dataofObject, Formatting.Indented);
            state = JsonConvert.DeserializeObject<List<StateModel>>(extractedDataJson); 
        }
        return View("StateList", state);
    }
    #endregion

    #region Delete

    [HttpGet]

    public IActionResult Delete(int StateID)
    {
        HttpResponseMessage response =
            _client.DeleteAsync($"{_client.BaseAddress}/State/DeleteState/{StateID}").Result;
        if (response.IsSuccessStatusCode)
        {
            TempData["Message"] = "State Deleted Successfully!";
        }

        return RedirectToAction("StateList");
    }

    #endregion

    #region AddState
    
    [HttpPost]
    public async Task<IActionResult> StateAddEdit([FromForm] StateModel modelState)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(modelState);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response;
                if (modelState.StateID == 0) // Insert operation
                {
                    response = await _client.PostAsync($"{_client.BaseAddress}/State/InsertState", content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "State Inserted Successfully!";
                        ViewBag.CountryList = CountryDropDown();
                        // change
                        return RedirectToAction("StateList");
                    }
                }
                else // Update operation
                {
                    response = await _client.PutAsync($"{_client.BaseAddress}/State/UpdateState/{modelState.StateID}",
                        content);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "State Updated Successfully!";
                        ViewBag.CountryList = CountryDropDown();
                        return RedirectToAction("StateList");
                    }
                    else if (response.StatusCode == HttpStatusCode.NoContent) // Handle 204 No Content response
                    {
                        TempData["Message"] = "State Updated Successfully! No content returned.";
                        ViewBag.CountryList = CountryDropDown();
                        return RedirectToAction("StateList");
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
        return RedirectToAction("StateList");
    }
    #endregion
    
    [HttpGet]
    public async Task<IActionResult> StateAdd(int? StateID)
    {
        StateModel stateModel = new StateModel();
        HttpResponseMessage response =
            _client.GetAsync($"{_client.BaseAddress}/State/GetByPK/{StateID}").Result;
        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(data);
            var extractedData = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            stateModel = JsonConvert.DeserializeObject<StateModel>(extractedData);
        }
        ViewBag.CountryList = CountryDropDown();
        return View("StateForm", stateModel);
    }
    
    // change
    #region Country DropDown
    [HttpGet]
    public List<CountryDropDownModel> CountryDropDown()
    {
        List<CountryDropDownModel> stateDropDownModels = new List<CountryDropDownModel>();
        HttpResponseMessage response = _client.GetAsync($"{_client.BaseAddress}/State/StateDropDown").Result;
        if (response.IsSuccessStatusCode)
        {
            string data = response.Content.ReadAsStringAsync().Result;
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(data);
            var extractedData = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            stateDropDownModels = JsonConvert.DeserializeObject<List<CountryDropDownModel>>(extractedData);
        }
        return stateDropDownModels;
    }
    #endregion
    
    
}