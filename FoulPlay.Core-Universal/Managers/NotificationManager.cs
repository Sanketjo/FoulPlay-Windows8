﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Foulplay_Windows8.Core.Entities;
using Newtonsoft.Json;

namespace Foulplay_Windows8.Core.Managers
{
    public class NotificationManager
    {
        public async Task<NotificationEntity> GetNotifications(string username, UserAccountEntity userAccountEntity)
        {
            try
            {
                var authenticationManager = new AuthenticationManager();
                var user = userAccountEntity.GetUserEntity();
                if (userAccountEntity.GetAccessToken().Equals("refresh"))
                {
                    await authenticationManager.RefreshAccessToken(userAccountEntity);
                }
                string url = string.Format("https://{0}-ntl.np.community.playstation.net/notificationList/v1/users/{1}/notifications?fields=@default%2Cmessage%2CactionUrl&npLanguage={2}", user.Region, username, user.Language);
                var theAuthClient = new HttpClient();
                // TODO: Fix this cheap hack to get around caching issue. For some reason, no-cache is not working...
                url += "&r=" + Guid.NewGuid();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userAccountEntity.GetAccessToken());
                request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
                HttpResponseMessage response = await theAuthClient.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseContent))
                {
                    return null;
                }
                var notification = JsonConvert.DeserializeObject<NotificationEntity>(responseContent);
                return notification;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static async Task<bool> ClearNotification(NotificationEntity.Notification notification,
            UserAccountEntity userAccountEntity)
        {
            try
            {
                var authenticationManager = new AuthenticationManager();
                var user = userAccountEntity.GetUserEntity();
                if (userAccountEntity.GetAccessToken().Equals("refresh"))
                {
                    await authenticationManager.RefreshAccessToken(userAccountEntity);
                }
                string url = string.Format("https://{0}-ntl.np.community.playstation.net/notificationList/v1/users/{1}/notifications/{2}/{3}", user.Region, user.OnlineId, notification.NotificationGroup, notification.NotificationId);
                var theAuthClient = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Put, url)
                {
                    Content = new StringContent("{\"seenFlag\":true}", Encoding.UTF8, "application/json")
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", userAccountEntity.GetAccessToken());
                request.Headers.CacheControl = new CacheControlHeaderValue { NoCache = true };
                HttpResponseMessage response = await theAuthClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
