﻿using UnityEngine;
using System;
using System.Collections;
using UnityEditor;
using PlayFab.Editor.EditorModels;

using System.Collections.Generic;
using System.Linq;

namespace PlayFab.Editor
{
    public class PlayFabEditorApi
    {
#region FROM EDITOR API SETS ----------------------------------------------------------------------------------------------------------------------------------------

        public static void RegisterAccouint(RegisterAccountRequest request, Action<RegisterAccountResult> resultCallback,
            Action<EditorModels.PlayFabError> errorCb)
        {
            PlayFabEditorHttp.MakeApiCall("/DeveloperTools/User/RegisterAccount", PlayFabEditorHelper.DEV_API_ENDPOINT, request, resultCallback, errorCb);            
        }

        public static void Login(LoginRequest request, Action<LoginResult> resultCallback,
            Action<EditorModels.PlayFabError> errorCb)
        {
            PlayFabEditorHttp.MakeApiCall("/DeveloperTools/User/Login", PlayFabEditorHelper.DEV_API_ENDPOINT, request, resultCallback, errorCb);
        }
       
        public static void Logout(LogoutRequest request, Action<LogoutResult> resultCallback,
            Action<EditorModels.PlayFabError> errorCb)
        {
            PlayFabEditorHttp.MakeApiCall("/DeveloperTools/User/Logout", PlayFabEditorHelper.DEV_API_ENDPOINT, request, resultCallback, errorCb);
        }

        public static void GetStudios(GetStudiosRequest request, Action<GetStudiosResult> resultCallback,
            Action<EditorModels.PlayFabError> errorCb)
        {
            var token = PlayFabEditorDataService.accountDetails.devToken;
            request.DeveloperClientToken = token;
            PlayFabEditorHttp.MakeApiCall("/DeveloperTools/User/GetStudios", PlayFabEditorHelper.DEV_API_ENDPOINT, request, resultCallback, errorCb);
        }

        public static void CreateTitle(CreateTitleRequest request, Action<RegisterAccountResult> resultCallback,
            Action<EditorModels.PlayFabError> errorCb)
        {
            var token = PlayFabEditorDataService.accountDetails.devToken;
            request.DeveloperClientToken = token;
            PlayFabEditorHttp.MakeApiCall("/DeveloperTools/User/CreateTitle", PlayFabEditorHelper.DEV_API_ENDPOINT, request, resultCallback, errorCb);
        }

#endregion




#region FROM ADMIN / SERVER API SETS ----------------------------------------------------------------------------------------------------------------------------------------
        public static void GetTitleData( Action<GetTitleDataResult> resultCb, Action<EditorModels.PlayFabError> errorCb)
        {
            var titleId = PlayFabEditorDataService.envDetails.selectedTitleId;
            var apiEndpoint = String.Format("https://{0}{1}", titleId, PlayFabEditorHelper.TITLE_ENDPOINT);
            PlayFabEditorHttp.MakeApiCall<GetTitleDataRequest, GetTitleDataResult>("/Admin/GetTitleData", apiEndpoint, new GetTitleDataRequest(), resultCb, errorCb);
        }

        public static void SetTitleData(Dictionary<string, string> keys, Action<SetTitleDataResult> resultCb, Action<EditorModels.PlayFabError> errorCb)
        {
            foreach(var pair in keys)
            {
                var req = new SetTitleDataRequest() { Key = pair.Key, Value = pair.Value };

                var titleId = PlayFabEditorDataService.envDetails.selectedTitleId;
                var apiEndpoint = String.Format("https://{0}{1}", titleId, PlayFabEditorHelper.TITLE_ENDPOINT);
                PlayFabEditorHttp.MakeApiCall<SetTitleDataRequest, SetTitleDataResult>("/Admin/SetTitleData", apiEndpoint, req, resultCb, errorCb);
            }
        }
        public static void GetTitleInternalData( Action<GetTitleDataResult> resultCb, Action<EditorModels.PlayFabError> errorCb)
        {
            var titleId = PlayFabEditorDataService.envDetails.selectedTitleId;
            var apiEndpoint = String.Format("https://{0}{1}", titleId, PlayFabEditorHelper.TITLE_ENDPOINT);
            PlayFabEditorHttp.MakeApiCall<GetTitleDataRequest, GetTitleDataResult>("/Admin/GetTitleInternalData", apiEndpoint, new GetTitleDataRequest(), resultCb, errorCb);
        }

        public static void SetTitleInternalData(Dictionary<string, string> keys, Action<SetTitleDataResult> resultCb, Action<EditorModels.PlayFabError> errorCb)
        {
            foreach(var pair in keys)
            {
                var req = new SetTitleDataRequest() { Key = pair.Key, Value = pair.Value };

                var titleId = PlayFabEditorDataService.envDetails.selectedTitleId;
                var apiEndpoint = String.Format("https://{0}{1}", titleId, PlayFabEditorHelper.TITLE_ENDPOINT);
                PlayFabEditorHttp.MakeApiCall<SetTitleDataRequest, SetTitleDataResult>("/Admin/SetTitleInternalData", apiEndpoint, req, resultCb, errorCb);
            }
        }

        public static void UpdateCloudScript(UpdateCloudScriptRequest request, Action<UpdateCloudScriptResult> resultCb, Action<EditorModels.PlayFabError> errorCb)
        {
            var titleId = PlayFabEditorDataService.envDetails.selectedTitleId;
            var apiEndpoint = String.Format("https://{0}{1}", titleId, PlayFabEditorHelper.TITLE_ENDPOINT);
            PlayFabEditorHttp.MakeApiCall<UpdateCloudScriptRequest, UpdateCloudScriptResult>("/Admin/UpdateCloudScript", apiEndpoint, request, resultCb, errorCb);
        }
#endregion
    }
}