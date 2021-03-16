using PAKNAPI.Controllers;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace PAKNAPI.Models.Results
{
	public class ReturnResultApi : BaseApiController
	{
		//public HttpResponseMessage ReturnResulApiWithMessage(ActionResult req, ResultApi result)
		//{
		//	var response1 = req.CreateResponse(HttpStatusCode.OK);
		//	var jsonstring1 = JsonConvert.SerializeObject(result);
		//	response1.Content = new StringContent(jsonstring1, Encoding.UTF8, "application/json");
		//	return response1;
		//}

		//public HttpResponseMessage ReturnResulApiWithObject(ActionResult req, Object obj)
		//{
		//	var response1 = req.CreateResponse(HttpStatusCode.OK);
		//	var jsonstring1 = JsonConvert.SerializeObject(obj);
		//	response1.Content = new StringContent(jsonstring1, Encoding.UTF8, "application/json");
		//	return response1;
		//}

		//public HttpResponseMessage ReturnResulApiWithJson(HttpRequestMessage req, string json)
		//{
		//	var response1 = req.CreateResponse(HttpStatusCode.OK);
		//	response1.Content = new StringContent(json, Encoding.UTF8, "application/json");
		//	return response1;
		//}
	}
}