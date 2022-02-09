using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using PAKNAPI.Common;

namespace PAKNAPI.Model
{
	public class RefreshTokens
	{
		private SQLCon _sQLCon;

		public RefreshTokens(IAppSetting appSetting)
		{
			_sQLCon = new SQLCon(appSetting.GetConnectstring());
		}

		public RefreshTokens()
		{
		}

		public async Task<int?> Indert(RefreshToken model)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Token", model.Token);
			DP.Add("Expires", model.Expires);
			DP.Add("Created", model.Created);
			DP.Add("UserId", model.UserId);
			DP.Add("IsUse", model.IsUse);
			DP.Add("Revoked", model.Revoked);
			DP.Add("JwtId", model.JwtId);

			return await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_RefreshToken_Insert", DP);
		}
		public async Task<int?> Update(RefreshToken model)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("Id", model.Id);
			DP.Add("Token", model.Token);
			DP.Add("Expires", model.Expires);
			DP.Add("Created", model.Created);
			DP.Add("UserId", model.UserId);
			DP.Add("IsUse", model.IsUse);
			DP.Add("Revoked", model.Revoked);
			DP.Add("JwtId", model.JwtId);

			return (await _sQLCon.ExecuteScalarDapperAsync<int?>("SY_RefreshToken_Update", DP));
		}
		public async Task<RefreshToken> GetByToken(string refreshToken)
		{
			DynamicParameters DP = new DynamicParameters();
			DP.Add("RefreshToken", refreshToken);

			return (await _sQLCon.ExecuteListDapperAsync<RefreshToken>("SY_RefreshToken_GetByToken", DP)).FirstOrDefault();
		}

		/// <example>
		/// { 
		///		"AccessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjMwMzg4IiwiVX
		///		Nlck5hbWUiOiJ0dF9jaHV5ZW52aWVuQGdtYWlsLmNvbSIsIkZ1bGxOYW1lIjoiVHJ1bmcgdMOibS
		///		IsIlR5cGUiOiIxIiwiVW5pdElkIjoiNTAiLCJFbWFpbCI6InR0X2NodXllbnZpZW5AZ21haWwuY2
		///		9tIiwianRpIjoiNTY2MDY2NzEtN2RkOC00NDIyLTg0YmQtOWE2Mzk4ZmI5ODBjIiwibmJmIjoxNj
		///		Q0MzExNDI4LCJleHAiOjE2NDUxNzU0MjgsImlhdCI6MTY0NDMxMTQyOCwiaXNzIjoiVGVzdC5jb2
		///		0iLCJhdWQiOiJUZXN0LmNvbSJ9.m9stQWrIBfU_7iK7gHUpad_hC7O6VU6UJcNyJXakcgg",
		///		"RefreshToken" : "QDX6/o3fmf77dWW1MvY7MX64ghTuBQh7fmFQlaizD6w5FXBv8i805DfXiC246baH7E1ls+tZFdrqi6DTakKZtw=="
		/// }
		/// </example>
		public class RefreshTokenRequest
		{
			[Required]
			public string AccessToken { get; set; }
			[Required]
			public string RefreshToken { get; set; }
		}
		public class RefreshToken
		{
			[Key]
			public int Id { get; set; }
			public string Token { get; set; }
			public DateTime Expires { get; set; }
			public bool IsExpired => DateTime.UtcNow >= Expires;
			public DateTime Created { get; set; }
			public long UserId { get; set; }
			public bool IsRevoked => Revoked != null;
			public DateTime? Revoked { get; set; }
			public bool IsUse { get; set; }
			public string JwtId { get; set; }
		}
	}
}
