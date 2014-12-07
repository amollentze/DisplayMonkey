﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml;
using DisplayMonkey.Language;


namespace DisplayMonkey
{
	public class Weather : Frame
	{
		public Weather(int frameId, int panelId, int displayId, int woeid, string tempUnit)
		{
			PanelId = panelId;
			_templatePath = HttpContext.Current.Server.MapPath("~/files/frames/weather.htm");
			string sql = string.Format(
				"SELECT TOP 1 * FROM Weather WHERE FrameId={0};", 
				frameId
				);

			using (DataSet ds = DataAccess.RunSql(sql))
			{
				if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					DataRow dr = ds.Tables[0].Rows[0];
					FrameId = DataAccess.IntOrZero(dr["FrameId"]);
				}
			}

			Woeid = woeid;
			TemperatureUnit = tempUnit;
			if (Woeid == 0 || string.IsNullOrEmpty(TemperatureUnit))
			{
				Location location = new Location(displayId);
				Woeid = location.Woeid;
				TemperatureUnit = location.TemperatureUnit;
			}
		}

        private string GetYahooWeather(string temperatureUnit, int woeid)
        {
            // get repsonse from yahoo
            // TODO: switch to YQL: http://query.yahooapis.com/v1/public/yql?q=select+*+from+weather.forecast+where+woeid%3D2502265+and+u%3D%22c%22
            string url = string.Format(
                @"http://weather.yahooapis.com/forecastrss?u={0}&w={1}",
                TemperatureUnit,
                Woeid
                );
            using (WebClient client = new WebClient())
            {
                return Encoding.ASCII.GetString(client.DownloadData(url));
            }
        }
        
        public override string Payload
		{
			get
			{
				string html = Resources.ErrorNoWeatherDataWasRetrieved;
				try
				{
					// load template
					string template = File.ReadAllText(_templatePath);

					// fill template
                    string response = "", key = string.Format("weather_{0}_{1}", TemperatureUnit, Woeid);
                    response = HttpRuntime.Cache.GetOrAddAbsolute(
                        key,
                        () => { return GetYahooWeather(TemperatureUnit, Woeid); },
                        DateTime.UtcNow.AddMinutes(1)
                        );

                    //Regex rex1 = new Regex(@"(?<=\<!\[CDATA\[)\s*(?:.(?<!\]\]>)\s*)*(?=\]\]>)");

					// get data strips
					int j=0;
					Dictionary<string, string> map = new Dictionary<string, string>();
					MatchCollection strips = new Regex(@"(?<=<yweather:).+(?=/>)").Matches(response);
					foreach (Match strip in strips)
					{
						int i=0;
						string strip_name = "";
						MatchCollection fields = new Regex("(^\\w+(?=\\s))|(\\w+=\"[^\"]*\")").Matches(strip.Value);
						foreach (Match field in fields)
						{
							if (0 == i++)
								strip_name = field.Value;
							else
							{
								string[] pair = field.Value.Split(new char[] {'='});
								if (strip_name == "forecast")
									strip_name += (++j).ToString();
								map.Add(string.Format("{0}_{1}", strip_name, pair[0]), pair[1].Replace("\"", ""));
							}
						}
					}

					// populate template
					if (map.Count > 0)
					{
                        html = template.Replace( "{{frame_id}}",
                            FrameId.ToString()
                            );
                        foreach (string k in map.Keys)
						{
							html = html.Replace(string.Format("{{{0}}}", k), map[k]);
						}
					}

					else
					{
						// something went wrong...
						XmlDocument doc = new XmlDocument();
						doc.LoadXml(response);
                        HttpRuntime.Cache[key] = null;
                        XmlNode nodeDescription = doc.SelectSingleNode("//channel/item/description");
						if (nodeDescription != null)
						{
							html = nodeDescription.InnerText;
						}
                    }
				}

				catch (Exception ex)
				{
					html = ex.Message;
				}

				// return html
				return html;
			}
		}

		/*
			<style type="text/css" scoped>
			...
			</style>
		 * */
        public int Woeid = 56199578;            // Old Sacramento
		public string TemperatureUnit = "f";
	}
}