using Godot;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandHeightGame.api
{
	public class MapDataFetcher
	{
		#region Fields
		private string _url;
		#endregion
		#region Constructors
		public MapDataFetcher(string url = "https://jobfair.nordeus.com/jf24-fullstack-challenge/test")
		{
			_url = url;
		}
		#endregion
		#region Methods
		public async Task<RestResponse> FetchMapDataAsync()
		{
			var request = new RestRequest(_url, Method.Get);
			var response = await Http.Client.ExecuteAsync(request);
			return response;
		}
		#endregion
	}
}
