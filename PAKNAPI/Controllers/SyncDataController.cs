using PAKNAPI.Common;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Linq;
using PAKNAPI.Models.Recommendation;

namespace PAKNAPI.Controllers
{
	[Route("api/SyncData")]
	[ApiController]
	public class SyncDataController : BaseApiController
	{
		private readonly IAppSetting _appSetting;

		public SyncDataController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
		}

		[Route("SyncKhanhHoa")]
		[HttpGet]
        [AllowAnonymous]
        public List<GopYKienNghi> SyncKhanhHoa()
        {
            HtmlWeb htmlWeb = new HtmlWeb()
            {
                AutoDetectEncoding = false,
                OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
            };//Load trang web, nạp html vào document
            HtmlDocument document = htmlWeb.Load("https://www.khanhhoa.gov.vn/module/gop-y");

            var items = new List<GopYKienNghi>();
            var threadItems = document.DocumentNode.Descendants("div")
                .First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "content-feedback")
                .ChildNodes.Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "row").First()
                .ChildNodes.Where(node => node.Attributes.Contains("class") && node.Attributes["class"].Value == "item col-xs-12 col-sm-6 col-md-4").ToList();

            foreach (var item in threadItems)
            {
                var objectAdd = new GopYKienNghi();
                var linkNode = item.Descendants("a").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("title"));
                var link = linkNode.Attributes["href"].Value;
                var textHead = linkNode.InnerText.Split("\r\n");
                objectAdd.Questioner = textHead[0].Trim();
                if (textHead.Length > 1)
                {
                    objectAdd.Question = textHead[1].Trim();
                }
                objectAdd.CreatedDate = item.Descendants("span").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("feedday")).InnerText;
                HtmlDocument documentDetail = htmlWeb.Load("https://www.khanhhoa.gov.vn/" + link);

                var threadItemsChild = documentDetail.DocumentNode.Descendants("div")
                .First(node => node.Attributes.Contains("id") && node.Attributes["id"].Value == "print-chitiet");
                objectAdd.QuestionContent = threadItemsChild.Descendants("div").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("chitietbaiviet")).InnerHtml;
                objectAdd.Reply = threadItemsChild.Descendants("div").First(node => node.Attributes.Contains("class") && node.Attributes["class"].Value.Contains("feedback-traloi-content")).InnerHtml.Trim();
                items.Add(objectAdd);
            }
            new RecommendationDAO(_appSetting).SyncKhanhHoa(items);
            return items;
        }
    }
}
