using PAKNAPI.Common;
using PAKNAPI.Controllers;
using PAKNAPI.Models;
using PAKNAPI.ModelBase;
using PAKNAPI.Models.Results;
using System;
using Dapper;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace PAKNAPI.ControllerBase
{
	[Route("api/MRTableBase")]
	[ApiController]
	public class MRTableBaseController : BaseApiController
	{
		private readonly IAppSetting _appSetting;
		public MRTableBaseController(IAppSetting appSetting)
		{
			_appSetting = appSetting;
		}

		#region MRRecommendation

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationGetByID")]
		public async Task<ActionResult<object>> MRRecommendationGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendation(_appSetting).MRRecommendationGetByID(Id) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationGetAll")]
		public async Task<ActionResult<object>> MRRecommendationGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendation(_appSetting).MRRecommendationGetAll() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationGetAllOnPage")]
		public async Task<ActionResult<object>> MRRecommendationGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<MRRecommendationOnPage> rsMRRecommendationOnPage = await new MRRecommendation(_appSetting).MRRecommendationGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendation", rsMRRecommendationOnPage},
						{"TotalCount", rsMRRecommendationOnPage != null && rsMRRecommendationOnPage.Count > 0 ? rsMRRecommendationOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationOnPage != null && rsMRRecommendationOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationOnPage != null && rsMRRecommendationOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationInsert")]
		public async Task<ActionResult<object>> MRRecommendationInsert(MRRecommendation _mRRecommendation)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendation(_appSetting).MRRecommendationInsert(_mRRecommendation) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationListInsert")]
		public async Task<ActionResult<object>> MRRecommendationListInsert(List<MRRecommendation> _mRRecommendations)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendation _mRRecommendation in _mRRecommendations)
				{
					int? result = await new MRRecommendation(_appSetting).MRRecommendationInsert(_mRRecommendation);
					if (result != null)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationUpdate")]
		public async Task<ActionResult<object>> MRRecommendationUpdate(MRRecommendation _mRRecommendation)
		{
			try
			{
				int count = await new MRRecommendation(_appSetting).MRRecommendationUpdate(_mRRecommendation);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationDelete")]
		public async Task<ActionResult<object>> MRRecommendationDelete(MRRecommendation _mRRecommendation)
		{
			try
			{
				int count = await new MRRecommendation(_appSetting).MRRecommendationDelete(_mRRecommendation);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationListDelete")]
		public async Task<ActionResult<object>> MRRecommendationListDelete(List<MRRecommendation> _mRRecommendations)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendation _mRRecommendation in _mRRecommendations)
				{
					var result = await new MRRecommendation(_appSetting).MRRecommendationDelete(_mRRecommendation);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationDeleteAll")]
		public async Task<ActionResult<object>> MRRecommendationDeleteAll()
		{
			try
			{
				int count = await new MRRecommendation(_appSetting).MRRecommendationDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion MRRecommendation

		#region MRRecommendationConclusion

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationConclusionGetByID")]
		public async Task<ActionResult<object>> MRRecommendationConclusionGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionGetByID(Id) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationConclusionGetAll")]
		public async Task<ActionResult<object>> MRRecommendationConclusionGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionGetAll() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationConclusionGetAllOnPage")]
		public async Task<ActionResult<object>> MRRecommendationConclusionGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<MRRecommendationConclusionOnPage> rsMRRecommendationConclusionOnPage = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationConclusion", rsMRRecommendationConclusionOnPage},
						{"TotalCount", rsMRRecommendationConclusionOnPage != null && rsMRRecommendationConclusionOnPage.Count > 0 ? rsMRRecommendationConclusionOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationConclusionOnPage != null && rsMRRecommendationConclusionOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationConclusionOnPage != null && rsMRRecommendationConclusionOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionInsert")]
		public async Task<ActionResult<object>> MRRecommendationConclusionInsert(MRRecommendationConclusion _mRRecommendationConclusion)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionInsert(_mRRecommendationConclusion) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionListInsert")]
		public async Task<ActionResult<object>> MRRecommendationConclusionListInsert(List<MRRecommendationConclusion> _mRRecommendationConclusions)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationConclusion _mRRecommendationConclusion in _mRRecommendationConclusions)
				{
					int? result = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionInsert(_mRRecommendationConclusion);
					if (result != null)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionUpdate")]
		public async Task<ActionResult<object>> MRRecommendationConclusionUpdate(MRRecommendationConclusion _mRRecommendationConclusion)
		{
			try
			{
				int count = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionUpdate(_mRRecommendationConclusion);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionDelete")]
		public async Task<ActionResult<object>> MRRecommendationConclusionDelete(MRRecommendationConclusion _mRRecommendationConclusion)
		{
			try
			{
				int count = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionDelete(_mRRecommendationConclusion);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionListDelete")]
		public async Task<ActionResult<object>> MRRecommendationConclusionListDelete(List<MRRecommendationConclusion> _mRRecommendationConclusions)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationConclusion _mRRecommendationConclusion in _mRRecommendationConclusions)
				{
					var result = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionDelete(_mRRecommendationConclusion);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionDeleteAll")]
		public async Task<ActionResult<object>> MRRecommendationConclusionDeleteAll()
		{
			try
			{
				int count = await new MRRecommendationConclusion(_appSetting).MRRecommendationConclusionDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion MRRecommendationConclusion

		#region MRRecommendationConclusionFiles

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationConclusionFilesGetByID")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesGetByID(Id) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationConclusionFilesGetAll")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesGetAll() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationConclusionFilesGetAllOnPage")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<MRRecommendationConclusionFilesOnPage> rsMRRecommendationConclusionFilesOnPage = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationConclusionFiles", rsMRRecommendationConclusionFilesOnPage},
						{"TotalCount", rsMRRecommendationConclusionFilesOnPage != null && rsMRRecommendationConclusionFilesOnPage.Count > 0 ? rsMRRecommendationConclusionFilesOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationConclusionFilesOnPage != null && rsMRRecommendationConclusionFilesOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationConclusionFilesOnPage != null && rsMRRecommendationConclusionFilesOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionFilesInsert")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesInsert(MRRecommendationConclusionFiles _mRRecommendationConclusionFiles)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesInsert(_mRRecommendationConclusionFiles) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionFilesListInsert")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesListInsert(List<MRRecommendationConclusionFiles> _mRRecommendationConclusionFiless)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationConclusionFiles _mRRecommendationConclusionFiles in _mRRecommendationConclusionFiless)
				{
					int? result = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesInsert(_mRRecommendationConclusionFiles);
					if (result != null)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionFilesUpdate")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesUpdate(MRRecommendationConclusionFiles _mRRecommendationConclusionFiles)
		{
			try
			{
				int count = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesUpdate(_mRRecommendationConclusionFiles);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionFilesDelete")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesDelete(MRRecommendationConclusionFiles _mRRecommendationConclusionFiles)
		{
			try
			{
				int count = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesDelete(_mRRecommendationConclusionFiles);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionFilesListDelete")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesListDelete(List<MRRecommendationConclusionFiles> _mRRecommendationConclusionFiless)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationConclusionFiles _mRRecommendationConclusionFiles in _mRRecommendationConclusionFiless)
				{
					var result = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesDelete(_mRRecommendationConclusionFiles);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationConclusionFilesDeleteAll")]
		public async Task<ActionResult<object>> MRRecommendationConclusionFilesDeleteAll()
		{
			try
			{
				int count = await new MRRecommendationConclusionFiles(_appSetting).MRRecommendationConclusionFilesDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion MRRecommendationConclusionFiles

		#region MRRecommendationFiles

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationFilesGetByID")]
		public async Task<ActionResult<object>> MRRecommendationFilesGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesGetByID(Id) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationFilesGetAll")]
		public async Task<ActionResult<object>> MRRecommendationFilesGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesGetAll() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationFilesGetAllOnPage")]
		public async Task<ActionResult<object>> MRRecommendationFilesGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<MRRecommendationFilesOnPage> rsMRRecommendationFilesOnPage = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationFiles", rsMRRecommendationFilesOnPage},
						{"TotalCount", rsMRRecommendationFilesOnPage != null && rsMRRecommendationFilesOnPage.Count > 0 ? rsMRRecommendationFilesOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationFilesOnPage != null && rsMRRecommendationFilesOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationFilesOnPage != null && rsMRRecommendationFilesOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationFilesInsert")]
		public async Task<ActionResult<object>> MRRecommendationFilesInsert(MRRecommendationFiles _mRRecommendationFiles)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesInsert(_mRRecommendationFiles) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationFilesListInsert")]
		public async Task<ActionResult<object>> MRRecommendationFilesListInsert(List<MRRecommendationFiles> _mRRecommendationFiless)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationFiles _mRRecommendationFiles in _mRRecommendationFiless)
				{
					int? result = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesInsert(_mRRecommendationFiles);
					if (result != null)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationFilesUpdate")]
		public async Task<ActionResult<object>> MRRecommendationFilesUpdate(MRRecommendationFiles _mRRecommendationFiles)
		{
			try
			{
				int count = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesUpdate(_mRRecommendationFiles);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationFilesDelete")]
		public async Task<ActionResult<object>> MRRecommendationFilesDelete(MRRecommendationFiles _mRRecommendationFiles)
		{
			try
			{
				int count = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesDelete(_mRRecommendationFiles);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationFilesListDelete")]
		public async Task<ActionResult<object>> MRRecommendationFilesListDelete(List<MRRecommendationFiles> _mRRecommendationFiless)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationFiles _mRRecommendationFiles in _mRRecommendationFiless)
				{
					var result = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesDelete(_mRRecommendationFiles);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationFilesDeleteAll")]
		public async Task<ActionResult<object>> MRRecommendationFilesDeleteAll()
		{
			try
			{
				int count = await new MRRecommendationFiles(_appSetting).MRRecommendationFilesDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion MRRecommendationFiles

		#region MRRecommendationForward

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationForwardGetByID")]
		public async Task<ActionResult<object>> MRRecommendationForwardGetByID(int? Id)
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForward(_appSetting).MRRecommendationForwardGetByID(Id) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationForwardGetAll")]
		public async Task<ActionResult<object>> MRRecommendationForwardGetAll()
		{
			try
			{
				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForward(_appSetting).MRRecommendationForwardGetAll() };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpGet]
		[Authorize]
		[Route("MRRecommendationForwardGetAllOnPage")]
		public async Task<ActionResult<object>> MRRecommendationForwardGetAllOnPage(int PageSize, int PageIndex)
		{
			try
			{
				List<MRRecommendationForwardOnPage> rsMRRecommendationForwardOnPage = await new MRRecommendationForward(_appSetting).MRRecommendationForwardGetAllOnPage(PageSize, PageIndex);
				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"MRRecommendationForward", rsMRRecommendationForwardOnPage},
						{"TotalCount", rsMRRecommendationForwardOnPage != null && rsMRRecommendationForwardOnPage.Count > 0 ? rsMRRecommendationForwardOnPage[0].RowNumber : 0},
						{"PageIndex", rsMRRecommendationForwardOnPage != null && rsMRRecommendationForwardOnPage.Count > 0 ? PageIndex : 0},
						{"PageSize", rsMRRecommendationForwardOnPage != null && rsMRRecommendationForwardOnPage.Count > 0 ? PageSize : 0},
					};
				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationForwardInsert")]
		public async Task<ActionResult<object>> MRRecommendationForwardInsert(MRRecommendationForward _mRRecommendationForward)
		{
			try
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = await new MRRecommendationForward(_appSetting).MRRecommendationForwardInsert(_mRRecommendationForward) };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationForwardListInsert")]
		public async Task<ActionResult<object>> MRRecommendationForwardListInsert(List<MRRecommendationForward> _mRRecommendationForwards)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationForward _mRRecommendationForward in _mRRecommendationForwards)
				{
					int? result = await new MRRecommendationForward(_appSetting).MRRecommendationForwardInsert(_mRRecommendationForward);
					if (result != null)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationForwardUpdate")]
		public async Task<ActionResult<object>> MRRecommendationForwardUpdate(MRRecommendationForward _mRRecommendationForward)
		{
			try
			{
				int count = await new MRRecommendationForward(_appSetting).MRRecommendationForwardUpdate(_mRRecommendationForward);
				if (count > 0)
				{
					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationForwardDelete")]
		public async Task<ActionResult<object>> MRRecommendationForwardDelete(MRRecommendationForward _mRRecommendationForward)
		{
			try
			{
				int count = await new MRRecommendationForward(_appSetting).MRRecommendationForwardDelete(_mRRecommendationForward);
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationForwardListDelete")]
		public async Task<ActionResult<object>> MRRecommendationForwardListDelete(List<MRRecommendationForward> _mRRecommendationForwards)
		{
			try
			{
				int count = 0;
				int errcount = 0;
				foreach (MRRecommendationForward _mRRecommendationForward in _mRRecommendationForwards)
				{
					var result = await new MRRecommendationForward(_appSetting).MRRecommendationForwardDelete(_mRRecommendationForward);
					if (result > 0)
					{
						count++;
					}
					else
					{
						errcount++;
					}
				}

				IDictionary<string, object> json = new Dictionary<string, object>
					{
						{"CountSuccess", count},
						{"CountError", errcount}
					};
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

				return new ResultApi { Success = ResultCode.OK, Result = json };
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		[HttpPost]
		[Authorize]
		[Route("MRRecommendationForwardDeleteAll")]
		public async Task<ActionResult<object>> MRRecommendationForwardDeleteAll()
		{
			try
			{
				int count = await new MRRecommendationForward(_appSetting).MRRecommendationForwardDeleteAll();
				if (count > 0)
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.OK, Result = count };
				}
				else
				{
					new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, null);

					return new ResultApi { Success = ResultCode.ORROR, Message = ResultMessage.ORROR };
				}
			}
			catch (Exception ex)
			{
				new LogHelper(_appSetting).ProcessInsertLogAsync(HttpContext, ex);

				return new ResultApi { Success = ResultCode.ORROR, Message = ex.Message };
			}
		}

		#endregion MRRecommendationForward
	}
}
