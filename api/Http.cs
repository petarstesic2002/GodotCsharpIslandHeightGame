using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IslandHeightGame.api
{
	public static class Http
	{
		#region Fields
		private static RestClient _client;
		#endregion
		#region Properties
		public static RestClient Client
		{
			get
			{
				if(_client == null)
					_client = new RestClient();
				return _client;
			}
		}
		#endregion
	}
}
